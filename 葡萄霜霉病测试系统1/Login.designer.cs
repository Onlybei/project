namespace 葡萄霜霉病防控测试系统1
{
    partial class Login
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
			this.txtPwd = new System.Windows.Forms.TextBox();
			this.txtName = new System.Windows.Forms.TextBox();
			this.btnLogin = new System.Windows.Forms.Button();
			this.btnRegister = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtPwd
			// 
			resources.ApplyResources(this.txtPwd, "txtPwd");
			this.txtPwd.Name = "txtPwd";
			this.txtPwd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPwd_KeyPress);
			// 
			// txtName
			// 
			resources.ApplyResources(this.txtName, "txtName");
			this.txtName.Name = "txtName";
			// 
			// btnLogin
			// 
			resources.ApplyResources(this.btnLogin, "btnLogin");
			this.btnLogin.Name = "btnLogin";
			this.btnLogin.UseVisualStyleBackColor = true;
			this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
			// 
			// btnRegister
			// 
			resources.ApplyResources(this.btnRegister, "btnRegister");
			this.btnRegister.Name = "btnRegister";
			this.btnRegister.UseVisualStyleBackColor = true;
			this.btnRegister.Click += new System.EventHandler(this.BtnRegister_Click);
			// 
			// Login
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnRegister);
			this.Controls.Add(this.btnLogin);
			this.Controls.Add(this.txtPwd);
			this.Controls.Add(this.txtName);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "Login";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnRegister;
    }
}

