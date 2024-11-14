using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Parking_System;
// Parkeringssystemsklass
class ParkingLot
{
    private readonly int _totalSpots;
    private readonly List<(Vehicle, DateTime, int)> _parkedVehicles = new List<(Vehicle, DateTime, int)>(); // fordon, tid, plats index

    // För att hålla reda på vilka platser som är upptagna
    private readonly List<bool> _parkingSpaces;
    private readonly List<int> _premiumSpots; // lyxplatser
    private static bool hasSeenFine = false; // om vakten markerade en fordon som ska ha boten

    public ParkingLot(int totalSpots = 25)
    {
        _totalSpots = totalSpots;
        _parkingSpaces = new List<bool>(new bool[totalSpots]); // Alla platser tomma
        _premiumSpots = new List<int> { 0, 1, 2 }; // forsta tre platser är lyxiga
    }

    // funktion att hitta en ledig plats
    private int GetFreeSpot(double spotsNeeded)
    {
        int consecutiveSpacesNeeded = (int)Math.Ceiling(spotsNeeded);  // För att säkerställa att vi får plats för hela fordonet
        for (int i = 0; i <= _parkingSpaces.Count - consecutiveSpacesNeeded; i++)
        {
            bool isFree = true;
            for (int j = 0; j < consecutiveSpacesNeeded; j++)
            {
                if (_parkingSpaces[i + j])
                {
                    isFree = false;
                    break;
                }
            }

            if (isFree) return i;  // Returnera första lediga platsen
        }

        return -1;  // Ingen ledig plats
    }

    // Lägg till ett fordon på parkeringen
    public bool AddVehicle(Vehicle vehicle, int parkingDurationSeconds)
    {
        int freeSpot = GetFreeSpot(vehicle.SpotsNeeded);
        if (freeSpot == -1)
        {
            Console.WriteLine("Ingen ledig plats för detta fordon.");
            return false;
        }

        // Fråga om bilen vill ha en lyxplats om de är lediga
        if (vehicle is Car && _premiumSpots.Contains(freeSpot))
        {
            Console.WriteLine("Vill du parkera på en bekvämare plats nära utgången? (Ja/Nej)");
            string response = Console.ReadLine().ToLower();
            if (response == "ja")
            {
                freeSpot = _premiumSpots.First();
            }
        }

        // Markera platserna som upptagna
        for (int i = freeSpot; i < freeSpot + (int)Math.Ceiling(vehicle.SpotsNeeded); i++)
        {
            _parkingSpaces[i] = true;
        }

        // Lägg till fordonet på parkeringsplatsen
        _parkedVehicles.Add((vehicle, DateTime.Now.AddSeconds(parkingDurationSeconds), freeSpot));
        Console.WriteLine($"Fordon {vehicle.RegNumber} tilldelas plats {freeSpot + 1}. Parkeringstid: {parkingDurationSeconds} sekunder.");
        return true;
    }

    // Släpp ut fordonet från parkering, beräkna avgift
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

        // Om fordonet har parkerat för länge och vakten har sett detta, ge böter
        if (parkedSeconds > 0 && hasSeenFine)
        {
            Console.WriteLine("Parkeringsböter på 500 kr appliceras.");
            fee += 500; // Lägger till böterna
        }

        Console.WriteLine($"{vehicleData.Item1.RegNumber} lämnar parkeringen. Avgift: {fee:F2} kr.");

        // Lämnar parkeringsplatsen
        for (int i = vehicleData.Item3; i < vehicleData.Item3 + (int)Math.Ceiling(vehicleData.Item1.SpotsNeeded); i++)
        {
            _parkingSpaces[i] = false;
        }

        // Tar bort fordonet från listan över parkerade fordon
        _parkedVehicles.Remove(vehicleData);
        return true;
    }

    // Visar parkeringsstatus för alla parkerade fordon
    public void DisplayParkingLot()
    {
        Console.WriteLine("Parkeringsstatus:");
        if (_parkedVehicles.Count == 0)
        {
            Console.WriteLine("Ingen parkerad bil.");
        }
        else
        {
            foreach (var (vehicle, parkingTime, spotIndex) in _parkedVehicles)
            {
                var remainingTime = Math.Max(0, (int)(parkingTime - DateTime.Now).TotalSeconds);
                Console.WriteLine($"Plats {spotIndex + 1} - {vehicle.GetType().Name} {vehicle.RegNumber} {vehicle.Color} ({remainingTime} sek kvar)");
            }
        }
    }

    public void ShowParkingInfo()
    {
        Console.WriteLine($"Totalt antal platser: {_totalSpots}");
        Console.WriteLine($"Lediga platser: {_parkingSpaces.Count(p => !p)}");
    }

    // Sätt om vakten har sett böterna
    public static void SetHasSeenFine(bool seen)
    {
        hasSeenFine = seen;
    }

    // Kollar om ett fordon har böter
    public (Vehicle RegNumber, DateTime, int)? CheckIfHasFine(string regNumber)
    {
        var vehicleData = _parkedVehicles.FirstOrDefault(v => v.Item1.RegNumber == regNumber);
        if (vehicleData != default)
        {
            var parkedSeconds = (int)(DateTime.Now - vehicleData.Item2).TotalSeconds;
            if (parkedSeconds > 0 && hasSeenFine)
            {
                return vehicleData;
            }
        }
        return null;
    }
}
