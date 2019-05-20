namespace 葡萄霜霉病防控测试系统1

{
	partial class Register
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Register));
			this.txtUsername = new System.Windows.Forms.TextBox();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.txtTrueName = new System.Windows.Forms.TextBox();
			this.txtTelephone = new System.Windows.Forms.TextBox();
			this.txtCompany = new System.Windows.Forms.TextBox();
			this.btnRegister = new System.Windows.Forms.Button();
			this.returnLogin = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtUsername
			// 
			this.txtUsername.Font = new System.Drawing.Font("宋体", 10F);
			this.txtUsername.Location = new System.Drawing.Point(115, 130);
			this.txtUsername.Name = "txtUsername";
			this.txtUsername.Size = new System.Drawing.Size(227, 23);
			this.txtUsername.TabIndex = 5;
			// 
			// txtPassword
			// 
			this.txtPassword.Font = new System.Drawing.Font("宋体", 10F);
			this.txtPassword.Location = new System.Drawing.Point(115, 174);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '*';
			this.txtPassword.Size = new System.Drawing.Size(227, 23);
			this.txtPassword.TabIndex = 6;
			// 
			// txtTrueName
			// 
			this.txtTrueName.Font = new System.Drawing.Font("宋体", 10F);
			this.txtTrueName.Location = new System.Drawing.Point(115, 215);
			this.txtTrueName.Name = "txtTrueName";
			this.txtTrueName.Size = new System.Drawing.Size(227, 23);
			this.txtTrueName.TabIndex = 7;
			// 
			// txtTelephone
			// 
			this.txtTelephone.Font = new System.Drawing.Font("宋体", 10F);
			this.txtTelephone.Location = new System.Drawing.Point(115, 255);
			this.txtTelephone.Name = "txtTelephone";
			this.txtTelephone.Size = new System.Drawing.Size(227, 23);
			this.txtTelephone.TabIndex = 8;
			// 
			// txtCompany
			// 
			this.txtCompany.Font = new System.Drawing.Font("宋体", 10F);
			this.txtCompany.Location = new System.Drawing.Point(115, 296);
			this.txtCompany.Name = "txtCompany";
			this.txtCompany.Size = new System.Drawing.Size(227, 23);
			this.txtCompany.TabIndex = 9;
			// 
			// btnRegister
			// 
			this.btnRegister.Location = new System.Drawing.Point(229, 341);
			this.btnRegister.Name = "btnRegister";
			this.btnRegister.Size = new System.Drawing.Size(93, 33);
			this.btnRegister.TabIndex = 11;
			this.btnRegister.Text = "注册";
			this.btnRegister.UseVisualStyleBackColor = true;
			this.btnRegister.Click += new System.EventHandler(this.Register_Click);
			// 
			// returnLogin
			// 
			this.returnLogin.Location = new System.Drawing.Point(56, 341);
			this.returnLogin.Name = "returnLogin";
			this.returnLogin.Size = new System.Drawing.Size(93, 33);
			this.returnLogin.TabIndex = 10;
			this.returnLogin.Text = "返回登录";
			this.returnLogin.UseVisualStyleBackColor = true;
			this.returnLogin.Click += new System.EventHandler(this.ReturnLogin_Click);
			// 
			// Register
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = new System.Drawing.Size(367, 401);
			this.Controls.Add(this.btnRegister);
			this.Controls.Add(this.returnLogin);
			this.Controls.Add(this.txtCompany);
			this.Controls.Add(this.txtTelephone);
			this.Controls.Add(this.txtTrueName);
			this.Controls.Add(this.txtPassword);
			this.Controls.Add(this.txtUsername);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "Register";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "注册";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtUsername;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.TextBox txtTrueName;
		private System.Windows.Forms.TextBox txtTelephone;
		private System.Windows.Forms.TextBox txtCompany;
		private System.Windows.Forms.Button btnRegister;
		private System.Windows.Forms.Button returnLogin;
	}
}