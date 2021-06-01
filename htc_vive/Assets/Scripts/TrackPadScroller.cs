using CharSpell;
using HTC.UnityPlugin.Vive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace CahrSpell
{
    public class TrackPadScroller : MonoBehaviour
    {
        [SerializeField] private float speed = 10, deadzone = 0.1f;

        private SteamVR_RenderModel vive;
        private CharMagnetic _magnite;

        private void Start()
        {
            _magnite = GetComponent<CharMagnetic>();
        }

        private void Update()
        {
            if (vive == null)
                vive = GetComponentInChildren<SteamVR_RenderModel>();

            float dp = ViveInput.GetPadTouchDelta(HandRole.RightHand).y;

            if (Mathf.Abs(dp) > deadzone)
            {
                _magnite.ChangeSpringPower(dp * speed);
                vive.controllerModeState.bScrollWheelVisible = true;
            }

            if (vive == null)
                vive = GetComponentInChildren<SteamVR_RenderModel>();

            float dp1 = ViveInput.GetPadTouchDelta(HandRole.LeftHand).y;

            if (Mathf.Abs(dp1) > deadzone)
            {
                _magnite.ChangeSpringPower(dp1 * speed);
                vive.controllerModeState.bScrollWheelVisible = true;
            }
        }
    }
}
