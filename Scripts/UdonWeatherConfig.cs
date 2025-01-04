
using Sonic853.Udon.UrlLoader;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Sonic853.Udon.Weather
{
    public class UdonWeatherConfig : UdonSharpBehaviour
    {
        public VRCUrl weatherUrl;
        public VRCUrl weatherCnUrl;
        public string defaultWeather;
        public bool rememberWeatherName = true;
        public UdonWeather udonWeather;
        public UrlSubmitter urlSubmitter;
        [SerializeField] TextAsset testText;
        void Start()
        {
            if (udonWeather != null && !string.IsNullOrEmpty(defaultWeather))
            {
                udonWeather.defaultWeather = defaultWeather;
                udonWeather.rememberWeatherName = rememberWeatherName;
                udonWeather.Init();
                if (testText != null) udonWeather.LoadWeather(testText.text);
            }
            if (urlSubmitter != null && !string.IsNullOrEmpty(weatherUrl.ToString()))
            {
                var currentLanguage = VRCPlayerApi.GetCurrentLanguage() ?? "en";
                if (currentLanguage == "zh-CN")
                {
                    urlSubmitter.url = !string.IsNullOrEmpty(weatherCnUrl.ToString()) ? weatherCnUrl : weatherUrl;
                    urlSubmitter.altUrl = weatherUrl;
                }
                else
                {
                    urlSubmitter.url = weatherUrl;
                    urlSubmitter.altUrl = !string.IsNullOrEmpty(weatherCnUrl.ToString()) ? weatherCnUrl : weatherUrl;
                }
                urlSubmitter.SubmitUrlWithUpdate();
            }
        }
    }
}
