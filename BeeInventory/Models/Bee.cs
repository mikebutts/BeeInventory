namespace BeeInventory.Models
{
    public abstract class Bee
    {
        public string Job { get; private set; } = "Idle";

        public virtual int HoneyConsumptionRate => 1;
        public abstract string Work(Hive hive);
        public override string ToString() => Job;



    }
}
