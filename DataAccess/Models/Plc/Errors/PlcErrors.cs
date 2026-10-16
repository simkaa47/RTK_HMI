using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Plc.Errors
{
    public class PlcErrors : PropertyChangedBase
    {
        /// <summary>
        /// Датчик влажности: обрыв (4..20 мА)
        /// </summary>
        private bool _errHumiditySensorBreak;
        public bool ErrHumiditySensorBreak { get => _errHumiditySensorBreak; set => Set(ref _errHumiditySensorBreak, value); }
        /// <summary>
        /// Датчик влажности: вне диапазона АЦП
        /// </summary>
        private bool _errHumiditySensorOutOfRange;
        public bool ErrHumiditySensorOutOfRange { get => _errHumiditySensorOutOfRange; set => Set(ref _errHumiditySensorOutOfRange, value); }
        /// <summary>
        /// Датчик влажности: сводная ошибка
        /// </summary>
        private bool _errHumiditySensor;
        public bool ErrHumiditySensor { get => _errHumiditySensor; set => Set(ref _errHumiditySensor, value); }
        /// <summary>
        /// Абс. давление: обрыв
        /// </summary>
        private bool _errAbsPressureSensorBreak;
        public bool ErrAbsPressureSensorBreak { get => _errAbsPressureSensorBreak; set => Set(ref _errAbsPressureSensorBreak, value); }
        /// <summary>
        /// Абс. давление: вне диапазона
        /// </summary>
        private bool _errAbsPressureSensorOutOfRange;
        public bool ErrAbsPressureSensorOutOfRange { get => _errAbsPressureSensorOutOfRange; set => Set(ref _errAbsPressureSensorOutOfRange, value); }
        /// <summary>
        /// Абс. давление: сводная ошибка
        /// </summary>
        private bool _errAbsPressureSensor;
        public bool ErrAbsPressureSensor { get => _errAbsPressureSensor; set => Set(ref _errAbsPressureSensor, value); }
        /// <summary>
        /// Дифф. давление: обрыв
        /// </summary>
        private bool _errDiffPressureSensorBreak;
        public bool ErrDiffPressureSensorBreak { get => _errDiffPressureSensorBreak; set => Set(ref _errDiffPressureSensorBreak, value); }
        /// <summary>
        /// Дифф. давление: вне диапазона
        /// </summary>
        private bool _errDiffPressureSensorOutOfRange;
        public bool ErrDiffPressureSensorOutOfRange { get => _errDiffPressureSensorOutOfRange; set => Set(ref _errDiffPressureSensorOutOfRange, value); }
        /// <summary>
        /// Дифф. давление: сводная ошибка
        /// </summary>
        private bool _errDiffPressureSensor;
        public bool ErrDiffPressureSensor { get => _errDiffPressureSensor; set => Set(ref _errDiffPressureSensor, value); }
        /// <summary>
        /// Температура: обрыв
        /// </summary>
        private bool _errTempPressureSensorBreak;
        public bool ErrTempPressureSensorBreak { get => _errTempPressureSensorBreak; set => Set(ref _errTempPressureSensorBreak, value); }
        /// <summary>
        /// Температура: вне диапазона
        /// </summary>
        private bool _errTempPressureSensorOutOfRange;
        public bool ErrTempPressureSensorOutOfRange { get => _errTempPressureSensorOutOfRange; set => Set(ref _errTempPressureSensorOutOfRange, value); }
        /// <summary>
        /// Температура: сводная ошибка
        /// </summary>
        private bool _errTempPressureSensor;
        public bool ErrTempPressureSensor { get => _errTempPressureSensor; set => Set(ref _errTempPressureSensor, value); }
        /// <summary>
        /// ПЧ не готов (сигнал fc_rdy отсутствует)
        /// </summary>
        private bool _errFcNotRdy;
        public bool ErrFcNotRdy { get => _errFcNotRdy; set => Set(ref _errFcNotRdy, value); }
        /// <summary>
        /// Нет связи с ПЧ по Modbus
        /// </summary>
        private bool _errFcComm;
        public bool ErrFcComm { get => _errFcComm; set => Set(ref _errFcComm, value); }
        /// <summary>
        /// Аварийный стоп (кнопка), защёлка
        /// </summary>
        private bool _errEmergencyStop;
        public bool ErrEmergencyStop { get => _errEmergencyStop; set => Set(ref _errEmergencyStop, value); }
        /// <summary>
        /// Автонастройка ни разу не выполнялась (блок авто-режима)
        /// </summary>
        private bool _errNoTuning;
        public bool ErrNoTuning { get => _errNoTuning; set => Set(ref _errNoTuning, value); }
    }
}
