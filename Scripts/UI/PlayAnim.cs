
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Sonic853.Udon.Weather.UI
{
    public class PlayAnim : UdonSharpBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] string boolName = "Show";
        public void Show() => animator.SetBool(boolName, true);
        public void Hide() => animator.SetBool(boolName, false);
    }
}
