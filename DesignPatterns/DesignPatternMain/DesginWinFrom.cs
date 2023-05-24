using DesignPatternMain.Service.FactoryPattern;
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
        /// <summary>
        /// 工厂模式  简单工厂模式/工厂方法模式/抽象工厂模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_FactoryPattern_Click(object sender, EventArgs e)
        {
            //简单共产模式
            /*
                优点：是需要提前在工厂里面加工好，如果客户想要其他的这个时候就要修改工厂的方法
                    1、简单工厂模式解决了客户端直接依赖于具体对象的问题，客户端可以消费直接创建对象的责任，从而是需要消费产品。因此简单工厂模式实现了对责任的分割。
                    2、代码的复用，有了简单工厂过后，去餐馆吃饭的人就不需要那么麻烦了，只要负责消费就可以了。统一从这一个工厂里面出去。缺点是这个工厂要是瘫痪了，那么整个客户也就无法进行消费了。
                缺点：
                    1、工厂类中集中了所有实现方法，一旦这个工厂不能使用之后，那么整个系统就会崩溃
                    2、系统扩展困难，一旦添加新的产品，就不得不修改工厂的视线逻辑，这样会导致工厂的逻辑复杂。
             */
            Thread th = new Thread(() =>
            {
                Food food1 = BaseFactory.Cook("西红柿炒鸡蛋");
                food1.Print();
                Food food2 = BaseFactory.Cook("土豆丝炒肉");
                food2.Print();
     
            })
            { IsBackground = true };
            th.Start();
        }
    }
}
