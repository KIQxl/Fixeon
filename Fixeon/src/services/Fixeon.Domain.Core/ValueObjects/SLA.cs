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
                Accomplished = DateTime.UtcNow;
        }
        public void SetDeadline(int deadlineInMinutes)
        {
            if(!Deadline.HasValue)
                Deadline = DateTime.UtcNow.AddMinutes(deadlineInMinutes);
        }

        public void RestartResolutionDate()
        {
            if (Accomplished.HasValue)
                Accomplished = null;
        }
    }
}
