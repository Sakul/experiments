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

# Symmetric Encryption (AES)
# Asymmetric Encryption (RSA)
# Digital Signatures