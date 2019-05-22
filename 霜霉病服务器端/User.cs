using System;
using System.Net.Sockets;
using System.Threading;

namespace ClientUser
{
	class User
	{
		public string username = "";
		public Socket socket = null;
		public Thread thread = null;
		public string trueName = "";
		public string telephone = "";
		public string company = "";
		public string state = "offline";

		public User(string username, Socket socket, Thread thread, string trueName, string telephone, string company, string state)
		{
			this.username = username;
			this.socket = socket;
			this.thread = thread;
			this.trueName = trueName;
			this.telephone = telephone;
			this.company = company;
			this.state = state;
		}
	}
}
