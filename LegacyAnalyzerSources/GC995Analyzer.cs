using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SadraAQMS.Analyzers
{
    class GC995Analyzer : deviceAnalyzer
    {
        public GC995Analyzer(DeviceType dvType, string devID)
        {
            //this.company = DeviceCompany.Synspec;
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
        }
        public GC995Analyzer(DeviceType dvType, string devID, string devName, string ignorableErrors)
        {
            // this.company = DeviceCompany.Ecotech;
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            this.DeviceName = devName;
            int.TryParse(ignorableErrors, out IgnorableErrors);
        }

        public override void sendReadCommand()
        {
            messageSendOnSerial("LL\r\n\r\n\r\n");
        }

        public override void parseMessage()
        {
            //deviceRes = "N	 18-04-13	 15:30	 0.15	   59739	  171	 0.00	       0	  257	 0.70	  181961	  414	 0.00	       0	  452	 0.00	       0	  388	 0.19	    5219	   85	 	  885 hPa 	 35.92 ?C	 815.00 steps	 1.48 corr	";
            try
            {
                string deviceResponse = Encoding.UTF8.GetString(messageReceived.ToArray());
                if (deviceResponse[0] == 'N')
                {
                    //System.Windows.Forms.MessageBox.Show(deviceResponse);
                    string[] tmpStrData = deviceResponse.Split(':')[1].Split('\t');
                    for (int ind = 0; ind < polutions.Count; ind++)
                    {
                        // System.Windows.Forms.MessageBox.Show(tmpStrData[3 * ind + 1]);
                        if (deviceType == DeviceType.Synspec_GC995)
                        {
                            if (tmpStrData.Length > 3 * ind + 1 + 2 + 4)    //2: adding data related to 3 * ind + 1.    4: other data like 885 hPa 	 35.92 ?C	 815.00 steps	 1.48 corr
                                if (!float.TryParse(tmpStrData[3 * ind + 1], out polutions[ind].concentration))
                                    errorLog(string.Format("Device: {0} ParseMessage: Could not Convert concentration data : {1} to float for polutionId {2}", deviceName, tmpStrData[3 * ind + 1], polutions[ind]));

                            //try
                            //{
                            //    polutions[ind].concentration = System.Convert.ToSingle(tmpStrData[3 * ind + 1]);
                            //}
                            //catch (Exception ex)
                            //{
                            //    errorLog("[GC995 Analyzer Parse Convert To Single] Index - " + ind.ToString() + " - " + ex.Message);
                            //    return;
                            //}
                        }
                        else if (deviceType == DeviceType.Synspec_GCAlpha)
                        {
                            if (tmpStrData.Length > 3 * ind + 3)
                                if (!float.TryParse(tmpStrData[3 * ind + 3], out polutions[ind].concentration))
                                    errorLog(string.Format("Device: {0} ParseMessage: Could not Convert concentration data : {1} to float for polutionId {2}", deviceName, tmpStrData[3 * ind + 3], polutions[ind]));

                            try
                            {
                                polutions[ind].concentration = System.Convert.ToSingle(tmpStrData[3 * ind + 3]);
                            }
                            catch (Exception ex)
                            {
                                errorLog("[GC Alpha Analyzer Parse Convert To Single] Index - " + ind.ToString() + " - " + ex.Message);
                                return;
                            }
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
                else
                {
                    errorLog("Wrong Resposne from GC995");
                }
            }
            catch (Exception ex)
            {
                errorLog("[GC995 Analyzer ParseMessage] - " + ex.Message);
            }
        }
        internal override void Disconnect()
        {
            base.Disconnect();

            if (conSerial.IsOpen)
                conSerial.Close();
        }

    }
}
