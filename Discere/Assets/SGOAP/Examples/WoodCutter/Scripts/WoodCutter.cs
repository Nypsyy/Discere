using SGoap.Services;

namespace SGoap.Example
{
    public class WoodCutter : BasicAgent
    {
        public StringReference WorkToDoState;

        private void Update()
        {
            if (ObjectManager<TreeObject>.Count == 0)
                States.RemoveState(WorkToDoState.Value);
            else
                States.SetState(WorkToDoState.Value, 1);
        }
    }
}