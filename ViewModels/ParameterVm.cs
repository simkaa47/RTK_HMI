using DataAccess;
using DataAccess.Models;
using DataAccess.Repositories;
using RTK_HMI.Infrastructure.Commands;
using RTK_HMI.Views.DialogWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace RTK_HMI.ViewModels
{
    internal class ParameterVm : PropertyChangedBase
    {
        public ParameterVm(MainViewModel mainVm)
        {
            _parameterRepository = new EfRepository<Parameter>();
            InitParameters();
            MainVm = mainVm;
        }

        void InitParameters()
        {
            Parameters = new ObservableCollection<Parameter>(_parameterRepository.Init(ParameterDataFactory.Get())); 
        }

        #region Параметры
        private ObservableCollection<Parameter> _parameters;
        public ObservableCollection<Parameter> Parameters
        {
            get => _parameters;
            set => Set(ref _parameters, value);
        }
        public MainViewModel MainVm { get; }
        #endregion

        private readonly IRepository<Parameter> _parameterRepository;


        #region Выбранный параметр
        private Parameter _selectedParameter;
        public Parameter SelectedParameter
        {
            get => _selectedParameter;
            set => Set(ref _selectedParameter, value);
        }
        #endregion

        #region Редактировать параметр
        private RelayCommand _editParameterCommand;
        public RelayCommand EditParameterCommand => _editParameterCommand ?? (_editParameterCommand = new RelayCommand(par => 
        {
            if (SelectedParameter is null) return;
            SafetyAction(() =>
            {
                ChangeParameterWindow dialog = new ChangeParameterWindow(SelectedParameter);
                if (dialog.ShowDialog() == true)
                {
                    _parameterRepository.Update(SelectedParameter);
                }
            });
        }, canExex => true));
        #endregion


        #region Добавить параметр
        /// <summary>
        /// Добавить параметр
        /// </summary>
        RelayCommand _addParameterCommand;
        /// <summary>
        /// Добавить параметр
        /// </summary>
        public RelayCommand AddParameterCommand => _addParameterCommand ?? (_addParameterCommand = new RelayCommand(execPar => 
        { 
            var newParam = new Parameter();
            SafetyAction(() =>
            {
                ChangeParameterWindow dialog = new ChangeParameterWindow(newParam);
                if (dialog.ShowDialog() == true)
                {
                    _parameterRepository.Add(newParam);
                }
            });

        }, canExecPar => true));
        #endregion

        #region Удалить параметр
        /// <summary>
        /// Удалить параметр
        /// </summary>
        RelayCommand _deleteCommand;
        /// <summary>
        /// Удалить параметр
        /// </summary>
        public RelayCommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new RelayCommand(execPar => 
        {
            if (SelectedParameter is null) return;
            SafetyAction(() => 
            {
                _parameterRepository.Delete(SelectedParameter);
            });
        
        }, canExecPar => true));
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
    }
}
