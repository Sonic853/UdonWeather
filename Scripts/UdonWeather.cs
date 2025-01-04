
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using Sonic853.Translate;
using VRC.SDK3.Data;
using Sonic853.Udon.ArrayPlus;
using Sonic853.Udon.Weather.UI;
using System;
using VRC.SDK3.Persistence;

namespace Sonic853.Udon.Weather
{
    public class UdonWeather : UdonSharpBehaviour
    {
        public TranslateManager translateManager;
        public WeatherUI weatherUI;
        public bool rememberWeatherName = true;
        [SerializeField] LocationUI[] locationUIs;
        public TranslateManager Translate => translateManager == null ? translateManager = TranslateManager.Instance() : translateManager;
        public Sprite[] qWIcons;
        [NonSerialized] public DataDictionary qWIconsIndex = new DataDictionary();
        public Sprite[] accuWIcons;
        [NonSerialized] public DataDictionary accuWIconsIndex = new DataDictionary();
        [SerializeField] LocationItem locationItemPrefab;
        [SerializeField] Transform locationItemsTransform;
        /// <summary>
        /// 所有地区的天气信息
        /// </summary>
        [SerializeField] LocationItem[] locationItems = new LocationItem[0];
        LocationItem currentLocationItem;
        DataDictionary locations = new DataDictionary();
        public string defaultWeather;
        // [SerializeField] TextAsset testText;
        DataDictionary data;
        public string content;
        // void Start()
        // {
        //     Init();
        //     // if (testText != null) LoadWeather(testText.text);
        // }
        public void Init()
        {
            if (!translateManager.LoadedTranslate) translateManager.LoadTranslate();
            InitIndex();
            foreach (var locationUI in locationUIs)
            {
                if (locationUI == null) { continue; }
                locationUI.udonWeather = this;
            }
            if (weatherUI != null)
            {
                weatherUI.udonWeather = this;
            }
        }
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
                    var locationKey = $"{adm1Name}|{locationName}".ToLower();
                    locationItem.tAdm1Name = _(adm1Name);
                    locationItem.tLocationName = adm1Name != locationName ? _(locationKey) : _(locationName);
                    if (locations.ContainsKey(locationKey))
                    {
                        locations.SetValue(locationKey, locationItems.Length - 1);
                    }
                    else
                    {
                        locations.Add(locationKey, locationItems.Length - 1);
                    }
                    locationItem.UpdateData(adm1Name, locationName, locationData);
                }
            }
            foreach (var locationUI in locationUIs)
            {
                if (locationUI == null) { continue; }
                if (locations.TryGetValue(locationUI.location.ToLower(), out var indexToken) && indexToken.TokenType == TokenType.Int)
                {
                    locationUI.LoadData(locationItems[indexToken.Int]);
                }
                else
                {
                    locationUI.Clear();
                    locationUI.locationItem = null;
                }
            }
            ReadDefaultWeather();
        }
        public void ReadDefaultWeather()
        {
            var localPlayer = Networking.LocalPlayer;
            if (localPlayer != null && PlayerData.TryGetString(localPlayer, $"Sonic853.Udon.Weather", out var _defaultWeather))
            {
                if (!string.IsNullOrEmpty(_defaultWeather))
                    defaultWeather = _defaultWeather;
            }
            if (!defaultWeather.Contains("|"))
            {
                ShowWeather($"{defaultWeather}|{defaultWeather}");
            }
            else
            {
                ShowWeather(defaultWeather);
            }
        }
        public void ShowWeather(string locationName) => ShowWeather(GetWeather(locationName));
        public void ShowWeather(int locationIndex)
        {
            if (locationIndex == -1 || locationIndex >= locationItems.Length) { return; }
            ShowWeather(locationItems[locationIndex]);
        }
        public void ShowWeather(LocationItem location)
        {
            if (location == null) { return; }
            if (currentLocationItem == location) { return; }
            currentLocationItem = location;
            defaultWeather = $"{location.adm1Name}|{location.locationName}";
            PlayerData.SetString($"Sonic853.Udon.Weather", defaultWeather);
            location.LoadUI();
        }
        public LocationItem[] SearchWeathers(string userType)
        {
            if (string.IsNullOrEmpty(userType)) { return new LocationItem[0]; }
            var foundIndex = new DataList();
            var _userType = userType.ToLower();
            for (int i = 0; i < locationItems.Length; i++)
            {
                var locationItem = locationItems[i];
                if (
                    locationItem.locationName.ToLower().Contains(_userType)
                    || locationItem.adm1Name.ToLower().Contains(_userType)
                    || locationItem.tAdm1Name.ToLower().Contains(_userType)
                    || locationItem.tLocationName.ToLower().Contains(_userType)
                )
                {
                    foundIndex.Add(i);
                }
            }
            var foundIndexCount = foundIndex.Count;
            if (foundIndexCount > 0)
            {
                var foundLocationItems = new LocationItem[foundIndexCount];
                for (int i = 0; i < foundIndexCount; i++)
                {
                    foundLocationItems[i] = locationItems[foundIndex[i].Int];
                }
                return foundLocationItems;
            }
            return new LocationItem[0];
        }
        public LocationItem GetWeather(string locationName)
        {
            if (string.IsNullOrEmpty(locationName)) { return null; }
            var _defaultWeather = locationName.ToLower();
            if (_defaultWeather.Contains("|"))
            {
                if (locations.TryGetValue(_defaultWeather, out var indexToken) && indexToken.TokenType == TokenType.Int)
                {
                    var index = indexToken.Int;
                    return locationItems[index];
                }
                else
                {
                    var locationNames = _defaultWeather.Split('|');
                    var _locationName = locationNames[0];
                    if (!string.IsNullOrEmpty(locationNames[1])) _locationName = locationNames[1];
                    foreach (var locationItem in locationItems)
                    {
                        if (locationItem.locationName.ToLower() == _locationName)
                        {
                            return locationItem;
                        }
                    }
                }
            }
            else
            {
                foreach (var locationItem in locationItems)
                {
                    if (locationItem.locationName.ToLower() == _defaultWeather)
                    {
                        return locationItem;
                    }
                }
            }
            return null;
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
        public override void OnPlayerDataUpdated(VRCPlayerApi player, PlayerData.Info[] infos)
        {
            if (!rememberWeatherName) { return; }
            ReadDefaultWeather();
        }
        /// <summary>
        /// 当玩家数据加载后触发
        /// </summary>
        /// <param name="player"></param>
        public override void OnPlayerRestored(VRCPlayerApi player)
        {
            ReadDefaultWeather();
        }
        #region 翻译
        public string _(string text) => Translate.GetText(text);
        #endregion
    }
}
