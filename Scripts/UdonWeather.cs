
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using Sonic853.Translate;
using VRC.SDK3.Data;
using Sonic853.Udon.ArrayPlus;
using Sonic853.Udon.Weather.UI;

namespace Sonic853.Udon.Weather
{
    public class UdonWeather : UdonSharpBehaviour
    {
        public TranslateManager translateManager;
        public WeatherUI weatherUI;
        public TranslateManager Translate => translateManager == null ? translateManager = TranslateManager.Instance() : translateManager;
        [SerializeField] LocationItem locationItemPrefab;
        [SerializeField] Transform locationItemsTransform;
        /// <summary>
        /// 所有地区的天气信息
        /// </summary>
        [SerializeField] LocationItem[] locationItems;
        DataDictionary locations = new DataDictionary();
        public string defaultWeather;
        [SerializeField] TextAsset testText;
        DataDictionary data;
        public string content;
        void Start()
        {
            if (!translateManager.LoadedTranslate) translateManager.LoadTranslate();
            if (weatherUI != null)
            {
                weatherUI.udonWeather = this;
                weatherUI.InitIndex();
            }
            if (testText != null) LoadWeather(testText.text);
        }
        public void LoadWeather() => LoadWeather(content);
        public void LoadWeather(string _content)
        {
            Clear();
            content = _content;
            if (!VRCJson.TryDeserializeFromJson(_content, out var result)) { return; }
            if (result.TokenType != TokenType.DataDictionary) { return; }
            data = result.DataDictionary;
            var adm1Names = data.GetKeys();
            var adm1NamesCount = adm1Names.Count;
            for (var a = 0; a < adm1NamesCount; a++)
            {
                if (!adm1Names.TryGetValue(a, out var adm1NameToken)) { continue; }
                var adm1Name = adm1NameToken.String;
                if (!data.TryGetValue(adm1Name, out var adm1DataToken)) { continue; }
                var adm1Data = adm1DataToken.DataDictionary;
                var locationNames = adm1Data.GetKeys();
                var locationNamesCount = locationNames.Count;
                for (var l = 0; l < locationNamesCount; l++)
                {
                    if (!locationNames.TryGetValue(l, out var locationNameToken)) { continue; }
                    var locationName = locationNameToken.String;
                    if (!adm1Data.TryGetValue(locationName, out var locationDataToken)) { continue; }
                    var locationData = locationDataToken.DataDictionary;
                    var locationItem = (LocationItem)Instantiate(locationItemPrefab.gameObject, locationItemsTransform).GetComponent(typeof(UdonBehaviour));
                    locationItem.udonWeather = this;
                    UdonArrayPlus.Add(ref locationItems, locationItem);
                    locations.Add($"{adm1Name}|{locationName}".ToLower(), locationItems.Length - 1);
                    locationItem.UpdateData(adm1Name, locationName, locationData);
                }
            }
            ShowWeather(defaultWeather);
        }
        public void ShowWeather(string locationName)
        {
            if (string.IsNullOrEmpty(locationName)) { return; }
            var _defaultWeather = locationName.ToLower();
            if (_defaultWeather.Contains("|"))
            {
                if (locations.TryGetValue(_defaultWeather, out var indexToken) && indexToken.TokenType == TokenType.Int)
                {
                    var index = indexToken.Int;
                    locationItems[index].LoadUI();
                }
            }
            else
            {
                foreach (var locationItem in locationItems)
                {
                    if (locationItem.locationName.ToLower() == _defaultWeather)
                    {
                        locationItem.LoadUI();
                        break;
                    }
                }
            }
        }
        public void SendFunction() => LoadWeather(content);
        public void Clear()
        {
            if (locationItems.Length == 0) { return; }
            foreach (var locationItem in locationItems)
            {
                locationItem.Clear();
                Destroy(locationItem.gameObject);
            }
            locationItems = new LocationItem[0];
            locations = new DataDictionary();
        }
        #region 翻译
        public string _(string text) => Translate.GetText(text);
        #endregion
    }
}
