using System;
using System.Collections.Generic;
using System.Threading;

namespace Semaphores
{
    internal class Program
    {
        static void Main(string[] args)
            {
                List<Thread> threads = new List<Thread>();

                for (int i = 1; i <= 7; i++)
                {
                    Animal animal = new Animal(i);
                    threads.Add(animal.GetThread());
                }

                foreach (var thread in threads)
                {
                    thread.Join();
                }

                Console.WriteLine("Все животные наелись!");
            }
        }

        class Animal
        {
            static Semaphore sem = new Semaphore(4, 4);
            private int satiety = 0;
            private const int MaxSatiety = 3;
            private Thread myThread;
            private int id;

            public Animal(int id)
            {
                this.id = id;
                myThread = new Thread(Eat);
                myThread.Name = $"Животное {id}";
                myThread.Start();
            }

            public Thread GetThread() => myThread;

            public void Eat()
            {
                while (satiety < MaxSatiety)
                {
                    sem.WaitOne();

                    Console.WriteLine($"{Thread.CurrentThread.Name} заходит на поляну\n");
                    Console.WriteLine($"{Thread.CurrentThread.Name} ест");

                    satiety++;
                    Thread.Sleep(1000);

                    Console.WriteLine($"{Thread.CurrentThread.Name} уходит с поляны. Сытость: {satiety}\n");

                    sem.Release(); 

                    Thread.Sleep(1000);
                }
            }
        }
}
