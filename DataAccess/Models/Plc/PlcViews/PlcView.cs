using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Plc.PlcViews
{
    public class PlcView : PropertyChangedBase
    {
        private short _adc;
        public short Adc { get => _adc; set => Set(ref _adc, value); }

        private float _voltageCur;
        public float VoltageCur { get => _voltageCur; set => Set(ref _voltageCur, value); }

        private float _value;
        public float Value { get => _value; set => Set(ref _value, value); }


    }
}
