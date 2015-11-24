using MindKeeperBase.Interfaces;

namespace MindKeeperBase.Model.TopicFactory
{
    public class TopicWorker
    {
        private Topic _topic;
        private TopicPointer _topicPointer;

        public TopicWorker(Factories.TopicFactory factory, Map map, Topic parentTopic, ITopicConnection connectionType)
        {
            _topic = factory.CreateTopic(map, parentTopic);
            _topicPointer = factory.CreateTopicPointer(connectionType);

            _topic.Pointer = _topicPointer;
        }

        public Topic GetTopic()
        {
            return _topic;
        }
    }
}
