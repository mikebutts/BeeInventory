using BeeInventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BeeInventory.Models { 
    public class Hive
    {
        public int NectarCollectorUpgradeLevel { get; private set; } = 1;
        public int QueenEggUpgradeLevel { get; private set; } = 1;
        public int HoneyManufacturerUpgradeLevel { get; private set; } = 1;
        // Stater basic costs , can be adjusted
        public int NectarCollectorUpgradeCost => 25 * NectarCollectorUpgradeLevel;
        public int QueenEggUpgradeCost => 30 * QueenEggUpgradeLevel;
        public int HoneyManufacturerUpgradeCost => 20 * HoneyManufacturerUpgradeLevel;
        public bool pauseHatching = false;

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
            // Start with 3 workers: one of each type
            bees.Add(new NectarCollector());
            bees.Add(new HoneyManufacturer());
            bees.Add(new EggCare());
        }

        // --- Counts for the UI ---
        public int TotalWorkers => bees.OfType<WorkerBee>().Count();
        public int UnassignedWorkers => bees.OfType<WorkerBee>().Count(b => b.Job == "Idle");

        public int NectarCollectors => bees.Count(b => b.Job == "Nectar Collector");
        public int HoneyManufacturers => bees.Count(b => b.Job == "Honey Manufacturer");
        public int EggCareBees => bees.Count(b => b.Job == "Egg Care");

        // --- Called by Queen.AssignBee(job) ---
        public string AssignBee(string job)
        {
            var index = bees.FindIndex(b => b is WorkerBee w && w.Job == "Idle"); ;

            if (index == -1)
                return "No unassigned workers available.";

            Bee newWorker = job switch
            {
                "Nectar Collector" => new NectarCollector(),
                "Honey Manufacturer" => new HoneyManufacturer(),
                "Egg Care" => new EggCare(),
                _ => new WorkerBee("Idle")
            };

            bees[index] = newWorker;

            return $"Assigned a worker to '{job}'.";
        }
        public void pauseHatchingToggle()
        {
            pauseHatching = !pauseHatching;
        }

        private void AddNewWorker()
        {
            // Add a brand-new idle worker (same as book logic)
            bees.Add(new WorkerBee("Idle"));
        }

        // --- Called by Queen.WorkTheNextShift() ---
        public string WorkTheNextShift()
        {
            ShiftNumber++;

            var report = new StringBuilder();
            report.AppendLine($"--- Shift #{ShiftNumber} Report ---");

            // Each bee consumes honey
            foreach (var bee in bees.ToList())          
            {
                Honey -= bee.HoneyConsumptionRate;
            }

            if (Honey <= 0)
            {
                Honey = 0;
                report.AppendLine("The hive has run out of honey! Bee-nkruptcy!");
                report.AppendLine(BuildCountsReport());
                return report.ToString();
            }

            // Queen lays eggs
            LayEggs();

            // Bees do their work (workers override Work)
            foreach (var worker in bees.OfType<WorkerBee>().ToList())   
            {
                report.AppendLine(worker.Work(this));
            }

            report.AppendLine();
            report.AppendLine(BuildCountsReport());

            return report.ToString();
        }

        public bool UpgradeHoneyManufacturers()
        {
            if (Nectar < HoneyManufacturerUpgradeCost)
            {
                return false;
            }
            else
            {
                Nectar -= HoneyManufacturerUpgradeCost;
                HoneyManufacturerUpgradeLevel++;
                return true;
            }
        }
        public bool UpgradeNectarCollectors()
        {
            if (Nectar < NectarCollectorUpgradeCost)
            {
                return false;

            }
            else
            {
                Nectar -= NectarCollectorUpgradeCost;
                NectarCollectorUpgradeLevel++;
                return true;
            }
        }

        public bool UpgradeQueenEggs()
        {
            if (Honey < QueenEggUpgradeCost)
                return false;

            Honey -= QueenEggUpgradeCost;
            QueenEggUpgradeLevel++;
            return true;
        }

        private string BuildCountsReport()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Honey: {Honey}");
            sb.AppendLine($"Nectar: {Nectar}");
            sb.AppendLine($"Eggs: {Eggs}");
            sb.AppendLine($"Unassigned Workers: {UnassignedWorkers}");
            sb.AppendLine($"Nectar Collectors: {NectarCollectors}");
            sb.AppendLine($"Honey Manufacturers: {HoneyManufacturers}");
            sb.AppendLine($"Egg Care Bees: {EggCareBees}");
            sb.AppendLine($"Total Workers: {TotalWorkers}");
            return sb.ToString();
        }

        // --- Helper methods for worker jobs to call ---

        public void CollectNectar(int amount)
        {
            Nectar += amount;
        }

        public void ConvertNectarToHoney(int nectarAmount, int honeyAmount)
        {
            if (Nectar >= nectarAmount)
            {
                Nectar -= nectarAmount;
                Honey += honeyAmount;
            }
        }

        public void LayEggs()
        {
            int eggsLaid = QueenEggUpgradeLevel;  // base 1 egg per level
            Eggs += eggsLaid;
        }

        public void CareForEggs(int eggsToHatch)
        {
            if (Eggs >= eggsToHatch)
            {
                Eggs -= eggsToHatch;

                // each hatched egg becomes a new idle worker
                for (int i = 0; i < eggsToHatch; i++)
                    bees.Add(new WorkerBee("Idle"));
            }
        }
    }
}
