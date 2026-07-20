using System.Collections.Generic;
using Arcade.UI;
using Arcade.UI.SongSelect;
using BepInEx.Logging;
using Rhythm;
using static Arcade.UI.SongSelect.ArcadeSongDatabase;
using static Rhythm.BeatmapIndex;

namespace UnbeatableWebsocket.Maps
{
    public class LocalPlayer
    {


        // "Progessions" are some kind of class 
        // that determine how the game behaves in stages
        // They are used to load the beatmap and the audio
        public class CustomProgression : ArcadeProgression
        {
            public new string stageScene = "TrainStationRhythm";

            public BeatmapItem beatmapItem;

            public CustomProgression(BeatmapItem beatmapItem, string chartPath)
                : base(beatmapItem.Path, RhythmGameType.ArcadeMode)
            {
                this.beatmapItem = beatmapItem;

                this.stageScene = "TrainStationRhythm";

                isCustomChart = true;

                customChartPath = chartPath;

                var mapDir = Encoder.DecodeMapName(beatmapItem.Path);
                var audioFilename = beatmapItem.Beatmap.general.audioFilename;
                customAudioPath = System.IO.Path.Combine(mapDir, audioFilename);
            }

            public new string GetBeatmapPath()
            {
                return beatmapItem.Path;
            }
            public new string GetSongName()
            {
                return beatmapItem.Song.name;
            }

            public new string GetDifficulty()
            {
                return "UNBEATABLE";
            }

            public new void Finish(string sceneIndex)
            {
                LevelManager.LoadLevel("ScoreScreenArcadeMode");
            }

            public new void Retry()
            {
                LevelManager.LoadLevel(string.IsNullOrEmpty(stageScene) ? "TrainStationRhythm" : stageScene);
            }

            public new void Back()
            {
                LevelManager.LoadLevel(JeffBezosController.arcadeMenuScene);
            }
        }

        public static void PlayBeatmapFromFile(string filePath = "C:\\Users\\Anwender\\Downloads\\testmap.txt")
        {


            if (!LocalLoader.LoadBeatmapFromFile(filePath, out BeatmapItem beatmapItem))
            {
                Plugin.Logger.LogInfo("Beatmap not found: " + filePath);
                return;
            }

            ArcadeProgression customProgression = new CustomProgression(beatmapItem, filePath);

            JeffBezosController.rhythmProgression = customProgression;

            if (string.IsNullOrEmpty(customProgression.stageScene))
            {
                customProgression.stageScene = "TrainStationRhythm";
            }

            LevelManager.LoadLevel(customProgression.stageScene);

        }


        // Retired function
        public static void PlayFromKey(string name)
        {
            var arcadeBGMManger = ArcadeBGMManager.Instance;
            var songList = ArcadeSongDatabase.Instance;
            var beatmapIndex = BeatmapIndex.defaultIndex;


            if (arcadeBGMManger != null && songList != null)
            {


                Plugin.Logger.LogInfo("Adding key: " + name);

                var beatmapItem = new BeatmapItem();

                beatmapItem.Path = "Custom_" + name + "/Beginner";
                var key = beatmapItem.Path;



                beatmapItem.Song = new Song(name);
                beatmapItem.Song.stageScene = "TrainStationRhythm";

                beatmapItem.Unlocked = true;



                beatmapItem.BeatmapInfo = new BeatmapInfo(null, "Beginner");


                //beatmapItem.Highscore = new HighScoreItem(key, 0, 0f, 0, cleared: false, new Dictionary<string, int>(), Modifiers.None);


                beatmapItem.Beatmap = new Beatmap();

                // This was a test and is not needed

                // This whole part would be needed for AddSongList()

                //beatmapItem.Beatmap.metadata.title = "Custom " + name;
                //beatmapItem.Beatmap.metadata.titleUnicode = "Custom " + name;
                //beatmapItem.Beatmap.metadata.artist = "Not You";
                //beatmapItem.Beatmap.metadata.artistUnicode = "Not You";
                //beatmapItem.Beatmap.metadata.tagData.Level = 10;
                //AddSongToArcadeList(songList,beatmapItem);

                Plugin.Logger.LogInfo("Playing Key: " + key);


                arcadeBGMManger.PlaySongPreview(beatmapItem);


            }

        }


    }
}
