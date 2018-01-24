using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpotifyAPI.Local;
using SpotifyAPI.Local.Enums;
using SpotifyAPI.Local.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;

namespace OverFy
{
    public class SpotifyWorker
    {
        private Thread _workerThread;
        private CancellationTokenSource _cancellationTokenSource;
        private static SpotifyLocalAPI _spotify;

        bool running = false;
        bool spotifyHooked = false;

        public SpotifyWorker()
        {
            HookSpotify();
        }

        private void HookSpotify()
        {
            _spotify = new SpotifyLocalAPI();
            if (!SpotifyLocalAPI.IsSpotifyRunning())
                return; //Make sure the spotify client is running
            if (!SpotifyLocalAPI.IsSpotifyWebHelperRunning())
                return; //Make sure the WebHelper is running

            if (!_spotify.Connect())
                return; //We need to call Connect before fetching infos, this will handle Auth stuff

            spotifyHooked = true;
        }

        public void Start()
        {
            if (_workerThread != null || running) return;

            _cancellationTokenSource = new CancellationTokenSource();
            _workerThread = new Thread(BackgroundWorker_DoWork)
            {
                Name = "OverFly",
                IsBackground = true
            };
            _workerThread.Start(_cancellationTokenSource.Token);
            running = true;
        }

        public void Stop()
        {
            ClearScreen();

            if (_workerThread == null || !running) return;

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = null;
            _workerThread.Join();
            _workerThread = null;
            running = false;
        }

        private void BackgroundWorker_DoWork(object tokenObject)
        {
            var cancellationToken = (CancellationToken)tokenObject;

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    if (!spotifyHooked)
                    {
                        HookSpotify();
                    }

                    var currentStatus = _spotify.GetStatus();
                    StringBuilder result = GetSpotifyInfo(currentStatus);

                    RivaTuner.print(result.ToString());

                    Task.Delay(1000, cancellationToken).Wait(cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Writes a empty string to the screen for cleaning rivatuner of junk because it doesn't update if the applicatin suddenly stops
        /// </summary>
        public static void ClearScreen()
        {
            RivaTuner.print(String.Empty);
        }

        private StringBuilder GetSpotifyInfo(StatusResponse currentStatus)
        {
            StringBuilder result = new StringBuilder();

            string lastItem = String.Empty;

            bool skip = false;

            if (!SpotifyLocalAPI.IsSpotifyRunning())
            {
                skip = true;
            }
            if (!SpotifyLocalAPI.IsSpotifyWebHelperRunning())
            {
                skip = true;
            }

            for (int i = App.appSettings.PropertiesOrder.Count - 1; i > 0; i--)
            {
                if (App.appSettings.PropertiesOrder[i] != "System Time")
                {
                    lastItem = App.appSettings.PropertiesOrder[i];
                    break;
                }
            }

            if (!skip)
            {
                foreach (var item in App.appSettings.PropertiesOrder)
                {
                    switch (item)
                    {
                        case "System Time":
                            break;
                        case "Song Name":
                            result.Append(NormalizeString(currentStatus.Track.TrackResource.Name)); //It is needed to normalize strings because some like ç can break rivatuner
                            break;
                        case "Artist Name":
                            result.Append(NormalizeString(currentStatus.Track.ArtistResource.Name));
                            break;
                        case "Song Running Time":
                            TimeSpan runningTime = TimeSpan.FromSeconds(currentStatus.PlayingPosition);
                            TimeSpan totalTime = TimeSpan.FromSeconds(currentStatus.Track.Length);
                            result.Append(runningTime.ToString(@"mm\:ss") + "/" + totalTime.ToString(@"mm\:ss"));
                            break;
                        case "Album Name":
                            result.Append(NormalizeString(currentStatus.Track.AlbumResource.Name));
                            break;
                        case "Label":
                            result.Append("Now Playing: ");
                            break;
                        default:
                            result.Append(item);
                            break;
                    }

                    if (result.ToString() != String.Empty &&
                        item != "Label" &&
                        item != lastItem &&
                        item != "System Time")
                    {
                        result.Append(", ");
                    }
                }
            }

            if (App.appSettings.PropertiesOrder.Contains("System Time"))
            {
                if (!String.IsNullOrEmpty(result.ToString()))
                {
                    result.AppendLine();
                }

                result.Append(DateTime.Now.ToString("HH:mm"));
            }

            return result;
        }

        /// <summary>
        /// Removes Special characters from a string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string NormalizeString(string input)
        {
            var decomposed = input.Normalize(NormalizationForm.FormD);
            var filtered = decomposed.Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark);
            return new String(filtered.ToArray());
        }
    }
}
