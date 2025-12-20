using System;
using System.Collections;
using System.Collections.Generic;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;

namespace Sonic853.Udon.Weather
{
    public class UdonIpInfo : UdonSharpBehaviour
    {
        public UdonWeather udonWeather;
        /// <summary>
        /// IP 地址
        /// </summary>
        [NonSerialized] public string Addr;
        /// <summary>
        /// 地区
        /// </summary>
        [NonSerialized] public string Country;
        /// <summary>
        /// 省市
        /// </summary>
        [NonSerialized] public string Province;
        /// <summary>
        /// 城市
        /// </summary>
        [NonSerialized] public string City;
        /// <summary>
        /// 运营商
        /// </summary>
        [NonSerialized] public string Isp;
        /// <summary>
        /// 纬度
        /// </summary>
        [NonSerialized] public float Latitude;
        /// <summary>
        /// 经度
        /// </summary>
        [NonSerialized] public float Longitude;
        public string content;
        void Update()
        {
            if (udonWeather == null) enabled = false;
            enabled = FindWeather();
        }
        public void ClearData()
        {
            Addr = string.Empty;
            Country = string.Empty;
            Province = string.Empty;
            City = string.Empty;
            Isp = string.Empty;
            Latitude = 0;
            Longitude = 0;
        }
        public void SendFunction() => ParseJson(content);
        public void ParseJson() => ParseJson(content);
        public void ParseJson(string _content)
        {
            ClearData();
            if (!VRCJson.TryDeserializeFromJson(_content, out var result)) return;
            if (result.TokenType != TokenType.DataDictionary) { return; }
            var body = result.DataDictionary;
            if (body.TryGetValue("code", out var codeToken) && codeToken.TokenType == TokenType.Int)
            {
                var code = codeToken.Int;
                if (code != 0)
                {
                    var message = body.TryGetValue("message", out var messageToken) && messageToken.TokenType == TokenType.String ? messageToken.String : string.Empty;
                    Debug.LogError($"[UdonIpInfo.ParseJson] code: {code}, message: {message}");
                    return;
                }
            }

            var data = body.TryGetValue("data", out var dataToken) && dataToken.TokenType == TokenType.DataDictionary ? dataToken.DataDictionary : null;
            if (data == null) return;

            if (data.TryGetValue("addr", out var ipToken) && ipToken.TokenType == TokenType.String) Addr = ipToken.String;
            if (data.TryGetValue("country", out var countryToken) && countryToken.TokenType == TokenType.String) Country = countryToken.String;
            if (data.TryGetValue("province", out var regionToken) && regionToken.TokenType == TokenType.String) Province = regionToken.String;
            if (data.TryGetValue("city", out var cityToken) && cityToken.TokenType == TokenType.String) City = cityToken.String;
            if (data.TryGetValue("isp", out var ispToken) && ispToken.TokenType == TokenType.String) Isp = ispToken.String;
            if (data.TryGetValue("longitude", out var lonToken) && lonToken.TokenType == TokenType.Float) Longitude = lonToken.Float;
            if (data.TryGetValue("latitude", out var latToken) && latToken.TokenType == TokenType.Float) Latitude = latToken.Float;

            enabled = FindWeather();
        }
        public bool FindWeather()
        {
            if (udonWeather == null) { return false; }
            if (string.IsNullOrEmpty(udonWeather.content))
            {
                // 等待加载
                return true;
            }
            var findCity = string.Empty;
            if (!string.IsNullOrEmpty(City))
            {
                findCity = City;
            }
            if (string.IsNullOrEmpty(findCity) && !string.IsNullOrEmpty(Province))
            {
                findCity = Province;
            }
            if (!string.IsNullOrEmpty(findCity))
            {
                var locationItems = udonWeather.SearchWeathers(findCity);
                var locationItemsLength = locationItems.Length;
                if (locationItemsLength > 0)
                {
                    udonWeather.ShowWeather(locationItems[locationItemsLength - 1], false);
                    return false;
                }
                else
                {
                    locationItems = udonWeather.SearchWeathers(Province);
                    locationItemsLength = locationItems.Length;
                    if (locationItemsLength > 0)
                    {
                        udonWeather.ShowWeather(locationItems[locationItemsLength - 1], false);
                        return false;
                    }
                }
            }
            return false;
        }
    }
}
