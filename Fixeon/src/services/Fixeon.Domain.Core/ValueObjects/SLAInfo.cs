namespace Fixeon.Domain.Core.ValueObjects
{
    public class SLAInfo
    {
        public SLAInfo() { }
        public SLA FirstInteraction { get; set; }
        public SLA Resolution { get; set; }

        public void SetFirstResponseAccomplished()
            => FirstInteraction.Accomplish();

        public void SetResolutionAccomplished()
            => Resolution.Accomplish();

        public void SetFirstInteractionDeadline(int? deadlineInMinutes)
            => FirstInteraction.SetDeadline(deadlineInMinutes.Value);

        public void SetResolutionDeadline(int? deadlineInMinutes)
            => Resolution.SetDeadline(deadlineInMinutes.Value);

        public void RestartResolutionDate()
            => Resolution.RestartResolutionDate();
    }
}
