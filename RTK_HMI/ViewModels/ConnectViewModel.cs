using DataAccess;
using DataAccess.Models;
using DataAccess.Repositories;
using RTK_HMI.Infrastructure;
using RTK_HMI.Infrastructure.Commands;
using RTK_HMI.Services;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace RTK_HMI.ViewModels
{
    public class ConnectViewModel : PropertyChangedBase
    {

        private readonly IRepository<ConnectSettings> _connectRepository;
        public ConnectViewModel(MainViewModel mainView)
        {
            MainView = mainView;
            _connectRepository = new EfRepository<ConnectSettings>();
            InitParameters();
            InitConnectService();
            Task.Factory.StartNew(() => CyclicProcess());
        }

        private Queue<Request> _requests = new Queue<Request>();    



        public MainViewModel MainView { get; }

        private const string noConnect = "There is no connection, click the \"Establish/Terminate connection\" button";


        #region Communication Status
        /// <summary>
        /// Communication Status
        /// </summary>
        private string _status = noConnect;
        /// <summary>
        /// Communication Status        
        /// </summary>
        public string Status
        {
            get => _status;
            set => Set(ref _status, value);
        }
        #endregion

        #region Error
        /// <summary>
        /// Error
        /// </summary>
        private bool _error;
        /// <summary>
        /// Error
        /// </summary>
        public bool Error
        {
            get => _error;
            set => Set(ref _error, value);
        }
        #endregion

        #region Req attempts count
        /// <summary>
        /// Req attempts count
        /// </summary>
        private int _reqAtempts;
        /// <summary>
        /// Req attempts count
        /// </summary>
        public int ReqAtempts
        {
            get => _reqAtempts;
            set => Set(ref _reqAtempts, value);
        }
        #endregion

        #region Req success count
        /// <summary>
        /// Req success count
        /// </summary>
        private int _reqSuccess;
        /// <summary>
        /// Req success count
        /// </summary>
        public int ReqSuccess
        {
            get => _reqSuccess;
            set => Set(ref _reqSuccess, value);
        }
        #endregion

        #region Connect Required
        /// <summary>
        /// Connect Required
        /// </summary>
        private bool _connectReq;
        /// <summary>
        /// Connect Required
        /// </summary>
        public bool ConnectReq
        {
            get => _connectReq;
            set
            {
                if(Set(ref _connectReq, value))
                {
                    if(!ConnectReq) Status = noConnect;
                }
            }
        }
        #endregion

        #region Индикатор загрузки параметра
        /// <summary>
        /// Индикатор загрузки параметра
        /// </summary>
        private bool _loadIndicator;
        /// <summary>
        /// Индикатор загрузки параметра
        /// </summary>
        public bool LoadIndicator
        {
            get => _loadIndicator;
            set => Set(ref _loadIndicator, value);
        }
        #endregion

        #region Данные обмена с RTK
        public ConnectData RtkExchange { get; } = new ConnectData();
        #endregion

        #region Сервисы

        #region Communication Service
        public ExchangeService ExchangeService { get; private set; }
        #endregion        

        #endregion

        #region Настроки связи
        private ConnectSettings _connectSettings;
        public ConnectSettings ConnectSettings
        {
            get => _connectSettings;
            set => Set(ref _connectSettings, value);
        }
        #endregion

        #region Cписок доступных Com портов 
        string[] _comPorts;
        public string[] ComPorts { get => _comPorts ?? (_comPorts = SerialPort.GetPortNames()); set => Set(ref _comPorts, value); }
        #endregion

        #region Команды

        #region Обновить ком. порты
        private RelayCommand _updateComPortsCommand;
        public RelayCommand UpdateComPortsCommand => _updateComPortsCommand ?? (_updateComPortsCommand = new RelayCommand(par =>
        {
            ComPorts = SerialPort.GetPortNames();
        }, canExec => true));
        #endregion

        #region Update comm purpose
        /// <summary>
        /// Update comm purpose
        /// </summary>
        RelayCommand _updateCommMethodCommand;
        /// <summary>
        /// Update comm purpose
        /// </summary>
        public RelayCommand UpdateCommMethodCommand => _updateCommMethodCommand ?? (_updateCommMethodCommand = new RelayCommand(execPar => 
        { 

        }, canExecPar => true));
        #endregion

        #region ip0
        /// <summary>
        /// ip0
        /// </summary>
        private byte _ip0;
        /// <summary>
        /// ip0
        /// </summary>
        public byte Ip0
        {
            get => _ip0;
            set
            {
                if(Set(ref _ip0, value))
                {
                    RecalculateIpFromBytes();
                }
            }
        }
        #endregion
        #region ip1
        /// <summary>
        /// ip0
        /// </summary>
        private byte _ip1;
        /// <summary>
        /// ip0
        /// </summary>
        public byte Ip1
        {
            get => _ip1;
            set
            {
                if (Set(ref _ip1, value))
                {
                    RecalculateIpFromBytes();
                }
            }
        }
        #endregion
        #region ip2
        /// <summary>
        /// ip0
        /// </summary>
        private byte _ip2;
        /// <summary>
        /// ip0
        /// </summary>
        public byte Ip2
        {
            get => _ip2;
            set
            {
                if (Set(ref _ip2, value))
                {
                    RecalculateIpFromBytes();
                }
            }
        }
        #endregion
        #region ip3
        /// <summary>
        /// ip0
        /// </summary>
        private byte _ip3;
        /// <summary>
        /// ip0
        /// </summary>
        public byte Ip3
        {
            get => _ip3;
            set
            {
                if (Set(ref _ip3, value))
                {
                    RecalculateIpFromBytes();
                }
            }
        }
        #endregion

        #region Connect
        private RelayCommand _connectCommand;
        public RelayCommand ConnectCommand => _connectCommand ?? (_connectCommand = new RelayCommand(par =>
        {
            ConnectReq = !ConnectReq;
            ReqSuccess = 0;
            ReqAtempts= 0;

        }, canExec => true));
        #endregion

        #region Прочитать все параметры
        /// <summary>
        /// Прочитать все параметры
        /// </summary>
        RelayCommand _readAllParamsCommand;
        /// <summary>
        /// Прочитать все параметры
        /// </summary>
        public RelayCommand ReadAllParamsCommand => _readAllParamsCommand ?? (_readAllParamsCommand = new RelayCommand(execPar =>
        {
            _requests.Enqueue(new Request(()=>ReadAllRegs(MainView.ParameterVm.Parameters)));

        }, canExecPar => true));
        #endregion

        #region Записать все параметры
        /// <summary>
        /// Записать все параметры
        /// </summary>
        RelayCommand _writeAllCommand;
        /// <summary>
        /// Записать все параметры
        /// </summary>
        public RelayCommand WriteAllCommand => _writeAllCommand ?? (_writeAllCommand = new RelayCommand(execPar =>
        {
            _requests.Enqueue(new Request(WriteAllRegs));
        }, canExecPar => true));
        #endregion

        #region Записать один параметр
        /// <summary>
        /// Записать один параметр
        /// </summary>
        RelayCommand _writeParameterCommand;
        /// <summary>
        /// Записать один параметр
        /// </summary>
        public RelayCommand WriteParameterCommand => _writeParameterCommand ?? (_writeParameterCommand = new RelayCommand(execPar =>
        {
            _requests.Enqueue(new Request(() =>
            {
                SafetyAction(() =>
                {
                    LoadIndicator = true;
                    if (SelectedParameter is null) return;
                    var bytes = RecognizeParameterFromArrService.GetRegisters(SelectedParameter);
                    ExchangeService.WriteRegisters(bytes, SelectedParameter.RegNum);
                    WriteToEeprom();
                    LoadIndicator = false;
                });
            }));            

        }, canExecPar => true));
        #endregion

        #endregion

        #region Выбранный параметр
        /// <summary>
        /// Выбранный параметр
        /// </summary>
        private Parameter _selectedParameter;
        /// <summary>
        /// Выбранный параметр
        /// </summary>
        public Parameter SelectedParameter
        {
            get => _selectedParameter;
            set => Set(ref _selectedParameter, value);
        }
        #endregion

        void InitParameters()
        {
            ConnectSettings = _connectRepository.Init(new List<ConnectSettings> { new ConnectSettings() }).FirstOrDefault();
            ConnectSettings.PropertyChanged += (o, e) => 
            {
                _connectRepository.Update(ConnectSettings);
                if (RtkExchange.Connected) Reconnect();
            } ;
            GetBytesFromIp();

        }

        void InitConnectService()
        {

            ExchangeService = new ExchangeService(RtkExchange, ConnectSettings);

        }

        void Reconnect()
        {
            _requests.Enqueue(new Request(() => 
            {
                Status = "Reconnection is performed using the changed settings";
                Error = false;                
                SafetyAction(ExchangeService.Reconnect);
            }));
        }

        void ReadAllRegs(IEnumerable<Parameter> parameters)
        {
            SafetyAction(() =>
            {
                if (!RtkExchange.Connected) 
                {
                    throw new Exception("You need to connect");                    
                }

                ReqAtempts++;
                Status = $"Requesting data..({ReqSuccess}/{ReqAtempts})";
                Error = false;

                if(ConnectSettings.CommandFlag)LoadIndicator = true;                
                ReadFromEeprom();
                
                var holdings = parameters.Where(p => p.RegType == Registers.Holding);
                var readings = parameters.Where(p => p.RegType == Registers.Reading);
                // Holding Regs;
                
                var addr = CalculateRegAdressService.GetStartAndCount(holdings);
                var buf = ExchangeService.ReadRegisters(addr.Item1, addr.Item2, Registers.Holding);
                if (buf is null) return;
                RecognizeParameterFromArrService.Recongnize(holdings, buf, addr.Item1);

                // Reading Regs;
                addr = CalculateRegAdressService.GetStartAndCount(readings);
                buf = ExchangeService.ReadRegisters(addr.Item1, addr.Item2, Registers.Reading);
                if (buf is null) return;
                RecognizeParameterFromArrService.Recongnize(readings, buf, addr.Item1);
                ReqSuccess++;
                Status = $"The request was completed successfully...({ReqSuccess}/{ReqAtempts})";
                Error = false;

                LoadIndicator = false;
            });

        }

        void WriteAllRegs()
        {
            SafetyAction(() =>
            {
                if (!RtkExchange.Connected)
                {
                    throw new Exception("Need to connect");
                }
                ReqAtempts++;
                Status = $"Saving data..({ReqSuccess}/{ReqAtempts})";
                Error = false;
                LoadIndicator = true;
                var holdings = MainView.ParameterVm.Parameters.Where(p => p.RegType == Registers.Holding);
                if (holdings == null || holdings.Count() == 0) 
                {
                    ReqSuccess++;
                    Status = $"There are no parameters to save...({ReqSuccess}/{ReqAtempts})";
                    LoadIndicator = false;
                    Error = false;
                    return;
                }
                var addr = CalculateRegAdressService.GetStartAndCount(holdings);
                var arr = new ushort[addr.Item2];
                foreach (var par in holdings)
                {
                    var regs = RecognizeParameterFromArrService.GetRegisters(par);
                    regs.CopyTo(arr, par.RegNum - addr.Item1);
                }
                for (int i = 0; i < addr.Item2; i += 27)
                {
                    int count = Math.Min(27, addr.Item2 - i);
                    ExchangeService.WriteRegisters(arr.Skip(i).Take(count).ToArray(), i + addr.Item1);
                }

                WriteToEeprom();
                LoadIndicator = false;
                ReqSuccess++;
                Status = $"Data successfully saved ...({ReqSuccess}/{ReqAtempts})";
                Error = false;
            });
        }

        void RecalculateIpFromBytes()
        {
            ConnectSettings.Ip = Ip0 + (Ip1<<8) + (Ip2 << 16) + (Ip3 << 24);
        }

        void GetBytesFromIp()
        {
            _ip0 = (byte)(ConnectSettings.Ip & 255);
            _ip1 = (byte)((ConnectSettings.Ip & (255<<8))>>8);
            _ip2 = (byte)((ConnectSettings.Ip & (255 << 16))>>16);
            _ip3 = (byte)((ConnectSettings.Ip & (255 << 24))>>24);
        }

        void SafetyAction(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                LoadIndicator = false;
                Error = true;
                Status = ex.Message;
                ExchangeService.Disconnect();
            }

        }


        void ReadFromEeprom()
        {
            if(_connectSettings.CommandFlag)
            {
                Thread.Sleep(500);
                ExchangeService.WriteRegisters(new ushort[] { 100 }, ConnectSettings.StartReg);
            }
            
        }

        void WriteToEeprom()
        {
            if(_connectSettings.CommandFlag)
            {
                Thread.Sleep(500);
                ExchangeService.WriteRegisters(new ushort[] { 200 }, ConnectSettings.StartReg);
            }           
        }

        void Connect()
        {
            SafetyAction(() =>
            {                
                if (!RtkExchange.Connected)
                {
                    Status = "Connecting...";
                    if (!ComPorts.Contains(ConnectSettings.ComName) && ConnectSettings.Way == ConnectWays.SerialPort) 
                        throw new Exception("Select COM port");
                    ExchangeService.Connect();
                    Status = "Connected";
                } 
            });
        }

        void Disconnect()
        {
            SafetyAction(() =>
            {
                Status = "Disconnecting...";
                ExchangeService.Disconnect();
                Status = "Disconnected";
            });
        }

        void CyclicProcess()
        {
            while (true)
            {
                if (!RtkExchange.Connected && ConnectReq) Connect();
                if(RtkExchange.Connected)
                {
                    while (_requests.Count > 0)
                    {
                        var request = _requests.Dequeue();
                        request.Action?.Invoke();
                    }
                    var cyclicParameters = MainView.ParameterVm.Parameters.Where(p => p.IsCyclic).ToList();
                    if (cyclicParameters.Count > 0)
                    {
                        ReadAllRegs(cyclicParameters);
                    }
                    if (RtkExchange.Connected && !ConnectReq) Disconnect();

                }
                Thread.Sleep(500);
            }
            
        }





    }
}
