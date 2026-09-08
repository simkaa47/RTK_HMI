using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Plc.Settings
{
    public class FlowSettings : PropertyChangedBase
    {
        /// <summary>
        /// Газ: 0 = воздух, 1 = аргон
        /// </summary>
        private short _gasType;
        public short GasType { get => _gasType; set => Set(ref _gasType, value); }
        /// <summary>
        /// Коэффициент зонда (K-фактор × поправка на профиль)
        /// </summary>
        private float _kProbe;
        public float KProbe { get => _kProbe; set => Set(ref _kProbe, value); }
        /// <summary>
        /// Смещение нуля датчика ΔP, Па
        /// </summary>
        private float _deltaPZero;
        public float DeltaPZero { get => _deltaPZero; set => Set(ref _deltaPZero, value); }
        /// <summary>
        /// Отсечка по ΔP, Па (5..10 % шкалы)
        /// </summary>
        private float _deltaPCutOff;
        public float DeltaPCutOff { get => _deltaPCutOff; set => Set(ref _deltaPCutOff, value); }
        /// <summary>
        /// Гистерезис снятия no_flow, Па
        /// </summary>
        private float _deltaPCutHyst;
        public float DeltaPHyst { get => _deltaPCutHyst; set => Set(ref _deltaPCutHyst, value); }
        /// <summary>
        /// Постоянная времени фильтра ΔP, с (0 = выкл)
        /// </summary>
        private float _filterTimeConst;
        public float FilterTimeConst { get => _filterTimeConst; set => Set(ref _filterTimeConst, value); }
    }
}
