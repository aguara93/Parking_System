using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Parking_System
{
    // Fordonsklasser
    class Vehicle
    {
        public string RegNumber { get; }
        public string Color { get; }

        public Vehicle(string regNumber, string color)
        {
            RegNumber = regNumber;
            Color = color;
        }

        public virtual double SpotsNeeded => 1;  // Standardstorlek på parkeringsplats
        public virtual double CalculateParkingFee(int parkedSeconds) => parkedSeconds * 1.5;  // Standardavgift
    }

    class Car : Vehicle
    {
        public bool IsElectric { get; }

        public Car(string regNumber, string color, bool isElectric) : base(regNumber, color)
        {
            IsElectric = isElectric;
        }

        public override double SpotsNeeded => 1;
        public override double CalculateParkingFee(int parkedSeconds)
        {
            double baseFee = parkedSeconds * 1.5;
            return baseFee;
        }
    }

    class Motorcycle : Vehicle
    {
        public string Brand { get; }

        public Motorcycle(string regNumber, string color, string brand) : base(regNumber, color)
        {
            Brand = brand;
        }

        public override double SpotsNeeded => 0.5;  // MC tar mindre plats
    }

    class Bus : Vehicle
    {
        public int PassengerCount { get; }

        public Bus(string regNumber, string color, int passengerCount) : base(regNumber, color)
        {
            PassengerCount = passengerCount;
        }

        public override double SpotsNeeded => 2;  // Bussar tar mer plats
    }
}