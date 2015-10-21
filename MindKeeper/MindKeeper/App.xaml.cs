namespace MindKeeper
{
    using System.Windows;
    using MindKeeper.View;
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private LoginWindow _loginWindow;
        public void CloseLoginWindow()
        {
            if (_loginWindow != null)
                _loginWindow.Close();
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            _loginWindow = new LoginWindow();
            _loginWindow.Show();
        }
    }
}
