using MindKeeperBase.Interfaces;
using MindKeeperBase.Model.TopicFactory.Bodes;
using MindKeeperBase.Model.TopicFactory.Pointers;

namespace MindKeeperBase.Model.TopicFactory.Factories
{
    public class SubTopicFactory : TopicFactory
    {
        private Topic _topic;
        public override Topic CreateTopic(Map map, Topic parentTopic)
        {
            _topic = parentTopic;
            return new SubTopicBode(parentTopic);
        }

        public override TopicPointer CreateTopicPointer(ITopicConnection connectionType)
        {
            return new SubTopicPointer(connectionType, _topic);
        }
    }
}
