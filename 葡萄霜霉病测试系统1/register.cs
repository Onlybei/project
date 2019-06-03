using System;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Collections;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace 葡萄霜霉病防控测试系统1
{
	public partial class Register : Form
	{
		static Socket ClientSocket;
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
				if(!CheckUserInfo(username, password, trueName, telephone))
				{
					return;
				}
				ClientSocket = Login.ConnectSocket();
				if (ClientSocket != null && ClientSocket.RemoteEndPoint != null )
				{
					//把数据发送到服务器
					string resultMessage = "";
					Hashtable ht = new Hashtable();
					ht.Add("username", username);
					string safePwd = Convert.ToBase64String(Encoding.ASCII.GetBytes(password));
					ht.Add("password", safePwd);
					ht.Add("telephone", telephone);
					ht.Add("trueName", trueName);
					ht.Add("company", company);
					string jsonData = JsonConvert.SerializeObject(ht, new Newtonsoft.Json.JsonSerializerSettings() { StringEscapeHandling = Newtonsoft.Json.StringEscapeHandling.EscapeNonAscii });
					byte[] userInfo = Encoding.ASCII.GetBytes("R" + jsonData);
					ClientSocket.Send(userInfo);
					byte[] receive = new byte[20];
					int realLength = ClientSocket.Receive(receive);
					string result = Encoding.ASCII.GetString(receive, 0, realLength);
					if (result == "yes")
					{
						resultMessage = "注册成功";
						this.DialogResult = DialogResult.Retry;
					}
					else if (result == "username")
					{
						resultMessage = "该用户名已经被注册！";
					}
					else if (result == "trueName")
					{
						resultMessage = "该真实姓名已经被注册！";
					}
					else if (result == "telephone")
					{
						resultMessage = "该手机号码已经被注册！";
					}
					MessageBox.Show(resultMessage);
				}
			}
			else
			{
				MessageBox.Show("所有项目都不能为空！");
			}
		}

		private bool CheckUserInfo(string username, string passwrod, string trueName, string telephone)
		{
			if (Regex.IsMatch(username, "^[a-zA-Z]\\w{5,15}$"))
			{
				
				if (Regex.IsMatch(passwrod, "^\\w{6,15}$"))
				{
					if(Regex.IsMatch(trueName, "^[\u4e00-\u9fa5]{0,}$"))
					{

						if (Regex.IsMatch(telephone, "^1[34578]\\d{9}$"))
						{
							return true;
						}
						else
						{
							MessageBox.Show("请输入正确的手机号");
							return false;
						}
					}
					else
					{
						MessageBox.Show("真实姓名必须是中文");
						return false;
					}
				}
				else
				{
					MessageBox.Show("密码应该在6~15位之间");
					return false;
				}
			}
			else
			{
				MessageBox.Show("用户名首位是字母的6-16位");
				return false;
			}
			
		}
	}
}
