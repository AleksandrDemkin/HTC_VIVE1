using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HTC.UnityPlugin.Vive
{
    public class MyTeleport : Teleportable
    {
        [SerializeField] private float speed;
        [SerializeField] private float coolDown;

        public override IEnumerator StartTeleport(RaycastResult hitResult, Vector3 position, Quaternion rotation, float delay)
        {
            while (true)
            {
                target.position = Vector3.MoveTowards(target.position, position, speed * Time.deltaTime);

                Vector3 v = position;
                v.y = target.position.y;

                if (Vector3.Distance(target.position, v) < 0.1f)
                {
                    yield return new WaitForSeconds(coolDown);
                    teleportCoroutine = null;
                    yield break;
                }
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
