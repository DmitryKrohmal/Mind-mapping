namespace MindKeeper.View
{
    using System.Windows.Media.Animation;
    using System;
    using System.Windows;
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private bool _isLoginUC;
        public LoginWindow()
        {
            InitializeComponent();
            _isLoginUC = true;
        }


        private void Timeline_OnCompleted(object sender, EventArgs e)
        {
            if (_isLoginUC)
            {
                RegistrationUC regUC = new RegistrationUC();
                ContentGrid.Children.Clear();
                ContentGrid.Children.Add(regUC);

                LoginRegTextBlock.Text = "or login now!";
                _isLoginUC = false;
            }
            else
            {
                LoginUC loginUC = new LoginUC();
                ContentGrid.Children.Clear();
                ContentGrid.Children.Add(loginUC);

                LoginRegTextBlock.Text = "or register now!";
                _isLoginUC = true;
            }

            ContentGrid.BeginStoryboard(this.Resources["ShowContentGridStoryboard"] as Storyboard);
        }
    }
}
