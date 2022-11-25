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
    internal class ConnectViewModel : PropertyChangedBase
    {

        private readonly IRepository<ConnectSettings> _connectRepository;
        public ConnectViewModel(MainViewModel mainView)
        {
            MainView = mainView;
            _connectRepository = new EfRepository<ConnectSettings>();
            InitParameters();
            InitConnectService();
            Task.Factory.StartNew(() => CuclicProcess());
        }

        private Queue<Request> _requests = new Queue<Request>();    



        public MainViewModel MainView { get; }

        #region Данные обмена с RTK
        public ConnectData RtkExchange { get; } = new ConnectData();
        #endregion

        #region Сервисы

        public ExchangeService ExchangeService { get; private set; }

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

        #region Connect
        private RelayCommand _connectCommand;
        public RelayCommand ConnectCommand => _connectCommand ?? (_connectCommand = new RelayCommand(par =>
        {
            Task.Run(() =>
            {
                Connect();
            });

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
                    if (SelectedParameter is null) return;
                    var bytes = RecognizeParameterFromArrService.GetRegisters(SelectedParameter);
                    ExchangeService.WriteRegisters(bytes, SelectedParameter.RegNum);
                    WriteToEeprom();
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
            ConnectSettings.PropertyChanged += (o, e) => _connectRepository.Update(ConnectSettings);
        }

        void InitConnectService()
        {
            ExchangeService = new ExchangeService(RtkExchange, ConnectSettings);


        }

        void ReadAllRegs(IEnumerable<Parameter> parameters)
        {
            SafetyAction(() =>
            {
                if (!RtkExchange.Connected) 
                {
                    throw new Exception("Необходимо подключиться");                    
                }
                
                ReadFromEeprom();
                var addr = CalculateRegAdressService.GetStartAndCount(parameters);
                var buf = ExchangeService.ReadRegisters(addr.Item1, addr.Item2);
                if (buf is null) return;

                //string text;
                //int temp = 0;
                //using (StreamReader reader = new StreamReader("arr.txt"))
                //{
                //    text = reader.ReadToEnd();
                //    Console.WriteLine(text);
                //}
                //var buf = text.Split(new char[] { ' ', '\n', '\t', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                //.Where(s => int.TryParse(s, out temp))
                //.Select(s => temp)
                //.ToArray();
                //RecognizeParameterFromArrService.Recongnize(MainView.ParameterVm.Parameters, buf, 65);
                RecognizeParameterFromArrService.Recongnize(parameters, buf, addr.Item1);
            });

        }

        void WriteAllRegs()
        {
            SafetyAction(() =>
            {
                if (!RtkExchange.Connected)
                {
                    throw new Exception("Необходимо подключиться");
                }
                var addr = CalculateRegAdressService.GetStartAndCount(MainView.ParameterVm.Parameters);
                var arr = new ushort[addr.Item2];
                foreach (var par in MainView.ParameterVm.Parameters)
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
            });
        }

        void SafetyAction(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if(RtkExchange.Connected)ExchangeService.Disconnect();
            }

        }


        void ReadFromEeprom()
        {
            Thread.Sleep(500);
            ExchangeService.WriteRegisters(new ushort[] { 100 }, ConnectSettings.StartReg);
        }

        void WriteToEeprom()
        {
            Thread.Sleep(500);
            ExchangeService.WriteRegisters(new ushort[] { 200 }, ConnectSettings.StartReg);
        }

        void Connect()
        {
            SafetyAction(() =>
            {

                if (!RtkExchange.Connected)
                {
                    if (!ComPorts.Contains(ConnectSettings.ComName)) throw new Exception("Выберите COM порт");
                    ExchangeService.Connect();
                }
                else ExchangeService.Disconnect();

            });
        }

        void CuclicProcess()
        {
            while (true)
            {
                if(RtkExchange.Connected)
                {
                    var cyclicParameters = MainView.ParameterVm.Parameters.Where(p => p.IsCyclic).ToList();
                    if (cyclicParameters.Count > 0)
                    {
                        ReadAllRegs(cyclicParameters);
                    }
                    //while (_requests.Count > 0)
                    //{
                    //    var request = _requests.Dequeue();
                    //    request.Action?.Invoke();
                    //}
                }
                else Thread.Sleep(500);
            }
            
        }





    }
}
