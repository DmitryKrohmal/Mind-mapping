namespace MindKeeper.ViewModel
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using MindKeeperBase.Model.EFContext;
    using MindKeeperBase.Security;
    using Base;
    public class LoginUCVM : ViewModelBase
    {
        #region FIELDS
        private GeneralVM _generalVM;

        private string _userLoginString;
        private string _userPasswordString;
        private bool _isRememberMe = false;

        private SolidColorBrush _loginBorderBrush;
        private SolidColorBrush _passwordColorBrush;

        private static readonly SolidColorBrush ErrorBorderBrush = Brushes.OrangeRed;
        private static readonly SolidColorBrush DefaultBorderBrush = Brushes.DimGray;

        private bool _isLoginError;
        private string _lastUserInfoPath = "LastUserInfo.usr";
        #endregion

        #region PROPERTIES

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

        public bool IsRememberMe
        {
            get { return _isRememberMe; }
            set
            {
                _isRememberMe = value;
                OnPropertyChanged("IsRememberMe");
            }
        }

        public SolidColorBrush LoginBorderBrush
        {
            get
            {
                if(_loginBorderBrush == null) _loginBorderBrush = DefaultBorderBrush;
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
                if (_passwordColorBrush == null) _passwordColorBrush = DefaultBorderBrush;
                return _passwordColorBrush;
            }

            set
            {
                _passwordColorBrush = value;
                OnPropertyChanged("PasswordBorderBrush");
            }
        }

        public GeneralVM GeneralVM
        {
            get
            {
                if (_generalVM == null) _generalVM = GeneralVM.Instance();
                return _generalVM;
            }
        }
        #endregion

        private bool hasErrors()
        {
            return string.IsNullOrEmpty(UserLoginString) || string.IsNullOrEmpty(UserPasswordString);
        }

        private bool CheckErrors()
        {
            _isLoginError = false;
            if (hasErrors())
            {
                _isLoginError = true;
                LoginBorderBrush = string.IsNullOrEmpty(UserLoginString) ? ErrorBorderBrush : DefaultBorderBrush;
                PasswordBorderBrush = string.IsNullOrEmpty(UserPasswordString) ? ErrorBorderBrush : DefaultBorderBrush;
            }

            return hasErrors();
        }

        #region COMMANDS

        private DelegateCommand _loginCommand;
        private DelegateCommand _isRememberMeCommand;

        public ICommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                    _loginCommand = new DelegateCommand(ExecuteLoginCommand);
                return _loginCommand;
            }
        }

        public ICommand IsRememberMeCommand
        {
            get
            {
                if (_isRememberMeCommand == null)
                    _isRememberMeCommand = new DelegateCommand(ExecuteIsRememberMeCommand);
                return _isRememberMeCommand;
            }
        }

        private void ExecuteLoginCommand(object parameter)
        {
            CheckErrors();
            if (!_isLoginError)
            {
                try
                {
                    using (MKDbContext db = new MKDbContext())
                    {
                        var encryptedPass = Security.EncryptPassword(UserLoginString, UserPasswordString);
                        GeneralVM.ActiveUser = db.Users.FirstOrDefault(u => u.Login == UserLoginString &&
                                                                                  u.Password == encryptedPass);
                        if (GeneralVM.ActiveUser == null)
                        {
                            MessageBox.Show("Login failed. User data is wrong or user is not exist.");
                            return;
                        }
                    }
                    
                    //GeneralVM.ActiveUser = User.Find(UserLoginString, UserPasswordString);
                    //User.Users.Remove(User.Users.Values.ToList()[0].UserId);
                    MainWindow mainWindow = new MainWindow();
                    CloseWindow();
                    mainWindow.Show();
                }
                catch (Exception)
                {
                    _isLoginError = true;
                    MessageBox.Show("Login failer. Check data.");
                    //ErrorLoginTextVisibility = Visibility.Visible;
                }
            }

            if (!_isLoginError && IsRememberMe)
            {
                try
                {
                    var str = GeneralVM.ActiveUser.Login + "|" + UserPasswordString;
                    Security.SerializeProtectedString(str, _lastUserInfoPath);
                }
                catch (Exception)
                {
                }
            }
        }

        private void CloseWindow()
        {
            ((App)Application.Current).CloseLoginWindow();
        }

        private void ExecuteIsRememberMeCommand(object parameter)
        {
            if (IsRememberMe)
            {
                if (!string.IsNullOrEmpty(UserLoginString) || !string.IsNullOrEmpty(UserPasswordString))
                {
                    IsRememberMe = true;
                    try
                    {
                        var str = UserLoginString + "|" + UserPasswordString;
                        Security.SerializeProtectedString(str, _lastUserInfoPath);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            else
            {
                IsRememberMe = false;
                if (File.Exists(_lastUserInfoPath))
                {
                    File.Delete(_lastUserInfoPath);
                }
            }
        }

        #endregion

        #region WINDOW LOADED COMMAND
        private DelegateCommand _windowLoadedCommand;
        public ICommand WidnowLodadedCommand
        {
            get
            {
                if (_windowLoadedCommand == null)
                    _windowLoadedCommand = new DelegateCommand(ExecuteWindowLoadedCommand);
                return _windowLoadedCommand;
            }
        }

        public void ExecuteWindowLoadedCommand(object parameter)
        {
            try
            {
                var str = Security.DeserializeProtectedString(_lastUserInfoPath);
                if (!string.IsNullOrEmpty(str))
                {
                    var userLoginPass = str.Split('|');
                    if (userLoginPass.Count() != 2) throw new Exception();
                    IsRememberMe = true;
                    UserLoginString = userLoginPass[0];
                    UserPasswordString = userLoginPass[1];
                }
            }
            catch (Exception)
            {
                IsRememberMe = false;
            }
        }
        #endregion
    }
}
