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
			System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup("在线", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewGroup listViewGroup6 = new System.Windows.Forms.ListViewGroup("离线", System.Windows.Forms.HorizontalAlignment.Left);
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
			this.btnSelect = new System.Windows.Forms.Button();
			this.lbOnline = new System.Windows.Forms.ListView();
			this.id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ip = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.threadNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.trueName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.telephone = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.company = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.groupBoxCommunicate = new System.Windows.Forms.GroupBox();
			this.lbOnlineNumber = new System.Windows.Forms.Label();
			this.lbCurrentOnline = new System.Windows.Forms.Label();
			this.groupBoxLog = new System.Windows.Forms.GroupBox();
			this.groupBoxCommunicate.SuspendLayout();
			this.groupBoxLog.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(390, 29);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(75, 23);
			this.btnStart.TabIndex = 0;
			this.btnStart.Text = "开始监听";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// txtPort
			// 
			this.txtPort.Location = new System.Drawing.Point(265, 29);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(75, 21);
			this.txtPort.TabIndex = 1;
			this.txtPort.Text = "23456";
			this.txtPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ipPort_KeyPress);
			// 
			// txtServer
			// 
			this.txtServer.Location = new System.Drawing.Point(100, 29);
			this.txtServer.Name = "txtServer";
			this.txtServer.Size = new System.Drawing.Size(145, 21);
			this.txtServer.TabIndex = 2;
			this.txtServer.Text = "39.107.95.53";
			this.txtServer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ipPort_KeyPress);
			// 
			// txtLog
			// 
			this.txtLog.BackColor = System.Drawing.Color.Lavender;
			this.txtLog.Location = new System.Drawing.Point(8, 20);
			this.txtLog.Multiline = true;
			this.txtLog.Name = "txtLog";
			this.txtLog.Size = new System.Drawing.Size(586, 77);
			this.txtLog.TabIndex = 3;
			// 
			// txtMsg
			// 
			this.txtMsg.BackColor = System.Drawing.SystemColors.Window;
			this.txtMsg.Location = new System.Drawing.Point(8, 332);
			this.txtMsg.Multiline = true;
			this.txtMsg.Name = "txtMsg";
			this.txtMsg.Size = new System.Drawing.Size(415, 51);
			this.txtMsg.TabIndex = 4;
			// 
			// txtPath
			// 
			this.txtPath.Location = new System.Drawing.Point(8, 399);
			this.txtPath.Name = "txtPath";
			this.txtPath.Size = new System.Drawing.Size(336, 21);
			this.txtPath.TabIndex = 5;
			// 
			// btnSendFile
			// 
			this.btnSendFile.Location = new System.Drawing.Point(427, 399);
			this.btnSendFile.Name = "btnSendFile";
			this.btnSendFile.Size = new System.Drawing.Size(75, 23);
			this.btnSendFile.TabIndex = 7;
			this.btnSendFile.Text = "发送";
			this.btnSendFile.UseVisualStyleBackColor = true;
			this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
			// 
			// btnSend
			// 
			this.btnSend.BackColor = System.Drawing.Color.Transparent;
			this.btnSend.Location = new System.Drawing.Point(429, 360);
			this.btnSend.Name = "btnSend";
			this.btnSend.Size = new System.Drawing.Size(75, 23);
			this.btnSend.TabIndex = 8;
			this.btnSend.Text = "发送消息";
			this.btnSend.UseVisualStyleBackColor = false;
			this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
			// 
			// btnZD
			// 
			this.btnZD.Location = new System.Drawing.Point(427, 332);
			this.btnZD.Name = "btnZD";
			this.btnZD.Size = new System.Drawing.Size(77, 23);
			this.btnZD.TabIndex = 9;
			this.btnZD.Text = "震动";
			this.btnZD.UseVisualStyleBackColor = true;
			this.btnZD.Click += new System.EventHandler(this.btnZD_Click);
			// 
			// btnSelect
			// 
			this.btnSelect.Location = new System.Drawing.Point(348, 399);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.Size = new System.Drawing.Size(73, 21);
			this.btnSelect.TabIndex = 12;
			this.btnSelect.Text = "选择文件";
			this.btnSelect.UseVisualStyleBackColor = true;
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			// 
			// lbOnline
			// 
			this.lbOnline.CheckBoxes = true;
			this.lbOnline.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.id,
            this.name,
            this.ip,
            this.threadNo,
            this.trueName,
            this.telephone,
            this.company});
			this.lbOnline.Cursor = System.Windows.Forms.Cursors.Default;
			listViewGroup5.Header = "在线";
			listViewGroup5.Name = "online";
			listViewGroup5.Tag = "online";
			listViewGroup6.Header = "离线";
			listViewGroup6.Name = "offline";
			listViewGroup6.Tag = "offline";
			this.lbOnline.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup5,
            listViewGroup6});
			this.lbOnline.Location = new System.Drawing.Point(6, 32);
			this.lbOnline.Name = "lbOnline";
			this.lbOnline.Size = new System.Drawing.Size(588, 285);
			this.lbOnline.TabIndex = 13;
			this.lbOnline.UseCompatibleStateImageBehavior = false;
			this.lbOnline.View = System.Windows.Forms.View.Details;
			// 
			// id
			// 
			this.id.Text = "编号";
			this.id.Width = 42;
			// 
			// name
			// 
			this.name.Text = "用户名";
			// 
			// ip
			// 
			this.ip.Text = "ip地址";
			this.ip.Width = 117;
			// 
			// threadNo
			// 
			this.threadNo.Text = "线程号";
			this.threadNo.Width = 53;
			// 
			// trueName
			// 
			this.trueName.Text = "真实姓名";
			this.trueName.Width = 62;
			// 
			// telephone
			// 
			this.telephone.Text = "手机号";
			this.telephone.Width = 87;
			// 
			// company
			// 
			this.company.Text = "工作单位";
			this.company.Width = 153;
			// 
			// groupBoxCommunicate
			// 
			this.groupBoxCommunicate.BackColor = System.Drawing.Color.Transparent;
			this.groupBoxCommunicate.Controls.Add(this.btnZD);
			this.groupBoxCommunicate.Controls.Add(this.btnSelect);
			this.groupBoxCommunicate.Controls.Add(this.btnSendFile);
			this.groupBoxCommunicate.Controls.Add(this.lbOnline);
			this.groupBoxCommunicate.Controls.Add(this.lbOnlineNumber);
			this.groupBoxCommunicate.Controls.Add(this.lbCurrentOnline);
			this.groupBoxCommunicate.Controls.Add(this.txtMsg);
			this.groupBoxCommunicate.Controls.Add(this.txtPath);
			this.groupBoxCommunicate.Controls.Add(this.btnSend);
			this.groupBoxCommunicate.Location = new System.Drawing.Point(100, 168);
			this.groupBoxCommunicate.Name = "groupBoxCommunicate";
			this.groupBoxCommunicate.Size = new System.Drawing.Size(600, 438);
			this.groupBoxCommunicate.TabIndex = 15;
			this.groupBoxCommunicate.TabStop = false;
			this.groupBoxCommunicate.Text = "客户端通信";
			// 
			// lbOnlineNumber
			// 
			this.lbOnlineNumber.AutoSize = true;
			this.lbOnlineNumber.Location = new System.Drawing.Point(101, 17);
			this.lbOnlineNumber.Name = "lbOnlineNumber";
			this.lbOnlineNumber.Size = new System.Drawing.Size(11, 12);
			this.lbOnlineNumber.TabIndex = 13;
			this.lbOnlineNumber.Text = "0";
			// 
			// lbCurrentOnline
			// 
			this.lbCurrentOnline.AutoSize = true;
			this.lbCurrentOnline.Location = new System.Drawing.Point(6, 17);
			this.lbCurrentOnline.Name = "lbCurrentOnline";
			this.lbCurrentOnline.Size = new System.Drawing.Size(89, 12);
			this.lbCurrentOnline.TabIndex = 12;
			this.lbCurrentOnline.Text = "当前在线人数：";
			// 
			// groupBoxLog
			// 
			this.groupBoxLog.BackColor = System.Drawing.Color.Transparent;
			this.groupBoxLog.Controls.Add(this.txtLog);
			this.groupBoxLog.Location = new System.Drawing.Point(100, 58);
			this.groupBoxLog.Name = "groupBoxLog";
			this.groupBoxLog.Size = new System.Drawing.Size(600, 103);
			this.groupBoxLog.TabIndex = 16;
			this.groupBoxLog.TabStop = false;
			this.groupBoxLog.Text = "系统日志";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
			this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.BackgroundImage = global::霜霉病服务器端.Properties.Resources.server;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(784, 618);
			this.Controls.Add(this.txtServer);
			this.Controls.Add(this.txtPort);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.groupBoxCommunicate);
			this.Controls.Add(this.groupBoxLog);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "葡萄霜霉病防控系统服务端";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.groupBoxCommunicate.ResumeLayout(false);
			this.groupBoxCommunicate.PerformLayout();
			this.groupBoxLog.ResumeLayout(false);
			this.groupBoxLog.PerformLayout();
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
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.ListView lbOnline;
		private System.Windows.Forms.GroupBox groupBoxCommunicate;
		private System.Windows.Forms.ColumnHeader id;
		private System.Windows.Forms.Label lbCurrentOnline;
		private System.Windows.Forms.Label lbOnlineNumber;
		private System.Windows.Forms.ColumnHeader name;
		private System.Windows.Forms.GroupBox groupBoxLog;
		private System.Windows.Forms.ColumnHeader ip;
		private System.Windows.Forms.ColumnHeader threadNo;
		private System.Windows.Forms.ColumnHeader trueName;
		private System.Windows.Forms.ColumnHeader telephone;
		private System.Windows.Forms.ColumnHeader company;
	}
}

