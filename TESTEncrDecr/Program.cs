using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TESTEncrDecr
{
    class Program
    {
        public static byte[] GetKeyBytes(string login)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] loginBytes = Encoding.UTF8.GetBytes(login);
                byte[] hashedBytes = sha256.ComputeHash(loginBytes);

                // Усекаем хеш до 16 байт для AES-128, 24 байта для AES-192 или 32 байта для AES-256
                byte[] truncatedBytes = new byte[16];
                Buffer.BlockCopy(hashedBytes, 0, truncatedBytes, 0, truncatedBytes.Length);

                return truncatedBytes;
            }
        }
        public static string DecryptString(string cipherText, string key)
        {
            byte[] keyBytes = GetKeyBytes(key);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            using (Aes aesAlg = Aes.Create())
            {
                // Извлекаем IV из массива байт
                byte[] ivBytes = new byte[aesAlg.IV.Length];
                Buffer.BlockCopy(cipherTextBytes, 0, ivBytes, 0, aesAlg.IV.Length);

                aesAlg.Key = keyBytes;
                aesAlg.IV = ivBytes;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream())
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write))
                    {
                        // Зашифрованный текст начинается после IV
                        csDecrypt.Write(cipherTextBytes, aesAlg.IV.Length, cipherTextBytes.Length - aesAlg.IV.Length);
                        csDecrypt.FlushFinalBlock();

                        byte[] decryptedBytes = msDecrypt.ToArray();
                        return Encoding.UTF8.GetString(decryptedBytes);
                    }
                }
            }
            
        }
        static string EncryptString(string plainText, string key)
        {
            byte[] keyBytes = GetKeyBytes(key);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.GenerateIV();

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(plainTextBytes, 0, plainTextBytes.Length);
                        csEncrypt.FlushFinalBlock();

                        byte[] encryptedBytes = msEncrypt.ToArray();

                        // Объединяем IV и зашифрованный текст в один массив байт
                        byte[] resultBytes = new byte[aesAlg.IV.Length + encryptedBytes.Length];
                        Buffer.BlockCopy(aesAlg.IV, 0, resultBytes, 0, aesAlg.IV.Length);
                        Buffer.BlockCopy(encryptedBytes, 0, resultBytes, aesAlg.IV.Length, encryptedBytes.Length);

                        return Convert.ToBase64String(resultBytes);
                    }
                }
            }
        }
        static void Main(string[] args)
        {


            string plainText = Console.ReadLine();
            string key = Console.ReadLine();
            var cipherText = EncryptString(plainText, key);

            Console.WriteLine(cipherText);

            Console.WriteLine("=====");

            var decryptString = DecryptString(cipherText, key);

            Console.WriteLine(decryptString);



        }
    }
}
