
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
                urlSubmitter.url = weatherUrl;
                urlSubmitter.SubmitUrlWithUpdate();
            }
        }
    }
}
