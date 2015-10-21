using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace MindKeeperBase.Model
{
    [Serializable]
    public class Map
    {
        public Map()
        {
            CreationDateTime = DateTime.Now;
            MapId = Guid.NewGuid();
            Topics = new List<Topic>();
        }
        public Guid MapId { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public DateTime CreationDateTime { get; set; }

        [NotMapped]
        public bool IsSecure { get; set; }

        public Guid UserId { get; set; }

        [XmlIgnore]
        public virtual User User { get; set; }
        [XmlIgnore]
        public virtual ICollection<Topic> Topics { get; set; }


        #region GetHashCode, Equals, ToString override
        public override int GetHashCode()
        {
            return MapId.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (!(obj is User))
                return false;

            return Equals((User)obj);
        }
        public bool Equals(User other)
        {
            return MapId == other.UserId;
        }
        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}
