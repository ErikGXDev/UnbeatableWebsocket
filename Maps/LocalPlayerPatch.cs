using System;
using System.IO;
using FMOD.Studio;
using HarmonyLib;
using Rhythm;

namespace UnbeatableWebsocket.Maps
{
    public static class LocalPlayerPatch
    {

        // Detect if the beatmap is a custom one
        // Since ParseBeatmap attempts to load a TextAsset (which we cant create)
        // we have to patch the function and make it load from text contents instead.
        [HarmonyPatch(typeof(BeatmapParser), "ParseBeatmap", new Type[] { })]
        [HarmonyPrefix]
        public static bool ParseBeatmapPatch(ref BeatmapParser __instance)
        {
            if (JeffBezosController.rhythmProgression is LocalPlayer.CustomProgression progression)
            {
                Plugin.Logger.LogInfo("Loading custom beatmap...");


                __instance.beatmapIndex = BeatmapIndex.defaultIndex;



                // Set the beatmap to the one by the parser
                __instance.beatmap = progression.beatmapItem.Beatmap;
                __instance.audioKey = progression.GetSongName();
                __instance.beatmapPath = progression.GetBeatmapPath();

                return false;
            }
            return true;
        }

        // Make the game play local files
        [HarmonyPatch(typeof(RhythmTracker), "PrepareInstance", new Type[] { typeof(EventInstance), typeof(PlaySource), typeof(string) })]
        [HarmonyPrefix]
        public static bool PrepareInstancePatch(EventInstance instance, ref PlaySource source, ref string key)
        {
            if (key.StartsWith(Encoder.customPathIndicator) && key.Contains("."))
            {

                key = Encoder.DecodeAudioName(key);
                
                if (File.Exists(key))
                {
                    Plugin.Logger.LogInfo("Loading custom audio: " + key);
                    source = PlaySource.FromFile;
                }
                else
                {
                    Plugin.Logger.LogInfo("Custom audio not found: " + key);
                }
            }
            return true;
        }


        // Patch to load audio from file
        // Since the game really wants to load from its own audio table,
        // we need to patch this to load from a file instead
        // That way we can make the game load any sound file (and in this case,
        // the one from our custom level)
        [HarmonyPatch(typeof(RhythmTracker), "PreloadFromTable")]
        [HarmonyPrefix]
        public static bool PreloadFromTablePatch(string key, ref RhythmTracker __instance)
        {
            if (key.Contains("."))
            {
                if (File.Exists(key))
                {
                    __instance.PreloadFromFile(key);
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return true;
        }
        
    }
}
