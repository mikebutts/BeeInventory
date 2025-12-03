

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
            //if there are no eggs yet, just report that nothing is hatched
            if (hive.Eggs < EggsHatchedPerShift)
            {
                return "Egg care bee tended the brood, but no eggs were ready to hatch.";
            }

            // use a helper on Hive to convert eggs -> workers
            hive.CareForEggs(EggsHatchedPerShift);

            return $"Egg care bee hatched {EggsHatchedPerShift} new worker bee.";
        }
    }
}
