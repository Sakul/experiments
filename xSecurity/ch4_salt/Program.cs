using System;
using System.Text;
using System.Security.Cryptography;

namespace ch4_salt
{
	class Program
	{
		static void Main(string[] args)
		{
			var plainText = "Hello world";
			var byteText = Encoding.ASCII.GetBytes(plainText);

			var plainSalt = Guid.NewGuid().ToString();
			var byteSalt = Encoding.ASCII.GetBytes(plainSalt);

			var textWithSalt = new byte[byteText.Length + byteSalt.Length];
			Buffer.BlockCopy(byteText, 0, textWithSalt, 0, byteText.Length);
			Buffer.BlockCopy(byteSalt, 0, textWithSalt, 0, byteSalt.Length);
			
			var byteResult = SHA256.Create().ComputeHash(textWithSalt);
			var textResult = Convert.ToBase64String(byteResult);
			Console.WriteLine(textResult);
		}
	}
}
