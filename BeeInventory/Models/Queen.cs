using System.Text;

namespace BeeInventory.Models
{
    public class Queen
    {
        private readonly Hive hive;

        public Queen(Hive hive)
        {
            this.hive = hive;
            StatusReport = "The queen is ready.";
        }

        public string StatusReport { get; set; } = "The queen is ready.";

        public void AssignBee(string selectedJob)
        {
            // Delegate the real work to the hive, then update the report
            var assignmentResult = hive.AssignBee(selectedJob);

            StatusReport = BuildStatusReport(
                $"Assignment: {assignmentResult}"
            );
        }



        
        /// Runs the next shift. Returns false if the hive ran out of honey.
       
        public bool WorkTheNextShift()
        {
            string shiftReport = hive.WorkTheNextShift();
            StatusReport = BuildStatusReport(shiftReport);

            // Simple rule: hive is “dead” if honey < 0
            return hive.Honey >= 0;
        }

        private string BuildStatusReport(string headerLine)
        {
            var sb = new StringBuilder();

            sb.AppendLine(headerLine);
            sb.AppendLine();
            sb.AppendLine($"Shift: {hive.ShiftNumber}");
            sb.AppendLine($"Honey: {hive.Honey}");
            sb.AppendLine($"Nectar: {hive.Nectar}");
            sb.AppendLine($"Eggs: {hive.Eggs}");
            sb.AppendLine();
            sb.AppendLine($"Unassigned workers: {hive.UnassignedWorkers}");
            sb.AppendLine($"Nectar collectors: {hive.NectarCollectors}");
            sb.AppendLine($"Honey manufacturers: {hive.HoneyManufacturers}");
            sb.AppendLine($"Egg care bees: {hive.EggCareBees}");
            sb.AppendLine($"Total workers: {hive.TotalWorkers}");

            return sb.ToString();
        }
    }
}
