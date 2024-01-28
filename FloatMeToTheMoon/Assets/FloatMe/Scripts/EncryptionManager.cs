using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace FloatMeToTheMoon
{
    public class EncryptionManager : MonoBehaviour
    {
        private static string encryptionKey = "YourEncryptionKey123"; // Cambia esto a una clave segura y secreta

        public static string Encrypt(string plainText)
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = System.Text.Encoding.UTF8.GetBytes(encryptionKey);
                aesAlg.IV = aesAlg.Key;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = System.Text.Encoding.UTF8.GetBytes(encryptionKey);
                aesAlg.IV = aesAlg.Key;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        private void Start()
        {
            // Ejemplo de uso:
            string originalText = "Hola, mundo!";
            string encryptedText = Encrypt(originalText);
            string decryptedText = Decrypt(encryptedText);

            Debug.Log("Original: " + originalText);
            Debug.Log("Encrypted: " + encryptedText);
            Debug.Log("Decrypted: " + decryptedText);
        }
    }
}
