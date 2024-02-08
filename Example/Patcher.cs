using BepInEx;
using UnityEngine;

using CustomPatcher;

namespace Patcher_Example
{
    [BepInPlugin("com.null.lol", "null", "1.0")]
    public class Class1 : BaseUnityPlugin
    {
        void Awake()
        {
            if (!Patcher.instance._patched && Patcher.instance != null)
            {
                Patcher.instance.PatchClient(this, "null", "1", "2", (m) => // args: null = guid, 1 = plugin name, 2 = plugin version
                {
                    if (m != null)
                        Debug.Log(m?.ToString());
                });
            }
            else
            {
                Debug.Log("Hm, It seems to be already patched. Thought about reloading the module?");
                return; // stop rest of code from executing
            }

            Debug.Log("it worked :)");
        }
    }
}
