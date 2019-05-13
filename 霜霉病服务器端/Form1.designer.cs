namespace _19server
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnStart = new System.Windows.Forms.Button();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnSendFile = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnZD = new System.Windows.Forms.Button();
            this.cboUsers = new System.Windows.Forms.ComboBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.lbOnline = new System.Windows.Forms.ListView();
            this.btnSendToAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(287, 29);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "开始监听";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(185, 29);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(75, 21);
            this.txtPort.TabIndex = 1;
            this.txtPort.Text = "23456";
            this.txtPort.TextChanged += new System.EventHandler(this.txtPort_TextChanged);
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(34, 29);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(145, 21);
            this.txtServer.TabIndex = 2;
            this.txtServer.Text = "203.195.204.153";
            this.txtServer.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.Lavender;
            this.txtLog.Location = new System.Drawing.Point(34, 82);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(518, 78);
            this.txtLog.TabIndex = 3;
            // 
            // txtMsg
            // 
            this.txtMsg.BackColor = System.Drawing.Color.Lavender;
            this.txtMsg.Location = new System.Drawing.Point(34, 164);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(406, 32);
            this.txtMsg.TabIndex = 4;
            this.txtMsg.TextChanged += new System.EventHandler(this.txtMsg_TextChanged);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(12, 378);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(471, 21);
            this.txtPath.TabIndex = 5;
            // 
            // btnSendFile
            // 
            this.btnSendFile.Location = new System.Drawing.Point(537, 350);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(75, 23);
            this.btnSendFile.TabIndex = 7;
            this.btnSendFile.Text = "发送文件";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnSend.Location = new System.Drawing.Point(477, 173);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 8;
            this.btnSend.Text = "发送消息";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnZD
            // 
            this.btnZD.Location = new System.Drawing.Point(737, 296);
            this.btnZD.Name = "btnZD";
            this.btnZD.Size = new System.Drawing.Size(75, 23);
            this.btnZD.TabIndex = 9;
            this.btnZD.Text = "震动";
            this.btnZD.UseVisualStyleBackColor = true;
            this.btnZD.Click += new System.EventHandler(this.btnZD_Click);
            // 
            // cboUsers
            // 
            this.cboUsers.FormattingEnabled = true;
            this.cboUsers.Location = new System.Drawing.Point(428, 379);
            this.cboUsers.Name = "cboUsers";
            this.cboUsers.Size = new System.Drawing.Size(197, 20);
            this.cboUsers.TabIndex = 11;
            this.cboUsers.SelectedIndexChanged += new System.EventHandler(this.cboUsers_SelectedIndexChanged);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(618, 350);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 12;
            this.btnSelect.Text = "选择";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // lbOnline
            // 
            this.lbOnline.Location = new System.Drawing.Point(381, 14);
            this.lbOnline.Name = "lbOnline";
            this.lbOnline.Size = new System.Drawing.Size(171, 62);
            this.lbOnline.TabIndex = 13;
            this.lbOnline.UseCompatibleStateImageBehavior = false;
            this.lbOnline.SelectedIndexChanged += new System.EventHandler(this.lbOnline_SelectedIndexChanged);
            // 
            // btnSendToAll
            // 
            this.btnSendToAll.Location = new System.Drawing.Point(715, 171);
            this.btnSendToAll.Name = "btnSendToAll";
            this.btnSendToAll.Size = new System.Drawing.Size(75, 23);
            this.btnSendToAll.TabIndex = 14;
            this.btnSendToAll.Text = "群发消息";
            this.btnSendToAll.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(572, 282);
            this.Controls.Add(this.btnSendToAll);
            this.Controls.Add(this.lbOnline);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.cboUsers);
            this.Controls.Add(this.btnZD);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnSendFile);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.btnStart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "葡萄霜霉病防控系统服务端";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnZD;
        private System.Windows.Forms.ComboBox cboUsers;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.ListView lbOnline;
        private System.Windows.Forms.Button btnSendToAll;
    }
}

