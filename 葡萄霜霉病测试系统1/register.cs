using System;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Collections;
using Newtonsoft.Json;

namespace 葡萄霜霉病防控测试系统1
{
	public partial class Register : Form
	{
		static Socket ClientSocket;
		static readonly int port = 23456;
		static readonly string IP = "39.107.95.53";
		//static readonly string IP = "127.0.0.1";
		public Register()
		{
			InitializeComponent();
		}
		//返回到登录界面
		private void ReturnLogin_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Retry;
		}

		//注册事件
		private void Register_Click(object sender, EventArgs e)
		{
			//获取用户填的信息
			string username = txtUsername.Text.Trim();
			string password = txtPassword.Text;
			string telephone = txtTelephone.Text.Trim();
			string trueName = txtTrueName.Text.Trim();
			string company = txtCompany.Text.Trim();
			//每一个项目都不能为空
			if(username != "" && password != "" && telephone != "" && trueName != "" && company != "")
			{
				//把数据发送到服务器
				string resultMessage = "";
				IPAddress ip = IPAddress.Parse(IP);
				ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				IPEndPoint endPoint = new IPEndPoint(ip, port);
				ClientSocket.Connect(endPoint);
				Hashtable ht = new Hashtable();
				ht.Add("username", username);
				ht.Add("password", password);
				ht.Add("telephone", telephone);
				ht.Add("trueName", trueName);
				ht.Add("company", company);
				string jsonData = JsonConvert.SerializeObject(ht, new Newtonsoft.Json.JsonSerializerSettings() { StringEscapeHandling = Newtonsoft.Json.StringEscapeHandling.EscapeNonAscii });
				byte[] userInfo = Encoding.ASCII.GetBytes(jsonData);
				ClientSocket.Send(userInfo);

				byte[] receive = new byte[256];
				int realLength = ClientSocket.Receive(receive);
				string result = Encoding.ASCII.GetString(receive, 0, realLength);
				if (result == "yes")
				{
					resultMessage = "注册成功";
					this.DialogResult = DialogResult.Retry;
				}
				else if(result == "username")
				{
					resultMessage = "该用户名已经被注册！";
				}
				else if(result == "trueName")
				{
					resultMessage = "该真实姓名已经被注册！";
				}
				else if(result == "telephone")
				{
					resultMessage = "该手机号码已经被注册！";
				}
				MessageBox.Show(resultMessage);
			}
			else
			{
				MessageBox.Show("所有项目都不能为空！");
			}
		}
	}
}
