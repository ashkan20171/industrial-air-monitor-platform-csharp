using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SadraAQMS.Analyzers
{
    class AIO2_9800 : deviceAnalyzer
    {
        //Dictionary<int, float> Polution_Value = new Dictionary<int, float>();
        public AIO2_9800(DeviceType dvType, string devID)
        {
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();

            //_alarmValue = 0;
            //Polution_Value.Add(0, 0);
            //Polution_Value.Add(1, 0);
            //Polution_Value.Add(2, 0);
            //Polution_Value.Add(3, 0);
            //Polution_Value.Add(4, 0);
            //Polution_Value.Add(5, 0);
        }

        public AIO2_9800(DeviceType dvType, string devID, string devName, string ignorableErrors)
        {
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            this.DeviceName = devName;

            int.TryParse(ignorableErrors, out IgnorableErrors);


            //_alarmValue = 0;
        }
        public override void sendReadCommand()
        {
          //  messageSendOnSerialBytes(Encoding.UTF8.GetBytes(""));
        }


        //internal override void initialize_Connect()
        //{
        //    try
        //    {
        //        base.initialize_Connect();
        //    }
        //    catch (Exception ex)
        //    {
        //        errorLog("[error initialize_Connect in AIO2_9800 INNER] - " + ex.Message);
        //    }

        //}
        internal override void Disconnect()
        {
            base.Disconnect();

            if (conSerial.IsOpen)
                conSerial.Close();
        }
        public override void parseByte(byte data)
        {
            timeLastByteRecieved = DateTime.Now;
            messageReceived.Add(data);
            if ((data == 10 && messageReceived.Count >= 2 && messageReceived[messageReceived.Count - 2] == 13))
            {
                parseMessage();
                messageReceived.Clear();
            }
            if (messageReceived.Count > 200)
                messageReceived.Clear();
        }
        public override void parseMessage()
        {
            timeLastMessageRecieved = DateTime.Now;
            string deviceResponse = Encoding.UTF8.GetString(messageReceived.ToArray());

            try
            {
                string[] parts = deviceResponse.Split(',');

                for (int i = 0; i < polutions.Count; i++)
                {
                    //polutions[i].concentration=System.Convert.ToSingle(parts[i]);
                    if (!float.TryParse(parts[i], out polutions[i].concentration))
                    {
                        errorLog("[error in AIO2_9800 INNER LOOP for] - "+ parts[i]);
                    }
                   
                }

                timeLastMessageRecieved = DateTime.Now;
                waitForResponseList[1].Set();
                workingStatus = true;

                if (writeMode)
                {
                    sw.WriteLine(deviceResponse);
                    sw.Flush();
                }
            }
            catch (Exception ex)
            { errorLog("[error parsing in AIO2_9800 OUTER LOOP] - " + ex.Message); }
        }

    }
}
