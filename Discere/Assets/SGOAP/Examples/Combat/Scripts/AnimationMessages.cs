using System.Collections.Generic;
using UnityEngine;

public class AnimationMessages : MonoBehaviour
{
    private Dictionary<string, Signal> _signals = new Dictionary<string, Signal>();

    public AnimationProcessRoot AttackProcess;
    public AnimationProcessRoot ParryProcess;

    public bool Attacking => AttackProcess.State;

    private void Awake()
    {
        ParryProcess = new AnimationProcessRoot("Parry", _signals);
        AttackProcess = new AnimationProcessRoot("Attack", _signals);
    }

    public void Send(string message)
    {
        var process = _signals[message];
        process.Update();
    }

    public class Signal
    {
        public string Name;
        public bool State;

        public AnimationProcessRoot Root;

        public Signal(string name, AnimationProcessRoot root, bool state)
        {
            Name = name;
            Root = root;
            State = state;
        }

        public void Update()
        {
            Root.State = State;
        }
    }

    public class AnimationProcessRoot
    {
        public string Name;
        public bool State;

        public Signal Start;
        public Signal End;

        public AnimationProcessRoot(string name, Dictionary<string, Signal> currentSignals)
        {
            Name = name;
            Start = new Signal($"{name}Start", this, true);
            End = new Signal($"{name}End", this, false);

            currentSignals.Add(Start.Name, Start);
            currentSignals.Add(End.Name, End);
        }
    }
}
