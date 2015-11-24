using System.Drawing;

namespace MindKeeperBase.Model.TopicFactory.Bodes
{
    public class MainTopicBode : Topic
    {
        public MainTopicBode()
        {
            
        }
        public MainTopicBode(Map map)
        {
            Location = new Point(map.Width/2, map.Height/2);
            Width = 200;
            Heigh = 120;
            Map = map;
            MapId = map.MapId;
            Brush = System.Windows.Media.Brushes.Black;
        }

        public override void InitializeTopic()
        {
            Location = new Point(Map.Width / 2, Map.Height / 2);
            Width = 200;
            Heigh = 66;
            Brush = System.Windows.Media.Brushes.Black;
        }
    }
}
