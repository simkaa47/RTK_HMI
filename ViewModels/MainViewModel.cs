using DataAccess;
using DataAccess.Models;
using DataAccess.Repositories;
using System.Collections.Generic;

namespace RTK_HMI.ViewModels
{
    internal class MainViewModel : PropertyChangedBase
    {

        public ParameterVm ParameterVm { get; set; }


        public MainViewModel()
        {
            ParameterVm = new ParameterVm(this);
        }

        
    }
}
