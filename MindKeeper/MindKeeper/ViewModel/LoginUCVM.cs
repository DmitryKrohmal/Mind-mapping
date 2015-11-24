using System.Data.Entity;

namespace MindKeeper.ViewModel
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Collections;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Media;
    using Base;
    using MindKeeperBase.Model.EFContext;
    using MindKeeperBase.Security;

    public class LoginUCVM : ViewModelBase, INotifyDataErrorInfo
    {
        #region FIELDS
        private GeneralVM _generalVM;

        private string _userLoginString;
        private string _userPasswordString;
        private bool _isRememberMe = false;

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
                if (_userLoginString == value) return;
                _userLoginString = value;
                Validate();
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
                if (_userPasswordString == value) return;
                _userPasswordString = value;
                Validate();
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

        public GeneralVM GeneralVM
        {
            get
            {
                if (_generalVM == null) _generalVM = GeneralVM.Instance();
                return _generalVM;
            }
        }
        #endregion

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
            Validate();
            if(!string.IsNullOrEmpty(UserLoginString) && !string.IsNullOrEmpty(UserPasswordString))
            {
                try
                {
                        var encryptedPass = Security.EncryptPassword(UserLoginString, UserPasswordString);
                        GeneralVM.ActiveUser = GeneralVM.MKDbContext.Users.FirstOrDefault(u => u.Login == UserLoginString &&
                                                                                  u.Password == encryptedPass);
                        if (GeneralVM.ActiveUser == null)
                        {
                            MessageBox.Show("Login failed. User data is wrong or user is not exist.");
                            return;
                        }
                    
                    MainWindow mainWindow = new MainWindow();
                    CloseWindow();
                    mainWindow.Show();
                }
                catch (Exception)
                {
                    MessageBox.Show("Login failer. Check data.");
                }
            }

            if (!HasErrors && IsRememberMe)
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

        #region INotifyDataErrorInfo implementation

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            List<string> errors = new List<string>();
            if (propertyName != null)
            {
                propErrors.TryGetValue(propertyName, out errors);
                return errors;
            }
            else
                return null;
        }

        public bool HasErrors
        {
            get { return propErrors.Values.Any(l => l.Count > 0); }
        }


        Dictionary<string, List<string>> propErrors = new Dictionary<string, List<string>>();

        private void Validate()
        {
            Task.Run(() => DataValidation());
        }

        private void DataValidation()
        {
            //Validate Name property
            List<string> listLoginErrors;
            if (propErrors.TryGetValue(UserLoginString, out listLoginErrors) == false)
                listLoginErrors = new List<string>();
            else
                listLoginErrors.Clear();

            if (string.IsNullOrEmpty(UserLoginString))
                listLoginErrors.Add("User login should not be empty.");

            propErrors["UserLoginString"] = listLoginErrors;

            //if (listLoginErrors.Count > 0)
            {
                OnPropertyErrorsChanged("UserLoginString");
            }

            //Validate Password property
            List<string> listPasswordErrors;
            if (propErrors.TryGetValue(UserLoginString, out listPasswordErrors) == false)
                listPasswordErrors = new List<string>();
            else
                listPasswordErrors.Clear();

            if (string.IsNullOrEmpty(UserPasswordString))
                listPasswordErrors.Add("User password should not be empty.");

            propErrors["UserPasswordString"] = listPasswordErrors;

            //if (listPasswordErrors.Count > 0)
            {
                OnPropertyErrorsChanged("UserPasswordString");
            }
        }

        private void OnPropertyErrorsChanged(string p)
        {
            if (ErrorsChanged != null)
                ErrorsChanged.Invoke(this, new DataErrorsChangedEventArgs(p));
        }

        #endregion
    }
}
