namespace AirportBurtseva;

class Program
{
    static void Main()
    {
        Airport airport = new Airport();
        airport.AddFlight(new Flight("PS101", "Kyiv", 5, 100));
        airport.AddFlight(new Flight("PS202", "Lviv", 8, 80));
        airport.AddFlight(new Flight("PS303", "Odesa", 12, 120));

        airport.RunSimulation();
    }
}