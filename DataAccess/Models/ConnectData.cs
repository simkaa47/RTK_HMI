using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public  class ConnectData:PropertyChangedBase
    {
        #region Статус соединения
        private bool _connected;
        public bool Connected
        {
            get => _connected;
            set => Set(ref _connected, value);
        } 
        #endregion

        public ushort[] HoldingRegisters { get; set; }= new ushort[200];
    }
}
