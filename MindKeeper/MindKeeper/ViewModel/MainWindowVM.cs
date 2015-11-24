using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MindKeeper.View;
using MindKeeper.ViewModel.Base;
using MindKeeperBase.Model;
using MindKeeperBase.Model.TopicConnection;
using MindKeeperBase.Model.TopicFactory;
using MindKeeperBase.Model.TopicFactory.Factories;

namespace MindKeeper.ViewModel
{
    public class MainWindowVM : ViewModelBase
    {
        public MainWindowVM()
        {
        }
        private GeneralVM _generalVm;
        public GeneralVM GeneralVm
        {
            get
            {
                if (_generalVm == null)
                    _generalVm = GeneralVM.Instance();
                return _generalVm;
            }
        }
        public int ActiveUserMapsCount
        {
            get
            {
                return GeneralVm.ActiveUser.Maps != null ? GeneralVm.ActiveUser.Maps.Count : 0;
            }
        }
        public string ActiveUserMapsCountString
        {
            get { return "Maps count: " + ActiveUserMapsCount; }
        }
        public string SelectedMapName
        {
            get { return SelectedMap == null ? "Map is not selected" : SelectedMap.Name; }
        }
        public string CreationDateTimeString
        {
            get
            {
                return SelectedMap == null ? string.Empty :
                SelectedMap.CreationDateTime.ToUniversalTime().ToString();
            }
        }


        private Map _selectedMap;
        public Map SelectedMap
        {
            get { return _selectedMap; }
            set
            {
                _selectedMap = value;
                ActiveMapTopics.Clear();
                foreach (var t in SelectedMap.Topics)
                {
                    t.InitializeTopic();
                    ActiveMapTopics.Add(t);
                }
                RefreshProperties();
            }
        }


        private ObservableCollection<Topic> _activeMapTopics;
        public ObservableCollection<Topic> ActiveMapTopics
        {
            get
            {
                if (_activeMapTopics == null)
                    _activeMapTopics = new ObservableCollection<Topic>();

                return _activeMapTopics;
            }
            set
            {
                _activeMapTopics = value;
                OnPropertyChanged("ActiveMapTopics");
            }
        }


        private DelegateCommand _newMapCommand;
        public ICommand NewMapCommand
        {
            get
            {
                if(_newMapCommand == null)
                    _newMapCommand = new DelegateCommand(ExecuteNewMapCommand);
                return _newMapCommand;
            }
        }
        private void ExecuteNewMapCommand(object parameter)
        {
            NewMapWindow nmw = new NewMapWindow();
            nmw.DataContext = new NewMapVM(this);
            nmw.ShowDialog();
        }


        private DelegateCommand _windowGotFocusCommand;
        public ICommand WindowGotFocusCommand
        {
            get
            {
                if (_windowGotFocusCommand == null)
                    _windowGotFocusCommand = new DelegateCommand(ExecuteWindowGotFocusCommand);
                return _windowGotFocusCommand;
            }
        }
        private void ExecuteWindowGotFocusCommand(object parameter)
        {
            RefreshProperties();
        }



        private DelegateCommand _newTopicCommand;
        public ICommand NewTopicCommand
        {
            get
            {
                if (_newTopicCommand == null)
                    _newTopicCommand = new DelegateCommand(ExecuteNewTopicCommand);
                return _newTopicCommand;
            }
        }

        private void ExecuteNewTopicCommand(object parameter)
        {
            SelectedMap.MainTopic = SelectedMap.Topics[0]; //FIX!
            TopicWorker tw = new TopicWorker(new NodeTopicFactory(), SelectedMap, SelectedMap.MainTopic, new LineConnection());
            Topic t = tw.GetTopic();

            SelectedMap.Topics.Add(t);
            GeneralVm.MKDbContext.Topics.Add(t);
            GeneralVm.MKDbContext.SaveChanges();

            ActiveMapTopics.Add(t);
        }


        
        private void RefreshProperties()
        {
            OnPropertyChanged("SelectedMap");
            GeneralVm.OnPropertyChanged("ActiveMaps");
            OnPropertyChanged("SelectedMapName");
            OnPropertyChanged("CreationDateTimeString");
            OnPropertyChanged("ActiveMapTopics");
        }
    }
}
