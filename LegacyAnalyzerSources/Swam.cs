using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SadraAQMS.Analyzers
{
    class Swam : deviceAnalyzer
    {
        public int lastIndex = -1;
        bool MessageChannel_A_Recieved = false, MessageChannel_B_Recieved = false;
        public Swam(DeviceType dvType, string devID)
        {
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            //_alarmValue = 0;
        }
        public Swam(DeviceType dvType, string devID, string devName, string ignorableErrors)
        {
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            this.DeviceName = devName;

            int.TryParse(ignorableErrors, out IgnorableErrors);

            //_alarmValue = 0;
        }

        /// <summary>
        /// Start of reading data by enquiring Infromation of Data COMMAND 41.
        /// After reception this COMMAND response the Other Main Read Command (42xxx => {xxx = Index last data }) is Sent
        /// Refer page 133 Manual SWAM
        /// </summary>
        public override void sendReadCommand()
        {
            /*
              دوشامل با فرستادن عدد 41 منظر یک  مسیج از دستگاه می باشیم که از این پیام دریافتی یک عدد که  نقش ایندکس را دارد که در مرحله ی بعد از این عدد استفاده میشه
            دوتا فلگ داریم که یکی   یکی بی  که قبل اینکه دریافت جواب مقدار ای و بی را داشته باشم در ابتدا فالس میکنیم که بعد از دریافت جواب ای  یا بی هر کدام را ترو کنیم
           
             */
            messageSendOnSerialBytes(Encoding.UTF8.GetBytes("41\r"));
            //HalfofMsgRecive = false;// this flag set for recognize First message that this is A or B!
            MessageChannel_A_Recieved = MessageChannel_B_Recieved = false;
        }

        internal override void Disconnect()
        {
            base.Disconnect();

            if (conSerial.IsOpen)
                conSerial.Close();
        }
        //bool HalfofMsgRecive = false;
        #region Serial
        public override void parseByte(byte data)
        {
            /*
             بعد از ارسال پکیج پیام جهت دریافت اطلاعات  جوابی از دستگاه میاید که این جواب و با این تابع بررسی میکنیم و پیام را به پارس مسیج میدهیم
             */
            timeLastByteRecieved = DateTime.Now;
            messageReceived.Add(data);
            if (data == 13)  // Messages end with "\CR" = \r = ASCII 13  
            {
                parseMessage();
                messageReceived.Clear();
            }

            if (messageReceived.Count > 500)   // longest message around 400 bytes(character) 
                messageReceived.Clear();
        }


        public override void parseMessage()
        {
            /*
             وقتی در جواب علامت = یعنی ابتدای مسیح هست
            اگر طول پیام دریافتی 9 باشد یعنی این پیامی است که شامل شماره ایندکس  است که این شماره ایندکس را بعد از ذخیره کردن در لست ایندکس به عدد 42 چسبانده و عدد 2  را برای پلوشن 2.5 ارسال میکنم
            ای دی پولشن 2.5 عدد 7 و ای دی پولوشن 10 عدد 8 می باشد.
            اگر از پولوشن ای دی مپر میخواهیم استفاده کنیم:
            اگر  تعدا پولوشن دوتا باشد که یا تعداد یکی باشد ولی همان یکی هم 2.5 است  
             این 42 و به همراه یکی کمتر از شماره ایندکس و عدد 2 که جهت این ارسال میشود که تا زمانی که  ست نشده است سراغ بعدی نرود  ارسال میشود 
             و اگر پولوشن یکی باشد و ان یکی هم  10 باشد مقدار 42 به همراه ایندکس و عدد 1 جهت ویت ان ارسال میشود
            بعد از اینکه هر دستور ارسال کردیم و دستگاه جواب داد و مجدد به   پارس بایت و سپس به پارس مسیج وارد میشویم و این دفعه بعد از علامت مساوی  طول پیام به دست امده عدد 58 می باشد
             اگر طول 58 بود و در پیام عدد ای بود یعنی مقدار 2.5 را دارد ارسال میکند که مقدار عدد  خانه 54 را در مقدار کنسنتریشن میریزد و ویت عدد 2 را ست میکنیم
            و مقدار فلگ ای را ترو میکنیم
            اگر  هنوز مقدار بی ارسال نشده بود یعنی 10 ترو نشده بود و هنوز کانت پولوشن ها بزرگتر از 1 بود  باید پیام درخواست مقدار 10 با ویت 1 به همراه همان شماره ایندکس 
             ارسال  کنیم و منتظر جواب و ارسال پیام دستگاه باشیم و مجدد چون ویت کرده ایم تا زمانی که ست نشود منتظز جواب 1 می ماند و سپس بعد از پارس بایت و سپس پارس مسیج
            اگر  طول ان پیام 58 عدد بود و  بی در پیام موجود بود 
            یعنی پولوشسن خانه 1 متعلق به 10 می باشد که  یه احتمال وجود دارد که اگر  تعداد کانت یک بود و خانه 0 ان  ای دی 8  ک برای 10 است بود 
            باید شماره ایندکس خانه پولوشن را که در حالت پیشفرض یک است به صفر تغییر دهیم و سپس مقادیر را وارد کنسنتریسشن کنیم
             و سپس مقدار  ویت 1 را ست کنیم و  فلگ ببی ترو شود و در همین حالت باید احتمال اینکه ابتدا بی دریافت شده باشد را در نظر بگیریم سپس با بررسی اینکه اگر ای فالس بود و هنوز کانت بزرگتر از 1 بود  دستور برای دریافت مقدار ای را بدهد با ویت 2 را صدار کنیم


             */
            if (messageReceived[0] == '=')
            {
                try
                {
                    string deviceResponse = Encoding.UTF8.GetString(messageReceived.ToArray());
                    deviceResponse = deviceResponse.Remove(0, 1);
                    string[] tmp = deviceResponse.Split(',');
                    // =00143,00003,00018,008,B,15/11/18,13:15,15/11/18,14:15
                    // 
                    // 00278,11 / 04 / 21 07:00,11 / 04 / 21 08:00,0004,00021,012,A,00:00,00.32,-03.0,000.960,000.836,096.0,293.0,293.0,293.0,294.1,295.8,297.3,017.5,094.6,094.7,094.7,00.4,30.3,35.1,35.1,004.3,11 / 04 / 21 07:59,000:00,+02.8,0020,04635952,01264461,0764,01286946,0278,300.5,094.1,603.6,009,04644104,0123,01258475,0102,00959430,0157,298.1,094.7,603.7,022,00128,   382,0000,   56.4,   64.8,0,01000000
                    // 00279,11 / 04 / 21 07:00,11 / 04 / 21 08:00,0004,00022,012,B,00:00,00.16,+02.0,000.959,000.836,096.3,293.0,293.0,293.0,294.2,295.6,296.8,016.5,094.6,094.7,094.7,00.4,35.0,41.0,41.0,003.8,11 / 04 / 21 07:59,000:00,+02.6,0020,04635952,01264461,0764,01111861,1334,300.5,094.1,603.6,009,04644104,0134,01258475,0102,00803950,1359,298.1,094.7,603.7,022,00128,  1075,0001,  139.8,  160.4,0,01000000
                    // 00279,00004,00022,012,B,11 / 04 / 21,07:00,11 / 04 / 21,08:00

                    if (tmp.Length == 9)
                    {
                        if (int.TryParse(tmp[0], out lastIndex))
                        {
                            // The Main Read Command (42xxx => {xxx = Index last data }) that requires the index of the data (Refer page 133 Manual SWAM)
                            if (polutions.Count > 1 || (polutions.Count == 1 && polutions[0].ID == 7))// 7:PM2.5  8:PM10
                                messageSendOnSerialBytesAwaitingResponse(Encoding.UTF8.GetBytes(String.Format("42{0:D3}\r", lastIndex - 1)), 2);
                            else if (polutions.Count == 1 && polutions[0].ID == 8)
                                messageSendOnSerialBytesAwaitingResponse(Encoding.UTF8.GetBytes(String.Format("42{0:D3}\r", lastIndex)), 1);

                          //  statusLog(string.Format("polutions count: {0}", polutions.Count));

                            //HalfofMsgRecive = true;
                        }
                        else
                            errorLog("[ParseMessage] - " + deviceName + ": Can't convert the index in MSG 41");
                    }
                    else if (tmp.Length == 58 && tmp[6] == "A")//pm2.5
                    {
                        //statusLog(string.Format("A: {0}", polutions[0].concentration));{
                            if (!float.TryParse(tmp[54], out polutions[0].concentration))
                            {
                                errorLog("[error in SWAM Line A dont correct Fotmat concentration] - " + polutions[0].concentration);
                            }
                            // polutions[0].alarmValue = convertWarningSwam(tmp[57].Substring(0, 8));
                            polutions[0].alarmValue = 0;
                      
                        //alarmValue = polutions[0].alarmValue;
                        //statusLog(String.Format("A Recived concentration{0}", System.Convert.ToSingle(tmp[54])));
                        waitForResponseList[2].Set();
                       
                        MessageChannel_A_Recieved = true;
                        if (MessageChannel_B_Recieved == false && polutions.Count > 1)
                        {
                            //  statusLog("A HalfofMsgRecive ");
                            messageSendOnSerialBytesAwaitingResponse(Encoding.UTF8.GetBytes(String.Format("42{0:D3}\r", lastIndex)), 1);
                            //HalfofMsgRecive = true;
                        }
                        else
                        {
                            waitForResponseList[1].Set();
                            workingStatus = true;
                        }
                        
                    }
                    else if (tmp.Length == 58 && tmp[6] == "B")//pm10
                    {
                        int index = 1;
                        if (polutions.Count == 1 && polutions[0].ID == 8)
                            index = 0;
                        try
                        {
                            if (!float.TryParse(tmp[54], out polutions[index].concentration))
                            {
                                errorLog("[error in SWAM Line B dont correct Fotmat concentration] - " + polutions[index].concentration);
                            }
                            polutions[index].alarmValue = 0;
                        }
                        catch (Exception ex)
                        {

                        }

                        //alarmValue = polutions[1].alarmValue;
                        // statusLog(String.Format("B Recived concentration{0}", System.Convert.ToSingle(tmp[54])));
                        waitForResponseList[1].Set();
                        MessageChannel_B_Recieved = true;
                        if (MessageChannel_A_Recieved == false && polutions.Count > 1)
                        {
                            messageSendOnSerialBytesAwaitingResponse(Encoding.UTF8.GetBytes(String.Format("42{0:D3}\r", lastIndex)), 2);
                            //HalfofMsgRecive = true;
                            //statusLog("B HalfofMsgRecive");
                        }
                        else
                            workingStatus = true;
                    }
                    else if (tmp.Length == 58 && tmp[6] == "N")
                    {
                        polutions[1].concentration = 0;
                        polutions[1].alarmValue = 0;
                        polutions[0].concentration = 0;
                        polutions[0].alarmValue = 0;
                        //if (!waitForResponseList.ContainsKey(3))
                        //    waitForResponseList[3] = new AutoResetEvent(true);
                        //else
                        //    waitForResponseList[3].Set();

                        //alarmValue = polutions[1].alarmValue;

                        workingStatus = true;
                    }
                    //If one of line A or B  disconnect recive "R"
                    else if (tmp.Length == 58 && tmp[6] == "R")
                    {
                        //errorLog("[SWAMMM] R ");
                        if (waitForResponseList.ContainsKey(1)) // 1 => work only pm10 Or B
                        {
                            // if  it not work remove comments lines: 136-137 ,142-143   
                            //polutions[1].concentration = 0; 
                            //polutions[1].alarmValue = 0;
                            waitForResponseList[1].Set();
                            //errorLog("[SWAMMM] R 1 ");
                        }
                        else if (waitForResponseList.ContainsKey(2)) // 2 => work only pm2.5 Or A
                        {
                            //polutions[0].concentration = 0;
                            //polutions[0].alarmValue = 0;
                            waitForResponseList[2].Set();
                            //errorLog("[SWAMMM] R 2 ");
                        }
                        workingStatus = true;
                    }

                    //if ((polutions.Count == 2 && MessageChannel_A_Recieved && MessageChannel_B_Recieved)     // if two data record should be read
                    //    || (polutions.Count == 1 && polutions[0].mapperID == 1 && MessageChannel_A_Recieved)   // if one data record A (is equvalent to 1 in mapper ID) should be read
                    //    || (polutions.Count == 1 && polutions[0].mapperID == 2 && MessageChannel_B_Recieved))  // if one data record B (is equvalent to 2 in mapper ID)should be read
                    //{
                    //    statusLog(string.Format("{0},polutions[0].concentration.ToString()"));
                    //    workingStatus = true;
                    //}

                    timeLastMessageRecieved = DateTime.Now;
                    if (writeMode)
                    {
                        sw.WriteLine(deviceResponse);
                        sw.Flush();
                    }
                }
                catch (Exception ex)
                { errorLog("[error parsing in Swam OUTER LOOP] - " + ex.Message); }
            }
        }

        private int convertWarningSwam(string warningStr)
        {
            int alarm32bit = 0;
            for (int k = 0; k < 8; k++)
            {
                alarm32bit = alarm32bit << 4 | (Convert.ToByte(warningStr.Substring(k, 1), 16) & 0x0F);
            }
            return alarm32bit;
        }

        #endregion Serial
    }

}

