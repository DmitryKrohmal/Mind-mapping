using MindKeeperBase.Interfaces;

namespace MindKeeperBase.Model.TopicConnection
{
    public class TopicConnector
    {
        private ITopicConnection _topicConnectionType;
        private Topic _activeTopic;

        public TopicConnector(ITopicConnection connectionType, Topic topic)
        {
            _topicConnectionType = connectionType;
            _activeTopic = topic;
        }

        public System.Windows.Shapes.Path CreatePathConnection()
        {
            return _topicConnectionType.CreatePathConnection(_activeTopic);
        }
    }
}
