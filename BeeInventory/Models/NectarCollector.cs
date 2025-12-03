namespace BeeInventory.Models
{
    public class NectarCollector : WorkerBee
    {
        // Book/GitHub value: collect 3 nectar per shift
        private const int NectarPerShift = 3;

        public NectarCollector() : base("Nectar Collector")
        {
        }

        // All worker bees eat 1 honey per shift, same as other jobs
        public override int HoneyConsumptionRate => 1;

        public override string Work(Hive hive)
        {
            hive.CollectNectar(NectarPerShift);
            return $"Nectar collector gathered {NectarPerShift} nectar.";
        }
    }
}
