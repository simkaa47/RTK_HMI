using DataAccess;
using DataAccess.Models;
using DataAccess.Repositories;
using System.Collections.Generic;

namespace RTK_HMI.ViewModels
{
    public class MainViewModel : PropertyChangedBase
    {

        #region Версия ПО
        /// <summary>
        /// Версия ПО
        /// </summary>
        private string _version="1.1";
        /// <summary>
        /// Версия ПО
        /// </summary>
        public string Version
        {
            get => _version;
            set => Set(ref _version, value);
        }
        #endregion

        public ParameterVm ParameterVm { get; set; }
        public ConnectViewModel ConnectVM { get; set; }
        public SaveLoadViewModel SaveLoadVM { get; set; }
        public UserVm UserVm { get; set; }


        public MainViewModel()
        {
            ParameterVm = new ParameterVm(this);
            ConnectVM = new ConnectViewModel(this);
            SaveLoadVM = new SaveLoadViewModel(this);
            UserVm = new UserVm(this);
        }



    }
}
