using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SadraAQMS.Analyzers
{
    class DeltaOHMWeather : deviceAnalyzer
    {

        public DeltaOHMWeather(DeviceType dvType, string devID, string devName, string ignorableErrors)
        {
            this.deviceType = DeviceType.DeltaOHMWeather;
            this.deviceName = DeviceType.DeltaOHMWeather.ToString();
            this.DeviceName = devName;
            int.TryParse(ignorableErrors, out IgnorableErrors);

            //_alarmValue = 0;

        }
        public override void sendReadCommand()
        {
            //  <stx>DA<ETX>04\n
            // messageSendOnSerialBytes(new byte[] { 0x02, 0x44, 0x41, 0x03, 0x30, 0x34, 0x0A });
        }


        public override void parseMessage()
        {
            // First 4 Space then Sperator 3 space
            //   1-WS   0-WD   2-Pressure   3-Temp    4-RH   5-SoLar  ....  10-Elev  11-???   12-SonicTemp  

            try
            {
                string messageStr = Encoding.UTF8.GetString(messageReceived.ToArray());
                
                //messageStr = "Reset \r\n";
                //messageStr = "   2.22   334.7  1015.1    18.1    46.4     0.0    0.94   -1.99    0.24    2.20     6.2   341.6    16.4 \r\n";

                List <string> tmpStrData = new List<string>(messageStr.Split(' '));
                int tmpAlarm = 0;

                for (int k = 0; k < tmpStrData.Count; k++)
                {
                    if (tmpStrData[k].Trim() == "")
                    {
                        tmpStrData.RemoveAt(k);
                        k--;
                    }
                }

                //statusLog("Length:" + tmpStrData.Count);

                if (tmpStrData.Count >= 10)
                {
                    foreach (var pol in polutions)
                    {
                        if (!float.TryParse(tmpStrData[pol.mapperID], out pol.concentration))
                            errorLog(string.Format(
                                "Device: {0} ParseMessage: Could not Convert concentration data : {1} to float",
                                deviceName, tmpStrData[pol.mapperID]));
                        //statusLog("polID:" + pol.ID + ":" + pol.mapperID);
                    }

                    timeLastMessageRecieved = DateTime.Now;
                    waitForResponseList[1].Set();
                    workingStatus = true;
                }

                if (writeMode)
                {
                    sw.WriteLine(messageStr);
                    sw.Flush();
                }
            }
            catch (Exception ex)
            {
                errorLog("[Error Parse Delta OHM] - " + ex.Message);
            }
        }

        internal override void Disconnect()
        {
            if (conSerial.IsOpen)
                conSerial.Close();

            base.Disconnect();
        }
    }
}
