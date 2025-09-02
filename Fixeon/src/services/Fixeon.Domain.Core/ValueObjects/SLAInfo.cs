namespace Fixeon.Domain.Core.ValueObjects
{
    public class SLAInfo
    {
        public SLA FirstInteraction { get; set; }
        public SLA Resolution { get; set; }

        public SLAInfo(int? firstResponseDeadline)
        {
            FirstInteraction = new SLA(DateTime.Now.AddMinutes(firstResponseDeadline.Value), null);
            Resolution = new SLA(null, null);
        }

        public void SetFirstResponseAccomplished()
            => FirstInteraction.Accomplish();

        public void SetResolutionAccomplished()
            => Resolution.Accomplish();

        public void SetResolutionDeadline(int? resolutionDeadline)
            => Resolution.Deadline = DateTime.Now.AddMinutes(resolutionDeadline.Value);
    }
}
