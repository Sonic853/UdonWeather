
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Sonic853.Udon.Weather
{
    public class ColliderLayerChanger : UdonSharpBehaviour
    {
        bool isFound = false;
        [SerializeField] LayerMask excludeLayers;
        void Update()
        {
            if (isFound) { return; }
            var collider = GetComponent<Collider>();
            if (collider == null) { return; }
            isFound = true;
            collider.excludeLayers = excludeLayers;
        }
    }
}
