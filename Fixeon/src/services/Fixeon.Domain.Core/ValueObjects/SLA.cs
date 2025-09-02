namespace Fixeon.Domain.Core.ValueObjects
{
    public class SLA
    {
        public SLA() { }
        public DateTime? Deadline { get; set; }
        public DateTime? Accomplished { get; set; }
        public bool? WithinDeadline =>
            Deadline.HasValue && Accomplished.HasValue
            ? Accomplished <= Deadline
            : null;

        public void Accomplish()
        {
            if(!Accomplished.HasValue && Deadline.HasValue)
                Accomplished = DateTime.Now;
        }
        public void SetDeadline(int deadlineInMinutes)
        {
            if(!Deadline.HasValue)
                Deadline = DateTime.Now.AddMinutes(deadlineInMinutes);
        }
    }
}
