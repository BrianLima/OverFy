using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OverFy
{
    /// <summary>
    /// Interaction logic for AuthorizeSpotify.xaml
    /// </summary>
    public partial class AuthorizeSpotify : Window
    {
        public AuthorizeSpotify()
        {
            InitializeComponent();
        }

        public AuthorizationCodeAuth _auth { get; private set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LaunchWebAuthentication();
        }

        private void LaunchWebAuthentication()
        {
            _auth = new AuthorizationCodeAuth(App.keys.ClientId, App.keys.SecretId, "http://localhost:4002", "http://localhost:4002", Scope.UserReadCurrentlyPlaying);

            _auth.AuthReceived += _auth_AuthReceived;
            _auth.Start();
            _auth.OpenBrowser();
        }

        private void _auth_AuthReceived(object sender, AuthorizationCode payload)
        {
            if (payload.Error != null)
            {
                AuthorizationCodeAuth auth = (AuthorizationCodeAuth)sender;
                auth.Stop();
                App.keys.u = payload.Code;
                Keys.SaveKeys(App.keys);
            }
            
        }
    }
}
