using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SadraAQMS.Analyzers
{
    class Horiba370Analyzers : deviceAnalyzer
    {
        public Horiba370Analyzers(DeviceType dvType, string devID)
        {
            //this.company = DeviceCompany.Horiba;
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();

            //_alarmValue = 0;
        }
        public Horiba370Analyzers(DeviceType dvType, string devID, string devName, string ignorableErrors)
        {
            //this.company = DeviceCompany.Ecotech;
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            this.DeviceName = devName;

            int.TryParse(ignorableErrors, out IgnorableErrors);

            //_alarmValue = 0;
        }

        public override void sendReadCommand()
        {
            messageSendOnSerialBytesAwaitingResponse(new byte[] { 0x02, 0x44, 0x41, 0x03, 0x30, 0x34 }, 1); // [02]DA[03][CHSUM: 04] 
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
                        msgRecv.checkSum = 0x02 ^ 0x00;
                        msgRecv.StrChkSum = ""; 
                    }
                    break;
                case HoribaLoggerStatusType.GOT02_SYNC:
                    if (data == 32)
                        statusMsgRecv++;
                    else
                        msgRecv.ID += Convert.ToChar(data);

                    break;
                case HoribaLoggerStatusType.GOT_ID:
                    if (data == 0x03)
                        statusMsgRecv++;
                    else
                        msgRecv.Data += Convert.ToChar(data);
                    break;
                case HoribaLoggerStatusType.GOT03_DATA:
                    msgRecv.StrChkSum += Convert.ToChar(data);
                    ++statusMsgRecv;
                    break;
                case HoribaLoggerStatusType.GOT0A_CHKSUM1:
                    msgRecv.StrChkSum += Convert.ToChar(data);
                    Byte chs;

                    if (!Byte.TryParse(msgRecv.StrChkSum, NumberStyles.HexNumber, new CultureInfo("en-US"), out chs))
                        errorLog("[Horiba3XX] [ParseByte] Cant Parse To Hex Data: " + msgRecv.StrChkSum);

                    if (chs != msgRecv.checkSum)
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
        restart: statusMsgRecv = HoribaLoggerStatusType.UNINIT;
            return;
        }

        public override void parseMessage()
        {
            // MD10 001 + 2238 - 02 00 00 003 000000 006 + 6884 - 02 00 00 003 000000 002 + 5426 - 01 0
            //0 0F 003 000000 003 + 4636 - 02 00 1A 003 000000 004 + 8359 - 02 00 1A 003 000000 017
            //      - 2498 + 00 00 0F 003 000000 016 - 1221 - 03 00 0F 003 000000 015 - 2502 + 00 00 0F 003 0
            //00000 012 + 6002 + 01 00 00 003 000000 013 + 0000 + 00 00 FF 003 000000 20

            try
            {
                //msgRecv.Data = "025 +2238-02 00 00 003 000000 006 +6884-02 00 00 003 000000 002 +5426-01 00 0F 003 000000 003 +4636-02 00 1A 003 000000 004 +8359-02 00 1A 003 000000 017 -2498+00 00 0F 003 000000 016 -1221-03 00 0F 003 000000 015 -2502+00 00 0F 003 000000 012 +6002+01 00 00 003 000000 013 +0000+00 00 FF 003 000000";
                //001 +2520-02 00 00 2BD 000000 002 +2780+00 00 00 2BD 000000 003 +2410-02 00 00 001 000000 004 +4350-02 00 00 001 000000 005 +1940-02 00 00 001 000000 006 +9920-02 00 00 2BD 000000 015 +9503+00 00 00 2BD 000000 016 +5975+00 00 00 2BD 000000 017 +3528+00 00 00 2BD 000000 030 +0000+00 00 FF 000 000000 061 +2700+01 00 00 701 000000 
                //006 +1264-01 00 00 008 000000 003 +1360-02 00 00 008 000000 005 +2020-02 00 00 008 000000 004 +3380-02 00 00 008 000000 002 -1800-01 00 00 008 000000 012 +1969+01 00 00 008 000000 021 +5749-01 00 00 008 000000 022 +0000+00 00 FF 008 000000 023 +2472+01 00 00 008 000000 024 +1884+01 00 00 008 000000 025 +7369+02 00 00 008 000000 

                string[] tmpStrData = msgRecv.Data.Trim().Split(' ');
                //int tmpAlarm = 0;


                for (int k = 0; k < polutions.Count; k++)
                {
                    polutions[k].concentration = tmpStrData.Length > 6 * k ? getFloatNumHoribaLogger(tmpStrData[6 * k + 1]) : - 10000f;
                    polutions[k].alarmValue = tmpStrData.Length > 6 * k + 3 ? (Convert.ToByte(tmpStrData[6 * k + 3], 16)) : 0;
                }
              //  alarmValue = polutions[0].alarmValue;

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
                //float tmp = Convert.ToSingle(numStr.Substring(0, 5));
                //tmp *= (float)Math.Pow(10, Convert.ToSingle(numStr.Substring(5, 3)));

                float num, pow;

                if (float.TryParse(numStr.Substring(0, 5), out num))
                    if (float.TryParse(numStr.Substring(5, 3), out pow))

                        return num * (float)Math.Pow(10, pow);
            }
            catch (Exception ex)
            {
                errorLog("[Problem Converting in Horiba 370] for " + numStr + " - " + ex.Message);
            }

            return -10000f;
        }

        public override byte getStatus(int alarm)
        {
            byte state = base.getStatus(alarm);

            // ******* check the protocol ***************************

            if ((/*_alarmValue*/alarm & (1 << 11)) > 0)
                state = 3; // Span

            if ((/*_alarmValue*/alarm & (1 << 10)) > 0)
                state = 2; // Zero

            return state;
        }

        internal override void Disconnect()
        {
            if (conSerial != null && conSerial.IsOpen)
                conSerial.Close();

            base.Disconnect();

        }

    }
}
