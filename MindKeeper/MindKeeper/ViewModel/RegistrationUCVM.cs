namespace MindKeeper.ViewModel
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using MindKeeperBase.Model;
    using MindKeeperBase.Model.EFContext;
    using MindKeeperBase.Security;
    using Base;
    public class RegistrationUCVM : ViewModelBase
    {
        #region

        private string _userLoginString;
        private string _userPasswordString;
        private string _userConfirmPasswordString;

        private SolidColorBrush _loginBorderBrush;
        private SolidColorBrush _passwordBorderBrush;
        private SolidColorBrush _confirmPasswordBorderBrush;

        private static readonly SolidColorBrush ErrorBorderBrush = Brushes.OrangeRed;
        private static readonly SolidColorBrush DefaultBorderBrush = Brushes.DimGray;

        private bool _isRegistrationFailed;
        private GeneralVM _generalVm;

        #endregion

        #region PROPERTIES

        public GeneralVM GeneralVm
        {
            get
            {
                if (_generalVm == null) _generalVm = GeneralVM.Instance();
                return _generalVm;
            }
        }

        public string UserLoginString
        {
            get
            {
                if (_userLoginString == null) return string.Empty;
                return _userLoginString;
            }

            set
            {
                _userLoginString = value;
                OnPropertyChanged("UserLoginString");
            }
        }

        public string UserPasswordString
        {
            get
            {
                if (_userPasswordString == null) return string.Empty;
                return _userPasswordString;
            }

            set
            {
                _userPasswordString = value;
                OnPropertyChanged("UserPasswordString");
            }
        }

        public string UserConfirmPasswordString
        {
            get
            {
                if (_userConfirmPasswordString == null) return string.Empty;
                return _userConfirmPasswordString;
            }

            set
            {
                _userConfirmPasswordString = value;
                OnPropertyChanged("UserConfirmPasswordString");
            }
        }

        public SolidColorBrush LoginBorderBrush
        {
            get
            {
                if (_loginBorderBrush == null) _loginBorderBrush = DefaultBorderBrush;
                return _loginBorderBrush;
            }

            set
            {
                _loginBorderBrush = value;
                OnPropertyChanged("LoginBorderBrush");
            }
        }

        public SolidColorBrush PasswordBorderBrush
        {
            get
            {
                if (_passwordBorderBrush == null) _passwordBorderBrush = DefaultBorderBrush;
                return _passwordBorderBrush;
            }

            set
            {
                _passwordBorderBrush = value;
                OnPropertyChanged("PasswordBorderBrush");
            }
        }

        public SolidColorBrush ConfirmPasswordBorderBrush
        {
            get
            {
                if (_confirmPasswordBorderBrush == null) _confirmPasswordBorderBrush = DefaultBorderBrush;
                return _confirmPasswordBorderBrush;
            }

            set
            {
                _confirmPasswordBorderBrush = value;
                OnPropertyChanged("ConfirmPasswordBorderBrush");
            }
        }

        #endregion

        private bool hasErrors()
        {
            return string.IsNullOrEmpty(UserLoginString) || string.IsNullOrEmpty(UserPasswordString)
                   || string.IsNullOrEmpty(UserConfirmPasswordString) || UserPasswordString != UserConfirmPasswordString;
        }

        private bool CheckErrors()
        {
            _isRegistrationFailed = false;
            if (hasErrors())
            {
                _isRegistrationFailed = true;
                LoginBorderBrush = string.IsNullOrEmpty(UserLoginString) ? ErrorBorderBrush : DefaultBorderBrush;
                PasswordBorderBrush = string.IsNullOrEmpty(UserPasswordString) ? ErrorBorderBrush : DefaultBorderBrush;
                ConfirmPasswordBorderBrush = string.IsNullOrEmpty(UserConfirmPasswordString) ||
                                             UserConfirmPasswordString != UserPasswordString
                    ? ErrorBorderBrush
                    : DefaultBorderBrush;
            }

            return hasErrors();
        }

        private void CloseWindow()
        {
            ((App)Application.Current).CloseLoginWindow();
        }

        #region COMMANDS

        private DelegateCommand _registerCommand;

        public ICommand RegisterCommand
        {
            get
            {
                if(_registerCommand == null)
                    _registerCommand = new DelegateCommand(ExecuteRegisterCommand);
                return _registerCommand;
            }
        }

        private void ExecuteRegisterCommand(object parameter)
        {
            CheckErrors();
            if (!_isRegistrationFailed)
            {
                try
                {
                    using (MKDbContext db = new MKDbContext())
                    {
                        User u = new User();
                        u.Login = UserLoginString;
                        u.Password = Security.EncryptPassword(UserLoginString, UserPasswordString);
                        u.HomeDirectoryPath = @".\Users\" + u.Login;

                        db.Users.Add(u);
                        db.SaveChanges();

                        GeneralVm.ActiveUser = u;

                        MainWindow mainWindow = new MainWindow();
                        CloseWindow();
                        mainWindow.Show();
                    }

                    //GeneralVM.ActiveUser = User.Find(UserLoginString, UserPasswordString);
                    //User.Users.Remove(User.Users.Values.ToList()[0].UserId);
                }
                catch (Exception)
                {
                    _isRegistrationFailed = true;
                    MessageBox.Show("Registration failer :(");
                    //ErrorLoginTextVisibility = Visibility.Visible;
                }
            }
        }

        #endregion
    }
}
