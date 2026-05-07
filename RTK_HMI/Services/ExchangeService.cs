using DataAccess.Models;
using FluentModbus;
using System;
using System.Linq;
using System.Net;
using System.Threading;

namespace RTK_HMI.Services
{
    public class ExchangeService
    {
        private const int ChunkSize = 100;

        private readonly ConnectData _connectData;
        private readonly ConnectSettings _connectSettings;

        private readonly ModbusRtuClient _rtuClient = new ModbusRtuClient();
        private readonly ModbusTcpClient _tcpClient = new ModbusTcpClient();
        private ModbusClient _client;

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private bool _rtuConnected;

        public event Action<string> ErrorEvent;

        public bool Connected => _connectSettings.Way == ConnectWays.SerialPort
            ? _rtuConnected
            : _tcpClient.IsConnected;

        public ExchangeService(ConnectData connectData, ConnectSettings connectSettings)
        {
            _connectData = connectData;
            _connectSettings = connectSettings;
            _tcpClient.ReadTimeout = 500;
            _tcpClient.WriteTimeout = 500;            
        }


       

        public void Connect()
        {
            switch (_connectSettings.Way)
            {
                case ConnectWays.SerialPort:
                    PortInit();
                    break;
                case ConnectWays.Tcp:
                    ConnectTcp();
                    break;
            }
            _connectData.Connected = Connected;
        }

        public void Disconnect()
        {
            if (_client is ModbusTcpClient tcpClient)
            {
                tcpClient.Disconnect();
            }
            else if (_client is ModbusRtuClient)
            {
                _rtuClient.Close();
                _rtuConnected = false;
            }
            _connectData.Connected = false;
        }

        public void Reconnect()
        {
            Disconnect();
            Connect();
        }

        public int[] ReadRegisters(int startNum, int count, Registers type)
        {
            int[] result = new int[count];
            for (int i = 0; i < count; i += ChunkSize)
            {
                int num = Math.Min(ChunkSize, count - i);
                int[] temp = type == Registers.Holding
                    ? ReadHoldingRegisters(startNum + i, num)
                    : ReadInputRegisters(startNum + i, num);
                temp?.CopyTo(result, i);
            }
            return result;
        }

        public void WriteRegisters(ushort[] source, int startNum)
        {
            if (_client is null) throw new Exception("Необходимо подключиться");
            _semaphore.Wait();
            try
            {
                _client.WriteMultipleRegisters(_connectSettings.ModbAddr, startNum, source);
                Thread.Sleep(100);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        #region Private connection helpers

        void PortInit()
        {
            _rtuClient.BaudRate = _connectSettings.Baudrate;
            _rtuClient.Parity = _connectSettings.Parity;
            _rtuClient.ReadTimeout = _connectSettings.ConnectionTimeout;
            _rtuClient.Connect(_connectSettings.ComName, ModbusEndianness.BigEndian);
            _client = _rtuClient;
            _rtuConnected = true;
        }

        void ConnectTcp()
        {
            var ip = GetStringFromIp(_connectSettings.Ip);
            _tcpClient.Connect(
                new IPEndPoint(IPAddress.Parse(ip), _connectSettings.PortNumber),
                ModbusEndianness.BigEndian);
            _client = _tcpClient;
        }

        static string GetStringFromIp(int ip)
        {
            byte[] addr = new byte[4];
            addr[0] = (byte)(ip & 255);
            addr[1] = (byte)((ip & (255 << 8)) >> 8);
            addr[2] = (byte)((ip & (255 << 16)) >> 16);
            addr[3] = (byte)((ip & (255 << 24)) >> 24);
            return $"{addr[3]}.{addr[2]}.{addr[1]}.{addr[0]}";
        }

        #endregion

        #region Private register read helpers

        int[] ReadHoldingRegisters(int startNum, int count)
        {
            if (_client is null) throw new Exception("Необходимо сначала подключиться");
            _semaphore.Wait();
            try
            {
                var span = _client.ReadHoldingRegisters<ushort>(
                    _connectSettings.ModbAddr, startNum, count);
                Thread.Sleep(100);
                return span.ToArray().Select(x => (int)x).ToArray();
                
            }
            finally
            {
                _semaphore.Release();
            }
        }

        int[] ReadInputRegisters(int startNum, int count)
        {
            if (_client is null) throw new Exception("Необходимо сначала подключиться");
            _semaphore.Wait();
            try
            {
                var span = _client.ReadInputRegisters<ushort>(
                    _connectSettings.ModbAddr, startNum, count);
                Thread.Sleep(100);
                return span.ToArray().Select(x => (int)x).ToArray();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        #endregion
    }
}
