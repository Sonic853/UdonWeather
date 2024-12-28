
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
    public class DayUI : UdonSharpBehaviour
    {
        public WeatherUI weatherUI;
        /// <summary>
        /// 预报日期
        /// </summary>
        DateTime fxDate;
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
        /// 预报夜间天气状况的图标代码：https://dev.qweather.com/docs/resource/icons/
        /// </summary>
        public string iconNight;
        [SerializeField] Image qWImage;
        [SerializeField] Image qWImageNight;
        [SerializeField] Image accuWImage;
        [SerializeField] Image accuWImageNight;
        [SerializeField] TMP_Text week;
        [SerializeField] TMP_Text date;
        [SerializeField] TMP_Text temp;
        public void LoadData(DataDictionary dayData)
        {
            Clear();
            if (dayData.TryGetValue("fxDate", out var fxDateToken) && fxDateToken.TokenType == TokenType.String)
            {
                if (DateTime.TryParse(fxDateToken.String, out fxDate))
                {
                    var thisDay = DateTime.Now;
                    if (thisDay.Month == fxDate.Month && thisDay.Day == fxDate.Day) week.text = _("Today");
                    else week.text = _(fxDate.DayOfWeek.ToString());
                    date.text = fxDate.ToString(_("MM/dd"));
                }
            }
            if (dayData.TryGetValue("tempMax", out var tempMaxToken) && tempMaxToken.TokenType == TokenType.String)
            {
                int.TryParse(tempMaxToken.String, out tempMax);
            }
            if (dayData.TryGetValue("tempMin", out var tempMinToken) && tempMinToken.TokenType == TokenType.String)
            {
                int.TryParse(tempMinToken.String, out tempMin);
            }
            temp.text = $"{tempMax}°<color=#FFFFFFC8><size=28>{tempMin}°</size></color>";
            if (dayData.TryGetValue("iconDay", out var iconDayToken) && iconDayToken.TokenType == TokenType.String)
            {
                iconDay = iconDayToken.String;
                if (iconDay.Length == 3)
                {
                    // QWeather图标
                    qWImage.gameObject.SetActive(true);
                    accuWImage.gameObject.SetActive(false);
                    qWImage.sprite = weatherUI.GetSprite(iconDay);
                }
                else
                {
                    // AccuWeather图标
                    qWImage.gameObject.SetActive(false);
                    accuWImage.gameObject.SetActive(true);
                    accuWImage.sprite = weatherUI.GetSprite(iconDay);
                }
            }
            if (dayData.TryGetValue("iconNight", out var iconNightToken) && iconNightToken.TokenType == TokenType.String)
            {
                iconNight = iconNightToken.String;
                if (iconNight.Length == 3)
                {
                    // QWeather图标
                    qWImageNight.gameObject.SetActive(true);
                    accuWImageNight.gameObject.SetActive(false);
                    qWImageNight.sprite = weatherUI.GetSprite(iconNight);
                }
                else
                {
                    // AccuWeather图标
                    qWImageNight.gameObject.SetActive(false);
                    accuWImageNight.gameObject.SetActive(true);
                    accuWImageNight.sprite = weatherUI.GetSprite(iconNight);
                }
            }
        }
        public void Clear() {
            qWImage.gameObject.SetActive(true);
            qWImage.sprite = weatherUI.GetSprite("999");
            accuWImage.gameObject.SetActive(false);
            qWImageNight.gameObject.SetActive(true);
            qWImageNight.sprite = weatherUI.GetSprite("999");
            accuWImageNight.gameObject.SetActive(false);
            week.text = "-";
            date.text = "-/-";
            temp.text = "-°";
        }
        #region 翻译
        public string _(string text) => weatherUI._(text);
        #endregion
    }
}
