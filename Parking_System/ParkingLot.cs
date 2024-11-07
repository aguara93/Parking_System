using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSystem
{
    // Parkeringssystemsklass
    class ParkingLot
    {
        private readonly int _totalSpots;
        private readonly List<(Vehicle, DateTime)> _parkedVehicles = new List<(Vehicle, DateTime)>();

        public ParkingLot(int totalSpots = 25)
        {
            _totalSpots = totalSpots;
        }

        public bool AddVehicle(Vehicle vehicle, int parkingDurationSeconds)
        {
            if (GetFreeSpots() >= vehicle.SpotsNeeded)
            {
                _parkedVehicles.Add((vehicle, DateTime.Now.AddSeconds(parkingDurationSeconds)));
                Console.WriteLine($"Fordon {vehicle.RegNumber} tilldelat plats i parkeringen.");
                return true;
            }
            Console.WriteLine("Ingen ledig plats för detta fordon.");
            return false;
        }

        public bool ReleaseVehicle(string regNumber)
        {
            var vehicleData = _parkedVehicles.FirstOrDefault(v => v.Item1.RegNumber == regNumber);
            if (vehicleData == default)
            {
                Console.WriteLine("Fordon ej funnet.");
                return false;
            }

            var parkedSeconds = (int)(DateTime.Now - vehicleData.Item2).TotalSeconds;
            double fee = vehicleData.Item1.CalculateParkingFee(parkedSeconds);
            Console.WriteLine($"{vehicleData.Item1.RegNumber} lämnar parkeringen. Avgift: {fee:F2} kr.");

            _parkedVehicles.Remove(vehicleData);
            return true;
        }
        public void DisplayParkingLot()
        {
            Console.WriteLine("Parkeringsstatus:");
            foreach (var (vehicle, parkingTime) in _parkedVehicles)
            {
                var remainingTime = Math.Max(0, (int)(parkingTime - DateTime.Now).TotalSeconds);
                Console.WriteLine($"Fordon {vehicle.RegNumber} ({vehicle.Color}), {remainingTime} sek kvar.");
            }
        }

        private double GetFreeSpots()
        {
            double usedSpots = _parkedVehicles.Sum(v => v.Item1.SpotsNeeded);
            return _totalSpots - usedSpots;
        }
    }

    // Hjälpfunktion för att skapa slumpmässiga registreringsnummer
    static class Helper
    {
        private static Random random = new Random();
        public static string GenerateRegNumber()
        {
            string letters = new string(Enumerable.Range(0, 3).Select(_ => (char)random.Next('A', 'Z')).ToArray());
            string numbers = new string(Enumerable.Range(0, 3).Select(_ => (char)random.Next('0', '9')).ToArray());
            return letters + numbers;
        }
    }

}
