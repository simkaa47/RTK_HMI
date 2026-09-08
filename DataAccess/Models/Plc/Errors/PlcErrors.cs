using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Plc.Errors
{
    public class PlcErrors
    {
        /// <summary>
        /// Датчик влажности: обрыв (4..20 мА)
        /// </summary>
        public bool ErrHumiditySensorBreak { get; set; }
        /// <summary>
        /// Датчик влажности: вне диапазона АЦП
        /// </summary>
        public bool ErrHumiditySensorOutOfRange { get; set; }
        /// <summary>
        /// Датчик влажности: сводная ошибка
        /// </summary>
        public bool ErrHumiditySensor { get; set; }
        /// <summary>
        /// Абс. давление: обрыв
        /// </summary>
        public bool ErrAbsPressureSensorBreak { get; set; }
        /// <summary>
        /// Абс. давление: вне диапазона
        /// </summary>
        public bool ErrAbsPressureSensorOutOfRange { get; set; }
        /// <summary>
        /// Абс. давление: сводная ошибка
        /// </summary>
        public bool ErrAbsPressureSensor { get; set; }
        /// <summary>
        /// Дифф. давление: обрыв
        /// </summary>
        public bool ErrDiffPressureSensorBreak { get; set; }
        /// <summary>
        /// Дифф. давление: вне диапазона
        /// </summary>
        public bool ErrDiffPressureSensorOutOfRange { get; set; }
        /// <summary>
        /// Дифф. давление: сводная ошибка
        /// </summary>
        public bool ErrDiffPressureSensor { get; set; }
        /// <summary>
        /// Температура: обрыв
        /// </summary>
        public bool ErrTempPressureSensorBreak { get; set; }
        /// <summary>
        /// Температура: вне диапазона
        /// </summary>
        public bool ErrTempPressureSensorOutOfRange { get; set; }
        /// <summary>
        /// Температура: сводная ошибка
        /// </summary>
        public bool ErrTempPressureSensor { get; set; }
        /// <summary>
        /// ПЧ не готов (сигнал fc_rdy отсутствует)
        /// </summary>
        public bool ErrFcNotRdy { get; set; }
        /// <summary>
        /// Нет связи с ПЧ по Modbus
        /// </summary>
        public bool ErrFcComm { get; set; }
        /// <summary>
        /// Аварийный стоп (кнопка), защёлка
        /// </summary>
        public bool ErrEmergencyStop { get; set; }
        /// <summary>
        /// Автонастройка ни разу не выполнялась (блок авто-режима)
        /// </summary>
        public bool ErrNoTuning { get; set; }
    }
}
