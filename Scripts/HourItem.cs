
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;

namespace Sonic853.Udon.Weather
{
    public class HourItem : UdonSharpBehaviour
    {
        /// <summary>
        /// 预报时间
        /// </summary>
        public DateTime fxTime;
        /// <summary>
        /// 温度，默认单位：摄氏度
        /// </summary>
        public int temp = -1;
        /// <summary>
        /// 天气状况的图标代码
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
        /// 风速，公里
        /// </summary>
        public string windSpeed;
        /// <summary>
        /// 相对湿度，百分比数值
        /// </summary>
        public string humidity;
        /// <summary>
        /// 逐小时预报降水概率，百分比数值，可能为空
        /// </summary>
        public string pop;
        /// <summary>
        /// 当前小时累计降水量，默认单位：毫米
        /// </summary>
        public string precip;
        /// <summary>
        /// 大气压强，默认单位：百帕
        /// </summary>
        public string pressure;
        /// <summary>
        /// 云量，百分比数值。可能为空
        /// </summary>
        public string cloud;
        /// <summary>
        /// 露点温度。可能为空
        /// </summary>
        public string dew;
        public void UpdateData(DataDictionary hourData)
        {
            if (hourData.TryGetValue("fxTime", out var fxTimeToken) && fxTimeToken.TokenType == TokenType.String)
            {
                DateTime.TryParse(fxTimeToken.String, out fxTime);
            }
            if (hourData.TryGetValue("temp", out var tempToken) && tempToken.TokenType == TokenType.String)
            {
                int.TryParse(tempToken.String, out temp);
            }
            if (hourData.TryGetValue("icon", out var iconToken) && iconToken.TokenType == TokenType.String)
            {
                icon = iconToken.String;
            }
            if (hourData.TryGetValue("text", out var textToken) && textToken.TokenType == TokenType.String)
            {
                text = textToken.String;
            }
            if (hourData.TryGetValue("wind360", out var wind360Token) && wind360Token.TokenType == TokenType.String)
            {
                wind360 = wind360Token.String;
            }
            if (hourData.TryGetValue("windDir", out var windDirToken) && windDirToken.TokenType == TokenType.String)
            {
                windDir = windDirToken.String;
            }
            if (hourData.TryGetValue("windScale", out var windScaleToken) && windScaleToken.TokenType == TokenType.String)
            {
                windScale = windScaleToken.String;
            }
            if (hourData.TryGetValue("windSpeed", out var windSpeedToken) && windSpeedToken.TokenType == TokenType.String)
            {
                windSpeed = windSpeedToken.String;
            }
            if (hourData.TryGetValue("humidity", out var humidityToken) && humidityToken.TokenType == TokenType.String)
            {
                humidity = humidityToken.String;
            }
            if (hourData.TryGetValue("pop", out var popToken) && popToken.TokenType == TokenType.String)
            {
                pop = popToken.String;
            }
            if (hourData.TryGetValue("precip", out var precipToken) && precipToken.TokenType == TokenType.String)
            {
                precip = precipToken.String;
            }
            if (hourData.TryGetValue("pressure", out var pressureToken) && pressureToken.TokenType == TokenType.String)
            {
                pressure = pressureToken.String;
            }
            if (hourData.TryGetValue("cloud", out var cloudToken) && cloudToken.TokenType == TokenType.String)
            {
                cloud = cloudToken.String;
            }
            if (hourData.TryGetValue("dew", out var dewToken) && dewToken.TokenType == TokenType.String)
            {
                dew = dewToken.String;
            }
        }
    }
}
