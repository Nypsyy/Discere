using System.Collections;
using SGoap.Services;
using UnityEngine;

namespace SGoap.Example
{
    public class ChopTree : BasicAction
    {
        public StringReference Wood;

        public override float StaggerTime => 0;
        public override float CooldownTime => 0;

        public TreeObject Tree;

        public override bool PrePerform()
        {
            if (Tree == null)
                return false;

            StartCoroutine(Routine());
            return base.PrePerform();

            IEnumerator Routine()
            {
                while (Tree != null && Tree.Wood >= 1)
                {
                    yield return new WaitForSeconds(1);

                    Tree.TakeWood();
                    States.ModifyState(Wood.Value, 1);
                }

                ObjectManager<TreeObject>.Remove(Tree);
                Destroy(Tree.gameObject);
                Tree = null;
            }
        }

        public override EActionStatus Perform()
        {
            return Tree == null ? EActionStatus.Success : EActionStatus.Running;
        }
    }
}