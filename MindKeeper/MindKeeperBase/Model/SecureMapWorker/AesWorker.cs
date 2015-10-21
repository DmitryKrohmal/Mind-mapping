using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using MindKeeperBase.Interfaces;

namespace MindKeeperBase.Model.SecureMapWorker
{
    public class AesWorker : IMapEncrypter
    {
        public void EncryptAndSerialize(Map map, string pathToFile, string key)
        {
            AesManaged aes = new AesManaged();
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;

            Rfc2898DeriveBytes Key = new Rfc2898DeriveBytes(key, new byte[] { 1, 22, 33, 11, 0, 7, 4, 2 });

            aes.Key = Key.GetBytes(aes.KeySize / 8);
            aes.IV = Key.GetBytes(aes.BlockSize / 8);

            aes.Mode = CipherMode.CBC;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var fs = new FileStream(pathToFile, FileMode.Create, FileAccess.Write))
                using (var cryptoStream = new CryptoStream(fs, encryptor, CryptoStreamMode.Write))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(cryptoStream, map);
                    //cryptoStream.FlushFinalBlock();
                }
        }

        public Map DecryptAndDeserialize(string pathToFile, string key)
        {
            AesManaged aes = new AesManaged();
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;

            Rfc2898DeriveBytes Key = new Rfc2898DeriveBytes(key, new byte[] { 1, 22, 33, 11, 0, 7, 4, 2 });

            aes.Key = Key.GetBytes(aes.KeySize / 8);
            aes.IV = Key.GetBytes(aes.BlockSize / 8);

            aes.Mode = CipherMode.CBC;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var fs = new FileStream(pathToFile, FileMode.Open, FileAccess.Read))
                using (var cryptoStream = new CryptoStream(fs, encryptor, CryptoStreamMode.Read))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return (Map)formatter.Deserialize(cryptoStream);
                }
        }

        public override string ToString()
        {
            return "AES algorythm";
        }
    }
}
