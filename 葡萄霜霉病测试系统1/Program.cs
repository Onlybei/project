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
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Login());
            Login login = new Login();
            if (login.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new 葡萄霜霉病防控系统());
            }
        }
    }
}
