using Hangfire.Common;
using Hangfire.States;
using System;

namespace AHSync.Worker.Shared.Filters
{
    public class RetryFilterAttribute : JobFilterAttribute, IElectStateFilter
    {
        public void OnStateElection(ElectStateContext context)
        {
            var enqueuedState = context.CandidateState as EnqueuedState;
            if (enqueuedState != null)
            {
                var qn = context.GetJobParameter<string>("QueueName");
                if (!String.IsNullOrWhiteSpace(qn))
                {
                    enqueuedState.Queue = qn;
                }
                else
                {
                    context.SetJobParameter("ah-sync", enqueuedState.Queue);
                }
            }
        }
    }
}
