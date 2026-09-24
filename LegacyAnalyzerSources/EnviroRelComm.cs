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
    public class EnviroRelComm : deviceAnalyzer
    {
        // For debug 
        //System.IO.StreamWriter swRemoteESA;

        #region Variable & Fields

        int swicthProtocol = 1; // 1: Data, 2: Graphics 3: Data in Float Format

        //static Dictionary<DeviceType, string> EnviroPollutionName = new Dictionary<DeviceType, string>()
        //    {
        //        {DeviceType.Enviro_CO, "CO"},//CO12
        //        {DeviceType.Enviro_SO2,"SO2"},
        //        {DeviceType.Enviro_O3, "O3"},
        //        {DeviceType.Enviro_NOx,"NO,NO2,NOX"}
        //    };

        private Dictionary<int, string> PolutionNames = new Dictionary<int, string>()
        {
            {1,"O3,400"},
            {2,"CO,300"},
            {3,"NO,200"},
            {4,"NO2,200"},
            {5,"NOX,200"},
            {6,"SO2,100"},
            {7, "PM 2.5"},
            {8,"PM 10"}
        };

        int PolutionId;

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

        public EnviroRelComm(DeviceType dvType, string devID)
        {
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
        }
        public EnviroRelComm(DeviceType dvType, string devID, string devName, string ignorableErrors)
        {
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            this.DeviceName = devName;
            this.IgnorableErrors = int.Parse(ignorableErrors);
        }

        #region Send Data on Serial

        // Method for reading gas concentration reported by device

        bool DataRecived = false;
        public override void sendReadCommand()
        {
            traceLog("SendRead Command polutions " + string.Join(",", polutions.Select(m => m.ID)));

            DataRecived = false;
                 
            foreach (var p in polutions)
            {
                //if (polutions.Count == 4)//for read no and o3 with together
                //{

                //}
                traceLog(string.Format("SendRead Command for polutionID={0} mapperID={1}", p.ID, p.mapperID));

                PolutionId = p.ID;
                p.concentration = -10000;

                // Open Related COM
                messageSendOnSerialBytesAwaitingResponse(new byte[] { 4, Convert.ToByte(48 + p.mapperID) }, 52);
                waitForResponseList[52].WaitOne(50);
                messageReceived.Clear();

                if (p.ID > 6)  // for BAAM Pm10 -> 8 & PM2.5 -> 7
                {
                    messageSendOnSerialBytesAwaitingResponse(Encoding.UTF8.GetBytes("\r\n\r\n\r\n\r\n64"), Convert.ToByte(10 + p.ID));
                    waitForResponseList[Convert.ToByte(10 + p.ID)].WaitOne(2000);
                    continue;
                }
                
                SendAlarmCommand(Convert.ToByte(2 + p.ID));
                waitForResponseList[Convert.ToByte(2 + p.ID)].WaitOne(2000);

                var pol = PolutionNames[p.ID].Split(',');

                messageSendOnSerialBytesAwaitingResponse(Encoding.UTF8.GetBytes(string.Format("T {0} LIST {1}\r", pol[1], pol[0])), Convert.ToByte(10 + p.ID));
                waitForResponseList[Convert.ToByte(10 + p.ID)].WaitOne(1000);

            }

            if (DataRecived && polutions.Count(m => m.concentration > 0) > 0)
                waitForResponseList[1].Set();
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
                    if (PolutionId > 6)
                        ParseBAM(deviceResponse);
                    else
                        ParseEnviro(deviceResponse);

                    workingStatus = true;
                }

            }
            catch (Exception ex)
            {
                errorLog("[APIEnviro parseMessage] device: " + deviceName + " - " + ex.Message);
            }
        }

        private void ParseBAM(string deviceResponse)
        {
            string[] tmpStrData = deviceResponse.Split(',');

            if (tmpStrData.Length > 0)
            {
                //polutions[0].concentration = System.Convert.ToSingle(tmpStrData[1]);

                var polIndex = polutions.FindIndex(m => m.ID == PolutionId);

                traceLog("deviceResponse:" + deviceResponse + "PolutionId: " + PolutionId + "Pol Index: " + polIndex);

                if (!float.TryParse(tmpStrData[1], out polutions[polIndex].concentration))
                    errorLog(string.Format("Device: {0} ParseMessage: Could not Convert concentration data : {1} to float", deviceName, tmpStrData[1]));

                if (tmpStrData.Length > 20)
                {
                    int tmpAlarm = 0;
                    int tmpData = 0;
                    for (int ind = 9; ind < 21; ind++)
                    {
                        if (!int.TryParse(tmpStrData[ind], out tmpData))
                            errorLog(string.Format("Device: {0} ParseMessage: Could not Convert Alarm data[{2}] : {1} to int", deviceName, tmpStrData[ind], ind));

                        tmpAlarm = (tmpAlarm | tmpData) << 1;

                        //tmpAlarm = (tmpAlarm | System.Convert.ToInt32(tmpStrData[ind])) << 1;
                    }
                    polutions[polIndex].alarmValue = tmpAlarm >> 1;
                  //  alarmValue = tmpAlarm >> 1;
                }

                waitForResponseList[Convert.ToByte(10 + PolutionId)].Set();
                DataRecived = true;
                workingStatus = true;
            }
        }

        private void ParseEnviro(string deviceResponse)
        {
            string[] tmpSData = deviceResponse.Split(' ');
            List<string> tmpStrData = new List<string>();
            for (int k = 0; k < tmpSData.Length; k++)
            {
                if (tmpSData[k].Trim() != "")
                    tmpStrData.Add(tmpSData[k]);
            }

            traceLog("ParseEnviro tmpStrData: " + string.Join(",", tmpStrData) + "PolutionId " + PolutionId);

            if (tmpStrData.Count > 3)
            {
                if (tmpStrData[0] == "W")
                {
                    var tmAlarm = 0;

                    if (!int.TryParse(tmpStrData[3].Substring(2, 8), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture,  out tmAlarm))
                        errorLog(string.Format("Device : {0} [parseMessage] can not parse Alarm {1}", deviceName, tmpStrData[3].Substring(2, 8)));
                   // alarmValue = tmAlarm & 0xFFF8;
                    var polIndex = polutions.FindIndex(m => m.ID == PolutionId);
                    polutions[polIndex].alarmValue= tmAlarm & 0xFFF8;

                    traceLog("[APIEnviro] [ParseMessage] Get Alarm: " + tmpStrData[3]);

                    waitForResponseList[Convert.ToByte(2 + PolutionId)].Set();
                }
                else
                {
                    string[] tmp = deviceResponse.Split('=');

                    if (tmp.Count() > 1)
                        foreach (var t in tmp[1].Split(' '))
                        {
                            if (!string.IsNullOrEmpty(t))
                            {
                                //trace if found
                                var polIndex = polutions.FindIndex(m => m.ID == PolutionId);

                                traceLog("deviceResponse:" + deviceResponse + "PolutionId: " + PolutionId + "Pol Index: " + polIndex);

                                if (!float.TryParse(t, out polutions[polIndex].concentration))
                                    errorLog(string.Format("[RelComm] [parseMessage] can not parse concentration {0} for Polution {1}", t, PolutionNames[PolutionId]));
                                break;
                            }
                        }

                    waitForResponseList[Convert.ToByte(10 + PolutionId)].Set();
                    DataRecived = true;
                }
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