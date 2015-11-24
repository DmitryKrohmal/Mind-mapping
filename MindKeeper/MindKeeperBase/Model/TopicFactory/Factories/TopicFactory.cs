using MindKeeperBase.Interfaces;

namespace MindKeeperBase.Model.TopicFactory.Factories
{
    public abstract class TopicFactory
    {
        public abstract Topic CreateTopic(Map map, Topic parentTopic);
        public abstract TopicPointer CreateTopicPointer(ITopicConnection connectionType);
    }
}
