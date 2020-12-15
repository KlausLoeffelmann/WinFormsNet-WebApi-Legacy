using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleInvokeDemo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private async void BtnKickOfAsyncWork_Click(object sender, EventArgs e)
        {
            await DoComputeIntensiveWork();
        }

        private async Task<int> DoComputeIntensiveWork()
        {
            return await Task.Run(async () =>
            {
                int result = 0;

                for (var i = 0; i < 5; i++)
                {
                    Thread.SpinWait(100000);
                    result += await customControl1.InvokeAsync(async () => await DoSomeAsyncWorkOnTheUiThread(i));
                    //result += await customControl1.InvokeAsync(() => DoSomeNonAsyncWorkOnTheUiThread(i));
                    await Task.Delay(100);
                }

                return result;
            });
        }

        private int DoSomeNonAsyncWorkOnTheUiThread(int valueToPrint)
        {
            Thread.Sleep(1000);
            lblCount.Text = valueToPrint.ToString();
            Thread.Sleep(1000);
            return 42;
        }

        private async Task<int> DoSomeAsyncWorkOnTheUiThread(int valueToPrint)
        {
            await Task.Delay(1000);
            lblCount.Text = valueToPrint.ToString();
            await Task.Delay(1000);
            return 42;
        }
    }
}
