using System.Windows.Controls;
using MindKeeper.TopicDisplayFactory.Pointers;
using MindKeeper.View;
using MindKeeper.ViewModel;
using MindKeeperBase.Interfaces;
using MindKeeperBase.Model;
using MindKeeperBase.Model.TopicConnection;

namespace MindKeeper.TopicDisplayFactory.Factories
{
    public class NormalStyleTopicFactory : DisplayTopicFactory
    {
        public override UserControl CreateTopicElement(Topic topic)
        {
            UserControl topicEl = new NormalTopicElementUC();
            topicEl.DataContext = new TopicElementUCVM(topic);

            return topicEl;
        }

        public override TopicPointerElement CreatePointerElement(ITopicConnection connectionType, TopicPointer pointer)
        {
            if (pointer.Topic.Parent == null) return null;
            TopicPointerElement tp = new NormalStyleTopicPointer(connectionType, pointer);
            var connector = new TopicConnector(connectionType, pointer.Topic);
            tp.Path = connector.CreatePathConnection();
            tp.Thickness = 5;
            tp.Brush = pointer.Topic.Brush;

            return tp;
        }

        public override void DisplayTopicElement(Panel panel, Topic topic, UserControl topicElement)
        {
            Canvas.SetLeft(topicElement, topic.Location.X);
            Canvas.SetTop(topicElement, topic.Location.Y);
            panel.Children.Add(topicElement);
        }

        public override void DisplayPointerElement(Panel panel, TopicPointerElement topicPointerElement)
        {
            topicPointerElement.Path.StrokeThickness = topicPointerElement.Thickness;
            topicPointerElement.Path.Stroke = topicPointerElement.Brush;
            panel.Children.Add(topicPointerElement.Path);
        }
    }
}
