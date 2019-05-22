using System;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Collections;
using Newtonsoft.Json;

namespace 葡萄霜霉病防控测试系统1
{
	public partial class Login : Form
	{
		public static Socket ClientSocket = null;
		static readonly int port = 23456;
		//static readonly string IP = "39.107.95.53";
		static readonly string IP = "127.0.0.1";
		public static string username = "";
		public Login()
		{
			InitializeComponent();
		}
		public static Socket ConnectSocket()
		{
			if (ClientSocket == null)
			{
				IPAddress ip = IPAddress.Parse(IP);
				ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				IPEndPoint endPoint = new IPEndPoint(ip, port);
				ClientSocket.Connect(endPoint);				
			}
			return ClientSocket;
		}

		private void btnLogin_Click(object sender, EventArgs e)
		{
			string name = txtName.Text.Trim();
			string pwd = txtPwd.Text;
			if (name != "" && pwd != "")
			{
				ConnectSocket();
				try
				{						
					Hashtable ht = new Hashtable();
					string safePwd = Convert.ToBase64String(Encoding.ASCII.GetBytes(pwd));
					ht.Add("name", name);
					ht.Add("pwd", safePwd);
					string jsonData = JsonConvert.SerializeObject(ht);
					byte[] userInfo = Encoding.ASCII.GetBytes(jsonData);
					ClientSocket.Send(userInfo);

					byte[] receive = new byte[256];
					int realLength = ClientSocket.Receive(receive);
					string result = Encoding.ASCII.GetString(receive, 0, realLength);
					if (result == "yes")
					{
						username = name;
						this.DialogResult = DialogResult.OK;
					}
					else
					{
						MessageBox.Show("用户名或密码错误！");
						txtName.Clear();
						txtPwd.Clear();
					}
				}
				catch
				{
					MessageBox.Show("请求服务器超时！");
					return ;
				}
					
				
				//String sql = String.Format("select count(8) from Login where username='{0}' and password ='{1}'", name, pwd);
				//if (name == "ptsmb" && pwd == "ptsmb")
				//{
				//  MessageBox.Show("登录成功");
				// MessageBox.Show("Dear Student:登录失败，请重新输入");
				////内存中创建窗体2对象

				//this.Hide();
				//// this.Dispose();
				//葡萄霜霉病防控系统 frm2 = new 葡萄霜霉病防控系统();
				////展示当前窗体
				//frm2.Show();
				//}

				//this.SendUsername(name);
			}
			else
			{
				MessageBox.Show("用户名或密码不能为空！");
				txtName.Clear();
				txtPwd.Clear();
			}
			//创建SqlCommand对象
			//MySQLCommand command = new MySQLCommand(sql, conn);
			//int num = Convert.ToInt32(command.ExecuteScalar());
			//try
			//{
			//    if (num > 0)
			//    {

			//    }
			//    else
			//    {
			//        MessageBox.Show("登录失败");
			//    }
			//}
			//catch
			//{
			//    MessageBox.Show("错误异常");
			//}
			//finally {
			//    conn.Close();
			//}
			////

		}
		//向服务器发送用户名和密码

		private void txtPwd_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == 13)
			{
				this.btnLogin.Focus();
				this.btnLogin.PerformClick();
			}
		}

		//注册事件
		private void BtnRegister_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.No;
		}
	}
}
