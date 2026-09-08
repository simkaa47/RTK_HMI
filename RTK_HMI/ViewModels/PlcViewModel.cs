using DataAccess;
using DataAccess.Models.Plc;
using RTK_HMI.Infrastructure.Commands;
using RTK_HMI.Services;
using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace RTK_HMI.ViewModels
{
    public class PlcViewModel : PropertyChangedBase
    {
        private PlcService _plcService;

        public PlcDevice plcDevice { get; set; }

        public MainViewModel MainVm { get; set; }
        public PlcViewModel(MainViewModel mainView) 
        {
            MainVm = mainView;
            plcDevice = new PlcDevice();
            _plcService = new PlcService(plcDevice);
        }

        #region Статус ПЛК
        private bool _isPlcConnected = false;

        public bool IsPlcConnected
        {
            get => _isPlcConnected;
            set => Set(ref _isPlcConnected, value);
        }

        #endregion

        public CancellationTokenSource _cts = new ();
        //public CancellationToken token = _cts.Token;

        public async Task InitPlc()
        {
            _plcService = new PlcService(plcDevice);

            await _plcService.Initialize();

        }

        #region Получение данных ПЛК

        public async Task UpdatePlc(CancellationToken cts) 
        {
            try
            {
                await _plcService.GetPlcDataAsync(cts).ConfigureAwait(false);
            }
            catch (Exception ex) 
            {
                Debug.Write(ex);
                
            }
        }

        #endregion

        #region Команды

        #region Подключение 
        /// <summary>
        /// Подключение 
        /// </summary>
        RelayCommand _connectPlcCommand;
        /// <summary>
        /// Подключение
        /// </summary>
        public RelayCommand ConnectPlcCommand => _connectPlcCommand ?? (_connectPlcCommand = new RelayCommand(execPar =>
        {
            SafetyAction(async () =>
            {
                await InitPlc();
                await _plcService.Connect().ConfigureAwait(false);
                if (_plcService.isPlcConnected) IsPlcConnected = true; 
                await UpdatePlc(_cts.Token);

            });
        }, canExecPar => true));
        #endregion

        #region Подключение 
        /// <summary>
        /// Подключение 
        /// </summary>
        RelayCommand _disconnectPlcCommand;
        /// <summary>
        /// Подключение
        /// </summary>
        public RelayCommand DisconnectPlcCommand => _disconnectPlcCommand ?? (_disconnectPlcCommand = new RelayCommand(execPar =>
        {
            SafetyAction(async () =>
            {
                _cts.Cancel();
                await _plcService.Disconnect();
                IsPlcConnected = false;

            });
        }, canExecPar => true));
        #endregion

        #region Запись 
        /// <summary>
        /// Подключение 
        /// </summary>
        RelayCommand _writePlcCommand;
        /// <summary>
        /// Подключение
        /// </summary>
        public RelayCommand WritePlcCommand => _writePlcCommand ?? (_writePlcCommand = new RelayCommand(execPar =>
        {
            if (_plcService.isPlcConnected)
            {
                
            }
            SafetyAction(async () =>
            {
                


            });
        }, canExecPar => true));
        #endregion


        #endregion

        async void SafetyAction(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
