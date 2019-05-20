using System;
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
			Register register =  new Register();
			//Application.EnableVisualStyles();
			//Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new Login());
			Login login = new Login();
			//Application.Run(login);
			login.ShowDialog();
			葡萄霜霉病防控系统 pSystem = new 葡萄霜霉病防控系统();
			while(true)
			{
				if (login.DialogResult == DialogResult.OK)
				{
					Application.Run(pSystem);
					login.DialogResult = DialogResult.None;
					//new 葡萄霜霉病防控系统().ShowDialog();
				}
				else if (login.DialogResult == DialogResult.No)
				{
					
					//Application.Run(register);
					register.ShowDialog();
					login.DialogResult = DialogResult.None;
				}
				//if (register != null)
				//{
				//返回登录
				if (register.DialogResult == DialogResult.Retry)
				{
					login.ShowDialog();
					register.DialogResult = DialogResult.None;
					//Application.Run(login);
				}
				//}
				if(login.DialogResult == DialogResult.Cancel || pSystem.DialogResult == DialogResult.Cancel)
				{
					break;
				}
				else{
					if(register != null) 
					{
						if(register.DialogResult == DialogResult.Cancel)
						{
							break;
						}
					}
				}
			}
        }
    }
}
