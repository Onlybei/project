﻿using System;
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
			if (ClientSocket == null || ClientSocket.RemoteEndPoint == null )
			{
				try
				{
					IPAddress ip = IPAddress.Parse(IP);
					ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
					IPEndPoint endPoint = new IPEndPoint(ip, port);
					ClientSocket.Connect(endPoint);
				}
				catch
				{
					MessageBox.Show("请求服务器超时！");
					return null;
				}
			}
			return ClientSocket;
		}

		private void btnLogin_Click(object sender, EventArgs e)
		{
			string name = txtName.Text.Trim();
			string pwd = txtPwd.Text;
			if (name != "" && pwd != "")
			{
				if(ConnectSocket() != null)
				{
					Hashtable ht = new Hashtable();
					string safePwd = Convert.ToBase64String(Encoding.ASCII.GetBytes(pwd));
					ht.Add("name", name);
					ht.Add("pwd", safePwd);
					string jsonData = JsonConvert.SerializeObject(ht);
					byte[] userInfo = Encoding.ASCII.GetBytes("L" + jsonData);
					ClientSocket.Send(userInfo);
					byte[] receive = new byte[20];
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
			}
			else
			{
				MessageBox.Show("用户名或密码不能为空！");
				txtName.Clear();
				txtPwd.Clear();
			}
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
