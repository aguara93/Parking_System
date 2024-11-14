using Parking_System;
using System;

namespace Parking_System
{
    public class Program
    {
        static void Main(string[] args)
        {
            ParkingLot parkingLot = new ParkingLot();

            while (true)
            {
                Console.WriteLine("Välj användartyp:");
                Console.WriteLine("1. Chef (Hantera parkeringen)");
                Console.WriteLine("2. Vakt (Parkera/Släpp ut fordon och ge böter)");
                Console.WriteLine("3. Kund (Se status, parkera fordon och kontrollera böter)");
                Console.WriteLine("4. Avsluta");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": // Chef
                        Console.WriteLine("\n** Chef - Hantera Parkeringen **");
                        parkingLot.ShowParkingInfo();
                        break;

                    case "2": // Vakt
                        Console.WriteLine("\n** Vakt - Parkera/Släpp ut Fordon och Ge Böter **");
                        Console.WriteLine("1. Parkera Fordon");
                        Console.WriteLine("2. Släpp ut Fordon");
                        Console.WriteLine("3. Ge Böter");
                        string guardChoice = Console.ReadLine();

                        if (guardChoice == "1") // Parkera Fordon
                        {
                            Console.WriteLine("Ange typ av fordon: Bil, MC, Buss");
                            string vehicleType = Console.ReadLine().ToLower();

                            Console.WriteLine("Ange färg på fordon:");
                            string color = Console.ReadLine();

                            string regNumber = Helper.GenerateRegNumber();

                            Vehicle vehicle = vehicleType switch
                            {
                                "bil" => new Car(regNumber, color, false),
                                "mc" => new Motorcycle(regNumber, color, "Harley"),
                                "buss" => new Bus(regNumber, color, 50),
                                _ => null
                            };

                            if (vehicle != null)
                            {
                                Console.WriteLine($"Fordonet {vehicle.RegNumber} kommer att parkeras.");
                                Console.WriteLine("Ange parkeringslängd i sekunder:");
                                int duration = int.Parse(Console.ReadLine());
                                parkingLot.AddVehicle(vehicle, duration);
                            }
                            else
                            {
                                Console.WriteLine("Ogiltig fordonstyp.");
                            }
                        }
                        else if (guardChoice == "2") // Släpp ut Fordon
                        {
                            Console.WriteLine("Ange registreringsnummer för fordon som ska släppas ut:");
                            string regNumber = Console.ReadLine();
                            parkingLot.ReleaseVehicle(regNumber);
                        }
                        else if (guardChoice == "3") // Ge Böter
                        {
                            Console.WriteLine("Har du sett att ett fordon står för länge? (Ja/Nej)");
                            string seenFine = Console.ReadLine().ToLower();

                            if (seenFine == "ja")
                            {
                                ParkingLot.SetHasSeenFine(true);
                                Console.WriteLine("Böter har satts på alla fordon som står över sin parkeringslängd.");
                            }
                            else
                            {
                                ParkingLot.SetHasSeenFine(false);
                                Console.WriteLine("Inga böter sätts.");
                            }
                        }
                        break;

                    case "3": // Kund
                        Console.WriteLine("\n** Kund - Se status, parkera och kontrollera böter **");
                        Console.WriteLine("1. Se Parkeringsstatus");
                        Console.WriteLine("2. Parkera Fordon");
                        Console.WriteLine("3. Kontrollera om jag har böter");
                        string customerChoice = Console.ReadLine();

                        if (customerChoice == "1") // Visa parkeringsstatus
                        {
                            parkingLot.DisplayParkingLot();
                        }
                        else if (customerChoice == "2") // Parkera fordon
                        {
                            Console.WriteLine("Ange typ av fordon: Bil, MC, Buss");
                            string vehicleType = Console.ReadLine().ToLower();

                            Console.WriteLine("Ange färg på fordon:");
                            string color = Console.ReadLine();

                            string regNumber = Helper.GenerateRegNumber();

                            Vehicle vehicle = vehicleType switch
                            {
                                "bil" => new Car(regNumber, color, false),
                                "mc" => new Motorcycle(regNumber, color, "Harley"),
                                "buss" => new Bus(regNumber, color, 50),
                                _ => null
                            };

                            if (vehicle != null)
                            {
                                Console.WriteLine($"Fordonet {vehicle.RegNumber} kommer att parkeras.");
                                Console.WriteLine("Ange parkeringslängd i sekunder:");
                                int duration = int.Parse(Console.ReadLine());
                                parkingLot.AddVehicle(vehicle, duration);
                            }
                            else
                            {
                                Console.WriteLine("Ogiltig fordonstyp.");
                            }
                        }
                        // För att skriva ut böter om de finns
                        else if (customerChoice == "3") // Kontrollera om kunden har böter
                        {
                            Console.WriteLine("Ange ditt registreringsnummer för att kontrollera om du har böter:");
                            string regNumber = Console.ReadLine();
                            var vehicleData = parkingLot.CheckIfHasFine(regNumber);

                            if (vehicleData != null)
                            {
                                // Här har vi en tuple: vehicleData.Item1 är fordonet, 
                                // så vi kan komma åt RegNumber genom vehicleData.Item1.RegNumber.
                                Console.WriteLine($"Fordon {vehicleData.Value.Item1.RegNumber} har böter på 500 kr om de är för sena.");
                            }
                            else
                            {
                                Console.WriteLine("Inga böter hittades.");
                            }
                        }
                        break;

                    case "4": // Avsluta
                        Console.WriteLine("Programmet avslutas.");
                        return;

                    default:
                        Console.WriteLine("Ogiltigt val, försök igen.");
                        break;
                }
            }
        }
    }
}
