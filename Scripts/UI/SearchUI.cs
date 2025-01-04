
using Sonic853.Udon.ArrayPlus;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Sonic853.Udon.Weather.UI
{
    public class SearchUI : UdonSharpBehaviour
    {
        public UdonWeather udonWeather;
        [SerializeField] TMP_InputField inputField;
        [SerializeField] InputField inputFieldLegacy;
        [SerializeField] LocationUI locationUIPrefab;
        [SerializeField] Transform locationUIsTransform;
        [SerializeField] LocationUI[] locationUIs = new LocationUI[0];
        public void OnTypeInput()
        {
            Clear();
            inputField.text = inputFieldLegacy.text;
            if (string.IsNullOrEmpty(inputField.text)) { return; }
            var result = udonWeather.SearchWeathers(inputField.text.Trim());
            for (var i = 0; i < result.Length; i++)
            {
                LocationUI locationUI;
                if (i >= locationUIs.Length)
                {
                    locationUI = (LocationUI)Instantiate(locationUIPrefab.gameObject, locationUIsTransform).GetComponent(typeof(UdonBehaviour));
                    locationUI.udonWeather = udonWeather;
                    UdonArrayPlus.Add(ref locationUIs, locationUI);
                }
                else
                {
                    locationUI = locationUIs[i];
                }
                locationUI.gameObject.SetActive(true);
                locationUI.LoadData(result[i]);
            }
        }
        public void SendFunction() => OnTypeInput();
        public void Clear()
        {
            foreach (var locationUI in locationUIs)
            {
                locationUI.Clear();
                locationUI.gameObject.SetActive(false);
            }
        }
    }
}
