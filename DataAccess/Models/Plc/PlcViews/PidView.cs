using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Plc.PlcViews
{
    public class PidView : PropertyChangedBase
    {
        /// <summary>
        /// Скорость на входе ПИД, м/с
        /// </summary>
        private float _pv;
        public float Pv {  get => _pv; set => Set(ref _pv, value); }
        /// <summary>
        /// Выход ПИД, %
        /// </summary>
        private float _outPct;
        public float OutPct { get => _outPct; set => Set(ref _outPct, value); }
        /// <summary>
        /// Задание частоты в ПЧ, Гц
        /// </summary>
        private float _outHz;
        public float OutHz { get => _outHz; set => Set(ref _outHz, value); }
        /// <summary>
        /// Защёлка последнего достоверного выхода, %
        /// </summary>
        private float _outLast;
        public float OutLast { get => _outLast; set => Set(ref _outLast, value); }
        /// <summary>
        /// Состояние ПИД: 0=Inactive 1=Pretune 2=Finetune 3=Auto 4=Manual 5=Subst
        /// </summary>
        private short _state;
        public short State { get => _state; set => Set(ref _state, value); }
        /// <summary>
        /// Служебное (прошлый режим)
        /// </summary>
        private short _modePrev;
        public short ModePrev { get => _modePrev; set => Set(ref _modePrev, value); }
        /// <summary>
        /// Битовая маска ошибок PID_Compact
        /// </summary>
        private uint _errorBits;
        public uint ErrorBits { get => _errorBits; set => Set(ref _errorBits, value); }
        /// <summary>
        /// Счётчик выдержки допуска, с
        /// </summary>
        private float _inRangeTmp;
        public float InRangeTmr { get => _inRangeTmp; set => Set(ref _inRangeTmp, value); }
        /// <summary>
        /// Автонастройка выполнялась хотя бы раз
        /// </summary>
        private bool _tuned;
        public bool Tuned { get => _tuned; set => Set(ref _tuned, value); }
        /// <summary>
        /// Служебное (State прошлого цикла)
        /// </summary>
        private short _statePrev;
        public short StatePrev { get => _statePrev; set => Set(ref _statePrev, value); }
    }
}
