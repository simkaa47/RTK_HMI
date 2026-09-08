using DataAccess.Models.Plc;
using DataAccess.Models.Plc.Settings;
using S7.Net;
using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        private CancellationToken _cts = new CancellationToken();

        public async Task Initialize()
        {
            _client = new Plc(CpuType.S71200, "192.168.1.215", 0, 2);
        }

        public async Task Connect()
        {
            if (_client is not null)
            {
                _client.Open();
                isPlcConnected = true;
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
                            //Disconnect();
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
            _plcDevice.Mode = (ushort)_client.Read("DB9.DBW0");
            _plcDevice.RstErrors = (bool)_client.Read("DB9.DBX2.0");
            _plcDevice.Run = (bool)_client.Read("DB9.DBX2.1");
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




        #endregion



    }
}
