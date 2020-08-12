using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Linq;

namespace ShadAhm.Katalalu.App.Services
{
    public static class StringCipher
    {
        private static PasswordDeriveBytes KeyFromMasterPasssword(string masterPassword, string salt)
        {
            byte[] saltArray = Encoding.ASCII.GetBytes(salt);
            return new PasswordDeriveBytes(masterPassword, saltArray);
        }

         public static string Encrypt(string masterPassword, string plainText, string salt)  
        {  
            byte[] array;
            string ivBase64 = string.Empty;
            PasswordDeriveBytes rfc2898DeriveBytes = KeyFromMasterPasssword(masterPassword, salt);
  
            using (Aes aes = Aes.Create())  
            {  
                aes.Key = rfc2898DeriveBytes.GetBytes(aes.KeySize / 8);

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);  
                ivBase64 = Convert.ToBase64String(aes.IV);
  
                using (MemoryStream memoryStream = new MemoryStream())  
                {  
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))  
                    {  
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))  
                        {  
                            streamWriter.Write(plainText);  
                        }  
  
                        array = memoryStream.ToArray();  
                    }  
                }  
            }  
  
            string cipherTextBase64 = Convert.ToBase64String(array);

            return $"{cipherTextBase64};{ivBase64}";  
        }  
  
        public static string Decrypt(string masterPassword, string cipherText, string salt)  
        {  
            string textBase64 = cipherText.Split(';')[0];
            string ivBase64 = cipherText.Split(';')[1]; 
            byte[] buffer = Convert.FromBase64String(textBase64);  
            PasswordDeriveBytes rfc2898DeriveBytes = KeyFromMasterPasssword(masterPassword, salt);
  
            using (Aes aes = Aes.Create())  
            {  
                aes.Key = rfc2898DeriveBytes.GetBytes(aes.KeySize / 8);

                byte[] iv = Convert.FromBase64String(ivBase64);  
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, iv);  
  
                using (MemoryStream memoryStream = new MemoryStream(buffer))  
                {  
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))  
                    {  
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))  
                        {  
                            return streamReader.ReadToEnd();  
                        }  
                    }  
                }  
            }  
        }  
    }
}