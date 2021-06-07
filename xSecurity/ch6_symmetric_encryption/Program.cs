using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace ch6_symmetric_encryption
{
	class Program
	{
		static void Main(string[] args)
		{
			var plainText = "Hello world";
			Console.WriteLine($"Plain text: {plainText}");

			Console.WriteLine("\nDES");
			EncryptAndDecrypt(plainText, new DESCryptoServiceProvider { Key = GetRandomData(8), IV = GetRandomData(8) });

			Console.WriteLine("\nTriple DES (3 keys)");
			EncryptAndDecrypt(plainText, new TripleDESCryptoServiceProvider { Key = GetRandomData(24), IV = GetRandomData(8) });

			Console.WriteLine("\nTriple DES (2 keys)");
			EncryptAndDecrypt(plainText, new TripleDESCryptoServiceProvider { Key = GetRandomData(16), IV = GetRandomData(8) });

			Console.WriteLine("\nAES");
			EncryptAndDecrypt(plainText, new AesCryptoServiceProvider { Key = GetRandomData(32), IV = GetRandomData(16) });

			var key = GetRandomData(32); // Key size (32 byte)
			var nonce = GetRandomData(12); // Nonce size (12 byte)
			var associatedData = GetRandomData(4); // Like PID, ReferenceId (it can be anything)

			Console.WriteLine("\nAES (GCM)");
			var encryptedGcm = EncryptGCM(plainText, key, nonce, associatedData);
			var decryptedGcm = DecryptGCM(encryptedGcm.CipherText, key, nonce, encryptedGcm.Tag, associatedData);
			Console.WriteLine($"Encrypted: {Convert.ToBase64String(encryptedGcm.CipherText)}");
			Console.WriteLine($"Decrypted: {Encoding.ASCII.GetString(decryptedGcm)}");

			Console.WriteLine("\nAES (CCM)");
			var encryptedCcm = EncryptCCM(plainText, key, nonce, associatedData);
			var decryptedCcm = DecryptCCM(encryptedCcm.CipherText, key, nonce, encryptedCcm.Tag, associatedData);
			Console.WriteLine($"Encrypted: {Convert.ToBase64String(encryptedCcm.CipherText)}");
			Console.WriteLine($"Decrypted: {Encoding.ASCII.GetString(decryptedCcm)}");
		}

		static byte[] GetRandomData(int size)
		{
			var byteResult = new byte[size];
			using var rng = new RNGCryptoServiceProvider();
			rng.GetBytes(byteResult);
			return byteResult;
		}

		static void EncryptAndDecrypt(string text, SymmetricAlgorithm algorithm)
		{
			var byteText = Encoding.ASCII.GetBytes(text);
			var encryptedByte = TransformData(byteText, algorithm.CreateEncryptor());
			var decryptedByte = TransformData(encryptedByte, algorithm.CreateDecryptor());
			Console.WriteLine($"Encrypted: {Convert.ToBase64String(encryptedByte)}");
			Console.WriteLine($"Decrypted: {Encoding.ASCII.GetString(decryptedByte)}");
		}

		static byte[] TransformData(byte[] data, ICryptoTransform transform)
		{
			using var memory = new MemoryStream();
			var stream = new CryptoStream(memory, transform, CryptoStreamMode.Write);
			stream.Write(data, 0, data.Length);
			stream.FlushFinalBlock();
			return memory.ToArray();
		}

		#region CCM
		static (byte[] CipherText, byte[] Tag) EncryptCCM(string text, byte[] key, byte[] nonce, byte[] associatedData = null)
		{
			var byteText = Encoding.ASCII.GetBytes(text);
			var tag = new byte[16];
			var cipherText = new byte[byteText.Length];
			using var gcm = new AesCcm(key);
			gcm.Encrypt(nonce, byteText, cipherText, tag, associatedData);
			return (cipherText, tag);
		}
		static byte[] DecryptCCM(byte[] cipherText, byte[] key, byte[] nonce, byte[] tag, byte[] associatedData = null)
		{
			var decryptedData = new byte[cipherText.Length];
			using var gcm = new AesCcm(key);
			gcm.Decrypt(nonce, cipherText, tag, decryptedData, associatedData);
			return decryptedData;
		}
		#endregion CCM

		#region GCM
		static (byte[] CipherText, byte[] Tag) EncryptGCM(string text, byte[] key, byte[] nonce, byte[] associatedData = null)
		{
			var byteText = Encoding.ASCII.GetBytes(text);
			var tag = new byte[16];
			var cipherText = new byte[byteText.Length];
			using var gcm = new AesGcm(key);
			gcm.Encrypt(nonce, byteText, cipherText, tag, associatedData);
			return (cipherText, tag);
		}
		static byte[] DecryptGCM(byte[] cipherText, byte[] key, byte[] nonce, byte[] tag, byte[] associatedData = null)
		{
			var decryptedData = new byte[cipherText.Length];
			using var gcm = new AesGcm(key);
			gcm.Decrypt(nonce, cipherText, tag, decryptedData, associatedData);
			return decryptedData;
		}
		#endregion GCM
	}
}
