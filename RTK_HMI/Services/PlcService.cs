using DataAccess.Models.Plc;
using DataAccess.Models.Plc.Errors;
using DataAccess.Models.Plc.Parameter;
using DataAccess.Models.Plc.Settings;
using S7.Net;
using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace RTK_HMI.Services
{
    public class PlcService
    {
        public PlcService(PlcDevice plcDevice) 
        {
            Initialize();
            _plcDevice = plcDevice;
        }

        private Plc _client; 

        public PlcDevice _plcDevice = new PlcDevice();

        public bool isPlcConnected = false;

        private CancellationTokenSource _cts = new CancellationTokenSource();

        public async Task Initialize()
        {
            _client = new Plc(CpuType.S71200, "192.168.1.215", 0, 0);
        }

        public async Task Connect()
        {
            if (_client is not null)
            {
                try
                {
                    _client.Open();
                    isPlcConnected = true;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public async Task Disconnect()
        {
            if (_client is not null)
            {
                _client.Close();
                isPlcConnected = false;
            }
        }

        #region Чтение
        public async Task GetPlcDataAsync(CancellationToken cancellationToken)
        {
            if (isPlcConnected)
            {
                while (!cancellationToken.IsCancellationRequested) {
                    await Task.Run(async () =>
                    {
                        try
                        {
                            GetControl();
                            GetSensorSettings();
                            GetViewSettings();
                            GetErrorData();
                            

                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                        }
                    });
                }

            }
        }

        private void GetSensorSettings()
        {
            _client.ReadClass(_plcDevice.HummSensorSettings, 1);
            _client.ReadClass(_plcDevice.TempSensorSettings, 1, 14);
            _client.ReadClass(_plcDevice.DiffPressureSettings, 1, 28);
            _client.ReadClass(_plcDevice.AbsPressureSettings, 1, 42);
            _client.ReadClass(_plcDevice.FlowSettings, 1, 56);
            _client.ReadClass(_plcDevice.PidSettings, 1, 100);
            _plcDevice.PlcParameters.Speed.Value = _plcDevice.PidSettings.Speed;
            _plcDevice.PlcParameters.ManPct.Value = _plcDevice.PidSettings.ManPct;
            _plcDevice.PlcParameters.Tol.Value = _plcDevice.PidSettings.Tol;
            _plcDevice.PlcParameters.InRangeT.Value = _plcDevice.PidSettings.InRangeTime;

            _plcDevice.PlcParameters.GasType.Value = _plcDevice.FlowSettings.GasType;
            _plcDevice.PlcParameters.KProbe.Value = _plcDevice.FlowSettings.KProbe;
            _plcDevice.PlcParameters.DpZero.Value = _plcDevice.FlowSettings.DeltaPZero;
            _plcDevice.PlcParameters.DpCutoff.Value = _plcDevice.FlowSettings.DeltaPCutOff;
            _plcDevice.PlcParameters.DpHyst.Value = _plcDevice.FlowSettings.DeltaPHyst;
            _plcDevice.PlcParameters.FilterT.Value = _plcDevice.FlowSettings.FilterTimeConst;


            _plcDevice.PlcParameters.TempSensorInputType.Value = _plcDevice.TempSensorSettings.InputType;
            _plcDevice.PlcParameters.TempSensorMinAdc.Value = _plcDevice.TempSensorSettings.MinAdc;
            _plcDevice.PlcParameters.TempSensorMaxAdc.Value = _plcDevice.TempSensorSettings.MaxAdc;
            _plcDevice.PlcParameters.TempSensorMinValue.Value = _plcDevice.TempSensorSettings.MinValue;
            _plcDevice.PlcParameters.TempSensorMaxValue.Value = _plcDevice.TempSensorSettings.MaxValue;


            _plcDevice.PlcParameters.HummSensorInputType.Value = _plcDevice.HummSensorSettings.InputType;
            _plcDevice.PlcParameters.HummSensorMinAdc.Value = _plcDevice.HummSensorSettings.MinAdc;
            _plcDevice.PlcParameters.HummSensorMaxAdc.Value = _plcDevice.HummSensorSettings.MaxAdc;
            _plcDevice.PlcParameters.HummSensorMinValue.Value = _plcDevice.HummSensorSettings.MinValue;
            _plcDevice.PlcParameters.HummSensorMaxValue.Value = _plcDevice.HummSensorSettings.MaxValue;


            _plcDevice.PlcParameters.DiffPressureSensorInputType.Value = _plcDevice.DiffPressureSettings.InputType;
            _plcDevice.PlcParameters.DiffPressureSensorMinAdc.Value = _plcDevice.DiffPressureSettings.MinAdc;
            _plcDevice.PlcParameters.DiffPressureSensorMaxAdc.Value = _plcDevice.DiffPressureSettings.MaxAdc;
            _plcDevice.PlcParameters.DiffPressureSensorMinValue.Value = _plcDevice.DiffPressureSettings.MinValue;
            _plcDevice.PlcParameters.DiffPressureSensorMaxValue.Value = _plcDevice.DiffPressureSettings.MaxValue;

            _plcDevice.PlcParameters.AbsPressureSensorInputType.Value = _plcDevice.AbsPressureSettings.InputType;
            _plcDevice.PlcParameters.AbsPressureSensorMinAdc.Value = _plcDevice.AbsPressureSettings.MinAdc;
            _plcDevice.PlcParameters.AbsPressureSensorMaxAdc.Value = _plcDevice.AbsPressureSettings.MaxAdc;
            _plcDevice.PlcParameters.AbsPressureSensorMinValue.Value = _plcDevice.AbsPressureSettings.MinValue;
            _plcDevice.PlcParameters.AbsPressureSensorMaxValue.Value = _plcDevice.AbsPressureSettings.MaxValue;
        }

        private void GetViewSettings()
        {
            _client.ReadClass(_plcDevice.HummSensorView, 2);
            _client.ReadClass(_plcDevice.TempSensorView, 2, 10);
            _client.ReadClass(_plcDevice.DiffPressureView, 2, 20);
            _client.ReadClass(_plcDevice.AbsPressureView, 2, 30);
            _client.ReadClass(_plcDevice.FlowView, 2, 40);
            _client.ReadClass(_plcDevice.PidView, 2, 80);

        }

        private void GetErrorData()
        {
            _client.ReadClass(_plcDevice.Errors, 2, 120);
        }

        private void GetControl()
        {
            _plcDevice.Mode.Value = (ushort)_client.Read("DB9.DBW0");
            _plcDevice.RstErrors.Value = (bool)_client.Read("DB9.DBX2.0");
            _plcDevice.Run.Value = (bool)_client.Read("DB9.DBX2.1");
        }


        #endregion

        #region Запись

        public void WriteClass(object par, int db, int startbyte)
        {
            if (par != null) 
            {
                _client.WriteClass(par, db, startbyte);
            }
            
        }


        public void WriteParameter(object value, int db, int startByteAdr, int byteAdr)
        {
            if (value is bool boolValue)
            {
                // Параметры: (Тип, Номер DB, Номер байта, Номер бита, Значение)
                _client.WriteBit(DataType.DataBlock, db, startByteAdr, byteAdr, boolValue);
            }
            if (value is int intValue) 
            {
                _client.Write(DataType.DataBlock, db, startByteAdr, byteAdr, intValue);
            }
            if (value is float realValue)
            {
                _client.Write(DataType.DataBlock, db, startByteAdr + byteAdr, realValue);
            }

            

        }

        public void WriteVariable(string str, object value)
        {
            _client.Write(str, value);
        }


        #endregion



    }
}
