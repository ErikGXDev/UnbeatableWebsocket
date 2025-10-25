using BepInEx;
using BepInEx.Logging;

namespace UnbeatableWebsocket;

[BepInPlugin("erikg.unbeatable.ws", "Unbeatable Websocket", "0.1.0")]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin UnbeatableWS is loaded!");
    }
}