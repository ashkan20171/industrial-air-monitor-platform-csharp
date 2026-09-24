using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using FTD2XXSADRA;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Management;
using System.IO;

namespace SadraAQMS.Analyzers
{
    // Class for definition of Gas and Particle Analyzers
    public class EcotechAnalyzer : deviceAnalyzer                                
    {
        #region Variable & Fields

        FTDIDevice EcotechFtdi = new FTDIDevice();

        public byte MultiDropID { get; set; }

        //enum menuStageType { unknown, Quick_Menu, Main_Page, Main_Menu, Calibration_Menu, Selected_Calibration, ended };
        enum menuStageType { unknown, Quick_Menu, Selected_Calibration, ended };
        static Dictionary<string, byte[]> checkPixelBytes = new Dictionary<string, byte[]>()
        {
            {"Quick_Menu", new byte[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 51, 30, 60, 103, 0, 99, 127, 99, 51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 51, 51, 12, 102, 102, 0, 119, 70, 103, 51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 51, 51, 12, 3, 54, 0, 127, 22, 111, 51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 51, 51, 12, 3, 30, 0, 107, 30, 123, 51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 59, 51, 12, 3, 54, 0, 99, 22, 115, 51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 51, 12, 102, 102, 0, 99, 70, 99, 51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 56, 63, 30, 60, 103, 0, 99, 127, 99, 63, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,                                                                                                                                                                  0 }},
            {"ZeroCalib_Selected",  new byte[] {255, 230, 225, 228, 225, 255, 252, 225, 243, 241, 193, 228, 225, 193, 225, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 95, 56, 12, 255, 243, 204, 201, 204, 255, 252, 207, 243, 243, 153, 201, 207, 243, 204, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 95, 124, 12, 255, 185, 192, 201, 204, 255, 252, 193, 243, 243, 153, 201, 193, 243, 192, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 95, 0, 12, 255, 156, 252, 249, 204, 255, 153, 204, 243, 243, 153, 249, 204, 211, 252, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 95, 0, 12, 255, 128, 225, 240, 225, 255, 195, 145, 225, 225, 194, 240, 145, 231, 225, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 95, 0, 12, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 223, 255, 15, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 159, 255, 15, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 16, 0, 0 }},
            {"SpanCalib_Selected", new byte[] {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 16, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 16, 0, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 31, 0, 0, 255, 225, 255, 255, 255, 255, 195, 255, 241, 243, 248, 255, 255, 247, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 31, 0, 0, 255, 204, 255, 255, 255, 255, 153, 255, 243, 255, 249, 255, 255, 243, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 31, 0, 0, 255, 248, 196, 225, 224, 255, 252, 225, 243, 241, 193, 228, 225, 193, 225, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 31, 0, 0, 255, 227, 153, 207, 204, 255, 252, 207, 243, 243, 153, 201, 207, 243, 204, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 31, 0, 0, 255, 199, 153, 193, 204, 255, 252, 193, 243, 243, 153, 201, 193, 243, 192, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 31, 0, 0 }}
        };

        static Dictionary<DeviceType, string> EcotechUSBName = new Dictionary<DeviceType, string>()
        {
            {DeviceType.Ecotech_CO,  "Serinus S30 Analyser"}, //30
            {DeviceType.Ecotech_NOx,                                                                                                                                                                                                                                                                                                                                                                                        "Serinus S40 Analyser"}, //40
            {DeviceType.Ecotech_O3,  "Serinus S10 Analyser"}, //10
            {DeviceType.Ecotech_SO2, "Serinus S50 Analyser"}, //50
            {DeviceType.Ecotech_CO2A, "Serinus S50 Analyser"}, //50
            {DeviceType.Ecotech55_H2S,"Serinus S55 Analyser"}, //55
            {DeviceType.Ecotech98_CO, "9830"}, //30
            {DeviceType.Ecotech98_NOx,"9841"}, //41
            {DeviceType.Ecotech9842_Nx,"9842"}, //42
            {DeviceType.Ecotech9850_SO2,"9850"}, //50
            {DeviceType.Ecotech9852_SO2,"9852"}, //52
            {DeviceType.Ecotech98_O3 ,"9810"},  //41
            
        };

        static string[] calibTypeStr = new string[3] { "", "ZeroCalib_Selected", "SpanCalib_Selected" };

        public enum MessageAdvanceProtocolStatusType
        {
            UNINIT = 0, GOT_SYNC, GOT_DeviceID, GOT_MessgaeID, GOT_DATA2, GOT_LEN, GOT_PAYLOAD, GOT_CHECKSUM
        };
        public MessageAdvanceProtocolStatusType statusMsgRecv;
        private byte messageIndexRecieve;

        public class MessageAdvanceProtocol
        {
            public byte ID;
            public byte deviceID = 0;
            public byte length;
            List<byte> _data = new List<byte>();
            public List<byte> data
            {
                get
                {
                    return _data;
                }
                set
                {
                    _data = value;
                    length = (byte)_data.Count;
                }
            }

            public byte data2 = 3;
            public byte checkSum;

            public MessageAdvanceProtocol(byte msgID, byte dvcID, byte[] msgData)
            {
                ID = msgID;
                deviceID = dvcID;
                data = msgData.ToList<byte>();
            }

            public MessageAdvanceProtocol()
            {
                ID = 0;
                deviceID = 0;
                data = new List<byte>();
            }
        }

        MessageAdvanceProtocol messageReceivedAdvanceProtocol = new MessageAdvanceProtocol();
        byte recentMsgIdent = 0;

        List<byte> buffer_data = new List<byte>();

        #endregion Variable & Fields

        // Constructor
        public EcotechAnalyzer(DeviceType dvType, string devID)
        {
            //this.company = DeviceCompany.Ecotech;
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();

            initializeStrData();

            try
            {
                this.MultiDropID = Convert.ToByte(devID);
            }
            catch
            {
                errorLog("[Initialize of Ecotech] - Wrong Format of Device ID");
            }
            //_alarmValue = 0;
        }

        public EcotechAnalyzer(DeviceType dvType, string devID, string devName, string ignorableErrors)
        {
           //this.company = DeviceCompany.Ecotech;
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();

            if (devName.Contains("$"))
            {
                var items = devName.Split('$');
                this.DeviceName = items[0];
                EcotechUSBName[dvType] = items[1];
            }
            else
                this.DeviceName = devName;
            int.TryParse(ignorableErrors, out IgnorableErrors);

            initializeStrData();

            try
            {
                this.MultiDropID = Convert.ToByte(devID);
            }
            catch
            {
                errorLog("[Initialize of Ecotech] - Wrong Format of Device ID");
            }
            //_alarmValue = 0;
        }

        #region Send Data on Serial
        private byte computeAdvanceProtocolChecksum(List<byte> msg)
        {
            byte chksum = 1;
            for (int k = 0; k < msg.Count; k++)
                chksum ^= msg[k];
            return chksum;
        }

        public void sendAdvanceProtocolMessage(MessageAdvanceProtocol msg, byte msgIdentifier)
        {
            List<byte> tmpMsg = new List<byte>();
            tmpMsg.Add(2);
            tmpMsg.Add(msg.deviceID);
            tmpMsg.Add(msg.ID);
            tmpMsg.Add(msg.data2);
            tmpMsg.Add(msg.length);
            tmpMsg.AddRange(msg.data);
            tmpMsg.Add(computeAdvanceProtocolChecksum(tmpMsg));
            tmpMsg.Add(4);

            if (this.portCOM == "USB") // FDTI in ecotech Devices
            {
                if (msgIdentifier == 0) // Zero no waitHandle
                    messageSendBytesUSB(tmpMsg.ToArray());
                else
                    messageSendBytesUSBAwaitingResponse(tmpMsg.ToArray(), msgIdentifier);
            }
            else // Normal Com Port
            {
                messageSendOnSerialBytesAwaitingResponse(tmpMsg.ToArray(), msgIdentifier);
            }
        }

        // Method for reading gas concentration reported by device
        public override void sendReadCommand()
        {
            if ((int)deviceType < 5 || portCOM.Trim() == "USB")  //Advance Protocol
            {
                MessageAdvanceProtocol msg;
                byte[] tableRowNo = new byte[polutions.Count + 1];

                for (int i = 0; i < polutions.Count; i++)
                {
                    tableRowNo[i] = (byte)(50 + i);
                }
                tableRowNo[polutions.Count] = 83;
                msg = new MessageAdvanceProtocol(1, MultiDropID, tableRowNo);

                //if (polutions.Count == 1)//(deviceType != DeviceType.Ecotech_NOx && deviceType != DeviceType.Ecotech9852_SO2 && deviceType != DeviceType.Ecotech98_NOx)
                //{
                //    msg = new MessageAdvanceProtocol(1, MultiDropID, new byte[] { 50, 83 });
                //}
                //else //if (polutions.Count == 3)
                //{
                //    msg = new MessageAdvanceProtocol(1, MultiDropID, new byte[] { 50, 51, 52, 83 });
                //}
                sendAdvanceProtocolMessage(msg, 1);
            }
            else   // Ecotech 9800 Commands
            {
                //string tmpCMD = "DCONC" + MultiDropID.ToString("D3") + "\r\n";
                if (remoteMode)
                    return;

                string tmpCMD = "DCONC," + MultiDropID.ToString("000") + "\r\n";
                messageSendOnSerialBytesAwaitingResponse(Encoding.UTF8.GetBytes(tmpCMD), 1); // read Data

            }
        }
        #endregion Send Data on Serial

        #region Parse Incoming Data
        protected override void ReadDataManual()
        {
            EcotechFtdi.ReadBytesFTDI(); // for USB read
        }

        private float getFloatNumAdvanceProtocol(List<byte> responseBytes, int index)
        {
            try
            {
                return BitConverter.ToSingle(new byte[] { responseBytes[index + 3], responseBytes[index + 2], responseBytes[index + 1], responseBytes[index] }, 0);
            }
            catch (Exception ex)
            {
                errorLog("Problem in change value to float in device " + deviceName + " : " + ex.Message);
                return -1;
            }
        }

        internal override void initialize_Connect()
        {
            if (portCOM.Contains("USB"))
            {
                EcotechFtdi.disconnect();
                statusLog(EcotechFtdi.GetDeviceInfo());
                isConnected = EcotechFtdi.connect(EcotechUSBName[deviceType], (uint)(baudrate));
                EcotechFtdi.parseByte += new FTD2XXSADRA.FTDIDevice.parseEventHandler(parseByte);
            }
            
            base.initialize_Connect();
        }

        internal void ClearStrData()
        {
            StrData = new List<string>();
            StrStyle = new List<string>();
        }

        private List<byte> getByteFromFloat(float input)
        {
            List<byte> tmp = BitConverter.GetBytes(input).ToList<byte>();
            tmp.Reverse();
            return tmp;
        }

        public override void parseByte(byte data)
        {

            timeLastByteRecieved = DateTime.Now;
            try
            {

                if ((int)deviceType < 5 || portCOM.Trim() == "USB") // Advance Protocol
                {
                    parseByteAdvanceProtocol(data);
                }
                else  // Ecotech 9800 Commands
                {
                    timeLastByteRecieved = DateTime.Now;

                    if (!remoteMode)
                    {
                        messageReceived.Add(data);
                        if ((data == 10 && messageReceived.Count >= 2 && messageReceived[messageReceived.Count - 2] == 13)) // {13,10}: CR and LF for end of message (most string-type msg)
                        {                                                                                   // {15}: End of message for GCC995 string-type response
                            parseMessage();
                            messageReceived.Clear();
                        }
                        if (messageReceived.Count > 500)
                            messageReceived.Clear();

                        return;
                    }
                    // Check for Remote Response ------------------------------------------------------------------------------

                    //messageReceived.RemoveAt(messageReceived.Count - 1);
                    RemoteScreenParseMessage_Ecotech9800_Com(data);
                    //signalR send (buffer + timer)
                    buffer_data.Add(data);
                    if (buffer_data.Count > 50)
                    {
                        OnSendingDataSignalR(deviceType, buffer_data);
                        buffer_data.Clear();
                    }
                    messageReceived.Clear();
                    //messageReceived.Add(data);

                }
            }
            catch (Exception ex)
            {
                errorLog(string.Format("ParseByte Device: {3}  message: {0} data:{2} ErrorMessage:{1}", Encoding.UTF8.GetString(messageReceived.ToArray()), ex.Message, data, deviceType.ToString()));
            }
        }

        public void parseByteAdvanceProtocol(byte data)
        {
            try
            {
                if (statusMsgRecv < MessageAdvanceProtocolStatusType.GOT_PAYLOAD)
                    messageReceivedAdvanceProtocol.checkSum ^= data;

                switch (statusMsgRecv)
                {
                    case MessageAdvanceProtocolStatusType.UNINIT:
                        if (data == 0x02)
                        {
                            statusMsgRecv++;
                            messageReceivedAdvanceProtocol.checkSum = 0x02 ^ 0x01;
                        }

                        break;
                    case MessageAdvanceProtocolStatusType.GOT_SYNC:
                        messageReceivedAdvanceProtocol.deviceID = data;
                        statusMsgRecv++;

                        break;
                    case MessageAdvanceProtocolStatusType.GOT_DeviceID:
                        messageReceivedAdvanceProtocol.ID = data;
                        statusMsgRecv++;

                        break;
                    case MessageAdvanceProtocolStatusType.GOT_MessgaeID:
                        messageReceivedAdvanceProtocol.data2 = data; // usually 3
                        statusMsgRecv++;

                        break;
                    case MessageAdvanceProtocolStatusType.GOT_DATA2:
                        messageReceivedAdvanceProtocol.data = new List<byte>();
                        messageReceivedAdvanceProtocol.length = data;
                        messageIndexRecieve = 0;
                        if (messageReceivedAdvanceProtocol.length == 0)
                            statusMsgRecv += 2;
                        else
                            statusMsgRecv++;

                        break;
                    case MessageAdvanceProtocolStatusType.GOT_LEN:
                        messageReceivedAdvanceProtocol.data.Add(data);
                        messageIndexRecieve++;
                        if (messageIndexRecieve >= messageReceivedAdvanceProtocol.length)
                        {
                            statusMsgRecv++;
                        }
                        break;
                    case MessageAdvanceProtocolStatusType.GOT_PAYLOAD:
                        statusMsgRecv++;
                        if (data != messageReceivedAdvanceProtocol.checkSum)
                        {
                            goto error;
                        }
                        else
                        {
                            parseMessage();
                        }
                        break;
                    case MessageAdvanceProtocolStatusType.GOT_CHECKSUM:
                        goto restart;
                        //break;
                }
                return;
                error:
                restart: statusMsgRecv = MessageAdvanceProtocolStatusType.UNINIT;
                return;

            }
            catch (Exception ex)
            {
                errorLog(string.Format("ParseByte Device: {3} Advance Protocol  message: {0} data:{2} ErrorMessage:{1}", Encoding.UTF8.GetString(messageReceived.ToArray()), ex.Message, data, deviceType.ToString()));
            }
        }


        public override void parseMessage()
        {
            timeLastMessageRecieved = DateTime.Now;


            if ((int)deviceType < 5 || portCOM.Trim() == "USB") // Advance Ecotech Protocol
            {
                if (writeMode)
                {
                    //sw.Write(" Ecotech :");
                    sw.Write(writePath);
                    string fullPath = ((FileStream)(sw.BaseStream)).Name;
                    sw.WriteLine(BitConverter.ToString(messageReceivedAdvanceProtocol.data.ToArray()));
                   // statusLog("Write Ecotechhhhhhhhhhhhhhhhhhhh");
                    statusLog(fullPath);

                    sw.Flush();
                }
                //statusLog("[TestMsg1] " + deviceType.ToString()+ "Befor Switch:" + "Msg:" + BitConverter.ToString(messageReceivedAdvanceProtocol.data.ToArray()));
                switch (messageReceivedAdvanceProtocol.ID)
                {
                    case 1: //Read Data Concentration
                        try
                        {
                            //statusLog("[TestMsg2] " + deviceType.ToString() + "after Switch");
                            if (polutions.Count == 1)// (deviceType != DeviceType.Ecotech_NOx && deviceType != DeviceType.Ecotech9852_SO2 && deviceType != DeviceType.Ecotech98_NOx)
                            {
                                //statusLog("[TestMsg3] " + deviceType.ToString() + "after Switch");
                                polutions[0].concentration = getFloatNumAdvanceProtocol(messageReceivedAdvanceProtocol.data, 1);
                                polutions[0].alarmValue = (int)(getFloatNumAdvanceProtocol(messageReceivedAdvanceProtocol.data, 6)) & 0xFFF8;
                                //alarmValue = (int)(getFloatNumAdvanceProtocol(messageReceivedAdvanceProtocol.data, 6)) & 0xFFF8;
                            }
                            else if (polutions.Count == 2)// echotech9842_Nx
                            {
                                polutions[0].concentration = getFloatNumAdvanceProtocol(messageReceivedAdvanceProtocol.data, 1);
                                polutions[1].concentration = getFloatNumAdvanceProtocol(messageReceivedAdvanceProtocol.data, 6);
                                polutions[0].alarmValue= (int)(getFloatNumAdvanceProtocol(messageReceivedAdvanceProtocol.data, 11)) & 0xFFF8;
                                polutions[1].alarmValue = polutions[0].alarmValue;
                                //alarmValue = (int)(getFloatNumAdvanceProtocol(messageReceivedAdvanceProtocol.data, 11)) & 0xFFF8;
                            }
                            else
                            {
                                polutions[0].concentration = getFloatNumAdvanceProtocol(messageReceivedAdvanceProtocol.data, 1);
                                polutions[1].concentration = getFloatNumAdvanceProtocol(messageReceivedAdvanceProtocol.data, 6);
                                polutions[2].concentration = getFloatNumAdvanceProtocol(messageReceivedAdvanceProtocol.data, 11);
                                polutions[0].alarmValue= (int)(getFloatNumAdvanceProtocol(messageReceivedAdvanceProtocol.data, 16)) & 0xFFF8;
                                polutions[1].alarmValue = polutions[0].alarmValue;
                                polutions[2].alarmValue= polutions[0].alarmValue;
                                //alarmValue = (int)(getFloatNumAdvanceProtocol(messageReceivedAdvanceProtocol.data, 16)) & 0xFFF8;
                            }
                            waitForResponseList[1].Set();
                            workingStatus = true;
                        }
                        catch (Exception ex)
                        {
                            errorLog(string.Format("Advance Protocol Parse  message Device Name: {2} data:{0} ErrorMessage:{1} ", BitConverter.ToString(messageReceivedAdvanceProtocol.data.ToArray()), ex.Message, deviceName));
                        }
                        break;
                    case 4: // response calibration 
                        statusLog("Response Calibration: " + BitConverter.ToString(messageReceivedAdvanceProtocol.data.ToArray()));
                        if (messageReceivedAdvanceProtocol.length == 0)
                            if (recentMsgIdent == 85)
                            {
                                waitForResponseList[recentMsgIdent].Set();
                            }
                            else if (recentMsgIdent == 86)
                            {
                                waitForResponseList[recentMsgIdent].Set();
                            }
                        break;
                    case 8: // Remote

                        // signalR send if exists
                        if (Properties.Settings.Default.SignalR)
                        {
                            statusLog("Sending image data");
                            OnSendingDataSignalR(deviceType, messageReceivedAdvanceProtocol.data);
                        }

                        byte idxLine = messageReceivedAdvanceProtocol.data[0];

                        if ((int)deviceType > 4)
                        {
                            string tmp_msg = "";
                            foreach (byte b in messageReceivedAdvanceProtocol.data)
                                tmp_msg += "" + b;
                            statusLog(tmp_msg);
                            if (Properties.Settings.Default.SignalR)
                            {
                                statusLog("Sending image data");
                                OnSendingDataSignalR(deviceType, messageReceivedAdvanceProtocol.data);
                            }

                            messageReceivedAdvanceProtocol.data.RemoveAt(0);
                            getHalfPageRemoteAdvanceProtocolEcotech98_USB(messageReceivedAdvanceProtocol.data);
                            waitForResponseList[(byte)(10 + idxLine)].Set();
                            break;
                        }
                        getLinePixelsRemoteAdvanceProtocolEcotech(messageReceivedAdvanceProtocol.data);
                        LCDLineData[(byte)(idxLine - 16)] = messageReceivedAdvanceProtocol.data;

                        waitForResponseList[idxLine].Set();

                        break;
                    case 9: // response commands
                        statusLog("Response remote commands: " + BitConverter.ToString(messageReceivedAdvanceProtocol.data.ToArray()));
                        break;
                }
            }
            else  // Ecotech 9800 Commands
            {
                if (writeMode)
                {
                    sw.WriteLine(Encoding.UTF8.GetString(messageReceived.ToArray()));
                    sw.Flush();
                }
                try
                {
                    string deviceResponse = Encoding.UTF8.GetString(messageReceived.ToArray());
                    if (deviceResponse.Contains('\0'))
                    {
                        //timeLastMessageRecieved.AddMinutes(-30);
                        readingStatusDevice = 1; // when bad byte recived
                        traceLog(string.Format("Device: {0} ParseMessage: tmpData : {1} and time last message recieved {2}", deviceName, string.Join(", ", deviceResponse), timeLastMessageRecieved));
                    }
                    else
                    {
                        string[] tmpData = deviceResponse.Split(' ');

                        //var tmpData = new List<string>();

                        //foreach (var i in td)
                        //{
                        //    if (i.Length > 0)
                        //        tmpData.Add(i);
                        //}
                        //string[] tmpData = data.ToArray();

                        traceLog(string.Format("Device: {0} ParseMessage: tmpData : {1}", deviceName, string.Join(", ", tmpData)));

                        if (polutions.Count == 1)
                        {
                            if (!float.TryParse(tmpData[0], out polutions[0].concentration))
                                errorLog(string.Format("Device: {0} ParseMessage: Could not Convert concentration data : {1} to float", deviceName, tmpData[0]));

                            int alarmtmp;
                            if (!int.TryParse(tmpData[tmpData.Length - 1], out alarmtmp))
                                errorLog(string.Format("Device: {0} ParseMessage: Could not Convert alarm data : {1} to int", deviceName, tmpData[tmpData.Length - 1]));
                            polutions[0].alarmValue= alarmtmp & 0xFFF8;
                            // alarmValue = alarmtmp & 0xFFF8;
                        }
                        else if (tmpData.Length > 3)
                        {

                            if (!float.TryParse(tmpData[0], out polutions[0].concentration))
                                errorLog(string.Format("Device: {0} ParseMessage: Could not Convert concentration data : {1} to float", deviceName, tmpData[0]));

                            if (!float.TryParse(tmpData[1], out polutions[1].concentration))
                                errorLog(string.Format("Device: {0} ParseMessage: Could not Convert concentration data : {1} to float", deviceName, tmpData[1]));

                            if (!float.TryParse(tmpData[2], out polutions[2].concentration))
                                errorLog(string.Format("Device: {0} ParseMessage: Could not Convert concentration data : {1} to float", deviceName, tmpData[2]));

                            int alarmtmp;
                            if (!int.TryParse(tmpData[3], out alarmtmp))
                                errorLog(string.Format("Device: {0} ParseMessage: Could not Convert alarm data : {1} to int", deviceName, tmpData[3]));

                            //alarmValue = alarmtmp & 0xFFF8;
                            polutions[0].alarmValue= alarmtmp & 0xFFF8;
                            polutions[1].alarmValue = polutions[0].alarmValue;
                            polutions[2].alarmValue = polutions[0].alarmValue;
                            //polutions[0].concentration = Convert.ToSingle(tmpData[0]);
                            //polutions[1].concentration = Convert.ToSingle(tmpData[1]);
                            //polutions[2].concentration = Convert.ToSingle(tmpData[2]);
                            //alarmValue = (int)(Convert.ToUInt16(tmpData[3]) & 0xFFF8);
                        }
                        else
                        {
                            errorLog(string.Format("Device: {0} ParseMessage: Polutions count is not match to 9800 protocol tmpData: {1}", deviceName, string.Join(", ", tmpData)));
                        }

                        waitForResponseList[1].Set();
                        workingStatus = true;
                    }
                }
                catch (Exception ex)
                {
                    errorLog("[Ecotech 9800 CMD parse Problem] - " + deviceName + " - " + ex.Message);
                }
            }
        }

        #endregion Parse Incoming Data

        #region Calibrate

        public override void Calibrate()
        {
            if (workingStatus && calibParam != null)
            {
                // set valve to calibration
                MessageAdvanceProtocol msg;
                if (calibParam.calibType == CalibrationType.Zero || calibParam.calibType == CalibrationType.CheckZero)
                    msg = new MessageAdvanceProtocol(4, 0, new byte[] { 85, 64, 0, 0, 0 }); // 2.0f
                else // Span Calibration
                    msg = new MessageAdvanceProtocol(4, 0, new byte[] { 85, 64, 64, 0, 0 }); // 3.0f

                recentMsgIdent = 85;
                sendAdvanceProtocolMessage(msg, recentMsgIdent);

                if (waitForResponseList[recentMsgIdent].WaitOne(maximumResponseTime))
                {
                    dilutionCalibrator.calibrationDeviceState |= CalibrationDeviceState.AnalyzerInCalibMode;
                    dilutionCalibrator._calibrationProcessStatus = CalibrationProcessStatus._3AnalyzerStartedCalib;
                }
                else
                {
                    calibErrorIDs errID = calibErrorIDs.Analyzer_Entering_Calib_Command_Failed;
                    errorLog("[Calibration] " + calibParam.calibType.ToString() + " is NOT Started on : " + deviceType.ToString() + " - " + errID.ToString(), 2, (byte)errID);
                    stopCalib(true);
                    return;
                }

                waitAndsendConcAtEndCalib(); // Time of calibration has been spent. We should decide if exert Calib Coeff OR Not!

                if (true) //..............// feedback query for Calibration Coeff
                {
                    dilutionCalibrator._calibrationProcessStatus = CalibrationProcessStatus._4AnalyzerFinishedCalib;
                }

                if (calibParam.calibType == CalibrationType.Span || calibParam.calibType == CalibrationType.Zero)
                {
                    if (polutions[0].ID != 1)  // [1] is indicator of "O3" Analyzer [ Caz we have "O3 Concentration" and "Gas Concentration"]
                        setCalibValue(calibParam.calibType, calibParam.gasCalibPoint.exactGasConcentration);// non-O3
                    else
                        setCalibValue(calibParam.calibType, calibParam.gasCalibPoint.exactO3Concentration); // O3
                }


                // [4]: Send CMD to Analyzer to get out of Calibration Mode ===========================================================
                stopCalib(false); // false : Normal stop of Calibration
            }
            else
            {
                calibErrorIDs errID = calibErrorIDs.Analyzer_Not_Working;
                errorLog("[Calibration] " + deviceType.ToString() + " -" + errID.ToString(), 2, (byte)errID);
            }
        }

        protected override void stopCalib(bool forceFully)
        {
            bool success = false;
            if (workingStatus && calibParam != null)
            {
                if (!forceFully || (dilutionCalibrator.calibrationDeviceState.HasFlag(CalibrationDeviceState.AnalyzerInCalibMode)))
                {
                    MessageAdvanceProtocol msg = new MessageAdvanceProtocol(4, 0, new byte[] { 85, 0, 0, 0, 0 });
                    for (int i = 0; i < maxTryCalibStop; i++)
                    {
                        sendAdvanceProtocolMessage(msg, 86); // HANDLE THE RESPONSE
                        recentMsgIdent = 86;

                        if (waitForResponseList[recentMsgIdent].WaitOne(maximumResponseTime))
                        {
                            success = true;
                            dilutionCalibrator.calibrationDeviceState &= ~CalibrationDeviceState.AnalyzerInCalibMode;
                            dilutionCalibrator._calibrationProcessStatus = CalibrationProcessStatus._5AnalyzerExitCalib;
                            break;
                        }
                    }
                    if (!success)
                    {
                        calibErrorIDs errID = calibErrorIDs.Analyzer_Exiting_Calib_Command_Failed;
                        errorLog("[Calibration] " + deviceType.ToString() + " -" + errID.ToString(), 2, (byte)errID);
                    }
                }
                // Send CMD to Solenoid and Calibrator to Tower Off
                base.stopCalib(forceFully);
            }
            else
            {
                calibErrorIDs errID = calibErrorIDs.Analyzer_Exiting_Calib_Command_Failed_NOT_WORKING;
                errorLog("[Calibration] " + deviceType.ToString() + " -" + errID.ToString(), 2, (byte)errID);
            }
        }

        #endregion Calibrate

        #region Remote
        const int LCDWidthPx = 240; // 30 column * 8 px
        const int LCDHeightPx = 128; // 16 Line * 8 px ****** Ecotech98_USB has 14 line
        public byte[] LCDPixels = new byte[LCDWidthPx * LCDHeightPx];  // 30720 = 240 px * 128 px
        public byte[] LCDPixelsStyle = new byte[LCDWidthPx * LCDHeightPx];  // 30720 = 240 px * 128 px
        List<byte>[] LCDLineData = new List<byte>[16];  // 240 byte * 16 Line

        //indexLine = 15 => whole screen
        public Bitmap getRemoteScreenTillLine(byte indexLine)
        {
            byte[] tmpLCDPixels;
            int heightPx = 8 * (indexLine + 1);
            tmpLCDPixels = new byte[LCDWidthPx * heightPx];  // 30720 = 240 px * 8 px

            // a new bitmap.
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(LCDWidthPx, heightPx, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

            // Lock the bitmap's bits.  
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;


            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, tmpLCDPixels, 0, tmpLCDPixels.Length);

            for (int k = 0; k < tmpLCDPixels.Length; k++)
                tmpLCDPixels[k] = LCDPixels[k];

            System.Runtime.InteropServices.Marshal.Copy(tmpLCDPixels, 0, ptr, tmpLCDPixels.Length);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            // Draw the modified image.
            return bmp;
        }

        public void sendGetLineRemoteAdvanceProtocolEcotech(byte idxLine)
        {
            // ignore 16 for ecotech9800 USB port

            MessageAdvanceProtocol msg = new MessageAdvanceProtocol(8, 0, new byte[] { (byte)(16 + idxLine) });
            sendAdvanceProtocolMessage(msg, (byte)(16 + idxLine));
        }
        public void sendHalfPageRemoteAdvanceProtocolEcotech98_USB(byte idxLine)
        {
            // ignore 10 for ecotech9800 USB port

            MessageAdvanceProtocol msg = new MessageAdvanceProtocol(8, 0, new byte[] { (byte)(idxLine) });
            sendAdvanceProtocolMessage(msg, (byte)(10 + idxLine));
        }

        public void getLinePixelsRemoteAdvanceProtocolEcotech(List<byte> LineDataByte)
        {
            byte startIdxLineInPixel = (byte)(LineDataByte[0] - 16);
            LineDataByte.RemoveAt(0);
            byte[] bytePixelData;
            for (int i = 0; i < 240/*responseBytes.Count*/; i++)
            {
                bytePixelData = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
                //bytePixelData = new byte[8] { 0, 1, 1, 0, 1, 1, 0, 0 };
                string tmpBinaryByte = Convert.ToString(Convert.ToInt32(LineDataByte[i]), 2);
                for (int j = 0; j < tmpBinaryByte.Length; j++)
                {
                    bytePixelData[j] = Convert.ToByte(tmpBinaryByte.Substring(tmpBinaryByte.Length - (j + 1), 1));
                }
                for (int k = 0; k < 8; k++)
                    LCDPixels[8 * (LCDWidthPx * startIdxLineInPixel + i) + k] = (byte)(bytePixelData[k] * 255);
            }
        }

        public void getHalfPageRemoteAdvanceProtocolEcotech98_USB(List<byte> LineDataByte)
        {
            var tmpLine = "";
            var tmpLineStyle = "";


            for (var i = 0; i < LineDataByte.Count; i++)
            {
                if ((i + 1) % 30 == 0)
                {
                    StrData.Add(tmpLine);
                    StrStyle.Add(tmpLineStyle);

                    tmpLine = "";
                    tmpLineStyle = "";
                }

                if (LineDataByte[i] > 127)
                {

                    tmpLine = string.Concat(new[] { tmpLine, Encoding.GetEncoding("iso-8859-1").GetString(new byte[] { (byte)(LineDataByte[i] - 96) }) });
                    tmpLineStyle = string.Concat(new[] { tmpLineStyle, "h" });
                }
                else
                {
                    tmpLine = string.Concat(new[] { tmpLine, Encoding.GetEncoding("iso-8859-1").GetString(new byte[] { (byte)(LineDataByte[i] + 32) }) });
                    tmpLineStyle = string.Concat(new[] { tmpLineStyle, "n" });
                }
            }
        }

        public override void prepareRemoteScreen()
        {
            try
            {
                resetLCDPixels();
                for (byte k = 0; k < 16; k++)
                {
                    sendGetLineRemoteAdvanceProtocolEcotech(k);
                    if (!waitForResponseList[(byte)(k + 16)].WaitOne(maximumResponseTime)) // 
                        errorLog("Line: " + (k - 15).ToString() + " in Remote Mode Not Recieved for Device" + deviceType.ToString());
                }
                waitForResponseList[213].Set();
            }
            catch (Exception ee)
            {
                errorLog("Remote Screen Problem : " + ee.Message);
            }
        }

        public bool setCalibValue(CalibrationType calibType, double exactGasConcentration)
        {
            bool success = false;
            menuStageType menuStage = menuStageType.unknown;
            sendRemoteCommand((byte)(60)); //down

            List<byte> FirstLine8bitData = new List<byte>();
            List<byte> OtherLine8bitData = new List<byte>();

            int counter = 0;
            //Line8bitData = new byte[] { 0, 0, 10, 17, 16 };
            while (menuStage != menuStageType.ended)
            {
                counter++;
                switch (menuStage)
                {
                    case menuStageType.unknown:
                        sendGetLineRemoteAdvanceProtocolEcotech(0);
                        waitForResponseList[16].WaitOne();
                        FirstLine8bitData = LCDLineData[0];
                        if (Enumerable.SequenceEqual(FirstLine8bitData, checkPixelBytes["Quick_Menu"]))
                        {
                            menuStage = menuStageType.Quick_Menu;
                        }
                        else
                        {
                            sendRemoteCommand((byte)(60)); //left (back)
                        }
                        break;
                    case menuStageType.Quick_Menu:
                        if (calibType == CalibrationType.Zero)
                        {
                            sendGetLineRemoteAdvanceProtocolEcotech(4);
                            waitForResponseList[20].WaitOne();
                            OtherLine8bitData = LCDLineData[4];
                        }
                        else
                        {
                            sendGetLineRemoteAdvanceProtocolEcotech(2);
                            waitForResponseList[18].WaitOne();
                            OtherLine8bitData = LCDLineData[2];
                        }
                        if (Enumerable.SequenceEqual(OtherLine8bitData, checkPixelBytes[calibTypeStr[(int)calibType]]))
                        {
                            sendRemoteCommand((byte)(62)); //right(open)
                            menuStage = menuStageType.Selected_Calibration;
                        }
                        else
                        {
                            sendRemoteCommand((byte)(118)); //down
                        }
                        break;
                    case menuStageType.Selected_Calibration:
                        string valueStr = exactGasConcentration.ToString("0.000");
                        foreach (char c in valueStr)
                        {
                            sendRemoteCommand((byte)(c)); //enter numbers
                        }
                        sendRemoteCommand((byte)(62)); //right(Accept)
                        sendRemoteCommand((byte)(60)); //left(Back)
                        menuStage = menuStageType.ended;
                        break;
                }
                if (counter > 30)
                    errorLog("Unsuccessful Calibration of " + this.deviceType.ToString() + " at MenuStage");//return false; // unsuccessful Attempt
            }

            if (menuStage == menuStageType.ended)
            {
                success = true;
            }
            return success;
        }

        internal void resetLCDPixels()
        {
            LCDPixels = new byte[LCDWidthPx * LCDHeightPx];
        }


        #region Common
        public override void sendRemoteCommand(byte command)
        {
            if ((int)deviceType < 5 || portCOM.Trim() == "USB")
            {
                MessageAdvanceProtocol msg = new MessageAdvanceProtocol(9, 0, new byte[] { (byte)(command + 128) });
                sendAdvanceProtocolMessage(msg, 9);
                Thread.Sleep(200);
                msg = new MessageAdvanceProtocol(9, 0, new byte[] { command });
                sendAdvanceProtocolMessage(msg, 9);
                Thread.Sleep(200);
                return;
            }

            switch (command)
            {
                case 27:   // Esc (Exit Button)
                    messageSendOnSerialBytesAwaitingResponse(new byte[] { 27, 91, 79, 81 }, remoteWaitHandlerIndex);
                    break;
                case 65: // Up : A
                case 66: // Down : B
                case 67: // Left ( Select) : C
                case 68: // Right ( Page Up) : D  
                    messageSendOnSerialBytesAwaitingResponse(new byte[] { 27, 91, command }, remoteWaitHandlerIndex);
                    break;
                default:
                    messageSendOnSerialBytesAwaitingResponse(new byte[] { command }, remoteWaitHandlerIndex);  // Used for Enter (13)
                    break;
            }

        }
        public override Bitmap getRemoteScreen()
        {
            if ((int)deviceType < 5) // Advance Ecotech Protocol
                return getRemoteScreenTillLine(15);

            //if (portCOM.Trim() == "USB")
            //    return getRemoteScreen_Ecotech_USB(30);

            return getRemoteScreenText();
        }

        public override void startRemote()
        {
            if (!((int)deviceType < 5 || portCOM.Trim() == "USB"))
            {
                string tmp = "REMOTE," + System.Convert.ToInt32(DeviceID).ToString("D3") + "\r\n";
                messageSendOnSerialBytesAwaitingResponse(Encoding.ASCII.GetBytes(tmp),0);
            }

            base.startRemote();
        }

        public override void stopRemote()
        {
            if (!((int)deviceType < 5 || portCOM.Trim() == "USB"))
            {
                string tmp = "DCONC," + System.Convert.ToInt32(DeviceID).ToString("D3") + "\r\n";
                messageSendOnSerialBytesAwaitingResponse(Encoding.ASCII.GetBytes(tmp), 0);
                //messageSendOnSerial(tmp);
            }

            base.stopRemote();
        }


        #endregion


        #region Ecotech9800
        // ************EC9800 ***************

        public enum remoteEC9800Status
        {
            strWriting = 0, Start = 1, AfterStart = 2, GotRawInfo = 3,
            x2Cursor = 10, sepCursor = 11, y1Cursor = 12, y2Cursor = 13
        }
        remoteEC9800Status RGCS;
        byte tmpRawData;
        const int numLinesRemote = 20;
        List<string> StrData = new List<string>(new string[numLinesRemote]);
        List<string> StrStyle = new List<string>(new string[numLinesRemote]);
        List<string> PreData = new List<string>(new string[numLinesRemote]);
        int[] globalCursor = new int[2];
        int[] cursonInfo = new int[4];
        byte counterForStrData = 0;
        const byte remoteWaitHandlerIndex = 10;  // remote
        string styleType = "n";

        byte textMode = 0; // 0 : Normal Mode, 1 = Highlight Mode
        //int indexLineRemote = 0;

        public void RemoteScreenParseMessage_Ecotech9800_Com(byte data)
        {
            workingStatus = true;
            timeLastMessageRecieved = DateTime.Now;
            var HighlightPosition = new List<string>();
            var highlightStr = "";

            try
            {
                //foreach (byte data in Data)
                //{
                switch (RGCS)
                {
                    case remoteEC9800Status.strWriting:

                        if (data == 27)  // 0x1b  ESC
                            RGCS++;
                        else
                        {
                            string tmpStr = Encoding.GetEncoding("iso-8859-1").GetString(new byte[] { data });
                            //if (counterForStrData <= 1)
                            //{
                            lock (StrData)
                            {
                                StrData[globalCursor[0]] = StrData[globalCursor[0]].Remove(counterForStrData, 1).Insert(counterForStrData, tmpStr);

                                StrStyle[globalCursor[0]] = StrStyle[globalCursor[0]].Remove(counterForStrData, 1).Insert(counterForStrData, styleType);
                            }
                            counterForStrData++;

                        }

                        break;

                    case remoteEC9800Status.Start:
                        if (data == 91)  // 0x5b [
                            RGCS++;
                        break;

                    case remoteEC9800Status.AfterStart:
                        tmpRawData = data;   // 
                        RGCS++;
                        break;

                    case remoteEC9800Status.GotRawInfo:
                        if (data == 74)    // J  // Clear Screen
                        {
                            initializeStrData();  // Clear Screen
                            RGCS = remoteEC9800Status.strWriting;
                        }
                        else if (data == 109)   // 6d  : m   //Text mode Highlight tmpRawData : 7 or not : 0
                        {
                            switch (tmpRawData)
                            {
                                case 55:
                                    styleType = "h";
                                    break;
                                case 49:
                                    styleType = "b";
                                    break;
                                default:
                                    styleType = "n";
                                    break;
                            }

                            RGCS = remoteEC9800Status.strWriting;

                            //if (tmpRawData == 48) // No HighLight Line
                            //{
                            //    isHighlight = false;
                            //    //StrData[globalCursor[0]] = StrData[globalCursor[0]].Remove(0, 3).Insert(0, Style);
                            //    //initializePreData();
                            //    //PreData[globalCursor[0]] = ">> ";
                            //}
                            //else if (tmpRawData == 55)
                            //{
                            //    isHighlight = true;
                            //}
                            //else if (tmpRawData == 49)
                            //else
                            //    RGCS = remoteGasCalStatus.strWriting;
                        }
                        else  //H
                        {
                            cursonInfo[0] = tmpRawData;
                            cursonInfo[1] = data;
                            globalCursor[0] = (cursonInfo[1] - 48) + 10 * (cursonInfo[0] - 48);
                            if (globalCursor[0] < 0)
                                RGCS = remoteEC9800Status.strWriting;
                            RGCS = remoteEC9800Status.x2Cursor;
                        }
                        break;
                    case remoteEC9800Status.x2Cursor:
                        RGCS++;
                        break;
                    case remoteEC9800Status.sepCursor:
                        cursonInfo[2] = data;
                        RGCS++;
                        break;
                    case remoteEC9800Status.y1Cursor:
                        cursonInfo[3] = data;
                        globalCursor[1] = (cursonInfo[3] - 48) + 10 * (cursonInfo[2] - 48);
                        counterForStrData = (byte)(globalCursor[1]);
                        RGCS++;
                        break;
                    case remoteEC9800Status.y2Cursor:
                        //if (globalCursor[0] < 20 && globalCursor[0] >= 0)
                        //{
                        //    RGCS++;
                        //}
                        //else
                        RGCS = remoteEC9800Status.strWriting;
                        break;
                }
                //}
            }
            catch (Exception ex)
            {
                Device.LogHandler.errorLog("[Ecotech98 RemoteScreenParseMessage] " + ex.Message);
                RGCS = remoteEC9800Status.strWriting;
            }

        }

        // clean array of Lines
        public void initializeStrData()
        {
            var str = new String(' ', 500);
            for (int k = 0; k < StrData.Count; k++)
            {
                StrData[k] = str;
                StrStyle[k] = str;
            }
        }

        public Bitmap getRemoteScreenText()
        {
            //tmpRawData
            //48 = normal
            //55 = Highlight
            //49 = bold

            var result = new Bitmap(480, 340);
            var g = Graphics.FromImage(result);
            char[] str;
            char[] style;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            var drawFont = new Font("Courier New", 10);
            var CharWidth = (int)g.MeasureString("A", drawFont).Width;
            var CharHeight = (int)g.MeasureString("A", drawFont).Height;

            var beginPoint = new Point(10, 0);

            try
            {
                for (var i = 0; i < StrData.Count; i++)
                {
                    str = (StrData[i] ?? "").ToArray();

                    style = StrStyle[i].ToArray();


                    for (var j = 0; j < str.Length; j++)
                    {

                        switch (style[j])
                        {
                            case 'h': // Highlight

                                var rect = new RectangleF(beginPoint.X, beginPoint.Y, CharWidth, CharHeight);
                                g.FillRectangle(Brushes.Yellow, rect);

                                g.DrawString(str[j].ToString(), drawFont, Brushes.DarkGreen, rect);
                                break;

                            case 'b': // Bold

                                g.DrawString(str[j].ToString(), new Font("Courier New", 10, FontStyle.Bold), Brushes.Yellow, beginPoint);
                                break;

                            default: //Normal

                                g.DrawString(str[j].ToString(), drawFont, Brushes.Yellow, beginPoint);
                                break;
                        }

                        beginPoint.X += CharWidth;

                    }
                    beginPoint.X = 0;
                    beginPoint.Y += CharHeight;
                }

                g.Flush();

            }
            catch (Exception ex)
            {
                errorLog("[Ecotech98 getRemoteScreen] " + ex.Message);
            }

            return result;
        }

        #endregion

        #endregion Remote

        public override byte getStatus(int alarm)
        {
            byte state = 1;
            var alarmVal = /*_alarmValue*/alarm & ~IgnorableErrors;

            // refer to Ecotech DataSheet
            if ((alarmVal & (1 << 3)) == (1 << 3))
                state = 3; // Span

            if ((alarmVal & (1 << 4)) == (1 << 4))
                state = 2; // Zero

            if (alarmVal > 0x1F)
                state = 4; // Defect

            return state;
        }

        //public override string alarmTranslate(int alarm)
        //{
        //    string s = Convert.ToString(alarm, 2);
        //    string A = "";
        //    //DatabaseHandler.ds.Tables["DeviceTypes"].Select("DeviceType = " + deviceType.ToString());
        //    for (int i = 0; i < s.Length; i++)
        //    {
        //        if (s[s.Length - i - 1].ToString() == "1")
        //        {
        //            //A = ((ds.DeviceTypes.Rows[deviceType]["Alarm_Name" + (15 - i).ToString("D2")]).ToString());
        //            //A += ds.DeviceTypes.DefaultView[0]["Alarm_Name" + (i + 1).ToString("D2")].ToString() + ",";
        //            A += alarmNames[i] + ",";
        //        }
        //    }
        //    A = A.Remove(A.Length - 1);
        //    return A;
        //}

        //protected override void disconnectUSB()
        //{
        //    EcotechFtdi.Close();
        //}

        #region Sending

        // Method for couple of  orders to send message on serial
        protected void messageSendBytesUSB(byte[] message)
        {
            try
            {
                lock (lockConn2Device)
                {
                    if (EcotechFtdi.IsOpen)
                    {
                        UInt32 numBytesWritten = 0;

                        FTDI.FT_STATUS ftStatus = EcotechFtdi.Write(message, message.Length, ref numBytesWritten);
                        if (ftStatus != FTDI.FT_STATUS.FT_OK)
                        {
                            errorLog("Problem in Sending DatBytes to Ecotech " + ftStatus.ToString());
                            workingStatus = false;
                        }
                        else
                        {
                            if ((int)deviceType > 4 && portCOM == "USB" && remoteMode)
                                Thread.Sleep(500);
                            else
                                Thread.Sleep(100);

                            ReadDataManual();

                        }
                    }
                    else
                        workingStatus = false;

                }
            }
            catch
            {
                errorLog("Problem in Sending Data for Device : " + deviceName);
            }

        }

        protected void messageSendBytesUSBAwaitingResponse(byte[] message, byte msgIndetifier)
        {
            if (!waitForResponseList.ContainsKey(msgIndetifier))
                waitForResponseList[msgIndetifier] = new AutoResetEvent(false);
            else
                waitForResponseList[msgIndetifier].Reset();
            messageSendBytesUSB(message);
        }

        #endregion     

        protected override void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                if (disposing)
                {
                    if (EcotechFtdi.IsOpen)
                        EcotechFtdi.Close();
                }
                // Release unmanaged resources. If disposing is false, 
            }
            base.Dispose(true);

        }
        internal override void Disconnect()
        {
            base.Disconnect();

            if (portCOM.Contains("USB"))
            {
                EcotechFtdi.Close();
            }
            else
            {
                if (conSerial.IsOpen)
                    conSerial.Close();
            }
        }
    }
}