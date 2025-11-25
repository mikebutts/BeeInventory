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


        //methods used by Queen
        public string AssignBee(string job)
        {
            var unassigned = bees.OfType<WorkerBee>().FirstOrDefault(bees => bees.Job == "Idle");
            if (unassigned is null)
            {
                return "No unassigned bees are available.";
            }
            else
            {
                unassigned.AssignJob(job);
                AddNewWorker();
                return $"A bee has been assigned to {job}.";
            }
        }

        private void AddNewWorker()
        {
            if (Eggs >= 1)
            {
                bees.Add(new WorkerBee("Idle"));
                Eggs--;
            }
        }

        public string WorkTheNextShift()
        {
            ShiftNumber++;
            var report = new StringBuilder();
            report.AppendLine($"--- Shift #{ShiftNumber} Report ---");

            foreach(var bee in bees)
            {
                Honey -= bee.HoneyConsumptionRate;
            }
            if (Honey < 0)
            {
                report.AppendLine("The hive has run out of honey and the bees are starving.");
                report.AppendLine(BuildCountsReport());
                return report.ToString();
            }

            // Bees do thier work for the shift
            foreach (var worker in bees.OfType<WorkerBee>())
            {
                report.AppendLine(worker.Work(this));
            }
            {
                report.AppendLine(worker.Work(this));
            }

            // End of shift status
            report.AppendLine();
            report.AppendLine(BuildCountsReport());
            
            return report.ToString();  
        }

    }
}
