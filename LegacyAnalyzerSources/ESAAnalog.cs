using System;
using System.Text;
using System.IO;
using Automation.BDaq;

namespace SadraAQMS.Analyzers
{
    class ESAAnalog : deviceAnalyzer
    {
        InstantAiCtrl ctrlAI;

        //byte[] Channel; // is the same NO of polutions
        byte currentReadChannel = 0;

        public ESAAnalog(DeviceType dvType, string Channel)
        {
            //company = DeviceCompany.EnviromentSA;
            deviceType = dvType;
            deviceName = dvType.ToString();

            //_alarmValue = 0;
        }
        public ESAAnalog(DeviceType dvType, string devID, string devName, string ignorableErrors)
        {
            //company = DeviceCompany.EnviromentSA;
            deviceType = dvType;
            deviceName = dvType.ToString();
            DeviceName = devName;
            DeviceID = devID;
            int.TryParse(ignorableErrors, out IgnorableErrors);

            //_alarmValue = 0;
        }

        public override void sendReadCommand()
        {
            //double[] values = new double[16];

            bool flagSomeDataRecieved = false;

            double[] values = new double[ctrlAI.ChannelCount];
            //short[] raw = new short[8];
            ErrorCode ret = ctrlAI.Read(0,ctrlAI.ChannelCount, values);

            for (byte k = 0; k < polutions.Count; k++)
            {
                try
                {
                    //ErrorCode ret = ctrlAI.Read(Convert.ToInt32(polutions[k].mapperID), out value);
                    if (ret == ErrorCode.Success)
                    {
                        polutions[k].concentration = (float)values[polutions[k].mapperID];
                        flagSomeDataRecieved = true;
                    }
                    else
                    {
                        polutions[k].concentration = -10000.0f;
                        //    listBox1.Items.Add(ret.ToString());
                        //    //System.out.println(ret.toString());
                        //    //break;
                    }


                        if (writeMode)
                        {
                            sw.WriteLine(polutions[k].concentration);
                            sw.Flush();
                        }
                    
                }
                catch (Exception ex)
                {
                    errorLog("[Advantech AI Read Channels] - CH" + k.ToString() + " - " + ex.Message);
                }

            }

            if (flagSomeDataRecieved)
            {
                waitForResponseList[1].Set();
                workingStatus = true;
                timeLastByteRecieved = DateTime.Now;
                timeLastMessageRecieved = DateTime.Now;
            }


            }

        internal override void initialize_Connect()
        {
            try
            {
                statusLog("[Start Setting Advantech 1713]");
                ctrlAI = new InstantAiCtrl();

                //Step 2: select the device with 'ModeWriteWithReset' mode

                //string ProfileName = "PCI_1713.xml";
                //string path = Path.Combine(Environment.CurrentDirectory, ProfileName);
                //if (File.Exists(path))
                //{
                //    Console.WriteLine("PCI1713");
                //ErrorCode ret = ctrlAI.LoadProfile(path);
                //}
                //else
                //{
                //    Console.WriteLine("No Profile");
                //}

                ctrlAI.SelectedDevice = new DeviceInformation(int.Parse(DeviceID));

                var valueRange = ValueRange.V_Neg10To10;
                var signalType = AiSignalType.Differential;

                if (!string.IsNullOrEmpty(Setting))
                {
                    var st = Setting.Split(',');
                    valueRange = (ValueRange)int.Parse(st[0]);
                    signalType = (AiSignalType)int.Parse(st[1]);
                }


                // Step 3: *** configure the channels if needed ***

                if (ctrlAI.Channels != null)
                    try
                    {


                        foreach (var channel in ctrlAI.Channels)
                        {
                            //bool canedit  = ctrlAI.CanEditProperty;
                            //bool inited = ctrlAI.Initialized;
                            channel.ValueRange = valueRange; // ValueRange.V_Neg10To10;
                            channel.SignalType = signalType; // AiSignalType.Differential;
                            
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        errorLog("ESAAnalog Features : Setting Unsuccessful " + ex.Message);
                        //return;
                    }

                else
                    statusLog("ESAAnalog Features : NO Channels");

                isConnected = true;


                if (ctrlAI == null || ctrlAI.Features == null)
                {
                    errorLog("ESAAnalog Features : NO Features are available");
                    //return;
                }

                string tmp = "";
                try
                {
                    tmp += " ranges : " + string.Join(",", ctrlAI.Features.ValueRanges);
                    tmp += "\r\n  Channel Count :" + ctrlAI.ChannelCount;
                }
                catch (Exception ex)
                {
                    errorLog("ESAAnalog Features : NO ranges " + ex.Message);
                    //return;
                }
                try
                {
                    tmp += "\r\n  ChannelCountBase : " + ctrlAI.Features.ChannelCountBase;
                }
                catch (Exception ex)
                {
                    errorLog("ESAAnalog Features : ChannelCountBase " + ex.Message);
                    //return;
                }
                try
                {
                    tmp += "\r\n  ChannelCountMax : " + ctrlAI.Features.ChannelCountMax;
                }
                catch (Exception ex)
                {
                    errorLog("ESAAnalog Features : NO ChannelCountMax " + ex.Message);
                    //return;

                }
                try
                {
                    tmp += "\r\n  ChannelStartBase : " + ctrlAI.Features.ChannelStartBase;
                }
                catch (Exception ex)
                {
                    errorLog("ESAAnalog Features : NO ChannelStartBase " + ex.Message);
                    //return;

                }
                try
                {
                    tmp += "\r\n  DataSize : " + ctrlAI.Features.DataSize;
                }
                catch (Exception ex)
                {
                    errorLog("ESAAnalog Features : NO DataSize " + ex.Message);
                    //return;

                }
                try
                {
                    tmp += "\r\n  Resolution : " + ctrlAI.Features.Resolution;
                }
                catch (Exception ex)
                {
                    errorLog("ESAAnalog Features : NO Resolution " + ex.Message);
                    //return;
                }
                try
                {
                    tmp += "\r\n  FilterTypes : " + string.Join(",", ctrlAI.Features.FilterTypes);
                }
                catch (Exception ex)
                {
                    errorLog("ESAAnalog Features : NO FilterTypes" + ex.Message);
                    //return;
                }

                statusLog("ESAAnalog Features : " + tmp);

            }
            catch (Exception ex)
            {
                errorLog("[Advantech AI Init] - " + ex.Message);
            }

            base.initialize_Connect();
        }

        internal override void Disconnect()
        {
            if (ctrlAI != null)
                ctrlAI.Cleanup();

            if (writeMode)
                sw.Close();
        }

    }
}
