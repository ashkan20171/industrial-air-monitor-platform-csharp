using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Reflection;

namespace SadraAQMS
{
    // Type for Company Name
    //public enum DeviceCompany
    //{
    //    None = 0, Ecotech, EnviromentSA, Horiba, Synspec, BAM, Hajir, Faratel, EnviroTechnology, Unitec
    //}
     
    // Type for Gas Analyzer devices  // Should be omitted replaced by table
    public enum DeviceType
    {
        None = 0,
        // Ecotech Devices :
        // O3, CO, NOx, SO2, CO2
        Ecotech_CO = 1, Ecotech_NOx, Ecotech_SO2, Ecotech_O3, Ecotech98_CO, Ecotech98_NOx, Ecotech9852_SO2, Ecotech98_O3, Ecotech9850_SO2 = 9,
        Ecotech9842_Nx = 63, Ecotech55_H2S = 64, Ecotech_CO2A = 65,

        // Environmental SA Devices :  
        // O3, CO, NOx, SO2, Organic compunds, Suspended particulate matters
        ESA_O3 = 10, ESA_CO, ESA_Nox = 12, ESA_SO2 = 13, ESA_Organic = 14, ESA_susPariculate2_5 = 15, ESA_susPariculate10 = 16, ESA_susPariculateNew = 17, ESA_MicroStation = 18, ESA_susPariculateNew2_5 = 19, ESA_susPariculateNew10 = 20,
        // Horiba Devices : 
        // O3, CO, NOx, SO2  and Logger All
        Horiba_CO = 21, Horiba_NOx, Horiba_SO2, Horiba370_O3, Horiba370_CO, Horiba370_SO2, Horiba370_Nox, Horiba370_Hydrocarbon, Horiba_Logger = 29, Horiba_O3 = 32, DURAG_PM10 = 61, DURAG_PM2_5 = 62,

        // Synspec Devices :
        // Organic compounds+
        Synspec_GC995 = 30, Synspec_GCAlpha = 31,

        // BAM Device : Met One Instrument
        // Suspended particulate matters
        BAM2_5 = 40, BAM10, E_BAM2_5, E_BAM10, Old_BAM2_5, Old_BAM10,

        // Envirotechlogy www.et.co.uk Device : Met One Instrument
        // Suspended particulate matters
        Enviro_SO2 = 50, Enviro_NOx, Enviro_CO, Enviro_O3, RelComm, Enviro_NOx_O3

        , Unitec = 70
        , Thermo_PM2_5 = 80, Thermo_PM10 = 81

        , TPAnalog = 90
        , Advantech1713 = 91
        , BC_MetOne_1054 = 92
        , Aethalometer_AE = 100, Aethalometer_AE33 = 101
        , DeltaOHMWeather = 102
        // GRIMM Aerosol Technik GmbH & Co. KG
        // Suspended particulate matters
        , Grimm_dust = 103
        , Visala = 104
        , Swam = 105
        , AMA = 106
        , HoribaLan = 107
        , AIO2_9800 = 108
        , Modbus = 109
        , VantageWeather = 110
        , LeqHD2110L = 111
        , WeatherHD52 = 112
        , ESA_OrganicNew = 113
        , ESAModBus_VOC = 114 /*VOC71M*/, ESAModBus_SO2 = 115/*AF22M*/, ESAModBus_O3 = 116 /*O342M*/, ESAMdBus_NOX = 117/*AC32M*/, ESAModBus_‍CO = 118 /*CO12M*/, ESAModBus_‍HC = 119 /*HC51M-> THC,CH4,HCnm*/
        , Teledyne_CO = 120, Teledyne_NOx = 121, Teledyne_SO2 = 122, Teledyne_O3 = 123, Aeroqual = 124,Older_BAM2_5= 125, Older_BAM10 = 126
        , PalasAnalyzers = 127,
        
    }

    // Class for definition of Polutions 
    public class polutionType
    {
        /// <summary>
        ///  ID : polution SDRA AQMS
        /// </summary>
        public byte ID;
        /// <summary>
        /// mapperID : ID in device or location in Message
        /// </summary>
        public int  mapperID;
        public int unitID;
        public float concentration;
        //public int alarmValue;//write set and get for this
        public float gain;
        public float offset;

        public delegate void AlarmValueChangedPolEventHandler(int pollutionId, int alarmValue, int device);
        public event AlarmValueChangedPolEventHandler AlarmValueChangedPerPollution;
        // public int alarmValuePerPolution;
        protected int _alarmValue;
        public int deviceT;
        public int alarmValue
        {
            get { return _alarmValue; }
            set
            {
                if (value != _alarmValue)
                {
                    _alarmValue = value;
                    //deviceAnalyzer objDevice = new deviceAnalyzer();
                    //Type myTypeObj = objDevice.GetType();
                    //FieldInfo[] fields = myTypeObj.GetFields(BindingFlags.Public | BindingFlags.Instance);
                    //for (int i = 0; i < fields.Length; i++)
                    //{
                    //    if (fields[i].Name == "deviceType")
                    //    {
                    //        deviceT = (int)fields[i].GetValue(objDevice);
                    //    }
                    //}
                    AlarmValueChangedPerPollution?.Invoke(ID, _alarmValue, deviceT);
                }
            }
        }

        //public byte DAQchannel;

        public polutionType(byte polID, int deviceTypeId, int alarmVal, byte untID, float _gin = 1, float _offst = 0, byte _mapperID = 0)
        {
            ID = polID;
            unitID = untID;
            concentration = 0;
            gain = _gin;
            offset = _offst;
            mapperID = _mapperID;
            //alarmValue = 0;
            deviceT = deviceTypeId;
            alarmValue = alarmVal;
        }
    }

    // Class for definition of Gas and Particle Analyzers
    public class deviceAnalyzer : Device.SerialDeviceType
    {
        #region Variable & Fields


        public int timeDataReadSec { get; set; }
        public DeviceType deviceType { get; set; }
        public string DeviceName { get; set; }
        //public DeviceCompany company { get; set; }
        public List<string> alarmNames { get; set; }
        public List<polutionType> polutions = new List<polutionType>();
        public int IgnorableErrors;
        //protected int _alarmValue;
        public bool flagGetCurrentData = true;
        public bool flagReadDataOnetime = false;
        public string[] extractDate;
        public bool InCalibrationDataMode = false;
        public DateTime StartCalibrationData;
        //public int alarmValue
        //{
        //    get { return _alarmValue; }
        //    set
        //    {
        //        if (value != _alarmValue)
        //        {
        //            _alarmValue = value;
        //            AlarmValueChanged((int)deviceType, _alarmValue);
        //        }
        //    }
        //}


        public int numberOfDigits { get; set; }
        public bool remoteMode { get; set; }

        public bool writeMode { get; set; }
        public string writePath
        {
            get
            {
                if (!Directory.Exists("AnalyzersStream"))
                    Directory.CreateDirectory("AnalyzersStream");

                return "AnalyzersStream/{0}.txt";

            }
        }

        public StreamWriter sw { get; set; }

        public string DeviceID { get; set; }
        public int maxTryCalibStop = 5;

        public static int counterSecCalibrationAnalyzer = 0;
        internal static System.Threading.Timer timerCalibrationAnalyzer;

        // Timer Read Concentration Data Analyzers
        internal System.Threading.Timer timerRead;

        // Calibration related
        public static System.Threading.Thread doingCalibThread;
        public calibProperties calibParam;
        private DeviceType dvType;
        private string devID;

        public deviceAnalyzer(DeviceType dvType, string devID)
        {
            this.dvType = dvType;
            this.devID = devID;
        }

        public string Setting { get; set; }
        #endregion Variable & Fields

        // Events

        public delegate void CalibrationExitEventHandler();
        public event CalibrationExitEventHandler CalibrationExited;

        public delegate void CalibValueEventHandler();
        public event CalibValueEventHandler CalibrationValue;

        public delegate void AlarmValueChangedEventHandler(int sender, int alarmValue);
        public event AlarmValueChangedEventHandler AlarmValueChanged;

        public delegate void SpecialMessageRecivedHandeler(string message);
        public event SpecialMessageRecivedHandeler SpecialMessageRecived;

        protected void OnSpecialMessageRecived(string message)
        {
            SpecialMessageRecived(message);
        }
        public delegate void sendValueAtEndCalib(int deviceType, CalibrationType calibTypes, List<float> analayzerValue, List<float> exactValue);
        public event sendValueAtEndCalib sendingValueAtEndCalib;

        public delegate void sendDataSignalR(DeviceType type, List<byte> data);
        public event sendDataSignalR sendingDataSignalR;

        // Used for Ecotech USB Data Read
        protected virtual void ReadDataManual() { }

        internal virtual void initialize_Connect()
        {
            waitForResponseList = new Dictionary<byte, AutoResetEvent>();
            waitForResponseList[1] = new AutoResetEvent(false);  // 1 ->  Always data

            if (portCOM.Contains("COM"))    // For RS232 Devices [All others are connected in the overload of this method]
                openSerialPort(true);

            statusLog(" Initialize_Connect for " + deviceName + " isConnected: " + isConnected);
           
            // isConnected = true;  // for tests
            if (isConnected || portCOM.Contains('.') || portCOM.Contains("PRT")) //  || portCOM.Contains('.') -> portCOM.Contains('.') means the device has a IP port then it is lan Unitec
            {
                if (writeMode)
                    sw = new StreamWriter(string.Format(writePath, DeviceName + "_" + DeviceID));

                //isConnected = true;

                // Initialaze Timers for Reading Data from Analyzer Devices                        
                System.Threading.TimerCallback callback = new System.Threading.TimerCallback(tickReadData);
                timerRead = new System.Threading.Timer(callback, null, 100, timeDataReadSec * 1000);

                statusLog(" Timer Created for " + deviceName);
            }

        }

        protected void tickReadData(object o)
        {
            if (InCalibrationDataMode && (DateTime.Now - StartCalibrationData).TotalMinutes >= Properties.Settings.Default.minute4StopCalibration)//greater than 40 minute stop calibration
            {
                ExitCalibrationDataMode();
                CalibrationExited();
            }
            if (InCalibrationDataMode && CalibrationValue != null)
            {
                CalibrationValue();
                // waitForResponseList[1].Set();
            }

            //  if (InCalibrationDataMode)
            // {
            //    Random rnd = new Random();
            //   polutions[0].concentration = (float)rnd.Next(1, 1000);
            // }

            if (isConnected || portCOM.Contains('.') || portCOM.Contains("PRT")) // portCOM.Contains('.') means the device has a IP port then it is lan
            {
                sendReadCommand();
            }
        }


        // Method for reading gas concentration reported by device
        public virtual void sendReadCommand() { throw new NotImplementedException(); }

        public virtual void getHistory(int mode, int count, string dateTimeStr="",string dateTimeEnd="") { throw new NotImplementedException(); }

        internal virtual void EnterCalibrationDataMode()
        {
            InCalibrationDataMode = true;
            //System.Threading.TimerCallback callback = new System.Threading.TimerCallback(tickReadData);//delete -after test calibrasion
            // timerRead = new System.Threading.Timer(callback, null, 100, timeDataReadSec * 1000);//delete -after test calibrasion
            timerRead.Change(1000, (Properties.Settings.Default.Time4SendCalibration) * 1000);
            StartCalibrationData = DateTime.Now;
        }
        internal virtual void ExitCalibrationDataMode()
        {
            InCalibrationDataMode = false;
            timerRead.Change(1000, timeDataReadSec * 1000);
        }
        #region Calibration
        // Methods For Calibration Start in Analyzer
        public virtual void Calibrate() { throw new NotImplementedException(); }

        // start the whole calibration process
        public void startCalibrate(CalibrationType calibType)
        {

            if (doingCalibThread != null)
                if (doingCalibThread.IsAlive)
                {
                    errorLog("[Calibration] - Calibration Thread Busy, Calibration for " + deviceType.ToString() + " hasn't started");
                    doingCalibThread.Abort();
                }
            if (!getCalibPointFromTable(calibType)) // The function role is to construct global var "calibParam" )
            {
                errorLog("[Calibration] - Calibration Point Parameters is wrong [-1]", 2, (byte)calibErrorIDs.Wrong_calibration_point_ID);
                return;
            }

            bool successDilutor = false, successSelonoid = true;

            MDIParent.AnalyzerInCalibrationSpec.calibAnalyzerRunning = true;

            //  [1]: Calibrator = Send Data to calibrator to produce gass =============================================================
            if (calibProperties.calibrator != null)
            {
                successDilutor = calibProperties.calibrator.sendCalibCommand(calibParam.gasCalibPoint);
                if (successDilutor)
                {
                    dilutionCalibrator._calibrationProcessStatus = CalibrationProcessStatus._1CalibratorStartWorking;
                    dilutionCalibrator.calibrationDeviceState |= CalibrationDeviceState.CalibratorGasProducing;
                }
                else
                {
                    calibErrorIDs errID = calibErrorIDs.Calibrator_Gas_Production_Failed;
                    errorLog("[Calibration] " + errID.ToString(), 2, (byte)errID);
                }
            }
            else if (Properties.Settings.Default.GasDiulatorType == 0)  // This is for microstation SA calibration Process
            {
                successDilutor = true;
            }

            //  [2]: Opening related solenoid value ==================================================================================
            if (successDilutor)
            {
                // Open the Gas Related Selonoid Valve
                if (calibParam.gasCalibPoint.solenoidDO < 8)
                {
                    if (!MDIParent.DAQDevice.SelonoidPower(calibParam.gasCalibPoint.solenoidDO, true))
                    {
                        calibErrorIDs errID = calibErrorIDs.DAQ_Selonoid_Power_ON_Failed;
                        errorLog("[Calibration] " + errID.ToString(), 2, (byte)errID);
                        successSelonoid = false;
                    }
                    else
                    {
                        dilutionCalibrator.calibrationDeviceState |= CalibrationDeviceState.SelonoidValveOpen;
                        dilutionCalibrator._calibrationProcessStatus = CalibrationProcessStatus._2SolenoidValuOpen;
                    }
                }
            }

            // [3]: Start Calibration process with sending CMD to Analyzers ==========================================================
            if (successSelonoid & successDilutor)
            {
                if (true) //................................//
                {

                    timerCalibrationAnalyzer = new System.Threading.Timer(new System.Threading.TimerCallback(timerCalibrationAnalyzerTick), null, 0, 1000);

                    doingCalibThread = new Thread(new ThreadStart(this.Calibrate));
                    doingCalibThread.Start();
                    statusLog("[Calibration] - Start of Calibration on Device : " + deviceType.ToString() + " - Type : " + calibType.ToString());
                }
            }
            else
            {
                errorLog("[Calibration] - Can't initiate the Calibrator OR the Selonoid");
                if (successSelonoid)
                {

                    if (MDIParent.DAQDevice.SelonoidPower(calibParam.gasCalibPoint.solenoidDO, false))
                        dilutionCalibrator.calibrationDeviceState &= ~CalibrationDeviceState.SelonoidValveOpen;
                }

                if (successDilutor)
                {
                    if (Properties.Settings.Default.GasDiulatorType != 0)
                    {
                        successDilutor = calibProperties.calibrator.stopGasProduction();
                        if (successDilutor)
                            dilutionCalibrator.calibrationDeviceState &= ~CalibrationDeviceState.CalibratorGasProducing;
                    }
                }
            }

        }
        private void timerCalibrationAnalyzerTick(object state)
        {
            counterSecCalibrationAnalyzer++;
        }
        protected void waitAndsendConcAtEndCalib()
        {
            statusLog("[Calibration] Wait for Stabilization on Device : " + deviceType.ToString() + " - Interval : " + (calibParam.CalibWaitTimeSec).ToString() + " sec");

            Thread.Sleep(calibParam.CalibWaitTimeSec * 950);

            if (deviceType == DeviceType.ESA_MicroStation)  // MicroStation Devices
            {
                sendingValueAtEndCalib((int)deviceType, calibParam.calibType, new List<float> { polutions[1].concentration, polutions[3].concentration, polutions[4].concentration },
                    new List<float> { calibParam.gasCalibPoint.exactGasConcentration, getConcCalibFromTable(calibParam.calibType, 2, true), getConcCalibFromTable(calibParam.calibType, 3, false) }); // NOx,O3,CO 1,3,4
            }
            else if (polutions[0].ID == 1)  // O3  Devices
                sendingValueAtEndCalib((int)deviceType, calibParam.calibType, new List<float>() { polutions[0].concentration }, new List<float>() { calibParam.gasCalibPoint.exactO3Concentration });
            else if (polutions[0].ID == 3)  // NOx Devices
                sendingValueAtEndCalib((int)deviceType, calibParam.calibType, new List<float>() { polutions[1].concentration }, new List<float>() { calibParam.gasCalibPoint.exactGasConcentration });
            else // Other Devices
                sendingValueAtEndCalib((int)deviceType, calibParam.calibType, new List<float>() { polutions[0].concentration }, new List<float>() { calibParam.gasCalibPoint.exactGasConcentration });

            Thread.Sleep(calibParam.CalibWaitTimeSec * 50);
        }

        private bool getCalibPointFromTable(CalibrationType calibType)
        {
            bool successCalibParam = false;
            try
            {
                GasCalibPoint gasPoint;

                byte clbType = 1;
                bool zeroGas = false;
                if (calibType == CalibrationType.Zero || calibType == CalibrationType.CheckZero)
                {
                    zeroGas = true;
                    clbType = 1;
                }
                else
                    clbType = 2;

                DataRow[] calibRow = DatabaseHandler.ds.Tables["calibration_schedule"].Select("DeviceType = " + ((int)deviceType).ToString() + " AND [Action] = " + clbType.ToString());

                DataRow drv = calibRow[0];
                int indexPoint = Convert.ToInt32(drv["CalibGasId"]);

                DataRow[] gasRow = DatabaseHandler.ds.Tables["calib_gas_points"].Select("CalibGasId = " + indexPoint.ToString());
                System.Data.DataRow dr = gasRow[0];

                gasPoint = new GasCalibPoint((int)dr["GasDiulatorPoint"], Convert.ToSingle(dr["GasExactConcentration"]), (UnitType)(Convert.ToByte(dr["GasUnitId"])),
                                                Convert.ToSingle(dr["GasFlow"]), Convert.ToByte(dr["GasInputPort"]), Convert.ToSingle(dr["GasExactO3Concentration"]), zeroGas,
                                                Convert.ToByte(dr["GasSolenoidDO"]));

                calibParam = new calibProperties(calibType, (int)dr["Duration_sec"], gasPoint);
                successCalibParam = true;

            }
            catch (Exception ex)
            {
                calibErrorIDs errID = calibErrorIDs.Problem_Getting_Gas_Calibration_Properties;
                errorLog("[Calibration] " + errID.ToString(), 2, (byte)errID);

                errorLog("[Calibration] " + ex.Message);
            }

            return successCalibParam;
        }

        public float getConcCalibFromTable(CalibrationType calibType, int id, bool getO3)
        {
            try
            {
                byte clbType = 2;
                if (calibType == CalibrationType.Zero || calibType == CalibrationType.CheckZero)
                {
                    clbType = 1;
                }

                DataRow[] calibRow = DatabaseHandler.ds.Tables["calibration_schedule"].Select("DeviceType = " + Convert.ToByte(deviceType).ToString() + " AND [Action] = " + clbType.ToString());

                DataRow drv = calibRow[0];
                int GasIndex;
                if (id == 1)
                    GasIndex = Convert.ToInt32(drv["CalibGasId"]);
                else
                    GasIndex = Convert.ToInt32(drv["CalibGasId" + id.ToString()]);

                DataRow[] gasRow = DatabaseHandler.ds.Tables["calib_gas_points"].Select("CalibGasId = " + GasIndex.ToString());
                System.Data.DataRow dr = gasRow[0];

                if (getO3)
                    return Convert.ToSingle(dr["GasExactO3Concentration"]);
                else
                    return (Convert.ToSingle(dr["GasExactConcentration"]));

            }
            catch (Exception ex)
            {
                calibErrorIDs errID = calibErrorIDs.Problem_Getting_Gas_Calibration_Properties;
                errorLog("[Calibration] " + errID.ToString(), 2, (byte)errID);
                errorLog("[Calibration] - Getting Gas Calibration Properties - " + ex.Message);
            }
            return -1.0f;
        }
        // Method to Stop Solenoid and Gas Diluator of Calibration
        protected virtual void stopCalib(bool forceFully)
        {
            // [5]: Send CMD to Solenoid to Power off  ===============================================================================
            if (calibParam.gasCalibPoint.solenoidDO < 8)
            {
                if (!forceFully || (dilutionCalibrator.calibrationDeviceState.HasFlag(CalibrationDeviceState.SelonoidValveOpen)))
                {
                    if (MDIParent.DAQDevice.SelonoidPower(calibParam.gasCalibPoint.solenoidDO, false))
                    {
                        dilutionCalibrator.calibrationDeviceState &= ~CalibrationDeviceState.SelonoidValveOpen;
                        dilutionCalibrator._calibrationProcessStatus = CalibrationProcessStatus._6SolenoidValueClose;

                    }
                    else
                    {
                        calibErrorIDs errID = calibErrorIDs.DAQ_Selonoid_Power_OFF_Failed;
                        errorLog("[Calibration] " + deviceType.ToString() + " -" + errID.ToString(), 2, (byte)errID);     // Input 2 means error related to Calibration Process               
                    }
                }
            }

            // [6]: Send CMD to Calibrator to Power off  ===============================================================================
            bool success = false;
            if (calibProperties.calibrator != null)
            {
                if (!forceFully || (dilutionCalibrator.calibrationDeviceState.HasFlag(CalibrationDeviceState.CalibratorGasProducing)))
                {
                    for (int i = 0; i < maxTryCalibStop; i++)
                    {
                        success = calibProperties.calibrator.stopGasProduction();
                        if (success)
                        {
                            dilutionCalibrator.calibrationDeviceState &= ~CalibrationDeviceState.CalibratorGasProducing;
                            dilutionCalibrator._calibrationProcessStatus = CalibrationProcessStatus._7CalibratorStopCalib;
                            break;
                        }
                    }
                    if (!success)
                    {
                        calibErrorIDs errID = calibErrorIDs.Stop_of_Calibrator_Gas_Production_Failed;
                        errorLog("[Calibration] " + deviceType.ToString() + " -" + errID.ToString(), 2, (byte)errID);     // Input 2 means error related to Calibration Process               

                        if (!MDIParent.DAQDevice.SelonoidPower(Properties.Settings.Default.CalibratorRelayDO, false))
                        {
                            errID = calibErrorIDs.Stop_of_Calibrator_Gas_Production_Failed;
                            errorLog("[Calibration] " + deviceType.ToString() + " -" + errID.ToString(), 2, (byte)errID);     // Input 2 means error related to Calibration Process               

                        }
                        else
                            dilutionCalibrator._calibratorStatus = CalibratorStatus._0UnInit;
                    }
                }
            }
            else
                success = true;


            dilutionCalibrator._calibrationProcessStatus = CalibrationProcessStatus._0UnInit;
        }

        public void stopCalibForcefully()
        {
            stopCalib(true);
            if (doingCalibThread != null)
                if (doingCalibThread.IsAlive)
                    doingCalibThread.Abort();

            errorLog("[Calibration] - Manual Stop of Calibration", 2, (byte)calibErrorIDs.Manual_calibration_stop);
        }

        #endregion Calibration

        // Methods For Alarm Handling  state : 1 Normal  2:Zero Calib   3: Span Calib  4L:Alarm Error  
        public virtual byte getStatus(int alarm)
        {
            byte state = 1;
            var x = alarm & ~IgnorableErrors;

            if (x > 0)
                state = 4;

            return state;
        }
        //public virtual string alarmTranslate(int alarm)
        //{
        //    string s = Convert.ToString(alarm, 2);
        //    string A = "";
        //    DatabaseHandler.ds.Tables["DeviceTypes"].Select("DeviceType = " + deviceType.ToString());
        //    for (int i = 0; i < s.Length; i++)
        //    {
        //        if (s[s.Length - i - 1].ToString() == "1")
        //        {
        //            //A = ((ds.DeviceTypes.Rows[deviceType]["Alarm_Name" + (15 - i).ToString("D2")]).ToString());
        //            //A += ds.DeviceTypes.DefaultView[0]["Alarm_Name" + (i + 1).ToString("D2")].ToString() + ",";
        //            A += alarmNames[i] + ",";
        //        }
        //    }
        //    A = A.Remove(A.Length - 1);
        //    return A;
        //}

        #region Remote
        // Methods For Remote Screen
        public virtual void prepareRemoteScreen() { }
        public virtual void sendRemoteCommand(byte command) { }
        public virtual System.Drawing.Bitmap getRemoteScreen() { return new System.Drawing.Bitmap(240, 128); }

        public virtual void startRemote() { remoteMode = true; }
        public virtual void stopRemote() { remoteMode = false; }
        #endregion Remote

        protected virtual void OnSendingDataSignalR(DeviceType type, List<byte> data)
        {
            if (sendingDataSignalR != null)
                sendingDataSignalR(type, data);
        }

        internal virtual void Disconnect()
        {
            workingStatus = false;
            if (timerRead != null)
            {
                timerRead.Change(Timeout.Infinite, Timeout.Infinite);
                timerRead.Dispose();
                timerRead = null;
            }

            if (writeMode && sw != null)
                sw.Close();

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Disconnect();
                if (AlarmValueChanged != null)
                {
                    foreach (Delegate d in AlarmValueChanged.GetInvocationList())
                    {
                        AlarmValueChanged -= (AlarmValueChangedEventHandler)d;
                    }

                }

                if (sendingValueAtEndCalib != null)
                {
                    foreach (Delegate d in sendingValueAtEndCalib.GetInvocationList())
                    {
                        sendingValueAtEndCalib -= (sendValueAtEndCalib)d;
                    }
                }

                if (sendingDataSignalR != null)
                {
                    if (Properties.Settings.Default.SignalR)
                    {
                        foreach (Delegate d in sendingDataSignalR.GetInvocationList())
                        {
                            sendingDataSignalR -= (sendDataSignalR)d;
                        }
                    }
                }


                this.AlarmValueChanged = null;
                this.sendingValueAtEndCalib = null;
                this.sendingDataSignalR = null;
            }
            base.Dispose(disposing);
        }
        public override string GetStringData()
        {
            string strData = "The Channels are : \r\n\r\n";
            foreach (var pol in polutions)
            {
                strData += pol.ID + " : " + pol.concentration + "\r\n";
            }
            return strData.Remove(strData.Length - 1, 1);
        }

    }


}
