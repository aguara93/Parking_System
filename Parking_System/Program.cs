namespace Parking_System
{
    public class Program
    {
        static void Main(string[] args)
        {
            ParkingLot parkingLot = new ParkingLot();

            // Skapa några fordon slumpmässigt och parkera dem i systemet
            Random rand = new Random();
            for (int i = 0; i < 5; i++) // Lägg till 5 slumpmässiga fordon
            {
                string regNumber = Helper.GenerateRegNumber();
                string color = rand.Next(0, 2) == 0 ? "Röd" : "Blå";
                Vehicle vehicle;
                if (rand.Next(0, 3) == 0)
                {
                    vehicle = new Car(regNumber, color, rand.Next(0, 2) == 0);
                }
                else if (rand.Next(0, 3) == 1)
                {
                    vehicle = new Motorcycle(regNumber, color, "Yamaha");
                }
                else
                {
                    vehicle = new Bus(regNumber, color, 50);
                }

                parkingLot.AddVehicle(vehicle, rand.Next(30, 120)); // Slumpmässig parkeringstid mellan 30-120 sek
            }

            while (true)
            {
                Console.WriteLine("Välj användartyp:");
                Console.WriteLine("1. Chef (Hantera parkeringen)");
                Console.WriteLine("2. Vakt (Parkera/Släpp ut fordon)");
                Console.WriteLine("3. Kund (Se status och parkera fordon)");
                Console.WriteLine("4. Avsluta");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": // Chef
                        Console.WriteLine("\n** Chef - Hantera Parkeringen **");
                        parkingLot.ShowParkingInfo();
                        break;

                    case "2": // Vakt
                        Console.WriteLine("\n** Vakt - Parkera/Släpp ut Fordon **");
                        Console.WriteLine("1. Parkera Fordon");
                        Console.WriteLine("2. Släpp ut Fordon");
                        string guardChoice = Console.ReadLine();

                        if (guardChoice == "1")
                        {
                            Console.WriteLine("Ange fordonets typ (Bil/Motorcykel/Buss):");
                            string vehicleType = Console.ReadLine();
                            Console.WriteLine("Ange registreringsnummer:");
                            string regNumber = Console.ReadLine();
                            Vehicle vehicle = null;

                            if (vehicleType.ToLower() == "bil")
                                vehicle = new Car(regNumber, "Blå", true);
                            else if (vehicleType.ToLower() == "motorcykel")
                                vehicle = new Motorcycle(regNumber, "Svart", "Yamaha");
                            else if (vehicleType.ToLower() == "buss")
                                vehicle = new Bus(regNumber, "Gul", 55);

                            if (vehicle != null)
                            {
                                parkingLot.AddVehicle(vehicle, 60); // Parkeringstid på 60 sekunder
                            }
                        }
                        else if (guardChoice == "2")
                        {
                            Console.WriteLine("Ange registreringsnummer för fordon som ska släppas ut:");
                            string regNumber = Console.ReadLine();
                            parkingLot.ReleaseVehicle(regNumber);
                        }
                        break;

                    case "3": // Kund
                        Console.WriteLine("\n** Kund - Välj ett alternativ **");
                        Console.WriteLine("1. Se Parkeringsstatus");
                        Console.WriteLine("2. Parkera ett Fordon");
                        Console.WriteLine("3. Släpp ut ett Fordon");
                        Console.WriteLine("4. Tillbaka");
                        string customerChoice = Console.ReadLine();

                        if (customerChoice == "1")
                        {
                            parkingLot.DisplayParkingLot();
                        }
                        else if (customerChoice == "2")
                        {
                            Console.WriteLine("Ange fordonets typ (Bil/Motorcykel/Buss):");
                            string vehicleType = Console.ReadLine();
                            Console.WriteLine("Ange registreringsnummer:");
                            string regNumber = Console.ReadLine();
                            Console.WriteLine("Ange parkeringstid i sekunder:");
                            int parkingTime = int.Parse(Console.ReadLine());
                            Vehicle vehicle = null;

                            if (vehicleType.ToLower() == "bil")
                                vehicle = new Car(regNumber, "Blå", true);
                            else if (vehicleType.ToLower() == "motorcykel")
                                vehicle = new Motorcycle(regNumber, "Svart", "Yamaha");
                            else if (vehicleType.ToLower() == "buss")
                                vehicle = new Bus(regNumber, "Gul", 55);

                            if (vehicle != null)
                            {
                                parkingLot.AddVehicle(vehicle, parkingTime);
                            }
                        }
                        else if (customerChoice == "3")
                        {
                            Console.WriteLine("Ange registreringsnummer för fordon som ska släppas ut:");
                            string regNumber = Console.ReadLine();
                            parkingLot.ReleaseVehicle(regNumber);
                        }
                        else if (customerChoice == "4")
                        {
                            continue; // Gå tillbaka till huvudmenyn
                        }
                        break;


                    case "4": // Avsluta
                        Console.WriteLine("Avslutar programmet.");
                        return;

                    default:
                        Console.WriteLine("Ogiltigt val, försök igen.");
                        break;
                }
                Console.WriteLine("\nTryck på en tangent för att fortsätta...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
