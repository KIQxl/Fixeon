﻿namespace Fixeon.Domain.Core.ValueObjects
{
    public record User
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
