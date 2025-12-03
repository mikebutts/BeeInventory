
namespace BeeInventory.Models
{
    public abstract class Bee
    {
        public string Job { get; protected set; } = "Idle";

        // How much honey this bee eats per shift
        public virtual int HoneyConsumptionRate => 1;

        // Runs once per shift
        // Returns a string describing what the bee accomplished.
        public abstract string Work(Hive hive);

        // Used as a display name or identifier
        public override string ToString() => Job;
    }
}
