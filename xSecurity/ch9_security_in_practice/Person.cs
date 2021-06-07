using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ch9_security_in_practice
{
    public class Person
    {
        private readonly RSACryptoServiceProvider rsa;

        public string Name { get; }
        public RSAParameters PublicKey => rsa.ExportParameters(false);

        public Person(string name)
        {
            Name = name;
            rsa = new RSACryptoServiceProvider(2048);
        }

        public SecretPackage Encrypt(string message, RSAParameters recipientPublicKey)
        {
            var sessionKey = new AesCryptoServiceProvider();
            var encryptedMessage = encryptMessage();
            var encryptedSessionKey = encryptSessionKey();
            var hashedMessage = hash(encryptedMessage, sessionKey.Key);
            var signature = signHashedMessage();
            return new SecretPackage
            {
                IV = sessionKey.IV,
                SessionKey = encryptedSessionKey,
                Message = encryptedMessage,
                Hash = hashedMessage,
                Signature = signature,
            };

            byte[] encryptMessage()
            {
                var msg = Encoding.Default.GetBytes(message);
                using var memory = new MemoryStream();
                var stream = new CryptoStream(memory, sessionKey.CreateEncryptor(), CryptoStreamMode.Write);
                stream.Write(msg, 0, msg.Length);
                stream.FlushFinalBlock();
                return memory.ToArray();
            }
            byte[] encryptSessionKey()
            {
                using var rsa = new RSACryptoServiceProvider();
                rsa.ImportParameters(recipientPublicKey);
                return rsa.Encrypt(sessionKey.Key, true);
            }
            byte[] signHashedMessage()
            {
                return rsa.SignHash(hashedMessage, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }

        public byte[] Decrypt(SecretPackage package, RSAParameters senderPublicKey)
        {
            var decryptedSessionKey = decryptPackageSessionKey();
            if (!verifyPackageHash())
            {
                throw new CryptographicException("Invalid HASH");
            }
            if (!verifyPackageSignature())
            {
                throw new CryptographicException("Invalid Signature");
            }
            var decryptedMessage = decryptMessage();
            return decryptedMessage;

            byte[] decryptPackageSessionKey()
            {
                return rsa.Decrypt(package.SessionKey, true);
            }
            bool verifyPackageHash()
            {
                var rehashedData = hash(package.Message, decryptedSessionKey);
                var result = package.Hash.Length == rehashedData.Length;
                for (var i = 0; i < package.Hash.Length && i < rehashedData.Length; ++i)
                {
                    result &= package.Hash[i] == rehashedData[i];
                }
                return result;
            }
            bool verifyPackageSignature()
            {
                using var rsa = new RSACryptoServiceProvider();
                rsa.ImportParameters(senderPublicKey);
                return rsa.VerifyHash(package.Hash, package.Signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
            byte[] decryptMessage()
            {
                using var aes = new AesCryptoServiceProvider { Key = decryptedSessionKey, IV = package.IV };
                using var memory = new MemoryStream();
                var stream = new CryptoStream(memory, aes.CreateDecryptor(), CryptoStreamMode.Write);
                stream.Write(package.Message, 0, package.Message.Length);
                stream.FlushFinalBlock();
                return memory.ToArray();
            }
        }

        private byte[] hash(byte[] encryptedData, byte[] key)
        {
            return new HMACSHA256(key).ComputeHash(encryptedData);
        }
    }
}
