![](https://github.com/DevElkami/PasswordVault/actions/workflows/workflow.yml/badge.svg?branch=master)

![CodeQL](https://github.com/DevElkami/MesConges/actions/workflows/codeql.yml/badge.svg)

# PasswordVault
Personal password vault with firefox / thunderbird importer. This vault use triple DES encryption.

![](https://github.com/DevElkami/PasswordVault/blob/master/vault.png)

Step for configure:
1. In "Security.cs" @ Instance() function: change the MD5 hash by your favorite password MD5 hash.
2. Rebuild program
3. Start it
4. In firefox tab, click on "Import auto"
5. Click on save: file SimpleDb.sqlite is generated.

Note: MD5 hash is also used by triple DES encryption as a hash key.
