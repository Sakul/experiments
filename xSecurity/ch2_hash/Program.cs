using System;
using System.Text;
using System.Security.Cryptography;

namespace ch2_hash
{
	class Program
	{
		static void Main(string[] args)
		{
			var plainText1 = "Hello world";
			var plainText2 = "hello world";
			var longestText = "This is the longest text ever";

			Console.WriteLine("MD5");
			using var md5 = MD5.Create();
			Console.WriteLine(GetHashedText(plainText1, md5));
			Console.WriteLine(GetHashedText(plainText2, md5));
			Console.WriteLine(GetHashedText(longestText, md5));

			Console.WriteLine("SHA-1");
			using var sha1 = SHA1.Create();
			Console.WriteLine(GetHashedText(plainText1, sha1));
			Console.WriteLine(GetHashedText(plainText2, sha1));
			Console.WriteLine(GetHashedText(longestText, sha1));

			Console.WriteLine("SHA-256");
			using var sha256 = SHA256.Create();
			Console.WriteLine(GetHashedText(plainText1, sha256));
			Console.WriteLine(GetHashedText(plainText2, sha256));
			Console.WriteLine(GetHashedText(longestText, sha256));

			Console.WriteLine("SHA-512");
			using var sha512 = SHA512.Create();
			Console.WriteLine(GetHashedText(plainText1, sha512));
			Console.WriteLine(GetHashedText(plainText2, sha512));
			Console.WriteLine(GetHashedText(longestText, sha512));
		}

		static string GetHashedText(string text, HashAlgorithm algorithm)
		{
			var byteText = Encoding.UTF8.GetBytes(text);
			var byteResult = algorithm.ComputeHash(byteText);
			return Convert.ToBase64String(byteResult);
		}
	}
}
