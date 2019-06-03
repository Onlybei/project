using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.IO.Ports;
using Newtonsoft.Json;

namespace 葡萄霜霉病防控测试系统1
{
	public partial class 葡萄霜霉病防控系统 : Form
	{

		private Socket socketClient = Login.ClientSocket;
		private Thread threadClient = null;//创建用于接收服务器端消息的线程
		private string username = Login.username;
		public 葡萄霜霉病防控系统()
		{
			InitializeComponent();
			TextBox.CheckForIllegalCrossThreadCalls = false;
			ShowMsg("连接成功");
			//开启一个新的线程用来接收服务端发来的消息
			threadClient = new Thread(ReceiveAll);//Receive是demo里的RecMsg
			threadClient.IsBackground = true;
			threadClient.Start();
		}
		/// <summary>
		/// 不停接收服务端发来的消息
		///Receive是demo的RecMsg
		///
		void ReceiveAll()
		{
			while (true)
			{
				try
				{
					byte[] buffer = new byte[1024 * 1024 * 3];//定义一个3M的缓冲区
					//返回实际接收到的有效字节数
					int r = socketClient.Receive(buffer);
					if (r == 0)
					{
						break;
					}
					string s = Encoding.UTF8.GetString(buffer, 0, r);
					//ShowMsg(s);
					if (s.Substring(0, 1) == "P")       //表示接收到了大棚采集到的数据 
					{
						int number = Convert.ToInt32(s.Substring(2, 1));
						int nodeId = Convert.ToInt32(s.Substring(43, 2));
						if (number >= 1 && number <= 6)
						{
							if (nodeId == 1)
							{
								textBox2.Text = s.Substring(9, 4);
								textBox3.Text = s.Substring(23, 4);
								textBox4.Text = Convert.ToInt32(s.Substring(34, 5)).ToString();
							}
							else if (nodeId == 2)
							{
								textBox10.Text = s.Substring(9, 4);
								textBox9.Text = s.Substring(23, 4);
								textBox8.Text = Convert.ToInt32(s.Substring(34, 5)).ToString();
							}
							else if (nodeId == 3)
							{
								textBox18.Text = s.Substring(9, 4);
								textBox17.Text = s.Substring(23, 4);
								textBox16.Text = Convert.ToInt32(s.Substring(34, 5)).ToString();
							}
							else if (nodeId == 4)
							{
								textBox14.Text = s.Substring(9, 4);
								textBox13.Text = s.Substring(23, 4);
								textBox12.Text = Convert.ToInt32(s.Substring(34, 5)).ToString();
							}
						}

					}
					//下线操作
					else if (s.Substring(0, 1) == "O")
					{
						this.DialogResult = DialogResult.Retry;
						MessageBox.Show(s.Substring(1));
					}
					//光照阈值
					else if (s.Substring(0, 1) == "c")
					{
						int hum = BitConverter.ToInt32(buffer.Skip(1).Take(4).ToArray(), 0);
						int light = BitConverter.ToInt32(buffer.Skip(5).Take(4).ToArray(), 0);
						cboHumidityYZ.SelectedIndex = cboHumidityYZ.Items.IndexOf(hum.ToString());
						cboLightYZ.SelectedIndex = cboLightYZ.Items.IndexOf(light.ToString());
						string username = Encoding.UTF8.GetString(buffer.Skip(4).Take(r - 9).ToArray());
						ShowMsg(GetCurrentTime() + "：用户'" + username + "'修改相对湿度为" + hum.ToString() + "，光照强度为" + light.ToString());
					}
					//收到来自服务器的消息
					else if (s.Substring(0, 1) == "m")
					{
						//解包
						int msgLength = 0;			//消息长度
						int ReMainSize = r;			//剩下的长度
						int startIndex = 0;			//开始的长度
						while (true)
						{
							byte[] btMsgLength = buffer.Skip(startIndex + 1).Take(4).ToArray();
							msgLength = BitConverter.ToInt32(btMsgLength, 0);
							if (msgLength <= ReMainSize - 5)
							{
								//说明后面还有
								byte[] currentMsg = buffer.Skip(startIndex + 5).Take(msgLength).ToArray();
								string str = Encoding.UTF8.GetString(currentMsg);
								ShowMsg("服务器（" + str.Substring(0, 19) + "）: " + str.Substring(19));
								ReMainSize = r - msgLength - 5;
								startIndex += msgLength + 5;
							}
							else
							{
								break;
							}
						}
					}
					//收到文件
					else if (s.Substring(0, 1) == "f")
					{
						//解包
						int fileLength = 0;
						int ReMainSize = r;
						int startIndex = 0;
						while (true)
						{
							byte[] btFileLength = buffer.Skip(startIndex + 1).Take(4).ToArray();
							fileLength = BitConverter.ToInt32(btFileLength, 0);
							if (fileLength <= ReMainSize - 5)
							{
								//说明后面还有
								byte[] btRecieve = buffer.Skip(startIndex + 5).Take(fileLength).ToArray();
								ReceiveFile(btRecieve, fileLength);
								ReMainSize = r - fileLength - 5;
								startIndex += fileLength + 5;
							}
							else
							{
								break;
							}
						}
					}
					//接收查询到的数据
					else if (s.Substring(0, 1) == "S")
					{
						this.dataGridView1.DataSource = JsonConvert.DeserializeObject(s.Substring(1));
					}
					else
					{

					}
				}
				catch
				{

				}
			}
		}   //不停接收服务端发来的消息

		private void btnConnect_Click(object sender, EventArgs e)    //打开串口工作事件
		{
			try
			{
				serialPort1.PortName = cboPort.Text;
				serialPort1.BaudRate = Convert.ToInt32(cboBaudrate.Text);//在波特率手动添加集合中 是字符串 需要转为十进制数
				serialPort1.Open();
				btnConnect.Enabled = false;//这句表示已经打开串口就不能再打开 打开串口不能使用
				btnEndPort.Enabled = true;//可以关闭串口
			}
			catch
			{
				MessageBox.Show("端口错误，请检查端口", "错误");
			}
		}

		private void btnEndPort_Click(object sender, EventArgs e)    //关闭串口工作事件
		{
			try
			{
				serialPort1.Close();//关闭串口
				btnConnect.Enabled = true;//打开串口按钮可以使用
				btnEndPort.Enabled = false;//关闭串口按钮不可用
			}
			catch
			{
				//一般不会出错
			}
		}

		private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)//串口连接时候的数据接收事件（仅字符无数值）
		{
			string str = serialPort1.ReadExisting();//字符串方式读
			ShowMsg(str);
		}

		private void btnClear_Click(object sender, EventArgs e)      //串口接收数据txtlog清屏
		{
			MsgBox.Clear();
		}

		private void 葡萄霜霉病防控系统_Load(object sender, EventArgs e)
		{
			dateTimePicker1.Format = DateTimePickerFormat.Custom; //设置为显示格式为自定义
			dateTimePicker1.CustomFormat = "yyyy/MM/dd"; //设置显示格式
			Control.CheckForIllegalCrossThreadCalls = false;
			for (int j = 1; j < 9; j++)         //添加串口号com1~com8
			{
				cboPort.Items.Add("COM" + j.ToString());
			}
			cboPort.Text = "COM1";            //默认端口号为COM1
			cboBaudrate.Text = "4800";        //默认波特率4800
			txtWelcome.Text = "您好，" + username;
			serialPort1.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);  //必须手动添加事件
		}

		private void btnExit_Click(object sender, EventArgs e)  //退出系统
		{
			DialogResult result = MessageBox.Show("你确定退出葡萄霜霉病客户端吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
			if(result == DialogResult.OK)
			{
				this.DialogResult = DialogResult.Cancel;
			}
			else
			{
				//什么也不干
			}
		}

		private void btnSendPort_Click(object sender, EventArgs e)//串口发送按钮
		{
			byte[] Data = new byte[1];
			if (serialPort1.IsOpen)//判断串口是否打开 如果打开执行下一步操作
			{
				if (txtMsgport.Text != "")
				{
					try
					{
						serialPort1.WriteLine(txtMsgport.Text);//写数据
					}
					catch
					{
						MessageBox.Show("串口数据写入错误", "错误");//出错提示
						serialPort1.Close();//出错后关闭串口
						btnConnect.Enabled = true;//打开串口的按钮可用
						btnEndPort.Enabled = false;//关闭串口的按钮不可用
					}
				}

			}

		}

		private void btnChaXun_Click(object sender, EventArgs e)
		{
			byte[] btSend = Encoding.UTF8.GetBytes("S" + dateTimePicker1.Text);
			socketClient.Send(btSend);
			//1M缓冲区
		}
		/// <summary>
		/// 下传修改参数指令 
		/// 原demo里的发送消息 这里用按钮触发 也就是发送消息
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnChangeYZ_Click(object sender, EventArgs e)
		{
			if (cboHumidityYZ.Text.Trim() == string.Empty || cboLightYZ.Text.Trim() == string.Empty)
			{
				MessageBox.Show("请选择您需要更改的湿度和光照强度阈值");
			}
			else
			{
				byte[] byflag = new byte[1];
				byflag[0] = 0x63;
				byte[] hum = new byte[4];
				hum = BitConverter.GetBytes(Convert.ToInt32(cboHumidityYZ.Text.Trim()));
				byte[] light = new byte[4];
				light = BitConverter.GetBytes(Convert.ToInt32(cboLightYZ.Text.Trim()));

				byte[] arrMsg = byflag.Concat(hum).Concat(light).ToArray();
				socketClient.Send(arrMsg);
				ShowMsg(GetCurrentTime() +  "：您修改相对湿度为" + cboHumidityYZ.Text.Trim() + "，光照强度为" + cboLightYZ.Text.Trim());
				//sendMsgBox.Clear();
			}
		}
		//向服务器发送消息
		private void BtnSendMsg_Click(object sender, EventArgs e)
		{
			string strMsg = sendMsgBox.Text.Trim();
			if (strMsg == "")
			{
				MessageBox.Show("发送消息为空，请重新输入！");
			}
			else
			{
				try
				{
					ShowMsg(username + "( " + GetCurrentTime() + "):" + strMsg);
					byte[] arrMsg = Encoding.UTF8.GetBytes("m" + GetCurrentTime() + strMsg);
					socketClient.Send(arrMsg);
					sendMsgBox.Clear();
				}
				catch
				{
					MessageBox.Show("请求服务器超时！");
				}
			}
		}
		private void ShowMsg(string str)
		{
			MsgBox.AppendText(str + "\r\n");
		}
		private string GetCurrentTime()
		{
			return DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
		}
		//接收文件
		//四个参数为，后台发来的字节流， 字符串， 字节流长度， 文件名的长度
		private void ReceiveFile(byte[] buffer, int realLength)
		{
			Thread td = new Thread(() => {
				//4表示长度,19表示时间
				int fileTimeLength = BitConverter.ToInt32(buffer.Take(4).ToArray(), 0);
				string sendTime = Encoding.UTF8.GetString(buffer.Skip(4).Take(19).ToArray());
				string path = Encoding.UTF8.GetString(buffer.Skip(4 + 19).Take(fileTimeLength - 19).ToArray());
				string fileNameSuffix = path.Substring(path.LastIndexOf("."));
				string fileName = path.Substring(path.LastIndexOf("\\") + 1);
				//将保存对话框的属性赋对应的值
				SaveFileDialog sfd = new SaveFileDialog();
				sfd.Filter = "(*" + fileNameSuffix + ")|*" + fileNameSuffix + "";//设置文件类型
				sfd.FilterIndex = 1;//设置默认文件类型显示顺序 
				sfd.RestoreDirectory = true;//保存对话框是否记忆上次打开的目录		
				sfd.FileName = fileName;
				//打开保存文件对话框
				if (sfd.ShowDialog() == DialogResult.OK)
				{
					string savePath = sfd.FileName;
					using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
					{
						//buffer 的内容
						//文件名和时间长度 +  时间 + 文件 + 文件名
						fs.Write(buffer, 4 + fileTimeLength, realLength - 4 - fileTimeLength);
						fs.Flush();
						fs.Close();
					}
					//获取文件名和路径
					string fName = savePath.Substring(savePath.LastIndexOf("\\") + 1);
					string fPath = savePath.Substring(0, savePath.LastIndexOf("\\"));
					ShowMsg(GetCurrentTime() + ": 您已经成功接收了" + sendTime + "由服务器发送的文件：“" + fName + "”,保存的路径为：“" + fPath + "”");
					socketClient.Send(Encoding.UTF8.GetBytes("FileDone"));
				}
			});
			td.IsBackground = true;
			td.SetApartmentState(ApartmentState.STA);
			td.Start();
		}
	}
}


 