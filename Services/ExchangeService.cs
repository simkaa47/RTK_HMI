using DataAccess.Models;
using EasyModbus;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RTK_HMI.Services
{
    internal class ExchangeService
    {
        private readonly ConnectData _connectData;
        private readonly ConnectSettings _connectSettings;
        private ModbusClient _client;

        public event Action<string> ErrorEvent;

        public ExchangeService(ConnectData connectData, ConnectSettings connectSettings)
        {
            _connectData = connectData;
            _connectSettings = connectSettings;
        }

        public async void Connect()
        {
            await Task.Run(() => PortInit());
        }

        public   void Disconnect()
        {
            if(_client!=null)
            {
                _client.Disconnect();
                _connectData.Connected = _client.Connected;
            }
            
        }

        public int[] ReadRegisters(int startNum, int count)
        {
            var result =  ReadHoldingRegisters(startNum, count);
            return result;
        }

        public void WriteRegisters(ushort[] source, int startNum)
        {
            if (_client is null || !_client.Connected) throw new Exception("Необходимо подлкючиться");
            _client.WriteMultipleRegisters(startNum, source.Select(s=>(int)s).ToArray());
        }

        #region Инициализаця порта
        void PortInit()
        {

            _client = new ModbusClient(_connectSettings.ComName);
            _client.ConnectionTimeout = _connectSettings.ConnectionTimeout;
            _client.Baudrate = _connectSettings.Baudrate; ;
            _client.UnitIdentifier = _connectSettings.ModbAddr;
            _client.Parity = _connectSettings.Parity;
            _client.Connect();
            _connectData.Connected = _client.Connected; 


        }
        #endregion
        

        

        int[] ReadHoldingRegisters(int startNum, int count)
        {
            int[] registers = null;
            try
            {
                
                if (_client is null || !_client.Connected) throw new Exception("Необходимо сначала подключиться");
                registers =  _client.ReadHoldingRegisters(startNum, count);
            }
            catch (Exception ex)
            {

                ErrorEvent.Invoke(ex.Message);
                _client?.Disconnect();
                if(!(_client is null))_connectData.Connected = _client.Connected;
            }
            return registers;
        }


    }
}
