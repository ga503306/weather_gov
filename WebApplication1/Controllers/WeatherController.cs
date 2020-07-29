using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class WeatherController : Controller
    {
        // GET: Weather
        public ActionResult Index()
        {

            List<Weather> Weathers = new List<Weather>();
            int index = 0;
            string state = ""; //英文描述
            string state_chs = ""; //中文描述
            string locationName = ""; //中文地名
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("https://opendata.cwb.gov.tw/fileapi/v1/opendataapi/F-D0047-065?Authorization=CWB-BC2E6A08-130F-4DCA-B26C-8E5ADF09F133&downloadType=WEB&format=XML");
            XmlNamespaceManager xnm = new XmlNamespaceManager(xmlDoc.NameTable);
            xnm.AddNamespace("pf", "urn:cwb:gov:tw:cwbcommon:0.1");
            XmlNode xn = xmlDoc.SelectSingleNode("//pf:cwbopendata/pf:dataset/pf:locations", xnm);
            XmlNodeList xnlA = xn.ChildNodes;
            foreach (XmlNode xnA in xnlA)
            {
                XmlElement xeB = (XmlElement)xnA;
                XmlNodeList xnlB = xeB.ChildNodes;
                List<Weather_detail> Weather_details = new List<Weather_detail>();
                foreach (XmlNode xnB in xnlB)
                {
                    if (xnB.Name == "locationName")
                    {
                        XmlElement xeC = (XmlElement)xnB;
                        XmlNodeList xnlC = xeC.ChildNodes;
                        locationName = xnlC[0].InnerText;//地名暫存
                    }
                    if (xnB.Name == "weatherElement")
                    {
                        XmlElement xeC = (XmlElement)xnB;
                        XmlNodeList xnlC = xeC.ChildNodes;
                        foreach (XmlNode xnC in xnlC)
                        {
                            if (xnC.Name == "elementName")
                            {
                                state = xnC.InnerText;
                            }
                            else if (xnC.Name == "description")
                            {
                                state_chs = xnC.InnerText;
                            }
                            else
                            {

                                if (xnC.Name == "time" && state == "T")
                                {
                                    XmlElement xeD = (XmlElement)xnC;
                                    XmlNodeList xnlD = xeD.ChildNodes;
                                    Weather_details.Add(new Weather_detail { 時間 = xnlD[0].InnerText, 溫度 = xnlD[1].InnerText });
                                }
                                if (xnC.Name == "time" && state == "Td")
                                {
                                    XmlElement xeD = (XmlElement)xnC;
                                    XmlNodeList xnlD = xeD.ChildNodes;
                                    index = Weather_details.FindIndex(x => (x.時間 == xnlD[0].InnerText));
                                    Weather_details[index].露點溫度 = xnlD[1].InnerText;
                                }
                                if (xnC.Name == "time" && state == "RH")
                                {
                                    XmlElement xeD = (XmlElement)xnC;
                                    XmlNodeList xnlD = xeD.ChildNodes;
                                    index = Weather_details.FindIndex(x => (x.時間 == xnlD[0].InnerText));
                                    Weather_details[index].相對溼度 = xnlD[1].InnerText;
                                }
                                if (xnC.Name == "time" && state == "PoP6h")
                                {
                                    XmlElement xeD = (XmlElement)xnC;
                                    XmlNodeList xnlD = xeD.ChildNodes;
                                    index = Weather_details.FindIndex(x => (x.時間 == xnlD[0].InnerText));
                                    Weather_details[index].六小時降雨機率 = xnlD[2].InnerText;
                                }
                                if (xnC.Name == "time" && state == "PoP12h")
                                {
                                    XmlElement xeD = (XmlElement)xnC;
                                    XmlNodeList xnlD = xeD.ChildNodes;
                                    index = Weather_details.FindIndex(x => (x.時間 == xnlD[0].InnerText));
                                    Weather_details[index].十二小時降雨機率 = xnlD[2].InnerText;
                                }
                                if (xnC.Name == "time" && state == "WD")
                                {
                                    XmlElement xeD = (XmlElement)xnC;
                                    XmlNodeList xnlD = xeD.ChildNodes;
                                    index = Weather_details.FindIndex(x => (x.時間 == xnlD[0].InnerText));
                                    Weather_details[index].風向 = xnlD[1].InnerText;
                                }
                                if (xnC.Name == "time" && state == "WS")
                                {
                                    XmlElement xeD = (XmlElement)xnC;
                                    XmlNodeList xnlD = xeD.ChildNodes;
                                    index = Weather_details.FindIndex(x => (x.時間 == xnlD[0].InnerText));
                                    Weather_details[index].風速 = xnlD[1].InnerText;
                                }
                                if (xnC.Name == "time" && state == "CI")
                                {
                                    XmlElement xeD = (XmlElement)xnC;
                                    XmlNodeList xnlD = xeD.ChildNodes;
                                    index = Weather_details.FindIndex(x => (x.時間 == xnlD[0].InnerText));
                                    Weather_details[index].舒適度指數 = xnlD[1].ChildNodes[0].InnerText + xnlD[2].ChildNodes[0].InnerText;
                                }
                                if (xnC.Name == "time" && state == "AT")
                                {
                                    XmlElement xeD = (XmlElement)xnC;
                                    XmlNodeList xnlD = xeD.ChildNodes;
                                    index = Weather_details.FindIndex(x => (x.時間 == xnlD[0].InnerText));
                                    Weather_details[index].體感溫度 = xnlD[1].InnerText;
                                }
                                if (xnC.Name == "time" && state == "Wx")
                                {
                                    XmlElement xeD = (XmlElement)xnC;
                                    XmlNodeList xnlD = xeD.ChildNodes;
                                    index = Weather_details.FindIndex(x => (x.時間 == xnlD[0].InnerText));
                                    //Weather_details[index].天氣現象 = xnlD[2].ChildNodes[0].InnerText;//中文
                                    Weather_details[index].天氣現象 = "https://www.cwb.gov.tw/V8/assets/img/weather_icons/weathers/svg_icon/day/" + xnlD[3].ChildNodes[0].InnerText +".svg";//圖
                                }
                                if (xnC.Name == "time" && state == "WeatherDescription")
                                {
                                    XmlElement xeD = (XmlElement)xnC;
                                    XmlNodeList xnlD = xeD.ChildNodes;
                                    index = Weather_details.FindIndex(x => (x.時間 == xnlD[0].InnerText));
                                    Weather_details[index].天氣預報綜合描述 = xnlD[2].ChildNodes[0].InnerText;
                                }
                            }
                        }

                    }

                }
                Weathers.Add(new Weather { 地點 = locationName, Weather_detail = Weather_details });
                //Weather_details.Clear();
            }
            Weathers.RemoveAt(0);
            ViewData["weathers"] = Weathers;
            ViewBag.weather = Weathers;
            return View();
        }

        public ActionResult Index_week()
        {

            List<Weatherweek> Weathers = new List<Weatherweek>();
            int index = 0;
            string state = ""; //英文描述
            string state_chs = ""; //中文描述
            string locationName = ""; //中文地名
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("https://opendata.cwb.gov.tw/fileapi/v1/opendataapi/F-D0047-067?Authorization=CWB-BC2E6A08-130F-4DCA-B26C-8E5ADF09F133&downloadType=WEB&format=XML");
            XmlNamespaceManager xnm = new XmlNamespaceManager(xmlDoc.NameTable);
            xnm.AddNamespace("pf", "urn:cwb:gov:tw:cwbcommon:0.1");
            XmlNode xn = xmlDoc.SelectSingleNode("//pf:cwbopendata/pf:dataset/pf:locations", xnm);
            XmlNodeList xnlA = xn.ChildNodes;
            foreach (XmlNode xnA in xnlA)
            {
                XmlElement xeB = (XmlElement)xnA;
                XmlNodeList xnlB = xeB.ChildNodes;
                List<Weatherweek_detail> Weather_details = new List<Weatherweek_detail>();
                foreach (XmlNode xnB in xnlB)
                {
                    if (xnB.Name == "locationName")
                    {
                        XmlElement xeC = (XmlElement)xnB;
                        XmlNodeList xnlC = xeC.ChildNodes;
                        locationName = xnlC[0].InnerText;//地名暫存
                    }
                    if (xnB.Name == "weatherElement")
                    {
                        XmlElement xeC = (XmlElement)xnB;
                        XmlNodeList xnlC = xeC.ChildNodes;
                        foreach (XmlNode xnC in xnlC)
                        {
                            if (xnC.Name == "elementName")
                            {
                                state = xnC.InnerText;
                            }
                            else if (xnC.Name == "description")
                            {
                                state_chs = xnC.InnerText;
                            }
                            else
                            {

                                if (xnC.Name == "time" && state == "T")
                                {
                                    XmlElement xeD = (XmlElement)xnC;
                                    XmlNodeList xnlD = xeD.ChildNodes;
                                    Weather_details.Add(new Weatherweek_detail { 時間 = xnlD[0].InnerText, 時間結 = xnlD[1].InnerText, 平均溫度 = xnlD[2].InnerText });
                                }
                                if (xnC.Name == "time" && state == "Td")
                                {
                                    XmlElement xeD = (XmlElement)xnC;
                                    XmlNodeList xnlD = xeD.ChildNodes;
                                    index = Weather_details.FindIndex(x => (x.時間 == xnlD[0].InnerText));
                                    Weather_details[index].平均露點溫度 = xnlD[2].InnerText;
                                }
                                if (xnC.Name == "time" && state == "RH")
                                {
                                    XmlElement xeD = (XmlElement)xnC;
                                    XmlNodeList xnlD = xeD.ChildNodes;
                                    index = Weather_details.FindIndex(x => (x.時間 == xnlD[0].InnerText));
                                    Weather_details[index].平均相對濕度 = xnlD[2].InnerText;
                                }
                                if (xnC.Name == "time" && state == "MaxT")
                                {
                                    XmlElement xeD = (XmlElement)xnC;
                                    XmlNodeList xnlD = xeD.ChildNodes;
                                    index = Weather_details.FindIndex(x => (x.時間 == xnlD[0].InnerText));
                                    Weather_details[index].最高溫度 = xnlD[2].InnerText;
                                }
                                if (xnC.Name == "time" && state == "MinT")
                                {
                                    XmlElement xeD = (XmlElement)xnC;
                                    XmlNodeList xnlD = xeD.ChildNodes;
                                    index = Weather_details.FindIndex(x => (x.時間 == xnlD[0].InnerText));
                                    Weather_details[index].最低溫度 = xnlD[2].InnerText;
                                }
                                if (xnC.Name == "time" && state == "MaxAT")
                                {
                                    XmlElement xeD = (XmlElement)xnC;
                                    XmlNodeList xnlD = xeD.ChildNodes;
                                    index = Weather_details.FindIndex(x => (x.時間 == xnlD[0].InnerText));
                                    Weather_details[index].最高體感溫度 = xnlD[2].InnerText;
                                }
                                if (xnC.Name == "time" && state == "MinAT")
                                {
                                    XmlElement xeD = (XmlElement)xnC;
                                    XmlNodeList xnlD = xeD.ChildNodes;
                                    index = Weather_details.FindIndex(x => (x.時間 == xnlD[0].InnerText));
                                    Weather_details[index].最低體感溫度 = xnlD[2].InnerText;
                                }
                                if (xnC.Name == "time" && state == "MaxCI")
                                {
                                    XmlElement xeD = (XmlElement)xnC;
                                    XmlNodeList xnlD = xeD.ChildNodes;
                                    index = Weather_details.FindIndex(x => (x.時間 == xnlD[0].InnerText));
                                    Weather_details[index].最大舒適度指數 = xnlD[2].ChildNodes[0].InnerText;
                                }
                                if (xnC.Name == "time" && state == "MinCI")
                                {
                                    XmlElement xeD = (XmlElement)xnC;
                                    XmlNodeList xnlD = xeD.ChildNodes;
                                    index = Weather_details.FindIndex(x => (x.時間 == xnlD[0].InnerText));
                                    Weather_details[index].最小舒適度指數 = xnlD[2].ChildNodes[0].InnerText;
                                }
                                if (xnC.Name == "time" && state == "PoP12h")
                                {
                                    XmlElement xeD = (XmlElement)xnC;
                                    XmlNodeList xnlD = xeD.ChildNodes;
                                    index = Weather_details.FindIndex(x => (x.時間 == xnlD[0].InnerText));
                                    Weather_details[index].十二小時降雨機率 = xnlD[2].InnerText;
                                }
                                if (xnC.Name == "time" && state == "WD")
                                {
                                    XmlElement xeD = (XmlElement)xnC;
                                    XmlNodeList xnlD = xeD.ChildNodes;
                                    index = Weather_details.FindIndex(x => (x.時間 == xnlD[0].InnerText));
                                    Weather_details[index].風向 = xnlD[2].InnerText;
                                }
                                if (xnC.Name == "time" && state == "WS")
                                {
                                    XmlElement xeD = (XmlElement)xnC;
                                    XmlNodeList xnlD = xeD.ChildNodes;
                                    index = Weather_details.FindIndex(x => (x.時間 == xnlD[0].InnerText));
                                    Weather_details[index].最大風速 = xnlD[2].InnerText;
                                }
                                if (xnC.Name == "time" && state == "Wx")
                                {
                                    XmlElement xeD = (XmlElement)xnC;
                                    XmlNodeList xnlD = xeD.ChildNodes;
                                    index = Weather_details.FindIndex(x => (x.時間 == xnlD[0].InnerText));
                                    Weather_details[index].天氣現象 = xnlD[2].ChildNodes[0].InnerText;
                                }
                                if (xnC.Name == "time" && state == "UVI")
                                {
                                    XmlElement xeD = (XmlElement)xnC;
                                    XmlNodeList xnlD = xeD.ChildNodes;
                                    index = Weather_details.FindIndex(x => (x.時間 == xnlD[0].InnerText));
                                    Weather_details[index].紫外線指數 = xnlD[2].ChildNodes[0].InnerText;
                                }
                                if (xnC.Name == "time" && state == "WeatherDescription")
                                {
                                    XmlElement xeD = (XmlElement)xnC;
                                    XmlNodeList xnlD = xeD.ChildNodes;
                                    index = Weather_details.FindIndex(x => (x.時間 == xnlD[0].InnerText));
                                    Weather_details[index].天氣預報綜合描述 = xnlD[2].ChildNodes[0].InnerText;
                                }
                            }
                        }

                    }

                }
                Weathers.Add(new Weatherweek { 地點 = locationName, Weatherweek_detail = Weather_details });
                //Weather_details.Clear();
            }
            Weathers.RemoveAt(0);
            ViewBag.weather = Weathers;
            return View();
        }

        public ActionResult Index_json()
        {
            List<Weather> Weathers = new List<Weather>();
            int index = 0;
            JArray jsondata = DB_function.getjson("https://opendata.cwb.gov.tw/fileapi/v1/opendataapi/F-D0047-065?Authorization=CWB-BC2E6A08-130F-4DCA-B26C-8E5ADF09F133&downloadType=WEB&format=JSON");
            foreach (JObject data in jsondata)//38地區
            {
                List<Weather_detail> weather_details = new List<Weather_detail>();
                string loactionname = (string)data["locationName"]; //地名
                foreach (JObject data_type in data["weatherElement"])//11種資訊
                {
                    if (data_type["elementName"].ToString() == "T")
                    {
                        foreach (JObject data_detail in data_type["time"])//24時間
                        {
                            string 時間 = Convert.ToDateTime(data_detail["dataTime"]).ToString("yyyy/MM/dd HH:mm:ss");
                            string 溫度 = (string)data_detail["elementValue"]["value"] + (string)data_detail["elementValue"]["measures"];
                            weather_details.Add(new Weather_detail { 時間 = 時間, 溫度 = 溫度 });
                        }
                    }
                    else if (data_type["elementName"].ToString() == "Td")
                    {
                        foreach (JObject data_detail in data_type["time"])//24時間
                        {
                            index = weather_details.FindIndex(x => (x.時間 == Convert.ToDateTime(data_detail["dataTime"]).ToString("yyyy/MM/dd HH:mm:ss")));
                            weather_details[index].露點溫度 = (string)data_detail["elementValue"]["value"] + (string)data_detail["elementValue"]["measures"];
                        }
                    }
                    //以下省
                }
                Weathers.Add(new Weather { 地點 = loactionname, Weather_detail = weather_details });
                ViewBag.weather = Weathers;
            }
            return View();
        }

        // GET: Weather/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Weather/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Weather/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Weather/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Weather/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Weather/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Weather/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
