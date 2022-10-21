using DataAccess;
using DataAccess.Models;
using DataAccess.Repositories;
using RTK_HMI.Infrastructure.Commands;
using RTK_HMI.Services;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RTK_HMI.ViewModels
{
    internal class ConnectViewModel : PropertyChangedBase
    {
        public const int controlRegNum=64;

        private readonly IRepository<ConnectSettings> _connectRepository;
        public ConnectViewModel(MainViewModel mainView)
        {
            MainView = mainView;
            _connectRepository = new EfRepository<ConnectSettings>();
            InitParameters();
            InitConnectService();


        }

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
            SafetyAction(() =>
            {
                if (!RtkExchange.Connected) ExchangeService.Connect();
                else ExchangeService.Disconnect();

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
            Task.Run(() => ReadAllRegs());           
            
        }, canExecPar => true));
        #endregion

        #endregion

        void InitParameters()
        {
            ConnectSettings = _connectRepository.Init(new List<ConnectSettings> { new ConnectSettings() }).FirstOrDefault();
            ConnectSettings.PropertyChanged += (o, e) => _connectRepository.Update(ConnectSettings);
        }

        void InitConnectService()
        {
            ExchangeService = new ExchangeService(RtkExchange, ConnectSettings);
            ExchangeService.ErrorEvent += (s) => MessageBox.Show(s);

        }

        void ReadAllRegs()
        {
            SafetyAction(() =>
            {
                ReadFromEeprom();
                var addr = CalculateRegAdressService.GetStartAndCount(MainView.ParameterVm.Parameters);
                var buf = ExchangeService.ReadRegisters(addr.Item1, addr.Item2);
                if (buf is null) return;
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
                ExchangeService.Disconnect();
            }
        }


        void ReadFromEeprom()
        {
            ExchangeService.WriteRegisters(new ushort[] { 100 }, controlRegNum);
        }



    }
}
