using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DesignPatternMain.Service.FactoryPattern
{
    /// <summary>
    /// 简单工厂模式
    /// </summary>
    public class BaseFactory
    {
        //烧菜方法
        public static Food Cook(string type)
        {
            Food food = null;
            switch (type)
            {
                case "西红柿炒鸡蛋":
                    {
                        food = new TomatoScrambleEggs();
                    }
                    break;
                case "土豆丝炒肉":
                    {
                        food = new ShreddedPorkWithPotatoes();
                    }
                    break;
                default:
                    break;
            }

            return food;

        }


    }

    /// <summary>
    /// 菜的抽象类 用于打印输出
    /// </summary>
    public abstract class Food
    {
        public abstract void Print();
    };


    //定义两道菜

    /// <summary>
    /// 西红柿炒鸡蛋
    /// </summary>
    public class TomatoScrambleEggs : Food
    {
        public override void Print()
        {
            Console.WriteLine("这是第一道西红柿炒鸡蛋");
        }
    }

    public class ShreddedPorkWithPotatoes : Food
    {
        public override void Print()
        {
            Console.WriteLine("这是第一道土豆丝炒肉丝");
        }
    }
}