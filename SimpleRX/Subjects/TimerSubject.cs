using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SimpleRX
{
    public class TimerSubject<T> : BaseSubject<T>
    {
        private TimeSpan dueTime;
        private TimeSpan period;
        private Timer timer;

        public TimerSubject(TimeSpan dueTime, TimeSpan period)
        {
            this.dueTime = dueTime;
            this.period = period;
        }

        public override void Execute()
        {
            timer = new Timer(obj =>
            {
                NotifyObservers(default(T));   
            }, default(T), dueTime, period);
        }
    }
}
