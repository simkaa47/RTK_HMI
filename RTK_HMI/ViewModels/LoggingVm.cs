using DataAccess;
using DataAccess.Models;
using Microsoft.Win32;
using RTK_HMI.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace RTK_HMI.ViewModels
{
    public class LoggingVm : PropertyChangedBase
    {
        public LoggingVm(MainViewModel mainVm)
        {
            MainVm = mainVm;
            Selected = MainVm.ParameterVm.Parameters.Where(p => p.LogStatus == true).ToList();
        }

        public MainViewModel MainVm { get; }

        #region Поля

        private System.Timers.Timer _recordingTimer;



        #endregion

        #region Свойства



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
        private int _timeInterval = 10000;
        /// <summary>
        /// Частота записи
        /// </summary>
        public int TimeInterval
        {
            get => _timeInterval;
            set => Set(ref _timeInterval, value);
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

        #region Выбранные параметры для лога
        /// <summary>
        /// Список выбранных параметров
        /// </summary>
        private List<Parameter> _selected;
        /// <summary>
        /// Список выбранных параметров
        /// </summary>
        public List<Parameter> Selected
        {
            get => _selected;
            set => Set(ref _selected, value);
        }
        #endregion

        #endregion

        #region Команды


        #region Запустить логирование
        private RelayCommand _startLoggingCommand;
        public RelayCommand StartLoggingCommand => _startLoggingCommand ?? (_startLoggingCommand = new RelayCommand(async par =>
        {
            IsLogging = !IsLogging;
            if (IsLogging)
            {
                Selected = MainVm.ParameterVm.Parameters.Where(p => p.LogStatus == true).ToList();
                try
                {
                    
                    var header = $"Дата;";

                    foreach (var head in Selected)
                    {
                        header += $"{head.Description};";
                    }
                    header += "\n";
                    await File.AppendAllTextAsync(SelectedFilePath,
                        header, Encoding.UTF8);
                    _recordingTimer = new System.Timers.Timer(TimeInterval); // Интервал в мс
                    _recordingTimer.Elapsed += async (s, e) => await RecordData();
                    _recordingTimer.Start();
                }

                catch(Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    IsLogging = false;
                }
            }

        }, canExec => SelectedFilePath is not null));
        #endregion

        #region Остановить логирование
        private RelayCommand _stopLoggingCommand;
        public RelayCommand StopLoggingCommand => _stopLoggingCommand ?? (_stopLoggingCommand = new RelayCommand(par =>
        {
            IsLogging = !IsLogging;
        }, canExec => IsLogging));
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
                FileName = $"Measures log {DateTime.Now:d} {DateTime.Now:HH_mm}",
                
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
                
                await File.AppendAllTextAsync(SelectedFilePath, $"{DateTime.Now};", Encoding.UTF8);
                var data = "";
                foreach (var par in Selected)
                {
                    data += $"{par.Value};";
                }
                data += "\n";
                await File.AppendAllTextAsync(SelectedFilePath, data.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка записи: {ex.Message}");
                
            }


        }

        #endregion
        void SafetyAction(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
    }
}
