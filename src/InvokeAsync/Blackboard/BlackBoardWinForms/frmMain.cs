using Microsoft.Identity.Client;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackBoardWinForms
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var loginBrowser = new frmWebLogin();
                loginBrowser.Show();
                await loginBrowser.WaitForInitializeAsync();
                var result = await BlackBoardApplication.TryLoginAsync(loginBrowser);
                loginBrowser.Close();
                var userLoginInfo = await BlackBoardApplication.GetUserLoginInfoAsync();

                txtBlackboard.Text = userLoginInfo.Blackboard;
                lblLoginInfo.Text = $"{userLoginInfo.Name} - ({userLoginInfo.UserId})";
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
