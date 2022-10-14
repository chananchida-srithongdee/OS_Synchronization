using System;
using System.Diagnostics;
using System.Threading;
// Processor	Intel(R) Core(TM) i7-10750H CPU @ 2.60GHz, 2592 Mhz, 6 Core(s), 12 Logical Processor(s)
namespace OS_Sync_01
{
    class Program
    {
        private static string x = ""; //x is shared value
        private static int exitflag = 0;
        private static bool inputflag = false;
        private static object _Lock = new object();
        static void ThReadX()
        {
            while (exitflag == 0)
            {
                lock (_Lock)
                {
                    if (inputflag == true)
                    {
                        inputflag = false;
                        Console.WriteLine("x = {0}", x);
                    }
                }
            }
        }
        static void ThWriteX()
        {
            string xx;
            while (exitflag == 0)
            {

                lock (_Lock)
                {
                    if (inputflag == false)
                    {
                        Console.Write("Input: ");
                        xx = Console.ReadLine();
                        if (xx == "exit")
                        {
                            exitflag = 1;
                            Console.WriteLine("Thread {0} is exiting...", Thread.CurrentThread.ManagedThreadId);
                        }
                        else
                        {
                            lock (_Lock)
                            {
                                x = xx;
                                inputflag = true;
                            }
                        }
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            Thread A = new Thread(new ThreadStart(ThReadX));
            Thread B = new Thread(new ThreadStart(ThWriteX));
            A.Start();
            B.Start();
        }
    }
}