using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SadraAQMS.Analyzers
{
    class ESAModBus : ESAAnalyzer
    {
        public ESAModBus(DeviceType dvType, string devID) : base(dvType, devID)
        {
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
        }

        public ESAModBus(DeviceType dvType, string devID, string devName, string ignorableErrors) : base(dvType, devID, devName, ignorableErrors)
        {
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            this.DeviceName = devName;
            int.TryParse(ignorableErrors, out IgnorableErrors);
        }

        /*Request:{STX, Type,  00,   Firt number register:bb,  Length bytes for set polution EXP:09=> for N-NOX-NO2 that one num register is 3 byte, first byte CRC,  second byte CRC}
        {01,03,00,bb,00,09,F5,E9}
         Response:{01,03,12,30,32,40,00,00,00,31,32,41,20,00,00,30,33,41,A0,00,00,Ef,13}
        {00,00,00,40}=> NO=2.0
        */

        byte devID = 1;
        public override void sendReadCommand()
        {
            if (byte.TryParse(DeviceID, out devID))
            {

                byte countPolutions = Convert.ToByte(polutions.Count() * 3);

                List<byte> sendCommand = new List<byte>() { 01, 0x03, 0x00, 0xbb, 0x00, countPolutions };

                var crcsendCommand = checkCRC(true, sendCommand);
                sendCommand.Add(crcsendCommand[0]);
                sendCommand.Add(crcsendCommand[1]);
                sendCommand.Add(13);//این خط باید حذف شود فقط برای کار در تستر اضافه شد
                messageSendOnSerialBytesAwaitingResponse(sendCommand.ToArray(), 1);
            }
            else
                errorLog("[sendReadCommand] DeviceName : " + deviceName + " Bad device  ID : " + DeviceID);

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
                    if (data == 0x01)
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

                    msgRecv.Length = data;
                    messageIndexRecieve = 0;
                    statusMsgRecv++;
                    break;


                /*دوبایت  کد الاینده رو میفرستد که کد هر الاینده در دیتاشیت است و بعد 4 بایت مقدار همان الاینده و بعد مجدد شماره کد گاز بعدی ...
                  Response:{01,03,12,30,32,40,00,00,00,31,32,41,20,00,00,30,33,41,A0,00,00,Ef,13}
                  30,32=>Num Code 02=>NO,  Value=2.0
                  30,33=>Num Code 03=>NO2, Value=10.0
                  31,34=>Num Code 12=>NOX, Value=20.0
                 */
                case ModBusStatusType.GOT_LEN:
                    msgRecv.data.Add(data);
                    //int countPolutions = (polutions.Count() * 4) + (polutions.Count() * 2);

                    if (++messageIndexRecieve >= msgRecv.Length)
                    {
                        statusMsgRecv++; 
                        messageIndexRecieve = 0;
                    }
                        ;
                    break;

                case ModBusStatusType.GOT_PAYLOAD:
                    msgRecv.checkSum.Add(data);
                    if (++messageIndexRecieve >= 2)
                    {
                        List<byte> checksumCalculate = new List<byte>();

                        var crcsendCommand = checkCRC(true, msgRecv.StrChkSum.GetRange(0,msgRecv.StrChkSum.Count - 2));
                        if (msgRecv.checkSum[0] == crcsendCommand[0] && msgRecv.checkSum[1] == crcsendCommand[1])
                        {
                            parseMessage();
                            msgRecv.StrChkSum.Clear();
                            //goto restart;
                        }
                        //statusMsgRecv++;
                        messageIndexRecieve = 0;
                        goto restart;
                    }

                    break;
            }
            return;
        restart: statusMsgRecv = ModBusStatusType.UNINIT;
            return;
        }
        public override void parseMessage()
        {
            float[] Data = new float[msgRecv.Length / 6];


            for (int j = 0; j < msgRecv.Length / 6; j++)
            {
                Data[j] = BitConverter.ToSingle(new byte[] { msgRecv.data[6 * j + 5], msgRecv.data[6 * j + 4], msgRecv.data[6 * j + 3], msgRecv.data[6 * j + 2] }, 0);

            }
            if (polutions.Count() > 1)
            {
                for (int i = 0; i < Data.Count(); i++)
                {
                    polutions[i].concentration = Data[i];
                }
            }
            timeLastMessageRecieved = DateTime.Now;
            waitForResponseList[1].Set();
            workingStatus = true;
        }

        static byte[] checkCRC(bool setCRC, List<byte> allBytes)
        {
            int CRC16 = 0xFFFF;

            int len = 0;
            if (setCRC) len = allBytes.Count;
            else len = allBytes.Count - 2;

            for (int i = 0; i < len; ++i)
            {
                CRC16 = (CRC16 ^ allBytes[i]);
                for (int j = 0; j < 8; j++)
                {
                    if ((CRC16 & 1) == 1)
                    { CRC16 = (CRC16 >>= 1) ^ 0xA001; }
                    else
                    {
                        CRC16 = (CRC16 >>= 1);
                        CRC16 = Convert.ToInt16(CRC16);
                    }

                }
            }
            byte[] crc = BitConverter.GetBytes(CRC16);
            return crc;

        }






    }


}
