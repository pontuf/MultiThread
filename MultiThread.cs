using System;
using System.Threading;

class Counter
{
    public static int Count = 0;
    public static Mutex Mtx = new Mutex();
}

class IncThread
{
    int num;
    public Thread thr1;

    public IncThread(string name, int n)
    {
        thr1 = new Thread(this.Run);
        num = n;
        thr1.Name = name;
        thr1.Start();
    }

    void Run()
    {

        Console.WriteLine(thr1.Name + " ожидает мьютекс.");
        Counter.Mtx.WaitOne();
        Console.WriteLine(thr1.Name + " захватывает мьютекс.");

        do
        {
            Thread.Sleep(500);
            Counter.Count++;
            Console.WriteLine("В потоке " + thr1.Name +
                              ", MultiThread.Count = " + Counter.Count);
            num--;
        } while (num > 0);

        Console.WriteLine(thr1.Name + " освобождает мьютекс.");
        Counter.Mtx.ReleaseMutex();
    }
}

class DecThread
{
    int num;
    public Thread thr2;

    public DecThread(string name, int n)
    {
        thr2 = new Thread(new ThreadStart(this.Run));
        num = n;
        thr2.Name = name;
        thr2.Start();
    }

    void Run()
    {

        Console.WriteLine(thr2.Name + " ожидает мьютекс.");
        Counter.Mtx.WaitOne();
        Console.WriteLine(thr2.Name + " захватывает мьютекс.");

        do
        {
            Thread.Sleep(500);
            Counter.Count--;
            Console.WriteLine("В потоке " + thr2.Name +
                              ", MultiThread.Count = " + Counter.Count);
            num--;
        } while (num > 0);

        Console.WriteLine(thr2.Name + " освобождает мьютекс.");
        Counter.Mtx.ReleaseMutex();
    }
}

class MultiThread
{
    static void Main()
    {
        IncThread mt1 = new IncThread("Инкрементирующий", 5);
        Thread.Sleep(1);
        DecThread mt2 = new DecThread("Декрементирующий", 5);
        mt1.thr1.Join();
        mt2.thr2.Join();
    }
}