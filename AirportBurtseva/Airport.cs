namespace AirportBurtseva;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class Airport
{
    private List<Flight> flights = new List<Flight>();
    private List<Passenger> passengers = new List<Passenger>();

    private Queue<Passenger> checkInQueue = new Queue<Passenger>();
    private Queue<Passenger> securityQueue = new Queue<Passenger>();
    private List<Passenger> waitingForBoarding = new List<Passenger>();

    private Random rand = new Random();
    private int time = 0;

    private const int checkInDesks = 3;
    private const int securityPoints = 2;
    private const int boardingSpeed = 5;

    public void AddFlight(Flight flight) => flights.Add(flight);

    public void RunSimulation()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Час симуляції: {time}");

            //можлива поява нового пасажира
            if (rand.NextDouble() < 0.8) // 80% шанс
            {
                var flight = flights[rand.Next(flights.Count)];
                var passenger = new Passenger($"Пасажир{passengers.Count + 1}", flight.FlightNumber);
                passengers.Add(passenger);
                checkInQueue.Enqueue(passenger);
                Console.WriteLine($"Новий пасажир: {passenger.Name} для рейсу {flight.FlightNumber}");
            }

            //реєстрація
            for (int i = 0; i < checkInDesks && checkInQueue.Count > 0; i++)
            {
                var p = checkInQueue.Dequeue();
                p.HasTicket = true;
                securityQueue.Enqueue(p);
                Console.WriteLine($"{p.Name} зареєструвався на рейс {p.FlightNumber}");
            }

            //контроль безпеки
            for (int i = 0; i < securityPoints && securityQueue.Count > 0; i++)
            {
                var p = securityQueue.Dequeue();
                p.PassedSecurity = true;
                waitingForBoarding.Add(p);
                Console.WriteLine($"{p.Name} пройшов контроль безпеки");
            }

            //оновлення статусів рейсів
            foreach (var flight in flights.ToList())
            {
                if (time == flight.DepartureTime - 2)
                    flight.Status = FlightStatus.Boarding;

                if (time >= flight.DepartureTime)
                    flight.Status = FlightStatus.Departed;

                //посадка
                if (flight.Status == FlightStatus.Boarding)
                {
                    var readyToBoard = waitingForBoarding
                        .Where(p => p.FlightNumber == flight.FlightNumber && p.PassedSecurity && !p.IsOnBoard)
                        .Take(boardingSpeed)
                        .ToList();

                    foreach (var p in readyToBoard)
                    {
                        p.IsOnBoard = true;
                        flight.BoardedCount++;
                        Console.WriteLine($"{p.Name} сів на рейс {flight.FlightNumber}");
                    }
                }

                //виліт
                if (flight.Status == FlightStatus.Departed)
                {
                    var departedPassengers = passengers.Where(p => p.FlightNumber == flight.FlightNumber && p.IsOnBoard).ToList();
                    foreach (var p in departedPassengers)
                        passengers.Remove(p);

                    Console.WriteLine($"Рейс {flight.FlightNumber} вилетів з {flight.BoardedCount}/{flight.Capacity} пасажирами");
                }
            }

            //вивід інформації
            Console.WriteLine("\nРейси:");
            foreach (var flight in flights)
            {
                switch (flight.Status)
                {
                    case FlightStatus.OnTime: Console.ForegroundColor = ConsoleColor.Green; break;
                    case FlightStatus.Boarding: Console.ForegroundColor = ConsoleColor.Yellow; break;
                    case FlightStatus.Departed: Console.ForegroundColor = ConsoleColor.Red; break;
                }

                string statusText = flight.Status switch
                {
                    FlightStatus.OnTime => "Вчасно",
                    FlightStatus.Boarding => "Посадка",
                    FlightStatus.Departed => "Вилетів",
                    FlightStatus.Delayed => "Затримано",
                    _ => flight.Status.ToString()
                };

                Console.WriteLine($"{flight.FlightNumber} до {flight.Destination} - {statusText} (Виліт о {flight.DepartureTime}, Місць {flight.Capacity})");
                Console.ResetColor();
            }

            Console.WriteLine($"\nЧерги: Реєстрація={checkInQueue.Count}, Контроль={securityQueue.Count}, Очікування={waitingForBoarding.Count(p => !p.IsOnBoard)}");

            //крок часу
            time++;
            Thread.Sleep(2000);
        }
    }
}
