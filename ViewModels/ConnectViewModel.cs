using DataAccess;
using DataAccess.Models;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTK_HMI.ViewModels
{
    internal class ConnectViewModel:PropertyChangedBase
    {

        private readonly IRepository<ConnectSettings> _connectRepository;
        public ConnectViewModel(MainViewModel mainView )
        {
            MainView = mainView;
            _connectRepository = new EfRepository<ConnectSettings>();  
            Init();
        }

        public MainViewModel MainView { get; }

        #region Настроки связи
        private ConnectSettings _connectSettings;
        public ConnectSettings ConnectSettings
        {
            get => _connectSettings;
            set => Set(ref _connectSettings, value);
        }
        #endregion

        void Init()
        {
            ConnectSettings = _connectRepository.Init(new List<ConnectSettings> { new ConnectSettings() }).FirstOrDefault();
        }

    }
}
