using System.Windows.Controls;
using MindKeeperBase.Interfaces;
using MindKeeperBase.Model;

namespace MindKeeper.TopicDisplayFactory
{
    public abstract class DisplayTopicFactory
    {
        public abstract UserControl CreateTopicElement(Topic topic);
        public abstract TopicPointerElement CreatePointerElement(ITopicConnection connectionType, TopicPointer pointer);
        public abstract void DisplayTopicElement(Panel panel, Topic topic, UserControl topicElement);
        public abstract void DisplayPointerElement(Panel panel, TopicPointerElement topicPointerElement);
    }
}
