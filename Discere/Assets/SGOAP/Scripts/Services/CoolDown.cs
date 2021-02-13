using System.Collections;
using UnityEngine;

namespace SGoap
{
    public class CoolDown
    {
        public bool Active { get; set; }

        private float _startTime;
        private float _targetDuration;

        public float TimeElapsed => Time.time - _startTime;

        public Coroutine Run(float duration)
        {
            if (Active)
                return null;

            if (duration == 0)
                return null;

            return CoroutineService.Instance.StartCoroutine(Routine());

            IEnumerator Routine()
            {
                Active = true;

                _targetDuration = duration;
                _startTime = Time.time;

                while (Time.time - _startTime < _targetDuration)
                    yield return null;

                Active = false;
            }
        }

        public void Stop()
        {
            _targetDuration = 0;
            Active = false;
        }
    }
}