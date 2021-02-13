using SGoap.Services;

namespace SGoap.Example
{
    public class WoodCutterSimulatorDataProvider : SimulatorDataProvider
    {
        public override void Setup()
        {
            var agent = GetComponent<BasicAgent>();
            agent.Initialize();

            var trees = FindObjectsOfType<TreeObject>();
            ObjectManager<TreeObject>.AddRange(trees);
        }

        public override void Clean()
        {
            ObjectManager<TreeObject>.Clear();
            TargetManager.Clear();
        }
    }
}