using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleRX
{
public class RepeatSubject<T> : BaseSubject<T>
{
    private T value;
    private int repeatCount;
    private bool repeatEndless = true;

    public class ThreadExecuter<T>
    {
        public T Value { get; set; }
        public int RepeatCount { get; set; }
        public bool RepeatEndless { get; set; }
        public List<IObserver<T>> Observers { get; set; }
        public IScheduler Scheduler { get; set; }
 
        public void Execute()
        {
            Action action = null;
            action = () =>
            {
                if (Observers.Count <= 0)
                    return;
                for(int i = 0; i < Observers.Count; i++)
                {
                    Observers[i].OnNext(Value);
                }
                if (!RepeatEndless)
                {
                    if (RepeatCount > 0)
                    {
                        RepeatCount--;                        
                        Scheduler.Schedule(action);
                    }
                    else
                    {
                        for (int i = 0; i < Observers.Count; i++)
                        {
                            Observers[i].OnCompleted();
                        }
                    }
                }
                else
                {
                    Scheduler.Schedule(action);
                }
                return;
            };
            action();
        }
    }

    public RepeatSubject(T value)
    {
        this.value = value;
    }

    public RepeatSubject(T value, IScheduler scheduler) : this(value)
    {
        this.Scheduler = scheduler;
    }

    public RepeatSubject(T value, int repeatCount) : this(value)
    {            
        this.repeatCount = repeatCount;
        this.repeatEndless = false;
    }

    public RepeatSubject(T value, int repeatCount, IScheduler scheduler) : this(value, repeatCount)
    {
        this.Scheduler = scheduler;
    }

    public override void Execute()
    {
        ThreadExecuter<T> threadExecuter = 
            new ThreadExecuter<T>() { 
                Value = value, 
                Observers = observers,
                RepeatCount = repeatCount, 
                RepeatEndless = repeatEndless, 
                Scheduler = this.Scheduler};
        Scheduler.Schedule(threadExecuter.Execute);
    }
}    
}
