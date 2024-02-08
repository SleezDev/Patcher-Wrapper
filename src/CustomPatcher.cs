using BepInEx;
using HarmonyLib;
using System;
using System.Reflection;

namespace CustomPatcher
{
    public class Patcher : IPatcher
    {
        public bool _patched = false;
        public static Patcher instance;
        private BepInPlugin p;

        public Patcher()
        {
            instance = this;
        }

        /// <summary>
        /// Patches a user-created mod via class instances
        /// </summary>
        /// <param name="clazz">The Type instance to patch</param>
        /// <param name="guid">The BepInEx unique identifier</param>
        /// <param name="gname">The plugin name</param>
        /// <param name="version">The patch's version.</param>
        /// <param name="cb">Optional callback, called when patch has ran</param>
        public void PatchClient<T>(T clazz, string guid, string gname, string version, Action<string> cb = null)
        {
            new Harmony(guid).PatchAll();
            typeof(T).GetField("instance", BindingFlags.Static | BindingFlags.NonPublic)?.SetValue(null, this);

            p = new BepInPlugin(guid, gname, version);
            if ((typeof(BepInPlugin).GetProperty("GUID")?.GetValue(p) as string) == gname)
            {
                _patched = true;
                cb?.Invoke($"Successfully Patched! [GUID: {(typeof(BepInPlugin).GetProperty("GUID")?.GetValue(p) as string)}, Name: {gname.ToString()}, Version: {version}]");
            }
            else
            {
                cb?.Invoke("<" + Assembly.GetExecutingAssembly().GetName().Name + "> There seems to be an unknown class patching issue. Relaunch?");
                return;
            }
        }
    }

    internal interface IPatcher
    {
        void PatchClient<T>(T clazz, string guid, string gname, string version, Action<string> cb = null);
    }
}
