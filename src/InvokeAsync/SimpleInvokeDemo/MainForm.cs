using SimpleInvokeDemo.Data;
using System;
using System.ComponentModel;
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

        private void BtnStartBlockingTask_Click(object sender, EventArgs e)
        {
            // Simulate getting the Data from a Report engine, and displaying them in the Grid via Binding.
            var bindingList = new BindingList<CustomerReportItem>();
            customerReportItemBindingSource.DataSource = bindingList;

            for (var i = 0; i < 30; i++)
            {
                CustomerReportItem.AddCustomerReportToBindingList(bindingList, i);
            }
        }

        private async void BtnUnblockingByAsync1_Click(object sender, EventArgs e)
        {
            // Simulate getting the Data from a Report engine, and displaying them in the Grid via Binding.
            var bindingList = new BindingList<CustomerReportItem>();
            customerReportItemBindingSource.DataSource = bindingList;

            await Task.Run(() =>
            {
                for (var i = 0; i < 30; i++)
                {
                    // Doesn't work: Cross-Thread Exception.
                    // CustomerReportItem.AddCustomerReportToBindingList(bindingList, i);
                    asyncControl.Invoke((MethodInvoker)delegate 
                    { 
                        CustomerReportItem.AddCustomerReportToBindingList(bindingList, i); 
                    });
                }
            });
        }

        private async void BtnUnblockingByAsync2_Click(object sender, EventArgs e)
        {
            var bindingList = new BindingList<CustomerReportItem>();
            customerReportItemBindingSource.DataSource = bindingList;

            await Task.Run(async () =>
            {
                for (var i = 0; i < 30; i++)
                {
                    // Doesn't work: Crashes.
                    // await CustomerReportItem.AddCustomerReportToBindingListAsync(bindingList, i);
                    await asyncControl.InvokeAsync(()=>CustomerReportItem.AddCustomerReportToBindingListAsync(bindingList, i));
                }
            });
        }

        private async void BtnKickOfAsyncWork_Click(object sender, EventArgs e)
        {
            var someVeryComplexToCalculateResult = await Task.Run(async () =>
            {
                int result = 0;

                for (var i = 0; i < 5; i++)
                {
                    Thread.SpinWait(100000);
                    result += await asyncControl.InvokeAsync(async () => await DoSomeAsyncWorkOnTheUiThread(i));
                    await Task.Delay(100);
                }

                return result;
            });
        }

        private int DoSomeNonAsyncWorkOnTheUiThread(int valueToPrint)
        {
            Thread.Sleep(1000);
            //lblCount.Text = valueToPrint.ToString();
            Thread.Sleep(1000);
            return 42;
        }

        private async Task<int> DoSomeAsyncWorkOnTheUiThread(int valueToPrint)
        {
            await Task.Delay(1000);
            //lblCount.Text = valueToPrint.ToString();
            await Task.Delay(1000);
            return 42;
        }
    }
}
