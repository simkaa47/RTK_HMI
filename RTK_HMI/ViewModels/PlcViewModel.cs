using DataAccess;
using DataAccess.Models.Plc;
using DataAccess.Models.Plc.Parameter;
using Microsoft.Win32;
using RTK_HMI.Infrastructure.Commands;
using RTK_HMI.Services;
using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        private System.Timers.Timer _recordingTimer;
        private System.Timers.Timer _uiTimer;
        private System.DateTime _loggingEndTime;

        #region Статус логирования
        /// <summary>
        /// Статус логирования
        /// </summary>
        private bool _isLogging = false;
        /// <summary>
        /// Статус логирования
        /// </summary>
        public bool IsLogging
        {
            get => _isLogging;
            set => Set(ref _isLogging, value);
        }
        #endregion
        #region Частота записи
        /// <summary>
        /// Частота записи
        /// </summary>
        private int _timeInterval = 1000;
        /// <summary>
        /// Частота записи
        /// </summary>
        public int TimeInterval
        {
            get => _timeInterval;
            set => Set(ref _timeInterval, value);
        }
        #endregion

        #region Время логирования
        // Время логирования в минутах
        private int _loggingDurationMinutes = 3;
        public int LoggingDurationMinutes
        {
            get => _loggingDurationMinutes;
            set
            {
                _loggingDurationMinutes = value;
                OnPropertyChanged(nameof(LoggingDurationMinutes));
            }
        }

        // Оставшееся время в удобном для UI виде, например "02:57"
        private string _loggingRemainingText = "00:00";
        public string LoggingRemainingText
        {
            get => _loggingRemainingText;
            set
            {
                _loggingRemainingText = value;
                OnPropertyChanged(nameof(LoggingRemainingText));
            }
        }

        #endregion

        #region Путь к файлу
        /// <summary>
        /// Путь к файлу
        /// </summary>
        private string _selectedFilePath;
        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string SelectedFilePath
        {
            get => _selectedFilePath;
            set => Set(ref _selectedFilePath, value);
        }
        #endregion




        public CancellationTokenSource _cts = new ();


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
                _cts = new();
                await InitPlc();
                await _plcService.Connect().ConfigureAwait(false);
                if (_plcService.isPlcConnected) IsPlcConnected = true; 
                await UpdatePlc(_cts.Token);

            });
        }, canExecPar => true));
        #endregion

        #region Отключение 
        /// <summary>
        /// Отключение 
        /// </summary>
        RelayCommand _disconnectPlcCommand;
        /// <summary>
        /// Отключение
        /// </summary>
        public RelayCommand DisconnectPlcCommand => _disconnectPlcCommand ?? (_disconnectPlcCommand = new RelayCommand(execPar =>
        {
            SafetyAction(async () =>
            {
                if (_cts is not null)
                {
                    _cts.Cancel();
                    _cts.Dispose();

                    await _plcService.Disconnect();
                    IsPlcConnected = false;
                }


            });
        }, canExecPar => true));
        #endregion

        #region Запись 
        /// <summary>
        /// Запись
        /// </summary>
        RelayCommand _writePlcCommand;
        /// <summary>
        /// Запись
        /// </summary>
        public RelayCommand WritePlcCommand => _writePlcCommand ?? (_writePlcCommand = new RelayCommand(execPar =>
        {
            if (_plcService.isPlcConnected)
            {
                if (execPar is PlcParameter<bool> boolParameter)
                {
                    _plcService.WriteParameter(boolParameter.WriteValue, boolParameter.DbNum, boolParameter.DbReg, boolParameter.DbOffset);
                }
                if (execPar is PlcParameter<int> intParameter)
                {
                    _plcService.WriteParameter(intParameter.WriteValue, intParameter.DbNum, intParameter.DbReg, intParameter.DbOffset);
                }
                if (execPar is PlcParameter<short> shortParameter)
                {
                    _plcService.WriteParameter(shortParameter.WriteValue, shortParameter.DbNum, shortParameter.DbReg, shortParameter.DbOffset);
                }
                if (execPar is PlcParameter<float> floatParameter)
                {
                    _plcService.WriteParameter(floatParameter.WriteValue, floatParameter.DbNum, floatParameter.DbReg, floatParameter.DbOffset);
                }

            }
        }, canExecPar => true));



        RelayCommand _writePlcParameterCommand;
        /// <summary>
        /// Подключение
        /// </summary>
        public RelayCommand WritePlcParameterCommand => _writePlcParameterCommand ?? (_writePlcParameterCommand = new RelayCommand(execPar =>
        {
            if (_plcService.isPlcConnected)
            {
                if (execPar is not null)
                {
                    switch (execPar)
                    {
                        case PlcParameter<float> par:
                            par.Value = par.WriteValue;
                            SafetyAction(async () =>
                            {
                                _plcService.WriteParameter(par.Value, par.DbNum, par.DbReg, par.DbOffset);
                            });
                            break;

                            case PlcParameter<int> par:
                            par.Value = par.WriteValue;
                            SafetyAction(async () =>
                            {
                                _plcService.WriteParameter(par.Value, par.DbNum, par.DbReg, par.DbOffset);
                            });
                            break;
                        case PlcParameter<short> par:
                            par.Value = par.WriteValue;
                            SafetyAction(async () =>
                            {
                                _plcService.WriteParameter(par.Value, par.DbNum, par.DbReg, par.DbOffset);
                            });
                            break;
                        case PlcParameter<bool> par:
                            par.Value = par.WriteValue;
                            SafetyAction(async () =>
                            {
                                _plcService.WriteParameter(par.Value, par.DbNum, par.DbReg, par.DbOffset);
                            });
                            break;

                    }
                 
                    

                }
                

            }

        }, canExecPar => true));

 
        /// <summary>
        /// Запуск установки
        /// </summary>
        RelayCommand _runCommand;
        /// <summary>
        /// Запуск установки
        /// </summary>
        public RelayCommand RunCommand => _runCommand ?? (_runCommand = new RelayCommand(execPar =>
        {
            if (_plcService.isPlcConnected)
            {
                plcDevice.Run.WriteValue = !plcDevice.Run.Value;
                _plcService.WriteParameter(plcDevice.Run.WriteValue, plcDevice.Run.DbNum, plcDevice.Run.DbReg, plcDevice.Run.DbOffset);

            }
        }, canExecPar => true));

        /// <summary>
        /// Сброс ошибок
        /// </summary>
        RelayCommand _rstCommand;
        /// <summary>
        /// Сброс ошибок
        /// </summary>
        public RelayCommand RstCommand => _rstCommand ?? (_rstCommand = new RelayCommand(execPar =>
        {
            if (_plcService.isPlcConnected)
            {
                plcDevice.RstErrors.WriteValue = !plcDevice.RstErrors.Value;
                _plcService.WriteParameter(plcDevice.RstErrors.WriteValue, plcDevice.RstErrors.DbNum, plcDevice.RstErrors.DbReg, plcDevice.RstErrors.DbOffset);

            }
        }, canExecPar => true));

        /// <summary>
        /// Смена режима управления
        /// </summary>
        RelayCommand _changeModeCommand;
        /// <summary>
        /// Смена режима управления
        /// </summary>
        public RelayCommand ChangeModeCommand => _changeModeCommand ?? (_changeModeCommand = new RelayCommand(execPar =>
        {
            if (_plcService.isPlcConnected)
            {

                //_plcService.WriteParameter(plcDevice.Mode.WriteValue, plcDevice.RstErrors.DbNum, plcDevice.RstErrors.DbReg, plcDevice.RstErrors.DbOffset);
                _plcService.WriteVariable("DB9.DBW0", plcDevice.Mode.WriteValue);

            }
        }, canExecPar => true));

        #endregion

        #region Логирование

        #region Запустить логирование
        private RelayCommand _startLoggingCommand;
        public RelayCommand StartLoggingCommand => _startLoggingCommand ?? (_startLoggingCommand = new RelayCommand(async par =>
        {
            if (IsLogging) return;

            if (LoggingDurationMinutes <= 0)
            {
                MessageBox.Show("Укажите время логирования больше 0 минут.");
                return;
            }

            try
            {
                var header = "Дата;Уставка скорости, м/с;Влажность;Температура, °C;Абс. давление, Па;" +
                             "Дифф. давление, Па;Плотность, кг/м3;Скорость, м/с\n";
                await File.AppendAllTextAsync(SelectedFilePath, header, Encoding.UTF8);

                // --- фиксируем момент окончания ---
                _loggingEndTime = System.DateTime.Now.AddMinutes(LoggingDurationMinutes);
                LoggingRemainingText = TimeSpan.FromMinutes(LoggingDurationMinutes).ToString(@"mm\:ss");

                IsLogging = true;

                // таймер записи данных (ваш существующий TimeInterval)
                _recordingTimer = new System.Timers.Timer(TimeInterval);
                _recordingTimer.Elapsed += async (s, e) => await RecordData();
                _recordingTimer.Start();

                // таймер обновления UI (раз в секунду)
                _uiTimer = new System.Timers.Timer(1000);
                _uiTimer.Elapsed += (s, e) => UpdateLoggingCountdown();
                _uiTimer.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsLogging = false;
            }
        }, canExec => SelectedFilePath is not null && !IsLogging));
        #endregion

        #region Остановить логирование
        private RelayCommand _stopLoggingCommand;
        public RelayCommand StopLoggingCommand => _stopLoggingCommand ?? (_stopLoggingCommand = new RelayCommand(par =>
        {
            StopLoggingInternal();
        }, canExec => IsLogging));

        private void StopLoggingInternal()
        {
            if (!IsLogging) return;

            IsLogging = false;

            _recordingTimer?.Stop();
            _recordingTimer?.Dispose();
            _recordingTimer = null;

            _uiTimer?.Stop();
            _uiTimer?.Dispose();
            _uiTimer = null;

            LoggingRemainingText = "00:00";
        }


        #endregion

        #region Обновление счётчика времени

        private void UpdateLoggingCountdown()
        {
            if (!IsLogging) return;

            var remaining = _loggingEndTime - System.DateTime.Now;

            if (remaining <= TimeSpan.Zero)
            {
                // Время вышло — останавливаемся (на UI-потоке)
                App.Current.Dispatcher.Invoke(() =>
                {
                    StopLoggingInternal();
                });
                return;
            }

            var totalSeconds = (LoggingDurationMinutes * 60.0);
            var progress = 1.0 - remaining.TotalSeconds / totalSeconds;

            App.Current.Dispatcher.Invoke(() =>
            {
                LoggingRemainingText = remaining.ToString(@"mm\:ss");
            });
        }

        #endregion

        #region Выбрать файл
        private RelayCommand _chooseFileCommand;
        public RelayCommand ChooseFileCommand => _chooseFileCommand ?? (_chooseFileCommand = new RelayCommand(par =>
        {
            var dialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "CSV файлы (*.csv)|*.csv|Excel файлы (*.xlsx)|*.xlsx",
                CheckFileExists = false, // Разрешаем создание нового файла
                AddExtension = true,
                FileName = $"Measures log {System.DateTime.Now:d} {System.DateTime.Now:HH_mm}",

            };

            if (dialog.ShowDialog() == true)
            {
                SelectedFilePath = dialog.FileName;
            }
        }, canExec => !IsLogging));
        #endregion


        #endregion

        #region Record Data

        private async Task RecordData()
        {
            if (!IsLogging) return;

            try
            {

                await File.AppendAllTextAsync(SelectedFilePath, $"{System.DateTime.Now};", Encoding.UTF8);
                var data = $"{plcDevice.PlcParameters.Speed.Value};{plcDevice.HummSensorView.Value};{plcDevice.TempSensorView.Value};" +
                    $"{plcDevice.AbsPressureView.Value};{plcDevice.DiffPressureView.Value};{plcDevice.FlowView.Density};{plcDevice.FlowView.Velocity}";
                
                data += "\n";
                await File.AppendAllTextAsync(SelectedFilePath, data.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка записи: {ex.Message}");

            }


        }

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
