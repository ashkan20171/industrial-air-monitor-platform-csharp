using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SadraAQMS.Analyzers
{
    class ESAAnalyzer : deviceAnalyzer
    {
        // For debug 
        //System.IO.StreamWriter swRemoteESA;

        #region Variable & Fields

        //Client client;  // For case of connecting with LAN
        private UdpClient udpclient;

        int swicthProtocol = 1; // 1: Data, 2: Graphics 3: Data in Float Format

        enum MessageESAProtocolStatusType
        {
            UNINIT = 0, GOT_SYNC, GOT_DeviceID, GOT_MessageID, GOT_LEN, GOT_PAYLOAD, GOT_CHECKSUM
        };
        MessageESAProtocolStatusType statusMsgRecv;

        byte messageIndexRecieve;

        static Dictionary<DeviceType, string> ESA_stringIDsSuggested = new Dictionary<DeviceType, string>()
            {
                {DeviceType.ESA_CO ,"CO12"},//CO12
                {DeviceType.ESA_Nox,"AC32"},
                {DeviceType.ESA_SO2,"AF22"},
                {DeviceType.ESA_O3 ,"O342"},

                {DeviceType.ESA_Organic,"VOC7"}, // ????

                {DeviceType.ESA_susPariculate2_5, "1498"},
                {DeviceType.ESA_susPariculate10,"1498"},
                {DeviceType.ESA_MicroStation,"MMS1"},
                {DeviceType.ESA_susPariculateNew,"2140"},
                {DeviceType.ESA_susPariculateNew2_5,"2140"},
                {DeviceType.ESA_susPariculateNew10,"2140"},
            };

        public class MessageESAProtocol
        {
            public byte[] deviceID = new byte[4];
            public byte[] messageID = new byte[2];
            public byte messageType;
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
            /// <summary>
            /// Compute Checksum of ESA Message and output (1 byte) in HEX String format (2 byte char) [XOR operation]
            /// </summary>
            /// <param name="msg"> the part of Message that in List-byte Format that is used in checksum calculation </param>
            /// <returns>CheckSum HEX String format (2 byte char)</returns>
            static public byte[] computeESAChecksum(List<byte> msg)
            {
                byte chksum = 0;
                for (int k = 1; k < msg.Count; k++)
                    chksum ^= msg[k];
                 return Encoding.UTF8.GetBytes(chksum.ToString("X2"));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="isResponse"></param>
            /// <returns></returns>
            public string GetBytesString(bool isResponse)
            {
                List<byte> tmpListByts = new List<byte>();
                if (isResponse)
                    tmpListByts.Add(6);
                else
                    tmpListByts.Add(2);

                tmpListByts.AddRange(deviceID);
                tmpListByts.AddRange(messageID);
                tmpListByts.AddRange(data);
                tmpListByts.AddRange(computeESAChecksum(tmpListByts));
               
                //tmpListByts.AddRange(Encoding.UTF8.GetBytes(chks.ToString("X2")));

                tmpListByts.Add(3);
                return String.Join(" ",tmpListByts.Select(b => b.ToString()));
            }
        }

        MessageESAProtocol messageReceivedESA = new MessageESAProtocol();

        #endregion Variable & Fields

        public ESAAnalyzer(DeviceType dvType, string devID) // Constructor 
        {
            //this.company = DeviceCompany.EnviromentSA;
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            this.DeviceID = ESA_stringIDsSuggested[dvType];
            //swRemoteESA = new System.IO.StreamWriter(".\\Logs\\ESARemote_" + deviceName + DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2") + ".txt");

            ClearScreen(2, true, true);
        }

        public ESAAnalyzer(DeviceType dvType, string devID, string devName, string ignorableErrors)
        {
            //this.company = DeviceCompany.Ecotech;
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            this.DeviceName = devName;
            int.TryParse(ignorableErrors, out IgnorableErrors);
            ClearScreen(2, true, true);
        }

        #region Send Data on Serial

        public void sendESAMessage(MessageESAProtocol msg, byte msgIdentifier)
        {
            try
            {
                List<byte> tmpMsg = new List<byte>();
                tmpMsg.Add(2);
                tmpMsg.AddRange(msg.deviceID);
                tmpMsg.AddRange(msg.messageID);
                tmpMsg.AddRange(msg.data);
                tmpMsg.AddRange(MessageESAProtocol.computeESAChecksum(tmpMsg));
                tmpMsg.Add(3);

                traceLog(string.Format("SendESAMessage: 2 deviceId: {0} messageId: {1} data: {2}, ComputedCheckSum: {3} 3", UTF8Encoding.UTF8.GetString(msg.deviceID), UTF8Encoding.UTF8.GetString(msg.messageID), UTF8Encoding.UTF8.GetString(msg.data.ToArray()), UTF8Encoding.UTF8.GetString(MessageESAProtocol.computeESAChecksum(tmpMsg))));

                if (portCOM.Contains(':') && udpclient != null)
                {
                   if (msgIdentifier == 0) // Zero no waitHandle
                       udpclient.Send(tmpMsg.ToArray(),tmpMsg.Count,portCOM.Split(':')[0], Convert.ToInt16(portCOM.Split(':')[1]));
                   else
                   {
                        if (!waitForResponseList.ContainsKey(msgIdentifier))
                            waitForResponseList[msgIdentifier] = new AutoResetEvent(false);
                        else
                            waitForResponseList[msgIdentifier].Reset();
                        udpclient.Send(tmpMsg.ToArray(), tmpMsg.Count, portCOM.Split(':')[0], Convert.ToInt16(portCOM.Split(':')[1]));
                   }
                    
                }
                else
                {
                    if (msgIdentifier == 0) // Zero no waitHandle
                        messageSendOnSerialBytes(tmpMsg.ToArray());
                    else
                        messageSendOnSerialBytesAwaitingResponse(tmpMsg.ToArray(), msgIdentifier);
                }
            }
            catch (Exception ex)
            {
                errorLog("sendESAMessage error: " + ex.Message);
            }
        }

        // Method for reading gas concentration reported by device
        public override void sendReadCommand()
        {
            MessageESAProtocol msg;

            // Message ID for Concentration reading: "04" ({48,52}) To get Errors and M-Span-Zero states
            msg = new MessageESAProtocol(Encoding.UTF8.GetBytes(DeviceID), new byte[] { 48, 52 }, new byte[] { });
            sendESAMessage(msg, 1);
            // if not very old
            if (deviceType != DeviceType.ESA_susPariculate10 && deviceType != DeviceType.ESA_susPariculate2_5 && deviceType != DeviceType.ESA_Organic)
            {

                Thread.Sleep(200);
                // Send read 24 for ESA with float numbers in binary format with no alarm
                msg = new MessageESAProtocol(Encoding.UTF8.GetBytes(DeviceID), new byte[] { 50, 52, 48, 48, 48, 49, 48, (byte)(48 + polutions.Count) }, new byte[] { });
                sendESAMessage(msg, 1);
               // statusLog("send msg 24"+deviceName);
            }
        }

        #endregion Send Data on Serial

        #region Parse Incoming Data

        //for mode ENSVA(Old devices ESA_pariculate) or for Alarms
        private float ESAProtocolDataInterpretor(byte[] responseBytes, int Length, int index)
        {
            
            //int ResDigit;
            float ResFloat = 0;
            switch (Length)
            {
                case 8:  // Integer data devided usually by 100 
                    {
                        try
                        {
                            ResFloat = (float)Math.Pow(10, -this.numberOfDigits) * Convert.ToInt32(Encoding.ASCII.GetString((responseBytes.ToList<byte>()).GetRange(index, Length).ToArray(), 0, Length));

                            traceLog("ESAProtocolDataInterpretor case 8 message range: " + Encoding.ASCII.GetString((responseBytes.ToList<byte>()).GetRange(index, Length).ToArray(), 0, Length));
                        }
                        catch (Exception ex)
                        {
                            errorLog("[ESA Proto Length 8 - Data] - " + deviceName + " : " + ex.Message + " * " + UTF8Encoding.UTF8.GetString(messageReceivedESA.data.ToArray()));
                            ResFloat = -1;
                        }
                    }
                    break;
                case 2:
                    {
                        try
                        {
                            ResFloat = Convert.ToInt32(Encoding.ASCII.GetString((responseBytes.ToList<byte>()).GetRange(index, Length).ToArray(), 0, Length),16);
                        }
                        catch (Exception ex)
                        {
                            errorLog("[ESA Proto Length 2 - Alarm] - " + deviceName + " : " + ex.Message + " * " + UTF8Encoding.UTF8.GetString(messageReceivedESA.data.ToArray()));
                        }
                    }
                    break;
                case 1:  // Input Port
                    {
                        if (responseBytes[0] == 77) { ResFloat = 0; } // Messurement (M)
                        else if (responseBytes[0] == 90) { ResFloat = 1; } // Zero (Z)
                        else if (responseBytes[0] == 83) { ResFloat = 2; } // Span (S)
                        else if (responseBytes[0] == 82) { ResFloat = 1; } // Ref. Zero (R)
                        else if (responseBytes[0] == 'V'){ ResFloat = 4; } // VOC Mode ????
                        else
                        {
                            errorLog("[ESA Proto Length 1 - Input] - " + deviceName + " * " + responseBytes[0].ToString() + " * " + UTF8Encoding.UTF8.GetString(messageReceivedESA.data.ToArray()));
                            ResFloat = 4;
                        }
                        break;
                    }
            }
            return ResFloat;
        }

        // For Mode 4 Communication (float) without alarm value
        private float getFloatNumESA(List<byte> responseBytes, int index)
        {
            try
            {
                return BitConverter.ToSingle(new byte[] { responseBytes[index + 3], responseBytes[index + 2], responseBytes[index + 1], responseBytes[index] }, 0);
            }
            catch (Exception ex)
            {
                errorLog("[ESA Number Convert] Problem in change value to float in device " + deviceName + " : " + ex.Message);
                return -1;
            }
        }
        int errorCount = 0;
        public override void parseByte(byte data)
        {
            try
            {
                timeLastByteRecieved = DateTime.Now;
                // Frodebug
                //swRemoteESA.Write(data.ToString() + ",");
                // This is a condition for that  be sure that not be (statusMsgRecv > 7 and length of data >1000)
                if (statusMsgRecv > MessageESAProtocolStatusType.GOT_CHECKSUM || messageReceivedESA.data.Count>1000)
                    statusMsgRecv = MessageESAProtocolStatusType.UNINIT;
                if (statusMsgRecv < MessageESAProtocolStatusType.GOT_PAYLOAD)
                    messageReceivedESA.chks ^= data;

                switch (statusMsgRecv)
                {
                    case MessageESAProtocolStatusType.UNINIT:
                        if (data == 0x06)   // Data Mode  ->  swicthProtocol = 1
                        {
                            statusMsgRecv++;
                            messageReceivedESA.chks = 0x00;
                            messageReceivedESA.data.Clear();
                            messageIndexRecieve = 0;
                            swicthProtocol = 1;   // Normal Response ( [ACK,6] byte in start of recieved msg )
                        }
                        if (data == 27)  // Graphic Mode (Remote Device) ->  swicthProtocol = 3
                        {
                            statusMsgRecv++;
                            messageReceivedESA.chks = 0x00;
                            messageReceivedESA.data.Clear();
                            messageIndexRecieve = 0;
                            swicthProtocol = 3;  // new recieve float data  Message ID = 24,
                        }                        // ( [ESC,27] byte in start of recieved msg )
                        break;
                    case MessageESAProtocolStatusType.GOT_SYNC:
                        messageReceivedESA.deviceID[messageIndexRecieve++] = data;
                        if (messageIndexRecieve == 4)
                        {
                            statusMsgRecv++;
                            messageIndexRecieve = 0;
                        }
                        break;
                    case MessageESAProtocolStatusType.GOT_DeviceID:
                        if (data == 27)
                        {
                            messageReceivedESA.messageID[0] = 0;
                            messageReceivedESA.messageID[1] = data;
                            swicthProtocol = 2;   //  Remote response ( [ACK,6] byte in start of recieved msg, with msgID == 27 ) 
                            statusMsgRecv++;
                        }
                        else
                        {
                            messageReceivedESA.messageID[messageIndexRecieve++] = data;
                            if (messageIndexRecieve == 2)
                            {
                                messageIndexRecieve = 0;
                                if (messageReceivedESA.messageID[0] == '2' && messageReceivedESA.messageID[1] == '4')
                                {
                                    swicthProtocol = 4;
                                    //statusLog("go to  protocol 24 recived"+deviceName);
                                }
                                statusMsgRecv++;
                            }
                        }
                        break;
                    case MessageESAProtocolStatusType.GOT_MessageID:
                        // For "Protocal 1"
                        if (swicthProtocol == 1)
                        {
                            messageReceivedESA.data.Add(data);
                            if (data == 27)
                            {
                                statusMsgRecv = MessageESAProtocolStatusType.GOT_SYNC;
                                messageReceivedESA.chks = 0x00;
                                messageReceivedESA.data.Clear();
                                messageIndexRecieve = 0;
                                swicthProtocol = 3;  // new recieve float data  Message ID = 24,
                            }                        // ( [ESC,27] byte in start of recieved msg )

                            if (data == 0x03) // 0x03 = ETX
                            {
                                messageReceivedESA.chks ^= 3;
                                messageReceivedESA.chks ^= messageReceivedESA.data[messageReceivedESA.data.Count - 2];
                                messageReceivedESA.chks ^= messageReceivedESA.data[messageReceivedESA.data.Count - 3];
                                byte chk = 0;
                                try
                                {
                                    chk = Convert.ToByte(Encoding.ASCII.GetString(messageReceivedESA.data.GetRange(messageReceivedESA.data.Count - 3, 2).ToArray<byte>()), 16);
                                }
                                catch (Exception ex)
                                {

                                    errorLog("[ESA parseByte - checksum normal response] device: " + deviceName + " - " + ex.Message + " * " +
                                        UTF8Encoding.UTF8.GetString(messageReceivedESA.deviceID.ToArray()) +
                                        UTF8Encoding.UTF8.GetString(messageReceivedESA.messageID.ToArray()) +
                                        UTF8Encoding.UTF8.GetString(messageReceivedESA.data.ToArray()));
                                    goto error;
                                }
                                if (chk != messageReceivedESA.chks)
                                { goto error; }
                                else
                                {
                                  //  statusLog("switch protocol 04 "+deviceName);
                                    messageReceivedESA.data.RemoveRange(messageReceivedESA.data.Count - 3, 3);
                                    parseMessage(); //parse Message for data
                                    statusMsgRecv = MessageESAProtocolStatusType.UNINIT;
                                    //For debug
                                    //swRemoteESA.WriteLine();
                                    //swRemoteESA.Flush();
                                    ///////////////////////////
                                }
                                //goto restart;
                            }
                            break;
                        }
                        // For "Protocol 2"
                        else if (swicthProtocol == 2)
                        {
                            // To discard "0" after "length" in "Protocol 2"
                            if (++messageIndexRecieve < 2)
                            { messageReceivedESA.length = data; }
                            else
                            {
                                statusMsgRecv++;
                                messageIndexRecieve = 0;
                                if (messageReceivedESA.length == 0)
                                    statusMsgRecv++;
                            }
                        }
                        else if (swicthProtocol == 3)
                        {
                            if (++messageIndexRecieve < 2)
                            { }
                            else
                            {
                                statusMsgRecv++;
                                messageIndexRecieve = 0;
                                messageReceivedESA.length = data;
                            }
                        }

                        //*** { swicthProtocol == 4 }  => when possible  in data have 3

                        else if (swicthProtocol == 4)
                        {
                           // if (messageIndexRecieve == 0)
                             //   messageReceivedESA.messageType = data;
                             if(messageIndexRecieve <3) { }
                            else
                            {
                                messageReceivedESA.length = (byte)(data * 6);
                             // statusLog("mdg24 and length msg=" + messageReceivedESA.length+"    "+deviceName);
                            }
                            messageIndexRecieve++;
                            if (messageIndexRecieve == 4)
                            {
                                statusMsgRecv++;
                                messageIndexRecieve = 0;
                            }
                        }
                        //****

                        break;
                    case MessageESAProtocolStatusType.GOT_LEN:
                        messageReceivedESA.data.Add(data);
                        if (++messageIndexRecieve >= messageReceivedESA.length)
                        {
                           // statusLog(string.Format("  recived got length 24 and  get data: {0} MessageId: {1} Data: {2}", DateTime.Now, Convert.ToInt32(Encoding.ASCII.GetString(messageReceivedESA.messageID, 0, 2)), messageReceivedESA.GetBytesString(true)));
                            statusMsgRecv++;
                            messageIndexRecieve = 0;
                        }
                        break;
                    case MessageESAProtocolStatusType.GOT_PAYLOAD:
                        messageReceivedESA.data.Add(data);
                        messageIndexRecieve++;
                        if (messageIndexRecieve >= 2)
                        {
                            string tmp = Encoding.ASCII.GetString(messageReceivedESA.data.GetRange(messageReceivedESA.data.Count - 2, 2).ToArray<byte>());
                            byte chk = 0;
                            try
                            {
                                chk = Convert.ToByte(tmp, 16);

                            }
                            catch (Exception ex)
                            {
                                errorLog("[ESA parseByte - checksum float data response] device: " + deviceName + " - " + ex.Message);
                                goto error;
                            }
                            if (chk == messageReceivedESA.chks)
                            {
                         //  statusLog(string.Format("Data 24 with check sum: {0}", messageReceivedESA.GetBytesString(true)));
                                messageReceivedESA.data.RemoveRange(messageReceivedESA.data.Count - 2, 2);
                                statusMsgRecv++;
                                if (swicthProtocol == 2)
                                {

                                    GData.AddRange(messageReceivedESA.data);

                                    //foreach (byte b in messageReceivedESA.data)
                                    //{
                                    //    swRemoteESA.Write(b.ToString()+",");
                                    //}
                                    //swRemoteESA.WriteLine();
                                    //swRemoteESA.Flush();

                                    //signalR send
                                    if (Properties.Settings.Default.SignalR)
                                    {
                                       statusLog("Sending image data");
                                        OnSendingDataSignalR(deviceType, messageReceivedESA.data);
                                    }

                                    if (writeMode)
                                    {
                                        sw.Write("Grph:");
                                        sw.WriteLine(BitConverter.ToString(messageReceivedESA.data.ToArray()));
                                        sw.Flush();
                                    }
                                }
                                else if (swicthProtocol == 3 || swicthProtocol == 4)
                                   // statusLog("go to parse msg");
                                    parseMessage();

                            }
                            else
                            {
                               // statusLog("chk!=chks"); goto error;
                            }
                              
                        }

                        break;

                    case MessageESAProtocolStatusType.GOT_CHECKSUM:
                        if (data == 0x03)
                        {
                           // statusLog("go to restart");
                            goto restart;
                        }
                        else if (data != 0x03)
                            goto error;
                        //For debug
                        //swRemoteESA.WriteLine();
                        //swRemoteESA.Flush();
                        ///////////////////////////
                        break;
                }
                return;
            error:
                //For debug
                //swRemoteESA.WriteLine("*ERROR*");
                //swRemoteESA.Flush();
                errorCount++;
            ///////////////////////////
            restart: statusMsgRecv = MessageESAProtocolStatusType.UNINIT;
                return;
            }
            catch (Exception ex)
            {
                errorLog("[ESA parseByte] device: " + deviceName + " - " + ex.Message + "- proto:" + swicthProtocol.ToString() + "- statusMsg:" + statusMsgRecv.ToString());
                statusMsgRecv = MessageESAProtocolStatusType.UNINIT;
                //For debug
                //swRemoteESA.WriteLine("*CATCH*");
                //swRemoteESA.Flush();
                ///////////////////////////
            }
        }

        public override void parseMessage()
        {
            try
            {
                timeLastMessageRecieved = DateTime.Now;
                switch (Convert.ToInt32(Encoding.ASCII.GetString(messageReceivedESA.messageID, 0, 2)))
                {
                    case 4: //Read for Alarm and Read Data only for ESA_susPariculate2_5 & 10  (String based)
                        int tmpAlarm = (int)ESAProtocolDataInterpretor(messageReceivedESA.data.GetRange(7, 2).ToArray(), 2, 0);
                        int concentraionStatus = (int)ESAProtocolDataInterpretor(messageReceivedESA.data.GetRange(6, 1).ToArray(), 1, 0);
                        //alarmValue = (concentraionStatus << 13) | tmpAlarm;
                        for (int k = 0; k < polutions.Count; k++)
                        {
                            polutions[k].alarmValue = (concentraionStatus << 13) | tmpAlarm;
                        }
                        //traceLog("ParseMessage DeviceType: " + deviceType + " Alarm: " + alarmValue);

                       if (deviceType == DeviceType.ESA_susPariculate2_5 || deviceType == DeviceType.ESA_susPariculate10 || deviceType == DeviceType.ESA_Organic)
                        {
                            for (int k = 0; k < polutions.Count; k++)
                            {
                                polutions[k].concentration = ESAProtocolDataInterpretor(messageReceivedESA.data.ToArray(), 8, 9 + 8*k);
                            }
                            waitForResponseList[1].Set();
                            workingStatus = true;

                            traceLog("ParseMessage Particulate Concentration: " + polutions[0].concentration);
                        }

                        break;
                    case 5:
                    case 6:
                    case 7:
                    case 8: //Start Calibration
                        waitForResponseList[2].Set();
                        break;
                    case 9: //Stop Calibration
                        waitForResponseList[3].Set();
                        break;
                    case 24: // read Float data  (binary based )
                        {
                          //  statusLog("strat parsmsg for case 24");
                            // Nox Data 
                            // 6 {65 67 51 50: Device ID: AC32} {50 52:Message ID:24} {1:?} {3:Number of Data} {2 0:Seprator ?} {64 231 0 126 : float Value 1 : NO} {2 0} {65 162 227 54} {2 0} {65 82 70 45} {56 54:Checksum} {3:ETX}

                            for (int k = 0; k < polutions.Count; k++)
                            {
                                polutions[k].concentration = getFloatNumESA(messageReceivedESA.data, 2 + 6 * k); /// 6:  {2, 0,  4-byte[Float Value Conc]}  
                            }
                            waitForResponseList[1].Set();
                            workingStatus = true;
                            //  Find the Alarms in float format -> No Such Thing
                        }
                        break;
                }
                
                if (writeMode)
                {
                    //sw.WriteLine(string.Format("{0} MessageId: {1} Data: {2}", DateTime.Now, Convert.ToInt32(Encoding.ASCII.GetString(messageReceivedESA.messageID, 0, 2)), BitConverter.ToString(messageReceivedESA.data.ToArray())));
                    sw.WriteLine(string.Format("{0} MessageId: {1} Data: {2}", DateTime.Now, Convert.ToInt32(Encoding.ASCII.GetString(messageReceivedESA.messageID, 0, 2)), messageReceivedESA.GetBytesString(true)));
                    sw.Flush();
                }

            }
            catch (Exception ex)
            {
                errorLog("[ESA parseMessage] device: " + deviceName + " - " + ex.Message);
            }
        }

        #endregion Parse Incoming Data

        #region Calibrate
        public override void Calibrate()
        {


            if (workingStatus && calibParam != null)
            {
                // set valve to calibration
                MessageESAProtocol msg = new MessageESAProtocol();

                switch (calibParam.calibType)
                {
                    case CalibrationType.CheckZero:
                        msg = new MessageESAProtocol(Encoding.UTF8.GetBytes(DeviceID), new byte[] { 48, 54 }, Encoding.UTF8.GetBytes(calibParam.CalibWaitTimeSec.ToString("D4")));
                        break;
                    case CalibrationType.CheckSpan:
                        msg = new MessageESAProtocol(Encoding.UTF8.GetBytes(DeviceID), new byte[] { 48, 56 }, Encoding.UTF8.GetBytes(calibParam.CalibWaitTimeSec.ToString("D4")));
                        break;
                    case CalibrationType.Zero:
                        msg = new MessageESAProtocol(Encoding.UTF8.GetBytes(DeviceID), new byte[] { 48, 53 }, Encoding.UTF8.GetBytes(calibParam.CalibWaitTimeSec.ToString("D4")));
                        break;
                    case CalibrationType.Span:
                        msg = new MessageESAProtocol(Encoding.UTF8.GetBytes(DeviceID), new byte[] { 48, 55 }, Encoding.UTF8.GetBytes(calibParam.CalibWaitTimeSec.ToString("D4")));
                        break;

                }

                if (deviceType == DeviceType.ESA_MicroStation && calibParam.calibType == CalibrationType.CheckSpan)
                {
                    waitForResponseList[4] = new AutoResetEvent(false);
                    sendRemoteCommand(19);

                    Thread.Sleep(500);
                    getRemoteScreen();

                    if (waitForResponseList[4].WaitOne(maximumResponseTime))
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
                }
                else
                {
                    sendESAMessage(msg, 2);
                    if (waitForResponseList[2].WaitOne(maximumResponseTime))
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
                }

                waitAndsendConcAtEndCalib();

                if (true) //..............// feedback query for Calibration Coeff
                {
                    dilutionCalibrator._calibrationProcessStatus = CalibrationProcessStatus._4AnalyzerFinishedCalib;
                }

                stopCalib(false);
            }
            else
            {
                calibErrorIDs errID = calibErrorIDs.Analyzer_Not_Working;
                errorLog("[Calibration] " + calibParam.calibType.ToString() + " is NOT Started on : " + deviceType.ToString() + " - " + errID.ToString(), 2, (byte)errID);
            }
        }

        protected override void stopCalib(bool forceFully)
        {
            bool success = false;
            if (workingStatus && calibParam != null)
            {
                if (!forceFully || (dilutionCalibrator.calibrationDeviceState.HasFlag(CalibrationDeviceState.AnalyzerInCalibMode)))
                {
                    MessageESAProtocol msg = new MessageESAProtocol(Encoding.UTF8.GetBytes(DeviceID), new byte[] { 48, 57 }, new byte[] { });
                    for (int i = 0; i < maxTryCalibStop; i++)
                    {
                        if (deviceType == DeviceType.ESA_MicroStation && calibParam.calibType == CalibrationType.CheckSpan)
                        {
                            waitForResponseList[5] = new AutoResetEvent(false);
                            sendRemoteCommand(17);

                            Thread.Sleep(500);
                            getRemoteScreen();

                            if (waitForResponseList[5].WaitOne(maximumResponseTime))
                            {
                                success = true;
                                dilutionCalibrator.calibrationDeviceState &= ~CalibrationDeviceState.AnalyzerInCalibMode;
                                dilutionCalibrator._calibrationProcessStatus = CalibrationProcessStatus._5AnalyzerExitCalib;
                                break;
                            }

                        }
                        else
                        {
                            sendESAMessage(msg, 3);

                            if (waitForResponseList[3].WaitOne(maximumResponseTime))
                            {
                                success = true;
                                dilutionCalibrator.calibrationDeviceState &= ~CalibrationDeviceState.AnalyzerInCalibMode;
                                dilutionCalibrator._calibrationProcessStatus = CalibrationProcessStatus._5AnalyzerExitCalib;
                                break;
                            }
                        }

                    }
                    if (!success)
                    {
                        calibErrorIDs errID = calibErrorIDs.Analyzer_Exiting_Calib_Command_Failed;
                        errorLog("[Calibration] " + calibParam.calibType.ToString() + " is NOt stoped on : " + deviceType.ToString() + " - " + errID.ToString(), 2, (byte)errID);
                    }
                }
                base.stopCalib(forceFully);
            }
            else
            {
                calibErrorIDs errID = calibErrorIDs.Analyzer_Exiting_Calib_Command_Failed_NOT_WORKING;
                errorLog("[Calibration] " + calibParam.calibType.ToString() + " is NOT Stoped on : " + deviceType.ToString() + " - " + errID.ToString(), 2, (byte)errID);
            }
        }
        #endregion

        #region Remote
        int[] PrePosition = new int[2];
        int Style = 1;
        byte[] globalCursor = new byte[2];
        byte Addr_Var, highlightLength;
        List<byte> GData = new List<byte>();
        byte graphicMode = 1; // Mode 1: Draw, Mode 0: Clear

        System.Threading.Timer remoteTimer;
        int counterRemoteTimer = 0;

        const int ESARemoteScreenHeight = 128, ESARemoteScreenWidth = 240;
        int Scale = 2;

        Bitmap textESAbitmap;
        Bitmap graphicESAbitmap;

        Graphics textObject;
        Graphics graphicObject;

        public override void sendRemoteCommand(byte command)
        {
            sendESAGraphicCommand(66, command);
            if (command > 13 && command < 20) //What is this
            {
                startESARemoteTimer();
            }
        }

        public override void startRemote()
        {
            if (!remoteMode)
            {
                base.startRemote();

                //sendESAGraphicCommand(49, 50);
                //sendESAGraphicCommand(49, 50);
                sendESAGraphicCommand(66, 14); //back
                Thread.Sleep(500);
                sendESAGraphicCommand(66, 19); //enter
                //Thread.Sleep(500);

                startESARemoteTimer();
            }

        }

        private void startESARemoteTimer()
        {
            stopESARemoteTimer();
            System.Threading.TimerCallback callback = new System.Threading.TimerCallback(graphicESATick);
            Thread.Sleep(500);
            remoteTimer = new System.Threading.Timer(callback, null, 0, 500);
        }

        private void stopESARemoteTimer()
        {
            counterRemoteTimer = 0;
            if (remoteTimer != null)
            {
                remoteTimer.Change(Timeout.Infinite, Timeout.Infinite);
                remoteTimer.Dispose();
                remoteTimer = null;
            }
        }

        private void graphicESATick(object input)
        {
            //int index = (int)input;
            sendRemoteCommand(13);
            //if (counterRemoteTimer++ >= 100)
            //    stopRemote();
        }

        private void sendESAGraphicCommand(byte Command1, byte Command2)
        {
            MessageESAProtocol msg;
            msg = new MessageESAProtocol(Encoding.UTF8.GetBytes(DeviceID), new byte[] { Command1, Command2 }, new byte[] { });
            sendESAMessage(msg, 1);
        }

        public override void stopRemote()
        {
            base.stopRemote();

            stopESARemoteTimer();

        }

        public override void prepareRemoteScreen()
        {
            try
            {
                Thread.Sleep(200);
                for (byte k = 0; k < 5; k++)
                {
                    sendRemoteCommand(13);
                    Thread.Sleep(300);
                }
                Thread.Sleep(300);
                waitForResponseList[213].Set();
            }
            catch (Exception ee)
            {
                errorLog("Remote Screen Problem : " + ee.Message);
            }
        }

        public override Bitmap getRemoteScreen()
        {

            //statusLog("Esa getRemoteScreen called");
            //var count = 0;

            //var str = "6 51 49 53 54 27 235 0 1 0 11 0 4 0 0 0 7 0 9 0 15 0 0 0 12 0 32 32 1 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 0 8 0 2 0 3 0 1 0 9 0 5 0 1 0 19 0 98 0 118 0 105 0 118 0 19 0 99 0 117 0 104 0 117 0 19 0 100 0 116 0 103 0 116 0 23 0 101 0 115 0 102 0 111 0 23 0 96 0 110 0 107 0 107 0 23 0 97 0 107 0 106 0 103 0 23 0 88 0 105 0 115 0 78 0 19 0 97 0 102 0 94 0 101 0 22 0 92 0 99 0 22 0 90 0 94 0 22 0 90 0 89 0 22 0 92 0 84 0 22 0 94 0 82 0 22 0 97 0 81 0 19 0 106 0 102 0 109 0 101 0 22 0 111 0 99 0 22 0 113 0 94 0 22 0 113 0 89 0 22 0 111 0 84 0 22 0 109 0 82 0 22 0 106 0 81 0 68 67 3 6 51 49 53 54 27 234 0 23 0 97 0 80 0 106 0 78 0 19 0 58 0 45 0 57 0 45 0 22 0 52 0 50 0 22 0 52 0 51 0 19 0 51 0 52 0 51 0 54 0 19 0 50 0 55 0 50 0 59 0 19 0 51 0 60 0 51 0 62 0 19 0 52 0 63 0 52 0 64 0 22 0 57 0 69 0 22 0 58 0 69 0 19 0 59 0 70 0 61 0 70 0 19 0 62 0 71 0 68 0 71 0 19 0 69 0 70 0 71 0 70 0 19 0 72 0 69 0 73 0 69 0 22 0 78 0 64 0 22 0 78 0 63 0 19 0 79 0 62 0 79 0 60 0 19 0 80 0 59 0 80 0 55 0 19 0 79 0 54 0 79 0 52 0 19 0 78 0 51 0 78 0 50 0 22 0 73 0 45 0 20 0 60 0 45 0 72 0 19 0 43 0 49 0 51 0 49 0 19 0 60 0 55 0 60 0 59 0 22 0 63 0 62 0 22 0 67 0 62 0 22 0 70 0 59 0 22 0 70 0 55 0 67 53 3 6 51 49 53 54 27 240 0 22 0 67 0 52 0 22 0 63 0 52 0 22 0 60 0 55 0 18 0 57 0 65 0 19 0 65 0 72 0 67 0 72 0 19 0 68 0 73 0 70 0 73 0 19 0 71 0 74 0 73 0 74 0 19 0 74 0 75 0 76 0 75 0 19 0 77 0 76 0 79 0 76 0 22 0 81 0 74 0 22 0 81 0 72 0 22 0 79 0 70 0 22 0 77 0 70 0 22 0 75 0 72 0 22 0 75 0 74 0 18 0 73 0 78 0 19 0 124 0 76 0 126 0 76 0 22 0 128 0 74 0 22 0 128 0 72 0 22 0 126 0 70 0 22 0 124 0 70 0 22 0 122 0 72 0 22 0 122 0 74 0 22 0 124 0 76 0 18 0 73 0 125 0 19 0 127 0 75 0 129 0 75 0 19 0 130 0 74 0 132 0 74 0 19 0 133 0 73 0 135 0 73 0 20 0 136 0 72 0 138 0 20 0 139 0 71 0 141 0 23 0 110 0 68 0 146 0 67 0 23 0 147 0 68 0 148 0 63 0 57 57 3 6 51 49 53 54 27 232 0 23 0 149 0 63 0 178 0 64 0 19 0 164 0 65 0 164 0 80 0 19 0 165 0 81 0 173 0 81 0 19 0 166 0 71 0 166 0 68 0 19 0 167 0 71 0 167 0 66 0 19 0 168 0 71 0 168 0 68 0 19 0 166 0 56 0 166 0 59 0 19 0 167 0 56 0 167 0 61 0 19 0 168 0 56 0 168 0 59 0 19 0 170 0 65 0 170 0 72 0 19 0 171 0 73 0 173 0 73 0 19 0 187 0 68 0 196 0 59 0 22 0 209 0 59 0 22 0 209 0 68 0 22 0 187 0 68 0 19 0 210 0 64 0 212 0 64 0 19 0 215 0 69 0 179 0 69 0 22 0 179 0 58 0 22 0 182 0 58 0 22 0 196 0 54 0 19 0 195 0 54 0 210 0 58 0 22 0 215 0 58 0 23 0 195 0 53 0 196 0 48 0 23 0 197 0 49 0 229 0 48 0 19 0 230 0 52 0 230 0 45 0 19 0 231 0 51 0 231 0 46 0 55 70 3 6 51 49 53 54 27 209 0 19 0 232 0 50 0 232 0 47 0 19 0 233 0 49 0 233 0 48 0 2 0 3 0 1 0 9 0 5 0 1 0 9 0 0 0 2 0 12 0 47 32 32 47 32 32 32 32 32 32 32 58 32 32 58 0 9 0 0 0 38 0 12 0 248 67 0 9 0 3 0 0 0 12 0 65 80 0 11 0 61 0 9 0 3 0 7 0 12 0 109 98 97 114 0 9 0 4 0 0 0 12 0 65 84 0 11 0 61 0 9 0 4 0 7 0 12 0 248 67 0 9 0 5 0 0 0 12 0 82 72 0 11 0 61 0 9 0 5 0 7 0 12 0 37 0 9 0 6 0 0 0 12 0 72 84 0 11 0 61 0 9 0 6 0 7 0 12 0 248 67 0 9 0 5 0 29 0 12 0 80 0 11 0 49 0 11 0 61 0 9 0 5 0 36 0 12 0 109 98 97 114 0 9 0 6 0 29 0 51 53 3";

            //foreach (var s in str.Split(' '))
            //{
            //    GData.Add(Convert.ToByte(s));
            //}

            {
            FIRST:
                while (GData.Count > 1)
                {
                    try
                    {
                        byte Identifier = GData[0]; GData.RemoveRange(0, 2);

                        switch (Identifier)
                        {
                            case 1: // Clear Screen
                                if (GData[0] == 11)
                                {
                                    ClearScreen(Scale, true, true);
                                    GData.RemoveRange(0, 2);
                                }
                                break;
                            case 9: // Getting Coordinates
                                globalCursor[0] = GData[0];
                                globalCursor[1] = GData[2];
                                Addr_Var = GData[2];
                                GData.RemoveRange(0, 4);
                                cntString = 0;
                                break;
                            case 10: // Highlighting
                                globalCursor[0] = GData[0];
                                globalCursor[1] = GData[2];
                                highlightLength = GData[4];
                                int Y = Convert.ToInt32(globalCursor[0]); int X = Convert.ToInt32(globalCursor[1]);

                                if (X == 34) // This is check for F6 clicking: F6 in instantanous page means "Span" Port.
                                    if (waitForResponseList.ContainsKey(4))
                                        waitForResponseList[4].Set();
                                if (X == 20) // This is check for F4 clicking: F6 in instantanous page means "Sample" Port.
                                    if (waitForResponseList.ContainsKey(5))
                                        waitForResponseList[5].Set();

                                int W0 = Convert.ToInt32(Math.Round(ESARemoteScreenWidth / 40.0 * Scale)); int W = X * W0;
                                int H0 = Convert.ToInt32(Math.Round(ESARemoteScreenHeight / 16.0 * Scale)); int H = Y * H0;

                                if (GData[6] == 2)
                                    textObject.DrawRectangle(new Pen(Brushes.White), W - 1, H - 1, W0 * highlightLength + 2, H0 + 2);
                                else if (GData[6] == 0)
                                    textObject.DrawRectangle(new Pen(Brushes.Black), W - 1, H - 1, W0 * highlightLength + 2, H0 + 2);
                                else if (GData[6] == 3)
                                    textObject.DrawRectangle(new Pen(Brushes.Red), W - 1, H - 1, W0 * highlightLength + 2, H0 + 2);

                                GData.RemoveRange(0, 8);
                                break;
                            case 12: { DRAWstr(GData); goto FIRST; }  // Drawing Strings
                            case 11: { DRAWstr(GData); goto FIRST; }
                            case 5: { graphicMode = GData[0]; GData.RemoveRange(0, 2); goto FIRST; }  // Start Drawing Objects
                            case 7: { goto FIRST; } // Propbably Clear Text Screen (Not Needed Always after 1 0 11 0)
                            case 8: { ClearScreen(Scale, true, false); GData.RemoveRange(0, 6); goto FIRST; } // Clear Graphic Screen
                            case 2: { GData.RemoveRange(0, 4); goto FIRST; } // Mystery 2 0 3 0 1 0
                            case 19: { Draw19(GData.Take(8).ToArray()); GData.RemoveRange(0, 8); goto FIRST; } // Drawing 19: LINE
                            case 20: { Draw20(GData.Take(6).ToArray()); GData.RemoveRange(0, 6); goto FIRST; } // Drawing 20: SEMI-LINE
                            case 21: { Draw21(GData.Take(6).ToArray()); GData.RemoveRange(0, 6); goto FIRST; } // Drawing 21: SEMI-LINE
                            case 22: { Draw22(GData.Take(4).ToArray()); GData.RemoveRange(0, 4); goto FIRST; } // Drawing 22: SEMI-LINE
                            case 23: { Draw23(GData.Take(8).ToArray()); GData.RemoveRange(0, 8); goto FIRST; } // Drawing 23: RECTANGLE
                            case 4:   // Changing String Style
                                Style = GData[0];
                                if (Style == 0)
                                    Style = 4;
                                GData.RemoveRange(0, 2);
                                goto FIRST;
                            default:
                                errorLog("[Remote ESA Unknown Command] device: " + deviceName + " - " + Identifier.ToString());
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        errorLog("[Remote ESA Drawing] device: " + deviceName + " - " + ex.Message);
                    }
                }
            }
            return createBitmapImage();
        }

        public Bitmap createBitmapImage()
        {
            Bitmap ESAbitmap = new Bitmap(textESAbitmap.Width, textESAbitmap.Height);

            using (Graphics g = Graphics.FromImage(ESAbitmap))
            {
                //set background color
                g.Clear(Color.Black);

                g.DrawImage(textESAbitmap, new Rectangle(0, 0, textESAbitmap.Width, textESAbitmap.Height));
                g.DrawImage(graphicESAbitmap, new Rectangle(0, 0, textESAbitmap.Width, textESAbitmap.Height));

                //g.DrawString("Hello ESA",new Font("Ms San Serif", 20.0f), System.Drawing.Brushes.Red, 10,10);
                //g.DrawString("remote",new Font("Ms San Serif", 20.0f), System.Drawing.Brushes.White, 100,100);
            }

            return ESAbitmap;

        }

        #endregion Remote

        # region Basic Function for Drawing Remote Screens
        //string str2Draw="";
        int cntString = 0;

        public void ClearScreen(int scale, bool clearGraphics, bool clearText)
        {

            if (clearText)
            {
                textESAbitmap = new Bitmap(ESARemoteScreenWidth * scale, ESARemoteScreenHeight * scale); // ,PixelFormat.Format8bppIndexed
                textObject = Graphics.FromImage(textESAbitmap);
            }
            if (clearGraphics)
            {
                graphicESAbitmap = new Bitmap(ESARemoteScreenWidth * scale, ESARemoteScreenHeight * scale);
                graphicObject = Graphics.FromImage(graphicESAbitmap);
            }
        }// Clear screen
        void Drawing(int[] Pos, string Type)
        {
            int[] PosC = Pos;

            switch (Type)
            {
                case "Line":
                    {
                        PosC[1] = ESARemoteScreenHeight * Scale - Pos[1];
                        PosC[3] = ESARemoteScreenHeight * Scale - Pos[3];
                        if (graphicMode == 1)
                            graphicObject.DrawLine(Pens.Yellow, PosC[0], PosC[1], PosC[2], PosC[3]);
                        else
                            graphicObject.DrawLine(Pens.Black, PosC[0], PosC[1], PosC[2], PosC[3]);
                        break;
                    }
                case "Rec":
                    {
                        PosC[1] = ESARemoteScreenHeight * Scale - Pos[1];
                        PosC[3] = ESARemoteScreenHeight * Scale - Pos[3];
                        if (graphicMode == 1)
                            graphicObject.DrawRectangle(Pens.Yellow, (PosC[0] < PosC[2]) ? PosC[0] : PosC[2], (PosC[1] < PosC[3]) ? PosC[1] : PosC[3], (PosC[0] < PosC[2]) ? PosC[2] - PosC[0] : PosC[0] - PosC[2], (PosC[1] < PosC[3]) ? PosC[3] - PosC[1] : PosC[1] - PosC[3]);
                        else
                            graphicObject.DrawRectangle(Pens.Black, (PosC[0] < PosC[2]) ? PosC[0] : PosC[2], (PosC[1] < PosC[3]) ? PosC[1] : PosC[3], (PosC[0] < PosC[2]) ? PosC[2] - PosC[0] : PosC[0] - PosC[2], (PosC[1] < PosC[3]) ? PosC[3] - PosC[1] : PosC[1] - PosC[3]);
                        break;
                    }
            }
        }                     // Basic Drawing
        void Draw12(byte chr, int Style)
        {
            string fontName = "Courier New";
            int[] fontsize = new int[] { 10, 14, 18 };

            if (chr == 248) { chr = 176; }
            if (chr == 1) { chr = 37; fontName = "Wingdings 3"; }
            if (chr == 2) { chr = 33; fontName = "Wingdings 3"; }
            if (chr == 3) { chr = 34; fontName = "Wingdings 3"; }
            if (chr == 4) { chr = 35; fontName = "Wingdings 3"; }
            if (chr == 5) { chr = 36; fontName = "Wingdings 3"; }
            if (chr == 6) { chr = 56; fontName = "Wingdings 3"; }

            byte[] INFO = new byte[1]; INFO[0] = chr;
            string SINFO = Encoding.GetEncoding("iso-8859-1").GetString(INFO);

            int Y = Convert.ToInt32(globalCursor[0]); int X = Convert.ToInt32(globalCursor[1]);

            int W0 = Convert.ToInt32(Math.Round(ESARemoteScreenWidth / 40.0 * Scale)); int W = X * W0;
            int H0 = Convert.ToInt32(Math.Round(ESARemoteScreenHeight / 16.0 * Scale)); int H = Y * H0;

            if (Style == 3 || Style == 2)  //  Style 2 is inverse and Style 3 is blinking
            {
                Font font1 = new System.Drawing.Font(fontName, fontsize[0], FontStyle.Regular, GraphicsUnit.Point);
                textObject.FillRectangle(Brushes.Yellow, W, H, W0, H0);
                textObject.DrawString(SINFO, font1, System.Drawing.Brushes.Black, W, H);
            }

            if (Style == 4)
            {
                Font font1 = new System.Drawing.Font(fontName, fontsize[0], FontStyle.Regular, GraphicsUnit.Point);
                SizeF stringSize = textObject.MeasureString(SINFO, font1);
                textObject.FillRectangle(Brushes.Black, W, H, W0, stringSize.Height); //H0
                textObject.DrawString(SINFO, font1, System.Drawing.Brushes.Yellow, W, H);
            }
            if (Style == 5)
            {
                int S = Convert.ToInt32(Math.Round(1.1 * W0));
                int S2 = Convert.ToInt32(Math.Round(1.2 * H0));
                int WW = W + Convert.ToInt32(Math.Round(0.05 * W0 * cntString++));
                Font font1 = new System.Drawing.Font(fontName, fontsize[1], FontStyle.Bold, GraphicsUnit.Point);
                textObject.FillRectangle(Brushes.Black, WW, H, S, S2);
                textObject.DrawString(SINFO, font1, System.Drawing.Brushes.Yellow, WW, H);
            }
            if (Style == 7)
            {
                int S = Convert.ToInt32(Math.Round(1.3 * W0));
                int S2 = Convert.ToInt32(Math.Round(1.3 * H0));
                int WW = W + Convert.ToInt32(Math.Round(0.3 * W0 * cntString++));
                int HH = Convert.ToInt32(Math.Round(1.0 * H));
                Font font1 = new System.Drawing.Font(fontName, S, FontStyle.Bold, GraphicsUnit.Point);
                textObject.FillRectangle(Brushes.Black, WW, H, S, S2);
                textObject.DrawString(SINFO, font1, System.Drawing.Brushes.Yellow, WW, H);
            }


            //if (Style == 4)
            //{
            //    Font font1 = new System.Drawing.Font(fontName, fontsize[0], FontStyle.Regular, GraphicsUnit.Point);
            //    SizeF stringSize = textObject.MeasureString(str2Draw, font1);
            //    textObject.FillRectangle(Brushes.Black, W, H, (int)stringSize.Width, (int)stringSize.Height);
            //    textObject.DrawString(str2Draw, font1, System.Drawing.Brushes.Yellow, W, H);
            //}
            //if (Style == 5)
            //{
            //    Font font1 = new System.Drawing.Font(fontName, fontsize[1], FontStyle.Bold, GraphicsUnit.Point);
            //    SizeF stringSize = textObject.MeasureString(str2Draw, font1);               
            //    int S = (int)stringSize.Width;
            //    int S2 = (int)stringSize.Height;
            //    textObject.FillRectangle(Brushes.Black, W, H, S, S2);
            //    textObject.DrawString(str2Draw, font1, System.Drawing.Brushes.Yellow, W, H);
            //}
            //if (Style == 7)
            //{
            //    Font font1 = new System.Drawing.Font(fontName, fontsize[2], FontStyle.Bold, GraphicsUnit.Point);
            //    SizeF stringSize = textObject.MeasureString(str2Draw, font1);
            //    int S = (int)stringSize.Width;
            //    int S2 = (int)stringSize.Height;
            //    textObject.FillRectangle(Brushes.Black, W, H, S, S2 );
            //    textObject.DrawString(str2Draw, font1, System.Drawing.Brushes.Yellow, W, H);
            //}
            //str2Draw = "";
        }                         // Drawing STRING
        void Draw19(byte[] Info)
        {
            int[] Pos = new int[4];
            for (int i = 0; i < 4; i++)
                Pos[i] = Convert.ToInt32(Math.Round(Convert.ToDouble(Info[2 * i])) * Scale);

            PrePosition = Pos.Skip(2).Take(2).ToArray();
            Drawing(Pos, "Line");
        }                                 // Drawing LINE
        void Draw20(byte[] Info)
        {
            int[] Pos = new int[4];
            for (int i = 0; i < 3; i++)
                Pos[i] = Convert.ToInt32(Math.Round(Convert.ToDouble(Info[2 * i])) * Scale);
            Pos[3] = Convert.ToInt32(Math.Round(Convert.ToDouble(Info[2])) * Scale);
            PrePosition = Pos.Skip(2).Take(2).ToArray();
            Drawing(Pos, "Line");
        } // Drawing SEMI-LINE
        void Draw21(byte[] Info)
        {
            int[] Pos = new int[4];
            for (int i = 0; i < 3; i++)
                Pos[i] = Convert.ToInt32(Math.Round(Convert.ToDouble(Info[2 * i])) * Scale);
            Pos[3] = Convert.ToInt32(Math.Round(Convert.ToDouble(Info[2])) * Scale);
            PrePosition = Pos.Skip(2).Take(2).ToArray();
            Drawing(Pos, "Line");
        } // Drawing SEMI-LINE
        void Draw22(byte[] Info)
        {
            int[] Pos = new int[4];
            for (int i = 2; i < 4; i++)
                Pos[i] = Convert.ToInt32(Math.Round(Convert.ToDouble(Info[2 * (i - 2)])) * Scale);

            Pos[0] = PrePosition[0]; Pos[1] = PrePosition[1];
            PrePosition = Pos.Skip(2).Take(2).ToArray();
            Drawing(Pos, "Line");
        }                                 // Drawing SEMI-LINE
        void Draw23(byte[] Info)
        {
            int[] Pos = new int[4];
            for (int i = 0; i < 4; i++)
                Pos[i] = Convert.ToInt32(Math.Round(Convert.ToDouble(Info[2 * i])) * Scale);

            PrePosition = Pos.Skip(2).Take(2).ToArray();
            Drawing(Pos, "Rec");
        }                                 // Drawing RECTANGLE
        void DRAWstr(List<byte> LData)
        {
            //Draw12(LData[0], Style);
            //LData.RemoveRange(0, 1);
            while (LData[0] != 0)
            {
                // Update Address for the next character               
                Draw12(LData[0], Style);
                globalCursor[1] = ++Addr_Var;
                LData.RemoveRange(0, 1);
            }
            LData.RemoveRange(0, 1);

            //int cnt = 0;
            //while (LData[0] != 0 || cnt++>50)
            //{
            //byte Info = LData[0];

            //if (Info == 248) { Info = 176; }

            //byte[] INFO = new byte[1]; INFO[0] = Info;
            //str2Draw += Encoding.UTF8.GetString(INFO); //GetEncoding("iso-8859-1")
            //LData.RemoveRange(0, 1);

            //}
            //if (LData[0] == 0)
            //{
            //    Draw12(Style);
            //    LData.RemoveRange(0, 1);
            //}
        }         // Drawing STRING (Group)
        # endregion Basic Function for Drawing Remote Screens

        public override byte getStatus(int alarm)
        {
            byte state = base.getStatus(alarm);

            if ((/*_alarmValue*/alarm & (1 << 14)) > 0)
                state = 3; // Span

            if ((/*_alarmValue*/alarm & (1 << 13)) > 0)
                state = 2; // Zero


            return state;
        }

        #region Section to Handle UDP Connection Recieve and Send

        private Thread receiveUdpThread;
        internal override void initialize_Connect()
        {
            if (portCOM.Contains(":"))
            {
                
                //lient.AddIPs(portCOM);

                //receiveUdpThread = new Thread( () => { ReceiveDataFunc(portCOM); });
                //receiveUdpThread.Start();

                try
                {
                    if (udpclient != null)
                    {
                        udpclient.Close();
                        udpclient = null;
                    }

                    udpclient = new UdpClient(baudrate);  // use baudrate as port 

                    udpclient.BeginReceive(new AsyncCallback(ReceiveDataFunc), null);
                }
                catch (Exception ex)
                {
                    errorLog("Begin Receive ESA device:" + deviceName + " error: " + ex.Message);
                }

                isConnected = true; // client.Connected;
            }


            base.initialize_Connect();
        }

        // Function to recieve UDP bytes and  Delivering to parsebyte of this Class
        private void ReceiveDataFunc(IAsyncResult res)
        {
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, baudrate);

            try
            {
                if (udpclient == null)
                    return;

                byte[] received = udpclient.EndReceive(res, ref RemoteIpEndPoint);

                //Process codes
                foreach (byte dataByte in received)
                        parseByte(dataByte);

                udpclient.BeginReceive(new AsyncCallback(ReceiveDataFunc), null);
            }
            catch (Exception ex)
            {
                errorLog("ReceiveDataFunc ESA device:" + deviceName + " error: " + ex.Message);
            }

            
        }

        //public void ReceiveDataFunc(string IP_port)
        //{
        //    try
        //    {
        //        //string strIP_port = IP_port.ToString();
        //        string IP = IP_port.Split(':')[0];
        //        int Port = Convert.ToInt16((IP_port.Split(':')[1]));
        //        IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(IP), Port);
        //        while (udpclient != null)
        //        {
        //            byte[] data = udpclient.Receive(ref ipend);
        //            foreach (byte dataByte in data)
        //                parseByte(dataByte);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        errorLog("ReceiveDataFunc ESA device:"+ deviceName + " error: " + ex.Message);
        //    }
        //}

        //private void Client_ErrorCommunication(string ErrorMessage)
        //{
        //    if (client != null)
        //        isConnected = client.Connected;
        //}

        //private void Client_SuccessCommunication(object sender, EventArgs e)
        //{
        //    try
        //    {
        //    }
        //    catch
        //    {
        //        workingStatus = false;
        //        //isConnected = client.Connected;
        //    }
        //}
        #endregion Section to Handle UDP Recieve and Send


        internal override void Disconnect()
        {
            base.Disconnect();

            if (conSerial.IsOpen)
                conSerial.Close();

            if (udpclient != null)
            {
                udpclient.Close();
                udpclient = null;
            }
        }
    }
}