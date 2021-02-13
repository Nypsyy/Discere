using SGoap.Services;

namespace SGoap.Example
{
    public class GoToTree : NavMeshAction
    {
        public ChopTree ChopAction;
        private TreeObject _tree;

        public override bool IsUsable()
        {
            _tree = ObjectManager<TreeObject>.FindClosest(AgentData.Position);

            if (_tree == null)
                return false;

            Target = _tree.gameObject;

            return base.IsUsable();
        }

        public override bool PostPerform()
        {
            ChopAction.Tree = _tree;

            return base.PostPerform();
        }
    }
}