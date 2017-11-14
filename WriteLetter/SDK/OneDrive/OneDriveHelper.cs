using Microsoft.Graph;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.OneDrive.Sdk;
using Microsoft.OneDrive.Sdk.Authentication;
using System;
using Windows.Security.Authentication.Web;
using Windows.Security.Authentication.Web.Core;
using Windows.Security.Credentials;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using System.IO;

namespace AppCore.SDK.OneDrive
{
    public class OneDriveHelper
    {

        private static object syncRoot = new object();

        private static OneDriveHelper instance;

        public static OneDriveHelper Instance
        {
            get
            {
                if (instance == null)
                    lock(syncRoot)
                    {
                        if(instance == null)
                            instance = new OneDriveHelper();
                    }                   
                return instance;
            }
        }

        public event EventHandler OneDriveStateChanged;

        #region OneDrive
        public IOneDriveClient OneDriveClient { get; set; }

        public IAuthenticationProvider AuthProvider { get; set; }

        // Set these values to your app's ID and return URL.
        private readonly string oneDriveForBusinessClientId = "00000000401CCC5B";
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

                try
                {
                    await authTask;
                    //app.NavigationStack.Add(new ItemModel(new Item()));
                    //this.Frame.Navigate(typeof(MainPage), e);
                }
                catch (ServiceException exception)
                {
                    // Swallow the auth exception but write message for debugging.
                    Debug.WriteLine(exception.Error.Message);
                }
            }
            else
            {
                //this.Frame.Navigate(typeof(MainPage), e);
            }
        }
        #endregion

        #region Function

        private async Task<Item> GetItemById(string itemId)
        {
            if (OneDriveClient == null)
                return null;
            var item = await OneDriveClient
                     .Drive
                     .Items[itemId]
                     .Request()
                     .GetAsync();
            return item;
        }

        private async Task<Item> GetItemByPath(string itemPath)
        {
            if (OneDriveClient == null)
                return null;
            var item = await OneDriveClient
                     .Drive
                     .Root
                     .ItemWithPath(itemPath)
                     .Request()
                     .GetAsync();
            return item;
        }

        private async void DeleteItemById(string itemId)
        {
            if (OneDriveClient == null)
                return;
            await OneDriveClient
              .Drive
              .Items[itemId]
              .Request()
              .DeleteAsync();
        }

        private async Task<Stream> DownloadItemById(string itemId)
        {
            var stream = await OneDriveClient
                              .Drive
                              .Items[itemId]
                              .Content
                              .Request()
                              .GetAsync();
            return stream;
        }

        private async Task UploadItem(Stream stream,string itemPath)
        {
            using (stream)
            {
                var uploadedItem = await OneDriveClient
                                           .Drive
                                           .Root
                                           .ItemWithPath(itemPath)
                                           .Content
                                           .Request()
                                           .PutAsync<Item>(stream);
            }
        }

        public bool IsAuthed()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveFile()
        {
            throw new NotImplementedException();
            return true;
        }

        public async Task<bool> UpLoadFile(Stream stream,string path)
        {
            try
            {
                await UploadItem(stream,path);
            }
            catch (Exception)
            {
                return false;
            }            
            return true;
        }

        public async Task<Stream> DownLoadFile(string id)
        {
            return await DownloadItemById(id);
        }

        public async Task<bool> ReadFile()
        {
            throw new NotImplementedException();
            return true;
        }

        #endregion
    }

}
