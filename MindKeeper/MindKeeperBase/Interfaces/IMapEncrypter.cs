using System.Security.Cryptography.X509Certificates;
using MindKeeperBase.Model;

namespace MindKeeperBase.Interfaces
{
    public interface IMapEncrypter
    {
        void EncryptAndSerialize(Map map, string pathToFile, string key);
        Map DecryptAndDeserialize(string pathToFile, string key);
    }
}
