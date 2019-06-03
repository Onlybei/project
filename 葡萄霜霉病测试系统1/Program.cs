﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using 葡萄霜霉病防控测试系统1;

namespace 葡萄霜霉病测试系统1
{
    static class Program
    {
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]

		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new Login());
			Login login = new Login();
			while(true)
			{
				login.ShowDialog();
				while (true)
				{
					if (login.DialogResult == DialogResult.Cancel)
					{
						return;
					}
					//注册
					if (login.DialogResult == DialogResult.No)
					{
						Register register = new Register();
						login.DialogResult = DialogResult.None;
						register.ShowDialog();
						//返回登录
						if (register.DialogResult == DialogResult.Retry)
						{
							register.DialogResult = DialogResult.None;
							login.ShowDialog();
						}
						else if (register.DialogResult == DialogResult.Cancel)
						{
							return;
						}
					}
					//登录成功
					else
					{
						if (login.DialogResult == DialogResult.OK)
						{
							break;
						}
					}
				}
				葡萄霜霉病防控系统 pSystem = new 葡萄霜霉病防控系统();
				pSystem.ShowDialog();
				if(pSystem.DialogResult == DialogResult.Retry)
				{
					continue;
				}
				else if(pSystem.DialogResult == DialogResult.Cancel)
				{
					break;
				}
			}

			//Application.Run(pSystem);
		}			
    }
}
