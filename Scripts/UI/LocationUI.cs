
using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Sonic853.Udon.Weather.UI
{
    public class LocationUI : UdonSharpBehaviour
    {
        public UdonWeather udonWeather;
        public string location;
        [NonSerialized] public LocationItem locationItem;
        [SerializeField] Image QWIcon;
        [SerializeField] Image AccuWIcon;
        [SerializeField] TMP_Text locationText;
        [SerializeField] TMP_Text temp;
        public void LoadData(LocationItem _locationItem)
        {
            if (_locationItem == null) { return; }
            locationItem = _locationItem;
            if (_locationItem.icon.Length == 3)
            {
                QWIcon.gameObject.SetActive(true);
                AccuWIcon.gameObject.SetActive(false);
                QWIcon.sprite = udonWeather.GetSprite(_locationItem.icon);
            }
            else
            {
                QWIcon.gameObject.SetActive(false);
                AccuWIcon.gameObject.SetActive(true);
                AccuWIcon.sprite = udonWeather.GetSprite(_locationItem.icon);
            }
            if (_locationItem.adm1Name != "olddata" && _locationItem.adm1Name != _locationItem.locationName)
            {
                var texts = _($"{_locationItem.adm1Name}|{_locationItem.locationName}").Split('|');
                var finalText = texts[0];
                if (texts.Length > 1)
                {
                    finalText = texts[1];
                }
                locationText.text = finalText;
            }
            else
            {
                locationText.text = _(_locationItem.locationName);
            }
            temp.text = $"{_locationItem.temp}°";
        }
        public void LoadWeather(LocationItem _locationItem)
        {
            if (_locationItem == null) { return; }
            udonWeather.ShowWeather(_locationItem);
        }
        public void SendFunction() => LoadWeather(locationItem);
        public void Clear()
        {
            QWIcon.gameObject.SetActive(true);
            QWIcon.sprite = udonWeather.GetSprite("999");
            AccuWIcon.gameObject.SetActive(false);
            locationText.text = _("Loading...");
            temp.text = "-°";
        }
        #region 翻译
        public string _(string text) => udonWeather._(text);
        #endregion
    }
}
