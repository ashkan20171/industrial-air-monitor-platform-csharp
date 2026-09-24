using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SadraAQMS.Analyzers
{
    class TPAnalog : deviceAnalyzer
    {
        //byte[] Channel; // is the same NO of polutions
        byte   currentReadChannel = 0;
        public TPAnalog(DeviceType dvType, string Channel)
        {
            //this.company = DeviceCompany.;
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();

            //_alarmValue = 0;
        }
        public TPAnalog(DeviceType dvType, string devID, string devName, string ignorableErrors)
        {
            //this.company = DeviceCompany.Ecotech;
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            this.DeviceName = devName;
            int.TryParse(ignorableErrors, out IgnorableErrors);

            //_alarmValue = 0;
        }


        int dataCount;
        public override void sendReadCommand()
        {
            for (byte k = 0; k < polutions.Count; k++ )
            {
                try
                {
                    byte[] data = Encoding.UTF8.GetBytes("VI" + polutions[k].mapperID.ToString("D3") + "\r\n");
                    currentReadChannel = k;
                    messageSendOnSerialBytesAwaitingResponse(data, (byte)(k + 10));
                    if(!waitForResponseList[(byte)(k + 10)].WaitOne(maximumResponseTime)) // in case of no response set -10000.0f in Concentratopn (gain and offset considered (-10000.0 - offset)/gain )
                    {
                        polutions[currentReadChannel].concentration = (-10000.0f - polutions[currentReadChannel].offset) / (polutions[currentReadChannel].gain != 0 ? polutions[currentReadChannel].gain : 1);// - 10000.0f;
                        dataCount++;
                    }

                }
                catch (Exception ex)
                {
                    errorLog("SendReadCommand to TPAnalog for polutionId " + polutions[k].ID + " : " + ex.Message);
                }
            }
        }

        public override void parseMessage()
        {
            // Input Voltage
            /// 2.0997\r\n
            /// 
            try
            {
                timeLastMessageRecieved = DateTime.Now;

                string deviceResponse = Encoding.UTF8.GetString(messageReceived.ToArray());

                if (!float.TryParse(deviceResponse.Split(',')[0], out polutions[currentReadChannel].concentration))
                        errorLog(string.Format("Device: {0} ParseMessage: Could not Convert concentration data : {1} to float for polutionId {2}", deviceName, deviceResponse, polutions[currentReadChannel]));

                //polutions[currentReadChannel].concentration = System.Convert.ToSingle(deviceResponse);
                waitForResponseList[(byte)(currentReadChannel + 10)].Set();

                if (++dataCount >= polutions.Count)
                {
                    waitForResponseList[1].Set();
                    workingStatus = true;
                }

                if (writeMode)
                {
                    sw.WriteLine(deviceResponse);
                    sw.Flush();
                }
            }
            catch (Exception ex)
            {
                errorLog(ex,"[TP Analog Analyzer ParseMessage] - " + ex.Message);
                messageReceived.Clear();
            }
        }
        internal override void Disconnect()
        {
            base.Disconnect();

            if (conSerial!=null && conSerial.IsOpen)
            {
                conSerial.Close();
                conSerial.Dispose();
            }
        }

    }
}
