using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace SadraAQMS.Analyzers
{
    public class UnitecAnalyzer : deviceAnalyzer
    {
        // Dictionary<short, byte> UnitecID2PollutionID = new Dictionary<short, byte>();
        Client client;

        public UnitecAnalyzer(DeviceType dvType)
        {
            //this.company = DeviceCompany.Unitec;
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();

            //_alarmValue = 0;

            //initialize_Connect();

            //StreamReader sr = new StreamReader("Unitec.id");

            //string data = sr.ReadToEnd();
            //string[] tmpIDmatching = data.Split(';');

            //// enable / horiba id / pollution id / unit /id
            //foreach (string keyPair in tmpIDmatching)
            //{
            //    string[] tmpKeyPair = keyPair.Split(',');
            //    if (Convert.ToByte(tmpKeyPair[0]) == 1)
            //    {
            //        polutions.Add(new polutionType(Convert.ToByte(tmpKeyPair[2]), Convert.ToByte(tmpKeyPair[3])));
            //        UnitecID2PollutionID.Add(Convert.ToInt16(tmpKeyPair[1]), Convert.ToByte(tmpKeyPair[2]));
            //    }
            //}
        }

        public UnitecAnalyzer(DeviceType dvType, string devID, string devName, string ignorableErrors)
        {
            // this.company = DeviceCompany.Unitec;
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            this.DeviceName = devName;
            int.TryParse(ignorableErrors, out IgnorableErrors);

            //_alarmValue = 0;

            //initialize_Connect();

            //StreamReader sr = new StreamReader("Unitec.id");

            //string data = sr.ReadToEnd();
            //string[] tmpIDmatching = data.Split(';');

            //// enable / horiba id / pollution id / unit /id
            //foreach (string keyPair in tmpIDmatching)
            //{
            //    string[] tmpKeyPair = keyPair.Split(',');
            //    if (Convert.ToByte(tmpKeyPair[0]) == 1)
            //    {
            //        polutions.Add(new polutionType(Convert.ToByte(tmpKeyPair[2]), Convert.ToByte(tmpKeyPair[3])));
            //        UnitecID2PollutionID.Add(Convert.ToInt16(tmpKeyPair[1]), Convert.ToByte(tmpKeyPair[2]));
            //    }
            //}
        }

        //public bool IsAlive = false;

        public override void sendReadCommand()
        {
            //LastMessage = "18-03-17;09:41:42;#01;11.8;687.3;1;#02;0.6;12.8;1;#03;202.8;3734.7;1;#04;0.0;147.1;1;#05;29.8;4837.3;1;#06;115.1;192.9;1;#07;45.0;2.4;1;#08;34.6;18.3;1;#09;3333.0;0;#10;3333.0;0;#11;3333.0;0;***";
            //parseMessage();

            if (!workingStatus && isConnected)
            {
                try
                {
                    string tmp = "@#iny\r\n";

                    List<byte> tcpSendBytes = new List<byte>();
                    tcpSendBytes.AddRange(UTF8Encoding.ASCII.GetBytes(tmp));

                    client.SendData(tcpSendBytes.ToArray());
                }
                catch
                {
                    workingStatus = false;
                    isConnected = client.Connected;
                }
            }
        }



        internal override void initialize_Connect()
        {
            //string[] IPdata = portCOM.Split(':');
            //string IP = IPdata[0];
            //ushort Port = Convert.ToUInt16(IPdata[1]);

            try
            {
                statusLog("start client for unitec");

                client = new Client();
                client.AddIPs(portCOM);

                client.ReceiveData += new Client.DataReceived(ReceiveDataFunc);
                client.ErrorCommunication += Client_ErrorCommunication;
                client.SuccessCommunication += Client_SuccessCommunication;
                client.Connect();

                isConnected = true; // client.Connected;

                base.initialize_Connect();

                //if (writeMode)
                //    sw = new StreamWriter(string.Format(writePath, "UnitecAnalyzer"));
            }
            catch (Exception ex)
            {
                errorLog("TCP conncetion problem for device analyzer Unitec: " + ex.Message);
            }
        }

        private void Client_ErrorCommunication(string ErrorMessage)
        {
            isConnected = client.Connected;
        }

        private byte Client_SuccessCommunication(object sender, EventArgs e)
        {
            byte status = 0;
            try
            {
                string tmp = "@#iny\r\n";

                List<byte> tcpSendBytes = new List<byte>();
                tcpSendBytes.AddRange(UTF8Encoding.ASCII.GetBytes(tmp));

                client.SendData(tcpSendBytes.ToArray());
                status = 1;
            }
            catch
            {
                workingStatus = false;
                isConnected = client.Connected;
            }
            return status;
        }

        string LastMessage = "";
        public void ReceiveDataFunc(byte[] Data)
        {
            foreach (byte dataByte in Data)
                parseByte(dataByte);
        }
        public override void parseByte(byte data)
        {
            timeLastByteRecieved = DateTime.Now;

            LastMessage += ((char)data);
            if (LastMessage.Length > 6)
            {
                if (data == '\r' && LastMessage[LastMessage.Length - 2] == '\r' && LastMessage[LastMessage.Length - 3] == '*' && LastMessage[LastMessage.Length - 4] == '*'
                    || data == '\r' && LastMessage[LastMessage.Length - 2] == '*' && LastMessage[LastMessage.Length - 3] == '*')
                {
                    parseMessage();
                }
            }
            if (LastMessage.Length > 500)  // WE have no Message this Long
            {
                errorLog("[Unitec] [parseByte] - Message is too long : " + LastMessage);

                LastMessage = "";
            }
        }
        public override void parseMessage()
        {
            //for test 
            //LastMessage = "14-08-17;11:19:32;#01;0.1;296.0;1;#02;500.0;47.2;1;#03;500.0;235.1;1;#04;-14.3;0.0;1;#05;0.0;333.0;1;#06;-3.5;0.0;1;#07;45.8;69.6;1;#08;0.0;0.0;1;#09;140.7;140.7;1;#10;1.5;1014.7;1;***";

            try
            {
                List<string> strParts = new List<string>(LastMessage.Split(';'));

                if (strParts.Count > 5)
                {
                    ////dataPol.time = Convert.ToDateTime(strParts[0]); // + " " + strParts[1]);
                    //DataPol.time = DateTime.Now;    //Convert.ToDateTime(strParts[0]); // + " " + strParts[1]);
                    //DataPol.strtime = strParts[0] + " " + strParts[1];

                    int idxStartData = strParts.FindIndex(s => s.Contains("#"));

                    while (idxStartData > 0)
                    {
                        byte idx = Convert.ToByte(strParts[idxStartData].Substring(1, strParts[idxStartData].Length - 1));

                        //if (UnitecID2PollutionID.ContainsKey(idx))
                        if (polutions.Exists(m => m.mapperID == idx))
                        {
                            //byte tmpPolID = UnitecID2PollutionID[idx];

                            //polutionType polution = polutions.Find(x => x.ID == tmpPolID);

                            polutionType polution = polutions.Find(x => x.mapperID == idx);

                            if (!float.TryParse(strParts[idxStartData + 1], out polution.concentration))
                                errorLog(string.Format("Device: {0} ParseMessage: Could not Convert concentration data : {1} to float for polutionId {2}", deviceName, strParts[idxStartData + 1], polution.ID));

                            //polution.concentration = Convert.ToSingle(strParts[idxStartData + 1]);
                            polution.alarmValue = 0;
                        }
                        idxStartData = strParts.FindIndex(idxStartData + 1, s => s.Contains("#"));
                    }


                    if (writeMode)
                    {
                        sw.WriteLine(LastMessage);
                        sw.Flush();
                    }
                }

                timeLastMessageRecieved = DateTime.Now;
                waitForResponseList[1].Set();
                workingStatus = true;
            }
            catch (Exception ex)
            {
                errorLog("[Error Parse Unitec Logger] - " + ex.Message);
            }
            finally
            {
                LastMessage = "";
            }
        }
        internal override void Disconnect()
        {
            base.Disconnect();

            if (client.Connected)
            {
                client.Disconnect();
                isConnected = client.Connected;
            }
        }
    }
}
