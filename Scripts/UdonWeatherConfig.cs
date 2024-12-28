
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
        public UdonWeather udonWeather;
        public UrlSubmitter urlSubmitter;
        void Start()
        {
            if (udonWeather != null && !string.IsNullOrEmpty(defaultWeather)) udonWeather.defaultWeather = defaultWeather;
            if (urlSubmitter != null && !string.IsNullOrEmpty(weatherUrl.ToString()))
            {
                urlSubmitter.url = weatherUrl;
                urlSubmitter.SubmitUrlWithUpdate();
            }
        }
    }
}
