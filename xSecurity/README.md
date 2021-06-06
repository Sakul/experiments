# Security Concepts
* Confidentiality
* Integrity
* Non-Repudiation
* Authentication

# 1.Security Random Number Generation (RNG)
* Basic random isn't enough for encryption keys (Not always truly random)
* .NET `RNGCryptoServiceProvider` is designed for secure random number
* Slower than basic random

# 2.Hashing algorithms
* One way operation (can't reverse engineering)
* MD5, SHA-1, SHA-256, ...
* MD5: Collision resistance flaw found in 1996, 2004 (legacy system)

# 3.Hashed Message Authentication Code (HMAC)
* Hashing algorithms + Key = authentication
* HMACMD5, HMACSHA-1, HMACSHA-256, ...

# 4.Encryption with Salt
* Enemies: Brute Force & Rainbow Table
* Secret must be unpredictable by Encrypt( Secret + Salt )
	* Salt is just a random text

# 5.Key Derivation
* Prevent Moore's Law (double speed / 2 years)
* Secret = (Encryption + Salte) x iterations
* Password Based Key Derivation Function (PBKDF)
* Good at 100,000 ierations (balance it by acceptable performance)
* Iterations number is increases over time
* .NET `Rfc2898DeriveBytes`

# 6.Symmetric Encryption
* Single key for Encryption & Decryption
	* Plain text > Encrypt > CipherText > Decrypt > Plain text
* Fast but hard to share
* Longer keys are exponentially harder to crack

## Data Encryption Standard (DES)
* Legacy
* Using 8 bytes (56 bit for the key)
* .NET `DESCryptoServiceProvider`

## Triple DES
* Encrypt 3 times by difference 3 keys or 2 keys
	* Plain text > DES (Key1) > DES (Key2) > DES (Key3) > Ciphertext
	* Plain text > DES (Key1) > DES (Key2) > DES (Key1) > Ciphertext
* Decrypt by revert those steps
* .NET `TripleDESCryptoServiceProvider`

## Advanced Encryption Standard (AES)
* .NET `AESCryptoServiceProvider`
* 128, 192, 256 bit keys family, separate data to blocks and encrypt those blocks by difference keys

## Brute Force Attack
|Key size|Time to crack|
|56 bit (8 byte)|399 second|
|128 bit (16 byte)| 1.02 x 10^18 years| 1 BB years|
|192 bit (24 byte)| 1.87 x 10^37 years|
|256 bit (32 byte)| 3.31 x 10^56 years|

## Initialize Value
* It is just a random value like salt
* Required this value for Encryption & Description
* No secret data in it

## Symmetric Algorithm
* Cipher Modes
	* CBC - Cipher Block Chaining (Default)
	* CFB - Ciphertext feedback
	* CTS - Ciphertext Stealing
	* ECB - Electronic Codebook
	* OFB - Ouput Feedback
	* CCM - Counter witch CBC & MAC [.NETCore3]
	* GCM - Galois/Counter Mode (Great) [.NETCore3]
* Padding Modes
	* ANSI X923
	* ISO 10126
	* None
	* PKCS7 [Default]
	* Zeros

# 7.Asymmetric Encryption (RSA)
* Paired Key (Encrypt & Decrypt)
* Key size
	* 1024 bit (Weak)
	* 2048 bit
	* 4096 bit
* Slow but easy to share
	* Protect sensitive data like AES
* Verifiable
* .NET `RSACryptoServiceProvider`
	* In memory keys
	* Key Container
	* ~~XML~~ [Deprecated]

## Key Derivation
* Properties
	* Exponent
	* Modules
	* InverseQ (Private key only)
	* P prime (Private key only)
	* Q prime (Private key only)
	* D (Private key only)
	* DP (Private key only)
	* DQ (Private key only)

## Exporting RSA Keys
* .NET `ExportPKCS8PrivateKey`
* .NET `ExportEncryptedPKCS8PrivateKey`

# 8.Digital Signatures
* Claiming authenticity of the message
* Authentication & Non-Repudiation
* Signing alforithms required RSA
	* Private key for signing
	* Public key for verification
* Sign by `Private Key`, recipients can verify it by the `Public Key`
* Signature Formatters
	* .NET `RSAPKCS1SignatureFormatter`
	* .NET `RSAPKCS1SignatureDeFormatter`