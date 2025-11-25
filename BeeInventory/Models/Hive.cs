using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeeInventory.Models
{
    public class Hive
    {
        private readonly List<Bee> bees = new();
        public int Honey { get; private set; } = 25;
        public int Nectar { get; private set; } = 100;
        public int Eggs { get; private set; } = 0;
        public int ShiftNumber { get; private set; } = 0;


        // Jobs the UI dropdown shows
        public IReadOnlyList<string> AvailableJobs { get; } =
            new List<string> { "Nectar Collector", "Honey Manufacturer", "Egg Care" };

        public Hive()
        {
            bees.Add(new NectarCollector());
            bees.Add(new HoneyManufacturer());
            bees.Add(new EggCare());
        }

        //counts for the UI
        public int TotalWorkers => bees.OfType<WorkerBee>().Count();
        public int UnassignedWorkers => bees.OfType<WorkerBee>().Count(b => b.Job =="Idle");
        public int NectarCollectors => bees.Count(b => b.Job == "Nectar Collector");
        public int HoneyManufacturers => bees.Count(b => b.Job == "Honey Manufacturer");
        public int EggCareBees => bees.Count(b => b.Job == "Egg Care");


    }
}
