using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InvokeAsync
{
    public partial class Form1 : Form
    {
        private int counter;

        public Form1()
        {
            InitializeComponent();
        }

        private async Task<int> DoComputeIntensiveWork()
        {
            return await Task.Run(async () =>
            {
                int result = 0;

                for (var i = 0; i < int.MaxValue; i++)
                {
                    Thread.SpinWait(100000);
                    result += await customControl1.InvokeAsync(() => DoSomeAsyncWorkOnTheUiThread(i));
                    result += await customControl1.InvokeAsync(() => DoSomeNonAsyncWorkOnTheUiThread(i));
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

        private async void button1_Click(object sender, EventArgs e)
        {
            await DoComputeIntensiveWork();
        }
    }
}
