using System;
using System.Collections.Generic;
using System.Text;


namespace SadraAQMS.Analyzers
{

    public class Visala_WXT510 : deviceAnalyzer
    {
        public Dictionary<int, float> Polution_ValueMapper = new Dictionary<int, float>();
        public Visala_WXT510(DeviceType dvType, string devID)
        {
            //this.company = DeviceCompany.BAM;
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();

            //_alarmValue = 0;
            Polution_ValueMapper.Add(0, 0);
            Polution_ValueMapper.Add(1, 0);
            Polution_ValueMapper.Add(2, 0);
            Polution_ValueMapper.Add(3, 0);
            Polution_ValueMapper.Add(4, 0);
            Polution_ValueMapper.Add(5, 0);
            Polution_ValueMapper.Add(6, 0);
            Polution_ValueMapper.Add(7, 0);

        }

        public Visala_WXT510(DeviceType dvType, string devID, string devName, string ignorableErrors)
        {
            //this.company = DeviceCompany.BAM;
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            this.DeviceName = devName;

            int.TryParse(ignorableErrors, out IgnorableErrors);
            
            
        //_alarmValue = 0;
        }

        #region VARIABLES
        // int timerIntervalGetWeatherDataSec = 10;

        public WeatherData weatherData = new WeatherData();
        #endregion VARIABLE

        #region EVENTS
        // public delegate void WeatherCom_Form_Handler(WeatherData weatherCom_Data);
        //   public event WeatherCom_Form_Handler WeatherCom_Form_Event;

        // public delegate int WeatherCom_DB_Handler(WeatherData weatherCom_Data);
        //  public event WeatherCom_DB_Handler WheatherCom_DB_Event;
        #endregion Events

        //#region Timers
        //public System.Threading.Timer timerReadWeatherData;

        //private void initTimer()
        //{
        //    System.Threading.TimerCallback callback1 = new System.Threading.TimerCallback(RequestWeatherData_TimerTick);
        //    timerReadWeatherData = new System.Threading.Timer(callback1, null, 3000, timerIntervalGetWeatherDataSec * 1000);
        //}

        public override void sendReadCommand()
        {
            messageSendOnSerial("0R0\r\n"); // ????? Correct This

            //statusLog((DateTime.Now - timeLastMessageRecieved).TotalSeconds.ToString());
            //if ((DateTime.Now - timeLastMessageRecieved).TotalSeconds > 120)
            //    workingStatus = false;
            //else
            //    workingStatus = true;
        }


        #region Methods
        internal override void initialize_Connect()
        {
            //statusLog("[init weather] " + workingStatus.ToString());
            base.initialize_Connect();

            waitForResponseList[(byte)'X'] = new System.Threading.AutoResetEvent(false);
            waitForResponseList[(byte)'W'] = new System.Threading.AutoResetEvent(false);
            waitForResponseList[(byte)'T'] = new System.Threading.AutoResetEvent(false);
            waitForResponseList[(byte)'R'] = new System.Threading.AutoResetEvent(false);
            waitForResponseList[(byte)'S'] = new System.Threading.AutoResetEvent(false);

            workingStatus = weatherMessageInit();

            statusLog("[Weatrher init ] - stat: " + workingStatus.ToString());
            //if (workingStatus)
            // initTimer();

        }
        internal override void Disconnect()
        {
            base.Disconnect();

            if (conSerial.IsOpen)
                conSerial.Close();
        }
        bool weatherMessageInit()
        {
            maximumResponseTime = 1000;

            // Refer to Datashet WXT510 page 51
            string tmpMsg = "0XU,M=P\r\n"; // ACSII polled without CRC
            if (messageSendOnSerialwait(tmpMsg, (byte)tmpMsg[1]))
                return false;

            //messageSendOnSerial("0XXU,C=2,B=19200,D=8,P=N,S=1");
            //messageSendOnSerial("0XZ");
            // Refer to Datashet WXT510 page 97
            // 1-8  bit in Serprate  Message R1: Dn,Dm,Dx,Sn,Sm,Sx,-,-   // 1:on , 0:off
            // 9-16 bit in Composite Message R0: Dn,Dm,Dx,Sn,Sm,Sx,-,-   // 1:on , 0:off
            tmpMsg = "0WU,R=0100100001001000\r\n";
            if (messageSendOnSerialwait(tmpMsg, (byte)tmpMsg[1]))  // Dm,Sm in both Messages
                return false;
            // [I]nterval : 60s  [A]verage : 60s [U]nit:m/s  [D]irection Correction = 0 deg  [N]ema Formatter : W(MWV)[wind speed and angle]
            tmpMsg = "0WU,I=60,A=60,U=M,D=0,N=W\r\n";
            if (messageSendOnSerialwait(tmpMsg, (byte)tmpMsg[1]))
                return false;

            // Refer to Datashet WXT510 page 102
            // 1-8  bit in Serprate  Message R1: Pa,Ta,Tp(internal),Ua,-,-,-,-   // 1:on , 0:off
            // 9-16 bit in Composite Message R0: Pa,Ta,Tp(internal),Ua,-,-,-,-   // 1:on , 0:off
            tmpMsg = "0TU,R=1101000011010000\r\n"; //  Pa Pressure Air, Ta, Temprature Air,  Ua Humidity Air ->  in both Messages
            if (messageSendOnSerialwait(tmpMsg, (byte)tmpMsg[1])) // Dm,Sm in both Messages
                return false;
            // [I]nterval : 60s  [P]ressure Unit : hPa  [T]emp Unit : Celsius
            tmpMsg = "0TU,I=60,P=H,T=C\r\n";
            if (messageSendOnSerialwait(tmpMsg, (byte)tmpMsg[1]))
                return false;

            // Refer to Datashet WXT510 page 106
            // Rc: Rain Amount  , Rd: Rain Duration , Ri: Rain Intensity  Hc: Hail Amount  , Hd: Hail Duration , Hi: Hail Intensity 
            // 1-8  bit in Serprate  Message R1: Rc,Rd,Ri,Hc,Hd,Hi,-,-    // 1:on , 0:off
            // 9-16 bit in Composite Message R0: Rc,Rd,Ri,Hc,Hd,Hi,-,-    // 1:on , 0:off
            tmpMsg = "0RU,R=1000000010000000\r\n";  // Rc : Rain Amount in both Messages
            if (messageSendOnSerialwait(tmpMsg, (byte)tmpMsg[1]))
                return false;

            // [I]nterval : 60s  [U]nits : {Me}tric (mm,s,mm/h)  Units for [S]urface hit : {Me}tric (accummmulated hail fall in hits/cm^2,s,hits/(cm^2*h))
            // [M]essurement Auto Send Mode : {T}ime based , [Z]Counter Reset : {A}utomatic
            tmpMsg = "0RU,I=60,U=M,S=M,M=T,Z=A\r\n";
            if (messageSendOnSerialwait(tmpMsg, (byte)tmpMsg[1]))
                return false;

            // Refer to Datashet WXT510 page 111
            // Th : Heating Temprature, Vh : Heating Voltage, Vs: Voltage Suply, Vr: Voltage Reference (3.5V)
            // 1-8  bit in Serprate  Message R1: Th,Vh,Vs,Vr,-,-,-,-   // 1:on , 0:off
            // 9-16 bit in Composite Message R0: Th,Vh,Vs,Vr,-,-,-,-   // 1:on , 0:off
            tmpMsg = "0SU,R=1010000010100000\r\n";  // Th, Vs in both Messages
            if (messageSendOnSerialwait(tmpMsg, (byte)tmpMsg[1]))
                return false;

            // [I]nterval : 60s  [S] Error Messaging Enabled : {Y}es  [H]eating Control Enabled : {Y}es
            tmpMsg = "0SU,I=60,S=Y,H=Y\r\n";
            if (messageSendOnSerialwait(tmpMsg, (byte)tmpMsg[1]))
                return false;

            // Refer to Datashet WXT510 page 51
            tmpMsg = "0XZ\r\n";// Reset the Device 
            if (messageSendOnSerialwait(tmpMsg, (byte)'T'))  // 0TX,Start-up
                return false;


            return true;
        }

        #endregion Methods

        #region Serial
        public override void parseByte(byte data)
        {
            timeLastByteRecieved = DateTime.Now;
            messageReceived.Add(data);
            if (data == 10 && messageReceived[messageReceived.Count - 2] == 13)
            {
                parseMessage();
                messageReceived.Clear();
            }
            if (messageReceived.Count > 200)
                messageReceived.Clear();
        }
        public override void parseMessage()
        {
            if (messageReceived[0] == '0' && messageReceived[1] == 'R' && messageReceived[2] == '0')
            {
                timeLastMessageRecieved = DateTime.Now;
                string deviceResponse = Encoding.UTF8.GetString(messageReceived.ToArray());
                deviceResponse = deviceResponse.Remove(deviceResponse.Length - 2);
                deviceResponse = deviceResponse.Remove(0, 4);

                weatherData.time = DateTime.Now;
                try
                {
                    string[] parts = deviceResponse.Split(',');

                    foreach (string wds in parts)
                    {
                        try
                        {
                            string[] equation = wds.Split('=');
                            if (equation[1][equation[1].Length - 1] == '#') // invalid value
                                weatherData.setVariableValueByAbbreviate(equation[0], 0,ref Polution_ValueMapper);
                            else
                            {
                                weatherData.setVariableValueByAbbreviate(equation[0], float.Parse(equation[1].Remove(equation[1].Length - 1)),ref Polution_ValueMapper);
                                int i = 101;
                                foreach (KeyValuePair<int, float> pair in Polution_ValueMapper)
                                {

                                    polutions[pair.Key].concentration = pair.Value;
                                   // polutions[pair.Key].mapperID = i;
                                }  
                            }
                        }
                        catch (Exception ex) { errorLog("[error parsing in Visala_WXT510 INNER LOOP] - " + ex.Message); }
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
                { errorLog("[error parsing in Visala_WXT510 OUTER LOOP] - " + ex.Message); }
            }
            else if (messageReceived[0] == '0')
            {
                timeLastMessageRecieved = DateTime.Now;
                try { waitForResponseList[messageReceived[1]].Set(); }
                catch { }
            }

        }

        bool messageSendOnSerialwait(string message, byte msgIdentifier)
        {
            //messageSendOnSerialAwaitingResponse(message, msgIdentifier, false);    // Dm,Sm in both Messages
            return !waitForResponseList[msgIdentifier].WaitOne(maximumResponseTime, false);
        }

        #endregion Serial
    }
    public class WeatherData 
    {
     
        public DateTime time;
        //public float windSpeed_, windDirection_;
        //public float temprature_, humidity_, pressure_;
        //public float rainGauge_;
        //public float heaterTemprature_, supplyVoltage_;
        // Dictionary<int, float?> Polution_ValueMapper;


        public float windSpeed, windDirection;
        public float temprature, humidity, pressure;
        public float rainGauge;
        public float heaterTemprature, supplyVoltage;
   
        public void setVariableValueByAbbreviate(string Abbr, float value,ref Dictionary<int,float> Polution_ValueMapper)
        {
            switch (Abbr)
            {
                case "Dm":
                    windDirection = value;
                    Polution_ValueMapper[0]= value;
                    break;
                case "Sm":
                    windSpeed = value;
                    Polution_ValueMapper[1]= value;
                    break;
                case "Ta":
                    temprature = value;
                    Polution_ValueMapper[2]= value;
                    break;
                case "Ua":
                    humidity = value;
                    Polution_ValueMapper[3]= value;
                    break;
                case "Pa":
                    pressure = value;
                    Polution_ValueMapper[4]= value;
                    break;
                case "Rc":
                    rainGauge = value;
                    Polution_ValueMapper[5]= value;
                    break;
                case "Th":
                    heaterTemprature = value;
                    Polution_ValueMapper[6]= value;
                    break;
                case "Vs":
                    supplyVoltage = value;
                    Polution_ValueMapper[7]= value;
                    break;

            }
          
        }

        internal string ValidateString(float? value)
        {
            if (value == null)
                return "--";
            else
                return value.ToString();
        }


    }
}
