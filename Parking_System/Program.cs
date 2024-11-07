namespace ParkingSystem
{
    public class Program
    {
        static void Main(string[] args)
        {
            ParkingLot parkingLot = new ParkingLot();

            // Skapa generellt fordon och specialiserade fordon
            Vehicle genericVehicle = new Vehicle(Helper.GenerateRegNumber(), "Blå");
            Car car = new Car(Helper.GenerateRegNumber(), "Röd", true);
            Motorcycle motorcycle = new Motorcycle(Helper.GenerateRegNumber(), "Svart", "Yamaha");
            Bus bus = new Bus(Helper.GenerateRegNumber(), "Gul", 55);

            // Lägg till fordon med angiven parkeringstid i sekunder
            parkingLot.AddVehicle(genericVehicle, 45); // Generellt fordon med 45 sek parkeringstid
            parkingLot.AddVehicle(car, 30);            // Bil med 30 sek parkeringstid
            parkingLot.AddVehicle(motorcycle, 60);     // Motorcykel med 60 sek parkeringstid
            parkingLot.AddVehicle(bus, 120);           // Buss med 120 sek parkeringstid

            // Visa parkeringsstatus
            parkingLot.DisplayParkingLot();

            // Simulera att tiden går och checka ut fordon
            Thread.Sleep(5000);  // Vänta 5 sekunder
            parkingLot.ReleaseVehicle(genericVehicle.RegNumber);

            // Visa uppdaterad status
            parkingLot.DisplayParkingLot();
        }
    }
}
