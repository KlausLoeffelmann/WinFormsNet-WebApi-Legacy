using Microsoft.Identity.Client.Extensibility;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackBoardWinForms
{
    public partial class frmWebLogin : Form, ICustomWebUi
    {
        private TaskCompletionSource<Uri> _loginOverAwaiter = new TaskCompletionSource<Uri>();

        private object _browserInitializationAwaiterGuard = new object();
        private Uri _redirectUri;

        public frmWebLogin()
        {
            InitializeComponent();

            tsEndLoginButton.ButtonClick += TsEndLoginButton_ButtonClick;
            SizeChanged += FrmWebLogin_SizeChanged;
        }

        private void FrmWebLogin_SizeChanged(object sender, EventArgs e) 
            => tsSizeLabel.Text = $"Size: {Size}";

        //private void CoreWebView2_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        //{
        //    Debug.WriteLine($"NavigationCompleted-Success: {e.IsSuccess}");
        //    Debug.WriteLine($"NavigationCompleted-Navigation ID: {e.NavigationId}");

        //    tsUriLabel.Text = $"{loginWebView.CoreWebView2.Source}";
        //    tsUriLabel.ToolTipText = tsUriLabel.Text;

        //    if (loginWebView.CoreWebView2.Source.ToString().StartsWith(_redirectUri.ToString()))
        //    {
        //        _loginOverAwaiter.TrySetResult(loginWebView.Source);
        //    }
        //}

        public void NavigateTo(Uri uri)
        {
            loginWebView.CoreWebView2.Navigate(uri.ToString());
        }

        public async Task WaitForInitializeAsync()
        {
            try
            {
                await loginWebView.EnsureCoreWebView2Async(null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex}");
                throw;
            }
        }

        public async Task<Uri> AcquireAuthorizationCodeAsync(Uri authorizationUri, Uri redirectUri, CancellationToken cancellationToken)
        {
            _redirectUri = redirectUri;

            try
            {
                return await InvokeAsync(
                    async () => await loginWebView.NavigateToAsync(
                        authorizationUri.ToString(),
                        (e, uri) => uri.StartsWith(_redirectUri.ToString())),
                    cancellationToken);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message );
                // Todo: Handle the exception here
                // Log the exception or take appropriate action
                // Return a default value or rethrow the exception
                return null;
            }
        }

        private void TsEndLoginButton_ButtonClick(object sender, EventArgs e)
        {
            Close();
            _loginOverAwaiter.TrySetResult(loginWebView.Source);
        }
    }
}
