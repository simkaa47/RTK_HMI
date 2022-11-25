using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.Models
{
    public class JsonData:PropertyChangedBase
    {

        #region Имя
        /// <summary>
        /// Имя
        /// </summary>
        private string _name;
        /// <summary>
        /// Имя
        /// </summary>
        [MinLength(3)]
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }
        #endregion

        #region Дата создания
        /// <summary>
        /// Дата создания
        /// </summary>
        private DateTime _changeTime;
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime ChangeTime
        {
            get => _changeTime;
            set => Set(ref _changeTime, value);
        }
        #endregion



    }
}
