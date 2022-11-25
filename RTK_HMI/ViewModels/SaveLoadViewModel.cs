using DataAccess;
using DataAccess.Models;
using RTK_HMI.Infrastructure.Commands;
using RTK_HMI.Services;
using RTK_HMI.Views.DialogWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace RTK_HMI.ViewModels
{
    internal class SaveLoadViewModel : PropertyChangedBase
    {
        public SaveLoadViewModel(MainViewModel vm)
        {
            Vm = vm;
            Init();
        }

        public MainViewModel Vm { get; }

        #region КОманды

        #region Сохранить набор
        /// <summary>
        /// Сохранить набор
        /// </summary>
        RelayCommand _saveCommand;
        /// <summary>
        /// Сохранить набор
        /// </summary>
        public RelayCommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(execPar => { SafetyAction(Save); }, canExecPar => true));
        #endregion

        #region Редактировать запись
        /// <summary>
        /// Редактировать запись
        /// </summary>
        RelayCommand _editCommand;
        /// <summary>
        /// Редактировать запись
        /// </summary>
        public RelayCommand EditCommand => _editCommand ?? (_editCommand = new RelayCommand(execPar => 
        {
            if (Vm.ConnectVM.SelectedParameter is null) return;
            SafetyAction(() =>
            {
                ChangeParameterWindow dialog = new ChangeParameterWindow(Vm.ConnectVM.SelectedParameter);
                if (dialog.ShowDialog() == true)
                {
                   
                }
            });
        }, canExecPar => true));
        #endregion

        #region Удалить запись
        /// <summary>
        /// Удалить запись
        /// </summary>
        RelayCommand _deleteCommand;
        /// <summary>
        /// Удалить запись
        /// </summary>
        public RelayCommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new RelayCommand(execPar => 
        {
            if (Vm.ConnectVM.SelectedParameter is null) return;
            var parameter = Vm.ConnectVM.SelectedParameter;
            Vm.ParameterVm.Parameters.Remove(parameter);           
        }, canExecPar => true));
        #endregion

        #region Добавить запись
        /// <summary>
        /// Добавить запись
        /// </summary>
        RelayCommand _addCommand;
        /// <summary>
        /// Добавить запись
        /// </summary>
        public RelayCommand AddCommand => _addCommand ?? (_addCommand = new RelayCommand(execPar => 
        {
            var parameter = new Parameter
            {
                Description = "Параметр",
                RegNum = 100,
                Type = DataType.Float32
            };
            ChangeParameterWindow dialog = new ChangeParameterWindow(parameter);
            if (dialog.ShowDialog() == true)
            {
                Vm.ParameterVm.Parameters.Add(parameter);
            }
        }, canExecPar => true));
        #endregion





        #endregion

        void SafetyAction(Action action)
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

        #region Имя текущего набора данных
        /// <summary>
        /// Имя текущего набора данных
        /// </summary>
        private string _curName;
        /// <summary>
        /// Имя текущего набора данных
        /// </summary>
        public string CurName
        {
            get => _curName;
            set => Set(ref _curName, value);
        }
        #endregion

        #region Коллекция наборов
        /// <summary>
        /// Коллекция наборов
        /// </summary>
        private List<JsonData> _jsonDataCollection;
        /// <summary>
        /// Коллекция наборов
        /// </summary>
        public List<JsonData> JsonDataCollection
        {
            get => _jsonDataCollection;
            set => Set(ref _jsonDataCollection, value);
        }
        #endregion

        #region Выбранный набор
        /// <summary>
        /// Выбранный набор
        /// </summary>
        private JsonData _selectedFileInfo;
        /// <summary>
        /// Выбранный набор
        /// </summary>
        public JsonData SelectedFileInfo
        {
            get => _selectedFileInfo;
            set
            {
                if(Set(ref _selectedFileInfo, value))
                {
                    SafetyAction(Load);
                }
            } 
        }
        #endregion


        void Init()
        { 
            SafetyAction(Refresh);
        }

        void Refresh()
        {
            JsonDataCollection = JsonSaveLoadService.GetFilesInfo();
        }


        void Save()
        {
            JsonSaveLoadService.Save(Vm.ParameterVm.Parameters, SelectedFileInfo);
            Refresh();
        }

        void Load()
        {
            if (SelectedFileInfo is null) return;
            Vm.ParameterVm.Parameters = new ObservableCollection<Parameter>(JsonSaveLoadService.LoadFromJson(SelectedFileInfo.Name));
        }


    }
}
