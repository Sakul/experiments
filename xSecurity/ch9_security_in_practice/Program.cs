using System;
using System.Text;

namespace ch9_security_in_practice
{
    class Program
    {
        static void Main(string[] args)
        {
            var john = new Person("John");
            var jane = new Person("Jane");

            var message = "I love you";
            var secretPackage = john.Encrypt(message, jane.PublicKey);
            var decryptedMessage = jane.Decrypt(secretPackage, john.PublicKey);
            Console.WriteLine($"SecretMsg: {message}");
            Console.WriteLine($"Decrypted: {Encoding.Default.GetString(decryptedMessage)}");
        }
    }
}
