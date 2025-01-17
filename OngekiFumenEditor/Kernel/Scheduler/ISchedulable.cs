﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OngekiFumenEditor.Kernel.Scheduler
{
    public interface ISchedulable
    {
        public string SchedulerName { get; }
        public void OnSchedulerTerm();
        public TimeSpan ScheduleCallLoopInterval { get; }
        public Task OnScheduleCall(CancellationToken cancellationToken);
    }
}
