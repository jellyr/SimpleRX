using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRX
{    
    public class ParallelGenerateSubject : ParallelBaseSubject<int>
    {
        private int initValue;
        private Predicate<int> condition;
        private Func<int, int> iterate;
        private Func<int, int> resultSelector;        

        public ParallelGenerateSubject(int initValue, Predicate<int> condition, 
            Func<int, int> iterate, Func<int, int> resultSelector)
        {
            this.initValue = initValue;
            this.condition = condition;
            this.iterate = iterate;
            this.resultSelector = resultSelector;
            BuildScheduler();
        }

        public ParallelGenerateSubject(int initValue, Predicate<int> condition, 
            Func<int, int> iterate, Func<int, int> resultSelector, IScheduler scheduler) :
            this(initValue, condition, iterate, resultSelector)
        {
            this.Scheduler = scheduler;
        }

        public override void BuildScheduler()
        {
            if (base.ParallelExecutionMode_ == ParallelExecutionMode.ForceParallelism ||
                base.DegreeOfParallelism_ > 1)
                if (base.CancellationToken_.CanBeCanceled)
                    this.Scheduler = new TaskScheduler(base.CancellationToken_);
                else
                    this.Scheduler = new TaskScheduler();
            else if (base.CancellationToken_.CanBeCanceled)
                this.Scheduler = new TaskScheduler(base.CancellationToken_);
            else
                this.Scheduler = new CurrentThreadScheduler();
        }

        public class ThreadExecuter
        {
            public int Value { get; set; }
            public Predicate<int> Condition { get; set; }
            public Func<int, int> Iterate { get; set; }
            public Func<int, int> ResultSelector { get; set; }
            public List<IObserver<int>> Observers { get; set; }
            public IScheduler Scheduler { get; set; }

            public void Execute()
            {
                Action<int> action = null;
                action = (int value) =>
                {
                    if (Observers.Count <= 0)
                        return;
                    if (Condition(value))
                    {
                        var selectorValue = ResultSelector(value);
                        for (int i = 0; i < Observers.Count; i++)
                        {
                            Observers[i].OnNext(value);
                        }
                        var iterateValue = Iterate(value);
                        Scheduler.Schedule(action, iterateValue);
                    }
                    else
                    {
                        for (int i = 0; i < Observers.Count; i++)
                        {
                            Observers[i].OnCompleted();
                        }
                    }
                };
                action(Value);
            }
        }

        public override void Execute()
        {
            ThreadExecuter threadExecuter =
                new ThreadExecuter()
                {
                    Value = initValue,
                    Observers = observers,
                    Condition = condition,
                    Iterate = iterate,
                    ResultSelector = resultSelector,
                    Scheduler = this.Scheduler
                };
            Scheduler.Schedule(threadExecuter.Execute);            
        }
    }
}
