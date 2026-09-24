using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Data;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;
using static SadraAQMS.AnalyzerParam;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SadraAQMS.Analyzers
{
    class Aeroqual : deviceAnalyzer
    {

        public Aeroqual(DeviceType dvType, string devID)
        {
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();

            //_alarmValue = 0;

        }
        public Aeroqual(DeviceType dvType, string devID, string devName, string ignorableErrors)
        {
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            this.DeviceName = devName;

            int.TryParse(ignorableErrors, out IgnorableErrors);


            //_alarmValue = 0;
        }
        public class CookieAwareWebClient : WebClient
        {
            public CookieAwareWebClient()
            {
                CookieContainer = new CookieContainer();
                this.ResponseCookies = new CookieCollection();
            }

            public CookieContainer CookieContainer { get; private set; }
            public CookieCollection ResponseCookies { get; set; }

            protected override WebRequest GetWebRequest(Uri address)
            {
                var request = (HttpWebRequest)base.GetWebRequest(address);
                request.CookieContainer = CookieContainer;
                return request;
            }


        }

        public override void sendReadCommand()
        {
            //try
            //{
            //    string URL = "http://" + portCOM + "/Account/Login";
            //    string userName = this.userName;
            //    string passWord = this.passWord;
            //    CookieAwareWebClient client = new CookieAwareWebClient();
            //    byte[] LoginResponse = client.UploadValues(URL, new NameValueCollection(){
            //            { "Username",userName},//administrator
            //            { "Password", passWord},//aqmaedmin
            //            { "RememberMe", "true" },// true
            //            { "ReturnUrl", "/Home" }//?????
            //        });

            //    string loginresult = System.Text.Encoding.UTF8.GetString(LoginResponse);

            //    if (loginresult.Contains("Home"))
            //    {
            //        URL = "http://" + portCOM + "/ManageData/";
            //        (DateTime.Now.ToString("MM/dd/yyyy")
            //        var address = string.Format("{0}{1}?Period={2}&AvgMinutes=10", URL, "GenerateDataTable", (DateTime.Now.ToString("MM/dd/yyyy") + "+to+" + DateTime.Now.ToString("MM/dd/yyyy")).Replace("/", "%2F"));//("{0}{1}?Period={2}&AvgMinutes=10", URL, "GenerateDataTable","02/13/2023 9:55"+ "+to+" + "02/14/2023 9:55".Replace("/", "%2F"));

            //        client.DownloadData(address);

            //        Task<string> t = HttpGetResponse();  ??????????????????
            //        t.Wait();

            //        string response = t.Result;

            //        byte[] prmByte = client.UploadValues(URL + "DataTable", new NameValueCollection());

            //        var result = System.Text.Encoding.UTF8.GetString(prmByte);

            //        var doc = new HtmlAgilityPack.HtmlDocument();
            //        doc.LoadHtml(result);

            //        var table = doc.DocumentNode.SelectNodes("//table");
            //        try
            //        {var rows = table[0].SelectNodes("tr");
            //            var headercells = rows[0].SelectNodes("th|td");
            //            var cells = rows[1].SelectNodes("td");

            //            for (int i = 0; i < cells.Count; i++)
            //            {
            //                float temp = 0;
            //                تاریخ وزمان(O3(ppb  (SO2(ppb    (NO(ppb     (NO2(ppb    (NOx(ppb    (CO(ppm     (PM10(μg/ m
            //                   Time PM10µg / m³
            //                   2 / 7 / 2023 10:35 AM      0.00
            //                switch (headercells[i].InnerText.ToString())
            //                {
            //                    case string a when a.Contains("Time"):
            //                        timeLastMessageRecieved = DateTime.Parse(cells[i].InnerText);
            //                        break;
            //                    case string a when a.Contains("PM10"):
            //                        if (float.TryParse(cells[i].InnerText, out temp))
            //                            polutions[0].concentration = temp;
            //                        break;
            //                    case string a when a.Contains("PM2.5"):
            //                        if (float.TryParse(cells[i].InnerText, out temp))
            //                            polutions[1].concentration = temp;
            //                        break;

            //                    case string a when a.Contains("SO2"):
            //                        if (float.TryParse(cells[i].InnerText, out temp))
            //                            polutions[7].concentration = temp;
            //                        break;
            //                    case string a when a.Contains("CO"):
            //                        if (float.TryParse(cells[i].InnerText, out temp))
            //                            polutions[3].concentration = temp;
            //                        break;
            //                    case string a when a.Contains("NOx"):
            //                        if (float.TryParse(cells[i].InnerText, out temp))
            //                            polutions[6].concentration = temp;
            //                        break;
            //                    case string a when a.Contains("NO2"):
            //                        if (float.TryParse(cells[i].InnerText, out temp))
            //                            polutions[5].concentration = temp;
            //                        break;
            //                    case string a when a.Contains("NO"):
            //                        if (float.TryParse(cells[i].InnerText, out temp))
            //                            polutions[4].concentration = temp;
            //                        break;
            //                    case string a when a.Contains("O3"):
            //                        if (float.TryParse(cells[i].InnerText, out temp))
            //                            polutions[2].concentration = temp;
            //                        break;
            //                }
            //                timeLastMessageRecieved = DateTime.Now;
            //                waitForResponseList[1].Set();
            //                workingStatus = true;
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            statusLog("Error occurred on FetchStationType8 inner:" + ex.Message);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    errorLog("sendReadCommand problem for device analyzer Aeroqual: " + ex.Message);
            //}

            try
            {
                string URL = "http://" + portCOM + "/Account/Login";
                string userName = this.userName;
                string passWord = this.passWord;
                CookieAwareWebClient client = new CookieAwareWebClient();
                byte[] LoginResponse = client.UploadValues(URL, new NameValueCollection(){
                        { "Username",userName},//administrator
                        { "Password", passWord},//aqmaedmin
                        { "RememberMe", "true" },// true
                        { "ReturnUrl", "/Home" }//?????
                    });
                string loginresult = System.Text.Encoding.UTF8.GetString(LoginResponse);

                if (loginresult.Contains("Home"))
                {
                    //192.168.1.99/ManageData/GraphData/PM10?Period=02/28/2023%20to%2002/28/2023&AvgMinutes=05
                    URL = "http://" + portCOM + "/ManageData/GraphData/PM10";
                    var address = string.Format("{0}?Period={1}&AvgMinutes=05", URL, (DateTime.Now.ToString("MM/dd/yyyy") + "+to+" + DateTime.Now.ToString("MM/dd/yyyy")).Replace("/", "%2F"));//("{0}{1}?Period={2}&AvgMinutes=10", URL, "GenerateDataTable","02/13/2023 9:55"+ "+to+" + "02/14/2023 9:55".Replace("/", "%2F"));
                    
                    try
                    {
                        string json = client.DownloadString(address);
                        var jsonObject = (JObject)JsonConvert.DeserializeObject(json);
                        if (jsonObject["Data"] != null && jsonObject["Data"][0] != null && jsonObject["Data"][0]["data"] != null)
                        {
                            var datajason = jsonObject["Data"][0]["data"].ToArray();
                            var lastdata = datajason.Last();
                            long date = Convert.ToInt64(lastdata[0]);
                            var value = Convert.ToSingle(lastdata[1]);
                            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                            dateTime = dateTime.AddMilliseconds(date).ToLocalTime();
                            if (dateTime != null && value != null)
                            {
                                timeLastMessageRecieved = dateTime;
                                polutions[0].concentration = value;
                                waitForResponseList[1].Set();
                                workingStatus = true;
                            }
                            else
                            {
                                workingStatus = false;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        errorLog("sendReadCommand problem for device analyzer Aeroqual: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                errorLog("sendReadCommand problem for device analyzer Aeroqual: " + ex.Message);
            }
        }

        static async Task<string> HttpGetResponse()
        {
            WebRequest request = WebRequest.Create("some_url");
            request.Headers.Add("cookie", "some_cookie");
            string responseData;
            Stream objStream = request.GetResponse().GetResponseStream();
            StreamReader objReader = new StreamReader(objStream);
            string sLine = "";
            int i = 0;
            while (sLine != null)
            {
                i++;
                sLine = objReader.ReadLine();
                if (sLine != null)
                    Console.WriteLine(sLine);
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("cookie", "some_cookie");
                using (var response = await client.GetAsync("some_url"))
                {
                    responseData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseData);
                }
            }

            return responseData;
        }

        internal override void initialize_Connect()
        {
            try
            {
                statusLog("start client for Aeroqual");
                isConnected = true;

                base.initialize_Connect();

            }
            catch (Exception ex)
            {
                errorLog("TCP conncetion problem for device analyzer Aeroqual: " + ex.Message);
            }
        }

        //public override void parseByte(byte data)
        //{
        //    timeLastByteRecieved = DateTime.Now;

        //    //data = Convert.ToByte(data);//this is for test2ToolStripMenuItem_Click
        //    messageReceived.Add(data);
        //    if ((data == 10 && messageReceived.Count >= 2 && messageReceived[messageReceived.Count - 2] == 13))//each message have \r\n
        //    {
        //        parseMessage();
        //        messageReceived.Clear();
        //    }
        //    if (messageReceived.Count > 500)
        //        messageReceived.Clear();
        //}

        //public override void parseMessage()
        //{
        //    try
        //    {
        //        timeLastMessageRecieved = DateTime.Now;
        //        byte[] Check = messageReceived.ToArray();
        //        if (Check[0] > 48 && Check[0] < 58)
        //        {
        //            string deviceResponse = Encoding.UTF8.GetString(messageReceived.ToArray());
        //            string[] tmpStrData = deviceResponse.Split('\t');
        //            Dictionary<string, int> Polution_Mapp = new Dictionary<string, int>();
        //            Polution_Mapp.Add("PM-10", 8);
        //            Polution_Mapp.Add("PM-2.5", 7);
        //            Polution_Mapp.Add("O3", 1);
        //            Polution_Mapp.Add("CO", 2);
        //            Polution_Mapp.Add("NO", 3);
        //            Polution_Mapp.Add("NO2", 4);
        //            Polution_Mapp.Add("NOX", 5);
        //            Polution_Mapp.Add("SO2", 6);
        //            foreach (KeyValuePair<string, int> pair in Polution_Mapp)
        //            {
        //                if ((tmpStrData[4].ToUpper().Trim()).ToString() == pair.Key)
        //                {
        //                    for (int idx = 0; idx < polutions.Count; idx++)
        //                    {
        //                        if (polutions[idx].ID == pair.Value)
        //                        {
        //                            polutions[idx].concentration = getFloatNumAeroqual(tmpStrData[5]);
        //                            polutions[idx].alarmValue = getalarm(tmpStrData[13]);
        //                        }
        //                    }

        //                }
        //            }

        //        }
        //        timeLastMessageRecieved = DateTime.Now;
        //        waitForResponseList[1].Set();
        //        workingStatus = true;
        //    }

        //    catch (Exception ex)
        //    {
        //        errorLog("[Error parseMessage Aeroqual] - " + ex.Message);
        //    }
        //}
        //public int getalarm(string alarm)
        //{
        //    try
        //    {
        //        string all = null;
        //        char[] b = alarm.ToCharArray();
        //        for (int i = 0; i < b.Length; i++)
        //        {
        //            if (b[i] == '_')
        //            {
        //                b[i] = '0';
        //            }
        //            else
        //            {
        //                b[i] = '1';
        //            }
        //            all = (all + b[i]);
        //        }
        //        return Convert.ToInt32(all, 2);
        //    }
        //    catch (Exception ex)
        //    {
        //        errorLog("[Error getalarm Aeroqual] - " + ex.Message);
        //    }
        //    return -1;
        //}
        //private float getFloatNumAeroqual(string numStr)
        //{
        //    try
        //    {
        //        float num;

        //        if (!float.TryParse(numStr, out num))
        //            errorLog(string.Format("Device : {0} [parseMessage] can not parse concentration {1} ", deviceName, numStr));

        //        return num;
        //    }
        //    catch (Exception ex)
        //    {
        //        errorLog("[Problem Converting in Aeroqual] for " + numStr + " - " + ex.Message);
        //    }

        //    return -10000f;
        //}

        internal override void Disconnect()
        {
            if (conSerial.IsOpen)
                conSerial.Close();

            base.Disconnect();

        }
    }
}
