using System;
using System.IO;
using System.Linq;
using UnbeatableWebsocket.Maps;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace UnbeatableWebsocket.WebSocket
{
    public class ServerBehaviour : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Plugin.Logger.LogInfo("WebSocket message received: " + e.Data);
            var command = e.Data.Split(' ');
            if (command.Length == 0) { 
                Send("ERROR Nothing sent");
                return;
            }
            switch (command[0]) {
                case "ping":
                    Send("pong");
                    break;
                case "play":
                    if (command.Length < 2)
                    {
                        Send("ERROR No song specified");
                        return;
                    }

                    var file = String.Join(" ", command.Skip(1));
                    Plugin.Logger.LogInfo("Command play: " + file);

                    if (!File.Exists(file))
                    {
                        Send("ERROR File not found");
                        return;
                    }

                    LocalPlayer.PlayBeatmapFromFile(file);

                    break;
                case "play_beatball":
                    if (command.Length < 2)
                    {
                        Send("ERROR No song specified");
                        return;
                    }

                    var file1 = String.Join(" ", command.Skip(1));
                    Plugin.Logger.LogInfo("Command play: " + file1);

                    if (!File.Exists(file1))
                    {
                        Send("ERROR File not found");
                        return;
                    }

                    //LocalBeatballPlayer.PlayBeatballFromFile(file1);

                    break;
            }

            Send("OK");
        }

    }

    public class Server
    {
        public static void Start()
        {
            try
            {
                var ws = new WebSocketServer(5080);

                ws.AddWebSocketService<ServerBehaviour>("/");
                ws.Start();

                Plugin.Logger.LogInfo("WebSocket server started on ws://localhost:5080");
            }
            catch (Exception e)
            {
                Plugin.Logger.LogInfo("WebSocket server failed to start: " + e.Message);
            }
        }
    }
}
