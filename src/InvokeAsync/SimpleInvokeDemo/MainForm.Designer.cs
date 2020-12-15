
namespace SimpleInvokeDemo
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.customControl1 = new System.Windows.Forms.AsyncControl();
            this.lblCount = new System.Windows.Forms.Label();
            this.btnKickOfAsyncWork = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // customControl1
            // 
            this.customControl1.Location = new System.Drawing.Point(167, 48);
            this.customControl1.Name = "customControl1";
            this.customControl1.Size = new System.Drawing.Size(207, 34);
            this.customControl1.TabIndex = 0;
            this.customControl1.Text = "customControl1";
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(364, 25);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(50, 20);
            this.lblCount.TabIndex = 1;
            this.lblCount.Text = "label1";
            // 
            // btnKickOfAsyncWork
            // 
            this.btnKickOfAsyncWork.Location = new System.Drawing.Point(314, 276);
            this.btnKickOfAsyncWork.Name = "btnKickOfAsyncWork";
            this.btnKickOfAsyncWork.Size = new System.Drawing.Size(170, 38);
            this.btnKickOfAsyncWork.TabIndex = 2;
            this.btnKickOfAsyncWork.Text = "Kick of Async Work";
            this.btnKickOfAsyncWork.UseVisualStyleBackColor = true;
            this.btnKickOfAsyncWork.Click += new System.EventHandler(this.BtnKickOfAsyncWork_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnKickOfAsyncWork);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.customControl1);
            this.Name = "MainForm";
            this.Text = "Simple Async Invoke Demo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.AsyncControl customControl1;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Button btnKickOfAsyncWork;
    }
}

