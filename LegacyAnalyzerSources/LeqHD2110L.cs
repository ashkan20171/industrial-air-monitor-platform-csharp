using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SadraAQMS.Analyzers
{
    internal class LeqHD2110L : deviceAnalyzer
    {

        public LeqHD2110L(DeviceType dvType, string devID)
        {
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();

            //_alarmValue = 0;
        }

        public LeqHD2110L(DeviceType dvType, string devID, string devName, string ignorableErrors)
        {
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            this.DeviceName = devName;
            int.TryParse(ignorableErrors, out IgnorableErrors);
            //_alarmValue = 0;
        }
        public override void sendReadCommand()
        {
            //<stx>#01<ETX>
            messageSendOnSerialBytes(new byte[] { 0x23, 0x30, 0x31, 0x0d });
        }
        public override void parseMessage()
        {
            //#01
            // > -00.009 - 00.007 - 00.009 - 00.009 - 0.0093 + 0.7921 + 0.0264 - 00.009=> leq:+ 0.7921
            // leq:+ 0.7921 mv so Convert to v Multiply 1000 
            try
            {
                string messageStr = Encoding.UTF8.GetString(messageReceived.ToArray());
                messageStr = messageStr.Contains('>') ? messageStr.Trim('>') : messageStr;
                List<string> tmpStrData = new List<string>(messageStr.Split(new char[] { '-', '+' }));
                int tmpAlarm = 0;
                for (int k = 0; k < tmpStrData.Count; k++)
                {
                    if (tmpStrData[k].Trim() == "")
                    {
                        tmpStrData.RemoveAt(k);
                        k--;
                    }
                }

                if (tmpStrData.Count >= 2)
                {
                    if (polutions[0].ID != polutions[0].mapperID)
                    {
                        foreach (var pol in polutions)
                        {
                            if (!float.TryParse(tmpStrData[pol.mapperID], out pol.concentration))
                                errorLog(string.Format(
                                    "Device: {0} ParseMessage: Could not Convert concentration data : {1} to float",
                                    deviceName, tmpStrData[pol.mapperID]));
                        }
                    }
                    else
                    {

                        if (messageStr.Contains("    ") )
                        {
                          //  statusLog(String.Format("LEQ 111 {0}",messageStr));

                            messageStr = messageStr.Trim();
                            tmpStrData = new List<string>(messageStr.Split(new char[] { '-', '+' }));
                            polutions[0].concentration = float.Parse(tmpStrData[3].Trim()) * 1000; //mv so convert to v Multiply 1000
                        }
                        else
                            polutions[0].concentration = float.Parse(tmpStrData[5]) * 1000; //mv so convert to v Multiply 1000

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
        internal override void initialize_Connect()
        {
            try
            {
                statusLog("start client for Aeroqual");
                isConnected = true;

                base.initialize_Connect();

            }
            catch (Exception ex)
            {
                errorLog("TCP conncetion problem for device analyzer Aeroqual: " + ex.Message);
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
