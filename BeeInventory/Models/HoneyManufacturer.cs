namespace BeeInventory.Models
{
    public class HoneyManufacturer : WorkerBee
    {
        // Book/GitHub numbers:
        // Uses 2 nectar → produces 1 honey
        private const int NectarConsumedPerShift = 2;
        
        private const int HoneyProducedPerShift = 1;

        public HoneyManufacturer() : base("Honey Manufacturer")
        {
        }

        // All worker bees eat 1 honey per shift
        public override int HoneyConsumptionRate => 1;

        public override string Work(Hive hive)
        {
            // If we don't have enough nectar, do nothing
            if (hive.Nectar < NectarConsumedPerShift)
            {
                return "Honey manufacturer found no nectar to convert.";
            }

            int amount = HoneyProducedPerShift * hive.HoneyManufacturerUpgradeLevel;
        // Convert nectar → honey through Hive's helper
        hive.ConvertNectarToHoney(
                NectarConsumedPerShift,
                amount
            );

            return $"Honey manufacturer converted {NectarConsumedPerShift} nectar into {HoneyProducedPerShift} honey.";
        }
    }
}
