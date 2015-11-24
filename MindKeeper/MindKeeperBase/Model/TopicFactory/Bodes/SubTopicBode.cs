using System.Drawing;

namespace MindKeeperBase.Model.TopicFactory.Bodes
{
    public class SubTopicBode : Topic
    {
        public SubTopicBode()
        {
            
        }
        public SubTopicBode(Topic parentTopic) : base()
        {
            Parent = parentTopic;
            ParentId = parentTopic.TopicId;
            Location = GenerateLocation(parentTopic);
            Width = parentTopic.Width - 15;
            Heigh = parentTopic.Heigh - 8;
            Map = parentTopic.Map;
            MapId = parentTopic.MapId;
            Brush = Parent.Brush;
        }

        private Point GenerateLocation(Topic parentTopic)
        {
            return new Point(parentTopic.Location.X + parentTopic.Width + 50, parentTopic.Location.Y + 15);
        }

        public override void InitializeTopic()
        {
            Location = GenerateLocation(Parent);
            Width = Parent.Width - 15;
            Heigh = Parent.Heigh - 8;
            Brush = Parent.Brush;
        }
    }
}
