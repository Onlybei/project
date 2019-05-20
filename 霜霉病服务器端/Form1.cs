using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using MySQLDriverCS;
using System.IO;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Collections;
using Newtonsoft.Json;

namespace _19server
{
	public partial class Form1 : Form
	{
		Socket socketSend;
		//protected override void OnLoad(EventArgs e)
		//{
		//    base.OnLoad(e);
		//    btnSend.PerformClick();
		//}

		/// <summary>
		/// 5.29新加的 多线程通信
		/// </summary>
		Thread threadwatch = null;//负责监听客户端连接请求的线程
		Socket socketwatch = null;
		//将远程连接的客户端的ip地址和Socket以及users存入集合当中
		Dictionary<string, Socket> dicSocket = new Dictionary<string, Socket>();
		Dictionary<string, Thread> dicThread = new Dictionary<string, Thread>();
		Dictionary<string, string> dicUsers = new Dictionary<string, string>();
		static int OnlineNumber = 0;

		/*初始化form*/
		public Form1()
		{
			InitializeComponent();
			TextBox.CheckForIllegalCrossThreadCalls = false;
		}
		private void btnStart_Click(object sender, EventArgs e)   //开始监听
		{
			try
			{
				//当点击开始监听的时候 在服务器端创建一个负责监听IP地址和端口号的socket
				socketwatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				IPAddress ip = IPAddress.Any;
				//   IPAddress ip = IPAddress.Parse(txtServer.Text);
				//创建端口号对象
				IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(txtPort.Text));
				//监听
				try
				{
					socketwatch.Bind(point);
				}
				catch (SocketException se)
				{
					MessageBox.Show("异常" + se.Message);
				}
				socketwatch.Listen(10);//一个监听时间点最大10个
									   //创建负责监听的线程
				threadwatch = new Thread(Listen);//threadWatch =  new  Thread（WatchConnecting）; 
				threadwatch.IsBackground = true;
				threadwatch.Start();//();;
				ShowMsg("监听成功");
			}
			catch
			{
			}

		}
		/// <summary>
		/// 等待客户端的连接，并且创建与之通信用的socket
		/// //监听客户端请求的方法：watchconnecting 本代码是listen
		/// 
		private void BtnStart_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == 13)
			{
				btnStart.Focus();
				btnStart.PerformClick();
			}
		}
		

		void Listen()//void Listen(object o)
		{
			// Socket socketWatch = o as Socket;
			//等待客户端的连接，并且创建一个负责通信的socket
			while (true)
			{
				try
				{
					//负责跟客户端通信的socket  
					socketSend = socketwatch.Accept();
					////将远程连接的客户端的IP地址和socket存入集合中
					//dicSocket.Add(socketSend.RemoteEndPoint.ToString(), socketSend);
					////将远程连接的客户端的Ip地址和端口号存入下拉框中
					//cboUsers.Items.Add(socketSend.RemoteEndPoint.ToString());//控件里有连接的ip信息
					////向列表控件中添加客户端的IP信息;  
					//lbOnline.Items.Add(socketSend.RemoteEndPoint.ToString());
					ShowMsg(socketSend.RemoteEndPoint.ToString() + ":" + "连接成功");
					////开启一个新线程，不停接收客户端发过来的消息
					Thread th = new Thread(Receive);//Receive在demo里是RecMsg
					th.IsBackground = true;
					th.Start(socketSend);
					//将与客户端连接的套接字对象添加到集合中;  
					dicThread.Add(socketSend.RemoteEndPoint.ToString(), th);
					
					/////////////////////////////////////////////////////////新加////////////////////////////////
					//将新建的线程添加到线程的集合中去

					//  MessageBox.Show("!2333");   
				}
				catch
				{
				}
			}
		}

		// List<byte> list = new List<byte>();
		//string ip = "139.199.184.179";

		/// <summary>
		/// 服务器端不停的接收客户端发来的消息
		/// demo中RecMsg
		/// <param name="o"></param>
		void Receive(object o)
		{

			string MyConnectionString = "Server=localhost;Database=testdb;Uid=root;Pwd=123456;";
			string Temp;
			string Humility;
			int Light;
			int Id;
			DateTime DateTime = new DateTime();

			MySqlConnection connection = new MySqlConnection(MyConnectionString);
			MySqlCommand cmd;
			connection.Open();//连接数据库
							  // MessageBox.Show("!!!");
			while (true)
			{
				Socket socketSend = o as Socket;
				try
				{
					//客户端连接成功后，服务器应该接收客户端发来的消息  2M的缓存区
					byte[] buffer = new byte[1024 * 1024 * 2];
					//实际接收的有效字节数
					int r = socketSend.Receive(buffer);
					string currentKey = socketSend.RemoteEndPoint.ToString();
					string currentClientIP = ((IPEndPoint)socketSend.RemoteEndPoint).Address.ToString();
					//如果用户下线则跳出循环
					if (r == 0)
					{
						break;
					}
					string str = Encoding.UTF8.GetString(buffer, 0, r);
					//ShowMsg(currentClientIP + ":" + str);
					if (str.Substring(0, 1) == "R")
					{
					}
					//对象json化第一个字符为"{"
					if (str.Substring(0, 1) == "{")
					{
						cmd = connection.CreateCommand();
						//登录
						if (str.Substring(2, 1) == "p")
						{
							string result = "no";
							int count = -1;
							string name = "";
							string pwd = "";
							Hashtable ht = JsonConvert.DeserializeObject<Hashtable>(str);
							if (ht.Contains("name") && ht.Contains("pwd"))
							{
								name = ht["name"].ToString();
								pwd = ht["pwd"].ToString();
							}
							if (dicUsers.ContainsValue(name))
							{
								foreach(string key in dicUsers.Keys)
								{
									//说明该用户已经上线
									if(dicUsers[key] == name)
									{
										//移除下拉框
										cboUsers.Items.Remove(key);
										foreach(int i in lbOnline.Items)
										{
											if(lbOnline.Items[i].ToString() == name)
											{
												//移除listview中的一行
												lbOnline.Items[i].Remove();
											}
										}
										//移除用户dic，socketdic，线程dic
										byte[] msg = Encoding.UTF8.GetBytes("msg" + GetCurrentTime() + "您的账号已经在别的设备上登录，本客户端已经下线");
										dicUsers.Remove(key);
										dicSocket.Remove(key);
										dicThread.Remove(key);
									}
								}
							}
							cmd.CommandText = "select COUNT(*) from tb_user where user_name='" + name + "' and user_password = '" + pwd + "';";
							count = Convert.ToInt32(cmd.ExecuteScalar());
							if (count > 0)
							{
								//将远程连接的客户端的IP地址和socket存入集合中
								dicSocket.Add(socketSend.RemoteEndPoint.ToString(), socketSend);
								dicUsers.Add(socketSend.RemoteEndPoint.ToString(), name);

								cboUsers.Items.Add(socketSend.RemoteEndPoint.ToString());//将远程连接的客户端的Ip地址和端口号存入下拉框中				
								lbOnline.Items.Add(socketSend.RemoteEndPoint.ToString());//向列表控件中添加客户端的IP信息;							 
								OnlineNumber++;//在线人数+1
								lbOnlineNumber.Text = OnlineNumber.ToString(); ;
								result = "yes";								
								ShowMsg(ht["name"].ToString() + "登录成功！");
							}
							byte[] send = Encoding.UTF8.GetBytes(result);
							socketSend.Send(send);
						}
						//注册
						else if (str.Substring(2, 1) == "u")
						{
							string result = "no";
							int count = -1;
							Hashtable ht = JsonConvert.DeserializeObject<Hashtable>(str);
							if (ht.Contains("username") && ht.Contains("password") && ht.Contains("telephone") && ht.Contains("trueName") && ht.Contains("company"))
							{
								//先查询是否有用户名、真实姓名、手机号相同的情况
								//用户名查重
								cmd.CommandText = "SELECT COUNT(*) FROM TB_USER WHERE User_Name='" + ht["username"].ToString() + "';";
								count = Convert.ToInt32(cmd.ExecuteScalar());
								if (count > 0)
								{
									result = "username";
								}
								else
								{
									//真实姓名查重
									cmd.CommandText = "SELECT COUNT(*) FROM TB_USER WHERE User_TrueName='" + ht["trueName"].ToString() + "';";
									count = Convert.ToInt32(cmd.ExecuteScalar());
									if (count > 0)
									{
										result = "trueName";
									}
									else
									{
										//手机号码查重
										cmd.CommandText = "SELECT COUNT(*) FROM TB_USER WHERE  User_Telephone='" + ht["telephone"].ToString() + "';";
										count = Convert.ToInt32(cmd.ExecuteScalar());
										if (count > 0)
										{
											result = "telephone";
										}
										else
										{
											//如果数据库没有记录，将新纪录插入到数据库中
											cmd.CommandText = "INSERT INTO tb_user(User_Name, User_Password, User_TrueName, User_Telephone, User_Company)VALUES(@username, @password, @trueName, @telephone, @company);";
											cmd.Parameters.AddWithValue("@username", ht["username"].ToString());
											cmd.Parameters.AddWithValue("@password", ht["password"].ToString());
											cmd.Parameters.AddWithValue("@trueName", ht["trueName"].ToString());
											cmd.Parameters.AddWithValue("@telephone", ht["telephone"].ToString());
											cmd.Parameters.AddWithValue("@company", ht["company"].ToString());
											if (cmd.ExecuteNonQuery() > 0)
											{
												result = "yes";
												ShowMsg(ht["username"].ToString() + "注册成功！");
											}
										}
									}
								}								
								byte[] send = Encoding.UTF8.GetBytes(result);
								socketSend.Send(send);
							}
						}
					}
					//接收来自客户端的消息
					else if(str.Substring(0, 3) == "msg")
					{
						ShowMsg(dicUsers[currentKey].ToString() + "(" + str.Substring(3, 19) + "):" + str.Substring(22));
						//ShowMsg(currentClientIP + "(" + str.Substring(3, 22) + "):" + str.Substring(22));
						//ShowMsg( str.Substring(3));
					}
					else if (str.Substring(0, 1) == "P") //收到了大棚采集到的数据
					{
						int HouseNumber = Convert.ToInt32(str.Substring(2, 1));
						if(HouseNumber >= 1 && HouseNumber <= 6)
						{
							Temp = str.Substring(9, 4);
							Humility = str.Substring(23, 4);
							Light = Convert.ToInt32(str.Substring(34, 5));
							Id = Convert.ToInt32(str.Substring(43, 2));
							cmd = connection.CreateCommand();
							cmd.CommandText = "INSERT INTO System_" + str.Substring(2, 1) + "(Id,Temp,Humility,Light,DateTime,Time)VALUES(@Id,@Temp,@Humility,@Light,@DateTime,@Time)";
							cmd.Parameters.AddWithValue("@Id", Id);
							cmd.Parameters.AddWithValue("@Humility", Humility);
							cmd.Parameters.AddWithValue("@Light", Light);
							cmd.Parameters.AddWithValue("@Temp", Temp);
							cmd.Parameters.AddWithValue("@DateTime", DateTime.Now);
							cmd.Parameters.AddWithValue("@Time", DateTime.Now.ToShortDateString());
							cmd.ExecuteNonQuery();
						}
						else if(HouseNumber == 100)
						{
							//if (str.Substring(2, 1) == "1")
							//{
							//	//  ShowMsg(socketSend.RemoteEndPoint + ":" + str);
							//	Temp = str.Substring(9, 4);
							//	Humility = str.Substring(23, 4);
							//	Light = Convert.ToInt32(str.Substring(34, 5));
							//	Id = Convert.ToInt32(str.Substring(43, 2));
							//	cmd = connection.CreateCommand();
							//	cmd.CommandText = "INSERT INTO System_1(Id,Temp,Humility,Light,DateTime,Time)VALUES(@Id,@Temp,@Humility,@Light,@DateTime,@Time)";
							//	cmd.Parameters.AddWithValue("@Id", Id);
							//	cmd.Parameters.AddWithValue("@Humility", Humility);
							//	cmd.Parameters.AddWithValue("@Light", Light);
							//	cmd.Parameters.AddWithValue("@Temp", Temp);
							//	cmd.Parameters.AddWithValue("@DateTime", DateTime.Now);
							//	cmd.Parameters.AddWithValue("@Time", DateTime.Now.ToShortDateString());
							//	cmd.ExecuteNonQuery();
							//	//string strMsg = txtMsg.Text;
							//}
							//else if (str.Substring(2, 1) == "2")
							//{
							//	Temp = str.Substring(9, 4);
							//	Humility = str.Substring(23, 4);
							//	Light = Convert.ToInt32(str.Substring(34, 5));
							//	Id = Convert.ToInt32(str.Substring(43, 2));
							//	cmd = connection.CreateCommand();
							//	cmd.CommandText = "INSERT INTO System_2(Id,Temp,Humility,Light,DateTime,Time)VALUES(@Id,@Temp,@Humility,@Light,@DateTime,@Time)";
							//	cmd.Parameters.AddWithValue("@Id", Id);
							//	cmd.Parameters.AddWithValue("@Humility", Humility);
							//	cmd.Parameters.AddWithValue("@Light", Light);
							//	cmd.Parameters.AddWithValue("@Temp", Temp);
							//	cmd.Parameters.AddWithValue("@DateTime", DateTime.Now);
							//	cmd.Parameters.AddWithValue("@Time", DateTime.Now.ToShortDateString());
							//	cmd.ExecuteNonQuery();
							//}
							//else if (str.Substring(2, 1) == "3")
							//{
							//	Temp = str.Substring(9, 4);
							//	Humility = str.Substring(23, 4);
							//	Light = Convert.ToInt32(str.Substring(34, 5));
							//	Id = Convert.ToInt32(str.Substring(43, 2));
							//	cmd = connection.CreateCommand();
							//	cmd.CommandText = "INSERT INTO System_3(Id,Temp,Humility,Light,DateTime,Time)VALUES(@Id,@Temp,@Humility,@Light,@DateTime,@Time)";
							//	cmd.Parameters.AddWithValue("@Id", Id);
							//	cmd.Parameters.AddWithValue("@Humility", Humility);
							//	cmd.Parameters.AddWithValue("@Light", Light);
							//	cmd.Parameters.AddWithValue("@Temp", Temp);
							//	cmd.Parameters.AddWithValue("@DateTime", DateTime.Now);
							//	cmd.Parameters.AddWithValue("@Time", DateTime.Now.ToShortDateString());
							//	cmd.ExecuteNonQuery();
							//}
							//else if (str.Substring(2, 1) == "4")
							//{
							//	Temp = str.Substring(9, 4);
							//	Humility = str.Substring(23, 4);
							//	Light = Convert.ToInt32(str.Substring(34, 5));
							//	Id = Convert.ToInt32(str.Substring(43, 2));
							//	cmd = connection.CreateCommand();
							//	cmd.CommandText = "INSERT INTO System_4(Id,Temp,Humility,Light,DateTime,Time)VALUES(@Id,@Temp,@Humility,@Light,@DateTime,@Time)";
							//	cmd.Parameters.AddWithValue("@Id", Id);
							//	cmd.Parameters.AddWithValue("@Humility", Humility);
							//	cmd.Parameters.AddWithValue("@Light", Light);
							//	cmd.Parameters.AddWithValue("@Temp", Temp);
							//	cmd.Parameters.AddWithValue("@DateTime", DateTime.Now);
							//	cmd.Parameters.AddWithValue("@Time", DateTime.Now.ToShortDateString());
							//	cmd.ExecuteNonQuery();
							//}
							//else if (str.Substring(2, 1) == "5")
							//{
							//	Temp = str.Substring(9, 4);
							//	Humility = str.Substring(23, 4);
							//	Light = Convert.ToInt32(str.Substring(34, 5));
							//	Id = Convert.ToInt32(str.Substring(43, 2));
							//	cmd = connection.CreateCommand();
							//	cmd.CommandText = "INSERT INTO System_5(Id,Temp,Humility,Light,DateTime,Time)VALUES(@Id,@Temp,@Humility,@Light,@DateTime,@Time)";
							//	cmd.Parameters.AddWithValue("@Id", Id);
							//	cmd.Parameters.AddWithValue("@Humility", Humility);
							//	cmd.Parameters.AddWithValue("@Light", Light);
							//	cmd.Parameters.AddWithValue("@Temp", Temp);
							//	cmd.Parameters.AddWithValue("@DateTime", DateTime.Now);
							//	cmd.Parameters.AddWithValue("@Time", DateTime.Now.ToShortDateString());
							//	cmd.ExecuteNonQuery();
							//}
							//else if (str.Substring(2, 1) == "6")
							//{
							//	Temp = str.Substring(9, 4);
							//	Humility = str.Substring(23, 4);
							//	Light = Convert.ToInt32(str.Substring(34, 5));
							//	Id = Convert.ToInt32(str.Substring(43, 2));
							//	cmd = connection.CreateCommand();
							//	cmd.CommandText = "INSERT INTO System_6(Id,Temp,Humility,Light,DateTime,Time)VALUES(@Id,@Temp,@Humility,@Light,@DateTime,@Time)";
							//	cmd.Parameters.AddWithValue("@Id", Id);
							//	cmd.Parameters.AddWithValue("@Humility", Humility);
							//	cmd.Parameters.AddWithValue("@Light", Light);
							//	cmd.Parameters.AddWithValue("@Temp", Temp);
							//	cmd.Parameters.AddWithValue("@DateTime", DateTime.Now);
							//	cmd.Parameters.AddWithValue("@Time", DateTime.Now.ToShortDateString());
							//	cmd.ExecuteNonQuery();
							//}
						}
						else
						{
							break;
						}

						byte[] arrMsg = Encoding.UTF8.GetBytes(str); //将要发送的字符串转换成Utf-8字节数组; 
						foreach (Socket s in dicSocket.Values)
						{
							s.Send(arrMsg);
						}
						// ShowMsg(strMsg);
						txtMsg.Clear();
						//MessageBox.Show("群发完毕！");
					}
					else if (str.Substring(0, 1) == "C")
					{
						//服务器接收到客户端发给它下传zigbee光照阈值的指令消息处理
						//对应找相应系统的IP
						byte[] arrMsg = Encoding.UTF8.GetBytes(str); //将要发送的字符串转换成Utf-8字节数组; 
						foreach (Socket s in dicSocket.Values)
						{
							s.Send(arrMsg);
						}
						// ShowMsg(strMsg);
						txtMsg.Clear();
						//MessageBox.Show("群发完毕！");
					}
					else
					{
						break;
					}
				}
				catch (SocketException se)
				{

					//throw;
					//从通信套接字集合中删除被中断连接的通信套接字
					dicSocket.Remove(socketSend.RemoteEndPoint.ToString());
					//从通信线程集合中删除被中断连接的通信线程对象
					dicThread.Remove(socketSend.RemoteEndPoint.ToString());
					//从列表中删除被中断的IP
					// lbOnline.Items.Remove(socketSend.RemoteEndPoint.ToString());
					ShowMsg("" + socketSend.RemoteEndPoint.ToString() + "断开异常消息\r\n" + se.Message);
					break;
				}

			}

			if (connection.State == ConnectionState.Open)
			{
				connection.Close();
				//LoadData();
			}
		}
		void ShowMsg(string str)
		{
			txtLog.AppendText(str + "\r\n");
		}


		private void Form1_Load(object sender, EventArgs e)
		{
			Control.CheckForIllegalCrossThreadCalls = false;
			//btnSend.PerformClick();
		}
		/// <summary>
		/// 服务器给客户端发送消息
		/// </summary>
		/// <param name="sender"></param>
		///// <param name="e"></param>
		//private void btnSend_Click(object sender, EventArgs e)
		//{
		//    try
		//    {
		//        string str = txtMsg.Text;

		//        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
		//        //思路一：再申请一个新数组，数组的长度是buffer.length+1 buffer[0]为0 表示文字 为1表示文件 为2表示震动
		//        //思路二：申请集合 能把数组转到集合中 可以能让集合的元素转为数组
		//        List<byte> list = new List<byte>();
		//        // list.Add(0);
		//        list.AddRange(buffer);
		//        //将泛型集合转为数组
		//        byte[] newBuffer = list.ToArray();
		//        //获得用户在 下拉框中选中的ip地址
		//string ip = cboUsers.SelectedItem.ToString();
		//        string ip = "127.0.0.1";
		//        dicSocket[ip].Send(newBuffer);
		//    }
		//    catch         ////////////////////////////////////////////////////////////////////////
		//    { }
		//    //socketSend.Send(buffer);
		//}
		/// <summary>
		/// 选择要发送的文件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSelect_Click(object sender, EventArgs e)
		{
			try
			{
				OpenFileDialog ofd = new OpenFileDialog();
				ofd.InitialDirectory = @"C:\Users\Administrator\Desktop";
				ofd.Title = "请选择要发送的文件";
				ofd.Filter = "所有文件|*.*";
				ofd.ShowDialog();
				txtPath.Text = ofd.FileName;
			}
			catch
			{ }
		}
		//发送已经选择了的文件
		private void btnSendFile_Click(object sender, EventArgs e)
		{
			string path = txtPath.Text;
			if(path.Trim().Length == 0 )
			{
				MessageBox.Show("请选择需要发送的文件！");
			}
			try
			{
				//获得要发送文件的路径
				
				using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
				{
					byte[] buffer = new byte[fs.Length];
					int fileLength = fs.Read(buffer, 0, buffer.Length);
					byte[] sendBuffer = new byte[fileLength + 1];
					sendBuffer[0] = 0x66;
					Buffer.BlockCopy(buffer, 0, sendBuffer, 1, fileLength);
					dicSocket[cboUsers.SelectedItem.ToString()].Send(buffer, 0, fileLength + 1, SocketFlags.None);
				}
			}
			catch
			{
				MessageBox.Show("发送失败！");
				return;
			}
		}
		/// <summary>
		/// 发送震动
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnZD_Click(object sender, EventArgs e)
		{
			try
			{
				byte[] buffer = new byte[1];
				buffer[0] = 2;
				dicSocket[cboUsers.SelectedItem.ToString()].Send(buffer);
			}
			catch
			{ }
		}

		private void txtPort_TextChanged(object sender, EventArgs e)
		{

		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{

		}

		//private void txtMsg_TextChanged(object sender, EventArgs e)
		//{
		//    //try
		//    //{
		//    //while (txtMsg.Text!=string.Empty)
		//    //{
		//        string str = txtMsg.Text;

		//        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
		//        //思路一：再申请一个新数组，数组的长度是buffer.length+1 buffer[0]为0 表示文字 为1表示文件 为2表示震动
		//        //思路二：申请集合 能把数组转到集合中 可以能让集合的元素转为数组
		//        List<byte> list = new List<byte>();
		//        // list.Add(0);
		//        list.AddRange(buffer);
		//        //将泛型集合转为数组
		//        byte[] newBuffer = list.ToArray();
		//        //获得用户在 下拉框中选中的ip地址
		//        //  string ip = cboUsers.SelectedItem.ToString();

		//        dicSocket[ip].Send(newBuffer);
		//        //}
		//        //catch
		//        //{ }
		//        ////socketSend.Send(buffer);
		//       // txtMsg.Text = String.Empty;
		//    //}
		//}

		private void btnSend_Click(object sender, EventArgs e)
		{
			if(cboUsers.Text != "请选择客户端")
			{
				string strMsg = txtMsg.Text.Trim();
				string strKey = cboUsers.Text;
				if(strKey != "")
				{
					if (strMsg == "")
					{
						MessageBox.Show("发送的消息不能为空，请重新输入!");
					}
					else
					{
						byte[] arrMsg = Encoding.UTF8.GetBytes("msg" + GetCurrentTime() + strMsg);
						dicSocket[strKey].Send(arrMsg);
						txtMsg.Clear();
						ShowMsg("向" + dicUsers[strKey] + "发送(" + GetCurrentTime() + "): " + strMsg);
					}
				}
			}
			else
			{
				MessageBox.Show("请选择客户端！");
			}
		}

		private void cboUsers_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void lbOnline_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void BtnSendAllOnline_MouseHover(object sender, EventArgs e)
		{
			// 创建the ToolTip 
			ToolTip toolTip1 = new ToolTip();
			// 设置显示样式
			toolTip1.AutoPopDelay = 5000;//提示信息的可见时间
			toolTip1.InitialDelay = 500;//事件触发多久后出现提示
			toolTip1.ReshowDelay = 500;//指针从一个控件移向另一个控件时，经过多久才会显示下一个提示框
			toolTip1.ShowAlways = true;//是否显示提示框
									   //  设置伴随的对象.
			toolTip1.SetToolTip(btnSendAllOnline, "向所有在线的用户发送消息！");//设置提示按钮和提示内容
		}

		private void BtnSendAllOnline_Click(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show("确定向当前在线的所有用户发送消息？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			//选择是
			if (result == DialogResult.Yes)
			{
				string strMsg = txtMsg.Text.Trim();
				if (strMsg == "")
				{
					MessageBox.Show("发送的消息不能为空，请重新输入!");
				}
				else
				{
					GroupSend("msg" + GetCurrentTime() + strMsg);
				}				
			}
			//选择否
			else
			{
				return;
			}
		}
		private void GroupSend(string str)
		{
			byte[] arrMsg = Encoding.UTF8.GetBytes(str); //将要发送的字符串转换成Utf-8字节数组; 
			if(dicSocket.Count == 0)
			{
				MessageBox.Show("当前没有用户在线！");
			}
			else
			{
				foreach (string key in dicSocket.Keys)
				{
					dicSocket[key].Send(arrMsg);
					ShowMsg("向" + dicUsers[key] + "发送(" + GetCurrentTime() + "): " + str.Substring(22));
				}
				txtMsg.Clear();
			}

			// ShowMsg(strMsg);
		}

		private string GetCurrentTime()
		{
			return DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
		}

		/// <summary>
		/// 群发消息
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		//private void btnSendToAll_Click(object sender, EventArgs e)
		//{
		//    string strMsg =txtMsg.Text.Trim() + "\r\n";
		//    byte[] arrMsg = System.Text.Encoding.UTF8.GetBytes(strMsg); //将要发送的字符串转换成Utf-8字节数组; 
		//    foreach (Socket s in dicSocket.Values)
		//    {
		//        s.Send(arrMsg);
		//    }
		//    ShowMsg(strMsg);
		//    txtMsg.Clear();
		// MessageBox.Show("群发完毕！");
		//}

	}
}
