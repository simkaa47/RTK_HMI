using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Plc.Settings
{
    public class PidSettings : PropertyChangedBase
    {
        /// <summary>
        /// Задание скорости, м/с
        /// </summary>
        private float _speed;
        public float Speed { get => _speed; set => Set(ref _speed, value); }
        /// <summary>
        /// Задание в ручном режиме, %
        /// </summary>
        private float _manPct;
        public float ManPct { get => _manPct; set => Set(ref _manPct, value); }
        /// <summary>
        /// Допуск для «в диапазоне», м/с
        /// </summary>
        private float _tol;
        public float Tol { get => _tol; set => Set(ref _tol, value); }
        /// <summary>
        /// Выдержка удержания в допуске, с
        /// </summary>
        private float _inRangeTime;
        public float InRangeTime { get => _inRangeTime; set => Set(ref _inRangeTime, value); }
    }
}
