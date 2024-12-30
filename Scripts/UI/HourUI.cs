
using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;

namespace Sonic853.Udon.Weather.UI
{
    public class HourUI : UdonSharpBehaviour
    {
        public WeatherUI weatherUI;
        /// <summary>
        /// 预报时间
        /// </summary>
        public DateTime fxTime;
        /// <summary>
        /// 温度，默认单位：摄氏度
        /// </summary>
        public int temp = -1;
        /// <summary>
        /// 预报白天天气状况的图标代码：https://dev.qweather.com/docs/resource/icons/
        /// </summary>
        public string icon;
        [SerializeField] Image qWImage;
        [SerializeField] Image accuWImage;
        [SerializeField] TMP_Text time;
        [SerializeField] TMP_Text tempText;
        public void LoadData(DataDictionary hourData)
        {
            if (hourData.TryGetValue("fxTime", out var fxTimeToken) && fxTimeToken.TokenType == TokenType.String)
            {
                DateTime.TryParse(fxTimeToken.String, out fxTime);

                var hour = fxTime.Hour;
                var isPm = fxTime.Hour >= 12;
                if (hour > 12) hour -= 12;
                var hourText = _($"{{0}} {(isPm ? "PM" : "AM")}");
                time.text = string.Format(hourText, hour);
            }
            if (hourData.TryGetValue("temp", out var tempToken) && tempToken.TokenType == TokenType.String)
            {
                int.TryParse(tempToken.String, out temp);
                tempText.text = $"{temp}°";
            }
            if (hourData.TryGetValue("icon", out var iconToken) && iconToken.TokenType == TokenType.String)
            {
                icon = iconToken.String;
                if (icon.Length == 3)
                {
                    // QWeather图标
                    qWImage.gameObject.SetActive(true);
                    accuWImage.gameObject.SetActive(false);
                    qWImage.sprite = weatherUI.GetSprite(icon);
                }
                else
                {
                    qWImage.gameObject.SetActive(false);
                    accuWImage.gameObject.SetActive(true);
                    accuWImage.sprite = weatherUI.GetSprite(icon);
                }
            }
        }
        public void Clear() {
            qWImage.gameObject.SetActive(true);
            qWImage.sprite = weatherUI.GetSprite("999");
            accuWImage.gameObject.SetActive(false);
            time.text = "-- --";
            tempText.text = "-°";
        }
        #region 翻译
        public string _(string text) => weatherUI._(text);
        #endregion
    }
}
