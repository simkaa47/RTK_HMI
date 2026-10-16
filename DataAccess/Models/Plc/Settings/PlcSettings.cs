using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Plc.Settings
{
    public class PlcSettings : PropertyChangedBase
    {
        private short _minAdc;
        public short MinAdc { get => _minAdc; set => Set(ref _minAdc, value);  }

        private short _maxAdc;
        public short MaxAdc { get => _maxAdc; set => Set(ref _maxAdc, value); }

        private float _minValue;
        public float MinValue { get => _minValue; set => Set( ref  _minValue, value);  }

        private float _maxValue;
        public float MaxValue { get => _maxValue; set => Set(ref _maxValue, value); }

        private short _inputType;
        public short InputType { get => _inputType; set => Set(ref _inputType, value); }

        public PlcSettings()
        {
            
        }


    }
}
