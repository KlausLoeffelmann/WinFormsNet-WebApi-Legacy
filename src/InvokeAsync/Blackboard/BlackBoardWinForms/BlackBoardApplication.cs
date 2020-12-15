using BlackBoardWebApi.Model;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensibility;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BlackBoardWinForms
{
    internal class BlackBoardApplication
    {
        // Client ID from the Azure Web Portal App Registration.
        private static string s_clientId = "963d0788-3eaf-4a00-b692-955e2f68de03";

        // 'common' - For Work, School and Microsoft personal accounts.
        // Can also be 'rganisations' (Work/School accounts),
        // 'consumers' (Personal MS accounts) or a particular tenent ID (GUID).
        private static readonly string s_tenant = "common";

        // Hostname for the Azure AD instance. {0} will be replaced by the value of ida:Tenant below
        // You can change this URL if you want your application to sign-in users from other clouds
        // than the Azure Global Cloud (See national clouds / sovereign clouds at https://aka.ms/aadv2-national-clouds)
        private static readonly string s_instance = "https://login.microsoftonline.com/{0}/v2.0";

        private static readonly string s_scope = $"{s_clientId}/access_as_user";
        private static readonly string s_authority = string.Format(CultureInfo.InvariantCulture, s_instance, s_tenant);

        // change this to the Azure Web App address for deployment.
        private static readonly string s_webApiBaseAddress = "https://localhost:44351";
        // private static readonly string s_webApiBaseAddress = "https://blackboardwebapi.azurewebsites.net";
        private static IPublicClientApplication s_clientApp;
        private static HttpClient HttpClient { get; } = new HttpClient();
        private static bool IsLoggedIn { get; set; }

        static BlackBoardApplication()
        {
            s_clientApp = PublicClientApplicationBuilder.Create(s_clientId)
                .WithAuthority(s_authority)
                .WithDefaultRedirectUri()
                .Build();

            TokenCacheHelper.EnableSerialization(s_clientApp.UserTokenCache);
        }

        internal static async Task ClearAccountsAsync(IEnumerable<IAccount> accounts)
        {
            foreach (var account in accounts)
            {
                await PublicClientApp.RemoveAsync(account);
            }
        }

        internal static async Task<AuthenticationResult> TryLoginAsync(frmWebLogin webLoginFormCreatedOnUIThread)
        {
            var accounts = (await PublicClientApp.GetAccountsAsync()).ToList();

            if (accounts?.Count > 0)
            {
                await ClearAccountsAsync(accounts);
            }

            AuthenticationResult authResult = null;

            try
            {
                authResult = await PublicClientApp
                    .AcquireTokenSilent(Scopes, accounts.FirstOrDefault())
                    .ExecuteAsync();
            }
            catch (MsalUiRequiredException)
            {
                try
                {
                    authResult = await PublicClientApp.AcquireTokenInteractive(BlackBoardApplication.Scopes)
                        .WithCustomWebUi(webLoginFormCreatedOnUIThread)
                        .ExecuteAsync();
                }
                catch (System.Exception)
                {
                    // TODO: Implement error handling.
                }
            }

            if (authResult.AccessToken != null)
            {
                // Once the token has been returned by MSAL, add it to the http authorization header, before making the call to access the To Do list service.
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
                IsLoggedIn = true;
            }
            else
            {
                IsLoggedIn = false;
            }

            return authResult;
        }

        public static async Task<UserLoginInfo> GetUserLoginInfoAsync()
        {
            if (IsLoggedIn)
            {
                HttpResponseMessage response = await HttpClient.GetAsync(UserLoginInfoApiAddress);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    var userLoginInfo = JsonConvert.DeserializeObject<UserLoginInfo>(s);

                    return userLoginInfo;
                }
            }

            return null;
        }

        public static IPublicClientApplication PublicClientApp => s_clientApp;
        public static string[] Scopes { get; } = new[] { s_scope };
        internal static string UserLoginInfoApiAddress => $"{s_webApiBaseAddress}/api/userlogininfo";
    }
}
