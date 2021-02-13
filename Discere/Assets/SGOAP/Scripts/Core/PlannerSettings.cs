using System;

namespace SGoap
{
    [Serializable]
    public class PlannerSettings
    {
        public float PlanRate = 1;
        public bool CanAbortPlans;
        public bool GenerateGoalReport;
        public bool GenerateFailedPlansReport;
    }
}