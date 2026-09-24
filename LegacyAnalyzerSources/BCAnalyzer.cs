using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SadraAQMS.Analyzers
{
    class BCAnalyzer : deviceAnalyzer
    {
        //StreamWriter swData;
        public BCAnalyzer(DeviceType dvType)
        {
            //this.company = DeviceCompany.MageeScientific;
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();

            //_alarmValue = 0;

            if (DeviceID == "333")
            {
                writeMode = true;
                //LogBytes = true;
            }

            //string addressLogData = @".\Logs\Data_Aethalometer_AE_" + DateTime.Now.ToString("yyyy.MM.dd") + ".txt";
            //if (File.Exists(addressLogData))
            //    swData = new StreamWriter(addressLogData, true);
            //else
            //    swData = new StreamWriter(addressLogData);
        }

        public BCAnalyzer(DeviceType dvType, string devID, string devName, string ignorableErrors)
        {
            //this.company = DeviceCompany.BAM;
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            this.DeviceName = devName;
            int.TryParse(ignorableErrors, out IgnorableErrors);

            //_alarmValue = 0;

            if (DeviceID == "333")
            {
                writeMode = true;
                //LogBytes = true;
            }

        }

        public override void sendReadCommand()
        {
            messageSendOnSerial("$AE33:D1\r");  // had no use Because It just Broadcast every 5 min for AE31 but 1 min for AE33 required  (this is for new Model)
        }

        public override void parseMessage()
        {
            /*Expanded Data Format: “date”, “time”, UV [370 nm] result,
            Blue [470 nm] result, Green [520 nm] result, Yellow [590 nm]
            result, Red [660 nm] result, IR1 [880 nm, “standard BC”] result,
            IR2 [950 nm] result, air flow (LPM), bypass fraction,
            and then the following columns of data repeated for the seven
            measurement wavelengths:
            sensing zero signal, sensing beam signal, reference zero signal,
            reference beam signal, optical attenuation, air flow (LPM), bypass
            fraction.*/

            //string test = "337,\"02-feb-17\",\"16:05\",  2273,  2313,  2263,  2378,  2230,  BC(2135),  2181,  3.1, 0.0213,  .7316, 0.0213, 2.4577, 1.00, 56.466, 0.0213, 1.6050, 0.0213, 4.5054, 1.00, 45.019, 0.0213, 1.2895, 0.0213, 1.8239, 1.00, 38.923, 0.0213, 1.8130, 0.0213, 2.2652, 1.00, 35.201, 0.0213, 1.3426, 0.0213, 4.1019, 1.00, 32.034, 0.0213, 1.2373, 0.0213, 1.8480, 1.00, 23.555, 0.0213, 1.8318, 0.0213, 1.4862, 1.00, 21.409";
            //string test = " 337,\"02-feb-17\",\"16:10\",  1883,  1901,  1748,  1720,  1798,  1732,  1739,  3.1, 0.0213,  .7152, 0.0213, 2.4572, 1.00, 58.778, 0.0213, 1.5761, 0.0213, 4.5052, 1.00, 46.857, 0.0213, 1.2703, 0.0213, 1.8239, 1.00, 40.449, 0.0213, 1.7885, 0.0213, 2.2641, 1.00, 36.526, 0.0213, 1.3261, 0.0213, 4.1013, 1.00, 33.274, 0.0213, 1.2264, 0.0213, 1.8478, 1.00, 24.449, 0.0213, 1.8168, 0.0213, 1.4863, 1.00, 22.242";
            try
            {
                timeLastMessageRecieved = DateTime.Now;
                string deviceResponse = Encoding.UTF8.GetString(messageReceived.ToArray()).Replace(" ", "");

                if (writeMode)
                {
                    sw.WriteLine(deviceResponse);
                    sw.Flush();
                }

                //swData.Write(deviceResponse);
                //swData.Flush();

                if (deviceType == DeviceType.Aethalometer_AE)
                {
                    string[] tmpStrData = deviceResponse.Split(',');
                    if (tmpStrData.Length > 11) // Normally 53 parts of data
                    {
                        foreach (var pol in polutions)
                        {
                            if (!float.TryParse(tmpStrData[pol.mapperID], out pol.concentration))
                                errorLog(string.Format("Device: {0} ParseMessage: Could not Convert concentration data: {1} to float for polutionId {2}", deviceName, tmpStrData[pol.mapperID], pol.ID));
                            
                            //// ???? Add to gain for BC
                            // pol.concentration /=  1000.0f;
                            //// ????
                        }

                        waitForResponseList[1].Set();
                        workingStatus = true;
                    }
                }
                else if (deviceType == DeviceType.Aethalometer_AE33)    //maybe AE is not necessary!
                {
                    string[] tmpStrData = deviceResponse.Split(',');
                    if (tmpStrData.Length > 50) // Normally more than 70 parts of data
                    {
                        float tmpAlarm = 0;
                        if (!float.TryParse(tmpStrData[32], out tmpAlarm))  //32:alarm????????????????
                            errorLog(string.Format("Device: {0} ParseMessage: Could not Convert concentration data: {1} to float for Alarm", deviceName, tmpStrData[32]));

                        foreach (var pol in polutions)
                        {

                            if (!float.TryParse(tmpStrData[pol.mapperID], out pol.concentration))
                                errorLog(string.Format(
                                    "Device: {0} ParseMessage: Could not Convert concentration data: {1} to float for polutionId {2}",
                                    deviceName, tmpStrData[pol.mapperID], pol.ID));

                            //pol.concentration /= 1000.0f;
                            
                            pol.alarmValue = (int)tmpAlarm;
                        }


                        //alarmValue = tmpAlarm;

                        waitForResponseList[1].Set();
                        workingStatus = true;
                    }
                }
            }
            catch (Exception ex)
            {
                errorLog("[Magniee Scientfic Analyzer ParseMessage] - " + ex.Message);
                messageReceived.Clear();
            }
        }

        public override byte getStatus(int alarm)
        {
            byte state = 1;

            var alarmValue = /*_alarmValue*/alarm & ~IgnorableErrors;

            if (alarmValue > 0)
                state = 4;

            return state;
        }

        internal override void Disconnect()
        {
            base.Disconnect();

            if (conSerial.IsOpen)
                conSerial.Close();
        }
    }
}
