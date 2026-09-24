using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SadraAQMS.Analyzers
{
    class VantageWeather : deviceAnalyzer
    {
        public VantageWeather(DeviceType dvType, string devID)
        {
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
        }
        public VantageWeather()
        {
        }

        public VantageWeather(DeviceType dvType, string devID, string devName, string ignorableErrors)
        {
            //this.company = DeviceCompany.BAM;
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            this.DeviceName = devName;

            int.TryParse(ignorableErrors, out IgnorableErrors);

            //_alarmValue = 0;
        }
        public override void sendReadCommand()
        {
            byte[] sendCommand = new byte[] { 0X4c, 0X4f, 0X4f, 0X50, 0X20, 0X31, 0X0a };
            messageSendOnSerialBytesAwaitingResponse(sendCommand, 1);

        }

        byte messageIndexRecieve;
        public enum Vantage_WeatherStatusType
        {
            UNINIT = 0, GOT_L, GOT_O1, GOT_O2, GOT_P, GOT_Status, GOT_NextRecord, GOT_PAYLOAD,GOT_LF,GOT_CR, GOT_CHECKSUM, GOT_Length
        };

        Vantage_WeatherStatusType statusMsgRecv = Vantage_WeatherStatusType.UNINIT;


        public class MessagVantage_Weather
        {

            public List<byte> StrChkSum { get; set; }
            public List<byte> checkSum { get; set; }
            public List<byte> data { get; set; }
            public byte Length;
            public MessagVantage_Weather()
            {
                data = new List<byte>();
                //data = "";
                StrChkSum = new List<byte>();
                checkSum = new List<byte>();
                Length = 0;
            }
        }


        MessagVantage_Weather msgRecv = new MessagVantage_Weather();
        /*
         
         Write:  LOOP 1 => Get One Data , LOOP 2 => Get Two Data ,....

         Read: [L, O, O,  P,{Packet Type 0 for LOOP and 1 for LOOP2 packet},{Next Record},Barometer,Barometer,Inside Temperature,Inside Temperature,.....] Page 22 of Data Sheet

                4c 4f 4f ec 00 e9 00 5b 64 fe 02 23 eb 02 00 00
                27 01 ff ff ff ff ff ff ff ff ff ff ff ff ff ff
                ff 28 ff ff ff ff ff ff ff 00 00 00 00 00 00 00
                ff ff 00 00 00 00 14 00 12 00 15 00 15 00 ff ff
                ff ff ff ff ff 00 00 00 00 00 00 00 00 00 00 00
                00 00 00 00 00 00 00 f3 02 03 c0 45 05 1e 09 0a
                0d 46 63                                       
        */
        int cntError = 0;
        public override void parseByte(byte data)
        {
            //msgRecv.StrChkSum.Clear();
            //msgRecv.data.c
            timeLastByteRecieved = DateTime.Now;
            
            if (statusMsgRecv < Vantage_WeatherStatusType.GOT_CR)
                msgRecv.StrChkSum.Add(data);


            switch (statusMsgRecv)
            {
                case Vantage_WeatherStatusType.UNINIT:
                    if (data == 0x4C) { statusMsgRecv++;  msgRecv.data.Clear(); }
                    else goto error;
                    break;

                case Vantage_WeatherStatusType.GOT_L:
                    if (data == 0x4f) statusMsgRecv++;
                    else goto error;
                    break;

                case Vantage_WeatherStatusType.GOT_O1:
                    if (data == 0x4f) statusMsgRecv++;
                    else goto error;
                    break;

                case Vantage_WeatherStatusType.GOT_O2:
                    //if (data == 0xec)
                    statusMsgRecv++;
                    break;

                case Vantage_WeatherStatusType.GOT_P:
                    if (data == 0x00)
                    {
                        statusMsgRecv++;
                        messageIndexRecieve = 0;

                    }
                    break;
                case Vantage_WeatherStatusType.GOT_Status:
                    if (++messageIndexRecieve >= 2)
                    {
                        statusMsgRecv++;
                        messageIndexRecieve = 0;
                    }

                    break;
                case Vantage_WeatherStatusType.GOT_NextRecord:
                    //msgRecv.StrChkSum.Add(data);
                    msgRecv.data.Add(data);
                    if (++messageIndexRecieve >= 88)
                    {
                        statusMsgRecv++;
                        messageIndexRecieve = 0;
                    }
                    break;
                case Vantage_WeatherStatusType.GOT_PAYLOAD://LF
                    if (data == 0x0A) statusMsgRecv++;
                    break;
                case Vantage_WeatherStatusType.GOT_LF://CR
                    if (data == 0x0D) statusMsgRecv++;
                    break;
                case Vantage_WeatherStatusType.GOT_CR:
                    msgRecv.checkSum.Add(data);
                    if (++messageIndexRecieve >= 2)
                        statusMsgRecv++;
                    break;
                case Vantage_WeatherStatusType.GOT_CHECKSUM:
                   // List<byte> StrChkSum = new List<byte>() { 0x4c, 0x4f, 0x4f, 0xec, 0x00, 0xea, 0x00, 0x5b, 0x64, 0xff, 0x02, 0x23, 0xeb, 0x02, 0x00, 0x00, 0x28, 0x01, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x28, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00, 0x14, 0x00, 0x13, 0x00, 0x15, 0x00, 0x15, 0x00, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xf3, 0x02, 0x03, 0xa1, 0x45, 0x05, 0x1e, 0x09, 0x0a, 0x0d };//crc: 146 253

                    List<byte> checksumCalculate = new List<byte>();
                    InitialCrcValue initialValue = InitialCrcValue.Zeros;
                    Crc16Ccitt crc = new Crc16Ccitt(initialValue);
                    string CalculateChecksum =
                     crc.ComputeChecksum(msgRecv.StrChkSum.ToArray()).ToString("x2");
                    checksumCalculate.Add(Convert.ToByte(CalculateChecksum.Substring(0, 2), 16));
                    checksumCalculate.Add(Convert.ToByte(CalculateChecksum.Substring(2, 2), 16));
                    if (checksumCalculate[0] == msgRecv.checkSum[0] && checksumCalculate[1] == msgRecv.checkSum[1])
                    {
                        parseMessage();
                        msgRecv.StrChkSum.Clear();
                        goto restart;
                    }
                    messageIndexRecieve = 0;
                    break;
                    
            }
            return;
            error: cntError++;
            restart: statusMsgRecv = Vantage_WeatherStatusType.UNINIT;
            return;
        }
        public override void parseMessage()
        {
            //string[] valueData = new string[7];
            float[] Data = new float[msgRecv.Length / 4];

            int polIdx_Barometer = polutions.FindIndex( x => x.ID == 105);
            if (polIdx_Barometer >= 0)
                polutions[polIdx_Barometer].concentration = BitConverter.ToUInt16(msgRecv.data.GetRange(0, 2).ToArray(),0)*0.0338639f;

            int polIdx_outTemperature = polutions.FindIndex(x => x.ID == 103);
            if (polIdx_outTemperature >= 0)
                polutions[polIdx_outTemperature].concentration = ((BitConverter.ToUInt16(msgRecv.data.GetRange(5, 2).ToArray(),0)*0.1f) - 32.0f)*0.5556f;

            int polIdx_WindSpeed = polutions.FindIndex(x => x.ID == 101);
            if (polIdx_WindSpeed >= 0)
                polutions[polIdx_WindSpeed].concentration = msgRecv.data[7];

            int polIdx_WindDirection = polutions.FindIndex(x => x.ID == 102);
            if (polIdx_WindDirection >= 0)
                polutions[polIdx_WindDirection].concentration = BitConverter.ToUInt16(msgRecv.data.GetRange(9, 2).ToArray(), 0);

            int polIdx_OutHumidity = polutions.FindIndex(x => x.ID == 104);
            if (polIdx_OutHumidity >= 0)
                polutions[polIdx_OutHumidity].concentration =msgRecv.data[26];

            int polIdx_RainRate = polutions.FindIndex(x => x.ID == 106);
            if (polIdx_RainRate >= 0)
                polutions[polIdx_RainRate].concentration = BitConverter.ToUInt16(msgRecv.data.GetRange(34, 2).ToArray(), 0);

            int polIdx_UV = polutions.FindIndex(x => x.ID == 121);
            if (polIdx_UV >= 0)
                polutions[polIdx_UV].concentration = msgRecv.data[36];

            int polIdx_SolarRadiation = polutions.FindIndex(x => x.ID == 120);
            if (polIdx_SolarRadiation >= 0)
                polutions[polIdx_SolarRadiation].concentration = BitConverter.ToUInt16(msgRecv.data.GetRange(37, 2).ToArray(), 0);

            int polIdx_RainStorm = polutions.FindIndex(x => x.ID == 119);
            if (polIdx_RainStorm >= 0)
                polutions[polIdx_RainStorm].concentration = BitConverter.ToUInt16(msgRecv.data.GetRange(38, 2).ToArray(), 0);

            int polIdx_RainDay = polutions.FindIndex(x => x.ID == 118);
            if (polIdx_RainDay >= 0)
                polutions[polIdx_RainDay].concentration = BitConverter.ToUInt16(msgRecv.data.GetRange(43, 2).ToArray(), 0);

            timeLastMessageRecieved = DateTime.Now;
            waitForResponseList[1].Set();
            workingStatus = true;
        }

        public enum InitialCrcValue { Zeros, NonZero1 = 0xffff, NonZero2 = 0x1D0F }

        public class Crc16Ccitt
        {
            const ushort poly = 4129;
            ushort[] table = new ushort[256];
            ushort initialValue = 0;

            public ushort ComputeChecksum(byte[] bytes)
            {
                ushort crc = this.initialValue;
                for (int i = 0; i < bytes.Length; ++i)
                {
                    crc = (ushort)((crc << 8) ^ table[((crc >> 8) ^ (0xff & bytes[i]))]);
                }
                return crc;
            }

            public byte[] ComputeChecksumBytes(byte[] bytes)
            {
                ushort crc = ComputeChecksum(bytes);
                return BitConverter.GetBytes(crc);
            }

            public Crc16Ccitt(InitialCrcValue initialValue)
            {
                this.initialValue = (ushort)initialValue;
                ushort temp, a;
                for (int i = 0; i < table.Length; ++i)
                {
                    temp = 0;
                    a = (ushort)(i << 8);
                    for (int j = 0; j < 8; ++j)
                    {
                        if (((temp ^ a) & 0x8000) != 0)
                        {
                            temp = (ushort)((temp << 1) ^ poly);
                        }
                        else
                        {
                            temp <<= 1;
                        }
                        a <<= 1;
                    }
                    table[i] = temp;
                }
            }
        }

        //public static class Crc16
        //{

            //const ushort polynomial = 0x00FF;
            //static readonly ushort[] table = new ushort[256];

            //public static ushort ComputeChecksum(byte[] bytes)
            //{
            //    ushort crc = 0xFFFF;
            //    for (int i = 0; i < bytes.Length; ++i)
            //    {
            //        byte index = (byte)(crc ^ bytes[i]);
            //        crc = (ushort)((crc >> 8) ^ table[index]);
            //    }
            //    return crc;
            //}

            //static Crc16()
            //{
            //    ushort value;
            //    ushort temp;
            //    for (ushort i = 0; i < table.Length; ++i)
            //    {
            //        value = 0;
            //        temp = i;
            //        for (byte j = 0; j < 8; ++j)
            //        {
            //            if (((value ^ temp) & 0x0001) != 0)
            //            {
            //                value = (ushort)((value >> 1) ^ polynomial);
            //            }
            //            else
            //            {
            //                value >>= 1;
            //            }
            //            temp >>= 1;
            //        }
            //        table[i] = value;
            //    }
            //}

           
        //}
    }
}
