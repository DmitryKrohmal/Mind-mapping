using MindKeeperBase.Interfaces;
using MindKeeperBase.Model;
using MindKeeperBase.Model.TopicConnection;

namespace MindKeeper.TopicDisplayFactory.Pointers
{
    public class NormalStyleTopicPointer : TopicPointerElement
    {
        public NormalStyleTopicPointer(ITopicConnection connectionType, TopicPointer pointer)
        {
            var connector = new TopicConnector(connectionType, pointer.Topic);
            Path = connector.CreatePathConnection();
            Thickness = pointer.Thickness;
            Brush = pointer.Topic.Brush;
        }
    }
}
