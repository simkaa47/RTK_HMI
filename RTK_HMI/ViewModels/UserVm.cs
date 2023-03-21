using DataAccess;
using DataAccess.Models;
using DataAccess.Repositories;
using System.Windows;
using System;
using System.Collections.ObjectModel;
using RTK_HMI.Infrastructure.Commands;
using RTK_HMI.Views.DialogWindows;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;

namespace RTK_HMI.ViewModels
{
    public class UserVm:PropertyChangedBase
    {
        private readonly IRepository<User> _userRepository;
        internal UserVm(MainViewModel mainView)
        {
            MainView = mainView;
            _userRepository = new EfRepository<User>();
            Init();
        }

        public MainViewModel MainView { get; }


        private void Init()
        {
            Users = _userRepository.Init(UserDataFactory.Get());
        }



        #region Флаг авторизации
        /// <summary>
        /// Флаг авторизации
        /// </summary>
        private bool _isAuthorized;
        /// <summary>
        /// Флаг авторизации
        /// </summary>
        public bool IsAuthorized
        {
            get => _isAuthorized;
            set => Set(ref _isAuthorized, value);
        }
        #endregion

        #region Пользователи
        /// <summary>
        /// Пользователи
        /// </summary>
        private IEnumerable<User> _users;
        /// <summary>
        /// Пользователи
        /// </summary>
        public IEnumerable<User> Users
        {
            get => _users;
            set => Set(ref _users, value);
        }
        #endregion

        #region Выбранный пользователь
        /// <summary>
        /// Выбранный пользователь
        /// </summary>
        private User _selectedUser;
        /// <summary>
        /// Выбранный пользователь
        /// </summary>
        public User SelectedUser
        {
            get => _selectedUser;
            set => Set(ref _selectedUser, value);
        }
        #endregion

        #region Текущий пользователь
        /// <summary>
        /// Текущий пользователь
        /// </summary>
        private User _currentUser;
        /// <summary>
        /// Текущий пользователь
        /// </summary>
        public User CurrentUser
        {
            get => _currentUser;
            set 
            {
                if(Set(ref _currentUser, value))
                {
                    IsAuthorized = _currentUser != null;
                }
            } 
        }
        #endregion

        #region Команды

        #region Редактировтаь 
        /// <summary>
        /// Редактировтаь 
        /// </summary>
        RelayCommand _editUserCommand;
        /// <summary>
        /// Редактировтаь 
        /// </summary>
        public RelayCommand EditUserCommand => _editUserCommand ?? (_editUserCommand = new RelayCommand(execPar => 
        {
            if (SelectedUser is null) return;
            SafetyAction(() =>
            {
                ChangeUserWindow dialog = new ChangeUserWindow(SelectedUser);
                if (dialog.ShowDialog() == true)
                {
                    _userRepository.Update(SelectedUser);
                }
            });
        }, canExecPar => true));

        #endregion

        #region Удалить параметр
        /// <summary>
        /// Удалить параметр
        /// </summary>
        RelayCommand _deleteUserCommand;
        /// <summary>
        /// Удалить параметр
        /// </summary>
        public RelayCommand DeleteUserCommand => _deleteUserCommand ?? (_deleteUserCommand = new RelayCommand(execPar => 
        {
            if (SelectedUser is null) return;
            SafetyAction(() =>
            {
                _userRepository.Delete(SelectedUser);
            });
        }, canExecPar => true));
        #endregion

        #region Добавить
        /// <summary>
        /// Добавить
        /// </summary>
        RelayCommand _addUserCommand;
        /// <summary>
        /// Добавить
        /// </summary>
        public RelayCommand AddUserCommand => _addUserCommand ?? (_addUserCommand = new RelayCommand(execPar => 
        {
            var user = new User();
            SafetyAction(() =>
            {
                ChangeUserWindow dialog = new ChangeUserWindow(user);
                if (dialog.ShowDialog() == true)
                {
                    _userRepository.Add(user);
                }
            });
        }, canExecPar => true));
        #endregion


        #region Authorisation
        /// <summary>
        /// Authorisation
        /// </summary>
        RelayCommand _authCommand;
        /// <summary>
        /// Authorisation
        /// </summary>
        public RelayCommand AuthCommand => _authCommand ?? (_authCommand = new RelayCommand(execPar => 
        {
            var dialog = new AuthorizationWindow(this);
            dialog.Show();

        }, canExecPar => true));
        #endregion

        #region Команда "Логаут"
        RelayCommand _logoutCommand;
        public RelayCommand LogoutCommand { get => _logoutCommand ?? (_logoutCommand = new RelayCommand(o => CurrentUser = null, o => true)); }
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
    }
}
