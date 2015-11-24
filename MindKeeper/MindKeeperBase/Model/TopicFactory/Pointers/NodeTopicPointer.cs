using MindKeeperBase.Interfaces;
using MindKeeperBase.Model.TopicConnection;

namespace MindKeeperBase.Model.TopicFactory.Pointers
{
    public class NodeTopicPointer : TopicPointer
    {
        public NodeTopicPointer()
        {
        }
        public NodeTopicPointer(ITopicConnection connectionType, Topic childTopic)
        {
            Topic = childTopic;
            var connector = new TopicConnector(connectionType, childTopic);
            //Path = connector.CreatePathConnection();

            Thickness = 5;

            //Brush = Topic.Brush;
        }
    }
}
