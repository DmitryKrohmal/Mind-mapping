using System.Windows;
using MindKeeper.ViewModel.Base;
using MindKeeperBase.Model;

namespace MindKeeper.ViewModel
{
    public class TopicElementUCVM : ViewModelBase
    {
        public TopicElementUCVM(Topic topic)
        {
            _topic = topic;
        }

        private Topic _topic;


        private string _topicName;
        public string TopicName
        {
            get
            {
                if (_topicName == null)
                    _topicName = _topic.Name;
                return _topicName;
            }
            set
            {
                _topicName = value;
                OnPropertyChanged("TopicName");
            }
        }


        public bool IsHaveNote
        {
            get
            {
                if (string.IsNullOrEmpty(_topic.Note))
                    return false;
                return true;
            }
        }
        public Visibility NoteButtonVisibility
        {
            get
            {
                if(IsHaveNote) return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }


        public bool IsHaveFileAttachment
        {
            get
            {
                if (_topic.FileAttachments == null || _topic.FileAttachments.Count == 0)
                    return false;
                return true;
            }
        }
        public Visibility FileAttachmentButtonVisibility
        {
            get
            {
                if(IsHaveFileAttachment) return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }


        public bool IsHaveImageAttachment
        {
            get
            {
                if (_topic.ImageAttachments == null || _topic.ImageAttachments.Count == 0)
                    return false;
                return true;
            }
        }
        public Visibility ImageAttachmentButtonVisibility
        {
            get
            {
                if (IsHaveImageAttachment) return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }


        public bool IsHaveMediaAttachment
        {
            get
            {
                if (_topic.MediaAttachments == null || _topic.MediaAttachments.Count == 0)
                    return false;
                return true;
            }
        }
        public Visibility MediaAttachmentButtonVisibility
        {
            get
            {
                if (IsHaveMediaAttachment) return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
    }
}
