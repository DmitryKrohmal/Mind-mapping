using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media;

namespace MindKeeperBase.Model
{
    public class TopicPointer
    {
        public TopicPointer()
        {
            PointerId = Guid.NewGuid();
        }
        [Key]
        public Guid PointerId { get; set; }
        public int Thickness { get; set; }

        public virtual Topic Topic { get; set; }
        //public Guid TopicId { get; set; }
    }
}