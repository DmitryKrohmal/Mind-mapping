using System.Windows.Media;

namespace MindKeeper.TopicDisplayFactory
{
    public abstract class TopicPointerElement
    {
        public int Thickness { get; set; }
        public System.Windows.Shapes.Path Path { get; set; }
        public SolidColorBrush Brush { get; set; }
    }
}
