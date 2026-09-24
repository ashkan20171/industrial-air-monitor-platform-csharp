using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
namespace SadraAQMS.Analyzers
{
    internal class WeatherHD52 : deviceAnalyzer
    {
        public WeatherHD52(DeviceType dvType, string devID)
        {
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();

            //_alarmValue = 0;
        }

        public WeatherHD52(DeviceType dvType, string devID, string devName, string ignorableErrors)
        {
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            this.DeviceName = devName;
            int.TryParse(ignorableErrors, out IgnorableErrors);
            //_alarmValue = 0;
        }


        enum MessageHDWeatherProtocolStatusType
        {
            UNINIT = 0, GOT_DeviceID, GOT_MessageType, GOT_LEN, GOT_PAYLOAD, GOT_CHECKSUM
        };
        MessageHDWeatherProtocolStatusType statusMsgRecv;

        int swicthProtocol = 1;
        byte messageIndexRecieve;

        public class MessageHDProtocol
        {
            public byte deviceID;
            public byte messageType;
            public byte length;
            public List<byte> data { get; set; }
            public List<byte> checkSum { get; set; }
            public byte chks;
            public List<byte> StrChkSum { get; set; }

            public MessageHDProtocol()
            {
                // deviceID = dvcID;  
                deviceID = 01;
                messageType = 04;
                //messageType = msgID;
                data = new List<byte>();
                checkSum = new List<byte>();
                // data = msgData.ToList<byte>();
                length = 0;
                StrChkSum = new List<byte>();

            }


            static public byte[] computeHDChecksum(List<byte> msg)
            {
                byte chksum = 0;
                for (int k = 1; k < msg.Count; k++)
                    chksum ^= msg[k];
                return Encoding.UTF8.GetBytes(chksum.ToString("X2"));
            }

            public string GetBytesString(bool isResponse)
            {
                List<byte> tmpListByts = new List<byte>();
                if (isResponse)
                    tmpListByts.Add(6);
                else
                    tmpListByts.Add(2);

                tmpListByts.Add(deviceID);
                tmpListByts.Add(messageType);
                tmpListByts.AddRange(data);
                return String.Join(" ", tmpListByts.Select(b => b.ToString()));
            }
        }
        MessageHDProtocol messageReceivedHD = new MessageHDProtocol();

        public override void sendReadCommand()
        {
            //<stx>#01<ETX>04\n
            messageSendOnSerialBytes(new byte[] { 0x01, 0x04, 0x00, 0x00, 0x00, 0x15, 0x31, 0xc5 });
        }
        /* 01 04 2a 00 8e 02 7d 00 8b 00 8a 00 8a 00 8f 01 48 21 03 0d b8 16 f4 00 8e 03 7c 01 93 ff ee 02 7d 00 7f 00 3e 00 20 00 00 00 00 00 00 25 22
           01: ID Device, 04: Read, 2a:Length, two byte and 16 bit is one parameter 
            * -> 1 register number is wind speed (WS)
            * -> 2 register number is wind direction (WD)
            * -> 6 register number is temprature (TC)
            * -> 7 register number is relative humidity (RH)
            * -> 8 register number is pressure (PA)
        */
        int errorCount = 0;

        public override void parseByte(byte data)
        {
            try
            {
                timeLastByteRecieved = DateTime.Now;

                if (statusMsgRecv > MessageHDWeatherProtocolStatusType.GOT_CHECKSUM || messageReceivedHD.data.Count > 1000)
                    statusMsgRecv = MessageHDWeatherProtocolStatusType.UNINIT;
                if (statusMsgRecv < MessageHDWeatherProtocolStatusType.GOT_PAYLOAD)
                    messageReceivedHD.StrChkSum.Add(data);
                //messageReceivedHD.chks ^= data;
                switch (statusMsgRecv)
                {
                    case MessageHDWeatherProtocolStatusType.UNINIT:
                        if (data == 0x01)
                        {
                            statusMsgRecv++;
                            messageReceivedHD.chks = 0x00;
                            messageReceivedHD.data.Clear();
                            messageReceivedHD.checkSum.Clear();
                            messageReceivedHD.deviceID = data;
                            messageIndexRecieve = 0;
                        }
                        break;
                    case MessageHDWeatherProtocolStatusType.GOT_DeviceID:
                        messageReceivedHD.messageType = data;
                        if (data == 0x04)
                        {
                            statusMsgRecv++;
                            messageIndexRecieve = 0;
                        }
                        break;
                    case MessageHDWeatherProtocolStatusType.GOT_MessageType:
                        if (data == 0x2a)
                        {
                            messageReceivedHD.length = data;
                            statusMsgRecv++;

                           // Device.LogHandler.errorLog(String.Format("length {0}", messageReceivedHD.length));
                        }
                        break;

                    case MessageHDWeatherProtocolStatusType.GOT_LEN:
                        
                        messageReceivedHD.data.Add(Convert.ToByte(data.ToString().ToUpper()));
                        if (++messageIndexRecieve == messageReceivedHD.length)
                        {
                            statusMsgRecv++;
                            messageIndexRecieve = 0;
                           // Device.LogHandler.errorLog(String.Format("data "));
                        }
                        break;
                    case MessageHDWeatherProtocolStatusType.GOT_PAYLOAD:

                        messageReceivedHD.checkSum.Add(data);
                       // Device.LogHandler.errorLog(String.Format("data checksum {0} ", messageReceivedHD.checkSum[0]));

                        if (++messageIndexRecieve == 2)
                        {
                           // Device.LogHandler.errorLog(String.Format("data checksum  {0}", messageReceivedHD.checkSum[1]));

                            List<byte> checksumCalculate = new List<byte>();
                           // Device.LogHandler.errorLog(String.Format("checksum  1 "));
                            string CalculateChecksum = Crc16.ComputeChecksum(messageReceivedHD.StrChkSum.ToArray()).ToString("x4").ToUpper();
                            //Device.LogHandler.errorLog(String.Format("checksum  2 "));
                            if (CalculateChecksum.Length==4)
                            {
                            checksumCalculate.Add(Convert.ToByte(CalculateChecksum.Substring(2, 2), 16));
                            checksumCalculate.Add(Convert.ToByte(CalculateChecksum.Substring(0, 2), 16));
                           // Device.LogHandler.errorLog(String.Format("checksum 3"));
                            }
                            else
                            {
                                Device.LogHandler.errorLog(String.Format("checksum  error", CalculateChecksum));
                                goto restart;
                            }


                            if (checksumCalculate[0] == messageReceivedHD.checkSum[0] && checksumCalculate[1] == messageReceivedHD.checkSum[1])
                            {
                                //Device.LogHandler.errorLog(String.Format("checksum {0}  ,  {1}", (Convert.ToByte(CalculateChecksum.Substring(2, 2), 16)).ToString(), (Convert.ToByte(CalculateChecksum.Substring(0, 2), 16)).ToString()));
                                parseMessage();
                               // Device.LogHandler.errorLog(String.Format("parseMessage "));

                                messageReceivedHD.StrChkSum.Clear();
                                //sw.WriteLine();
                                //sw.Flush();
                                goto restart;
                            }

                            statusMsgRecv++;
                        }

                        break;
  
                    case MessageHDWeatherProtocolStatusType.GOT_CHECKSUM:
                        if (data == 0x03)
                        {
                            // statusLog("go to restart");
                            goto restart;
                        }
                        else if (data != 0x03)
                        {
                            messageIndexRecieve = 0;
                            goto restart;
                        }
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
            restart: statusMsgRecv = MessageHDWeatherProtocolStatusType.UNINIT;
                return;
            }
            catch (Exception ex)
            {
                errorLog("[HD52 parseByte] device: " + deviceName + " - " + ex.Message + "- proto:" + swicthProtocol.ToString() + "- statusMsg:" + statusMsgRecv.ToString());
                statusMsgRecv = MessageHDWeatherProtocolStatusType.UNINIT;
            }
        }
        public override void parseMessage()
        {
            try
            {
                float[] Data = new float[messageReceivedHD.data.Count() / 2];
                for (int j = 0; j < messageReceivedHD.data.Count() / 2; j++)
                {
                    Data[j] = BitConverter.ToInt16(new byte[] { messageReceivedHD.data[2 * j + 1], messageReceivedHD.data[2 * j] }, 0);
                    Array.Reverse(messageReceivedHD.data.ToArray()); //need the bytes in the reverse order
                    int value = BitConverter.ToInt32(messageReceivedHD.data.ToArray(), 0);
                }
                polutions[0].concentration = Data[0] / 100; //WS
                polutions[1].concentration = Data[1] / 10;  //WD
                polutions[2].concentration = Data[2] / 10;  //T
                polutions[3].concentration = Data[6] / 10;  //RH
                polutions[4].concentration = Data[7] / 10;  //P
                timeLastMessageRecieved = DateTime.Now;
                waitForResponseList[1].Set();
                workingStatus = true;
                messageReceivedHD.data.Clear();
            }
            catch (Exception ex)
            {
                errorLog("[Error Parse Delta OHM] - " + ex.Message);
            }
        }

        public static class Crc16
        {
            const ushort polynomial = 0xA001;
            static readonly ushort[] table = new ushort[256];

            public static ushort ComputeChecksum(byte[] bytes)
            {
                ushort crc = 0xFFFF;
                for (int i = 0; i < bytes.Length; ++i)
                {
                    byte index = (byte)(crc ^ bytes[i]);
                    crc = (ushort)((crc >> 8) ^ table[index]);
                }
                return crc;
            }

            static Crc16()
            {
                ushort value;
                ushort temp;
                for (ushort i = 0; i < table.Length; ++i)
                {
                    value = 0;
                    temp = i;
                    for (byte j = 0; j < 8; ++j)
                    {
                        if (((value ^ temp) & 0x0001) != 0)
                        {
                            value = (ushort)((value >> 1) ^ polynomial);
                        }
                        else
                        {
                            value >>= 1;
                        }
                        temp >>= 1;
                    }
                    table[i] = value;
                }
            }
        }




    }
}