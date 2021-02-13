using System.Collections;
using UnityEngine;

namespace SGoap
{
    public abstract class CoroutineAction : BasicAction
    {
        public System.Action OnFirstPerform;

        public float MinimumRuntime = 0.5f;

        [Header("Cool down settings")]
        [MinMax(0, 15)]
        public RangeValue CooldownRangeValue;

        [MinMax(0, 15)]
        public RangeValue StaggerRangeValue = new RangeValue(0, 0);

        public override float CooldownTime => CooldownRangeValue.GetRandomValue();
        public override float StaggerTime => StaggerRangeValue.GetRandomValue();

        // When Agent planner can abort, this means a coroutine can get interrupted which we don't want.
        public override bool CanAbort() => false;

        public EActionStatus Status { get; set; }

        private Coroutine _coroutine;

        public override bool PrePerform()
        {
            if (!base.PrePerform())
                return false;

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(Routine());

            return true;

            IEnumerator Routine()
            {
                Status = EActionStatus.Running;

                OnFirstPerform?.Invoke();
                yield return PerformRoutine();
                Status = EActionStatus.Success;
            }
        }

        public override EActionStatus Perform()
        {
            if (TimeElapsed < MinimumRuntime)
                return EActionStatus.Running;

            return Status;
        }

        public abstract IEnumerator PerformRoutine();
    }
}