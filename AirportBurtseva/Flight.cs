namespace AirportBurtseva;
using System;
public enum FlightStatus { OnTime, Delayed, Boarding, Departed }

public class Flight
{
    public string FlightNumber { get; set; }
    public string Destination { get; set; }
    public int DepartureTime { get; set; } //у "тіках"
    public FlightStatus Status { get; set; } = FlightStatus.OnTime;
    public int Capacity { get; set; }
    public int BoardedCount { get; set; } = 0;

    public Flight(string flightNumber, string destination, int departureTime, int capacity)
    {
        FlightNumber = flightNumber;
        Destination = destination;
        DepartureTime = departureTime;
        Capacity = capacity;
    }

    public override string ToString()
    {
        return $"{FlightNumber} to {Destination} - {Status} (Departs at {DepartureTime}, Seats {Capacity})";
    }
}