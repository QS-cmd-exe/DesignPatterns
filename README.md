 ## 单例模式

[^说明]:保证一个类只有一个实例，并提供一个全局访问点（当第一次进入的时候需要实例化，其他任何时间进来都不需要进行实例化）,文章借鉴：https://www.cnblogs.com/zhili/

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

# 工厂模式

## 简单工厂模式

### 案例介绍

说到简单工厂，自然的第一个疑问当然就是什么是简单工厂模式了？ 在现实生活中工厂是负责生产产品的,同样在设计模式中,简单工厂模式我们也可以理解为负责**生产对象的一个类,** 我们平常编程中，当使用"new"关键字创建一个对象时，此时该类就依赖与这个对象，也就是他们之间的耦合度高，当需求变化时，我们就不得不去修改此类的源码，此时我们可以运用面向对象（OO）的很重要的原则去解决这一的问题，该原则就是——**封装改变，既然要封装改变，自然也就要找到改变的代码，然后把改变的代码用类来封装**，这样的一种思路也就是我们简单工厂模式的实现方式了。下面通过一个现实生活中的例子来引出简单工厂模式。

　　在外面打工的人，免不了要经常在外面吃饭，当然我们也可以自己在家做饭吃，但是自己做饭吃麻烦，因为又要自己买菜，然而，出去吃饭就完全没有这些麻烦的，我们只需要到餐馆点菜就可以了，买菜的事情就交给餐馆做就可以了，这里餐馆就充当简单工厂的角色，下面让我们看看现实生活中的例子用代码是怎样来表现的。

```c#
/// <summary>
    /// 顾客充当客户端，负责调用简单工厂来生产对象
    /// 即客户点菜，厨师（相当于简单工厂）负责烧菜(生产的对象)
    /// </summary>
    class Customer
    {
        static void Main(string[] args)
        {
            // 客户想点一个西红柿炒蛋        
            Food food1 = FoodSimpleFactory.CreateFood("西红柿炒蛋");
            food1.Print();

            // 客户想点一个土豆肉丝
            Food food2 = FoodSimpleFactory.CreateFood("土豆肉丝");
            food2.Print();

            Console.Read();
        }
    }

    /// <summary>
    /// 菜抽象类
    /// </summary>
    public abstract class Food
    {
        // 输出点了什么菜
        public abstract void Print();
    }

    /// <summary>
    /// 西红柿炒鸡蛋这道菜
    /// </summary>
    public class TomatoScrambledEggs : Food
    {
        public override void Print()
        {
            Console.WriteLine("一份西红柿炒蛋！");
        }
    }

    /// <summary>
    /// 土豆肉丝这道菜
    /// </summary>
    public class ShreddedPorkWithPotatoes : Food
    {
        public override void Print()
        {
            Console.WriteLine("一份土豆肉丝");
        }
    }

    /// <summary>
    /// 简单工厂类, 负责 炒菜
    /// </summary>
    public class FoodSimpleFactory
    {
        public static Food CreateFood(string type)
        {
            Food food = null;
            if (type.Equals("土豆肉丝"))
            {
                food= new ShreddedPorkWithPotatoes();
            }
            else if (type.Equals("西红柿炒蛋"))
            {
                food= new TomatoScrambledEggs();
            }

            return food;
        }
    }
```

###  优缺点

1、优点：

- 简单工厂模式解决了客户端直接依赖于具体对象的问题，客户端可以消除直接创建对象的责任，而仅仅是消费产品。简单工厂模式实现了对责任的分割。
- 简单工厂模式也起到了代码复用的作用，因为之前的实现（自己做饭的情况）中，换了一个人同样要去在自己的类中实现做菜的方法，然后有了简单工厂之后，去餐馆吃饭的所有人都不用那么麻烦了，只需要负责消费就可以了。此时简单工厂的烧菜方法就让所有客户共用了。（同时这点也是简单工厂方法的缺点——因为工厂类集中了所有产品创建逻辑，一旦不能正常工作，整个系统都会受到影响，也没什么不好理解的，就如**事物都有两面性一样道理**）

​	2、缺点：

- 工厂类集中了所有产品创建逻辑，一旦不能正常工作，整个系统都会受到影响（通俗地意思就是：一旦餐馆没饭或者关门了，很多不愿意做饭的人就没饭吃了）
- 系统扩展困难，一旦添加新产品就不得不修改工厂逻辑，这样就会造成工厂逻辑过于复杂。

![img](https://images0.cnblogs.com/blog/383187/201309/05170314-a7f4c3e70df3420fa5408c7ccca265d9.png)

### .net中关于简单工厂模式的视线

```c#
public static Encoding GetEncoding(int codepage)
{
    Encoding unicode = null;
    if (encodings != null)
    {
        unicode = (Encoding) encodings[codepage];
    }
    if (unicode == null)
    {
        object obj2;
        bool lockTaken = false;
        try
        {
            Monitor.Enter(obj2 = InternalSyncObject, ref lockTaken);
            if (encodings == null)
            {
                encodings = new Hashtable();
            }
            unicode = (Encoding) encodings[codepage];
            if (unicode != null)
            {
                return unicode;
            }
            switch (codepage)
            {
                case 0:
                    unicode = Default;
                    break;

                case 1:
                case 2:
                case 3:
                case 0x2a:
                    throw new ArgumentException(Environment.GetResourceString("Argument_CodepageNotSupported", new object[] { codepage }), "codepage");

                case 0x4b0:
                    unicode = Unicode;
                    break;

                case 0x4b1:
                    unicode = BigEndianUnicode;
                    break;

                case 0x6faf:
                    unicode = Latin1;
                    break;

                case 0xfde9:
                    unicode = UTF8;
                    break;

                case 0x4e4:
                    unicode = new SBCSCodePageEncoding(codepage);
                    break;

                case 0x4e9f:
                    unicode = ASCII;
                    break;

                default:
                    unicode = GetEncodingCodePage(codepage);
                    if (unicode == null)
                    {
                        unicode = GetEncodingRare(codepage);
                    }
                    break;
            }
            encodings.Add(codepage, unicode);
            return unicode;

        }
}
```

### 代码讲解

让我们逐步解析代码：

1. 方法开始时声明了一个名为 `unicode` 的 `Encoding` 对象的空引用。
2. 首先判断 `encodings` 是否为 `null`。如果不为 `null`，则尝试从 `encodings` 中获取对应 `codepage` 的 `Encoding` 对象，并将其赋值给 `unicode`。
3. 如果 `unicode` 为 `null`，则进入下一步。
4. 在 `try` 块中，首先通过 `Monitor.Enter` 方法获取一个同步对象的锁。这个同步对象是 `InternalSyncObject`。
5. 接着判断 `encodings` 是否为 `null`，如果是，则实例化一个新的 `Hashtable` 对象，并将其赋值给 `encodings`。
6. 再次尝试从 `encodings` 中获取对应 `codepage` 的 `Encoding` 对象，并将其赋值给 `unicode`。
7. 如果 `unicode` 不为 `null`，表示已经找到对应的 `Encoding`，直接返回 `unicode`。
8. 如果 `unicode` 仍然为 `null`，则通过 `switch` 语句根据 `codepage` 的不同进行处理：
   - 如果 `codepage` 为 0，将 `Default` 赋值给 `unicode`。
   - 如果 `codepage` 为 1、2、3 或 0x2a，抛出 `ArgumentException`，并显示一个错误消息，表明不支持该代码页。
   - 如果 `codepage` 为 0x4b0，将 `Unicode` 赋值给 `unicode`。
   - 如果 `codepage` 为 0x4b1，将 `BigEndianUnicode` 赋值给 `unicode`。
   - 如果 `codepage` 为 0x6faf，将 `Latin1` 赋值给 `unicode`。
   - 如果 `codepage` 为 0xfde9，将 `UTF8` 赋值给 `unicode`。
   - 如果 `codepage` 为 0x4e4，创建一个 `SBCSCodePageEncoding` 对象，传入 `codepage` 作为参数，并将其赋值给 `unicode`。
   - 如果 `codepage` 为 0x4e9f，将 `ASCII` 赋值给 `unicode`。
   - 对于其他情况，首先调用 `GetEncodingCodePage` 方法尝试获取对应的 `Encoding` 对象，如果返回值为 `null`，再调用 `GetEncodingRare` 方法尝试获取。最终将结果赋值给 `unicode`。
9. 将 `unicode` 添加到 `encodings` 中，使用 `codepage` 作为键。
10. 返回 `unicode`。

这段代码的功能是根据给定的代码页返回对应的编码对象。它首先从缓存中查找，如果找到则直接返回，否则根据不同的代码页进行处理，并将结果添加到缓存中以备下次使用。

![img](https://images0.cnblogs.com/blog/383187/201309/05173548-27a5da55b8434859adb53f386f9c3932.png)

## 工厂方法模式

工厂方法模式是为了解决简单工厂模式中难以扩展的问题，因为在简单工厂模式下，一旦添加新的产品或者之前的产品有变动时，就不得不修改工厂的方法，从而导致工厂类中的逻辑过于复杂。

