using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Xml.Serialization;
using MindKeeperBase.Interfaces;

namespace MindKeeperBase.Model.SecureMapWorker
{
    public class SimpleWorker : IMapEncrypter
    {
        public void EncryptAndSerialize(Map map, string pathToFile, string key)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(pathToFile, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, map);
            }
        }

        public Map DecryptAndDeserialize(string pathToFile, string key)
        {
            Map res;
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(pathToFile, FileMode.OpenOrCreate))
            {
                res = (Map)formatter.Deserialize(fs);
            }
            return res;
        }

        #region ToString override
        public override string ToString()
        {
            return "Without encrypting";
        }
        #endregion
    }
}
