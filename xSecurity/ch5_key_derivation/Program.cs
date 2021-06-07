using System;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;

namespace ch5_key_derivation
{
	class Program
	{
		static void Main(string[] args)
		{
			var plainSecret = "Hello world";
			var byteSecret = Encoding.ASCII.GetBytes(plainSecret);

			var plainSalt = Guid.NewGuid().ToString();
			var byteSalt = Encoding.ASCII.GetBytes(plainSalt);

			var iterations = 100000;
			using var rfc2898 = new Rfc2898DeriveBytes(byteSecret, byteSalt, iterations);
			var byteResult = rfc2898.GetBytes(20);
			var textResult = Convert.ToBase64String(byteResult);
			Console.WriteLine(textResult);
		}
	}
}
