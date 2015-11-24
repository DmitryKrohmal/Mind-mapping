using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Windows.Media;
using System.Xml.Serialization;
using MindKeeperBase.Model.TopicFactory.Bodes;

namespace MindKeeperBase.Model
{
    [Serializable]
    public abstract class Topic : ICloneable
    {
        public Topic()
        {
            TopicId = Guid.NewGuid();
            FileAttachments = new List<FileAttachment>();
            ImageAttachments = new List<ImageAttachment>();
            MediaAttachments = new List<MediaAttachment>();
            ChildTopics = new List<Topic>();
        }

        public Guid TopicId { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public string Note { get; set; }
        public ImportanceCategory Importance { get; set; }
        public Point Location { get; set; }
        public int Width { get; set; }
        public int Heigh { get; set; }
        [NotMapped]
        public SolidColorBrush Brush { get; set; }


        public virtual TopicPointer Pointer { get; set; }
        //public Guid PointerId { get; set; }


        public Guid MapId { get; set; }
        [XmlIgnore]
        public virtual Map Map { get; set; }
        [XmlIgnore]
        public virtual ICollection<FileAttachment> FileAttachments { get; set; }
        public virtual ICollection<ImageAttachment> ImageAttachments { get; set; }
        public virtual ICollection<MediaAttachment> MediaAttachments { get; set; }

            
        [XmlIgnore]
        public virtual Topic Parent { get; set; }
        public virtual ICollection<Topic> ChildTopics { get; set; }

        public abstract void InitializeTopic();


        #region GetHashCode, Equals, ToString override
        public override int GetHashCode()
        {
            return TopicId.GetHashCode();
        }



        public object Clone()
        {
            Topic cloneTopic = new NodeTopicBode(Map);
            cloneTopic.Name = Name;
            cloneTopic.Map = Map;
            cloneTopic.Location = new Point(Location.X + 75, Location.Y + 25);
            cloneTopic.MapId = Map.MapId;
            cloneTopic.Importance = Importance;
            cloneTopic.Note = string.Copy(Note);
            cloneTopic.Width = Width;
            cloneTopic.Heigh = Heigh;
            //cloneTopic.Parent = null;

            if (FileAttachments.Count != 0)
            {
                foreach (var attachment in FileAttachments)
                {
                    var att = (FileAttachment)attachment.Clone();
                    att.Topic = cloneTopic;
                    att.TopicId = cloneTopic.TopicId;
                    cloneTopic.FileAttachments.Add(att);
                }
            }

            if (ImageAttachments.Count != 0)
            {
                foreach (var attachment in ImageAttachments)
                {
                    var att = (ImageAttachment)attachment.Clone();
                    att.Topic = cloneTopic;
                    att.TopicId = cloneTopic.TopicId;
                    cloneTopic.ImageAttachments.Add(att);
                }
            }

            if (MediaAttachments.Count != 0)
            {
                foreach (var attachment in MediaAttachments)
                {
                    var att = (MediaAttachment)attachment.Clone();
                    att.Topic = cloneTopic;
                    att.TopicId = cloneTopic.TopicId;
                    cloneTopic.MediaAttachments.Add(att);
                }
            }

            return cloneTopic;
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
