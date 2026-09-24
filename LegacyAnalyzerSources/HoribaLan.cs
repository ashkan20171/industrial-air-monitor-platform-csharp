using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SadraAQMS.Analyzers
{
    class HoribaLan : deviceAnalyzer
    {

        public HoribaLan(DeviceType dvType, string devID)
        {
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();

            //_alarmValue = 0;

        }
        public HoribaLan(DeviceType dvType, string devID, string devName, string ignorableErrors)
        {
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            this.DeviceName = devName;

            int.TryParse(ignorableErrors, out IgnorableErrors);


            //_alarmValue = 0;
        }
        public override void sendReadCommand()
        {
            try
            {
                SendReadCommand();

            }
            catch (Exception ex)
            {
                errorLog("sendReadCommand problem for device analyzer HoribaLan: " + ex.Message);
            }
        }

        public void stopGetData()
        {
            messageSendOnSerial("\r");

            flagGetCurrentData = true;
        }
        private void SendReadCommand()
        {
            try
            {
                //byte[] first_get = { 0x8c, 0x8e, 0x76, 0x00, 0xbd, 0x82, 0x2c, 0x4d, 0x54, 0xea, 0xbe, 0x90, 0x08, 0x00, 0x45, 0x00, 0x01, 0x42, 0x53, 0x0a, 0x40, 0x00, 0x80, 0x06, 0x00, 0x00, 0xc0, 0xa8, 0x00, 0xc8, 0xc0, 0xa8, 0x00, 0xb8, 0xc8, 0x45, 0x00, 0x50, 0x02, 0x71, 0xac, 0x46, 0x03, 0x81, 0x45, 0xc2, 0x50, 0x18, 0x40, 0x29, 0x84, 0x05, 0x00, 0x00 };

                //get after cgi to http
                //byte[] get = { 0x2f, 0x63, 0x67, 0x69, 0x2d, 0x62, 0x69, 0x6e, 0x2f, 0x63, 0x67, 0x69, 0x2d, 0x69, 0x6f, 0x78, 0x3f, 0x70, 0x72, 0x6f, 0x63, 0x3d, 0x36, 0x30, 0x26, 0x70, 0x61, 0x74, 0x68, 0x3d, 0x69, 0x6f, 0x78, 0x2f, 0x61, 0x63, 0x74, 0x75, 0x61, 0x6c, 0x2d, 0x76, 0x61, 0x6c, 0x75, 0x65, 0x73, 0x2f, 0x61, 0x76, 0x30, 0x2e, 0x74, 0x78, 0x74, 0x26, 0x44, 0x61, 0x74, 0x65, 0x53, 0x65, 0x70, 0x3d, 0x2f, 0x26, 0x44, 0x65, 0x63, 0x69, 0x6d, 0x61, 0x6c, 0x53, 0x65, 0x70, 0x3d, 0x2e, 0x26, 0x44, 0x61, 0x74, 0x65, 0x46, 0x6f, 0x72, 0x6d, 0x61, 0x74, 0x3d, 0x79, 0x6d, 0x64, 0x26, 0x43, 0x72, 0x6f, 0x73, 0x73, 0x54, 0x61, 0x62, 0x6c, 0x65, 0x3d, 0x6e, 0x26, 0x46, 0x69, 0x65, 0x6c, 0x64, 0x53, 0x65, 0x70, 0x3d, 0x74, 0x61, 0x62, 0x26, 0x52, 0x65, 0x73, 0x6f, 0x6c, 0x75, 0x74, 0x69, 0x6f, 0x6e, 0x3d, 0x68, 0x69, 0x67, 0x68, 0x26, 0x55, 0x6e, 0x69, 0x74, 0x3d, 0x31, 0x26, 0x45, 0x78, 0x74, 0x49, 0x6e, 0x66, 0x6f, 0x3d, 0x31, 0x35 };

                //byte[] pass = { 0x41, 0x75, 0x74, 0x68, 0x6f, 0x72, 0x69, 0x7a, 0x61, 0x74, 0x69, 0x6f, 0x6e, 0x3a, 0x20, 0x42, 0x61, 0x73, 0x69, 0x63, 0x20, 0x61, 0x47, 0x39, 0x79, 0x61, 0x57, 0x4a, 0x68, 0x4f, 0x6e, 0x42, 0x68, 0x63, 0x33, 0x4e, 0x33, 0x62, 0x33, 0x4a, 0x6b, 0x0d, 0x0a };

                //string Ip = Encoding.ASCII.GetString(first_get);
                //string Get = Encoding.ASCII.GetString(get);
                //errorLog("sendReadCommand GET for device analyzer HoribaLan: " + Get);
                //string password= Encoding.ASCII.GetString(pass);

                //foreach(DataRow r in DatabaseHandler.ds.Tables["analyzer_parameters"].Rows)
                //{
                //    if(r["Device_Type"].ToString() =="107")
                //        r["Description"] = password;
                //}
                //DatabaseHandler.ds.Tables["analyzer_parameters"].WriteXml("AnalyzerParameters.xml", XmlWriteMode.WriteSchema);
                String username = "horiba";
                String password = "password";
                //"aG9yaWJhOnBhc3N3b3Jk"
                String encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
                // http://192.168.0.183/cgi-bin/cgi-iox/data.xml?proc=801&DateSep=%2F&DecimalSep=.&DateFormat=ymd&FieldSep=%3CTab%3E&Resolution=low&Unit=1&_=1686568013456
                    WebRequest request = WebRequest.Create("http://" + portCOM + "/cgi-bin/cgi-iox?proc=60&path=iox/actual-values/av0.txt&DateSep=/&DecimalSep=.&DateFormat=ymd&CrossTable=n&FieldSep=tab&Resolution=high&Unit=1&ExtInfo=15");
                    request.Headers.Add("Authorization", "Basic " + encoded);
                    request.Credentials = CredentialCache.DefaultCredentials;
                    WebResponse response = request.GetResponse();
                    string ct = response.ContentType;
                    Stream objStream = response.GetResponseStream();
                    BinaryReader breader = new BinaryReader(objStream);
                    byte[] buffer = breader.ReadBytes((int)response.ContentLength);
                    foreach (byte dataByte in buffer)
                        parseByte(dataByte);
            }
            catch (Exception ex)
            {
                errorLog("[Historical Data flagGetCurrentData is false] - " + ex.Message);
            }

        }

        private void SendReadCommand_RecoveryData(string strDate = "", string endDate = "")
        {
            statusLog(string.Format("SendReadCommand_RecoveryData:{0} -- {1}  ", strDate.ToString(), endDate.ToString()));

            String username = "horiba";
            String password = "password";
            //String encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
            //WebRequest request;

            try
            {
               // DateTime Startdate = DateTime.Parse(strDate);
               // DateTime EndDate = DateTime.Parse(endDate);


                // request = (HttpWebRequest)WebRequest.Create("http://192.168.0.229/cgi-bin/cgi-iox?proc=5");

                //request.Method = "POST";
                //request.ContentType = "application/x-www-form-urlencoded";
                //request.Headers.Add("Authorization", "Basic aG9yaWJhOnBhc3N3b3Jk");
                //using (var client = new HttpClient())
                //{
                //    client.DefaultRequestHeaders.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");

                //    var response = (HttpWebResponse)request.GetResponse();

                //    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    statusLog("SendReadCommand_RecoveryData 1111");
                   // using (var client1 = new HttpClient { BaseAddress = new Uri("https://aqms.doe.ir/Service/") })
                     System.Net.ServicePointManager.Expect100Continue = false;

                    using (var client1 = new HttpClient { BaseAddress = new Uri("http://192.168.0.229/") })
                    {
                        statusLog("SendReadCommand_RecoveryData 2222");

                        // client1.DefaultRequestHeaders.Add("Authorization", "aG9yaWJhOnBhc3N3b3Jk");
                        //client1.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "aG9yaWJhOnBhc3N3b3Jk");
                        client1.DefaultRequestHeaders.Add("Authorization", "Basic " + "aG9yaWJhOnBhc3N3b3Jk");
                        client1.DefaultRequestHeaders.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");

                       // client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9"));

                        statusLog("SendReadCommand_RecoveryData 3333");
                        //var body = new List<KeyValuePair<string, string>>
                        //{
                        //    new KeyValuePair<string, string>("grant_type", "password"),
                        //    new KeyValuePair<string, string>("username", "matinrad"),
                        //    new KeyValuePair<string, string>("password", "5560081981")
                        //};

                        var body = new List<KeyValuePair<string, string>>
                            {
                            new KeyValuePair<string, string>("POST_REPORTS", "YES"),
                            new KeyValuePair<string, string>("POST_COMP_01", "on"),
                            new KeyValuePair<string, string>("POST_COMP_02", "on"),
                            new KeyValuePair<string, string>("POST_COMP_03", "on"),
                            new KeyValuePair<string, string>("POST_COMP_LIST", "O3;CO;SO2;NO;NO2;NOx;PM2.5"),
                            new KeyValuePair<string, string>("POST_START_DAY", "22"),
                            new KeyValuePair<string, string>("POST_START_MONTH", "8"),
                            new KeyValuePair<string, string>("POST_START_YEAR", "2023"),
                            new KeyValuePair<string, string>("POST_START_HOUR", "0"),
                            new KeyValuePair<string, string>("POST_TIME_PERIOD", "24"),
                            new KeyValuePair<string, string>("POST_DATA_TYPE", "1"),
                            new KeyValuePair<string, string>("POST_UNIT2", "on"),

                        };

                        statusLog(string.Format("SendReadCommand_RecoveryData 4444 {0}", body[0].ToString()));

                        //client1.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("aG9yaWJhOnBhc3N3b3Jk");
                        statusLog(string.Format("SendReadCommand_RecoveryData 555 {0}", client1.DefaultRequestHeaders.Authorization.ToString()));

                        // httpWebRequest.Headers.Add("Authorization", "Basic aG9yaWJhOnBhc3N3b3Jk");
                        statusLog(string.Format("SendReadCommand_RecoveryData 555555 {0}", body.ToString()));


                        var content = new FormUrlEncodedContent(body);
                        //HttpResponseMessage result = new HttpResponseMessage();
                        try
                        {
                           // var result = client1.PostAsync("v1/login/", content).Result;
                            var result = client1.PostAsync("cgi-bin/cgi-iox?proc=5", content).Result;
                            statusLog(string.Format("SendReadCommand_RecoveryData 6666 {0}", result.Content.ToString()));

                            // var result = client1.PostAsync("?proc=5", content).Result;
                            statusLog(string.Format("SendReadCommand_RecoveryData 6666666 {0}", result.Content.ReadAsStringAsync().Result.ToString()));

                            if (result.IsSuccessStatusCode)
                            {
                                statusLog("SendReadCommand_RecoveryData 7777");

                                var auth_result = result.Content.ReadAsStringAsync().Result;
                                statusLog(string.Format("SendReadCommand_RecoveryData 8888{0}", auth_result.ToString()));

                                var jsonObject = (JObject)JsonConvert.DeserializeObject(auth_result);
                                statusLog(string.Format("SendReadCommand_RecoveryData 9999{0}", jsonObject.ToString()));

                            }
                        }
                        catch (Exception e)
                        {
                            //MessageBox.Show(e.Message);
                            //txtLastUpdate.Text = e.Message;
                        }
                    }

                    //var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.0.229/cgi-bin/cgi-iox?proc=5");
                    //httpWebRequest.ContentType = "application/json";
                    //httpWebRequest.Method = "POST";
                    //httpWebRequest.Headers.Add("Authorization", "aG9yaWJhOnBhc3N3b3Jk");
                    //using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    //{

                    //    string json = "{\"POST_REPORTS\":\"YES\"," +
                    //                  "\"POST_COMP_01\":\"on\"," +
                    //                  "\"POST_COMP_02\":\"on\"," +
                    //                  "\"POST_COMP_03\":\"on\"," +
                    //                  "\"POST_COMP_LIST\":\"O3;CO;SO2;NO;NO2;NOx;PM2.5\"," +
                    //                  "\"POST_START_DAY\":\"22\"," +
                    //                  "\"POST_START_MONTH\":\"8\"," +
                    //                  "\"POST_START_YEAR\":\"2023\"," +
                    //                  "\"POST_START_HOUR\":\"0\"," +
                    //                  "\"POST_TIME_PERIOD\":\"24\"," +
                    //                  "\"POST_DATA_TYPE\":\"1\"," +
                    //                  "\"POST_UNIT2\":\"on\"}";

                    //    streamWriter.Write(json);
                    //    statusLog(string.Format("SendReadCommand_RecoveryData 4444 {0}", json.ToString()));

                    //}
                    //var client = new WebClient();


                    //var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    //{

                    //    var result = streamReader.ReadToEnd();
                    //}

                }
                catch (Exception ex)
                {
                    statusLog("SendReadCommand_RecoveryData Eroorr" + ex.ToString());

                }
                //for (var day = Startdate.Date; day <= EndDate; day = day.AddDays(1))
                //{
                //    flagGetCurrentData = false;
                //    statusLog(string.Format("Startdate.Date:{0} ", day.ToString()));

                //    //if (EndDate.Month != DateTime.Now.Month && EndDate.Day != DateTime.Now.Day)
                //    //{
                //    request = WebRequest.Create("http://" + portCOM + "/cgi-bin/cgi-iox?proc=61&path=iox/database/av1.txt&unit=2&crosstable=y&time=" + day.Year.ToString() + "/" + day.Month.ToString() + "/" + (day.Day != 1 ? (day.Day) - 1 : day.Day).ToString() + "%2000:00&period=24");
                //    statusLog(string.Format("WebRequest:{0}   ", request.RequestUri.AbsoluteUri.ToString()));

                //    request.Headers.Add("Authorization", "Basic " + encoded);
                //    request.Credentials = CredentialCache.DefaultCredentials;
                //    WebResponse response = request.GetResponse();
                //    string ct = response.ContentType;
                //    Stream objStream = response.GetResponseStream();
                //    BinaryReader breader = new BinaryReader(objStream);
                //    byte[] buffer = breader.ReadBytes((int)response.ContentLength);
                //    Stream stream = new MemoryStream(buffer);

                //    //foreach (byte dataByte in buffer)
                //    //    parseByte(dataByte);
                //    statusLog(string.Format("Date Time:{0}   ", day.Day.ToString()));
        }

        internal override void initialize_Connect()
        {
            try
            {
                statusLog("start client for HoribaLan");
                isConnected = true;

                base.initialize_Connect();

            }
            catch (Exception ex)
            {
                errorLog("TCP conncetion problem for device analyzer HoribaLan: " + ex.Message);
            }
        }
        public override void parseByte(byte data)
        {
            timeLastByteRecieved = DateTime.Now;

            //data = Convert.ToByte(data);//this is for test2ToolStripMenuItem_Click
            messageReceived.Add(data);
            if ((data == 10 && messageReceived.Count >= 2 && messageReceived[messageReceived.Count - 2] == 13))//each message have \r\n
            {
                parseMessage();
                messageReceived.Clear();
            }
            if (messageReceived.Count > 500)
                messageReceived.Clear();
        }

        public override void parseMessage()
        {
            try
            {
                timeLastMessageRecieved = DateTime.Now;
                byte[] Check = messageReceived.ToArray();
                // statusLog(string.Format("Check:{0}-{1} ", Check[0].ToString(), Check[1].ToString()));
               // statusLog(string.Format("**(polutions.Count )**  :{0}  ", polutions.Count.ToString()));
                if (Check[0] > 48 && Check[0] < 58)
                {
                    string deviceResponse = Encoding.UTF8.GetString(messageReceived.ToArray());
                    string[] tmpStrData = deviceResponse.Split('\t');
                  //  statusLog(string.Format("* tmpStrData *  :{0}  ", tmpStrData.Length.ToString()));
                    if (tmpStrData.Length == 15)
                    {
                        //for instance data
                       // statusLog(string.Format("  /////for instance data:{0}  ", flagGetCurrentData.ToString()));
                        Dictionary<string, int> Polution_Mapp = new Dictionary<string, int>();
                        Polution_Mapp.Add("PM-10", 8);
                        Polution_Mapp.Add("PM-2.5", 7);
                        Polution_Mapp.Add("PM10", 8);
                        Polution_Mapp.Add("PM2.5", 7);
                        Polution_Mapp.Add("O3", 1);
                        Polution_Mapp.Add("CO", 2);
                        Polution_Mapp.Add("NO", 3);
                        Polution_Mapp.Add("NO2", 4);
                        Polution_Mapp.Add("NOX", 5);
                        Polution_Mapp.Add("SO2", 6);
                        foreach (KeyValuePair<string, int> pair in Polution_Mapp)
                        {
                            if ((tmpStrData[4].ToUpper().Trim()).ToString() == pair.Key)
                            {
                                for (int idx = 0; idx < polutions.Count; idx++)
                                {
                                    if (polutions[idx].ID == pair.Value)
                                    {
                                        polutions[idx].concentration = getFloatNumHoribaLan(tmpStrData[5]);
                                        polutions[idx].alarmValue = getalarm(tmpStrData[13]);
                                    }
                                }

                            }
                        }
                        timeLastMessageRecieved = DateTime.Now;
                        waitForResponseList[1].Set();
                        workingStatus = true;

                    }

                    else
                    {
                        // for Download

                        //statusLog(string.Format("***************  flagGetCurrentData:{0}  ", flagGetCurrentData.ToString()));
                        flagGetCurrentData = true;

                    }
                }

            }

            catch (Exception ex)
            {
                errorLog("[Error parseMessage HoribaLan] - " + ex.Message);
            }
        }
        public void parseDownloadHoriba()
        {
            try
            {
                timeLastMessageRecieved = DateTime.Now;
                byte[] Check = messageReceived.ToArray();
                if (Check[0] > 48 && Check[0] < 58)
                {
                    string deviceResponse = Encoding.UTF8.GetString(messageReceived.ToArray());
                    string[] tmpStrData = deviceResponse.Split('\t');
                    Dictionary<string, int> Polution_Mapp = new Dictionary<string, int>();
                    Polution_Mapp.Add("PM-10", 8);
                    Polution_Mapp.Add("PM-2.5", 7);
                    Polution_Mapp.Add("PM10", 8);
                    Polution_Mapp.Add("PM2.5", 7);
                    Polution_Mapp.Add("O3", 1);
                    Polution_Mapp.Add("CO", 2);
                    Polution_Mapp.Add("NO", 3);
                    Polution_Mapp.Add("NO2", 4);
                    Polution_Mapp.Add("NOX", 5);
                    Polution_Mapp.Add("SO2", 6);
                    foreach (KeyValuePair<string, int> pair in Polution_Mapp)
                    {
                        if ((tmpStrData[4].ToUpper().Trim()).ToString() == pair.Key)
                        {
                            for (int idx = 0; idx < polutions.Count; idx++)
                            {
                                if (polutions[idx].ID == pair.Value)
                                {
                                    polutions[idx].concentration = getFloatNumHoribaLan(tmpStrData[5]);
                                    polutions[idx].alarmValue = getalarm(tmpStrData[12]);
                                }
                            }

                        }
                    }

                }
                timeLastMessageRecieved = DateTime.Now;
                waitForResponseList[1].Set();
                workingStatus = true;
            }

            catch (Exception ex)
            {
                errorLog("[Error parseMessage HoribaLan] - " + ex.Message);
            }
        }
        public int getalarm(string alarm)
        {
            try
            {
                string all = null;
                char[] b = alarm.ToCharArray();
                for (int i = 0; i < b.Length; i++)
                {
                    if (b[i] == '_')
                    {
                        b[i] = '0';
                    }
                    else
                    {
                        b[i] = '1';
                    }
                    all = (all + b[i]);
                }
                return Convert.ToInt32(all, 2);
            }
            catch (Exception ex)
            {
                errorLog("[Error getalarm HoribaLan] - " + ex.Message);
            }
            return -1;
        }
        private float getFloatNumHoribaLan(string numStr)
        {
            try
            {
                float num;

                if (!float.TryParse(numStr, out num))
                    errorLog(string.Format("Device : {0} [parseMessage] can not parse concentration {1} ", deviceName, numStr));

                return num;
            }
            catch (Exception ex)
            {
                errorLog("[Problem Converting in Horiba Lan] for " + numStr + " - " + ex.Message);
            }

            return -10000f;
        }
        internal override void Disconnect()
        {
            if (conSerial.IsOpen)
                conSerial.Close();

            base.Disconnect();

        }

        public override void getHistory(int mode, int pointer, string strDate = "", string endDate = "")
        {
            try
            {
                WebRequest request;
                if (deviceType == DeviceType.HoribaLan)
                {
                    switch (mode)
                    {
                        case 0:
                            stopGetData();
                            break;

                        case 1:
                            break;

                        case 2:
                            break;

                        case 3:
                            DateTime Startdate = DateTime.Parse(strDate);
                            DateTime EndDate = DateTime.Parse(endDate);
                            SendReadCommand_RecoveryData(strDate, endDate);
                            break;

                        case 4:
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                errorLog("[Error getHistory HoribaLan] - " + ex.Message);

            }
        }
    }
}
