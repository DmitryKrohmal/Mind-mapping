using MindKeeperBase.Interfaces;
using MindKeeperBase.Model.TopicFactory.Bodes;
using MindKeeperBase.Model.TopicFactory.Pointers;

namespace MindKeeperBase.Model.TopicFactory.Factories
{
    public class NodeTopicFactory : TopicFactory
    {
        private Topic _topic;
        public override Topic CreateTopic(Map map, Topic parentTopic)
        {
            _topic = new NodeTopicBode(map);
            return _topic;
        }

        public override TopicPointer CreateTopicPointer(ITopicConnection connectionType)
        {
            return new NodeTopicPointer(connectionType, _topic);
        }
    }
}
