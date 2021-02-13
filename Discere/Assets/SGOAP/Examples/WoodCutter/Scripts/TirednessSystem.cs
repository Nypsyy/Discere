using UnityEngine;

namespace SGoap.Example
{
    public class TirednessSystem : MonoBehaviour 
    {
        public BasicAgent Agent;
        public StringReference TirednessState;

        public float Tiredness => Agent.States.GetValue("Tiredness");
        public float TirednessUsingState => Agent.States.GetValue(TirednessState.Value);
    }
}