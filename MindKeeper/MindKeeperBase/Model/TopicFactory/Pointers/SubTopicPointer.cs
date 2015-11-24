using MindKeeperBase.Interfaces;
using MindKeeperBase.Model.TopicConnection;

namespace MindKeeperBase.Model.TopicFactory.Pointers
{
    public class SubTopicPointer : TopicPointer
    {
        public SubTopicPointer()
        {
        }
        public SubTopicPointer(ITopicConnection connectionType, Topic childTopic)
        {
            Topic = childTopic;
            var connector = new TopicConnector(connectionType, childTopic);
            //Path = connector.CreatePathConnection();

            if (childTopic.Parent.Pointer == null) Thickness = 5;
            else if (childTopic.Parent.Pointer.Thickness != 1) Thickness = childTopic.Parent.Pointer.Thickness - 1;
            else Thickness = 1;

            //Brush = Topic.Brush;
        }
    }
}
