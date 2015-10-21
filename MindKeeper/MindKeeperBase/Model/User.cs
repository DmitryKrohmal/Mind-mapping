using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MindKeeperBase.Model
{
    [Serializable]
    public class User
    {
        public User()
        {
            UserId = Guid.NewGuid();
            Maps = new List<Map>();
            //ActiveMaps = new List<Map>();
        }
        public Guid UserId { get; set; }
        public string Login { get; set; }
        public byte[] Password { get; set; }
        public string HomeDirectoryPath { get; set; }

        public virtual ICollection<Map> Maps { get; set; } 

        //[XmlIgnore]
        //public List<Map> ActiveMaps { get; set; }

        public void Serialize()
        {
            string path = @".\Users\" + Login + @"\" + Login + @".usr";
            XmlSerializer formatter = new XmlSerializer(typeof(User));
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, this);
                MessageBox.Show("DONE! UserInfo: " + Login + " is serialized to file: " + path);
            }
        }

        #region GetHashCode, Equals, ToString override
        public override int GetHashCode()
        {
            return UserId.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (!(obj is User))
                return false;

            return Equals((User)obj);
        }
        public bool Equals(User other)
        {
            return UserId == other.UserId;
        }
        public override string ToString()
        {
            return Login;
        }
        #endregion
    }
}
