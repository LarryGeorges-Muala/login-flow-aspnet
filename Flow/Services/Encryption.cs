using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class AesProtector
{
    // Generate a secure 32-byte key for AES-256
    public static byte[] GenerateRandomKey() => RandomNumberGenerator.GetBytes(32);

    public static string Encrypt(string plainText, byte[] key)
    {
        using Aes aes = Aes.Create();
        aes.Key = key;
        // Generate a fresh IV for every unique encryption routine
        aes.GenerateIV(); 

        using MemoryStream ms = new MemoryStream();
        // Write the IV directly to the start of the stream
        ms.Write(aes.IV, 0, aes.IV.Length); 

        using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
        using (StreamWriter sw = new StreamWriter(cs, Encoding.UTF8))
        {
            sw.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    public static string Decrypt(string cipherText, byte[] key)
    {
        byte[] fullCipher = Convert.FromBase64String(cipherText);

        using Aes aes = Aes.Create();
        aes.Key = key;

        byte[] iv = new byte[aes.BlockSize / 8]; // 16 bytes for AES
        Array.Copy(fullCipher, 0, iv, 0, iv.Length);
        aes.IV = iv;

        using MemoryStream ms = new MemoryStream(fullCipher, iv.Length, fullCipher.Length - iv.Length);
        using CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
        using StreamReader sr = new StreamReader(cs, Encoding.UTF8);

        return sr.ReadToEnd();
    }
}
