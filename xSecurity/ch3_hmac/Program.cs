using System;
using System.Text;
using System.Security.Cryptography;

namespace ch3_hmac
{
	class Program
	{
		static void Main(string[] args)
		{
			var key = new byte[32]; // Key size (32 byte)
			using var rng = new RNGCryptoServiceProvider();
			rng.GetBytes(key);

			var plainText1 = "Hello world";
			var plainText2 = "hello world";
			var longestText = "This is the longest text ever";

			Console.WriteLine("HMACMD5");
			using var hmacMD5 = new HMACMD5(key);
			Console.WriteLine(GetHMACText(plainText1, hmacMD5));
			Console.WriteLine(GetHMACText(plainText2, hmacMD5));
			Console.WriteLine(GetHMACText(longestText, hmacMD5));

			Console.WriteLine("\nHMACSHA-1");
			using var hmacSHA1 = new HMACSHA1(key);
			Console.WriteLine(GetHMACText(plainText1, hmacSHA1));
			Console.WriteLine(GetHMACText(plainText2, hmacSHA1));
			Console.WriteLine(GetHMACText(longestText, hmacSHA1));

			Console.WriteLine("\nHMACSHA-256");
			using var hmacSHA256 = new HMACSHA256(key);
			Console.WriteLine(GetHMACText(plainText1, hmacSHA256));
			Console.WriteLine(GetHMACText(plainText2, hmacSHA256));
			Console.WriteLine(GetHMACText(longestText, hmacSHA256));

			Console.WriteLine("\nHMACSHA-512");
			using var hmacSHA512 = new HMACSHA512(key);
			Console.WriteLine(GetHMACText(plainText1, hmacSHA512));
			Console.WriteLine(GetHMACText(plainText2, hmacSHA512));
			Console.WriteLine(GetHMACText(longestText, hmacSHA512));
		}

		static string GetHMACText(string text, HMAC algorithm)
		{
			var byteText = Encoding.ASCII.GetBytes(text);
			var byteResult = algorithm.ComputeHash(byteText);
			return Convert.ToBase64String(byteResult);
		}
	}
}
