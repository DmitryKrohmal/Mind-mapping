using MindKeeper.View;
using MindKeeper.ViewModel.Base;

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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FileMenuBtn_OnClick(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsEnabled = true;
            (sender as Button).ContextMenu.PlacementTarget = sender as Button;
            (sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as Button).ContextMenu.IsOpen = true;
        }

        private void GenerateMapItem_OnClick(object sender, RoutedEventArgs e)
        {
            var m = new Map();
            m.Name = "NewTestMap";
            m.FilePath = @".\" + GeneralVM.Instance().ActiveUser.Login + @"\" + m.Name + @".mmp";
            m.UserId = GeneralVM.Instance().ActiveUser.UserId;
            Topic t = new Topic();
            t.Name = "NEW TOPIC!!!";
            m.Topics.Add(t);
            GeneralVM.Instance().ActiveMap = m;
        }

        private void SaveSecureMapItem_OnClick(object sender, RoutedEventArgs e)
        {
            SaveSecureMapWindow ss = new SaveSecureMapWindow();
            ss.Owner = this;
            ss.ShowDialog();
        }

        private void OpenSecureMapItem_OnClick(object sender, RoutedEventArgs e)
        {
            OpenSecureMapWindow os = new OpenSecureMapWindow();
            os.Owner = this;
            os.ShowDialog();
        }
    }
}