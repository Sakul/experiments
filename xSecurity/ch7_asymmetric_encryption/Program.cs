using System;
using System.Text;
using System.Security.Cryptography;

namespace ch7_asymmetric_encryption
{
	class Program
	{
		static void Main(string[] args)
		{
			const int KeySize = 2048;
			var plainText = "Hello world";
			Console.WriteLine($"Plain text: {plainText}");

			Console.WriteLine("\nRSA (In-memory)");
			var inmemRSA = CreateInMemoryRSA(KeySize);
			var inmemPublicKey = inmemRSA.ExportParameters(false);
			var inmemPrivateKey = inmemRSA.ExportParameters(true);
			var encryptedInmemRSA = GetEncryptedText(plainText, inmemPublicKey);
			var decryptedInmemRSA = GetDecryptexText(encryptedInmemRSA, inmemPrivateKey);
			Console.WriteLine($"Encrypted: {Convert.ToBase64String(encryptedInmemRSA)}");
			Console.WriteLine($"Decrypted: {Encoding.ASCII.GetString(decryptedInmemRSA)}");
			// Console.WriteLine(rsa.ToXmlString(true));

			Console.WriteLine("\nRSA (CSP)");
			const string ContainerName = "Saladpuk";
			var cspRSA = CreateRSA_WithCSP(ContainerName, "Microsoft Strong Cryptographic Provider");
			var cspParam = new CspParameters { KeyContainerName = ContainerName };
			var encryptedCspRSA = GetEncryptedText(plainText, KeySize, cspParam);
			var decryptedCspRSA = GetDecryptexText(encryptedCspRSA, KeySize, cspParam);
			Console.WriteLine($"Encrypted: {Convert.ToBase64String(encryptedCspRSA)}");
			Console.WriteLine($"Decrypted: {Encoding.ASCII.GetString(decryptedCspRSA)}");
			// Console.WriteLine(cspRSA.ToXmlString(true));
			// DeleteRSA(ContainerName);

			Console.WriteLine("\nRSA (Base class)");
			var rsa1 = RSA.Create(KeySize);
			var encrypted = Encrypt(plainText, rsa1);
			var decrypted = Decrypt(encrypted, rsa1);
			Console.WriteLine($"Encrypted: {Convert.ToBase64String(encrypted)}");
			Console.WriteLine($"1st Decrypted: {Encoding.ASCII.GetString(decrypted)}");

			// Export
			const string Password = "P@$$w0rd";
			var encryptedPrivateKey = GetPrivateKeyWithPassword(rsa1, Password);
			var publicKey = GetPublicKey(rsa1);

			// Import
			var rsa2 = RSA.Create(KeySize);
			ImportPublicKey(rsa2, publicKey);
			ImportEncryptedPrivateKeyWithPassword(rsa2, Password, encryptedPrivateKey);

			// Test decryption
			var decryptedRound2 = Decrypt(encrypted, rsa2);
			Console.WriteLine($"2nd Decrypted: {Encoding.ASCII.GetString(decryptedRound2)}");
			// Console.WriteLine(rsa1.ToXmlString(true));
			// Console.WriteLine(rsa2.ToXmlString(true));
		}

		#region RSA (In-memory)
		static RSA CreateInMemoryRSA(int keySize)
			=> new RSACryptoServiceProvider(keySize);
		static byte[] GetEncryptedText(string text, RSAParameters param)
		{
			using var rsa = new RSACryptoServiceProvider();
			rsa.ImportParameters(param);
			var byteText = Encoding.ASCII.GetBytes(text);
			return rsa.Encrypt(byteText, false);
		}
		static byte[] GetDecryptexText(byte[] cipherText, RSAParameters param)
		{
			using var rsa = new RSACryptoServiceProvider();
			rsa.ImportParameters(param);
			return rsa.Decrypt(cipherText, false);
		}
		#endregion RSA (In-memory)

		#region RSA (CSP)
		static RSA CreateRSA_WithCSP(string keyContainerName, string providerName)
		{
			var csp = new CspParameters(1)
			{
				KeyContainerName = keyContainerName,
				Flags = CspProviderFlags.UseDefaultKeyContainer,
				ProviderName = providerName,
			};
			return new RSACryptoServiceProvider(csp) { PersistKeyInCsp = true };
		}
		static void DeleteRSA(string keyContainerName)
		{
			var csp = new CspParameters { KeyContainerName = keyContainerName };
			var rsa = new RSACryptoServiceProvider(csp) { PersistKeyInCsp = false };
			rsa.Clear();
		}
		static byte[] GetEncryptedText(string text, int keySize, CspParameters csp)
		{
			using var rsa = new RSACryptoServiceProvider(keySize, csp);
			var byteText = Encoding.ASCII.GetBytes(text);
			return rsa.Encrypt(byteText, false);
		}
		static byte[] GetDecryptexText(byte[] cipherText, int keySize, CspParameters csp)
		{
			using var rsa = new RSACryptoServiceProvider(keySize, csp);
			return rsa.Decrypt(cipherText, false);
		}
		#endregion

		#region RSA (base class)
		static byte[] Encrypt(string text, RSA rsa)
			=> rsa.Encrypt(Encoding.ASCII.GetBytes(text), RSAEncryptionPadding.OaepSHA256);
		static byte[] Decrypt(byte[] cipher, RSA rsa)
			=> rsa.Decrypt(cipher, RSAEncryptionPadding.OaepSHA256);

		static byte[] GetPrivateKeyWithPassword(RSA rsa, string password, int iterations = 100000)
		{
			var param = new PbeParameters(PbeEncryptionAlgorithm.Aes256Cbc, HashAlgorithmName.SHA256, iterations);
			var bytePassword = Encoding.ASCII.GetBytes(password);
			return rsa.ExportEncryptedPkcs8PrivateKey(bytePassword, param);
		}
		static byte[] GetPublicKey(RSA rsa)
			=> rsa.ExportRSAPublicKey();

		static void ImportEncryptedPrivateKeyWithPassword(RSA rsa, string password, byte[] encryptedPrivateKey)
			=> rsa.ImportEncryptedPkcs8PrivateKey(Encoding.ASCII.GetBytes(password), encryptedPrivateKey, out int bytesRead);
		static void ImportPublicKey(RSA rsa, byte[] publicKey)
			=> rsa.ImportRSAPublicKey(publicKey, out int bytesRead);
		#endregion
	}
}
