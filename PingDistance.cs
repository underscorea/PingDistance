using BepInEx;
using RoR2;
using RoR2.UI;
using UnityEngine;

namespace underscorea
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.underscorea.pingdistance", "PingDistance", "1.0")]
    public class PingDistance : BaseUnityPlugin
    {
        public void Awake()
        {
            Debug.Log("PingDistance is awake, rise and shine.");
        }

        public void Update()
        {
            if (!Stage.instance)
            {
                return;
            }

            if (PingIndicator.instancesList.Count > 0)
            {
                Vector3 origin = new Vector3();
                var localPlayers = NetworkUser.readOnlyLocalPlayersList;
                if (localPlayers.Count > 0)
                {
                    // I don't know whether I can show different distances for different local players...
                    var target = localPlayers[0].cameraRigController.target;
                    if (target)
                    {
                        origin = target.GetComponent<CharacterBody>().footPosition;
                    }
                }

                foreach (PingIndicator pi in PingIndicator.instancesList)
                {
                    if (pi.enabled) // not sure if this is needed
                    {
                        float dist = Vector3.Distance(origin, pi.transform.position);
                        int index = pi.pingText.text.IndexOf((char)0x200B);
                        string sub = index >= 0 ? pi.pingText.text.Substring(0, index) : pi.pingText.text;
                        // I don't like this, but I also don't like having to call multiple GetComponent<T> just to get the pingText
                        pi.pingText.text = sub + (char)0x200B + " (" + dist.ToString("0.0") + "m)";
                    }
                }
            }
        }
    }
}
