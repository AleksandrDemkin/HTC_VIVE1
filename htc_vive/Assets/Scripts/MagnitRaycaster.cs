using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharSpell;
using UnityEngine.EventSystems;

namespace HTC.UnityPlugin.Pointer3D
{

    public class MagnitRaycaster : MonoBehaviour
    {
        public enum TypeOfMagnite
        {
            Blue,
            Red,
        }

        [SerializeField] private TypeOfMagnite ColorOfMagnite;
        [Tooltip("—сылка на spell персонажа")]
        [SerializeField] private CharMagnetic refToChar;

        private RaycastResult curObj;
        private Pointer3DRaycaster raycaster;

        private void OnValidate()
        {
            raycaster = GetComponent<Pointer3DRaycaster>();
        }

        private void LateUpdate()
        {
            Racasting();
        }

        public void Racasting()
        {
            curObj = raycaster.FirstRaycastResult();
        }

        public void StartMagnite()
        {
            if (curObj.isValid)
            {
                if (curObj.isValid)
                {
                    Rigidbody RG = curObj.gameObject.GetComponent<Rigidbody>();

                    switch ((int)ColorOfMagnite)
                    {
                        case 0:
                            if (RG != null) refToChar.SetBlue(curObj.gameObject.transform);
                            else refToChar.SetBlue(curObj.worldPosition);
                            break;

                        case 1:
                            if (RG != null) refToChar.SetRed(curObj.gameObject.transform);
                            else refToChar.SetRed(curObj.worldPosition);
                            break;
                    }
                }
                else return;
            }
        }
    }
}
