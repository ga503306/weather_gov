using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Weather
    {
        public string 地點 { get; set; }
        public List<Weather_detail> Weather_detail { get; set; }

    }

    public class Weather_detail
    {
        public string 時間 { get; set; }
        public string 溫度 { get; set; }
        public string 露點溫度 { get; set; }
        public string 相對溼度 { get; set; }
        public string 六小時降雨機率 { get; set; }
        public string 十二小時降雨機率 { get; set; }
        public string 風向 { get; set; }
        public string 風速 { get; set; }
        public string 舒適度指數 { get; set; }
        public string 體感溫度 { get; set; }
        public string 天氣現象 { get; set; }
        public string 天氣預報綜合描述 { get; set; }
    }
}