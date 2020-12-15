using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;
using System;
using System.Threading.Tasks;
using System.Net;

namespace BlackBoardWinForms
{
    public static class WebView2Extension
    {
        public async static Task<Uri> NavigateToAsync(this WebView2 webView2Control, string navigateToUri)
        {
            var tsc = new TaskCompletionSource<Uri>();

            EventHandler<CoreWebView2NavigationCompletedEventArgs> handler = (s, e) =>
            {
                if (e.IsSuccess)
                {
                    tsc.TrySetResult(webView2Control.Source);
                }
                else
                {
                    var wes = e.WebErrorStatus switch
                    {
                        CoreWebView2WebErrorStatus.Timeout => WebExceptionStatus.Timeout,
                        CoreWebView2WebErrorStatus.HostNameNotResolved => WebExceptionStatus.NameResolutionFailure,
                        CoreWebView2WebErrorStatus.ConnectionReset => WebExceptionStatus.ConnectFailure,
                        CoreWebView2WebErrorStatus.ConnectionAborted => WebExceptionStatus.ConnectionClosed,
                        _ => WebExceptionStatus.UnknownError
                    };

                    throw new WebException("NavigateToAsync returned an error:", wes);
                }
            };

            try
            {
                webView2Control.NavigationCompleted += handler;
                webView2Control.CoreWebView2.Navigate(navigateToUri);
                return await tsc.Task;
            }
            finally
            {
                webView2Control.NavigationCompleted -= handler;
            }
        }

        public async static Task<Uri> NavigateToAsync(
            this WebView2 webView2Control,
            string navigateToUri,
            Func<CoreWebView2NavigationCompletedEventArgs, string, bool> navigationCompleted)
        {
            var tsc = new TaskCompletionSource<Uri>();

            EventHandler<CoreWebView2NavigationCompletedEventArgs> handler = (s, e) =>
            {
                if (navigationCompleted(e, webView2Control.Source.ToString()))
                {
                    tsc.TrySetResult(webView2Control.Source);
                }
            };

            try
            {
                webView2Control.NavigationCompleted += handler;
                webView2Control.CoreWebView2.Navigate(navigateToUri);
                return await tsc.Task;
            }
            finally
            {
                webView2Control.NavigationCompleted -= handler;
            }
        }
    }
}
