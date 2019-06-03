using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using MySQLDriverCS;
using System.IO;
using MySql.Data.MySqlClient;
using System.Collections;
using Newtonsoft.Json;
using ClientUser;
using System.Reflection;

namespace _19server
{
	public partial class Form1 : Form
	{
		private static readonly string MyConnectionString = "Server=localhost;Database=testdb;Uid=root;Pwd=123456;Allow User Variables = True";
		private static List<List<User>> userList = new List<List<User>>();
		/// <summary>
		/// 5.29新加的 多线程通信
		/// </summary>
		/// 
		private Socket socketSend;
		private Thread threadwatch = null;//负责监听客户端连接请求的线程
		private Socket socketwatch = null;
		//将远程连接的客户端的ip地址和Socket以及users存入集合当中
		Dictionary<string, Thread> dicThread = new Dictionary<string, Thread>();

		/*初始化form*/
		public Form1()
		{
			InitializeComponent();
			//分别为在线和不在线  0 :在线 1:不在线
			userList.Add(new List<User>());
			userList.Add(new List<User>());
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
				threadwatch.Start();
				InitUserList();		//初始化用户列表
				InitListView();		//初始化listview
				ShowMsg("监听成功");
			}
			catch
			{
			}

		}
		private void InitUserList()
		{
			using (MySqlConnection connection = new MySqlConnection(MyConnectionString))
			{
				connection.Open();//连接数据库
				MySqlCommand cmd = connection.CreateCommand();
				cmd.CommandText = "SELECT * FROM TB_USER ORDER BY USER_ID";
				MySqlDataReader reader = cmd.ExecuteReader();

				//ListViewItem item = new ListViewItem("", listView1.Groups[0]);
				while (reader.Read())
				{
					//不在线用户
					//0:id
					//1:user_name
					//2.user_password
					//3.user_trueName
					//4.user_telephone
					//5.user_company
					userList[1].Add(new User(reader[1].ToString(), null, null, reader[3].ToString(), reader[4].ToString(), reader[5].ToString()));
				}
			}
		}
		private void InitListView()
		{
			lbOnline.Items.Clear();
			for (int i = 0 ; i < userList.Count; i++)
			{
				for (int j = 0 ; j < userList[i].Count(); j ++)
				{
					ListViewItem item = new ListViewItem((j + 1).ToString(), lbOnline.Groups[i]);
					//socket
					string ip = "";
					string threadNo = "";
					if (userList[i][j].socket != null)
					{
						ip = userList[i][j].socket.RemoteEndPoint.ToString();
					}
					if(userList[i][j].thread != null)
					{
						threadNo = userList[i][j].thread.ManagedThreadId.ToString();
					}
					//分别添加用户名、ip、线程号码、真实姓名、电话、公司
					item.SubItems.Add(userList[i][j].username);
					item.SubItems.Add(ip);
					item.SubItems.Add(threadNo);
					item.SubItems.Add(userList[i][j].trueName);
					item.SubItems.Add(userList[i][j].telephone);
					item.SubItems.Add(userList[i][j].company);
					lbOnline.Items.Add(item);
				}
			}
			lbOnlineNumber.Text = userList[0].Count.ToString();
		}
		//通过反射实现深拷贝
		public static T DeepClone<T>(T obj)
		{
			//如果是字符串或值类型则直接返回
			if (obj is string || obj.GetType().IsValueType) return obj;
			object retval = Activator.CreateInstance(obj.GetType());
			FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
			foreach (FieldInfo field in fields)
			{
				try { field.SetValue(retval, DeepClone(field.GetValue(obj))); }
				catch { }
			}
			return (T)retval;
			//return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(t));
		}
		private void userOnline(string username, Socket socket)
		{
			//当前在离线列表的user index
			int offlineIndex = FindUserIndexByUserName(userList[1], username);
			User user = FindUserByUsername(userList[1], username);	//找到这个user
			user.socket = socket;			//复制当前的的socket
			user.thread = dicThread[socket.RemoteEndPoint.ToString()];		//记录thread
			userList[1].RemoveAt(offlineIndex);			//离线列表删除
			userList[0].Add(user);                      //在线列表增加
			ShowMsg(username + "已经上线");
			userList[0] = userList[0].OrderBy(u => u.username).ToList();
			InitListView();								//初始化列表
		}
		private void userOffLine(string username)
		{
			//获取需要下线目前是在线的index
			int targetIndex = FindUserIndexByUserName(userList[0], username);
			string ip = userList[0][targetIndex].socket.RemoteEndPoint.ToString();
			//复制这个user
			//User user = DeepClone<User>(userList[0][targetIndex]);
			User user = FindUserByUsername(userList[0], username);
			user.socket = null;     //socket置空
			user.thread = null;
			userList[0].RemoveAt(targetIndex);
			userList[1].Add(user);
			ShowMsg(username + "已经下线");
			userList[1] = userList[1].OrderBy(u => u.username).ToList();
			InitListView();
			Thread tempTd = dicThread[ip];
			dicThread.Remove(ip);
			tempTd.Abort();              //线程终止
		}
		/// <summary>
		/// 等待客户端的连接，并且创建与之通信用的socket
		/// //监听客户端请求的方法：watchconnecting 本代码是listen
		/// 

		void Listen()//void Listen(object o)
		{
			//等待客户端的连接，并且创建一个负责通信的socket
			while (true)
			{
				try
				{
					//负责跟客户端通信的socket  
					socketSend = socketwatch.Accept();
					ShowMsg(socketSend.RemoteEndPoint.ToString() + ":" + "连接成功");
					////开启一个新线程，不停接收客户端发过来的消息
					Thread th = new Thread(Receive);//Receive在demo里是RecMsg
					th.IsBackground = true;
					th.Start(socketSend);
					//将与客户端连接的套接字对象添加到集合中;  
					dicThread.Add(socketSend.RemoteEndPoint.ToString(), th);
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
			MySqlConnection connection = new MySqlConnection(MyConnectionString);
			MySqlCommand cmd;
			connection.Open();//连接数据库
							  // MessageBox.Show("!!!");
			while (true)
			{
				Socket socketSend = o as Socket;
				try
				{
					//客户端连接成功后，服务器应该接收客户端发来的消息 3M的缓存区
					byte[] buffer = new byte[1024 * 1024 * 3];
					//实际接收的有效字节数
					int r = socketSend.Receive(buffer);
					string currentKey = socketSend.RemoteEndPoint.ToString();
					//string currentClientIP = ((IPEndPoint)socketSend.RemoteEndPoint).Address.ToString();
					string str = Encoding.UTF8.GetString(buffer, 0, r);
					//对象json化第一个字符为"{"
					if (str.Substring(0, 1) == "L")
					{
						//登录
						cmd = connection.CreateCommand();
						string result = "no";
						int count = -1;
						string name = "";
						string pwd = "";
						Hashtable ht = JsonConvert.DeserializeObject<Hashtable>(str.Substring(1));
						if (ht.Contains("name") && ht.Contains("pwd"))
						{
							name = ht["name"].ToString();
							pwd = ht["pwd"].ToString();
							//pwd = Encoding.ASCII.GetString(Convert.FromBase64String(ht["pwd"].ToString()));
							//pwd = ht["pwd"].ToString();
						}
						cmd.CommandText = "select COUNT(*) from tb_user where user_name='" + name + "'	and user_password = '" + pwd + "';";
						if(connection.State != ConnectionState.Open)
						{
							connection.Open();
						}
						count = Convert.ToInt32(cmd.ExecuteScalar());
						//账号密码正确
						if (count > 0)
						{
							User tempUser = FindUserByUsername(userList[0], name);
							if (tempUser != null)
							{
								byte[] msg = Encoding.UTF8.GetBytes("O" + GetCurrentTime() + "您的账号已经在别的设备上登录，本客户端已经下线");
								tempUser.socket.Send(msg);
								userOffLine(name);

							}
							userOnline(name, socketSend);
							ShowMsg(name + "登录成功！");
							result = "yes";
							byte[] send = Encoding.UTF8.GetBytes(result);
							socketSend.Send(send);
							CheckOfflineMessage(name);
							CheckOfflineFile(name);
						}
						else
						{
							result = "no";
							byte[] send = Encoding.UTF8.GetBytes(result);
							socketSend.Send(send);
						}

						//注册
					}
					else if (str.Substring(0, 1) == "R")
					{
						cmd = connection.CreateCommand();
						string result = "no";
						int count = -1;
						Hashtable ht = JsonConvert.DeserializeObject<Hashtable>(str.Substring(1));
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
										if(connection.State != ConnectionState.Open)
										{
											connection.Open();
										}
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
					//接收来自客户端的消息
					else if(str.Substring(0, 1) == "m")
					{
						User user = FindUserByIP(userList[0], currentKey);
						//如果没有找到，那么有可能是别的客户端，不是本地的客户端，那么过滤即可。
						if(user != null)
						{
							ShowMsg(user.username + "(" + str.Substring(1, 19) + "):" + str.Substring(20));
						}
					}
					else if (str.Substring(0, 1) == "P") //收到了大棚采集到的数据
					{
						int HouseNumber = Convert.ToInt32(str.Substring(2, 1));
						if(HouseNumber >= 1 && HouseNumber <= 6)
						{
							string Temp = str.Substring(9, 4);
							string Humidity = str.Substring(23, 4);
							string Light = Convert.ToInt32(str.Substring(34, 5)).ToString();
							int NodeId = Convert.ToInt32(str.Substring(43, 2));
							string dateTime = GetCurrentTime();
							try{
								if(connection.State != ConnectionState.Open)
								{
									connection.Open();
								}
								cmd = connection.CreateCommand();
								cmd.CommandText = "INSERT INTO TB_INFO" + "(NODE_Id, S_ID, TEMP, Humidity, LIGHT, Time)VALUES(@NodeId, @SId, @Temp, @Humidity, @Light, @Time)";
								cmd.Parameters.AddWithValue("@NodeId", NodeId);
								cmd.Parameters.AddWithValue("@SId", HouseNumber);
								cmd.Parameters.AddWithValue("@Temp", Temp);
								cmd.Parameters.AddWithValue("@Humidity", Humidity);
								cmd.Parameters.AddWithValue("@Light", Light);
								cmd.Parameters.AddWithValue("@Time", dateTime);
								//cmd.Parameters.AddWithValue("@Time", DateTime.Now.ToShortDateString());
								cmd.ExecuteNonQuery();
								for(int i = 0 ; i < userList[0].Count; i++)
								{
									User user = FindUserByUsername(userList[0], userList[0][i].username);
									if(user != null)
									{
										user.socket.Send(Encoding.UTF8.GetBytes(str));
									}
								}
							}
							catch(MySQLException e){
								MessageBox.Show("数据库异常：" + e.Message);
							}
						}
						else
						{
							continue;
						}

						byte[] arrMsg = Encoding.UTF8.GetBytes("P" + str); //将要发送的字符串转换成Utf-8字节数组 
						//收到大棚的数据以后给所有在线的发一下
						for(int i = 0; i < lbOnline.CheckedItems.Count; i ++)
						{
							if(lbOnline.CheckedItems[i].Group.Name == "online")
							{
								User user = FindUserByUsername(userList[0] ,lbOnline.CheckedItems[i].SubItems[1].Text);
								user.socket.Send(arrMsg);
							}
						}
					}
					else if (str.Substring(0, 1) == "c")
					{
						//wyc:我也不知道在干什么，就照着自己的想法改了改
						//服务器接收到客户端发给它下传zigbee光照阈值的指令消息处理
						//对应找相应系统的IP
						User user = FindUserByIP(userList[0] ,socketSend.RemoteEndPoint.ToString());
						if(user != null)
						{
							byte[] arrMSg = buffer.Take(9).Concat(Encoding.UTF8.GetBytes(user.username)).ToArray(); //将要发送的字符串转换成Utf-8字节数组; 
							foreach(User temp in userList[0])
							{
								if(user.username == temp.username)
								{
									continue;
								}
								temp.socket.Send(arrMSg);
							}
						}
					}
					else if(str.Substring(0, 1) == "S")
					{
						if(connection.State != ConnectionState.Open)
						{
							connection.Open();
						}
						try
						{
							string selectSql = "Set @i:=0; select(@i:= @i + 1) as 'id', Node_id, S_ID, Temp, Humidity, Light, Time from TB_INFO where TIME LIKE '"+ str.Substring(1) + "%' ORDER BY Time;  ";
							MySqlDataAdapter chaxun = new MySqlDataAdapter(selectSql, connection);
							DataSet ds = new DataSet();
							chaxun.Fill(ds, "table1");
							string jsonData = JsonConvert.SerializeObject(ds.Tables[0]);
							byte[] btSend = Encoding.UTF8.GetBytes("S" + jsonData);
							User user = FindUserByIP(userList[0], currentKey);
							user.socket.Send(btSend, 0, btSend.Length, SocketFlags.None);
						}
						catch(MySQLException e)
						{
							MessageBox.Show("异常" + e.Message);
						}
					}
					else
					{
						continue;
					}
				}
				catch
				{
					string ip = socketSend.RemoteEndPoint.ToString();
					User user = FindUserByIP(userList[0], ip);
					if(user != null)
					{
						userOffLine(user.username);

					}
				}
				if (connection.State == ConnectionState.Open)
				{
					connection.Close();
				}
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
			{
				MessageBox.Show("未知错误：选择文件失败！");
			}
		}
		//发送已经选择了的文件
		private void btnSendFile_Click(object sender, EventArgs e)
		{
			//得到文件路径和文件名
			string path = txtPath.Text;
			//检查是否选择了客户端
			if(lbOnline.CheckedItems.Count > 0)
			{
				if (path.Trim().Length == 0)
				{
					MessageBox.Show("请选择需要发送的文件！");
				}
				else
				{
					string sendTime = GetCurrentTime();
					for (int i = 0; i < lbOnline.CheckedItems.Count; i++)
					{
						if (lbOnline.CheckedItems[i].Group.Name == "online")
						{
							SendFile(path, lbOnline.CheckedItems[i].SubItems[1].Text, sendTime ,0);
						}
						else
						{
							SendFile(path, lbOnline.CheckedItems[i].SubItems[1].Text, sendTime, 1);
						}
					}
				}
			}
			else
			{
				MessageBox.Show("请选择客户端");
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
				if(lbOnline.CheckedItems.Count == 0)
				{
					MessageBox.Show("请选择客户端");
				}
				else
				{
					for(int i = 0; i < lbOnline.CheckedItems.Count; i++)
					{
						User user = FindUserByUsername(userList[0] ,lbOnline.CheckedItems[i].SubItems[1].Text);
						if(user != null)
						{
							user.socket.Send(buffer);
						}
					}
				}
			}
			catch
			{ }
		}

		//发消息
		private void btnSend_Click(object sender, EventArgs e)
		{
			//检查是否选择了客户端
			if (lbOnline.CheckedItems.Count > 0 )
			{
				if (txtMsg.Text.Trim() == "")
				{
					MessageBox.Show("发送的消息不能为空，请重新输入!");
				}
				else
				{
					string msg = txtMsg.Text.Trim();					
					SendMsg();
					txtMsg.Clear();
				}
			}
			else
			{
				MessageBox.Show("请选择客户端");
			}
		}
		private void CheckOfflineMessage(string username)
		{
			using (MySqlConnection connection = new MySqlConnection(MyConnectionString))
			{
				try
				{
					connection.Open();//连接数据库
					MySqlCommand cmd = connection.CreateCommand();
					cmd.CommandText = "SELECT * FROM TB_OFFLINE_MESSAGE WHERE USER_NAME = '" + username + "'  ORDER BY SEND_TIME";
					MySqlDataReader reader = cmd.ExecuteReader();
					while (reader.Read())
					{
						string strMsg = reader["offline_message"].ToString();		//用户编辑的消息
						string sendTime = reader["send_time"].ToString();			//用户发送的时间
						byte[] sendMsg = buildSendMsg(strMsg, sendTime);
						User user = FindUserByUsername(userList[0], username); 
						user.socket.Send(sendMsg);
					}
					reader.Close();
					reader.Dispose();
					cmd.CommandText = "DELETE FROM TB_OFFLINE_MESSAGE WHERE USER_NAME = '" + username + "';";
					cmd.ExecuteNonQuery();
				}
				catch (MySQLException e)
				{
					MessageBox.Show("异常：" + e.Message);
				}
			}
		}
		byte[] buildSendMsg(string strMsg, string sendTime)
		{
			byte[] arrMsg = Encoding.UTF8.GetBytes(sendTime + strMsg);		//消息和时间
			byte[] msgLength = BitConverter.GetBytes(arrMsg.Length);		//消息的长度
			byte[] flagByte = new byte[1];									//标志位
			flagByte[0] = 0x6D;			//"m"
			byte[] sendMsg = new byte[5 + arrMsg.Length];				//最终发给我客户端的msg					
			sendMsg = flagByte.Concat(msgLength).Concat(arrMsg).ToArray();	//标志位 + 长度 + 时间 + 消息
			return sendMsg;
		}
		//发送消息
		private void SendMsg()
		{
			string msg = txtMsg.Text.Trim();			//消息
			string sendTime = GetCurrentTime();		//发送时间
			byte[] arrMsg = buildSendMsg(msg, sendTime);
			for (int i = 0; i < lbOnline.CheckedItems.Count; i++)
			{
				//在线
				if(lbOnline.CheckedItems[i].Group.Name == "online")
				{
					User user = FindUserByUsername(userList[0], lbOnline.CheckedItems[i].SubItems[1].Text);
					user.socket.Send(arrMsg);
					ShowMsg("向" + user.username + "发送(" + sendTime + "): " + msg);
				}
				//离线
				else
				{
					using (MySqlConnection connection = new MySqlConnection(MyConnectionString))
					{
						try
						{
							//离线消息存到数据库里面，等客户登录的时候再给客户发过去
							connection.Open();//连接数据库
							MySqlCommand cmd = connection.CreateCommand();
							string username = lbOnline.CheckedItems[i].SubItems[1].Text;
							cmd.CommandText = "INSERT INTO TB_OFFLINE_MESSAGE(USER_NAME, OFFLINE_MESSAGE, SEND_TIME)VALUES(@username, @offlineMessage, @sendTime);";
							cmd.Parameters.AddWithValue("@username", username);
							cmd.Parameters.AddWithValue("@offlineMessage", msg);
							cmd.Parameters.AddWithValue("@sendTime", sendTime);
							cmd.ExecuteNonQuery();
						}
						catch (MySQLException e)
						{
							MessageBox.Show("异常：" + e.Message);
						}
					}			
				}
			}
		}		
		//获取当前的时间
		private string GetCurrentTime()
		{
			return DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
		}
		//发送文件
		private bool SendFile(string path, string username, string sendTime, int state)
		{
			using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				try
				{
					//根据path获取到文件名
					string fileName = path.Substring(path.LastIndexOf("\\"), path.Length - path.LastIndexOf("\\"));
					//最大文件大小10M	100表示的是文件名和时间等杂项
					if(fs.Length > 10 * 1024 * 1024 - 100)
					{
						MessageBox.Show("所选文件不能超过10M！");
						return false;
					}
					byte[] fileBuffer = new byte[fs.Length];			//文件字节流
					fs.Read(fileBuffer, 0, fileBuffer.Length);			//读文件
					byte[] btFileName = Encoding.UTF8.GetBytes(fileName);			//文件名字节流
					byte[] btFileTimeLength = new byte[4];
					byte[] btTime = Encoding.UTF8.GetBytes(sendTime);
					btFileTimeLength = BitConverter.GetBytes(btFileName.Length + btTime.Length);
					byte[] flagByte = new byte[1];									//标志位
					flagByte[0] = 0x66;									//发送文件的标记	"f"
					byte[] btFileLength = BitConverter.GetBytes(4 + btTime.Length +  btFileName.Length + fileBuffer.Length);
					//发送的内容为 "f" + 文件和文件名和时间总长度 +文件名和时间长度 + 时间 + 文件名 + 文件
					byte[] sendBuffer = new byte[1 + 4 + 4 + btTime.Length + btFileName.Length + fileBuffer.Length];
					sendBuffer = flagByte.Concat(btFileLength).Concat(btFileTimeLength).Concat(btTime).Concat(btFileName).Concat(fileBuffer).ToArray();
					
					//在线发送
					if(state == 0)
					{
						User user = FindUserByUsername(userList[state], username);
						//向客户端发送文件
						if (user != null)
						{
							user.socket.Send(sendBuffer, 0, sendBuffer.Length, SocketFlags.None);
							ShowMsg(sendTime + " 向" + user.username + "发送文件:" + fileName);
							fs.Close();
							//等待来自客户端的回应，是否发送成功
							byte[] result = new byte[20];
							int len = user.socket.Receive(result);
							if(Encoding.UTF8.GetString(result) == "FileDone")
							{
								ShowMsg("发送成功");
								return true;
							}
							else
							{
								MessageBox.Show("文件发送失败，请联系管理员解决！");
								return false;
							}
						}
						else
						{
							MessageBox.Show("文件发送失败，请联系管理员解决！");
							return false;
						}
					}
					//离线发送
					else
					{
						using (MySqlConnection connection = new MySqlConnection(MyConnectionString))
						{
							try
							{
								//离线消息存到数据库里面，等客户登录的时候再给客户发过去
								connection.Open();//连接数据库
								MySqlCommand cmd = connection.CreateCommand();
								cmd.CommandText = "INSERT INTO TB_OFFLINE_FILE(USER_NAME, OFFLINE_FILEPATH, SEND_TIME)VALUES(@username, @offlineFilepath, @sendTime);";
								cmd.Parameters.AddWithValue("@username", username);
								cmd.Parameters.AddWithValue("@offlineFilepath", path);
								cmd.Parameters.AddWithValue("@sendTime", sendTime);
								if(cmd.ExecuteNonQuery() > 0)
								{
									return true;
								}
								else
								{
									return false;
								}
							}
							catch (MySQLException e)
							{
								MessageBox.Show("异常：" + e.Message);
								return false;
							}
						}
					}
				}
				catch
				{
					MessageBox.Show("文件发送失败，请联系管理员解决！");
					return false;
				}
			}
		}
		private void CheckOfflineFile(string username)
		{
			using (MySqlConnection connection = new MySqlConnection(MyConnectionString))
			{
				try
				{
					connection.Open();//连接数据库
					MySqlCommand cmd = connection.CreateCommand();
					cmd.CommandText = "SELECT * FROM TB_OFFLINE_FILE WHERE USER_NAME = '" + username + "'  ORDER BY SEND_TIME";
					MySqlDataReader reader = cmd.ExecuteReader();
					string delIds = "";
					while (reader.Read())
					{
						string path = reader["offline_filepath"].ToString();		//用户编辑的消息
						string sendTime = reader["send_time"].ToString();			//用户发送的时间
						if(SendFile(path, username, sendTime, 0))
						{
							delIds += reader["id"].ToString() + "-";
						}
					}
					reader.Close();
					string[] arrayIds = delIds.Split('-');
					cmd.CommandText = "DELETE FROM TB_OFFLINE_FILE WHERE ";
					for(int i = 0; i < arrayIds.Length; i ++)
					{
						if (i == 0)
						{
							cmd.CommandText += "ID = '" + arrayIds[i] + "'";
						}
						else
						{
							cmd.CommandText += "OR ID = '" + arrayIds[i] + "'";
						}
					}
					cmd.ExecuteNonQuery();
					if(connection.State == ConnectionState.Open)
					{
						connection.Close();
					}
				}
				catch (MySQLException e)
				{
					MessageBox.Show("异常：" + e.Message);
				}
			}
		}
		private void ipPort_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == 13)
			{
				this.btnStart.Focus();
				this.btnStart.PerformClick();
			}
		}
		
		private User FindUserByUsername(List<User> list, string name)
		{
			foreach(User user in list)
			{
				if (user.username == name)
				{
					return user;
				}
			}
			return null;
		}
		private int FindUserIndexByUserName(List<User> list, string name)
		{
			for(int i = 0 ; i < list.Count; i++)
			{
				if(list[i].username == name)
				{
					return i;
				}
			}
			return -1;
		}
		private User FindUserByIP(List<User> list, string ip)
		{
			foreach(User user in list)
			{
				if(user.socket.RemoteEndPoint.ToString() == ip)
				{
					return user;
				}
			}
			return null;
		}
	}
}
