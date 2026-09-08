using DataAccess.Models.Plc.Settings;
using DataAccess.Models.Plc.PlcViews;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DataAccess.Models.Plc.Errors;

namespace DataAccess.Models.Plc
{
    public class PlcDevice : PropertyChangedBase
    {
        public PlcDevice() 
        {

        }

        #region Settings

        #region Humm Sensor Settings

        public PlcSettings HummSensorSettings { get; set; } = new PlcSettings(1);

        #endregion

        #region Temp Sensor Settings

        public PlcSettings TempSensorSettings { get; set; } = new PlcSettings(1);

        #endregion

        #region Diff Pressure Settings

        public PlcSettings DiffPressureSettings { get; set; } = new PlcSettings(1);

        #endregion

        #region Abs Pressure Settings

        public PlcSettings AbsPressureSettings { get; set; } = new PlcSettings(1);

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

        public PlcErrors Errors { get; set; }

        #endregion

        /// <summary>
        /// Mode: Режим: 0 = авто, 1 = ручной, 2 = автонастройка
        /// </summary>
        private ushort _mode;

        public ushort Mode
        {
            get => _mode;
            set => Set(ref _mode, value);

        }

        /// <summary>
        /// Сброс защёлкнутых аварий (импульс)
        /// </summary>
        private bool _rstErrors;

        public bool RstErrors
        {
            get => _rstErrors;
            set => Set(ref _rstErrors, value);
        }

        /// <summary>
        /// Общий пуск установки
        /// </summary>
        private bool _run;

        public bool Run
        {
            get => _run;
            set => Set(ref _run, value);
        }

    }
}
