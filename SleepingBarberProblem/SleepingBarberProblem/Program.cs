using System;
using System.Threading;
class Program
{
    static Random Rand = new Random();
    const int MaxCustomers = 15;
    const int NumChairs = 5;
    static Semaphore waitingRoom = new Semaphore(NumChairs, NumChairs);
    static Semaphore barberChair = new Semaphore(1, 1);
    static Semaphore barberPillow = new Semaphore(0, 1);
    static Semaphore seatBelt = new Semaphore(0, 1);
    static bool AllDone = false;
    static void Barber()
    {
        while (!AllDone)
        {
            Console.WriteLine("Парикмахер спит");
            barberPillow.WaitOne();
            if (!AllDone)
            {
                Console.WriteLine("Парикмахер работает");
                Thread.Sleep(Rand.Next(1, 3) * 1000);
                Console.WriteLine("Парикмахер закончил работу");
                seatBelt.Release();
            }
            else
            {
                Console.WriteLine("Парикмахер идет домой");
            }
        }
        return;
    }
    static void Customer(Object number)
    {
        int Number = (int)number;
        Thread.Sleep(Rand.Next(1, 5) * 1000);
        Console.WriteLine("Клиент {0} прибыл.", Number);
        waitingRoom.WaitOne();
        Console.WriteLine("Клиент {0} ожидает в приемной", Number);
        barberChair.WaitOne();
        waitingRoom.Release();
        Console.WriteLine("Клиент {0} хочет разбудить Парикмахера!", Number);
        barberPillow.Release();
        seatBelt.WaitOne();
        barberChair.Release();
        Console.WriteLine("Клиент {0} покидает салон.", Number);
    }
    static void Main()
    {
        Thread BarberThread = new Thread(Barber);
        BarberThread.Start();
        Thread[] Customers = new Thread[MaxCustomers];
        for (int i = 0; i < MaxCustomers; i++)
        {
            Customers[i] = new Thread(new ParameterizedThreadStart(Customer));
            Customers[i].Start(i);
        }
        for (int i = 0; i < MaxCustomers; i++)
        {
            Customers[i].Join();
        }
        AllDone = true;
        barberPillow.Release();
        // Wait for the Barber's thread to finish before exiting.
        BarberThread.Join();        
    }
}