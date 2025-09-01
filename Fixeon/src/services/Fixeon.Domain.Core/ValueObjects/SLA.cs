namespace Fixeon.Domain.Core.ValueObjects
{
    public class SLA
    {
        public DateTime? Deadline { get; set; }
        public DateTime? Accomplished { get; set; }
        public bool? WithinDeadline =>
            Deadline.HasValue && Accomplished.HasValue
            ? Accomplished <= Deadline
            : null;

        public SLA(DateTime? deadline, DateTime? accomplished)
        {
            Deadline = deadline;
            Accomplished = accomplished;
        }

        public void Accomplish() => Accomplished = DateTime.Now;
    }
}
