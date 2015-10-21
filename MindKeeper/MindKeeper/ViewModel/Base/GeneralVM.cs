namespace MindKeeper.ViewModel.Base
{
    using MindKeeperBase.Model;
    public class GeneralVM : ViewModelBase
    {
        #region Singleton implementation
        private static GeneralVM instance;
        protected GeneralVM()
        { }

        public static GeneralVM Instance()
        {
            if(instance == null)
                instance = new GeneralVM();

            return instance;
        }
        #endregion

        private User _activeUser;
        public User ActiveUser
        {
            get { return _activeUser; }
            set
            {
                _activeUser = value;
                OnPropertyChanged("ActiveUser");
            }
        }


        private Map _activeMap;
        public Map ActiveMap
        {
            get { return _activeMap; }
            set
            {
                _activeMap = value;
                OnPropertyChanged("ActiveMap");
            }
        }
    }
}
