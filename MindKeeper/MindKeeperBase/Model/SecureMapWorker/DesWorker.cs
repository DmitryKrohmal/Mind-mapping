using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using MindKeeperBase.Interfaces;

namespace MindKeeperBase.Model.SecureMapWorker
{
    public class DesWorker : IMapEncrypter
    {
        public void EncryptAndSerialize(Map map, string pathToFile, string key)
        {
            byte[] byteKey = Security.Security.Get8BytesByMD5Hash(key);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            using (var fs = new FileStream(pathToFile, FileMode.Create, FileAccess.Write))
            using (var cryptoStream = new CryptoStream(fs, des.CreateEncryptor(byteKey, byteKey), CryptoStreamMode.Write))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(cryptoStream, map);
            }
        }

        public Map DecryptAndDeserialize(string pathToFile, string key)
        {
            byte[] byteKey = Security.Security.Get8BytesByMD5Hash(key);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            using (var fs = new FileStream(pathToFile, FileMode.Open, FileAccess.Read))
            using (var cryptoStream = new CryptoStream(fs, des.CreateDecryptor(byteKey, byteKey), CryptoStreamMode.Read))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Map)formatter.Deserialize(cryptoStream);
            }
        }

        #region ToString override
        public override string ToString()
        {
            return "DES algorythm";
        }
        #endregion
    }
}
