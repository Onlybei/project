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
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("客户端");
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
			this.colClient = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.btnSendToAll = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnSendAllOnline = new System.Windows.Forms.Button();
			this.lbOnlineNumber = new System.Windows.Forms.Label();
			this.lbCurrentOnline = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(346, 29);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(75, 23);
			this.btnStart.TabIndex = 0;
			this.btnStart.Text = "开始监听";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			this.btnStart.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BtnStart_KeyPress);
			// 
			// txtPort
			// 
			this.txtPort.Location = new System.Drawing.Point(244, 29);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(75, 21);
			this.txtPort.TabIndex = 1;
			this.txtPort.Text = "23456";
			this.txtPort.TextChanged += new System.EventHandler(this.txtPort_TextChanged);
			// 
			// txtServer
			// 
			this.txtServer.Location = new System.Drawing.Point(93, 29);
			this.txtServer.Name = "txtServer";
			this.txtServer.Size = new System.Drawing.Size(145, 21);
			this.txtServer.TabIndex = 2;
			this.txtServer.Text = "39.107.95.53";
			this.txtServer.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// txtLog
			// 
			this.txtLog.BackColor = System.Drawing.Color.Lavender;
			this.txtLog.Location = new System.Drawing.Point(93, 82);
			this.txtLog.Multiline = true;
			this.txtLog.Name = "txtLog";
			this.txtLog.Size = new System.Drawing.Size(518, 78);
			this.txtLog.TabIndex = 3;
			// 
			// txtMsg
			// 
			this.txtMsg.BackColor = System.Drawing.SystemColors.Window;
			this.txtMsg.Location = new System.Drawing.Point(6, 52);
			this.txtMsg.Multiline = true;
			this.txtMsg.Name = "txtMsg";
			this.txtMsg.Size = new System.Drawing.Size(415, 51);
			this.txtMsg.TabIndex = 4;
			// 
			// txtPath
			// 
			this.txtPath.Location = new System.Drawing.Point(6, 130);
			this.txtPath.Name = "txtPath";
			this.txtPath.Size = new System.Drawing.Size(350, 21);
			this.txtPath.TabIndex = 5;
			// 
			// btnSendFile
			// 
			this.btnSendFile.Location = new System.Drawing.Point(536, 297);
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
			this.btnSend.Location = new System.Drawing.Point(427, 80);
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
			this.cboUsers.Location = new System.Drawing.Point(6, 20);
			this.cboUsers.Name = "cboUsers";
			this.cboUsers.Size = new System.Drawing.Size(197, 20);
			this.cboUsers.TabIndex = 11;
			this.cboUsers.Text = "请选择客户端";
			this.cboUsers.SelectedIndexChanged += new System.EventHandler(this.cboUsers_SelectedIndexChanged);
			// 
			// btnSelect
			// 
			this.btnSelect.Location = new System.Drawing.Point(455, 297);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.Size = new System.Drawing.Size(75, 23);
			this.btnSelect.TabIndex = 12;
			this.btnSelect.Text = "选择";
			this.btnSelect.UseVisualStyleBackColor = true;
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			// 
			// lbOnline
			// 
			this.lbOnline.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colClient});
			this.lbOnline.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
			this.lbOnline.Location = new System.Drawing.Point(440, 14);
			this.lbOnline.Name = "lbOnline";
			this.lbOnline.Size = new System.Drawing.Size(171, 62);
			this.lbOnline.TabIndex = 13;
			this.lbOnline.UseCompatibleStateImageBehavior = false;
			this.lbOnline.SelectedIndexChanged += new System.EventHandler(this.lbOnline_SelectedIndexChanged);
			// 
			// colClient
			// 
			this.colClient.Text = "Client";
			this.colClient.Width = 171;
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
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.Color.Transparent;
			this.groupBox1.Controls.Add(this.btnSendAllOnline);
			this.groupBox1.Controls.Add(this.lbOnlineNumber);
			this.groupBox1.Controls.Add(this.lbCurrentOnline);
			this.groupBox1.Controls.Add(this.cboUsers);
			this.groupBox1.Controls.Add(this.txtMsg);
			this.groupBox1.Controls.Add(this.txtPath);
			this.groupBox1.Controls.Add(this.btnSend);
			this.groupBox1.Location = new System.Drawing.Point(93, 167);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(518, 168);
			this.groupBox1.TabIndex = 15;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "客户端通信";
			// 
			// btnSendAllOnline
			// 
			this.btnSendAllOnline.Location = new System.Drawing.Point(362, 17);
			this.btnSendAllOnline.Name = "btnSendAllOnline";
			this.btnSendAllOnline.Size = new System.Drawing.Size(75, 23);
			this.btnSendAllOnline.TabIndex = 14;
			this.btnSendAllOnline.Text = "群发消息";
			this.btnSendAllOnline.UseVisualStyleBackColor = true;
			this.btnSendAllOnline.Click += new System.EventHandler(this.BtnSendAllOnline_Click);
			this.btnSendAllOnline.MouseHover += new System.EventHandler(this.BtnSendAllOnline_MouseHover);
			// 
			// lbOnlineNumber
			// 
			this.lbOnlineNumber.AutoSize = true;
			this.lbOnlineNumber.Location = new System.Drawing.Point(315, 23);
			this.lbOnlineNumber.Name = "lbOnlineNumber";
			this.lbOnlineNumber.Size = new System.Drawing.Size(11, 12);
			this.lbOnlineNumber.TabIndex = 13;
			this.lbOnlineNumber.Text = "1";
			// 
			// lbCurrentOnline
			// 
			this.lbCurrentOnline.AutoSize = true;
			this.lbCurrentOnline.Location = new System.Drawing.Point(228, 23);
			this.lbCurrentOnline.Name = "lbCurrentOnline";
			this.lbCurrentOnline.Size = new System.Drawing.Size(89, 12);
			this.lbCurrentOnline.TabIndex = 12;
			this.lbCurrentOnline.Text = "当前在线人数：";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
			this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(711, 438);
			this.Controls.Add(this.btnSendToAll);
			this.Controls.Add(this.lbOnline);
			this.Controls.Add(this.btnSelect);
			this.Controls.Add(this.btnZD);
			this.Controls.Add(this.btnSendFile);
			this.Controls.Add(this.txtLog);
			this.Controls.Add(this.txtServer);
			this.Controls.Add(this.txtPort);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.groupBox1);
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "葡萄霜霉病防控系统服务端";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
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
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ColumnHeader colClient;
		private System.Windows.Forms.Label lbCurrentOnline;
		private System.Windows.Forms.Button btnSendAllOnline;
		private System.Windows.Forms.Label lbOnlineNumber;
	}
}

