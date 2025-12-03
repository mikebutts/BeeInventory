namespace BeeInventory.Models
{
    public class EggCare : WorkerBee
    {
        private const int EggsHatchedPerShift = 1;

        public EggCare() : base("Egg Care")
        {
        }

        public override int HoneyConsumptionRate => 1;

        public override string Work(Hive hive)
        {
            if (hive.Eggs < EggsHatchedPerShift)
            {
                return "Egg care bee tended the brood, but no eggs hatched.";
            }

            hive.CareForEggs(EggsHatchedPerShift);

            return $"Egg care bee hatched {EggsHatchedPerShift} new worker bee.";
        }
    }
}
