using DesignPatternMain.Service.SingletonPattern;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignPatternMain
{
    public partial class DesginWinFrom : Form
    {
        public DesginWinFrom()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 单例模式：保证一个类只有一个实例，并提供一个访问它的全局访问点。（在第一个使用者创建了这个类的实例之后，其后需要使用这个类就只能使用之前创建的实例，无法再创建一个新的实例。）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Singletonpattern_Click(object sender, EventArgs e)  
        {
            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(() =>
                {
                    Singleton.GetSingleton3();//采用双重锁 最佳方案

                })
                { IsBackground = true };
                thread.Start();

            }
            Console.Write("结束!");
        }

    }
}
