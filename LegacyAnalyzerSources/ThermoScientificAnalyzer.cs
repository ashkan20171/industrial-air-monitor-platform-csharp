using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace SadraAQMS.Analyzers
{
    public class ThermoScientificAnalyzer : deviceAnalyzer
    {
        // Dictionary<short, byte> ThermoID2PollutionID = new Dictionary<short, byte>();
        Client client;

        public ThermoScientificAnalyzer(DeviceType dvType)
        {
            //this.company = DeviceCompany.Thermo;
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();

            //_alarmValue = 0;

            //initialize_Connect();

            //StreamReader sr = new StreamReader("Thermo.id");

            //string data = sr.ReadToEnd();
            //string[] tmpIDmatching = data.Split(';');

            //// enable / horiba id / pollution id / unit /id
            //foreach (string keyPair in tmpIDmatching)
            //{
            //    string[] tmpKeyPair = keyPair.Split(',');
            //    if (Convert.ToByte(tmpKeyPair[0]) == 1)
            //    {
            //        polutions.Add(new polutionType(Convert.ToByte(tmpKeyPair[2]), Convert.ToByte(tmpKeyPair[3])));
            //        ThermoID2PollutionID.Add(Convert.ToInt16(tmpKeyPair[1]), Convert.ToByte(tmpKeyPair[2]));
            //    }
            //}
        }

        public ThermoScientificAnalyzer(DeviceType dvType, string devID, string devName, string ignorableErrors)
        {
            // this.company = DeviceCompany.Thermo;
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            this.DeviceName = devName;
            int.TryParse(ignorableErrors, out IgnorableErrors);

            //_alarmValue = 0;

            //initialize_Connect();

            //StreamReader sr = new StreamReader("Thermo.id");

            //string data = sr.ReadToEnd();
            //string[] tmpIDmatching = data.Split(';');

            //// enable / horiba id / pollution id / unit /id
            //foreach (string keyPair in tmpIDmatching)
            //{
            //    string[] tmpKeyPair = keyPair.Split(',');
            //    if (Convert.ToByte(tmpKeyPair[0]) == 1)
            //    {
            //        polutions.Add(new polutionType(Convert.ToByte(tmpKeyPair[2]), Convert.ToByte(tmpKeyPair[3])));
            //        ThermoID2PollutionID.Add(Convert.ToInt16(tmpKeyPair[1]), Convert.ToByte(tmpKeyPair[2]));
            //    }
            //}
        }

        //public bool IsAlive = false;

        public override void sendReadCommand()
        {
            //LastMessage = "18-03-17;09:41:42;#01;11.8;687.3;1;#02;0.6;12.8;1;#03;202.8;3734.7;1;#04;0.0;147.1;1;#05;29.8;4837.3;1;#06;115.1;192.9;1;#07;45.0;2.4;1;#08;34.6;18.3;1;#09;3333.0;0;#10;3333.0;0;#11;3333.0;0;***";
            //parseMessage();
            
            if (!workingStatus)
            {
                try
                {
                    //string tmp = "@#iny\r\n";
                    //  Gesytec(Bayern - Hessen) Protocol  <stx> DA <etx>  04
                    var command = new byte[] { 0x02, 0x44, 0x41, 0x03, 0x30, 0x34 };

                    List<byte> tcpSendBytes = new List<byte>();
                    tcpSendBytes.AddRange(command);
                    
                    SendData(tcpSendBytes.ToArray());
                }
                catch
                {
                    workingStatus = false;
                    isConnected = client.Connected;
                }

            }
        }

        private void SendData(byte[] data)
        {
            if (portCOM.Contains(":") && client != null && client.Connected)
            {
                client.SendData(data);
            }
            else
            {
                messageSendOnSerialBytesAwaitingResponse(data, 1);
            }
        }

        internal override void initialize_Connect()
        {
            //string[] IPdata = portCOM.Split(':');
            //string IP = IPdata[0];
            //ushort Port = Convert.ToUInt16(IPdata[1]);

            try
            {
                if (portCOM.Contains(":"))
                {
                    statusLog("start client for Thermo");

                    client = new Client();
                    client.AddIPs(portCOM);

                    client.ReceiveData += new SadraAQMS.Client.DataReceived(ReceiveDataFunc);
                    client.ErrorCommunication += Client_ErrorCommunication;
                    client.Connect();

                    isConnected = client.Connected;


                    //tcpClient.Connect(IP, Port);

                    //stream = tcpClient.GetStream();

                    //writer = new BinaryWriter(stream);
                    //reader = new BinaryReader(stream);

                    //thdListenTCP = new Thread(new ThreadStart(ListenTCP));
                    //thdListenTCP.IsBackground = true;
                    //thdListenTCP.Start();

                }
                base.initialize_Connect();

                //if (writeMode)
                //    sw = new StreamWriter(string.Format(writePath, "ThermoAnalyzer"));
            }
            catch (Exception ex)
            {
                errorLog("TCP conncetion problem for device analyzer Thermo: " + ex.Message);
            }
        }

        private void Client_ErrorCommunication(string ErrorMessage)
        {
            isConnected = client.Connected;
        }

        public void ReceiveDataFunc(byte[] Data)
        {
            foreach (byte dataByte in Data)
                parseByte(dataByte);
        }
        public enum ThermoLoggerStatusType
        {
            UNINIT = 0, GOT02_SYNC = 1, GOT_ID = 2, GOT03_DATA = 3, GOT0A_CHKSUM1, GOT0A_CHKSUM2
        };
        ThermoLoggerStatusType statusMsgRecv = ThermoLoggerStatusType.UNINIT;

        public class ThermoLogger
        {
            public string ID;
            public string Data;
            public string StrChkSum;
            public byte checkSum;

            public ThermoLogger()
            {
                string ID = "";
                string Data = "";
                string StrChkSum = "";
                byte checkSum = 0;
            }
        }
        ThermoLogger msgRecv = new ThermoLogger();
        public override void parseByte(byte data)
        {
            timeLastByteRecieved = DateTime.Now;

            //sw.Write(Convert.ToChar(data));

            if (statusMsgRecv < ThermoLoggerStatusType.GOT03_DATA)
                msgRecv.checkSum ^= data;

            switch (statusMsgRecv)
            {
                case ThermoLoggerStatusType.UNINIT:
                    if (data == 0x02)
                    {
                        statusMsgRecv++;
                        //messageReceivedAdvanceProtocol.checkSum = 0x02 ^ 0x01;
                        msgRecv.checkSum = 0x02 ^ 0x00;
                        msgRecv.StrChkSum = "";
                    }
                    break;
                case ThermoLoggerStatusType.GOT02_SYNC:
                    if (data == 32)
                        statusMsgRecv++;
                    else
                        msgRecv.ID += Convert.ToChar(data);

                    break;
                case ThermoLoggerStatusType.GOT_ID:
                    if (data == 0x03)
                        statusMsgRecv++;
                    else
                        msgRecv.Data += Convert.ToChar(data);
                    break;
                case ThermoLoggerStatusType.GOT03_DATA:
                    msgRecv.StrChkSum += Convert.ToChar(data);
                    ++statusMsgRecv;
                    break;
                case ThermoLoggerStatusType.GOT0A_CHKSUM1:
                    msgRecv.StrChkSum += Convert.ToChar(data);
                    if (Convert.ToByte(msgRecv.StrChkSum, 16) != msgRecv.checkSum)
                    {
                        goto error;
                    }
                    else
                    {
                        parseMessage();
                        //sw.WriteLine();
                        //sw.Flush();
                        goto restart;
                    }
                    break;
            }
            return;
        error:
        restart: statusMsgRecv = ThermoLoggerStatusType.UNINIT;
            return;
        }
        public override void parseMessage()
        {
            try
            {
                //msgRecv.Data = "025 +2238-02 00 00 003 000000 006 +6884-02 00 00 003 000000 002 +5426-01 00 0F 003 000000 003 +4636-02 00 1A 003 000000 004 +8359-02 00 1A 003 000000 017 -2498+00 00 0F 003 000000 016 -1221-03 00 0F 003 000000 015 -2502+00 00 0F 003 000000 012 +6002+01 00 00 003 000000 013 +0000+00 00 FF 003 000000";
                //001 +2520-02 00 00 2BD 000000 002 +2780+00 00 00 2BD 000000 003 +2410-02 00 00 001 000000 004 +4350-02 00 00 001 000000 005 +1940-02 00 00 001 000000 006 +9920-02 00 00 2BD 000000 015 +9503+00 00 00 2BD 000000 016 +5975+00 00 00 2BD 000000 017 +3528+00 00 00 2BD 000000 030 +0000+00 00 FF 000 000000 061 +2700+01 00 00 701 000000 
                //006 +1264-01 00 00 008 000000 003 +1360-02 00 00 008 000000 005 +2020-02 00 00 008 000000 004 +3380-02 00 00 008 000000 002 -1800-01 00 00 008 000000 012 +1969+01 00 00 008 000000 021 +5749-01 00 00 008 000000 022 +0000+00 00 FF 008 000000 023 +2472+01 00 00 008 000000 024 +1884+01 00 00 008 000000 025 +7369+02 00 00 008 000000 

                string[] tmpStrData = msgRecv.Data.Trim().Split(' ');
                int tmpAlarm = 0;


                for (int k = 0; k < polutions.Count; k++)
                {
                    polutions[k].concentration = getFloatNumThermoLogger(tmpStrData[6 * k + 1]);
                    polutions[k].alarmValue = (Convert.ToByte(tmpStrData[6 * k + 3], 16) << 8) & Convert.ToByte(tmpStrData[6 * k + 4], 16);
                }
               // alarmValue = polutions[0].alarmValue;

                if (writeMode)
                {
                    sw.WriteLine(msgRecv.Data);
                    sw.Flush();
                }
            }
            catch (Exception ex)
            {
                errorLog("[Error Parse Horiba 370] - " + ex.Message);
            }
            finally
            {
                msgRecv = new ThermoLogger();
            }

            timeLastMessageRecieved = DateTime.Now;
            waitForResponseList[1].Set();
            workingStatus = true;

        }

        private float getFloatNumThermoLogger(string numStr)
        {
            try
            {
                float num;
                float pow;

                if (!float.TryParse(numStr.Substring(0, 5), out num))
                    errorLog(string.Format("Device : {0} [parseMessage] can not parse concentration {1} ", deviceName, numStr));

                if (!float.TryParse(numStr.Substring(5, 3), out pow))
                    errorLog(string.Format("Device : {0} [parseMessage] can not parse concentration {1} ", deviceName, numStr));

                //float tmp = Convert.ToSingle(numStr.Substring(0, 5));
                //tmp *= (float)Math.Pow(10, Convert.ToSingle(numStr.Substring(5, 3)));

                return num * (float)Math.Pow(10, pow);
            }
            catch (Exception ex)
            {
                errorLog("[Problem Converting in Horiba 370] for " + numStr + " - " + ex.Message);
            }

            return -10000f;
        }

        internal override void Disconnect()
        {
            base.Disconnect();

            if (client!=null && client.Connected)
            {
                client.Disconnect();
                isConnected = client.Connected;
            }
        }
    }
}
