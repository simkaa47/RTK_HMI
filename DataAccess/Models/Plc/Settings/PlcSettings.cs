using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Plc.Settings
{
    public class PlcSettings : PropertyChangedBase
    {
        private short _minAdc;
        public short MinAdc { get => _minAdc; set { _minAdc = value; } }

        private short _maxAdc;
        public short MaxAdc { get => _maxAdc; set { _maxAdc = value; } }

        private float _minValue;
        public float MinValue { get => _minValue; set { _minValue = value; } }

        private float _maxValue;
        public float MaxValue { get => _maxValue; set { _maxValue = value; } }

        private short _inputType;
        public short InputType { get => _inputType; set { _inputType = value; } }

        private readonly int _dbNumber;

        public PlcSettings(int dbNumber)
        {
            _dbNumber = dbNumber;
        }

        public int DbNumber => _dbNumber;

    }
}
