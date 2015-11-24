using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Xml.Serialization;
using MindKeeperBase.Interfaces;

namespace MindKeeperBase.Model
{
    [Serializable]
    public class MediaAttachment : ICloneable
    {
        [Key]
        public Guid AttachmentId { get; set; }
        public string FileName { get; set; }

        public byte[] FileData { get; set; }

        public void Initialize(string path)
        {
            PathToFile = path;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                FileData = new byte[fs.Length];
                fs.Read(FileData, 0, FileData.Length);
            }
        }

        [XmlIgnore]
        [NotMapped]
        public string PathToFile { get; set; }

        //REMINDER! SoundPlayer can play file from byte[]! (with MemoryStream)


        public Guid TopicId { get; set; }
        [XmlIgnore]
        public virtual Topic Topic { get; set; }

        public object Clone()
        {
            ImageAttachment fileAttachmentClone = new ImageAttachment();
            fileAttachmentClone.FileName = string.Copy(FileName);
            Array.Copy(FileData, fileAttachmentClone.FileData, FileData.Length);
            return fileAttachmentClone;
        }
    }
}
