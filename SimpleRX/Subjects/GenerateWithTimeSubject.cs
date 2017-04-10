using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SimpleRX
{
public class GenerateWithTimeSubject : BaseSubject<int>
{
    private int initValue;
    private Predicate<int> condition;
    private Func<int, int> iterate;
    private Func<int, int> resultSelector;
    private Func<int, TimeSpan> timeSelector;
    private Timer timer;

    public GenerateWithTimeSubject(int initValue, Predicate<int> condition, Func<int, int> iterate,
        Func<int, int> resultSelector, Func<int, TimeSpan> timeSelector)
    {
        this.initValue = initValue;
        this.condition = condition;
        this.iterate = iterate;
        this.resultSelector = resultSelector;
        this.timeSelector = timeSelector;
    }

    public class ThreadExecuter
    {
        public int Value { get; set; }
        public Predicate<int> Condition { get; set; }
        public Func<int, int> Iterate { get; set; }
        public Func<int, int> ResultSelector { get; set; }
        public Func<int, TimeSpan> TimespanSelector { get; set; }
        public List<IObserver<int>> Observers { get; set; }
        private Timer timer;
        public IScheduler Scheduler { get; set; }

        private AutoResetEvent autoResetEvent = new AutoResetEvent(false);

        public void Execute()
        {
            Action<int> action = null;
            action = (int value) =>
            {
                if (Observers.Count <= 0)
                    return;
                var tempInitValue = value;
                var timeSpan = TimespanSelector(tempInitValue);
                if (Condition(value))
                {
                    timer = new Timer(obj =>
                    {
                        int localInitValue = (int)obj;
                        var selectorValue = ResultSelector(localInitValue);
                        for (int i = 0; i < Observers.Count; i++)
                        {
                            Observers[i].OnNext(selectorValue);
                        }
                        autoResetEvent.Set();
                    }, tempInitValue, timeSpan, new TimeSpan(-1));                        
                    autoResetEvent.WaitOne();
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

    //private AutoResetEvent autoResetEvent = new AutoResetEvent(false);

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
                TimespanSelector = timeSelector,
                Scheduler = this.Scheduler
            };
        Scheduler.Schedule(threadExecuter.Execute);
    }
}
}
