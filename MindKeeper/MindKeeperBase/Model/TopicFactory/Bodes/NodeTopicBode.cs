using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Media;
using Brushes = System.Windows.Media.Brushes;

namespace MindKeeperBase.Model.TopicFactory.Bodes
{
    public class NodeTopicBode : Topic
    {
        public NodeTopicBode()
        {
            
        }
        public NodeTopicBode(Map map)
        {
            Parent = map.MainTopic;
            ParentId = map.MainTopic.TopicId;
            Location = GenerateLocation(map.MainTopic);
            Width = map.MainTopic.Width - 25;
            Heigh = map.MainTopic.Heigh - 15;
            Map = map;
            MapId = map.MapId;
            Brush = GenerateRandomBrush();
        }

        private Point GenerateLocation(Topic parentTopic)
        {
            return new Point(parentTopic.Location.X + parentTopic.Width + 50, parentTopic.Location.Y + 60);
        }

        private SolidColorBrush GenerateRandomBrush()
        {
            SolidColorBrush result = Brushes.Transparent;

            Random rnd = new Random();

            Type brushesType = typeof(Brushes);

            PropertyInfo[] properties = brushesType.GetProperties();

            int random = rnd.Next(properties.Length);
            result = (SolidColorBrush)properties[random].GetValue(null, null);

            return result;
        }

        public override void InitializeTopic()
        {
            Parent = Map.MainTopic;
            ParentId = Map.MainTopic.TopicId;
            Location = GenerateLocation(Map.MainTopic);
            Brush = GenerateRandomBrush();
        }
    }
}
