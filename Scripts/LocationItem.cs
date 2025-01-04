
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;

namespace Sonic853.Udon.Weather
{
    /// <summary>
    /// 单个地区的天气信息
    /// </summary>
    public class LocationItem : UdonSharpBehaviour
    {
        public UdonWeather udonWeather;
        /// <summary>
        /// 行政区域名称
        /// </summary>
        public string adm1Name;
        /// <summary>
        /// 翻译后的行政区域名称
        /// </summary>
        public string tAdm1Name;
        /// <summary>
        /// 地区名称
        /// </summary>
        public string locationName;
        /// <summary>
        /// 翻译后的地区名称
        /// </summary>
        public string tLocationName;
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime updateTime;
        /// <summary>
        /// 天气链接
        /// </summary>
        public string fxLink;
        /// <summary>
        /// 数据观测时间
        /// </summary>
        public DateTime obsTime;
        /// <summary>
        /// 温度，默认单位：摄氏度
        /// </summary>
        public int temp = -1;
        /// <summary>
        /// 体感温度，默认单位：摄氏度
        /// </summary>
        public int feelsLike = -1;
        /// <summary>
        /// 天气状况的图标代码：https://dev.qweather.com/docs/resource/icons/
        /// </summary>
        public string icon;
        /// <summary>
        /// 天气状况的文字描述，包括阴晴雨雪等天气状态的描述
        /// </summary>
        public string text;
        /// <summary>
        /// 风向360角度
        /// </summary>
        public string wind360;
        /// <summary>
        /// 风向
        /// </summary>
        public string windDir;
        /// <summary>
        /// 风力等级
        /// </summary>
        public string windScale;
        /// <summary>
        /// 风速，公里/小时
        /// </summary>
        public string windSpeed;
        /// <summary>
        /// 相对湿度，百分比数值
        /// </summary>
        public string humidity;
        /// <summary>
        /// 过去1小时降水量，默认单位：毫米
        /// </summary>
        public string precip;
        /// <summary>
        /// 大气压强，默认单位：百帕
        /// </summary>
        public string pressure;
        /// <summary>
        /// 能见度，默认单位：公里
        /// </summary>
        public string vis;
        /// <summary>
        /// 云量，百分比数值。可能为空
        /// </summary>
        public string cloud;
        /// <summary>
        /// 露点温度。可能为空
        /// </summary>
        public string dew;
        /// <summary>
        /// 每日预报
        /// </summary>
        [NonSerialized] public DataList daily;
        /// <summary>
        /// 每小时预报
        /// </summary>
        [NonSerialized] public DataList hourly;
        /// <summary>
        /// 数据源
        /// </summary>
        public string[] source;
        /// <summary>
        /// 开源协议
        /// </summary>
        public string[] license;
        // [SerializeField] DayItem dayItemPrefab;
        // [SerializeField] Transform dayItemsTransform;
        // [SerializeField] DayItem[] dayItems;
        // [SerializeField] HourItem hourItemPrefab;
        // [SerializeField] Transform hourItemsTransform;
        // [SerializeField] HourItem[] hourItems;
        public void UpdateData(string _adm1Name, string _locationName, DataDictionary locationData)
        {
            adm1Name = _adm1Name;
            locationName = _locationName;
            UpdateData(locationData);
        }
        public void UpdateData(DataDictionary locationData)
        {
            // now 是 DataDictionary，daily hourly source 是 DataList，fxLink updateTime 是 String
            if (locationData.TryGetValue("updateTime", out var updateTimeToken) && updateTimeToken.TokenType == TokenType.String)
            {
                DateTime.TryParse(updateTimeToken.String, out updateTime);
            }
            if (locationData.TryGetValue("fxLink", out var fxLinkToken) && fxLinkToken.TokenType == TokenType.String)
            {
                fxLink = fxLinkToken.String;
            }
            if (locationData.TryGetValue("source", out var sourcesToken) && sourcesToken.TokenType == TokenType.DataList)
            {
                var sourceList = sourcesToken.DataList;
                source = new string[sourceList.Count];
                for (var i = 0; i < sourceList.Count; i++)
                {
                    if (!sourceList.TryGetValue(i, out var sourceToken)) { continue; }
                    source[i] = sourceToken.String;
                }
            }
            if (locationData.TryGetValue("license", out var licensesToken) && licensesToken.TokenType == TokenType.DataList)
            {
                var licenseList = licensesToken.DataList;
                license = new string[licenseList.Count];
                for (var i = 0; i < licenseList.Count; i++)
                {
                    if (!licenseList.TryGetValue(i, out var licenseToken)) { continue; }
                    license[i] = licenseToken.String;
                }
            }
            if (locationData.TryGetValue("now", out var nowToken) && nowToken.TokenType == TokenType.DataDictionary)
            {
                var now = nowToken.DataDictionary;
                if (now.TryGetValue("obsTime", out var obsTimeToken) && obsTimeToken.TokenType == TokenType.String)
                {
                    DateTime.TryParse(obsTimeToken.String, out obsTime);
                }
                if (now.TryGetValue("temp", out var tempToken) && tempToken.TokenType == TokenType.String)
                {
                    int.TryParse(tempToken.String, out temp);
                }
                if (now.TryGetValue("feelsLike", out var feelsLikeToken) && feelsLikeToken.TokenType == TokenType.String)
                {
                    int.TryParse(feelsLikeToken.String, out feelsLike);
                }
                if (now.TryGetValue("icon", out var iconToken) && iconToken.TokenType == TokenType.String)
                {
                    icon = iconToken.String;
                }
                if (now.TryGetValue("text", out var textToken) && textToken.TokenType == TokenType.String)
                {
                    text = textToken.String;
                }
                if (now.TryGetValue("wind360", out var wind360Token) && wind360Token.TokenType == TokenType.String)
                {
                    wind360 = wind360Token.String;
                }
                if (now.TryGetValue("windDir", out var windDirToken) && windDirToken.TokenType == TokenType.String)
                {
                    windDir = windDirToken.String;
                }
                if (now.TryGetValue("windScale", out var windScaleToken) && windScaleToken.TokenType == TokenType.String)
                {
                    windScale = windScaleToken.String;
                }
                if (now.TryGetValue("windSpeed", out var windSpeedToken) && windSpeedToken.TokenType == TokenType.String)
                {
                    windSpeed = windSpeedToken.String;
                }
                if (now.TryGetValue("humidity", out var humidityToken) && humidityToken.TokenType == TokenType.String)
                {
                    humidity = humidityToken.String;
                }
                if (now.TryGetValue("precip", out var precipToken) && precipToken.TokenType == TokenType.String)
                {
                    precip = precipToken.String;
                }
                if (now.TryGetValue("pressure", out var pressureToken) && pressureToken.TokenType == TokenType.String)
                {
                    pressure = pressureToken.String;
                }
                if (now.TryGetValue("vis", out var visToken) && visToken.TokenType == TokenType.String)
                {
                    vis = visToken.String;
                }
                if (now.TryGetValue("cloud", out var cloudToken) && cloudToken.TokenType == TokenType.String)
                {
                    cloud = cloudToken.String;
                }
                if (now.TryGetValue("dew", out var dewToken) && dewToken.TokenType == TokenType.String)
                {
                    dew = dewToken.String;
                }
            }
            if (locationData.TryGetValue("daily", out var dailyToken) && dailyToken.TokenType == TokenType.DataList)
            {
                daily = dailyToken.DataList;
                // var dailyCount = daily.Count;
                // for (var i = 0; i < dailyCount; i++)
                // {
                //     if (!daily.TryGetValue(i, out var dayToken) || dayToken.TokenType != TokenType.DataDictionary) { continue; }
                //     var dayData = dayToken.DataDictionary;
                //     var dayItem = (DayItem)Instantiate(dayItemPrefab.gameObject, dayItemsTransform).GetComponent(typeof(UdonBehaviour));
                //     UdonArrayPlus.Add(ref dayItems, dayItem);
                //     dayItem.UpdateData(dayData);
                // }
            }
            if (locationData.TryGetValue("hourly", out var hourlyToken) && hourlyToken.TokenType == TokenType.DataList)
            {
                hourly = hourlyToken.DataList;
                // var hourlyCount = hourly.Count;
                // for (var i = 0; i < hourlyCount; i++)
                // {
                //     if (!hourly.TryGetValue(i, out var hourToken) || hourToken.TokenType != TokenType.DataDictionary) { continue; }
                //     var hourData = hourToken.DataDictionary;
                //     var hourItem = (HourItem)Instantiate(hourItemPrefab.gameObject, hourItemsTransform).GetComponent(typeof(UdonBehaviour));
                //     UdonArrayPlus.Add(ref hourItems, hourItem);
                //     hourItem.UpdateData(hourData);
                // }
            }
        }
        public void LoadUI()
        {
            udonWeather.weatherUI.LoadData(this);
        }
        public void Clear()
        {
            // if (dayItems.Length > 0)
            // {
            //     foreach (var dayItem in dayItems)
            //     {
            //         Destroy(dayItem.gameObject);
            //     }
            //     dayItems = new DayItem[0];
            // }
            // if (hourItems.Length > 0)
            // {
            //     foreach (var hourItem in hourItems)
            //     {
            //         Destroy(hourItem.gameObject);
            //     }
            //     hourItems = new HourItem[0];
            // }
        }
    }
}
