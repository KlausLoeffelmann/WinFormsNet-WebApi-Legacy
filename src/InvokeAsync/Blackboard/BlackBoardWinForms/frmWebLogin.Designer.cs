
namespace BlackBoardWinForms
{
    partial class frmWebLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWebLogin));
            this.loginWebView = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsUriLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsSizeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsEndLoginButton = new System.Windows.Forms.ToolStripSplitButton();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // loginWebView
            // 
            this.loginWebView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.loginWebView.CreationProperties = null;
            this.loginWebView.Location = new System.Drawing.Point(12, 12);
            this.loginWebView.Name = "loginWebView";
            this.loginWebView.Size = new System.Drawing.Size(758, 688);
            this.loginWebView.TabIndex = 2;
            this.loginWebView.Text = "loginWebView";
            this.loginWebView.ZoomFactor = 1D;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsUriLabel,
            this.tsSizeLabel,
            this.tsEndLoginButton});
            this.statusStrip1.Location = new System.Drawing.Point(0, 703);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(782, 30);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsUriLabel
            // 
            this.tsUriLabel.AutoSize = false;
            this.tsUriLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tsUriLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tsUriLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsUriLabel.Name = "tsUriLabel";
            this.tsUriLabel.Size = new System.Drawing.Size(637, 24);
            this.tsUriLabel.Spring = true;
            this.tsUriLabel.Text = "Uri:";
            this.tsUriLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsSizeLabel
            // 
            this.tsSizeLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tsSizeLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tsSizeLabel.Name = "tsSizeLabel";
            this.tsSizeLabel.Size = new System.Drawing.Size(43, 24);
            this.tsSizeLabel.Text = "Size:";
            // 
            // tsEndLoginButton
            // 
            this.tsEndLoginButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsEndLoginButton.DropDownButtonWidth = 0;
            this.tsEndLoginButton.Image = ((System.Drawing.Image)(resources.GetObject("tsEndLoginButton.Image")));
            this.tsEndLoginButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsEndLoginButton.Name = "tsEndLoginButton";
            this.tsEndLoginButton.Padding = new System.Windows.Forms.Padding(2);
            this.tsEndLoginButton.Size = new System.Drawing.Size(87, 28);
            this.tsEndLoginButton.Text = "End Login.";
            // 
            // frmWebLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 733);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.loginWebView);
            this.Name = "frmWebLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Blackboard Login ";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 loginWebView;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsUriLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripStatusLabel tsSizeLabel;
        private System.Windows.Forms.ToolStripSplitButton tsEndLoginButton;
    }
}