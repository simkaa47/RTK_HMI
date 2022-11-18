﻿using DataAccess;
using DataAccess.Models;
using RTK_HMI.Infrastructure.Commands;
using RTK_HMI.Services;
using System;
using System.Collections.Generic;
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
            Vm.ParameterVm.Parameters = JsonSaveLoadService.LoadFromJson(SelectedFileInfo.Name);
        }


    }
}
