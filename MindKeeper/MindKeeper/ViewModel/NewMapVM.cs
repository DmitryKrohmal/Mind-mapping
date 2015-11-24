using System;
using System.Data.Entity;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using MindKeeper.ViewModel.Base;
using MindKeeperBase.Model;
using MindKeeperBase.Model.TopicConnection;
using MindKeeperBase.Model.TopicFactory;
using MindKeeperBase.Model.TopicFactory.Factories;

namespace MindKeeper.ViewModel
{
    public class NewMapVM : ViewModelBase
    {
        private MainWindowVM _mainWindowVm;

        public NewMapVM(MainWindowVM mainWindowVm)
        {
            _mainWindowVm = mainWindowVm;
        }
        public GeneralVM GeneralVM
        {
            get { return GeneralVM.Instance(); }
        }

        private string _name;
        public string Name
        {
            get { return _name ?? string.Empty; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public string UserInfoString
        {
            get { return "User: " + GeneralVM.ActiveUser.Login; }
        }

        public string CreationDateTimeString
        {
            get { return "Creation date: " + DateTime.Now.ToShortDateString(); }
        }

        private SolidColorBrush ErrorBorderBrush = Brushes.Red;
        private SolidColorBrush OkBorderBrush = Brushes.Gray;
        private SolidColorBrush _nameBorderBrush;
        public SolidColorBrush NameBorderBrush
        {
            get
            {
                if (_nameBorderBrush == null)
                    _nameBorderBrush = OkBorderBrush;
                return _nameBorderBrush;
            }
            set
            {
                _nameBorderBrush = value;
                OnPropertyChanged("NameBorderBrush");
            }
        }


        private DelegateCommand _createMapCommand;
        public ICommand CreateMapCommand
        {
            get
            {
                if(_createMapCommand == null)
                    _createMapCommand = new DelegateCommand(ExecuteCreateMapCommand);
                return _createMapCommand;
            }
        }
        private void ExecuteCreateMapCommand(object parameter)
        {
            NameBorderBrush = OkBorderBrush;
            if (string.IsNullOrEmpty(Name))
            {
                NameBorderBrush = ErrorBorderBrush;
                return;
            }

            var map = CreateMap();
            var mainMapTopic = CreateTopicAndAddToMap(map);

            GeneralVM.MKDbContext.Topics.Add(mainMapTopic);
            GeneralVM.MKDbContext.TopicPointers.Add(mainMapTopic.Pointer);
            GeneralVM.MKDbContext.Maps.Add(map);
            GeneralVM.MKDbContext.Entry(GeneralVM.ActiveUser).State = EntityState.Modified;
            GeneralVM.MKDbContext.SaveChanges();

            _mainWindowVm.OnPropertyChanged("ActiveUserMapsCountString");

            Window window = parameter as Window;
            if (window != null)
            {
                window.Close();
            }
        }

        private Map CreateMap()
        {
            Map map = new Map();
            map.Name = Name;
            map.User = GeneralVM.ActiveUser;
            map.UserId = map.User.UserId;
            map.FilePath = @".\" + GeneralVM.ActiveUser.Login + @"\" + map.Name + @".mmp";
            map.Height = 600;
            map.Width = 800;
            return map;
        }
        private Topic CreateTopicAndAddToMap(Map map)
        {
            TopicWorker worker = new TopicWorker(new MainTopicFactory(), map, null, new LineConnection());
            var topic = worker.GetTopic();

            topic.Name = map.Name;
            map.Topics.Add(topic);
            GeneralVM.ActiveUser.Maps.Add(map);
            GeneralVM.ActiveMaps.Add(map);
            return topic;
        }
    }
}
