# PasswordVault
Personal password vault with firefox connector. This vault use triple DES encryption.

![](https://github.com/DevElkami/PasswordVault/blob/master/vault.png)

Step for configure:
1. In "Security.cs" @ Instance() function: change the MD5 hash by your favorite password MD5 hash.
2. Rebuild program
3. Start it
4. In firefox tab, click on "Import from firefox"
5. Click on save: file SimpleDb.sqlite is generated.

Note: MD5 hash is also used by triple DES encryption as a hash key.
If you forgot your password, good luck to retrieve it ;-)
