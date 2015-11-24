using MindKeeperBase.Interfaces;
using MindKeeperBase.Model.TopicFactory.Bodes;
using MindKeeperBase.Model.TopicFactory.Pointers;

namespace MindKeeperBase.Model.TopicFactory.Factories
{
    public class MainTopicFactory : TopicFactory
    {
        public override Topic CreateTopic(Map map, Topic parentTopic)
        {
            return new MainTopicBode(map);
        }

        public override TopicPointer CreateTopicPointer(ITopicConnection connectionType)
        {
            return new MainTopicPointer();
        }
    }
}
