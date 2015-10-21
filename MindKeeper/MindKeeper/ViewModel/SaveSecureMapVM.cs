using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using MindKeeper.ViewModel.Base;
using MindKeeperBase.Interfaces;
using MindKeeperBase.Model.SecureMapWorker;

namespace MindKeeper.ViewModel
{
    public class SaveSecureMapVM : ViewModelBase
    {
        #region FIELDS
        private GeneralVM _generalVm;

        private string _pathToFile;
        private bool _isProtected = false;
        private List<IMapEncrypter> _mapEncrypters;
        private string _password;
        private string _confirmPassword;
        private IMapEncrypter _selectedEncrypter;

        private SolidColorBrush _passwordBorderBrush;
        private SolidColorBrush _confirmPasswordBorderBrush;
        private SolidColorBrush _mapEncryptersBorderBrush;
        private SolidColorBrush _pathToFileBorderBrush;

        private static readonly SolidColorBrush ErrorBorderBrush = Brushes.OrangeRed;
        private static readonly SolidColorBrush DefaultBorderBrush = Brushes.DimGray;

        private bool _isFailed = false;
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

        public string PathToFile
        {
            get
            {
                if (string.IsNullOrEmpty(_pathToFile)) return string.Empty;
                return _pathToFile;
            }

            set
            {
                _pathToFile = value;
                OnPropertyChanged("PathToFile");
            }
        }
        public string Password
        {
            get
            {
                if (_password == null) return string.Empty;
                return _password;
            }

            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }
        public List<IMapEncrypter> MapEncrypters
        {
            get
            {
                if (_mapEncrypters == null)
                {
                    _mapEncrypters = new List<IMapEncrypter> {new SimpleWorker(), new DesWorker(), new AesWorker()};
                }
                return _mapEncrypters;
            }
        } 
        public string ConfirmPassword
        {
            get
            {
                if (_confirmPassword == null) return string.Empty;
                return _confirmPassword;
            }

            set
            {
                _confirmPassword = value;
                OnPropertyChanged("ConfirmPassword");
            }
        }
        public bool IsProtected
        {
            get { return _isProtected; }
            set
            {
                _isProtected = value;
                OnPropertyChanged("IsProtected");
                OnPropertyChanged("ProtectedSectionVisibility");
            }
        }
        public IMapEncrypter SelectedEncrypter
        {
            get { return _selectedEncrypter; }
            set
            {
                _selectedEncrypter = value;
                OnPropertyChanged("SelectedEncrypter");
            }
        }
        public Visibility ProtectedSectionVisibility
        {
            get { return IsProtected ? Visibility.Visible : Visibility.Collapsed; }
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
        public SolidColorBrush MapEncryptersBorderBrush
        {
            get
            {
                if (_mapEncryptersBorderBrush == null) _passwordBorderBrush = DefaultBorderBrush;
                return _mapEncryptersBorderBrush;
            }

            set
            {
                _mapEncryptersBorderBrush = value;
                OnPropertyChanged("MapEncryptersBorderBrush");
            }
        }
        public SolidColorBrush PathToFileBorderBrush
        {
            get
            {
                if (_pathToFileBorderBrush == null) _confirmPasswordBorderBrush = DefaultBorderBrush;
                return _pathToFileBorderBrush;
            }

            set
            {
                _pathToFileBorderBrush = value;
                OnPropertyChanged("PathToFileBorderBrush");
            }
        }
        #endregion

        private bool hasErrors()
        {
            if (IsProtected)
                return string.IsNullOrEmpty(PathToFile) || string.IsNullOrEmpty(Password) ||
                       string.IsNullOrEmpty(ConfirmPassword) || _selectedEncrypter == null || Password != ConfirmPassword;
            else return string.IsNullOrEmpty(PathToFile);
        }

        private bool CheckErrors()
        {
            _isFailed = false;
            if (hasErrors())
            {
                _isFailed = true;
                if (IsProtected)
                {
                    PathToFileBorderBrush = string.IsNullOrEmpty(PathToFile) ? ErrorBorderBrush : DefaultBorderBrush;
                    PasswordBorderBrush = string.IsNullOrEmpty(Password) ? ErrorBorderBrush : DefaultBorderBrush;
                    ConfirmPasswordBorderBrush = string.IsNullOrEmpty(ConfirmPassword) ? ErrorBorderBrush : DefaultBorderBrush;
                    ConfirmPasswordBorderBrush = ConfirmPassword != Password ? ErrorBorderBrush : DefaultBorderBrush;
                    MapEncryptersBorderBrush = SelectedEncrypter == null ? ErrorBorderBrush : DefaultBorderBrush;
                    PathToFileBorderBrush = string.IsNullOrEmpty(PathToFile) ? ErrorBorderBrush : DefaultBorderBrush;
                    return _isFailed;
                }
                else
                {
                    PathToFileBorderBrush = string.IsNullOrEmpty(PathToFile) ? ErrorBorderBrush : DefaultBorderBrush;
                    return _isFailed;
                }
            }
            return _isFailed;
        }

        #region COMMANDS

        private DelegateCommand _choosePathCommand;

        public DelegateCommand ChoosePathCommand
        {
            get
            {
                if(_choosePathCommand == null) _choosePathCommand = new DelegateCommand(ExecuteChoosePathCommand);
                return _choosePathCommand;
            }
        }

        private void ExecuteChoosePathCommand(object parameter)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "MyMindMap";
            dlg.DefaultExt = ".smmp";
            dlg.Filter = "Secure mind map files (*.smmp)|*.smmp| Mind map files (*.mmp)|*.mmp| All files (*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                PathToFile = dlg.FileName;
            }
        }


        private DelegateCommand _saveCommand;

        public DelegateCommand SaveCommand
        {
            get
            {
                if(_saveCommand == null) _saveCommand = new DelegateCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
                return _saveCommand;
            }
        }

        private void ExecuteSaveCommand(object parameter)
        {
            CheckErrors();
            if (!_isFailed)
            {
                var worker = new SecureMapWorker(PathToFile, Password, GeneralVm.ActiveMap, SelectedEncrypter);
                worker.EncryptAndSerialize();
                System.Windows.Forms.MessageBox.Show(PathToFile + " serialized");
            }
        }

        private bool CanExecuteSaveCommand(object parameter)
        {
            if (GeneralVm.ActiveMap == null || string.IsNullOrEmpty(PathToFile)) return false;
            return true;
        }
        #endregion
    }
}
