
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;

namespace Sonic853.Udon.Weather
{
    /// <summary>
    /// 预报天气（弃用，过于消耗性能）
    /// </summary>
    public class DayItem : UdonSharpBehaviour
    {
        /// <summary>
        /// 预报日期
        /// </summary>
        public DateTime fxDate;
        /// <summary>
        /// 日出时间
        /// </summary>
        public string sunrise;
        /// <summary>
        /// 日落时间
        /// </summary>
        public string sunset;
        /// <summary>
        /// 月升时间
        /// </summary>
        public string moonrise;
        /// <summary>
        /// 月落时间
        /// </summary>
        public string moonset;
        /// <summary>
        /// 月相
        /// </summary>
        public string moonPhase;
        /// <summary>
        /// 月相图标代码：https://dev.qweather.com/docs/resource/icons/
        /// </summary>
        public string moonPhaseIcon;
        /// <summary>
        /// 最高温度
        /// </summary>
        public int tempMax = -1;
        /// <summary>
        /// 最低温度
        /// </summary>
        public int tempMin = -1;
        /// <summary>
        /// 预报白天天气状况的图标代码：https://dev.qweather.com/docs/resource/icons/
        /// </summary>
        public string iconDay;
        /// <summary>
        /// 白天天气
        /// </summary>
        public string textDay;
        /// <summary>
        /// 预报夜间天气状况的图标代码：https://dev.qweather.com/docs/resource/icons/
        /// </summary>
        public string iconNight;
        /// <summary>
        /// 夜间天气
        /// </summary>
        public string textNight;
        /// <summary>
        /// 预报白天风向360角度
        /// </summary>
        public string wind360Day;
        /// <summary>
        /// 预报白天风向
        /// </summary>
        public string windDirDay;
        /// <summary>
        /// 预报白天风力等级
        /// </summary>
        public string windScaleDay;
        /// <summary>
        /// 预报白天风速，公里/小时
        /// </summary>
        public string windSpeedDay;
        /// <summary>
        /// 预报夜间风向360角度
        /// </summary>
        public string wind360Night;
        /// <summary>
        /// 预报夜间当天风向
        /// </summary>
        public string windDirNight;
        /// <summary>
        /// 预报夜间风力等级
        /// </summary>
        public string windScaleNight;
        /// <summary>
        /// 预报夜间风速，公里/小时
        /// </summary>
        public string windSpeedNight;
        /// <summary>
        /// 相对湿度，百分比数值
        /// </summary>
        public string humidity;
        /// <summary>
        /// 逐小时预报降水概率，百分比数值，可能为空
        /// </summary>
        public string pop;
        /// <summary>
        /// 预报当天总降水量，默认单位：毫米
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
        /// 紫外线强度指数
        /// </summary>
        public string uvIndex;
        public void UpdateData(DataDictionary dayData)
        {
            if (dayData.TryGetValue("fxDate", out var fxDateToken) && fxDateToken.TokenType == TokenType.String)
            {
                DateTime.TryParse(fxDateToken.String, out fxDate);
            }
            if (dayData.TryGetValue("sunrise", out var sunriseToken) && sunriseToken.TokenType == TokenType.String)
            {
                sunrise = sunriseToken.String;
            }
            if (dayData.TryGetValue("sunset", out var sunsetToken) && sunsetToken.TokenType == TokenType.String)
            {
                sunset = sunsetToken.String;
            }
            if (dayData.TryGetValue("moonrise", out var moonriseToken) && moonriseToken.TokenType == TokenType.String)
            {
                moonrise = moonriseToken.String;
            }
            if (dayData.TryGetValue("moonset", out var moonsetToken) && moonsetToken.TokenType == TokenType.String)
            {
                moonset = moonsetToken.String;
            }
            if (dayData.TryGetValue("moonPhase", out var moonPhaseToken) && moonPhaseToken.TokenType == TokenType.String)
            {
                moonPhase = moonPhaseToken.String;
            }
            if (dayData.TryGetValue("moonPhaseIcon", out var moonPhaseIconToken) && moonPhaseIconToken.TokenType == TokenType.String)
            {
                moonPhaseIcon = moonPhaseIconToken.String;
            }
            if (dayData.TryGetValue("tempMax", out var tempMaxToken) && tempMaxToken.TokenType == TokenType.String)
            {
                int.TryParse(tempMaxToken.String, out tempMax);
            }
            if (dayData.TryGetValue("tempMin", out var tempMinToken) && tempMinToken.TokenType == TokenType.String)
            {
                int.TryParse(tempMinToken.String, out tempMin);
            }
            if (dayData.TryGetValue("iconDay", out var iconDayToken) && iconDayToken.TokenType == TokenType.String)
            {
                iconDay = iconDayToken.String;
            }
            if (dayData.TryGetValue("textDay", out var textDayToken) && textDayToken.TokenType == TokenType.String)
            {
                textDay = textDayToken.String;
            }
            if (dayData.TryGetValue("iconNight", out var iconNightToken) && iconNightToken.TokenType == TokenType.String)
            {
                iconNight = iconNightToken.String;
            }
            if (dayData.TryGetValue("textNight", out var textNightToken) && textNightToken.TokenType == TokenType.String)
            {
                textNight = textNightToken.String;
            }
            if (dayData.TryGetValue("wind360Day", out var wind360DayToken) && wind360DayToken.TokenType == TokenType.String)
            {
                wind360Day = wind360DayToken.String;
            }
            if (dayData.TryGetValue("windDirDay", out var windDirDayToken) && windDirDayToken.TokenType == TokenType.String)
            {
                windDirDay = windDirDayToken.String;
            }
            if (dayData.TryGetValue("windScaleDay", out var windScaleDayToken) && windScaleDayToken.TokenType == TokenType.String)
            {
                windScaleDay = windScaleDayToken.String;
            }
            if (dayData.TryGetValue("windSpeedDay", out var windSpeedDayToken) && windSpeedDayToken.TokenType == TokenType.String)
            {
                windSpeedDay = windSpeedDayToken.String;
            }
            if (dayData.TryGetValue("wind360Night", out var wind360NightToken) && wind360NightToken.TokenType == TokenType.String)
            {
                wind360Night = wind360NightToken.String;
            }
            if (dayData.TryGetValue("windDirNight", out var windDirNightToken) && windDirNightToken.TokenType == TokenType.String)
            {
                windDirNight = windDirNightToken.String;
            }
            if (dayData.TryGetValue("windScaleNight", out var windScaleNightToken) && windScaleNightToken.TokenType == TokenType.String)
            {
                windScaleNight = windScaleNightToken.String;
            }
            if (dayData.TryGetValue("windSpeedNight", out var windSpeedNightToken) && windSpeedNightToken.TokenType == TokenType.String)
            {
                windSpeedNight = windSpeedNightToken.String;
            }
            if (dayData.TryGetValue("humidity", out var humidityToken) && humidityToken.TokenType == TokenType.String)
            {
                humidity = humidityToken.String;
            }
            if (dayData.TryGetValue("pop", out var popToken) && popToken.TokenType == TokenType.String)
            {
                pop = popToken.String;
            }
            if (dayData.TryGetValue("precip", out var precipToken) && precipToken.TokenType == TokenType.String)
            {
                precip = precipToken.String;
            }
            if (dayData.TryGetValue("pressure", out var pressureToken) && pressureToken.TokenType == TokenType.String)
            {
                pressure = pressureToken.String;
            }
            if (dayData.TryGetValue("vis", out var visToken) && visToken.TokenType == TokenType.String)
            {
                vis = visToken.String;
            }
            if (dayData.TryGetValue("cloud", out var cloudToken) && cloudToken.TokenType == TokenType.String)
            {
                cloud = cloudToken.String;
            }
            if (dayData.TryGetValue("uvIndex", out var uvIndexToken) && uvIndexToken.TokenType == TokenType.String)
            {
                uvIndex = uvIndexToken.String;
            }
        }
    }
}
