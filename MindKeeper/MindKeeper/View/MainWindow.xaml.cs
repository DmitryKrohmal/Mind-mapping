using System.Collections.Generic;
using System.Collections.Specialized;
using MindKeeper.TopicDisplayFactory;
using MindKeeper.TopicDisplayFactory.Factories;
using MindKeeper.ViewModel;
using MindKeeperBase.Model.TopicConnection;

namespace MindKeeper
{
    using System.Windows;
    using System.Windows.Controls;
    using MindKeeperBase.Model;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowVM vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = (MainWindowVM) DataContext;
            vm.ActiveMapTopics.CollectionChanged += ActiveMapTopics_CollectionChanged;
            newTopics = new List<Topic>();
        }

        private List<Topic> newTopics;
        void ActiveMapTopics_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedAction action = e.Action;
            if (action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                    {
                        foreach(Topic newItem in e.NewItems)
                        {
                            newTopics.Add(newItem);
                            
                            DisplayTopic(newItem);
                        }
                    }
            }
            if (action == NotifyCollectionChangedAction.Reset)
            {
                RefreshCanvas();
            }
        }

        private void RefreshCanvas()
        {
            MapCanvas.Children.Clear();
            foreach (var i in vm.ActiveMapTopics)
            {
                i.InitializeTopic();
                DisplayTopic(i);
            }
        }

        private void DisplayTopic(Topic t)
        {
            TopicDisplayWorker worker = new TopicDisplayWorker(new NormalStyleTopicFactory(), t, new LineConnection(), MapCanvas);
            worker.DisplayTopic();
        }

        private void FileMenuBtn_OnClick(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsEnabled = true;
            (sender as Button).ContextMenu.PlacementTarget = sender as Button;
            (sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as Button).ContextMenu.IsOpen = true;
        }
    }
}