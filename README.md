## 单例模式

[^说明]:保证一个类只有一个实例，并提供一个全局访问点（当第一次进入的时候需要实例化，其他任何时间进来都不需要进行实例化）

### 一、为什么会有单例模式

1、控制对象数量：某些情况下，系统只需要一个特定的对象。使用单例模式可以确定只有一个对象被创建，从而避免多个对象创建，减少了系统资源的消耗。

2、全局访问点：单例模式可以提供一个全局访问点，使得其他对象可以很方便的访问该单例对象。对于需要共享数据或提供公共服务的情况非常可用。

3、节省资源：某些对象的初始化和创建可能比较耗时，或者占用更多的系统资源。通过使用单例模式，可以避免频繁的创建和销毁对象，提供系统性能和资源利用率。

4、保持一致性：单例模式下只允许创建一个对象，可以确保该对象的状态和行为始终保持一致。对于跨多个模块使用同一个对象时，可以确保对象状态的一致性。

### 二、单例模式实现思路

1、单线程实现（懒汉模式）

```c#
/// <summary>
    /// 单例模式的实现
    /// </summary>
    public class Singleton
    {
        // 定义一个静态变量来保存类的实例
        private static Singleton uniqueInstance;

        // 定义私有构造函数，使外界不能创建该类实例
        private Singleton()
        {
        }

        /// <summary>
        /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
        /// </summary>
        /// <returns></returns>
        public static Singleton GetInstance()
        {
            // 如果类的实例不存在则创建，否则直接返回
            if (uniqueInstance == null)
            {
                uniqueInstance = new Singleton();
            }
            return uniqueInstance;
        }
    }
```

这种模式下只限制于单线程，当出现多线程时，此时 if (uniqueInstance == null)多次都是为空，就会创建多个对象，这个时候就违背了单例模式的设计思想。那么在多线程下要实现单例模式下，就需要使用线程同步的方式。

2、使用锁的方式来实现线程同步

​		定义locker对象，当线程执行时会对locker对象进行加锁处理，当第二个线程执行时就会进行判断，如果处于加锁状态就需要进行等待。这种方式虽然解决了线程同步的问题，但是每次线程执行时都要对辅助对象locker进行加锁然后进行判断，这样会增加系统额外的开销，其实只需要在第一次进行加锁，后续只需要判断uniqueInstance == null就可以了

```c#
/// <summary>
    /// 单例模式的实现
    /// </summary>
    public class Singleton
    {
        // 定义一个静态变量来保存类的实例
        private static Singleton uniqueInstance;

        // 定义一个标识确保线程同步
        private static readonly object locker = new object();

        // 定义私有构造函数，使外界不能创建该类实例
        private Singleton()
        {
        }

        /// <summary>
        /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
        /// </summary>
        /// <returns></returns>
        public static Singleton GetInstance()
        {
            // 当第一个线程运行到这里时，此时会对locker对象 "加锁"，
            // 当第二个线程运行该方法时，首先检测到locker对象为"加锁"状态，该线程就会挂起等待第一个线程解锁
            // lock语句运行完之后（即线程运行完之后）会对该对象"解锁"
            lock (locker)
            {
                // 如果类的实例不存在则创建，否则直接返回
                if (uniqueInstance == null)
                {
                    uniqueInstance = new Singleton();
                }
            }

            return uniqueInstance;
        }
    }
```

3、双重锁

​		实现双重锁可以有效避免额外的资源开销

```c#
/// <summary>
    /// 单例模式的实现
    /// </summary>
    public class Singleton
    {
        // 定义一个静态变量来保存类的实例
        private static Singleton uniqueInstance;

        // 定义一个标识确保线程同步
        private static readonly object locker = new object();

        // 定义私有构造函数，使外界不能创建该类实例
        private Singleton()
        {
        }

        /// <summary>
        /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
        /// </summary>
        /// <returns></returns>
        public static Singleton GetInstance()
        {
            // 当第一个线程运行到这里时，此时会对locker对象 "加锁"，
            // 当第二个线程运行该方法时，首先检测到locker对象为"加锁"状态，该线程就会挂起等待第一个线程解锁
            // lock语句运行完之后（即线程运行完之后）会对该对象"解锁"
            // 双重锁定只需要一句判断就可以了
            if (uniqueInstance == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (uniqueInstance == null)
                    {
                        uniqueInstance = new Singleton();
                    }
                }
            }
            return uniqueInstance;
        }
    }
```

