using DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTK_HMI.ViewModels
{
    internal class ConnectViewModel:PropertyChangedBase
    {
        public ConnectViewModel(MainViewModel mainView )
        {
            MainView = mainView;
        }

        public MainViewModel MainView { get; }
    }
}
