using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SadraAQMS.Analyzers
{
    class HoribaAnalyzers : deviceAnalyzer // horiba350 not tested any time
    {
        public HoribaAnalyzers(DeviceType dvType, string devID)
        {
            //this.company = DeviceCompany.Horiba;
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();

            //_alarmValue = 0;
        }
        public HoribaAnalyzers(DeviceType dvType, string devID, string devName, string ignorableErrors)
        {
            //this.company = DeviceCompany.Ecotech;
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            this.DeviceName = devName;
            int.TryParse(ignorableErrors, out IgnorableErrors);

            //_alarmValue = 0;
        }

        public override void sendReadCommand()
        {
            messageSendOnSerial("RA1," + DeviceID + "\r\n");
        }

        public void parseMessage(string deviceResponse)
        {
            try
            {
               // alarmValue = Convert.ToInt32(deviceResponse.Substring(7, 16));
                for (int ind = 0; ind < polutions.Count; ind++)
                {
                    if (deviceResponse.Length > (33 + 12 * ind) + 10)
                        if (!float.TryParse(deviceResponse.Substring(33 + 12 * ind, 11), out polutions[ind].concentration))
                            errorLog(string.Format("Device: {0} ParseMessage: Could not Convert concentration data : {1} to float for polutionId {2}", deviceName, deviceResponse.Substring(33 + 12 * ind, 11), polutions[ind]));
                    polutions[ind].alarmValue= Convert.ToInt32(deviceResponse.Substring(7, 16));
                    //polutions[ind].concentration = Convert.ToSingle(deviceResponse.Substring(33 + 12 * ind, 11));
                }

                if (writeMode)
                {
                    sw.WriteLine(deviceResponse);
                    sw.Flush();
                }
            }
            catch (Exception ex)
            {
                errorLog("[Horiba Analyzer ParseMessage] - " + ex.Message);
            }
        }

        public override void Calibrate()
        {

            // Horiba (O3)
            messageSendOnSerial("AZC,5" + "\r\n");

            // Horiba (CO)
            //messageSentOnSerial("AZC,?" + "\r\n"); //???

            // Horiba (NOx)
            //messageSentOnSerial("AZC,1" + "\r\n");

            // Horiba (SO2)
            //messageSentOnSerial("AZC,4" + "\r\n");

        }
        internal override void Disconnect()
        {
            if (conSerial.IsOpen)
                conSerial.Close();

            base.Disconnect();

        }

    }
}
