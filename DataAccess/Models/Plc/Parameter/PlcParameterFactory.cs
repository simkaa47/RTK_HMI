using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Plc.Parameter
{
    public class PlcParameterFactory : PropertyChangedBase
    {
        public PlcParameterFactory() { }

        #region HummSensor
        public PlcParameter<short> HummSensorMinAdc { get; set; } = new PlcParameter<short>("HummSensorMinAdc", "Датчик влажности, АЦП, минимальное значение", 0, 32767, 1, 0, 0, false);

        public PlcParameter<short> HummSensorMaxAdc { get; set; } = new PlcParameter<short>("HummSensorMaxAdc", "Датчик влажности, АЦП, минимальное значение", 0, 32767, 1, 0, 2, false);

        public PlcParameter<float> HummSensorMinValue { get; set; } = new PlcParameter<float>("HummSensorMinAdc", "Датчик влажности, Минимальное значение", 0, 100.0f, 1, 0, 4, false);

        public PlcParameter<float> HummSensorMaxValue { get; set; } = new PlcParameter<float>("HummSensorMinAdc", "Датчик влажности, Максимальное значение", 0, 100.0f, 1, 0, 8, false);

        public PlcParameter<short> HummSensorInputType { get; set; } = new PlcParameter<short>("HummSensorMinAdc", "Датчик влажности, АЦП, тип входа", 0, 3, 1, 0, 12, false);


        #endregion

        #region TempSensor
        public PlcParameter<short> TempSensorMinAdc { get; set; } = new PlcParameter<short>("TempSensorMinAdc", "Датчик Температуры, АЦП, минимальное значение", 0, 32767, 1, 14, 0, false);

        public PlcParameter<short> TempSensorMaxAdc { get; set; } = new PlcParameter<short>("TempSensorMaxAdc", "Датчик Температуры, АЦП, максимальное значение", 0, 32767, 1, 14, 2, false);

        public PlcParameter<float> TempSensorMinValue { get; set; } = new PlcParameter<float>("TempSensorMinAdc", "Нижняя граница темп. диапазона", -50, 100.0f, 1, 14, 4, false);

        public PlcParameter<float> TempSensorMaxValue { get; set; } = new PlcParameter<float>("TempSensorMinAdc", "Верхняя граница темп. диапазона", -49, 150, 1, 14, 8, false);

        /// <summary>
        /// Тип входа: 0=4..20мА,1=0..20мА,2=0..10В,3=±10В
        /// </summary>
        public PlcParameter<short> TempSensorInputType { get; set; } = new PlcParameter<short>("TempSensorMinAdc", "Датчик Температуры, тип входа", 0, 3, 1, 14, 12, false);


        #endregion

        #region DiffPressureSensor
        public PlcParameter<short> DiffPressureSensorMinAdc { get; set; } = new PlcParameter<short>("DiffPressureSensorMinAdc", "Дифф. давление, АЦП, минимальное значение", 0, 32767, 1, 28, 0, false);

        public PlcParameter<short> DiffPressureSensorMaxAdc { get; set; } = new PlcParameter<short>("DiffPressureSensorMaxAdc", "Дифф. давление, АЦП, максимальное значение", 0, 32767, 1, 28, 2, false);

        public PlcParameter<float> DiffPressureSensorMinValue { get; set; } = new PlcParameter<float>("DiffPressureSensorMinAdc", "Нижняя граница дифф. давления", -2000.0f, 2000.0f, 1, 28, 4, false);

        public PlcParameter<float> DiffPressureSensorMaxValue { get; set; } = new PlcParameter<float>("DiffPressureSensorMinAdc", "Верхняя граница дифф. давления", -2000.0f, 2000.0f, 1, 28, 8, false);

        /// <summary>
        /// Тип входа: 0=4..20мА,1=0..20мА,2=0..10В,3=±10В
        /// </summary>
        public PlcParameter<short> DiffPressureSensorInputType { get; set; } = new PlcParameter<short>("DiffPressureSensorMinAdc", "Дифф. давление, тип входа", 0, 3, 1, 28, 12, false);


        #endregion

        #region AbsPressureSensor
        public PlcParameter<short> AbsPressureSensorMinAdc { get; set; } = new PlcParameter<short>("AbsPressureSensorMinAdc", "Абс. давление, АЦП, минимальное значение", 0, 32767, 1, 42, 0, false);

        public PlcParameter<short> AbsPressureSensorMaxAdc { get; set; } = new PlcParameter<short>("AbsPressureSensorMaxAdc", "Абс. давление, АЦП, максимальное значение", 0, 32767, 1, 42, 2, false);

        public PlcParameter<float> AbsPressureSensorMinValue { get; set; } = new PlcParameter<float>("AbsPressureSensorMinAdc", "Нижняя граница абс. давления, Па", -2000.0f, 200000.0f, 1, 42, 4, false);

        public PlcParameter<float> AbsPressureSensorMaxValue { get; set; } = new PlcParameter<float>("AbsPressureSensorMinAdc", "Верхняя граница абс. давления, Па", -2000.0f, 200000.0f, 1, 42, 8, false);

        /// <summary>
        /// Тип входа: 0=4..20мА,1=0..20мА,2=0..10В,3=±10В
        /// </summary>
        public PlcParameter<short> AbsPressureSensorInputType { get; set; } = new PlcParameter<short>("AbsPressureSensorMinAdc", "Абс. давление, Тип входа", 0, 3, 1, 42, 12, false);


        #endregion

        #region FlowSettings
        /// <summary>
        /// Газ: 0 = воздух, 1 = аргон
        /// </summary>
        public PlcParameter<short> GasType { get; set; } = new PlcParameter<short>("GasType", "Тип газа", 0, 1, 1, 56, 0, false);

        public PlcParameter<float> KProbe { get; set; } = new PlcParameter<float>("KProbe", "Коэффициент зонда (K-фактор × поправка на профиль)", -2000.0f, 200000.0f, 1, 56, 2, false);

        public PlcParameter<float> DpZero { get; set; } = new PlcParameter<float>("DpZero", "Смещение нуля датчика ΔP, Па", -2000.0f, 200000.0f, 1, 56, 6, false);

        public PlcParameter<float> DpCutoff { get; set; } = new PlcParameter<float>("DpCutoff", "Отсечка по ΔP, Па (5..10 % шкалы)", -2000.0f, 200000.0f, 1, 56, 10, false);

        public PlcParameter<float> DpHyst { get; set; } = new PlcParameter<float>("DpHyst", "Гистерезис снятия no_flow, Па", -2000.0f, 200000.0f, 1, 56, 14, false);

        public PlcParameter<float> FilterT { get; set; } = new PlcParameter<float>("FilterT", "Постоянная времени фильтра ΔP, с (0 = выкл)", -2000.0f, 200000.0f, 1, 56, 18, false);

        #endregion

        #region PID Settings (Контур скорости)

        public PlcParameter<float> Speed { get; set; } = new PlcParameter<float>("Speed", "Задание скорости, м/с", -100.0f, 100.0f, 1, 100, 0, false);
        /// <summary>
        /// Задание в ручном режиме, %. 0% - 0 Гц, 100% - 50 Гц
        /// </summary>
        public PlcParameter<float> ManPct { get; set; } = new PlcParameter<float>("ManPct", "Задание в ручном режиме, %", 0.0f, 100.0f, 1, 100, 4, false);
        public PlcParameter<float> Tol { get; set; } = new PlcParameter<float>("Tol", "Допуск для «в диапазоне», м/с", -100.0f, 100.0f, 1, 100, 8, false);
        public PlcParameter<float> InRangeT { get; set; } = new PlcParameter<float>("InRangeT", "Выдержка удержания в допуске, с", 0.0f, 5000.0f, 1, 100, 12, false);



        #endregion


    }
}
