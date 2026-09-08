using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Plc.PlcViews
{
    public class FlowView : PropertyChangedBase
    {
        /// <summary>
        /// Плотность газа, кг/м³
        /// </summary>
        private float _density;
        public float Density { get => _deltaPNet; set => Set(ref _deltaPNet, value); }
        /// <summary>
        /// ΔP за вычетом нуля, Па
        /// </summary>
        private float _deltaPNet;
        public float DeltaPNet { get => _deltaPNet; set => Set(ref _deltaPNet, value); }
        /// <summary>
        /// ΔP после фильтра, Па
        /// </summary>
        private float _deltaPFilter;
        public float DeltaPFilter { get => _deltaPNet; set => Set(ref _deltaPNet, value); }
        /// <summary>
        /// Скорость потока, м/с (обратная связь ПИД)
        /// </summary>
        private float _velocity;
        public float Velocity { get => _velocity; set => Set(ref _velocity, value); }
    }
}
