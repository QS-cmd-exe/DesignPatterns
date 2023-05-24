using Masuit.Tools;
using Masuit.Tools.Html;
using Masuit.Tools.Systems;
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

namespace MasuitTools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Btn_ForInfo_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(ForInfo) { IsBackground = true };
            th.Start();
        }
        void ForInfo()
        {

            var tel = "15356479454";


           Console.WriteLine("信息脱敏："+tel.Mask());  

            //HiPerfTimer timer = HiPerfTimer.StartNew();
            //for (int i = 0; i < 1000000; i++)
            //{
            //    //todo
            //    i++;

            //}
            //timer.Stop();
            //Console.WriteLine("执行for循环100000次耗时" + timer.Duration + "s");

            //double time = HiPerfTimer.Execute(() =>
            //{
            //    for (int i = 0; i < 1000000; i++)
            //    {
            //        //todo
            //        i++;
            //    }
            //});
            //Console.WriteLine("执行for循环100000次耗时" + time + "s");
        }
    }
}
