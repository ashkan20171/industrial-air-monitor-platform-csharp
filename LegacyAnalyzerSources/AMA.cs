using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SadraAQMS.Analyzers
{
    class AMA : deviceAnalyzer
    {
        public AMA(DeviceType dvType, string devID)
        {
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
        }
        public AMA(DeviceType dvType, string devID, string devName, string ignorableErrors)
        {
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            this.DeviceName = devName;
            int.TryParse(ignorableErrors, out IgnorableErrors);
            //_alarmValue = 0;
        }

        public override void sendReadCommand()
        {
            // DATA Request is in page of 146
            // <stx><8>DA<ETX>12\n
            messageSendOnSerialBytes(new byte[] { 0x02, 0x02, 0x44, 0x41, 0x03, 0x30, 0x36 });
        }

        public enum Gesytec2StatusType
        {
            UNINIT = 0, GOTSTX_SYNC = 1, GOT_LEN = 2, GOT_MSG = 3, GOT_ETX, GOT0A_CHKSUM1, GOT0A_CHKSUM2
        };

        int countLen = 0;
        Gesytec2StatusType statusMsgRecv = Gesytec2StatusType.UNINIT;

        public class MessageGesytec2
        {
            public byte len;
            public string ID;
            public List<byte> Data;
            public string StrChkSum;
            public byte checkSum;

            public MessageGesytec2()
            {
                len = 0;
                ID = "";
                Data = new List<byte>();
                StrChkSum = "";
                checkSum = 0;
            }
        }

        MessageGesytec2 msgRecv = new MessageGesytec2();

        public override void parseByte(byte data)
        {
            try
            {
                timeLastByteRecieved = DateTime.Now;

                if (statusMsgRecv < Gesytec2StatusType.GOT_ETX)
                    msgRecv.checkSum ^= data;

                switch (statusMsgRecv)
                {
                    case Gesytec2StatusType.UNINIT:
                        if (data == 0x02)
                        {
                            msgRecv.checkSum = 0x02 ^ 0x00;
                            msgRecv.StrChkSum = "";
                            countLen = 0;
                            statusMsgRecv++;
                        }
                        break;
                    case Gesytec2StatusType.GOTSTX_SYNC:
                        msgRecv.len = data;
                        statusMsgRecv++;

                        break;
                    case Gesytec2StatusType.GOT_LEN:
                        //try
                        //{
                        //    msgRecv.ID += Convert.ToChar(data);
                        //}
                        //catch (Exception ex)
                        //{
                        //    errorLog("[Error Convert MSG ID AMA] - " + ex.Message);
                        //}

                        msgRecv.Data.Add(data);

                        if (countLen++ >= msgRecv.len-1)
                        {
                            statusMsgRecv++;
                            countLen = 0;
                        }

                        break;
                    case Gesytec2StatusType.GOT_MSG: // we are getting data end with 0x03 [ETX]

                        if (data == 0x03 ) // End of Message
                        {
                             statusMsgRecv++;
                        }
                        else
                        {
                            goto error;
                        }
                        break;
                    case Gesytec2StatusType.GOT_ETX:
                        try
                        {
                            msgRecv.StrChkSum += Convert.ToChar(data);
                        }
                        catch (Exception ex)
                        {
                            errorLog("[Error Convert StrChkSum Byte  AMA] - " + ex.Message);
                        }

                        statusMsgRecv++;
                        break;
                    case Gesytec2StatusType.GOT0A_CHKSUM1:
                        try
                        {
                            msgRecv.StrChkSum += Convert.ToChar(data);
                        }
                        catch (Exception ex)
                        {
                            errorLog("[Error Convert StrChkSum 2 Byte  AMA] - " + ex.Message);
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
                                goto restart;
                            }

                        }
                        catch (Exception ex)
                        {
                            errorLog("[Error call parsMessage  AMA] - " + ex.Message);
                        }

                        break;
                }
                return;
                error:
                restart: statusMsgRecv = Gesytec2StatusType.UNINIT;
                return;

            }
            catch (Exception ex)
            {
                errorLog("[Error Parse Byte AMA GC5000] - " + ex.Message);
                statusMsgRecv = Gesytec2StatusType.UNINIT;
            }
        }

        public override void parseMessage()
        {
            //deviceRes <STX><Length in byte>ADbb+- nnnn+-eebbbb+- nnnn+-eebbbb+- nnnn+-eebbbb+- nnnn+-eebbbb+- nnnn+-eebbbb+- nnnn+-eebbbb+- nnnn+-eebbbb+- nnnn+-eebbb<ETX><BCC1><BCC2>
            try
            {
                int tmpAlarm = 0;
                for (int idx = 0; idx < polutions.Count && idx < (msgRecv.len - 3) / 12; idx++) // msgRecv.Data[0] is length of data    // 
                {
                    var devPolId = msgRecv.Data[12 * idx + 3];
                    polutions[idx].concentration= getFloatNumGesytec(Encoding.UTF8.GetString(msgRecv.Data.GetRange(12 * idx + 4, 8).ToArray()));
                    //statusLog("concentration is:" + polutions[idx].concentration);
                    polutions[idx].alarmValue = msgRecv.Data[12 * idx + 13];
                    //if (polutions.Exists(m => m.mapperID == devPolIddevPolId))
                    //{
                    //    polutionType polution = polutions.Find(x => x.mapperID == devPolId);
                    //    polution.concentration = getFloatNumGesytec(Encoding.UTF8.GetString(msgRecv.Data.GetRange(12 * idx + 2, 8).ToArray()));
                    //    errorLog("concentration is:" + polution.concentration);
                    //    polution.alarmValue = msgRecv.Data[12 * idx + 11];

                    //    tmpAlarm |= (polution.alarmValue > 0 ? 1 << idx : 0);
                    //}
                }

                // alarmValue = tmpAlarm;

                if (writeMode)
                {

                    sw.WriteLine(DateTime.Now.ToString("MM/dd/yyyy h:mm tt") + ' ' + BitConverter.ToString(msgRecv.Data.ToArray()));
                    sw.Flush();
                }

            }
            catch (Exception ex)
            {
                errorLog("[Error Parse AMA GC ] - " + ex.Message);
            }
            finally
            {
                msgRecv = new MessageGesytec2();
            }

            timeLastMessageRecieved = DateTime.Now;
            waitForResponseList[1].Set();
            workingStatus = true;

        }
        private float getFloatNumGesytec(string numStr)
        {
            try
            {
                float num;
                float pow;

                if (!float.TryParse(numStr.Substring(0, 5), out num))
                    errorLog(string.Format("Device : {0} [parseMessage] can not parse concentration {1} ", deviceName, numStr));

                if (!float.TryParse(numStr.Substring(5, 3), out pow))
                    errorLog(string.Format("Device : {0} [parseMessage] can not parse concentration {1} ", deviceName, numStr));

                return num * (float)Math.Pow(10, pow);
            }
            catch (Exception ex)
            {
                errorLog("[Problem Converting to float Gystec AMA GC] for " + numStr + " - " + ex.Message);
            }

            return -10000f;
        }

        internal override void Disconnect()
        {
            base.Disconnect();

            if (conSerial.IsOpen)
                conSerial.Close();
        }
    }
}
