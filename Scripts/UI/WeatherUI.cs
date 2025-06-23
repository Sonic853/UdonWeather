
using System;
using Sonic853.Translate;
using Sonic853.Udon.ArrayPlus;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;

namespace Sonic853.Udon.Weather.UI
{
    public class WeatherUI : UdonSharpBehaviour
    {
        public UdonWeather udonWeather;
        [SerializeField] Image qWImage;
        [SerializeField] Image accuWImage;
        [SerializeField] TMP_Text dataSource;
        [SerializeField] TMP_Text where;
        [SerializeField] TMP_Text temp;
        [SerializeField] TMP_Text tempHi;
        [SerializeField] TMP_Text tempLo;
        [SerializeField] TMP_Text tempReal;
        [SerializeField] HourUI hourUIItemPrefab;
        [SerializeField] Transform hourUIItemsTransform;
        [SerializeField] HourUI[] hourUIItems;
        [SerializeField] DayUI dayUIItemPrefab;
        [SerializeField] Transform dayUIItemsTransform;
        [SerializeField] DayUI[] dayUIItems;
        public int maxDayCount = 7;
        public int maxHourCount = 7;
        public void LoadData(LocationItem locationItem)
        {
            Clear();
            dataSource.text = $"{_("Data Source: ")}";
            var ts = new string[locationItem.source.Length];
            for (var i = 0; i < locationItem.source.Length; i++)
            {
                ts[i] = _(locationItem.source[i]);
            }
            dataSource.text += string.Join(_(", "), ts);
            if (locationItem.icon.Length == 3)
            {
                qWImage.gameObject.SetActive(true);
                accuWImage.gameObject.SetActive(false);
                qWImage.sprite = udonWeather.GetSprite(locationItem.icon);
            }
            else
            {
                qWImage.gameObject.SetActive(false);
                accuWImage.gameObject.SetActive(true);
                accuWImage.sprite = udonWeather.GetSprite(locationItem.icon);
            }
            var locationName = "";
            if (!string.IsNullOrEmpty(locationItem.adm1Name)
                && locationItem.adm1Name != "olddata"
                && locationItem.adm1Name != locationItem.locationName) locationName = _($"{locationItem.adm1Name}|{locationItem.locationName}");
            else locationName = _(locationItem.locationName);
            var texts = locationName.Split('|');
            var finalText = texts[0];
            if (texts.Length > 1)
            {
                finalText = texts[1];
            }
            where.text = finalText;
            temp.text = $"{locationItem.temp}°";
            tempReal.text = $"{_("Real Feel: ")}{locationItem.feelsLike}°";
            // 从 daily 的第一个数据中获取最高最低温度
            if (locationItem.daily.Count > 0 && locationItem.daily.TryGetValue(0, out var dayToken) && dayToken.TokenType == TokenType.DataDictionary)
            {
                var dayData = dayToken.DataDictionary;
                if (dayData.TryGetValue("tempMax", out var tempMaxToken) && tempMaxToken.TokenType == TokenType.String)
                {
                    int.TryParse(tempMaxToken.String, out var tempMax);
                    tempHi.text = $"{_("Highest: ")}{tempMax}°";
                }
                if (dayData.TryGetValue("tempMin", out var tempMinToken) && tempMinToken.TokenType == TokenType.String)
                {
                    int.TryParse(tempMinToken.String, out var tempMin);
                    tempLo.text = $"{_("Lowest: ")}{tempMin}°";
                }
            }
            var _maxHourCount = locationItem.hourly.Count > maxHourCount ? maxHourCount : locationItem.hourly.Count;
            for (var i = 0; i < _maxHourCount; i++)
            {
                if (!locationItem.hourly.TryGetValue(i, out var _hourToken) || _hourToken.TokenType != TokenType.DataDictionary) { continue; }
                hourUIItemsTransform.gameObject.SetActive(true);
                var hourData = _hourToken.DataDictionary;
                if (hourUIItems.Length < 1 + i)
                {
                    var hourUI = (HourUI)Instantiate(hourUIItemPrefab.gameObject, hourUIItemsTransform).GetComponent(typeof(UdonBehaviour));
                    hourUI.weatherUI = this;
                    UdonArrayPlus.Add(ref hourUIItems, hourUI);
                }
                hourUIItems[i].gameObject.SetActive(true);
                hourUIItems[i].LoadData(hourData);
            }
            var _maxDayCount = locationItem.daily.Count > maxDayCount ? maxDayCount : locationItem.daily.Count;
            for (var i = 0; i < _maxDayCount; i++)
            {
                if (!locationItem.daily.TryGetValue(i, out var _dayToken) || _dayToken.TokenType != TokenType.DataDictionary) { continue; }
                dayUIItemsTransform.gameObject.SetActive(true);
                var dayData = _dayToken.DataDictionary;
                if (dayUIItems.Length < 1 + i)
                {
                    var dayUI = (DayUI)Instantiate(dayUIItemPrefab.gameObject, dayUIItemsTransform).GetComponent(typeof(UdonBehaviour));
                    dayUI.weatherUI = this;
                    UdonArrayPlus.Add(ref dayUIItems, dayUI);
                }
                dayUIItems[i].gameObject.SetActive(true);
                dayUIItems[i].LoadData(dayData);
            }
        }
        public void Clear()
        {
            dataSource.text = $"{_("Data Source: ")}";
            qWImage.gameObject.SetActive(true);
            qWImage.sprite = udonWeather.GetSprite("999");
            accuWImage.gameObject.SetActive(false);
            where.text = _("Loading...");
            tempReal.text = $"{_("Real Feel: ")}-°";
            temp.text = "-°";
            tempHi.text = $"{_("Highest: ")}-°";
            tempLo.text = $"{_("Lowest: ")}-°";
            foreach (var hourUIItem in hourUIItems)
            {
                hourUIItem.Clear();
                hourUIItem.gameObject.SetActive(false);
            }
            hourUIItemsTransform.gameObject.SetActive(false);
            foreach (var dayUIItem in dayUIItems)
            {
                dayUIItem.Clear();
                dayUIItem.gameObject.SetActive(false);
            }
            dayUIItemsTransform.gameObject.SetActive(false);
        }
        public Sprite GetSprite(string iconName) => udonWeather.GetSprite(iconName);
        #region 翻译
        public string _(string text) => udonWeather._(text);
        #endregion
    }
}
