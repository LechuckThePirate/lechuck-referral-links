using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.DataAccess.Entities;

namespace LeChuck.ReferralLinks.Lambda.Timer.Processors
{
    public interface ITimeProcessor
    {
        Task SetNextRun(TimedTaskDbEntity data);
    }

    public class TimeProcessor : ITimeProcessor
    {
        public Task SetNextRun(TimedTaskDbEntity data)
        {
            var nextRun = DateTime.Now.AddMinutes(data.RunSpan.Minutes);
            throw new NotImplementedException();
        }
    }
}
