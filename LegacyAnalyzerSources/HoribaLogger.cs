using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SadraAQMS.Analyzers
{
    class HoribaLogger : deviceAnalyzer
    {
        //Dictionary<short, byte> HoribaID2PollutionID = new Dictionary<short, byte>();

        //StreamWriter sw = new StreamWriter("HoribaTest.txt", true);

        public HoribaLogger(DeviceType dvType, string devID, string devName, string ignorableErrors)
        {
            //this.company = DeviceCompany.Horiba;
            this.deviceType = DeviceType.Horiba_Logger;
            this.deviceName = DeviceType.Horiba_Logger.ToString();
            this.DeviceName = devName;
            int.TryParse(ignorableErrors, out IgnorableErrors);

            //_alarmValue = 0;
            //StreamReader sr = new StreamReader("Horiba.id");
            //string data = sr.ReadLine();
            //string[] tmpIDmatching = data.Split(';');
            //// enable / horiba id / pollution id / unit /id
            //foreach (string keyPair in tmpIDmatching)
            //{
            //    string[] tmpKeyPair = keyPair.Split(',');
            //    if (Convert.ToByte(tmpKeyPair[0]) == 1)
            //    {
            //        polutions.Add(new polutionType(Convert.ToByte(tmpKeyPair[2]), Convert.ToByte(tmpKeyPair[3])));
            //        HoribaID2PollutionID.Add(Convert.ToInt16(tmpKeyPair[1]), Convert.ToByte(tmpKeyPair[2]));
            //    }
            //}
            ////HoribaID2PollutionID.Add(1, 6);  // SO2
            ////HoribaID2PollutionID.Add(2, 2);  // CO
            ////HoribaID2PollutionID.Add(3, 3);  // NO
            ////HoribaID2PollutionID.Add(4, 5);  // NOx
            ////HoribaID2PollutionID.Add(6, 1);  // O3
            ////HoribaID2PollutionID.Add(12, 8);  //  DustC -> PM10    ????
            ////HoribaID2PollutionID.Add(13, 7);  //  PM2.5
            ////HoribaID2PollutionID.Add(15, 10);  //  THC -> Toluene  ????
            ////HoribaID2PollutionID.Add(16, 14);  //  CH4  -> 1-3 Butadiene  ????
            ////HoribaID2PollutionID.Add(17, 15);  //  NMHC -> m-p Xylene  ????
        }
        public override void sendReadCommand()
        {
            //  <stx>DA<ETX>04\n
            messageSendOnSerialBytes(new byte[] { 0x02, 0x44, 0x41, 0x03, 0x30, 0x34, 0x0A });
        }

        public enum HoribaLoggerStatusType
        {
            UNINIT = 0, GOT02_SYNC = 1, GOT_ID = 2, GOT03_DATA = 3, GOT0A_CHKSUM1, GOT0A_CHKSUM2
        };

        HoribaLoggerStatusType statusMsgRecv = HoribaLoggerStatusType.UNINIT;

        public class MessageLoggerHoriba
        {
            public string ID;
            public string Data;
            public string StrChkSum;
            public byte checkSum;

            public void Clear()
            {
                this.ID = string.Empty;
                this.Data = string.Empty;
                this.StrChkSum = string.Empty;
            }

            public MessageLoggerHoriba()
            {
                string ID = "";
                string Data = "";
                string StrChkSum = "";
                byte checkSum = 0;
            }
        }

        MessageLoggerHoriba msgRecv = new MessageLoggerHoriba();

        public override void parseByte(byte data)
        {
            try
            {
                timeLastByteRecieved = DateTime.Now;

                //sw.Write(Convert.ToChar(data));

                if (statusMsgRecv < HoribaLoggerStatusType.GOT03_DATA)
                    msgRecv.checkSum ^= data;

                switch (statusMsgRecv)
                {
                    case HoribaLoggerStatusType.UNINIT:
                        if (data == 0x02)
                        {
                            statusMsgRecv++;
                            //messageReceivedAdvanceProtocol.checkSum = 0x02 ^ 0x01;
                            msgRecv.checkSum = 0x02 ^ 0x00;
                            msgRecv.StrChkSum = "";
                        }
                        break;
                    case HoribaLoggerStatusType.GOT02_SYNC:
                        if (data == 32)
                            statusMsgRecv++;
                        else
                        {
                            try
                            {
                                msgRecv.ID += Convert.ToChar(data);
                            }
                            catch (Exception ex)
                            {
                                errorLog("[Error Convert  ID Byte  Horiba Logger] - " + ex.Message);
                                goto restart;
                            }
                            
                        }
                        break;
                    case HoribaLoggerStatusType.GOT_ID:
                        if (data == 0x03)
                            statusMsgRecv++;
                        else
                        {
                            try
                            {
                                msgRecv.Data += Convert.ToChar(data);
                            }
                            catch (Exception ex)
                            {
                                errorLog("[Error Convert Data Byte  Horiba Logger] - " + ex.Message);
                                goto restart;
                            }
                        }
                            
                        break;
                    case HoribaLoggerStatusType.GOT03_DATA:
                        try
                        {
                            msgRecv.StrChkSum += Convert.ToChar(data);
                        }
                        catch (Exception ex)
                        {
                            errorLog("[Error Convert StrChkSum Byte  Horiba Logger] - " + ex.Message);
                            goto restart;
                        }
                        
                        ++statusMsgRecv;
                        break;
                    case HoribaLoggerStatusType.GOT0A_CHKSUM1:
                        try
                        {
                            msgRecv.StrChkSum += Convert.ToChar(data);
                        }
                        catch (Exception ex)
                        {
                            errorLog("[Error Convert StrChkSum Byte  Horiba Logger] - " + ex.Message);
                            goto restart;
                        }
                        try
                        {
                            if (Convert.ToByte(msgRecv.StrChkSum, 16) != msgRecv.checkSum)
                            {
                                goto error;
                            }
                            else
                            {
                                parseMessage();
                                msgRecv.Clear();
                                //sw.WriteLine();
                                //sw.Flush();
                                goto restart;
                            }

                        }
                        catch (Exception  ex)
                        {
                            errorLog("[Error call parsMessage  Horiba Logger] - " + ex.Message);
                            goto restart;
                        }  
                }
                return;
                error: msgRecv.Clear();
                restart: statusMsgRecv = HoribaLoggerStatusType.UNINIT;
                return;

            }
            catch (Exception ex)
            {
                errorLog("[Error Parse Byte Horiba Logger] - " + ex.Message);
                statusMsgRecv = HoribaLoggerStatusType.UNINIT;
            }
        }

        public override void parseMessage()
        {
            //            MD10 001 + 2238 - 02 00 00 003 000000 006 + 6884 - 02 00 00 003 000000 002 + 5426 - 01 0
            //0 0F 003 000000 003 + 4636 - 02 00 1A 003 000000 004 + 8359 - 02 00 1A 003 000000 017
            //      - 2498 + 00 00 0F 003 000000 016 - 1221 - 03 00 0F 003 000000 015 - 2502 + 00 00 0F 003 0
            //00000 012 + 6002 + 01 00 00 003 000000 013 + 0000 + 00 00 FF 003 000000 20

            try
            {
                //msgRecv.Data = "025 +2238-02 00 00 003 000000 006 +6884-02 00 00 003 000000 002 +5426-01 00 0F 003 000000 003 +4636-02 00 1A 003 000000 004 +8359-02 00 1A 003 000000 017 -2498+00 00 0F 003 000000 016 -1221-03 00 0F 003 000000 015 -2502+00 00 0F 003 000000 012 +6002+01 00 00 003 000000 013 +0000+00 00 FF 003 000000";
                //001 +2520-02 00 00 2BD 000000 002 +2780+00 00 00 2BD 000000 003 +2410-02 00 00 001 000000 004 +4350-02 00 00 001 000000 005 +1940-02 00 00 001 000000 006 +9920-02 00 00 2BD 000000 015 +9503+00 00 00 2BD 000000 016 +5975+00 00 00 2BD 000000 017 +3528+00 00 00 2BD 000000 030 +0000+00 00 FF 000 000000 061 +2700+01 00 00 701 000000 
                //006 +1264-01 00 00 008 000000 003 +1360-02 00 00 008 000000 005 +2020-02 00 00 008 000000 004 +3380-02 00 00 008 000000 002 -1800-01 00 00 008 000000 012 +1969+01 00 00 008 000000 021 +5749-01 00 00 008 000000 022 +0000+00 00 FF 008 000000 023 +2472+01 00 00 008 000000 024 +1884+01 00 00 008 000000 025 +7369+02 00 00 008 000000 

                string[] tmpStrData = msgRecv.Data.Trim().Split(' ');
                int tmpAlarm = 0;


                for (int k = 0; k < tmpStrData.Length / 6; k++)
                {
                    //if (HoribaID2PollutionID.ContainsKey(Convert.ToInt16(tmpStrData[6 * k])))
                    //{
                    //    byte tmpPolID = HoribaID2PollutionID[Convert.ToInt16(tmpStrData[6 * k])];
                    //    polutionType polution = polutions.Find(x => x.ID == tmpPolID);
                    //    polution.concentration = getFloatNumHoribaLogger(tmpStrData[6 * k + 1]);
                    //    polution.alarmValue = Convert.ToByte(tmpStrData[6 * k + 3], 16);
                    //}

                    var horibaPolId = Convert.ToInt16(tmpStrData[6 * k]);


                    if (polutions.Exists(m => m.mapperID == horibaPolId))
                    {
                        polutionType polution = polutions.Find(x => x.mapperID == horibaPolId);
                        polution.concentration = getFloatNumHoribaLogger(tmpStrData[6 * k + 1]);
                        polution.alarmValue = tmpStrData.Length > 6 * k + 3 ? (Convert.ToByte(tmpStrData[6 * k + 2], 16) << 8) & Convert.ToByte(tmpStrData[6 * k + 3], 16) : 0;

                        // tmpAlarm |= (polution.alarmValue > 0 ? 1 << k : 0);
                    }
                }
                //alarmValue = tmpAlarm;

                if (writeMode)
                {
                   
                    sw.WriteLine(DateTime.Now.ToString("MM/dd/yyyy h:mm tt") + ' '+ msgRecv.Data);
                    sw.Flush();
                }
            }
            catch (Exception ex)
            {
                errorLog("[Error Parse Horiba Logger] - " + ex.Message);
            }
            finally
            {
                msgRecv = new MessageLoggerHoriba();
            }

            timeLastMessageRecieved = DateTime.Now;
            waitForResponseList[1].Set();
            workingStatus = true;

        }

        private float getFloatNumHoribaLogger(string numStr)
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
                //tmp *= (float)Math.Pow(10,Convert.ToSingle(numStr.Substring(5, 3)));


                return num * (float)Math.Pow(10, pow);
            }
            catch (Exception ex)
            {
                errorLog("[Problem Converting in Horiba Logger] for " + numStr + " - " + ex.Message);
            }

            return -10000f;
        }

        //protected override void Dispose(bool disposing)
        //{
        //    //sw.Close();
        //    base.Dispose(disposing);
        //}
        internal override void Disconnect()
        {
            if (conSerial.IsOpen)
                conSerial.Close();

            base.Disconnect();

        }
    }
}
