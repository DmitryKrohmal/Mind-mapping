using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml.Serialization;
using MindKeeperBase.Interfaces;

namespace MindKeeperBase.Model
{
    [Serializable]
    public class ImageAttachment : ICloneable
    {
        [Key]
        public Guid AttachmentId { get; set; }
        public string FileName { get; set; }
        
        [XmlIgnore]
        [NotMapped]
        public Bitmap Image { get; set; }

        public byte[] FileData
        {
            get
            {
                if (Image == null) return null;
                using (MemoryStream ms = new MemoryStream())
                {
                    Image.Save(ms, ImageFormat.Bmp);
                    return ms.ToArray();
                }
            }
            set
            {
                if (value == null)
                {
                    Image = null;
                }
                else
                {
                    using (MemoryStream ms = new MemoryStream(value))
                    {
                        Image = new Bitmap(ms);
                    }
                }
            }
        }

        public void Initialize(string path)
        {
            PathToFile = path;
            Image = new Bitmap(path);
        }

        [XmlIgnore]
        [NotMapped]
        public string PathToFile { get; set; }

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
