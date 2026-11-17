namespace VaultGui.Services;

/// <summary>
/// Platform part of quick unlock: protects the vault key with a hardware-backed key that can only be
/// used after the user passes a biometric check.
/// <list type="bullet">
/// <item>Android: AES key in the Android Keystore, usable only after BiometricPrompt (strong biometrics),
/// invalidated when a new fingerprint/face is enrolled.</item>
/// <item>Windows: AES key derived from a Windows Hello signature (fingerprint, face or Hello PIN).</item>
/// </list>
/// </summary>
public interface IBiometricKeyProtector
{
    /// <summary>Display name ("fingerprint", "Windows Hello").</summary>
    string Name { get; }

    Task<bool> IsAvailableAsync();

    /// <summary>Ask for the biometric check and return <paramref name="key"/> protected, or null if cancelled.</summary>
    Task<string?> ProtectAsync(byte[] key);

    /// <summary>Ask for the biometric check and return the key, or null if cancelled.</summary>
    /// <exception cref="BiometricInvalidatedException">The protecting key no longer exists (biometrics changed...).</exception>
    Task<byte[]?> UnprotectAsync(string protectedKey);

    /// <summary>Delete the platform key.</summary>
    void Forget();
}

/// <summary>The platform key protecting quick unlock is gone: the master password is needed.</summary>
public sealed class BiometricInvalidatedException(string message, Exception? inner = null) : Exception(message, inner);

/// <summary>Platforms without quick unlock.</summary>
public sealed class NoBiometricKeyProtector : IBiometricKeyProtector
{
    public string Name => "";
    public Task<bool> IsAvailableAsync() => Task.FromResult(false);
    public Task<string?> ProtectAsync(byte[] key) => Task.FromResult<string?>(null);
    public Task<byte[]?> UnprotectAsync(string protectedKey) => Task.FromResult<byte[]?>(null);
    public void Forget() { }
}
