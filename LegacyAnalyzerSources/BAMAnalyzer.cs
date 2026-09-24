using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SadraAQMS.Analyzers
{
    class BAMAnalyzer : deviceAnalyzer
    {
        public BAMAnalyzer(DeviceType dvType, string devID)
            : base(dvType, devID) // ✅ صدا زدن سازنده کلاس پایه
        {
            //this.company = DeviceCompany.BAM;
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();

            //_alarmValue = 0;
        }

        public BAMAnalyzer(DeviceType dvType, string devID, string devName, string ignorableErrors)
            : base(dvType, devID) // ✅ صدا زدن سازنده کلاس پایه
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
            if (flagGetCurrentData)
            {
                if (deviceType == DeviceType.BAM2_5 || deviceType == DeviceType.BAM10)
                {
                    messageSendOnSerial("\n\r\n\r64");

                    //messageSendOnSerial("\n\r\n\r\n\r6");
                    //statusLog(string.Format("6"));
                    //Thread.Sleep(300);
                    //statusLog(string.Format("sleep"));
                    //messageSendOnSerial("4");
                    //statusLog(string.Format("4"));
                }
                else if (deviceType == DeviceType.E_BAM2_5 || deviceType == DeviceType.E_BAM10)
                    messageSendOnSerial("\r\r\r\r4"); // get Last Data
                else if (deviceType == DeviceType.Old_BAM10 || deviceType == DeviceType.Old_BAM2_5)
                    messageSendOnSerial("\r\r\r63");
                else if (deviceType == DeviceType.Older_BAM10 || deviceType == DeviceType.Older_BAM2_5)
                    messageSendOnSerial("\r\r\r1");// read command 1 all data day
                //else
                //    messageSendOnSerial("\n\r\n\r64");
            }
        }

        public override void parseMessage()
        {
            // For BAMs
            //deviceRes = "* 6 \n CSV Type Reports \n 2 - Display All Data \n 3 - Display New Data \n 4 - Display Last Data \n 5 - Display All Flow Stats \n 6 - Display New Flow Stats \n 7 - Display All 5-Min Flow \n 8 - Display New 5-Min Flow \n >4 - Display CSV Data Station, 5 \n Time,Conc(mg/m3),Qtot(m3),WS(MPS),WD(DEG),BP(mm),RH(%),Delta(C),AT(C),E,U,M,I,L,R,N,F,P,D,C,T \n 01/30/08 16:00, 0.084, 0.834, 0.0,0,0,30,57.0,27.1,1,0,0,0,0,0,0,0,0,0,0,1,";
            try
            {
                string deviceResponse = Encoding.UTF8.GetString(messageReceived.ToArray());
                deviceResponse = string.Join("", deviceResponse.Split('\0'));

                // deviceResponse = "02/28/18 15:00,   5341, 0.226, 0.002,   0.0, 0.000,    35, 0.000,  21.7,0,0,0,0,0,0,0,0,1,0,0,0";
                //statusLog(deviceResponse);

                if (writeMode)
                {
                    sw.WriteLine(deviceResponse);
                    sw.Flush();
                }
                if (flagGetCurrentData)
                {
                    if (deviceType == DeviceType.BAM2_5 || deviceType == DeviceType.BAM10)
                    {
                        getDataAlarmBAM(deviceResponse);
                    }
                    //For EBAMS  -> response to * 4
                    //deviceRes = "\n AutoMet Data Log Report \n 18-DEC-2008 16:22:45, \n SN,F1768  \n Time,ConcRT(mg/m3),ConcHr(mg/m3),Flow(l/m),WS(m/s),WD(Deg),AT(C),RHx(%),RHi(%),BV(V),FT(C),Alarm,Type  \n  03-DEC-2008 18:00:00,0.018,0.015,16.7,0.3,0,26.4,0,34,14.2,25.8,0,1
                    else if (deviceType == DeviceType.E_BAM2_5 || deviceType == DeviceType.E_BAM10)
                    {
                        getDataAlarmE_BAM(deviceResponse);
                    }
                    else if (deviceType == DeviceType.Old_BAM10 || deviceType == DeviceType.Old_BAM2_5)
                    {
                        getDataAlarmOldBAM(deviceResponse);
                    }
                    else if (deviceType == DeviceType.Older_BAM10 || deviceType == DeviceType.Older_BAM2_5)
                    {
                        getDataAlarmOlderBAM(deviceResponse);
                    }
                }
                else  // get History DATA
                {
                    getHisoryDataBAM(deviceResponse);
                }
            }
            catch (Exception ex)
            {
                errorLog("[BAM Analyzer ParseMessage] - " + ex.Message);
                messageReceived.Clear();
            }
        }

        private void getHisoryDataBAM(string deviceResponse)
        {
            if (deviceType == DeviceType.BAM2_5 || deviceType == DeviceType.BAM10)
            {
                if (deviceResponse.Contains("Report for"))
                {
                    extractDate = Regex.Split(deviceResponse, @"\D+");
                }
                else if (deviceResponse.Contains(":00 "))
                {
                    flagReadDataOnetime = true;
                    string[] value = Regex.Split(deviceResponse, " +");

                    string[] clock = Regex.Split(value[0], ":");

                    var alarm = Regex.Replace(value[1], "[A-Z]", "1");
                    alarm = Regex.Replace(alarm, "-", "0");
                    int alarmVal = Convert.ToInt32(alarm, 2);

                    timeLastMessageRecieved = new DateTime(Convert.ToInt32(extractDate[3]), Convert.ToInt32(extractDate[1]), Convert.ToInt32(extractDate[2]), Convert.ToInt32(clock[0]), Convert.ToInt32(clock[1]), 0);
                    if (polutions.Count > 1)
                    {
                        foreach (var pol in polutions)
                        {
                            if (alarmVal == 0)
                            {
                                DatabaseHandler.InsertAveConcentration(
                                    timeLastMessageRecieved,
                                    polutions[pol.mapperID].ID,
                                    Convert.ToSingle(value[2 + pol.mapperID]) * polutions[pol.mapperID].gain + polutions[pol.mapperID].offset,
                                    false, 9, alarmVal
                                );
                            }
                            else
                            {
                                DatabaseHandler.InsertAveConcentration(
                                    timeLastMessageRecieved,
                                    polutions[pol.mapperID].ID,
                                    Convert.ToSingle(value[2 + pol.mapperID]) * polutions[pol.mapperID].gain + polutions[pol.mapperID].offset,
                                    false, 10, alarmVal
                                );
                            }
                        }
                    }
                    else
                    {
                        if (alarmVal == 0)
                        {
                            DatabaseHandler.InsertAveConcentration(
                                timeLastMessageRecieved,
                                polutions[0].ID,
                                Convert.ToSingle(value[2]) * polutions[0].gain + polutions[0].offset,
                                false, 9, alarmVal
                            );
                        }
                        else
                        {
                            DatabaseHandler.InsertAveConcentration(
                                timeLastMessageRecieved,
                                polutions[0].ID,
                                Convert.ToSingle(value[2]) * polutions[0].gain + polutions[0].offset,
                                false, 10, alarmVal
                            );
                        }
                    }

                    /*NOTE: This alarm shows the alram of special hour. Now, it is not spreated with other alarms. */
                }
                else if (deviceResponse.Contains("Valid Daily") | (deviceResponse.Contains("* ") & flagReadDataOnetime))
                {
                    TimeSpan diffTime = DateTime.Now - timeLastMessageRecieved;
                    flagGetCurrentData = true;
                    flagReadDataOnetime = false;
                    if (diffTime.TotalHours > 1)
                    {
                        Thread.Sleep(3000);
                        getHistory(4, -1);
                    }
                }
                OnSpecialMessageRecived(deviceResponse);
            }
        }

        private void getDataAlarmOldBAM(string deviceResponse)
        {
            try
            {
                int tmpAlarm = 0;
                if (deviceResponse.Contains(":"))
                {
                    string[] tmpStrData = deviceResponse.Split(',');
                    if (tmpStrData.Length > 0)
                    {
                        string[] tmpDateAndTime = tmpStrData[0].Split(' ');
                        extractDate = Regex.Split(tmpDateAndTime[0], @"\D+");
                        string[] clock = Regex.Split(tmpDateAndTime[1], ":");
                        timeLastMessageRecieved = new DateTime(Convert.ToInt32(extractDate[2]) + 2000, Convert.ToInt32(extractDate[0]), Convert.ToInt32(extractDate[1]), Convert.ToInt32(clock[0]), Convert.ToInt32(clock[1]), 0);

                        if (polutions.Count > 1)
                        {
                            foreach (var pol in polutions)
                            {
                                if (!float.TryParse(tmpStrData[pol.mapperID], out pol.concentration))
                                    errorLog(string.Format("Device: {0} ParseMessage: Could not Convert concentration data : {1} to float", deviceName, tmpStrData[pol.mapperID]));
                            }
                        }
                        else
                        {
                            if (!float.TryParse(tmpStrData[1], out polutions[0].concentration))
                                errorLog(string.Format("Device: {0} ParseMessage: Could not Convert concentration data : {1} to float", deviceName, tmpStrData[1]));
                        }
                        if (tmpStrData.Length > 20)
                        {
                            int tmpData = 0;
                            for (int ind = 9; ind < 21; ind++)
                            {
                                if (!int.TryParse(tmpStrData[ind], out tmpData))
                                    errorLog(string.Format("Device: {0} ParseMessage: Could not Convert Alarm data[{2}] : {1} to int", deviceName, tmpStrData[ind], ind));

                                tmpAlarm = (tmpAlarm | tmpData) << 1;
                            }
                            polutions[0].alarmValue = tmpAlarm >> 1;
                        }

                        float conc = Convert.ToSingle(tmpStrData[1]) * polutions[0].gain + polutions[0].offset;
                        byte stat = 0;
                        if (polutions[0].alarmValue == 0)
                        {
                            DatabaseHandler.InsertAveConcentration(timeLastMessageRecieved, polutions[0].ID, conc, false, 9, polutions[0].alarmValue);
                            stat = 1;
                        }
                        else
                        {
                            DatabaseHandler.InsertAveConcentration(timeLastMessageRecieved, polutions[0].ID, conc, false, 10, polutions[0].alarmValue);
                            stat = 6;
                        }
                        DatabaseHandler.UpdatePolutions(conc, timeLastMessageRecieved, polutions[0].ID, stat);
                        workingStatus = true;
                    }
                }
            }
            catch (Exception ex)
            {
                errorLog("[Get Data OldBAM Analyzer] - " + ex.Message);
                messageReceived.Clear();
            }
        }

        private void getDataAlarmE_BAM(string deviceResponse)
        {
            try
            {
                int tmpAlarm = 0;
                if (deviceResponse.Contains(":") && deviceResponse.Contains("."))
                {
                    string[] tmpStrData = deviceResponse.Split(',');

                    if (polutions.Count == 1)
                    {
                        polutions[0].mapperID = 2;
                    }

                    foreach (var pol in polutions)
                    {
                        if (!float.TryParse(tmpStrData[pol.mapperID], out pol.concentration))
                            errorLog(string.Format("Device: {0} ParseMessage: Could not Convert concentration data : {1} to float", deviceName, tmpStrData[pol.mapperID]));
                    }

                    if (!int.TryParse(tmpStrData.Last(), out tmpAlarm))
                        errorLog(string.Format("Device: {0} ParseMessage: Could not Convert alarm data : {1} to int", deviceName, tmpStrData.Last()));

                    polutions[0].alarmValue = tmpAlarm;

                    timeLastMessageRecieved = DateTime.Now;
                    waitForResponseList[1].Set();
                    workingStatus = true;
                }
            }
            catch (Exception ex)
            {
                errorLog("[Get Data E_BAM Analyzer] - " + ex.Message);
                messageReceived.Clear();
            }
        }

        private void getDataAlarmOlderBAM(string deviceResponse)
        {
            try
            {
                /*  01:00------------  0.036  0.816  111.8  2.500  2.500  2.500  111.8  2.500
                    02:00------------  0.035  0.811  111.8  2.500  2.500  2.500  111.8  2.500
                    03:00------------  0.027  0.818  111.8  2.500  2.500  2.500  111.8  2.500
                    04:00------------  0.019  0.813  111.8  2.500  2.500  2.500  111.8  2.500
                    05:00------------  0.014  0.826  111.8  2.500  2.500  2.500  111.8  2.500
                    06:00------------  0.016  0.827  111.8  2.500  2.500  2.500  111.8  2.500
                    07:00------------  0.032  0.826  111.8  2.500  2.500  2.500  111.8  2.500
                    08:00------------  0.042  0.831  111.8  2.500  2.500  2.500  111.8  2.500
                    09:00------------  0.047  0.816  111.8  2.500  2.500  2.500  111.8  2.500
                    10:00------------  0.034  0.790  111.8  2.500  2.500  2.500  111.8  2.500
                    11:00------------  0.037  0.813  111.8  2.500  2.500  2.500  111.8  2.500
                    12:00------------  0.054  0.819  111.8  2.500  2.500  2.500  111.8  2.500
                    13:00------------  0.040  0.809  111.8  2.500  2.500  2.500  111.8  2.500
                    14:00------------  0.124  0.812  111.8  2.500  2.500  2.500  111.8  2.500
                    15:00------------  0.033  0.806  111.8  2.500  2.500  2.500  111.8  2.500
                    16:00------------  0.157  0.805  111.8  2.500  2.500  2.500  111.8  2.500*/

                int dateHour = DateTime.Now.Hour;
                if (deviceResponse.Contains(":") && deviceResponse.Contains(".") && deviceResponse.Contains(dateHour.ToString()))
                {
                    string[] tmpStrData = deviceResponse.Split(' ');
                    polutions[0].concentration = (float)Convert.ToDouble(tmpStrData[3]);
                    timeLastMessageRecieved = DateTime.Now;
                    waitForResponseList[1].Set();
                    workingStatus = true;
                }
            }
            catch (Exception ex)
            {
                errorLog(string.Format("Device: {0} ParseMessage", deviceName));
                messageReceived.Clear();
            }
        }

        private void getDataAlarmBAM(string deviceResponse)
        {
            int tmpAlarm = 0;
            try
            {
                if (deviceResponse.Contains(":"))
                {
                    string[] tmpStrData = deviceResponse.Split(',');

                    if (tmpStrData.Length > 0)
                    {
                        if (polutions.Count == 1)
                            polutions[0].mapperID = 1;

                        foreach (var pol in polutions)
                        {
                            if (!float.TryParse(tmpStrData[pol.mapperID].Trim(), out pol.concentration) || Math.Abs(pol.concentration) <= 0.0001)
                            {
                                workingStatus = false;
                                errorLog(string.Format("Device: {0} ParseMessage: Could not Convert concentration data : {1} to float , devResp: {2}", deviceName, tmpStrData[pol.mapperID], deviceResponse));
                                throw new Exception("Wrong BAM Data");
                            }
                        }
                        if (tmpStrData.Length > 20)
                        {
                            int tmpData = 0;
                            for (int ind = 9; ind < 21; ind++)
                            {
                                if (!int.TryParse(tmpStrData[ind], out tmpData))
                                    errorLog(string.Format("Device: {0} ParseMessage: Could not Convert Alarm data[{2}] : {1} to int", deviceName, tmpStrData[ind], ind));

                                tmpAlarm = (tmpAlarm | tmpData) << 1;
                            }
                            // pm باید اولین پولوتانت باشد
                            polutions[0].alarmValue = tmpAlarm >> 1;
                        }

                        timeLastMessageRecieved = DateTime.Now;
                        waitForResponseList[1].Set();
                        workingStatus = true;
                    }
                }
            }
            catch (Exception ex)
            {
                errorLog("[Get Data BAM Analyzer] - " + ex.Message);
                messageReceived.Clear();
            }
        }

        internal override void Disconnect()
        {
            base.Disconnect();

            if (conSerial.IsOpen)
                conSerial.Close();
        }

        public void changePointerData(int countDataBack)  // every data is for one Hour
        {
            // messageSendOnSerial(string.Format("\n\r\n\r\x1B3 {0}\r", count));
            List<byte> message = new List<byte>(new byte[] { 10, 13, 10, 13, 27, 51, 32 });
            message.AddRange(Encoding.UTF8.GetBytes(countDataBack.ToString()));
            message.Add(13);
            messageSendOnSerialBytes(message.ToArray());
        }

        public void getOldData()
        {
            messageSendOnSerial("\r\r\r3");
        }

        public void getAllOldData()
        {
            messageSendOnSerial("\r\r\r2");
        }

        public void getCurrentDayData()
        {
            messageSendOnSerial("\r\r\r1");
        }

        public void stopGetData()
        {
            messageSendOnSerial("\r");
            flagGetCurrentData = true;
        }

        public override void getHistory(int mode, int pointer, string strDate = "", string endDate = "")
        {
            flagGetCurrentData = false;
            if (deviceType == DeviceType.BAM2_5 || deviceType == DeviceType.BAM10)
            {
                switch (mode)
                {
                    case 0:
                        stopGetData();
                        break;
                    case 1:
                        getCurrentDayData();
                        break;
                    case 2:
                        getAllOldData();
                        break;
                    case 3:
                        changePointerData(pointer);
                        Thread.Sleep(1000);
                        getOldData();
                        break;
                    case 4:
                        getOldData();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
