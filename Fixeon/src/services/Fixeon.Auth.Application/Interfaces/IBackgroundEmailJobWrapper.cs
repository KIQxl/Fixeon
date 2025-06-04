using Fixeon.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixeon.Auth.Application.Interfaces
{
    public interface IBackgroundEmailJobWrapper
    {
        public void SendWelcomeEmail(EmailMessage email);
    }
}
