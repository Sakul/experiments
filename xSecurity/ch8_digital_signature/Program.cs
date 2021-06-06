using System;
using System.Text;
using System.Security.Cryptography;

namespace ch8_digital_signature
{
	class Program
	{
		static void Main(string[] args)
		{
			var plainText = "Hello world";
			var HashAlgorithm = HashAlgorithmName.SHA256.Name;
			var hashedData = HashData(plainText, HashAlgorithm);
			Console.WriteLine($"Plain text: {plainText}");
			Console.WriteLine($"Hashed text: {Convert.ToBase64String(hashedData)}");

			const int KeySize = 2048;
			using var rsa = RSACryptoServiceProvider.Create(KeySize);

			Console.WriteLine("\nUsing Signature Formatters");
			var signature1 = SignByFormatter(rsa, hashedData, HashAlgorithm);
			var verificationResult1 = VerifySignatureByFormatter(rsa, hashedData, signature1, HashAlgorithm);
			Console.WriteLine($"1st Signature: {Convert.ToBase64String(signature1)}");
			Console.WriteLine($"1st Verification result: {verificationResult1}");
			// Someone malicious edits your data
			var hashedData2 = (byte[])hashedData.Clone();
			hashedData2[0] = (byte)new Random().Next();
			var verificationResult2 = VerifySignatureByFormatter(rsa, hashedData2, signature1, HashAlgorithm);
			Console.WriteLine($"2nd Verification result: {verificationResult2}");

			Console.WriteLine("\nUsing RSA only");
			var signature2 = rsa.SignHash(hashedData, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
			var verificationResult3 = rsa.VerifyHash(hashedData, signature2, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
			Console.WriteLine($"2nd Signature: {Convert.ToBase64String(signature2)}");
			Console.WriteLine($"3rd Verification result: {verificationResult1}");
		}

		static byte[] HashData(string text, string algorithmName)
		{
			var byteText = Encoding.ASCII.GetBytes(text);
			var hash = HashAlgorithm.Create(algorithmName);
			return hash.ComputeHash(byteText);
		}

		static byte[] SignByFormatter(RSA rsa, byte[] hashedData, string algorithmName)
		{
			var formatter = new RSAPKCS1SignatureFormatter(rsa);
			formatter.SetHashAlgorithm(algorithmName);
			return formatter.CreateSignature(hashedData);
		}
		static string VerifySignatureByFormatter(RSA rsa, byte[] hashedData, byte[] signature, string algorithmName)
		{
			var formatter = new RSAPKCS1SignatureDeformatter(rsa);
			formatter.SetHashAlgorithm(algorithmName);
			return formatter.VerifySignature(hashedData, signature) ? "Valid" : "Invalid";
		}
	}
}
