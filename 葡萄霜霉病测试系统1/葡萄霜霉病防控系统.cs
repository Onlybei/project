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
using System.IO;
using System.IO.Ports;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace 葡萄霜霉病防控测试系统1
{
    public partial class 葡萄霜霉病防控系统 : Form
    {
        public 葡萄霜霉病防控系统()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;
        }

        private void cboYear_SelectedIndexChanged(object sender, EventArgs e)  //日期选择某月
        {
            //添加之前清空之前的内容
            cboMonth.Items.Clear();
            for (int i = 1; i <= 12; i++)
            {
                cboMonth.Items.Add(i);
            }
        }

        private void cboMonth_SelectedIndexChanged(object sender, EventArgs e)  //日期选择器
        {
            cboDay.Items.Clear();
            int days = 0;//存储天数
            //获得月份
            string strMonth = cboMonth.SelectedItem.ToString();
            string strYear = cboYear.SelectedItem.ToString();
            int year = Convert.ToInt32(strYear);
            int month = Convert.ToInt32(strMonth);
            switch (month)
            {
                case 1: days = 31;
                    break;
                case 2:
                    if (year % 400 == 0 || (year % 4 == 0 && year % 100 != 0))
                    { days = 29; }
                    else
                    { days = 28; }
                    break;
                case 3: days = 31;
                    break;
                case 4: days = 30;
                    break;
                case 5: days = 31;
                    break;
                case 6: days = 30;
                    break;
                case 7: days = 31;
                    break;
                case 8: days = 31;
                    break;
                case 9: days = 30;
                    break;
                case 10: days = 31;
                    break;
                case 11: days = 30;
                    break;
                case 12: days = 31;
                    break;
            }
            for (int i = 1; i <= days; i++)
            {
                cboDay.Items.Add(i);
            }
        } //日期选择某天

        Socket socketClient = null;
        Thread threadClient = null;//创建用于接收服务器端消息的线程
        DateTime DateTime = new System.DateTime();

        private void btnStart_Click(object sender, EventArgs e)  //TCP建立连接//demo 的btnConnect
        {
            try
            {
                //创建负责通信的socket
                socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(txtServer.Text);
                IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(txtPort.Text));
                //获得远程连接服务器应用程序的Ip地址和端口号
                socketClient.Connect(point);
                ShowMsg("连接成功");
                //开启一个新的线程用来接收服务端发来的消息
                threadClient = new Thread(Receive);//Receive是demo里的RecMsg
                threadClient.IsBackground = true;
                threadClient.Start();
            }
            catch
            { }
        }

        void ShowMsg(string str)    //Tcp socket聊天接收框显示数据
        {
            //  txtLog.AppendText(str + "\r\n");
            // textBox2.Text = str;
        }

        string MyConnectionString = "Server=localhost;Database=testdb;Uid=root;Pwd=123456;";
        MySqlCommand cmd;
        MySqlConnection conn = new MySqlConnection();

        private void btnSend_Click(object sender, EventArgs e)//Tcp socket聊天发送框发送数据
        {

            string str = txtMsg.Text.Trim();
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
            socketClient.Send(buffer);
        }

        private void LoadData()
        {

        }
        /// <summary>
        /// 不停接收服务端发来的消息
        ///Receive是demo的RecMsg
        ///
        /// 
        void Receive()
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
                    ShowMsg(s);
                    if (s.Substring(0,1) == "P")       //表示接收到了大棚采集到的数据 
                    {
                        switch (s.Substring(2, 1))
                        {
                            case "1": if (s.Substring(43, 1) == "1")
                                {
                                    textBox2.Text = s.Substring(9, 4);
                                    textBox3.Text = s.Substring(23, 4);
                                    textBox4.Text = s.Substring(34, 5);
                                }
                                else if (s.Substring(43, 1) == "2")
                                {
                                    textBox10.Text = s.Substring(9, 4);
                                    textBox9.Text = s.Substring(23, 4);
                                    textBox8.Text = s.Substring(34, 5);
                                }
                                else if (s.Substring(43, 1) == "3")
                                {
                                    textBox18.Text = s.Substring(9, 4);
                                    textBox17.Text = s.Substring(23, 4);
                                    textBox16.Text = s.Substring(34, 5);
                                }
                                else if (s.Substring(43, 1) == "4")
                                {
                                    textBox14.Text = s.Substring(9, 4);
                                    textBox13.Text = s.Substring(23, 4);
                                    textBox12.Text = s.Substring(34, 5);
                                }
                                break;
                            case "2": if (s.Substring(43, 1) == "1")
                                {
                                    textBox2.Text = s.Substring(9, 4);
                                    textBox3.Text = s.Substring(23, 4);
                                    textBox4.Text = s.Substring(34, 5);
                                }
                                else if (s.Substring(43, 1) == "2")
                                {
                                    textBox10.Text = s.Substring(9, 4);
                                    textBox9.Text = s.Substring(23, 4);
                                    textBox8.Text = s.Substring(34, 5);
                                }
                                else if (s.Substring(43, 1) == "3")
                                {
                                    textBox18.Text = s.Substring(9, 4);
                                    textBox17.Text = s.Substring(23, 4);
                                    textBox16.Text = s.Substring(34, 5);
                                }
                                else if (s.Substring(43, 1) == "4")
                                {
                                    textBox14.Text = s.Substring(9, 4);
                                    textBox13.Text = s.Substring(23, 4);
                                    textBox12.Text = s.Substring(34, 5);
                                }
                                break;
                            case "3": if (s.Substring(43, 1) == "1")
                                {
                                    textBox2.Text = s.Substring(9, 4);
                                    textBox3.Text = s.Substring(23, 4);
                                    textBox4.Text = s.Substring(34, 5);
                                }
                                else if (s.Substring(43, 1) == "2")
                                {
                                    textBox10.Text = s.Substring(9, 4);
                                    textBox9.Text = s.Substring(23, 4);
                                    textBox8.Text = s.Substring(34, 5);
                                }
                                else if (s.Substring(43, 1) == "3")
                                {
                                    textBox18.Text = s.Substring(9, 4);
                                    textBox17.Text = s.Substring(23, 4);
                                    textBox16.Text = s.Substring(34, 5);
                                }
                                else if (s.Substring(43, 1) == "4")
                                {
                                    textBox14.Text = s.Substring(9, 4);
                                    textBox13.Text = s.Substring(23, 4);
                                    textBox12.Text = s.Substring(34, 5);
                                }
                                break;
                            case "4": if (s.Substring(43, 1) == "1")
                                {
                                    textBox2.Text = s.Substring(9, 4);
                                    textBox3.Text = s.Substring(23, 4);
                                    textBox4.Text = s.Substring(34, 5);
                                }
                                else if (s.Substring(43, 1) == "2")
                                {
                                    textBox10.Text = s.Substring(9, 4);
                                    textBox9.Text = s.Substring(23, 4);
                                    textBox8.Text = s.Substring(34, 5);
                                }
                                else if (s.Substring(43, 1) == "3")
                                {
                                    textBox18.Text = s.Substring(9, 4);
                                    textBox17.Text = s.Substring(23, 4);
                                    textBox16.Text = s.Substring(34, 5);
                                }
                                else if (s.Substring(43, 1) == "4")
                                {
                                    textBox14.Text = s.Substring(9, 4);
                                    textBox13.Text = s.Substring(23, 4);
                                    textBox12.Text = s.Substring(34, 5);
                                }
                                break;
                            case "5": if (s.Substring(43, 1) == "1")
                                {
                                    textBox2.Text = s.Substring(9, 4);
                                    textBox3.Text = s.Substring(23, 4);
                                    textBox4.Text = s.Substring(34, 5);
                                }
                                else if (s.Substring(43, 1) == "2")
                                {
                                    textBox10.Text = s.Substring(9, 4);
                                    textBox9.Text = s.Substring(23, 4);
                                    textBox8.Text = s.Substring(34, 5);
                                }
                                else if (s.Substring(43, 1) == "3")
                                {
                                    textBox18.Text = s.Substring(9, 4);
                                    textBox17.Text = s.Substring(23, 4);
                                    textBox16.Text = s.Substring(34, 5);
                                }
                                else if (s.Substring(43, 1) == "4")
                                {
                                    textBox14.Text = s.Substring(9, 4);
                                    textBox13.Text = s.Substring(23, 4);
                                    textBox12.Text = s.Substring(34, 5);
                                }
                                break;
                            case "6": if (s.Substring(43, 1) == "1")
                                {
                                    textBox2.Text = s.Substring(9, 4);
                                    textBox3.Text = s.Substring(23, 4);
                                    textBox4.Text = s.Substring(34, 5);
                                }
                                else if (s.Substring(43, 1) == "2")
                                {
                                    textBox10.Text = s.Substring(9, 4);
                                    textBox9.Text = s.Substring(22, 4);
                                    textBox8.Text = s.Substring(33, 5);
                                }
                                else if (s.Substring(42, 1) == "3")
                                {
                                    textBox18.Text = s.Substring(8, 4);
                                    textBox17.Text = s.Substring(22, 4);
                                    textBox16.Text = s.Substring(33, 5);
                                }
                                else if (s.Substring(42, 1) == "4")
                                {
                                    textBox14.Text = s.Substring(8, 4);
                                    textBox13.Text = s.Substring(22, 4);
                                    textBox12.Text = s.Substring(33, 5);
                                }
                                break;
                        }
                    }
                    else if (s.Substring(0, 1) == "C")
                    {
                    }
                    else
                    {
                    }
                }
                catch
                {

                }
                //if (buffer[0] == 0)//接收到的是文字消息
                //{
                //    string s = Encoding.UTF8.GetString(buffer, 1, r - 1);//不用解析第一位的标志位
                //    ShowMsg(socketSend.RemoteEndPoint + ":" + s);
                //}
                //else if (buffer[0] == 1)
                //{
                //    SaveFileDialog sfd = new SaveFileDialog();
                //    sfd.InitialDirectory = @"C:\Users\Administrator\Desktop";
                //    sfd.Title = "请选择要保存的文件";
                //    sfd.Filter = "所有文件|*.*";
                //    sfd.ShowDialog(this);
                //    string path = sfd.FileName;
                //    using (FileStream fsWrite = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                //    {
                //        fsWrite.Write(buffer, 1, r - 1);
                //        MessageBox.Show("保存成功");
                //    }
                //}


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
            //      txtLog.AppendText(str);
        }

        private void btnClear_Click(object sender, EventArgs e)      //串口接收数据txtlog清屏
        {
            //      txtLog.Text = "";//清屏
        }

        private void 葡萄霜霉病防控系统_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom; //设置为显示格式为自定义
            dateTimePicker1.CustomFormat = "yyyy-MM-dd"; //设置显示格式
            Control.CheckForIllegalCrossThreadCalls = false;
            int year = DateTime.Now.Year;
            for (int i = year; i >= 2016; i--)      //添加年份 而且是倒序
            {
                cboYear.Items.Add(i);
            }
            for (int j = 1; j < 9; j++)         //添加串口号com1~com8
            {
                cboPort.Items.Add("COM" + j.ToString());
            }
            cboPort.Text = "COM1";            //默认端口号为COM1
            cboBaudrate.Text = "4800";        //默认波特率4800

            serialPort1.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);  //必须手动添加事件
        }

        private void btnExit_Click(object sender, EventArgs e)  //退出系统
        {
            //需要获得当前主窗体的对象才能关闭所有  要关掉FORM1 这里声明的FORM1与上面的FORM1不是同一个
            // this.close();// 关闭了FORM3
            //借助test.cs的字段_fr1Test 静态对象全局共享 存放原来Form1
            // test._exitAll.Close();
            // this.Dispose();
            Login.ActiveForm.Close();


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

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }



        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void btnChaXun_Click(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(MyConnectionString);
            MySqlCommand cmd = connection.CreateCommand();

            connection.Open();
            try
            {
                MySqlDataAdapter chaxun = new MySqlDataAdapter("select * from testtable where [Time]=format([DateTime],'yyyy-mm-dd')'" + dateTimePicker1.Text.ToString() + "'", connection);
                DataSet ds = new DataSet();
                chaxun.Fill(ds, "table1");
                this.dataGridView1.DataSource = ds.Tables[0];
                conn.Close();
            }
            catch
            {

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            //this.BackColor = Color.Transparent;
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
                string strMsg = "Change:HumidityYZ is " + cboHumidityYZ.Text.Trim() + ",LightYZ is " + cboLightYZ.Text.Trim();
                byte[] arrMsg = System.Text.Encoding.UTF8.GetBytes(strMsg);
                byte[] arrSendMsg = new byte[arrMsg.Length];
                Buffer.BlockCopy(arrMsg, 0, arrSendMsg, 0, arrMsg.Length);
                socketClient.Send(arrSendMsg);
                ShowMsg(strMsg);
                txtMsg.Clear();
            }
        }

        

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

      
        
    }
}


 