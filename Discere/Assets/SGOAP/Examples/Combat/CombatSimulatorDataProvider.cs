using SGoap.Services;

namespace SGoap
{
    using Combatant;

    public class CombatSimulatorDataProvider : SimulatorDataProvider
    {
        public BasicAgent Agent;

        public override void Setup()
        {
            Agent.Initialize();
            Agent.Data.Target = FindObjectOfType<PlayerController>().transform;

            var grenades = FindObjectsOfType<Grenade>();
            foreach (var grenade in grenades)
                ObjectManager<Grenade>.Add(grenade);
        }

        public override void Clean()
        {
            ObjectManager<Grenade>.Clear();
            TargetManager.Clear();
        }
    }
}