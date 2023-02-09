using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace DataAccess.Models
{
    public enum ConnectWays
    {
        SerialPort,
        Udp,
        Tcp
    }

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

        #region Communication Way
        /// <summary>
        /// Communication Way
        /// </summary>
        private ConnectWays _way;
        /// <summary>
        /// Communication Way
        /// </summary>
        public ConnectWays Way
        {
            get => _way;
            set => Set(ref _way, value);
        }
        #endregion

        #region Port Number
        /// <summary>
        /// Port Number
        /// </summary>
        private int _portNumber;
        /// <summary>
        /// Port Number
        /// </summary>
        public int PortNumber
        {
            get => _portNumber;
            set => Set(ref _portNumber, value);
        }
        #endregion

        #region IP
        /// <summary>
        /// IP
        /// </summary>
        private int _ip;
        /// <summary>
        /// IP
        /// </summary>
        public int Ip
        {
            get => _ip;
            set => Set(ref _ip, value);
        }
        #endregion

        #region There are command for read-write
        /// <summary>
        /// There are command for read-write
        /// </summary>
        private bool _commandFlag;
        /// <summary>
        /// There are command for read-write
        /// </summary>
        public bool CommandFlag
        {
            get => _commandFlag;
            set => Set(ref _commandFlag, value);
        }
        #endregion


    }
}
