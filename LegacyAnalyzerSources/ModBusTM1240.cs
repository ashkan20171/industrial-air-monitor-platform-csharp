using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SadraAQMS.Analyzers
{
    class ModBusTM1240 : deviceAnalyzer
    {
        public ModBusTM1240(DeviceType dvType, string devID)
        {
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
        }
        public ModBusTM1240(DeviceType dvType, string devID, string devName, string ignorableErrors)
        {
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            this.DeviceName = devName;
            int.TryParse(ignorableErrors, out IgnorableErrors);
        }
        byte devID = 250;
        public override void sendReadCommand()
        {
            if (byte.TryParse(DeviceID, out devID))
            {
                byte[] sendCommand = new byte[] { devID, 3, 0, 46, 0, 14 };
                //string SendreadChecksum = (Crc16.ComputeChecksum(sendCommand).ToString("x2"));
                List<byte> send = new List<byte> { devID, 0x03, 0x00, 0x2e, 0x00, 0x0e };

                send.AddRange(BitConverter.GetBytes(Crc16.ComputeChecksum(send.ToArray())));
                //string SendreadChecksum = (Crc16.ComputeChecksum(send.ToArray()).ToString("x2"));

                //send.Add(Convert.ToByte(SendreadChecksum.Substring(2, 2), 16));
                //send.Add(Convert.ToByte(SendreadChecksum.Substring(0, 2), 16));
                messageSendOnSerialBytesAwaitingResponse(send.ToArray(),1);
            }
            else
                errorLog("[sendReadCommand] DeviceName : " + deviceName + " Bad device  ID : " + DeviceID);
            //messageSendOnSerialBytes(new byte[] { 0, 03, 0, 50, 00, 10 });
            //messageSendOnSerialBytes(new byte[] { 250, 0x03, 0x9c, 0x73, 0x00, 0x0A, 0x0F, 0xCD });

        }

        byte messageIndexRecieve;
        public enum ModBusStatusType
        {
            UNINIT = 0, GOT_DeviceID, GOT_Func, GOT_LEN, GOT_PAYLOAD, GOT_CHECKSUM
        };

        ModBusStatusType statusMsgRecv = ModBusStatusType.UNINIT;
        public class MessagModBusTM
        {
            public byte ID;
            // public byte Data;
            //public string StrChkSum;
            public List<byte> StrChkSum { get; set; }
            //public string checkSum;
            public List<byte> checkSum { get; set; }
            public List<byte> data { get; set; }
            public byte Length;
            public MessagModBusTM()
            {
                ID = 0;
                data = new List<byte>();
                //data = "";
                StrChkSum = new List<byte>();
                checkSum = new List<byte>();
                Length = 0;
            }

        }
        MessagModBusTM msgRecv = new MessagModBusTM();


        public override void parseByte(byte data)
        {
            timeLastByteRecieved = DateTime.Now;
            if (statusMsgRecv < ModBusStatusType.GOT_PAYLOAD)

                msgRecv.StrChkSum.Add(data);
            switch (statusMsgRecv)
            {
                case ModBusStatusType.UNINIT:
                    if (data == devID)
                    {
                        msgRecv.ID = data;
                        msgRecv.data.Clear();
                       statusMsgRecv++;
                    }
                    break;

                case ModBusStatusType.GOT_DeviceID:
                    if (data == 0x03)
                        statusMsgRecv++;
                    break;

                case ModBusStatusType.GOT_Func:

                    if (data == 0x1C) //Length Byte For Data Float is 20 Byte =>5 Float
                        statusMsgRecv++;
                    msgRecv.Length = data;
                    messageIndexRecieve = 0;

                    break;

                case ModBusStatusType.GOT_LEN:

                    msgRecv.data.Add(data);
                    //msgRecv.data += data+" " ;
                    if (++messageIndexRecieve >= 28)
                    {
                        statusMsgRecv++;
                        messageIndexRecieve = 0;
                    }

                    break;
                case ModBusStatusType.GOT_PAYLOAD:
                    msgRecv.checkSum.Add(data);

                    //string[] ss = msgRecv.checkSum.Split(' ');
                    //foreach (byte c in r)
                    //{

                    //    r.Add(Convert.ToByte(c, 16));
                    //}
                    if (++messageIndexRecieve >= 2)
                    {
                        //var bytes = HexToBytes(msgRecv.StrChkSum[0]);
                        List<byte> checksumCalculate = new List<byte>();
                        string CalculateChecksum = Crc16.ComputeChecksum(msgRecv.StrChkSum.ToArray()).ToString("x2");
                        checksumCalculate.Add(Convert.ToByte(CalculateChecksum.Substring(2, 2), 16));
                        checksumCalculate.Add(Convert.ToByte(CalculateChecksum.Substring(0, 2), 16));
                        if (checksumCalculate[0] == msgRecv.checkSum[0] && checksumCalculate[1] == msgRecv.checkSum[1])
                        {
                            parseMessage();

                            msgRecv.StrChkSum.Clear();
                            //sw.WriteLine();
                            //sw.Flush();
                            goto restart;
                        }



                        statusMsgRecv++;
                        messageIndexRecieve = 0;
                    }

                    break;

            }
            return;
            error:
            restart: statusMsgRecv = ModBusStatusType.UNINIT;
            return;
        }
        public override void parseMessage()
        {
            //string[] valueData = new string[7];
            float[] Data = new float[msgRecv.Length / 4];
       

                for (int j = 0; j < msgRecv.Length/4; j++)
                {
                    //valueData[j] = Convert.ToString(msgRecv.data[i]+""+ msgRecv.data[++i] + "" + msgRecv.data[++i] + "" + msgRecv.data[++i]);
                    Data[j] = BitConverter.ToSingle(new byte[] { msgRecv.data[4 * j + 3], msgRecv.data[4 * j + 2], msgRecv.data[4 * j + 1], msgRecv.data[4 * j ] }, 0);
             
                }

            polutions[0].concentration = Data[2];
            polutions[2].concentration = Data[5];
            polutions[1].concentration = Data[6];
            timeLastMessageRecieved = DateTime.Now;
            waitForResponseList[1].Set();
            workingStatus = true;
        }




        public static byte[] ToByteArray(String HexString)
        {
            int NumberChars = HexString.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(HexString.Substring(i, 2), 16);
            }
            return bytes;
        }


        private static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        static byte[] HexToBytes(string input)
        {
            byte[] result = new byte[input.Length / 2];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Convert.ToByte(input.Substring(2 * i, 2), 16);
            }
            return result;
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
