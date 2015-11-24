using System.Windows.Controls;
using MindKeeperBase.Interfaces;
using MindKeeperBase.Model;

namespace MindKeeper.TopicDisplayFactory
{
    public class TopicDisplayWorker
    {
        private UserControl _topicElement;
        private TopicPointerElement _topicPointerElement;
        private DisplayTopicFactory _factory;
        private Topic _topic;
        private Panel _panel;


        public TopicDisplayWorker(DisplayTopicFactory factory, Topic topic, ITopicConnection connectionType, Panel panel)
        {
            _panel = panel;
            _topic = topic;
            _factory = factory;
            _topicElement = _factory.CreateTopicElement(topic);
            _topicPointerElement = _factory.CreatePointerElement(connectionType, topic.Pointer);
        }

        public void DisplayTopic()
        {
            _factory.DisplayTopicElement(_panel, _topic, _topicElement);
            if(_topicPointerElement != null) _factory.DisplayPointerElement(_panel, _topicPointerElement);
        }
    }
}
