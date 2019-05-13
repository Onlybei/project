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
		/*初始化form*/
		public Form1()
		{
			InitializeComponent();
			TextBox.CheckForIllegalCrossThreadCalls = false;
		}
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
		//将远程连接的客户端的ip地址和Socket存入集合当中
		Dictionary<string, Socket> dicSocket = new Dictionary<string, Socket>();
		Dictionary<string, Thread> dicThread = new Dictionary<string, Thread>();

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

		Socket socketSend;

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
					//将远程连接的客户端的IP地址和socket存入集合中
					dicSocket.Add(socketSend.RemoteEndPoint.ToString(), socketSend);
					//将远程连接的客户端的Ip地址和端口号存入下拉框中
					cboUsers.Items.Add(socketSend.RemoteEndPoint.ToString());//控件里有连接的ip信息												 //向列表控件中添加客户端的IP信息;  
					lbOnline.Items.Add(socketSend.RemoteEndPoint.ToString());
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
			DateTime DateTime = new System.DateTime();

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
					//如果用户下线则跳出循环
					if (r == 0)
					{
						break;
					}
					string str = Encoding.UTF8.GetString(buffer, 0, r);
					ShowMsg(str);
					if (str.Substring(0, 1) == "R")
					{
					}
					if (str.Substring(0, 1) == "{")
					{
						string name = "";
						string pwd = "";
						Hashtable ht = JsonConvert.DeserializeObject<Hashtable>(str);
						if (ht.Contains("name") && ht.Contains("pwd"))
						{
							name = ht["name"].ToString();
							pwd = ht["pwd"].ToString();
						}

						cmd = connection.CreateCommand();
						cmd.CommandText = "select user_password from tb_user where user_name='" + name + "'";
						MySqlDataReader reader = null;
						reader = cmd.ExecuteReader();
						string result = "no";
						while (reader.Read())
						{
							if (reader.ToString() != "")
							{
								if (reader[0].ToString() == pwd)
								{
									result = "yes";
								}
							}
						}
						byte[] send = Encoding.ASCII.GetBytes(result);
						socketSend.Send(send);
					}
					else if (str.Substring(0, 1) == "P") //收到了大棚采集到的数据
					{
						if (str.Substring(2, 1) == "1")
						{
							//  ShowMsg(socketSend.RemoteEndPoint + ":" + str);
							Temp = str.Substring(9, 4);
							Humility = str.Substring(23, 4);
							Light = Convert.ToInt32(str.Substring(34, 5));
							Id = Convert.ToInt32(str.Substring(43, 2));
							cmd = connection.CreateCommand();
							cmd.CommandText = "INSERT INTO System_1(Id,Temp,Humility,Light,DateTime,Time)VALUES(@Id,@Temp,@Humility,@Light,@DateTime,@Time)";
							cmd.Parameters.AddWithValue("@Id", Id);
							cmd.Parameters.AddWithValue("@Humility", Humility);
							cmd.Parameters.AddWithValue("@Light", Light);
							cmd.Parameters.AddWithValue("@Temp", Temp);
							cmd.Parameters.AddWithValue("@DateTime", DateTime.Now);
							cmd.Parameters.AddWithValue("@Time", DateTime.Now.ToShortDateString());
							cmd.ExecuteNonQuery();
							//string strMsg = txtMsg.Text;
						}
						else if (str.Substring(2, 1) == "2")
						{
							Temp = str.Substring(9, 4);
							Humility = str.Substring(23, 4);
							Light = Convert.ToInt32(str.Substring(34, 5));
							Id = Convert.ToInt32(str.Substring(43, 2));
							cmd = connection.CreateCommand();
							cmd.CommandText = "INSERT INTO System_2(Id,Temp,Humility,Light,DateTime,Time)VALUES(@Id,@Temp,@Humility,@Light,@DateTime,@Time)";
							cmd.Parameters.AddWithValue("@Id", Id);
							cmd.Parameters.AddWithValue("@Humility", Humility);
							cmd.Parameters.AddWithValue("@Light", Light);
							cmd.Parameters.AddWithValue("@Temp", Temp);
							cmd.Parameters.AddWithValue("@DateTime", DateTime.Now);
							cmd.Parameters.AddWithValue("@Time", DateTime.Now.ToShortDateString());
							cmd.ExecuteNonQuery();
						}
						else if (str.Substring(2, 1) == "3")
						{
							Temp = str.Substring(9, 4);
							Humility = str.Substring(23, 4);
							Light = Convert.ToInt32(str.Substring(34, 5));
							Id = Convert.ToInt32(str.Substring(43, 2));
							cmd = connection.CreateCommand();
							cmd.CommandText = "INSERT INTO System_3(Id,Temp,Humility,Light,DateTime,Time)VALUES(@Id,@Temp,@Humility,@Light,@DateTime,@Time)";
							cmd.Parameters.AddWithValue("@Id", Id);
							cmd.Parameters.AddWithValue("@Humility", Humility);
							cmd.Parameters.AddWithValue("@Light", Light);
							cmd.Parameters.AddWithValue("@Temp", Temp);
							cmd.Parameters.AddWithValue("@DateTime", DateTime.Now);
							cmd.Parameters.AddWithValue("@Time", DateTime.Now.ToShortDateString());
							cmd.ExecuteNonQuery();
						}
						else if (str.Substring(2, 1) == "4")
						{
							Temp = str.Substring(9, 4);
							Humility = str.Substring(23, 4);
							Light = Convert.ToInt32(str.Substring(34, 5));
							Id = Convert.ToInt32(str.Substring(43, 2));
							cmd = connection.CreateCommand();
							cmd.CommandText = "INSERT INTO System_4(Id,Temp,Humility,Light,DateTime,Time)VALUES(@Id,@Temp,@Humility,@Light,@DateTime,@Time)";
							cmd.Parameters.AddWithValue("@Id", Id);
							cmd.Parameters.AddWithValue("@Humility", Humility);
							cmd.Parameters.AddWithValue("@Light", Light);
							cmd.Parameters.AddWithValue("@Temp", Temp);
							cmd.Parameters.AddWithValue("@DateTime", DateTime.Now);
							cmd.Parameters.AddWithValue("@Time", DateTime.Now.ToShortDateString());
							cmd.ExecuteNonQuery();
						}
						else if (str.Substring(2, 1) == "5")
						{
							Temp = str.Substring(9, 4);
							Humility = str.Substring(23, 4);
							Light = Convert.ToInt32(str.Substring(34, 5));
							Id = Convert.ToInt32(str.Substring(43, 2));
							cmd = connection.CreateCommand();
							cmd.CommandText = "INSERT INTO System_5(Id,Temp,Humility,Light,DateTime,Time)VALUES(@Id,@Temp,@Humility,@Light,@DateTime,@Time)";
							cmd.Parameters.AddWithValue("@Id", Id);
							cmd.Parameters.AddWithValue("@Humility", Humility);
							cmd.Parameters.AddWithValue("@Light", Light);
							cmd.Parameters.AddWithValue("@Temp", Temp);
							cmd.Parameters.AddWithValue("@DateTime", DateTime.Now);
							cmd.Parameters.AddWithValue("@Time", DateTime.Now.ToShortDateString());
							cmd.ExecuteNonQuery();
						}
						else if (str.Substring(2, 1) == "6")
						{
							Temp = str.Substring(9, 4);
							Humility = str.Substring(23, 4);
							Light = Convert.ToInt32(str.Substring(34, 5));
							Id = Convert.ToInt32(str.Substring(43, 2));
							cmd = connection.CreateCommand();
							cmd.CommandText = "INSERT INTO System_6(Id,Temp,Humility,Light,DateTime,Time)VALUES(@Id,@Temp,@Humility,@Light,@DateTime,@Time)";
							cmd.Parameters.AddWithValue("@Id", Id);
							cmd.Parameters.AddWithValue("@Humility", Humility);
							cmd.Parameters.AddWithValue("@Light", Light);
							cmd.Parameters.AddWithValue("@Temp", Temp);
							cmd.Parameters.AddWithValue("@DateTime", DateTime.Now);
							cmd.Parameters.AddWithValue("@Time", DateTime.Now.ToShortDateString());
							cmd.ExecuteNonQuery();
						}
						else
						{
							break;
						}

						byte[] arrMsg = System.Text.Encoding.UTF8.GetBytes(str); //将要发送的字符串转换成Utf-8字节数组; 
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
						byte[] arrMsg = System.Text.Encoding.UTF8.GetBytes(str); //将要发送的字符串转换成Utf-8字节数组; 
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
					ShowMsg("" + socketSend.RemoteEndPoint.ToString() + "断开异常消息\r\n" + se.Message + "\r\n");
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
			btnSend.PerformClick();
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

		private void btnSendFile_Click(object sender, EventArgs e)
		{
			//获得要发送文件的路径
			try
			{
				string path = txtPath.Text;
				using (FileStream fsRead = new FileStream(path, FileMode.Open, FileAccess.Read))
				{
					byte[] buffer = new byte[1024 * 1024 * 5];
					int r = fsRead.Read(buffer, 0, buffer.Length);
					List<byte> list = new List<byte>();
					list.Add(1);
					list.AddRange(buffer);
					byte[] newbuffer = list.ToArray();

					dicSocket[cboUsers.SelectedItem.ToString()].Send(buffer, 0, r + 1, SocketFlags.None);
				}

			}
			catch
			{ }
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
			string strMsg = "203.195.204.153:23456" + "\r\n" + "->" + txtMsg.Text.Trim() + "\r\n";
			byte[] arrMsg = System.Text.Encoding.UTF8.GetBytes(strMsg); //将要发送的字符串转换成Utf-8字节数组;  
			byte[] arrSendMsg = new byte[arrMsg.Length];
			//arrSendMsg[0] = 0; //表示发送的是消息数据  
			Buffer.BlockCopy(arrMsg, 0, arrSendMsg, 0, arrMsg.Length);
			string strKey = "";
			strKey = lbOnline.Text.Trim();
			if (string.IsNullOrEmpty(strKey))// 判断    是不是选择了发送的对象  
			{
				//  MessageBox.Show("请选择你要发送的好友!!!");  
			}
			else
			{
				dicSocket[strKey].Send(arrSendMsg); //解决了sokConnection是局部变量，不能再本函数中引用的问题;  
				ShowMsg(strMsg);
				txtMsg.Clear();
			}
		}

		private void cboUsers_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void txtMsg_TextChanged(object sender, EventArgs e)
		{

		}

		private void lbOnline_SelectedIndexChanged(object sender, EventArgs e)
		{

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
