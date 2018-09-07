using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace OverFy
{
    public class SpotifyWorker : INotifyPropertyChanged
    {
        private Thread _workerThread;
        private CancellationTokenSource _cancellationTokenSource;
        private static AuthorizationCodeAuth _auth;
        private Keys keys;
        bool running = false;
        public bool spotifyHooked = false;
        short btcHitTimer = 100; //Count how many times the timer has ticked, when 100, allow downloading the current BTC Price
        string btcCache;

        private string _spotify_status;

        public string SpotifyStatus
        {
            get { return _spotify_status; }
            set { _spotify_status = value; }
        }

        private string _rtss_status;

        public string RTSSStatus
        {
            get { return _rtss_status; }
            private set { _rtss_status = value; }
        }

        private string _preview;

        public string Preview
        {
            get { return _preview; }
            set { _preview = value; NotifyPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public SpotifyWorker()
        {
            HookSpotify();
        }

        private void HookSpotify()
        {
            _spotify_status = "Not hooked";

            keys = Keys.LoadKeys();
            return;
            _auth = new AuthorizationCodeAuth(keys.ClientId, keys.SecretId, "http://localhost:4002", "http://localhost:4002", Scope.UserReadCurrentlyPlaying);

            _auth.AuthReceived += _auth_AuthReceived;
            _auth.Start();
            _auth.OpenBrowser();
            
            spotifyHooked = true;
            _spotify_status = "Ok";
        }

        private void _auth_AuthReceived(object sender, AuthorizationCode payload)
        {
            AuthorizationCodeAuth auth = (AuthorizationCodeAuth)sender;
        }

        public void Start()
        {
            if (_workerThread != null || running) return;

            _cancellationTokenSource = new CancellationTokenSource();
            _workerThread = new Thread(BackgroundWorker_DoWork)
            {
                Name = "OverFy",
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
            HookSpotify();
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    if (!spotifyHooked)
                    {
                    }

                    StringBuilder result = GetSpotifyInfo();

                    Preview = result.ToString();
                    try
                    {
                        if (RivaTuner.print(result.ToString()))
                        {
                            _rtss_status = "Ok";
                        }
                        else
                        {
                            _rtss_status = "Not Ok";
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    Task.Delay(500, cancellationToken).Wait(cancellationToken); //Wait half a second to write again
                }
                catch (OperationCanceledException)
                {
                    ClearScreen();
                    return;
                }
                catch (Exception ex)
                {
                    ClearScreen();
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

        private StringBuilder GetSpotifyInfo(/*StatusResponse currentStatus*/)
        {
            //StringBuilder result = new StringBuilder();
            //string lastItem = String.Empty;
            //bool skip = false;
            //bool addBtc = false;
            //string currency = "USD";

            //if (!SpotifyLocalAPI.IsSpotifyRunning() || !SpotifyLocalAPI.IsSpotifyWebHelperRunning() || currentStatus == null)
            //{
            //    skip = true;
            //    spotifyHooked = false;
            //}

            ////Check thelast item that isn`t written on a new row by default
            //for (int i = App.appSettings.PropertiesOrder.Count - 1; i > 0; i--)
            //{
            //    if (!App.appSettings.PropertiesOrder[i].Contains("System Time") && !App.appSettings.PropertiesOrder[i].StartsWith("BTC"))
            //    {
            //        lastItem = App.appSettings.PropertiesOrder[i];
            //        break;
            //    }
            //}

            //foreach (var item in App.appSettings.PropertiesOrder)
            //{
            //    if (item.Contains("BTC"))
            //    {
            //        currency = item.Split('/')[1];
            //        addBtc = true;
            //    }
            //}

            //if (!skip)
            //{
            //    foreach (var item in App.appSettings.PropertiesOrder)
            //    {
            //        switch (item)
            //        {
            //            case "System Time 12h":
            //                break;
            //            case "System Time 24h":
            //                break;
            //            case "Song Name":
            //                result.Append(NormalizeString(currentStatus.Track.TrackResource.Name));
            //                //We needed to normalize strings because some like ç can break rivatuner
            //                break;
            //            case "Artist Name":
            //                result.Append(NormalizeString(currentStatus.Track.ArtistResource.Name));
            //                break;
            //            case "Song Running Time":
            //                TimeSpan runningTime = TimeSpan.FromSeconds(currentStatus.PlayingPosition);
            //                TimeSpan totalTime = TimeSpan.FromSeconds(currentStatus.Track.Length);
            //                result.Append(runningTime.ToString(@"mm\:ss") + "/" + totalTime.ToString(@"mm\:ss"));
            //                break;
            //            case "Album Name":
            //                result.Append(NormalizeString(currentStatus.Track.AlbumResource.Name));
            //                break;
            //            case "Label":
            //                result.Append("Now Playing: ");
            //                break;
            //            case "New Line":
            //                result.Append(Environment.NewLine);
            //                break;
            //            default:
            //                if (!item.Contains("BTC"))
            //                {
            //                    result.Append(item);
            //                }
            //                break;
            //        }

            //        if (result.ToString() != String.Empty &&
            //            item != "Label" &&
            //            item != lastItem &&
            //            item != "System Time 24h" &&
            //            item != "System Time 12h" &&
            //            !item.Contains("BTC"))
            //        {
            //            result.Append(", ");
            //        }
            //    }
            //}

            //if (App.appSettings.PropertiesOrder.Contains("System Time 12h") || App.appSettings.PropertiesOrder.Contains("System Time 24h"))
            //{
            //    if (!String.IsNullOrEmpty(result.ToString()))
            //    {
            //        result.AppendLine();
            //    }

            //    if (App.appSettings.PropertiesOrder.Contains("System Time 24h"))
            //    {
            //        result.Append(DateTime.Now.ToString("HH:mm", CultureInfo.InvariantCulture));
            //    }
            //    else
            //    {
            //        result.Append(DateTime.Now.ToString("hh:mm tt", CultureInfo.InvariantCulture));
            //    }
            //}

            //if (addBtc )
            //{
            //    if (!String.IsNullOrEmpty(result.ToString()))
            //    {
            //        result.AppendLine();
            //    }
            //    if (btcHitTimer >= 100)
            //    {
            //        btcCache = BtcChecker.GetPrice(currency);
            //    }
            //    result.Append(btcCache);
            //}

            //if (btcHitTimer >= 100)
            //{
            //    btcHitTimer = 0;
            //}
            //else
            //{
            //    btcHitTimer++;
            //}
            return null;
            //return result;
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
