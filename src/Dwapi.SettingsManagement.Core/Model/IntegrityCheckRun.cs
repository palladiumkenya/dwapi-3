using System;
using Dwapi.SharedKernel.Enum;
using Dwapi.SharedKernel.Model;

namespace Dwapi.SettingsManagement.Core.Model
{
    public class IntegrityCheckRun:Entity<Guid>
    {
        public Guid IntegrityCheckId { get; set; }
        public DateTime RunDate { get; set; }
        public LogicStatus RunStatus { get; set; }

        public IntegrityCheckRun()
        {
        }
        public IntegrityCheckRun(LogicStatus runStatus,Guid integrityCheckId)
        {
            IntegrityCheckId = integrityCheckId;
            RunStatus = runStatus;
            RunDate=DateTime.Now;
        }
    }
}