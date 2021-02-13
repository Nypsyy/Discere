using UnityEngine;

namespace SGoap
{
    public interface IPickUp
    {
        Action Action { get; }
        StringReference StateReference { get; }
        Transform GetClosest();
        bool OtherAgentAlsoPicking();
    }
}