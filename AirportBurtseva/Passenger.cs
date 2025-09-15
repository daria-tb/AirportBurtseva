namespace AirportBurtseva;
using System;
public class Passenger
{
    public string Name { get; set; }
    public string FlightNumber { get; set; }
    public bool HasTicket { get; set; } = false;
    public bool PassedSecurity { get; set; } = false;
    public bool IsOnBoard { get; set; } = false;

    public Passenger(string name, string flightNumber)
    {
        Name = name;
        FlightNumber = flightNumber;
    }

    public override string ToString()
    {
        return $"{Name} ({FlightNumber}) - " +
               $"Ticket: {HasTicket}, Security: {PassedSecurity}, OnBoard: {IsOnBoard}";
    }
}