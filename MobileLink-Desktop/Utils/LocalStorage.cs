using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MobileLink_Desktop.Classes;

namespace MobileLink_Desktop.Utils;

public class LocalStorage()
{
    private const string LocalStorageFile = "LocalStorage.txt";
    private const string StorageFileSecret = "Su10z+yRVQ4$k!qyQ5}e`3M0yO[)f9qQ4}U_,/0S!jA&m;3";
    private const int KeySize = 256;
    private const int DerivationIterations = 1000;

    public LocalStorageContent? GetStorage()
    {
        try
        {
            using (FileStream fs = File.OpenRead(LocalStorageFile))
            {
                byte[] result = new byte[fs.Length];
                fs.Read(result, 0, (int)fs.Length);
                // var content = Decrypt(Encoding.ASCII.GetString(result), StorageFileSecret);
                return JsonSerializer.Deserialize<LocalStorageContent>(result);
            }
            
        }
        catch (Exception ex)
        {
            //TODO differentiate errors
            var emptyStorage = new LocalStorageContent();
            using (StreamWriter sw = File.CreateText(LocalStorageFile))
            {
                sw.Write(JsonSerializer.Serialize(emptyStorage));
            }
            return emptyStorage;
        }
    }

    public void SetStorage(LocalStorageContent content)
    {
        var serializedContent = JsonSerializer.Serialize(content);
        // var encryptedContent = Encrypt(serializedContent, StorageFileSecret);
        File.WriteAllText(LocalStorageFile, serializedContent);
    }
    private static string Encrypt(string plainText, string passPhrase)
    {
        var saltStringBytes = Generate256BitsOfRandomEntropy();
        var ivStringBytes = Generate256BitsOfRandomEntropy();
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
        {
            var keyBytes = password.GetBytes(KeySize / 8);
            using (var symmetricKey = new RijndaelManaged())
            {
                symmetricKey.BlockSize = 256;
                symmetricKey.Mode = CipherMode.CBC;
                symmetricKey.Padding = PaddingMode.PKCS7;
                using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                            cryptoStream.FlushFinalBlock();

                            var cipherTextBytes = saltStringBytes;
                            cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                            cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                            memoryStream.Close();
                            cryptoStream.Close();
                            return Convert.ToBase64String(cipherTextBytes);
                        }
                    }
                }
            }
        }
    }

    private static string Decrypt(string cipherText, string passPhrase)
    {
        var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
        var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(KeySize / 8).ToArray();
        var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(KeySize / 8).Take(KeySize / 8).ToArray();
        var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((KeySize / 8) * 2)
            .Take(cipherTextBytesWithSaltAndIv.Length - ((KeySize / 8) * 2)).ToArray();

        using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
        {
            var keyBytes = password.GetBytes(KeySize / 8);
            using (var symmetricKey = new RijndaelManaged())
            {
                symmetricKey.BlockSize = 256;
                symmetricKey.Mode = CipherMode.CBC;
                symmetricKey.Padding = PaddingMode.PKCS7;
                using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                {
                    using (var memoryStream = new MemoryStream(cipherTextBytes))
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        using (var streamReader = new StreamReader(cryptoStream, Encoding.UTF8))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }

    private static byte[] Generate256BitsOfRandomEntropy()
    {
        var randomBytes = new byte[32];
        using (var rngCsp = new RNGCryptoServiceProvider())
        {
            rngCsp.GetBytes(randomBytes);
        }

        return randomBytes;
    }
}