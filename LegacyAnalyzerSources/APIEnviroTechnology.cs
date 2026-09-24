using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using System.Threading;

namespace SadraAQMS.Analyzers
{
    class APIEnviroTechnology : deviceAnalyzer
    {
        // For debug 
        //System.IO.StreamWriter swRemoteESA;

        #region Variable & Fields

        int swicthProtocol = 1; // 1: Data, 2: Graphics 3: Data in Float Format

        static Dictionary<DeviceType, string> EnviroPollutionName = new Dictionary<DeviceType, string>()
            {
                {DeviceType.Enviro_CO, "CO"},//CO12
                {DeviceType.Enviro_SO2,"SO2"},
                {DeviceType.Enviro_O3, "O3"},
                {DeviceType.Enviro_NOx,"NO,NO2,NOX"},
                {DeviceType.Enviro_NOx_O3,"NO,NO2,NOX,O3"}
            };

        public class MessageESAProtocol
        {
            public byte[] deviceID = new byte[4];
            public byte[] messageID = new byte[2];
            public byte length;
            public List<byte> data { get; set; }
            public byte[] checkSum = new byte[2];
            public byte chks;

            public MessageESAProtocol(byte[] dvcID, byte[] msgID, byte[] msgData)
            {
                deviceID = dvcID;
                messageID = msgID;
                data = msgData.ToList<byte>();
                length = 0;
            }

            public MessageESAProtocol()
            {
                deviceID = new byte[4];
                messageID = new byte[2];
                data = new List<byte>();
                length = 0;
            }
        }

        MessageESAProtocol messageReceivedESA = new MessageESAProtocol();

        #endregion Variable & Fields

        public APIEnviroTechnology(DeviceType dvType, string devID) // Constructor 
        {
            //this.company = DeviceCompany.EnviroTechnology;
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            //this.DeviceID = ESA_stringIDsSuggested[dvType];
            //swRemoteESA = new System.IO.StreamWriter(".\\Logs\\ESARemote_" + deviceName + DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2") + ".txt");

            //ClearScreen(2,true,true);
        }
        public APIEnviroTechnology(DeviceType dvType, string devID, string devName, string ignorableErrors)
        {
            //this.company = DeviceCompany.Ecotech;
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            this.DeviceName = devName;
            int.TryParse(ignorableErrors, out IgnorableErrors);
            //this.DeviceID = ESA_stringIDsSuggested[dvType];
            //swRemoteESA = new System.IO.StreamWriter(".\\Logs\\ESARemote_" + deviceName + DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2") + ".txt");

            //ClearScreen(2,true,true);
        }

        #region Send Data on Serial

        // Method for reading gas concentration reported by device

        bool DataRecived = false;
        public override void sendReadCommand()
        {
            if (deviceType != DeviceType.Enviro_NOx && deviceType != DeviceType.Enviro_NOx_O3)
            {
                SendAlarmCommand(2);
                waitForResponseList[2].WaitOne(2 * maximumResponseTime);

                messageSendOnSerialBytesAwaitingResponse(Encoding.UTF8.GetBytes(string.Format("T {0:000} LIST {1}\r", DeviceID, EnviroPollutionName[deviceType])), 1);
                //waitForResponseList[1].WaitOne(maximumResponseTime);
            }
            else //DeviceType.Enviro_NOx
            {
                string[] tmpNames = EnviroPollutionName[deviceType].Split(',');

                SendAlarmCommand(2);
                waitForResponseList[2].WaitOne(maximumResponseTime);

                for (byte i = 0; i < Convert.ToByte(tmpNames.Length); i++)
                {
                    polutions[i].concentration = -10000;

                    messageSendOnSerialBytesAwaitingResponse(Encoding.UTF8.GetBytes(string.Format("T {0:000} LIST {1}\r", DeviceID, tmpNames[i])), (byte)(i + 10));

                    waitForResponseList[(byte)(i + 10)].WaitOne(maximumResponseTime);
                }

                if (DataRecived && polutions.Count(m => m.concentration > 0) > -5000)
                    waitForResponseList[1].Set();

                //foreach (string name in tmpNames)
                //{
                //    messageSendOnSerialBytesAwaitingResponse(Encoding.UTF8.GetBytes(string.Format("T {0:000} LIST {1}\r", DeviceID, name)), 2);
                //    if (waitForResponseList[2].WaitOne(maximumResponseTime))
                //    {
                //        SendAlarmCommand();
                //    }
                //}
            }
        }

        public void SendAlarmCommand(byte i)
        {
            //W LIST ALL, W LIST HEX
            messageSendOnSerialBytesAwaitingResponse(Encoding.UTF8.GetBytes("W LIST HEX\r\n"), i);

        }

        #endregion Send Data on Serial

        #region Parse Incoming Data

        public override void parseMessage()
        {
            try
            {
                timeLastMessageRecieved = DateTime.Now;
                string deviceResponse = Encoding.UTF8.GetString(messageReceived.ToArray());
                //deviceResponse = "T  301:12:12  0200  NO2= 38.0 PPB\r\n";

                if (writeMode)
                {
                    sw.WriteLine(string.Format("Message Recieved at: {0} [{1}]", DateTime.Now, deviceResponse));
                    sw.Flush();
                }

                if (deviceResponse.Contains(":"))
                {
                    string[] tmpSData = deviceResponse.Split(' ');
                    List<string> tmpStrData = new List<string>();
                    for (int k = 0; k < tmpSData.Length; k++)
                    {
                        if (tmpSData[k].Trim() != "")
                            tmpStrData.Add(tmpSData[k]);
                    }

                    if (tmpStrData.Count > 4)
                    {
                        if (tmpStrData[0] == "W")
                        {
                            var tmAlarm = 0;
                            if (!int.TryParse(tmpStrData[3].Substring(2, 8), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out tmAlarm))
                                errorLog(string.Format("Device : {0} [parseMessage] can not parse Alarm {1}", deviceName, tmpStrData[3].Substring(2, 8)));
                            foreach(polutionType pol in polutions)
                            {
                                pol.alarmValue= tmAlarm;
                            }
                            //alarmValue = tmAlarm;

                            traceLog("[APIEnviro] [ParseMessage] Get Alarm: " + tmpStrData[3]);

                            waitForResponseList[2].Set();
                        }

                        if (deviceType == DeviceType.Enviro_NOx || deviceType == DeviceType.Enviro_NOx_O3)
                        {
                            if (tmpStrData[3].Contains("NO="))
                            {
                                //string[] tmp = tmpStrData[3].Split('=');
                                //polutions[0].concentration = System.Convert.ToSingle(tmpStrData[4]);
                                if (!float.TryParse(tmpStrData[4], out polutions[0].concentration))
                                    if (!float.TryParse(tmpStrData[3].Split('=')[1], out polutions[0].concentration))
                                        errorLog(string.Format("Device : {0} [parseMessage] can not parse concentration {1} for Polution NO", deviceName, tmpStrData[4]));


                                traceLog("[APIEnviro] [ParseMessage] Get Polution No: " + tmpStrData[4]);
                                waitForResponseList[10].Set();
                                DataRecived = true;

                             
                            }
                            else if (tmpStrData[3].Contains("NO2="))
                            {
                                //string[] tmp = tmpStrData[3].Split('=');
                                //polutions[1].concentration = System.Convert.ToSingle(tmpStrData[4]);
                                if (!float.TryParse(tmpStrData[4], out polutions[1].concentration))
                                    if (!float.TryParse(tmpStrData[3].Split('=')[1], out polutions[1].concentration))
                                        errorLog(string.Format("Device : {0} [parseMessage] can not parse concentration {1} for Polution NO2", deviceName, tmpStrData[4]));

                                traceLog("[APIEnviro] [ParseMessage] Get Polution No2: " + tmpStrData[4]);
                                waitForResponseList[11].Set();
                                DataRecived = true;

                            
                            }
                            else if (tmpStrData[3].Contains("NOX="))
                            {
                                //string[] tmp = tmpStrData[3].Split('=');
                                //polutions[2].concentration = System.Convert.ToSingle(tmpStrData[4]);
                                if (!float.TryParse(tmpStrData[4], out polutions[2].concentration))
                                    if (!float.TryParse(tmpStrData[3].Split('=')[1], out polutions[2].concentration))
                                        errorLog(string.Format("Device : {0} [parseMessage] can not parse concentration {1} for Polution NOX", deviceName, tmpStrData[4]));

                                traceLog("[APIEnviro] [ParseMessage] Get Polution Nox: " + tmpStrData[4]);
                                waitForResponseList[12].Set();
                                DataRecived = true;
                            }

                            else if (tmpStrData[3].Contains("O3="))
                            {
                                //string[] tmp = tmpStrData[3].Split('=');
                                //polutions[2].concentration = System.Convert.ToSingle(tmpStrData[4]);
                                if (!float.TryParse(tmpStrData[4], out polutions[3].concentration))
                                    if (!float.TryParse(tmpStrData[3].Split('=')[1], out polutions[3].concentration))
                                        errorLog(string.Format("Device : {0} [parseMessage] can not parse concentration {1} for Polution NOX", deviceName, tmpStrData[4]));

                                traceLog("[APIEnviro] [ParseMessage] Get Polution Nox: " + tmpStrData[4]);
                                waitForResponseList[13].Set();
                                DataRecived = true;
                            }

                        }
                        else // Not  DeviceType.Enviro_NOx
                        {
                            //string[] tmp = tmpStrData[3].Split('=');

                            string[] tmp = deviceResponse.Split('=');
                            string tmpNum = tmp[1].Trim().Split(' ')[0];
                            if (!float.TryParse(tmpNum, out polutions[0].concentration))
                                errorLog(string.Format("[APIEnviro] [parseMessage] can not parse concentration {0} for Polution {1}", tmp[1], EnviroPollutionName[deviceType]));

                            traceLog(string.Format("[APIEnviro] [ParseMessage] Get Polution {0}: {1}", EnviroPollutionName[deviceType], tmp[1]));
                            waitForResponseList[1].Set();
                        }
                    }
                    workingStatus = true;
                }

            }
            catch (Exception ex)
            {
                errorLog("[APIEnviro parseMessage] device: " + deviceName + " - " + ex.Message);
            }
        }

        #endregion Parse Incoming Data

        public override byte getStatus(int alarm)
        {
            byte state = base.getStatus(alarm);

            // ******* check the protocol ***************************

            if ((/*_alarmValue*/alarm & (1 << 14)) > 0)
                state = 3; // Span

            if ((/*_alarmValue*/alarm & (1 << 13)) > 0)
                state = 2; // Zero

            return state;
        }

        internal override void Disconnect()
        {
            base.Disconnect();

            if (conSerial.IsOpen)
                conSerial.Close();
        }
    }
}