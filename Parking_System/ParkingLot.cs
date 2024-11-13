using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
namespace Parking_System
{
    // Parkeringssystemsklass
    class ParkingLot
    {
        private readonly int _totalSpots;
        private readonly List<(Vehicle, DateTime)> _parkedVehicles = new List<(Vehicle, DateTime)>();
        private readonly List<(Vehicle, DateTime, bool)> _luxuryParkedVehicles = new List<(Vehicle, DateTime, bool)>(); // Lyxparkering
        private readonly List<string> _fines = new List<string>(); // Böter

        public ParkingLot(int totalSpots = 25)
        {
            _totalSpots = totalSpots;
        }

        public bool AddVehicle(Vehicle vehicle, int parkingDurationSeconds, bool isLuxurySpot = false)
        {
            if (isLuxurySpot && !(vehicle is Car))
            {
                Console.WriteLine("Endast bilar kan parkera på lyxparkeringen.");
                return false;
            }

            if (GetFreeSpots() >= vehicle.SpotsNeeded)
            {
                if (isLuxurySpot)
                {
                    _luxuryParkedVehicles.Add((vehicle, DateTime.Now.AddSeconds(parkingDurationSeconds), false)); // Sätt "false" tills vakten ser böterna
                }
                else
                {
                    _parkedVehicles.Add((vehicle, DateTime.Now.AddSeconds(parkingDurationSeconds)));
                }

                double fee = vehicle.CalculateParkingFee(parkingDurationSeconds);
                if (isLuxurySpot)
                {
                    fee *= 1.5; // 50% mer för lyxparkering
                    Console.WriteLine($"Fordon {vehicle.RegNumber} parkerar på lyxparkering. Avgift: {fee:F2} kr.");
                }
                else
                {
                    Console.WriteLine($"Fordon {vehicle.RegNumber} parkerar. Avgift: {fee:F2} kr.");
                }
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

        // Funktion för att sätta böter
        public void AssignFine(string regNumber)
        {
            var vehicleData = _parkedVehicles.FirstOrDefault(v => v.Item1.RegNumber == regNumber);
            if (vehicleData == default)
            {
                Console.WriteLine("Fordon ej funnet.");
                return;
            }

            var parkedSeconds = (int)(DateTime.Now - vehicleData.Item2).TotalSeconds;
            double fine = 500; // Bötesbelopp
            Console.WriteLine($"Fordon {vehicleData.Item1.RegNumber} har överskridit parkeringstiden. Böter: {fine} kr.");
            _fines.Add($"{vehicleData.Item1.RegNumber} - Böter: {fine} kr");

            _parkedVehicles.Remove(vehicleData);
        }

        public void DisplayParkingLot()
        {
            Console.WriteLine("Parkeringsstatus:");
            if (_parkedVehicles.Count == 0 && _luxuryParkedVehicles.Count == 0)
            {
                Console.WriteLine("Ingen parkerad bil.");
            }
            else
            {
                foreach (var (vehicle, parkingTime) in _parkedVehicles)
                {
                    var remainingTime = Math.Max(0, (int)(parkingTime - DateTime.Now).TotalSeconds);
                    Console.WriteLine($"Fordon {vehicle.RegNumber} ({vehicle.Color}), {remainingTime} sek kvar.");
                }
                foreach (var (vehicle, parkingTime, fineIssued) in _luxuryParkedVehicles)
                {
                    var remainingTime = Math.Max(0, (int)(parkingTime - DateTime.Now).TotalSeconds);
                    Console.WriteLine($"Lyxparkering - Fordon {vehicle.RegNumber} ({vehicle.Color}), {remainingTime} sek kvar.");
                }
            }
        }

        private double GetFreeSpots()
        {
            double usedSpots = _parkedVehicles.Sum(v => v.Item1.SpotsNeeded);
            return _totalSpots - usedSpots;
        }

        public void ShowParkingInfo()
        {
            Console.WriteLine($"Totalt antal platser: {_totalSpots}");
            Console.WriteLine($"Lediga platser: {GetFreeSpots()}");
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