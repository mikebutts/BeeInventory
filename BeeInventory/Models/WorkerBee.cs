

namespace BeeInventory.Models
{
        public class WorkerBee : Bee
        {
            public WorkerBee(string job)
            {
                Job = job;
            }

            // Worker bees generally eat 1 honey per shift.
            // Individual subclasses may override this.
            public override int HoneyConsumptionRate => 1;

            // If a worker has a job, it should override this method in the subclass.
            // If the worker is idle, this fallback message is used.
            public override string Work(Hive hive)
            {
                if (Job == "Idle")
                {
                    return "An idle worker did nothing this shift.";
                }

                // If a subclass forgets to override Work(), this prevents crashes.
                return $"{Job} bee worked, but no specific action was defined.";
            }

            // Changes this worker’s job (used when assigning a bee)
            public void AssignJob(string job)
            {
                Job = job;
            }
        }
    }
