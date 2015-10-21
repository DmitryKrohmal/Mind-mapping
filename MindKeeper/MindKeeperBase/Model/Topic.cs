using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Drawing;
using System.Xml.Serialization;
using MindKeeperBase.Interfaces;
using MindKeeperBase.Model.EFContext;

namespace MindKeeperBase.Model
{
    [Serializable]
    public class Topic
    {
        public Topic()
        {
            TopicId = Guid.NewGuid();
            Attachments = new List<IAttachment>();
        }
        public Guid TopicId { get; set; }
        public string Name { get; set; }
        public Guid ParentId { get; set; }
        public string Note { get; set; }
        public ImportanceCategory Importance { get; set; }
        public Point Location { get; set; }
        public int Width { get; set; }
        public int Heigh { get; set; }


        public Guid MapId { get; set; }
        [XmlIgnore]
        public virtual Map Map { get; set; }
        [XmlIgnore]
        public virtual ICollection<IAttachment> Attachments { get; set; }


        [XmlIgnore]
        [NotMapped]
        public Topic Parent {
            get
            {
                using (MKDbContext db = new MKDbContext())
                {
                    return db.Topics.Find(ParentId);
                }
            }
            set { ParentId = value.TopicId; }
        }

        #region GetHashCode, Equals, ToString override
        public override int GetHashCode()
        {
            return TopicId.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (!(obj is User))
                return false;

            return Equals((User)obj);
        }
        public bool Equals(User other)
        {
            return TopicId == other.UserId;
        }
        public override string ToString()
        {
            return Name;
        }
        #endregion
    }

    public enum ImportanceCategory
    {
        Low = 0,
        Normal,
        High,
        Critical
    }
}
