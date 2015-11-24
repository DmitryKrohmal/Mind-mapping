using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using MindKeeperBase.Interfaces;

namespace MindKeeperBase.Model.TopicConnection
{
    public class LineConnection : ITopicConnection
    {
        public Path CreatePathConnection(Topic topic)
        {
            Path path = new Path();
            PathGeometry geometry = new PathGeometry();
            PathFigure figure = new PathFigure();
            figure.StartPoint = GetStartPoint(topic);
            figure.Segments.Add(new LineSegment()
            {
                Point = GetEndPoint(topic)
            });
            geometry.Figures.Add(figure);
            path.Data = geometry;
            return path;
        }

        private Point GetEndPoint(Topic topic)
        {
            if(topic.Location.X < topic.Parent.Location.X)
                return new Point(topic.Location.X + topic.Width, topic.Location.Y + topic.Heigh/2);
            if(topic.Location.X > topic.Parent.Location.X)
                return new Point(topic.Location.X, topic.Location.Y + topic.Heigh/2);
            return new Point(topic.Location.X, topic.Location.Y);
        }

        private Point GetStartPoint(Topic topic)
        {
            //if element under parent
            if(topic.Location.Y > topic.Parent.Location.Y + 10)
                return new Point(topic.Parent.Location.X + topic.Parent.Width/2, topic.Parent.Location.Y + topic.Parent.Heigh + 7);
            //if element above parent
            if(topic.Location.Y < topic.Parent.Location.Y - 15)
                return new Point(topic.Parent.Location.X + topic.Parent.Width / 2, topic.Parent.Location.Y);
            //if element left from parent
            if(topic.Location.X < topic.Parent.Location.X)
                return new Point(topic.Parent.Location.X, topic.Parent.Location.Y + topic.Parent.Heigh/2);
            //if element right from parent
            if(topic.Location.X > topic.Parent.Location.X)
                return new Point(topic.Parent.Location.X + topic.Parent.Width, topic.Parent.Location.Y + topic.Parent.Heigh / 2);
            return new Point(topic.Location.X, topic.Location.Y);
        }
    }
}
