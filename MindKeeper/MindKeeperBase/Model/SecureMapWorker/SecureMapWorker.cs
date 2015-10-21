using MindKeeperBase.Interfaces;

namespace MindKeeperBase.Model.SecureMapWorker
{
    public class SecureMapWorker
    {
        private IMapEncrypter _encrypter;
        private string _pathToFile;
        private string _key;
        private Map _activeMap;

        public SecureMapWorker(string pathToFile, string key, Map map, IMapEncrypter encrypter)
        {
            _encrypter = encrypter;
            _pathToFile = pathToFile;
            _key = key;
            _activeMap = map;
        }

        public void EncryptAndSerialize()
        {
            _encrypter.EncryptAndSerialize(_activeMap, _pathToFile, _key);
        }

        public Map DecryptAndDeserialize()
        {
            return _encrypter.DecryptAndDeserialize(_pathToFile, _key);
        }
    }
}
