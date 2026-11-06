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

        #region Control
        /// <summary>
        /// Mode: Режим: 0 = авто, 1 = ручной, 2 = автонастройка
        /// </summary>


        public PlcParameter<ushort> Mode { get; set; } = new PlcParameter<ushort>("Mode", "Режим работы ПЛК", 0, 2, 9, 0, 0, false);

        /// <summary>
        /// Сброс защёлкнутых аварий (импульс)
        /// </summary>


        public PlcParameter<bool> RstErrors { get; set; } = new PlcParameter<bool>("RstErrors", "Сброс защёлкнутых аварий", false, true, 9, 2, 0, false);

        /// <summary>
        /// Общий пуск установки
        /// </summary>


        public PlcParameter<bool> Run { get; set; }  = new PlcParameter<bool>("Run", "Пуск установки", false, true, 9, 2, 1, false);

        #endregion

        #region Parameters

        public PlcParameterFactory PlcParameters { get; set; } = new PlcParameterFactory();

        #endregion


    }
}
