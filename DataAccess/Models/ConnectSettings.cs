using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace DataAccess.Models
{
    public class ConnectSettings :PropertyChangedBase, IDatabased
    {
        public int Id { get ; set ; }

        #region Имя порта
        private string _comName="COM1";
        public string ComName
        {
            get => _comName;
            set => Set(ref _comName, value);
        }
        #endregion

        #region Паритет
        private Parity _parity;
        public Parity Parity { get => _parity; set => Set(ref _parity, value); }
        #endregion

        #region Бадрейт
        private int _baudrate=9600;
        public int Baudrate { get => _baudrate; set => Set(ref _baudrate, value); }
        #endregion

        #region Адрес в сети модбас
        private byte _modbAddr=1;
        public byte ModbAddr { get => _modbAddr; set => Set(ref _modbAddr, value); }
        #endregion

        #region Тайм-аут связи
        private int _connTimeout=1000;
        public int ConnectionTimeout { get => _connTimeout; set => Set(ref _connTimeout, value); }
        #endregion

        #region Стартовый регистр
        /// <summary>
        /// Стартовый регистр
        /// </summary>
        private int _startReg = 64;
        /// <summary>
        /// Стартовый регистр
        /// </summary>
        public int StartReg
        {
            get => _startReg;
            set => Set(ref _startReg, value);
        }
        #endregion


    }
}
