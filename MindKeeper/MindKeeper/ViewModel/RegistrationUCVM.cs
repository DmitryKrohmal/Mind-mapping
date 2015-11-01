namespace MindKeeper.ViewModel
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using MindKeeperBase.Model;
    using MindKeeperBase.Model.EFContext;
    using MindKeeperBase.Security;
    using Base;
    public class RegistrationUCVM : ViewModelBase, INotifyDataErrorInfo
    {
        #region

        private string _userLoginString;
        private string _userPasswordString;
        private string _userConfirmPasswordString;

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
                _userPasswordString = value;
                Validate();
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
                Validate();
                OnPropertyChanged("UserConfirmPasswordString");
            }
        }
        #endregion

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
            Validate();
            if (!string.IsNullOrEmpty(UserLoginString) && !string.IsNullOrEmpty(UserPasswordString) 
                && !string.IsNullOrEmpty(UserConfirmPasswordString) && UserConfirmPasswordString == UserPasswordString)
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
                    MessageBox.Show("Registration failer :(");
                }
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

            if (listLoginErrors.Count > 0)
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

            if (listPasswordErrors.Count > 0)
            {
                OnPropertyErrorsChanged("UserPasswordString");
            }

            //validate confirm password string
            List<string> listConfirmPasswordErrors;
            if (propErrors.TryGetValue(UserLoginString, out listConfirmPasswordErrors) == false)
                listConfirmPasswordErrors = new List<string>();
            else
                listConfirmPasswordErrors.Clear();

            if (string.IsNullOrEmpty(UserConfirmPasswordString))
                listConfirmPasswordErrors.Add("User confirm password should not be empty.");

            if (UserConfirmPasswordString != UserPasswordString)
                listConfirmPasswordErrors.Add("Password is not equals confirm password.");

            propErrors["UserConfirmPasswordString"] = listConfirmPasswordErrors;

            if (listConfirmPasswordErrors.Count > 0)
            {
                OnPropertyErrorsChanged("UserConfirmPasswordString");
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
