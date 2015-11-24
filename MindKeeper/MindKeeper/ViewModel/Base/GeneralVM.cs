using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using MindKeeperBase.Model.EFContext;

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


        private ObservableCollection<Map> _activeMaps;
        public ObservableCollection<Map> ActiveMaps
        {
            get
            {
                if (_activeMaps == null)
                {
                    _activeMaps = new ObservableCollection<Map>();
                    foreach (var m in ActiveUser.Maps)
                    {
                        _activeMaps.Add(m);
                    }
                }

                return _activeMaps;
            }
            set
            {
                _activeMaps = value;
                OnPropertyChanged("ActiveMaps");
            }
        }



        private MKDbContext _mkDbContext;
        public MKDbContext MKDbContext
        {
            get
            {
                if(_mkDbContext == null)
                    _mkDbContext = new MKDbContext();
                return _mkDbContext;
            }
        }
    }
}
