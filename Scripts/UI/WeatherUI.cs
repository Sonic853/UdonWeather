
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
        public Sprite[] qWIcons;
        [NonSerialized] public DataDictionary qWIconsIndex = new DataDictionary();
        public Sprite[] accuWIcons;
        [NonSerialized] public DataDictionary accuWIconsIndex = new DataDictionary();
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
        public void InitIndex()
        {
            qWIconsIndex = new DataDictionary();
            accuWIconsIndex = new DataDictionary();
            for (var i = 0; i < qWIcons.Length; i++)
            {
                qWIconsIndex.Add(qWIcons[i].name, i);
            }
            for (var i = 0; i < accuWIcons.Length; i++)
            {
                accuWIconsIndex.Add(accuWIcons[i].name, i);
            }
        }
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
                qWImage.sprite = GetSprite(locationItem.icon, qWIcons, qWIconsIndex);
            }
            else
            {
                qWImage.gameObject.SetActive(false);
                accuWImage.gameObject.SetActive(true);
                accuWImage.sprite = GetSprite(locationItem.icon, accuWIcons, accuWIconsIndex);
            }
            var _where = _(locationItem.locationName);
            if (!string.IsNullOrEmpty(locationItem.adm1Name)
                && locationItem.adm1Name != "olddata"
                && locationItem.adm1Name != locationItem.locationName) _where = _($"{locationItem.adm1Name}{", "}{locationItem.locationName}");
            where.text = _where;
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
            qWImage.sprite = GetSprite("999");
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
        public Sprite GetSprite(string iconName)
        {
            if (iconName.Length == 3) return GetSprite(iconName, qWIcons, qWIconsIndex);
            return GetSprite(iconName, accuWIcons, accuWIconsIndex);
        }
        public static Sprite GetSprite(string iconName, Sprite[] icons, DataDictionary iconsIndex)
        {
            if (iconsIndex.TryGetValue(iconName, out var indexToken) && indexToken.TokenType == TokenType.Int)
            {
                return icons[indexToken.Int];
            }
            return icons[icons.Length - 1];
        }
        #region 翻译
        public string _(string text) => udonWeather._(text);
        #endregion
    }
}
