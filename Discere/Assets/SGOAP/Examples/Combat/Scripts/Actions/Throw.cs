using UnityEngine;

namespace SGoap
{
    public class Throw : BasicAction
    {
        public StringReference State;
        public AnimationCurve Y;

        public override float CooldownTime => 15;
        public override float StaggerTime => 0.3f;

        public override bool PostPerform()
        {
            States.ModifyState(State.Value, -1);

            var cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            cube.transform.localScale = Vector3.one * 0.2f;
            cube.transform.position = AgentData.Position;

            var startY = cube.transform.position.y;
            var startPos = cube.transform.position;
            var endPos = AgentData.Target.position;

            LerpExtensions.Lerp(val =>
            {
                var pos = Vector3.Lerp(startPos, endPos, val);
                pos.y = startY + Y.Evaluate(val);
                cube.transform.position = pos;
            }, 0.5f);

            return base.PostPerform();
        }
    }
}