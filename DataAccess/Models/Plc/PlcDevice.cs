using DataAccess.Models.Plc.Settings;
using DataAccess.Models.Plc.PlcViews;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DataAccess.Models.Plc.Errors;
using DataAccess.Models.Plc.Parameter;

namespace DataAccess.Models.Plc
{
    public class PlcDevice : PropertyChangedBase
    {
        public PlcDevice() 
        {

        }

        #region Settings

        #region Humm Sensor Settings

        public PlcSettings HummSensorSettings { get; set; } = new PlcSettings();

        #endregion

        #region Temp Sensor Settings

        public PlcSettings TempSensorSettings { get; set; } = new PlcSettings();

        #endregion

        #region Diff Pressure Settings

        public PlcSettings DiffPressureSettings { get; set; } = new PlcSettings();

        #endregion

        #region Abs Pressure Settings

        public PlcSettings AbsPressureSettings { get; set; } = new PlcSettings();

        #endregion

        #region Flow Settings

        public FlowSettings FlowSettings { get; set; } = new FlowSettings();

        #endregion

        #region PID

        public PidSettings PidSettings { get; set; } = new PidSettings();

        #endregion


        #endregion

        #region View

        #region Humm Sensor View

        public PlcView HummSensorView { get; set; } = new PlcView();

        #endregion

        #region Temp Sensor View

        public PlcView TempSensorView { get; set; } = new PlcView();    

        #endregion

        #region Diff Pressure View

        public PlcView DiffPressureView { get; set; } = new PlcView();

        #endregion

        #region Abs Pressure View

        public PlcView AbsPressureView { get; set; } = new PlcView();

        #endregion

        #region FlowView

        public FlowView FlowView { get; set; } = new FlowView();

        #endregion

        #region PID View

        public PidView PidView { get; set; } = new PidView();

        #endregion


        #endregion

        #region Errors

        public PlcErrors Errors { get; set; } = new PlcErrors();

        #endregion

        /// <summary>
        /// Mode: Режим: 0 = авто, 1 = ручной, 2 = автонастройка
        /// </summary>
        //private ushort _mode;

        //public ushort Mode
        //{
        //    get => _mode;
        //    set => Set(ref _mode, value);

        //}

        public PlcParameter<ushort> Mode = new PlcParameter<ushort>("Mode", "Mode", 0, 2, 9, 0, 0, false);

        /// <summary>
        /// Сброс защёлкнутых аварий (импульс)
        /// </summary>
        //private bool _rstErrors;

        //public bool RstErrors
        //{
        //    get => _rstErrors;
        //    set => Set(ref _rstErrors, value);
        //}

        public PlcParameter<bool> RstErrors = new PlcParameter<bool>("RstErrors", "Сброс защёлкнутых аварий", false, true, 9, 2, 0, false);

        /// <summary>
        /// Общий пуск установки
        /// </summary>
        //private bool _run;

        //public bool Run
        //{
        //    get => _run;
        //    set => Set(ref _run, value);
        //}

        public PlcParameter<bool> Run = new PlcParameter<bool>("Run", "Пуск установки", false, true, 9, 2, 1, false);

        public PlcParameter<float> DeltaPZero { get; set; } = new PlcParameter<float>("DeltaPZero", "Смещение нуля датчика ΔP, Па", 0, 10000, 1, 56, 6, false);

        public PlcParameter<float> OutHz { get; set; } = new PlcParameter<float>("OutHz", "Частота ПЧ, ГЦ", 0.0f, 50.0f, 2, 80, 8, false);

    }
}
