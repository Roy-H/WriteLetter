using Microsoft.Graph;
using Microsoft.OneDrive.Sdk;
using Microsoft.OneDrive.Sdk.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace WriteLetter.SDK.OneDrive
{
    public class OneDriveHelper
    {
        private static OneDriveHelper instance;
        public static OneDriveHelper Instance
        {
            get
            {
                if (instance == null)
                    instance = new OneDriveHelper();
                return instance;
            }
        }

        #region OneDrive
        public IOneDriveClient OneDriveClient { get; set; }

        public IAuthenticationProvider AuthProvider { get; set; }

        // Set these values to your app's ID and return URL.
        private readonly string oneDriveForBusinessClientId = "Insert your OneDrive for Business client id";
        private readonly string oneDriveForBusinessReturnUrl = "http://localhost:8080";
        private readonly string oneDriveForBusinessBaseUrl = "https://graph.microsoft.com/";

        private readonly string oneDriveConsumerClientId = "00000000401CCC5B";
        private readonly string oneDriveConsumerReturnUrl = "https://login.live.com/oauth20_desktop.srf";
        private readonly string oneDriveConsumerBaseUrl = "https://api.onedrive.com/v1.0";
        private readonly string[] scopes = new string[] { "onedrive.readonly", "wl.signin", "offline_access" };

        public enum ClientType
        {
            Business,
            Consumer,
            ConsumerUwp
        }
        public async void InitializeClient(ClientType clientType)
        {
            var app = (App)Application.Current;
            if (OneDriveClient == null)
            {
                Task authTask;

                if (clientType == ClientType.Business)
                {
                    var adalAuthProvider = new AdalAuthenticationProvider(
                        this.oneDriveForBusinessClientId,
                        this.oneDriveForBusinessReturnUrl);
                    authTask = adalAuthProvider.AuthenticateUserAsync(this.oneDriveForBusinessBaseUrl);
                    OneDriveClient = new OneDriveClient(oneDriveForBusinessBaseUrl + "/_api/v2.0", adalAuthProvider);
                    AuthProvider = adalAuthProvider;
                }
                else if (clientType == ClientType.ConsumerUwp)
                {
                    var onlineIdAuthProvider = new OnlineIdAuthenticationProvider(
                        this.scopes);
                    authTask = onlineIdAuthProvider.RestoreMostRecentFromCacheOrAuthenticateUserAsync();
                    OneDriveClient = new OneDriveClient(this.oneDriveConsumerBaseUrl, onlineIdAuthProvider);
                    AuthProvider = onlineIdAuthProvider;
                }
                else
                {
                    var msaAuthProvider = new MsaAuthenticationProvider(
                        this.oneDriveConsumerClientId,
                        this.oneDriveConsumerReturnUrl,
                        this.scopes,
                        new CredentialVault(this.oneDriveConsumerClientId));
                    authTask = msaAuthProvider.RestoreMostRecentFromCacheOrAuthenticateUserAsync();
                    OneDriveClient = new OneDriveClient(this.oneDriveConsumerBaseUrl, msaAuthProvider);
                    AuthProvider = msaAuthProvider;
                }

                //try
                //{
                //    await authTask;
                //    app.NavigationStack.Add(new ItemModel(new Item()));
                //    this.Frame.Navigate(typeof(MainPage), e);
                //}
                //catch (ServiceException exception)
                //{
                //    // Swallow the auth exception but write message for debugging.
                //    Debug.WriteLine(exception.Error.Message);
                //}
            }
            else
            {
                //this.Frame.Navigate(typeof(MainPage), e);
            }
        }
        #endregion
    }
}
