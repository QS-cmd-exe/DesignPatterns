using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternMain.Service.SingletonPattern
{
    //懒汉模式
    public class Singleton
    { 
        
        private static Singleton instance;//定义私有静态变量来记录实例


        private static readonly object LOCK = new object();//定义一个锁，来实现线程同步

        /// <summary>
        /// 私有构造方法，让这个类只允许实例化一次（如果没有的话，编译器会自动生成一个公共的无参构造函数）
        /// </summary>
        private Singleton()
        {
            Console.WriteLine("实例化");
        }
        /// <summary>
        /// 公共方法获取实例  全局访问点
        /// 这种方式只能存在单线程，当多线程时就会失败
        /// </summary>
        /// <returns></returns>
        public static Singleton GetSingleton1()
        {
            if (instance == null)
            {
                instance = new Singleton();
            }
            return instance;
        }
        /// <summary>
        /// 采用锁的方式， 但是这种方式会每次对lock辅助对象进行加锁然后判断，这样会增加额外的开销，其实本质上第一次执行过后，我后面需要判断instance是否为空
        /// </summary>
        /// <returns></returns>
        public static Singleton GetSingleton2()
        {
            //第一次进来会对lock对象进行枷锁
            //第二次进来时就会判断，如果已经锁住，那么会等待第一个执行完成后然后再执行
            lock (LOCK)
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
            }

            return instance;
        }

        /// <summary>
        /// 采用双重锁的方式来减少额外得到开销
        /// </summary>
        /// <returns></returns>
        public static Singleton GetSingleton3()
        {
     
            if (instance == null)
            {
                lock (LOCK)
                {
                    if (instance == null)
                    {
                        instance = new Singleton();
                    }
                }
            }

            return instance;
        }


    }
}
