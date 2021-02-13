using SGoap;
using System.Linq;
using SGoap.Services;
using UnityEngine;

namespace SGoap
{
    public class PickUp<T> : Chase, IPickUp where T : Component, IItem
    {
        public StringReference State;
        private T _grenade;

        public StringReference StateReference => State;
        public Action Action => this;

        public override bool CanAbort()
        {
            return AgentData.DistanceToTarget < 1.5f || OtherAgentAlsoPicking();
        }

        public override bool PostPerform()
        {
            if (Target == null)
            {
                return false;
            }
            else
            {
                _grenade = Target.transform.GetComponent<T>();
            }

            _grenade.gameObject.SetActive(false);
            AgentData.Inventory.Add(_grenade);
            States.ModifyState(State.Value, 1);

            return base.PostPerform();
        }

        public Transform GetClosest()
        {
            if (_grenade != null)
                return _grenade.transform;

            _grenade = ObjectManager<T>.FindClosest(AgentData.Position);

            if (_grenade == null)
                return null;

            Target = _grenade.gameObject;
            return Target.transform;
        }

        public bool OtherAgentAlsoPicking()
        {
            var otherAgents = FindObjectsOfType<Agent>();
            if (otherAgents.Any(x => x != AgentData.Agent && x.CurrentAction is T pick && pick.transform.GetClosestNearby<T>(5) == _grenade))
                return true;

            return false;
        }
    }
}