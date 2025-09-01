namespace Fixeon.Domain.Core.ValueObjects
{
    public class SLAInfo
    {
        public SLA FirstResponse { get; set; }
        public SLA Resolution { get; set; }

        public SLAInfo(int firstResponseDeadline, int resolutionDeadline)
        {
            FirstResponse = new SLA(DateTime.Now.AddMinutes(firstResponseDeadline), null);
            Resolution = new SLA(DateTime.Now.AddMinutes(resolutionDeadline), null);
        }

        public void SetFirstResponseAccomplished()
            => FirstResponse.Accomplish();

        public void SetResolutionAccomplished()
            => Resolution.Accomplish();
    }
}
