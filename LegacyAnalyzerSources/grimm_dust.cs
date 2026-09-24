using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SadraAQMS.Analyzers
{
    class grimm_dust : deviceAnalyzer
    {


        public grimm_dust(DeviceType dvType, string devID, string devName, string ignorableErrors)
        {
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            this.DeviceName = devName;

            int.TryParse(ignorableErrors, out IgnorableErrors);

            //_alarmValue = 0;
        }

        public override void sendReadCommand()
        {
            if (deviceType == DeviceType.Grimm_dust)
            {
                SendAlarmCommand(2);
                waitForResponseList[2].WaitOne(2 * maximumResponseTime);
                messageSendOnSerialBytesAwaitingResponse(Encoding.UTF8.GetBytes("Z"), 1);

                //messageSendOnSerial("Z");
            }


        }

        public void SendAlarmCommand(byte i)
        {
            //W LIST ALL, W LIST HEX
            messageSendOnSerialBytesAwaitingResponse(Encoding.UTF8.GetBytes("E"), i);

        }

        public override void parseMessage()
        {
            // For Grimm 
            /*
            Z

            Mean PM10: 68.9; PM2.5:    12.5; PM1: 0.0

            V: 0.0003 m3

            N1,  492    71     0


            E
            Error :   0 

            */
            try
            {
                string deviceResponse = Encoding.UTF8.GetString(messageReceived.ToArray());
                int tmpAlarm = 0;

                // deviceResponse = "02/28/18 15:00,   5341, 0.226, 0.002,   0.0, 0.000,    35, 0.000,  21.7,0,0,0,0,0,0,0,0,1,0,0,0";
                //statusLog(deviceResponse);

                if (writeMode)
                {
                    sw.WriteLine(deviceResponse);
                    sw.Flush();
                }
                if (deviceResponse.Contains("Mean"))
                {
                    string[] tmpStrData = deviceResponse.Split(';');
                    string[] value = new string[] { };
                    //string[] num = Regex.Split(tmpStrData[0], @"\D+");
                    for (int i = 0; i < tmpStrData.Length; i++)
                    {
                        value = tmpStrData[i].Split(':');
                        tmpStrData[i] = value[1];
                        //value = Regex.Split(tmpStrData[i], @"\s+"); 
                    }

                    if (tmpStrData.Length > 0)
                    {
                        //polutions[0].concentration = System.Convert.ToSingle(tmpStrData[1]);

                        if (polutions.Count > 1)
                        {
                            foreach (var pol in polutions)
                            {
                                if (!float.TryParse(tmpStrData[pol.mapperID].Trim(), out pol.concentration))
                                    errorLog(string.Format("Device: {0} ParseMessage: Could not Convert concentration data : {1} to float", deviceName, tmpStrData[pol.mapperID]));
                            }
                        }
                        else
                        {

                            if (!float.TryParse(tmpStrData[0].Trim(), out polutions[0].concentration))
                                errorLog(string.Format("Device: {0} ParseMessage: Could not Convert concentration data : {1} to float", deviceName, tmpStrData[2]));
                        }

                        timeLastMessageRecieved = DateTime.Now;
                        waitForResponseList[1].Set();
                        workingStatus = true;

                    }
                }
                else if (deviceResponse.Contains("Error"))
                {
                    string[] tmpStrData = deviceResponse.Split(':');
                    if (Convert.ToInt32(tmpStrData[1].Trim()) != 0)
                        errorLog(string.Format("Device: {0} ParseMessage: deviceResponse  : {1} ", deviceName, deviceResponse));

                    for (int k = 0; k < polutions.Count; k++)
                    {
                        int tempAlarm = Convert.ToInt32(tmpStrData[1].Trim());
                            polutions[k].alarmValue = Convert.ToInt32(tmpStrData[1]);
                            errorLog(string.Format("Device: {0} ParseMessage: ValueError  : {1}", deviceName, Convert.ToInt32(tmpStrData[1])));
                    }

                    waitForResponseList[2].Set();
                    workingStatus = true;
                }
                
            }
            catch (Exception ex)
            {
                errorLog("[Grimm Analyzer ParseMessage] - " + ex.Message);
                messageReceived.Clear();
            }
        }

        internal override void Disconnect()
        {
            base.Disconnect();

            if (conSerial.IsOpen)
                conSerial.Close();
        }
    }
}
