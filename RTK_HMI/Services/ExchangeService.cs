using DataAccess.Models;
using EasyModbus;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RTK_HMI.Services
{
    public class ExchangeService
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

        public void Connect()
        {
            switch (_connectSettings.Way)
            {
                case ConnectWays.SerialPort:
                    PortInit();
                    break;
                case ConnectWays.Udp:
                    ConnectUdp();
                    break;
                case ConnectWays.Tcp:
                    break;
                default:
                    break;
            }
            _connectData.Connected = _client.Connected;
        }

        public   void Disconnect()
        {
            if(_client!=null)
            {
                _client.Disconnect();
                _connectData.Connected = false;
            }
            
        }

        public void Reconnect() 
        {
            Disconnect();
            Connect();
        }

        public int[] ReadRegisters(int startNum, int count, Registers type)
        {
            int[] result = new int[count];
            for (int i = 0; i < count; i+=100)
            {
                var num = Math.Min(100, count - i);
                int[] temp = null;
                if(type == Registers.Holding)
                {
                    temp = ReadHoldingRegisters(startNum + i, num);
                }
                else
                {
                    temp = ReadInputRegisters(startNum + i, num);
                }
                if(temp!=null)
                {
                    temp.CopyTo(result, i);
                }
                
            }            
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
        }
        #endregion

        #region Покдлючение UDP
        void ConnectUdp()
        {
            _client = new ModbusClient();
            _client.UDPFlag = true;
            _client.IPAddress = GetStringFromIp(_connectSettings.Ip);
            _client.Port = _connectSettings.PortNumber;
            _client.Connect();
        }
        #endregion

        static string GetStringFromIp(int ip)
        {
            byte[] addr = new byte[4];
            addr[0] = (byte)(ip & 255);
            addr[1] = (byte)((ip & (255 << 8)) >> 8);
            addr[2] = (byte)((ip & (255 << 16)) >> 16);
            addr[3] = (byte)((ip & (255 << 24)) >> 24);
            return $"{addr[3]}.{addr[2]}.{addr[1]}.{addr[0]}";
        }


        int[] ReadHoldingRegisters(int startNum, int count)
        {
            int[] registers = null;
            if (_client is null || !_client.Connected) throw new Exception("Необходимо сначала подключиться");
            registers = _client.ReadHoldingRegisters(startNum, count);
            return registers;
        }

        int[] ReadInputRegisters(int startNum, int count)
        {
            int[] registers = null;
            if (_client is null || !_client.Connected) throw new Exception("Необходимо сначала подключиться");
            registers = _client.ReadInputRegisters(startNum, count);
            return registers;
        }


    }
}
