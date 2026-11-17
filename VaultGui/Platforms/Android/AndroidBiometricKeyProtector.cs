using Android.Content;
using Android.Hardware.Biometrics;
using Android.OS;
using Android.Runtime;
using Android.Security.Keystore;
using Java.Lang;
using Java.Security;
using Javax.Crypto;
using Javax.Crypto.Spec;
using VaultGui.Resources.Strings;
using VaultGui.Services;
using Exception = System.Exception;

namespace VaultGui.Platforms.Android;

/// <summary>
/// Quick unlock with strong biometrics (Android 10+).
/// <para>
/// An AES-256-GCM key is generated in the Android Keystore (hardware-backed when available) with
/// "user authentication required": it can only encrypt or decrypt inside a successful BiometricPrompt,
/// and Android destroys it when a new fingerprint or face is enrolled.
/// </para>
/// </summary>
public sealed class AndroidBiometricKeyProtector : IBiometricKeyProtector
{
    private const string KeyStoreName = "AndroidKeyStore";
    private const string KeyAlias = "PasswordVault.QuickUnlock";
    private const string Transformation = "AES/GCM/NoPadding";
    private const int TagBits = 128;

    public string Name => Loc.Get(Loc.Biometrics);

    public Task<bool> IsAvailableAsync()
    {
        if (!OperatingSystem.IsAndroidVersionAtLeast(29))
            return Task.FromResult(false);

        BiometricManager? manager = (BiometricManager?)Platform.AppContext.GetSystemService(Context.BiometricService);
        if (manager is null)
            return Task.FromResult(false);

        BiometricCode code = OperatingSystem.IsAndroidVersionAtLeast(30)
            ? manager.CanAuthenticate((int)BiometricManagerAuthenticators.BiometricStrong)
            : manager.CanAuthenticate();
        return Task.FromResult(code == BiometricCode.Success);
    }

    public async Task<string?> ProtectAsync(byte[] key)
    {
        Forget();
        ISecretKey secretKey = CreateKey();

        Cipher cipher = Cipher.GetInstance(Transformation)!;
        cipher.Init(CipherMode.EncryptMode, secretKey);

        Cipher? authenticated = await AuthenticateAsync(cipher);
        if (authenticated is null)
            return null;

        byte[] ciphertext = authenticated.DoFinal(key)!;
        byte[] iv = authenticated.GetIV()!;
        return Convert.ToBase64String(iv) + "." + Convert.ToBase64String(ciphertext);
    }

    public async Task<byte[]?> UnprotectAsync(string protectedKey)
    {
        string[] parts = protectedKey.Split('.');
        if (parts.Length != 2)
            throw new BiometricInvalidatedException("Unknown quick unlock data.");
        byte[] iv = Convert.FromBase64String(parts[0]);
        byte[] ciphertext = Convert.FromBase64String(parts[1]);

        Cipher cipher = Cipher.GetInstance(Transformation)!;
        try
        {
            ISecretKey secretKey = LoadKey() ?? throw new BiometricInvalidatedException("The quick unlock key no longer exists.");
            cipher.Init(CipherMode.DecryptMode, secretKey, new GCMParameterSpec(TagBits, iv));
        }
        catch (KeyPermanentlyInvalidatedException exception)
        {
            // Biometrics changed (or screen lock removed): Android destroyed the key on purpose.
            throw new BiometricInvalidatedException("Biometrics changed.", exception);
        }

        Cipher? authenticated = await AuthenticateAsync(cipher);
        if (authenticated is null)
            return null;

        try
        {
            return authenticated.DoFinal(ciphertext);
        }
        catch (AEADBadTagException exception)
        {
            throw new BiometricInvalidatedException("Quick unlock data does not match the key.", exception);
        }
    }

    public void Forget()
    {
        try
        {
            KeyStore store = KeyStore.GetInstance(KeyStoreName)!;
            store.Load(null);
            if (store.ContainsAlias(KeyAlias))
                store.DeleteEntry(KeyAlias);
        }
        catch (Exception)
        {
        }
    }

    private static ISecretKey? LoadKey()
    {
        KeyStore store = KeyStore.GetInstance(KeyStoreName)!;
        store.Load(null);
        // JavaCast, not "as": GetKey returns a wrapper typed IKey, which a C# cast to ISecretKey
        // always turns into null (the key then looked deleted, and quick unlock turned itself off).
        return store.GetKey(KeyAlias, null)?.JavaCast<ISecretKey>();
    }

    private static ISecretKey CreateKey()
    {
        KeyGenParameterSpec.Builder builder = new KeyGenParameterSpec.Builder(KeyAlias, KeyStorePurpose.Encrypt | KeyStorePurpose.Decrypt)
            .SetBlockModes(KeyProperties.BlockModeGcm)!
            .SetEncryptionPaddings(KeyProperties.EncryptionPaddingNone)!
            .SetKeySize(256)!
            .SetUserAuthenticationRequired(true)!
            .SetInvalidatedByBiometricEnrollment(true)!;
        if (OperatingSystem.IsAndroidVersionAtLeast(30))
            builder.SetUserAuthenticationParameters(0, (int)KeyPropertiesAuthType.BiometricStrong);

        KeyGenerator generator = KeyGenerator.GetInstance(KeyProperties.KeyAlgorithmAes, KeyStoreName)!;
        generator.Init(builder.Build());
        return generator.GenerateKey()!;
    }

    /// <summary>Show BiometricPrompt bound to <paramref name="cipher"/>. Null if cancelled.</summary>
    private static Task<Cipher?> AuthenticateAsync(Cipher cipher)
    {
        global::Android.App.Activity activity = Platform.CurrentActivity ?? throw new InvalidOperationException("No activity.");
        TaskCompletionSource<Cipher?> completion = new(TaskCreationOptions.RunContinuationsAsynchronously);
        CancellationSignal cancel = new();
        Java.Util.Concurrent.IExecutor executor = activity.MainExecutor!;

        BiometricPrompt.Builder builder = new BiometricPrompt.Builder(activity)
            .SetTitle(Loc.Get(Loc.AppTitle))!
            .SetSubtitle(Loc.Get(Loc.QuickUnlockPrompt))!
            .SetNegativeButton(Loc.Get(Loc.Cancel), executor, new NegativeButtonListener(completion))!;
        if (OperatingSystem.IsAndroidVersionAtLeast(30))
            builder.SetAllowedAuthenticators((int)BiometricManagerAuthenticators.BiometricStrong);

        MainThread.BeginInvokeOnMainThread(() =>
            builder.Build().Authenticate(new BiometricPrompt.CryptoObject(cipher), cancel, executor, new Callback(completion)));
        return completion.Task;
    }

    private sealed class Callback(TaskCompletionSource<Cipher?> completion) : BiometricPrompt.AuthenticationCallback
    {
        public override void OnAuthenticationSucceeded(BiometricPrompt.AuthenticationResult? result) =>
            completion.TrySetResult(result?.CryptoObject?.Cipher);

        public override void OnAuthenticationError([GeneratedEnum] BiometricErrorCode errorCode, ICharSequence? errString)
        {
            // Cancelled by the user or the system, too many attempts (lockout)...: fall back to the password.
            completion.TrySetResult(null);
        }
    }

    private sealed class NegativeButtonListener(TaskCompletionSource<Cipher?> completion) : Java.Lang.Object, IDialogInterfaceOnClickListener
    {
        public void OnClick(IDialogInterface? dialog, int which) => completion.TrySetResult(null);
    }
}
