
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
        [NonSerialized] public int locationIndex = -1;
        [SerializeField] Image QWIcon;
        [SerializeField] Image AccuWIcon;
        [SerializeField] TMP_Text locationText;
        [SerializeField] TMP_Text temp;
        public void LoadData(LocationItem locationItem)
        {
            if (locationItem.icon.Length == 3)
            {
                QWIcon.gameObject.SetActive(true);
                AccuWIcon.gameObject.SetActive(false);
                QWIcon.sprite = udonWeather.GetSprite(locationItem.icon);
            }
            else
            {
                QWIcon.gameObject.SetActive(false);
                AccuWIcon.gameObject.SetActive(true);
                AccuWIcon.sprite = udonWeather.GetSprite(locationItem.icon);
            }
            if (locationItem.adm1Name != "olddata" && locationItem.adm1Name != locationItem.locationName)
            {
                var texts = _($"{locationItem.adm1Name}{"|"}{locationItem.locationName}").Split('|');
                var finalText = texts[0];
                if (texts.Length > 1)
                {
                    finalText = texts[1];
                }
                locationText.text = finalText;
            }
            else
            {
                locationText.text = _(locationItem.locationName);
            }
            temp.text = $"{locationItem.temp}°";
        }
        public void LoadWeather(int _locationIndex)
        {
            if (_locationIndex == -1) { return; }
            udonWeather.ShowWeather(_locationIndex);
        }
        public void SendFunction() => LoadWeather(locationIndex);
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
