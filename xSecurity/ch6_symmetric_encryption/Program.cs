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
	}
}
