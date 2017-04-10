using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class RangeSubject : BaseSubject<int>
    {
        private int value;
        private int xtime;

        public RangeSubject(int value, int xtime)
        {
            this.value = value;
            this.xtime = xtime;
        }

        public RangeSubject(int value, int xtime, IScheduler scheduler)
        {
            this.value = value;
            this.xtime = xtime;
            this.Scheduler = scheduler;
        }

        public class ThreadExecuter
        {
            public int Value { get; set; }
            public int XTime { get; set; }
            public List<IObserver<int>> Observers { get; set; }
            public IScheduler Scheduler { get; set; }
            private int repeatCounter;

            public void Execute()
            {
                Action<int> action = null;
                action = (int value) =>
                {
                    if (Observers.Count <= 0)
                        return;
                    for (int i = 0; i < Observers.Count; i++)
                    {
                        Observers[i].OnNext(value);
                    }
                    if (repeatCounter < XTime - 1)
                    {
                        repeatCounter++;                        
                        Scheduler.Schedule(action, ++value);
                    }
                    else
                    {
                        for (int i = 0; i < Observers.Count; i++)
                        {
                            Observers[i].OnCompleted();
                        }
                    }
                    return;
                };
                action(Value);
            }
        }
        
        public override void Execute()
        {
            ThreadExecuter threadExecuter = 
                new ThreadExecuter() { 
                    Value = value, 
                    Observers = observers,
                    XTime = xtime, 
                    Scheduler = this.Scheduler};
            Scheduler.Schedule(threadExecuter.Execute);
        }
    }
}
