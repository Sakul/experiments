using System;
using System.Security.Cryptography;

namespace ch1_random
{
	class Program
	{
		static void Main(string[] args)
		{
			using var rng = new RNGCryptoServiceProvider();
			var byteResult = new byte[32];
			rng.GetBytes(byteResult);
			var resultText = Convert.ToBase64String(byteResult);
			System.Console.WriteLine($"RNG: {resultText}");
		}
	}
}
