using DataAccess;
using DataAccess.Models;
using DataAccess.Repositories;
using RTK_HMI.Infrastructure.Commands;
using RTK_HMI.Views.DialogWindows;
using System;
using System.Collections.Generic;
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
            Parameters = _parameterRepository.Init(ParameterDataFactory.Get());
        }

        #region Параметры
        private IEnumerable<Parameter> _parameters;
        public IEnumerable<Parameter> Parameters
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
