using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpotifyAPI.Local;
using SpotifyAPI.Local.Enums;
using SpotifyAPI.Local.Models;
using System.Threading;
using System.Threading.Tasks;

namespace OverFy
{
    public class Work
    {
        private Thread _workerThread;
        private CancellationTokenSource _cancellationTokenSource;
        private static SpotifyLocalAPI _spotify;

        bool running = false;
        bool spotifyHooked = false;

        public Work()
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

                    if (!SpotifyLocalAPI.IsSpotifyRunning())
                    {
                        return;
                    }
                    if (!SpotifyLocalAPI.IsSpotifyWebHelperRunning())
                    {
                        return;
                    }

                    var currentStatus = _spotify.GetStatus();
                    StringBuilder result = new StringBuilder();

                    GetSpotifyInfo(result, currentStatus);

                    if (currentStatus.Playing)
                    {
                        RivaTuner.print(currentStatus.Track.TrackResource.Name + " " + currentStatus.Track.ArtistResource.Name + currentStatus.Track.Length);
                    }

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

        private void GetSpotifyInfo(StringBuilder result, StatusResponse currentStatus)
        {
            foreach (var item in App.appSettings.PropertiesOrder)
            {
                switch (item)
                {
                    case "Album":
                        result.Append(item);
                        break;
                    case "Artist":
                        result.Append(item);
                        break;
                    case "SongTime":
                        break;
                    case "Space":
                        result.Append(" ");
                        break;
                    case ":":
                        result.Append(":");
                        break;
                    case "Label":
                        result.Append("Song: ");
                        break;
                    default:
                        break;
                }
                if (App.appSettings.UseNewLine)
                {
                    result.AppendLine();
                }
            }
        }
    }
}
