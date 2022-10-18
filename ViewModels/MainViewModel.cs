using DataAccess;
using DataAccess.Models;
using System.Collections.Generic;

namespace RTK_HMI.ViewModels
{
    internal class MainViewModel : PropertyChangedBase
    {
        #region Параметры
        private IEnumerable<Parameter> _parameters;
        public IEnumerable<Parameter> Parameters
        {
            get => _parameters;
            set => Set(ref _parameters, value);
        } 
        #endregion

        void InitParameters()
        {

        }
    }
}
