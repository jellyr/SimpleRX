using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleRX
{
    //http://msdn.microsoft.com/en-us/library/hh212048(v=vs.103).aspx
    public static class Observable
    {
        public static IObservable<T> Empty<T>()
        {
            return new EmptySubject<T>();
        }

        public static IObservable<T> Return<T>(T p)
        {
            return new ReturnSubject<T>(p);
        }

        public static IObservable<T> Return<T>(T[] p)
        {
            return new ReturnSubjectArray<T>(p);
        }    

        public static IObservable<T> Return<T>(IEnumerable<T> collection)
        {
            return new ReturnSubjectCollection<T>(collection);
        }

        public static IObservable<int> Range(int p, int xtime)
        {
            return new RangeSubject(p, xtime);
        }

        public static IObservable<int> Range(int p, int xtime, IScheduler scheduler)
        {
            return new RangeSubject(p, xtime, scheduler);
        }

        public static IObservable<T> Repeat<T>(T value)
        {
            return new RepeatSubject<T>(value);
        }

        public static IObservable<T> Repeat<T>(T value, IScheduler scheduler)
        {
            return new RepeatSubject<T>(value, scheduler);
        }

        public static IObservable<T> Repeat<T>(T value, int repeatCount)
        {
            return new RepeatSubject<T>(value, repeatCount);
        }

        public static IObservable<T> Repeat<T>(T value, int repeatCount, IScheduler scheduler)
        {
            return new RepeatSubject<T>(value, repeatCount, scheduler);
        }

        public static IObservable<T> Throw<T>(Exception exception)
        {
            return new ThrowSubject<T>(exception);
        }

        public static IObservable<T> Never<T>()
        {
            return new NeverSubject<T>();
        }

        //public static IObservable<T> ObserveOn<T>(this IObservable<T> source)
        //{
        //    return new ObserveOnSubject<T>(source);
        //}

        public static IObservable<T> ObserveOn<T>(this IObservable<T> source, IScheduler scheduler)
        {
            return new ObserveOnSubject<T>(source, scheduler);
        }

        public static IObservable<int> Min(this IObservable<int> source)
        {
            return new MinSubjectInt(source);
        }

        public static IObservable<double> Min(this IObservable<double> source)
        {
            return new MinSubjectDouble(source);
        }

        public static IObservable<T> Distinct<T>(this IObservable<T> source)
        {
            return new DistinctSubject<T>(source);
        }

        public static IObservable<T> ElementAt<T>(this IObservable<T> source, int index)
        {
            return new ElementAtSubject<T>(source, index);
        }

        public static IObservable<int> Generate(int initValue, Predicate<int> condition, 
            Func<int, int> iterate, Func<int, int> resultSelector)
        {
            return new GenerateSubject(initValue, condition, iterate, resultSelector);
        }

        public static IObservable<int> Generate(int initValue, Predicate<int> condition,
            Func<int, int> iterate, Func<int, int> resultSelector, IScheduler scheduler)
        {
            return new GenerateSubject(initValue, condition, iterate, resultSelector, scheduler);
        }

        public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, int count)
        {
            var bufferSubject = new BufferSubject<IList<T>, T>(count);
            bufferSubject.Start(source);
            return bufferSubject;
        }

        public static IObservable<T> Delay<T>(this IObservable<T> source, TimeSpan dueTime)
        {
            return new DelaySubject<T>(source, dueTime);
        }

        public static IObservable<T> Catch<T>(this IObservable<T> firstsource, IObservable<T> secondsource)
        {
            return new CatchSubject<T>(firstsource, secondsource);
        }

        public static IObservable<T> Finally<T>(this IObservable<T> source, Action finallyAction)
        {
            return new FinallySubject<T>(source, finallyAction);
        }

        public static IObservable<T> Amb<T>(this IObservable<T> first, IObservable<T> second)
        {
            return new AmbSubject<T>(first, second);
        }        

        public static IObservable<T> Amb<T>(params IObservable<T>[] sources)
        {
            return new AmbSubjectArray<T>(sources);
        }

        public static IObservable<bool> Any<T>(this IObservable<T> source)
        {
            return new AnySubject<T>(source);
        }

        public static IObservable<double> Average(this IObservable<int> source)
        {
            return new AverageSubject<int>(source);
        }

        public static IObservable<bool> All<T>(this IObservable<T> source, Func<T, bool> predicate)
        {
            return new AllSubject<T>(source, predicate);
        }

        public static IObservable<T> Aggregate<T>(this IObservable<T> source, Func<T, T, T> accumulator)
        {
            return new AggregateSubject<T>(source, accumulator);
        }

        public static IObservable<T> Transform<T>(this IObservable<T> source, Func<T, T> transfrom)
        {
            return new TransformSubject<T>(source, transfrom);
        }

        public static IObservable<TResult> Cast<TResult>(this IObservable<object> source)
        {
            return new CastSubject<TResult>(source);
        }

        public static IObservable<T> Create<T>(Func<IObserver<T>, Action> subscribe)
        {
            return new CreateSubject<T>(subscribe);
        }

        public static IObservable<T> Concat<T>(this IObservable<T> firstsource, IObservable<T> secondsource)
        {
            return new ConcatSubject<T>(firstsource, secondsource);
        }

        //public static IObservable<T> Concat<T>(this IObservable<T> firstsource, params IObservable<T> additionalsources)
        //{

        //}

        public static IObservable<T> StartWith<T>(this IObservable<T> source, params T[] values)
        {
            return new StartWithSubject<T>(source, values);
        }

        public static IObservable<T> Scan<T>(this IObservable<T> source, Func<T, T, T> accumulator)
        {
            return new ScanSubject<T>(source, accumulator);
        }

        public static IObservable<T> Scan<T>(this IObservable<T> source, T seed, Func<T, T, T> accumulator)
        {
            return new ScanSubject<T>(source, seed, accumulator);
        }

        public static IObservable<TResult> Select<T, TResult>(this IObservable<T> source, Func<T, TResult> selector)
        {
            return new SelectSubject<T, TResult>(source, selector);
        }

        public static IObservable<ILookup<TKey, T>> ToLookup<T, TKey>(this IObservable<T> source, Func<T, TKey> keySelector)
        {
            return new LookupSubject<T, TKey>(source, keySelector);
        }

        public static IObservable<TResult> SelectMany<T, TResult>(this IObservable<T> source, Func<T, IEnumerable<TResult>> selector)
        {
            return new SelectManySubject<T, TResult>(source, selector);
        }

        public static IObservable<IGroupedObservable<T, TKey>> GroupBy<T, TKey>(this IObservable<T> source,
	        Func<T, TKey> keySelector)
        {
            return new GroupBySubject<T, TKey>(source, keySelector);
        }

        public static IObservable<T> Take<T>(this IObservable<T> source, int count)
        {
            return new TakeSubject<T>(source, count);
        }

        public static IObservable<T> Skip<T>(this IObservable<T> source, int count)
        {
            return new SkipSubject<T>(source, count);
        }

        public static IObservable<bool> Contains<T>(this IObservable<T> source, T value)
        {
            return new ContainsSubject<T>(source, value);
        }

        public static IObservable<int> Count<T>(this IObservable<T> source)
        {
            return new CountSubject<T>(source);
        }

        public static T First<T>(this IObservable<T> source)
        {
            var firstSubject = new FirstSubject<T>(source);
            return firstSubject.First;
        }

        public static T FirstOrDefault<T>(this IObservable<T> source)
        {
            var firstOrDefaultSubject = new FirstOrDefaultSubject<T>(source);
            return firstOrDefaultSubject.First;
        }

        public static T Last<T>(this IObservable<T> source)
        {
            var lastSubject = new LastSubject<T>(source);
            return lastSubject.Last;
        }

        public static T LastOrDefault<T>(this IObservable<T> source)
        {
            var lastOrDefaultSubject = new LastOrDefaultSubject<T>(source);
            return lastOrDefaultSubject.Last;
        }

        public static IObservable<TResult> Zip<TFirst, TSecond, TResult>(this IObservable<TFirst> first, 
            IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            return new ZipSubject<TFirst, TSecond, TResult>(first, second, resultSelector);
        }

        public static IObservable<T> Do<T>(this IObservable<T> source, Action<T> onNext)
        {
            return new DoSubject<T>(source, onNext);
        }

        public static void ForEach<T>(this IObservable<T> source, Action<T> onNext)
        {
            new ForeachSubject<T>(source, onNext);            
        }

        public static IObservable<T> Timer<T>(TimeSpan dueTime, TimeSpan period)
        {
            return new TimerSubject<T>(dueTime, period);
        }

        public static IObservable<int> GenerateWithTime(int initValue, 
            Predicate<int> condition, Func<int, int> iterate, 
            Func<int, int> resultSelector, Func<int, TimeSpan> timeSelector)
        {
            return new GenerateWithTimeSubject(initValue, condition, iterate, resultSelector, timeSelector);
        }
       
        public static IObservable<T> Throttle<T>(this IObservable<T> source, TimeSpan dueTime)
        {
            return new ThrottleSubject<T>(source, dueTime);
        }

        public static IObservable<T> DistinctUntilChanged<T>(this IObservable<T> source)
        {
            return new DistinctUntilChangedSubject<T>(source);
        }

        public static IObservable<Timestamped<T>> Timestamp<T>(this IObservable<T> source)
        {
            return new TimestampedSubject<T>(source);
        }

        public static IObservable<T> FromEvent<T>(object control, string eventName)
        {
            return new FromEventSubject<T>(control, eventName);
        }

        public static IObservable<TEventArgs> FromEvent<TEventArgs>(Action<Action<TEventArgs>> addHandler, Action<Action<TEventArgs>> removeHandler)
        {
            return new FromEventActionSubject<TEventArgs>(addHandler, removeHandler);
        }

        //http://msdn.microsoft.com/en-us/library/hh229241(v=vs.103).aspx
        //http://minirx.codeplex.com/
        public static IObservable<TEventArgs> FromEvent<TEventArgs, TDelegate>(Func<Action<TEventArgs>, TDelegate> conversion, 
            Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            return new FromEventFuncSubject<TDelegate, TEventArgs>(conversion, addHandler, removeHandler);
        }

        //http://msdn.microsoft.com/en-us/library/hh229271(v=vs.103).aspx
        public static IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            return new FromEventFuncGenericSubject<TDelegate, TEventArgs>(addHandler, removeHandler);
        }

        ////http://msdn.microsoft.com/en-us/library/hh229271(v=vs.103).aspx
        public static IObservable<EventArgs> FromEvent(Action<EventHandler> addHandler, Action<EventHandler> removeHandler)
        {
            return new FromEventFuncEventHandlerSubject(addHandler, removeHandler);
        }

        public async static Task<IObservable<T>> FromAsync<T>(Func<CancellationToken, T> func)
        {
            TaskFactory taskFactory = new TaskFactory();
            Task<IObservable<T>> task = taskFactory.StartNew(() => 
                { 
                    return (IObservable<T>) new FromAsyncSubject<T>(func); 
                });
            await task;
            return task.Result;
        }

        public static IObservable<T> Where<T>(this IObservable<T> source, Predicate<T> predicate)
        {
            return new WhereSubject<T>(source, predicate);
        }

        public static Func<T1, T2, T3, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, TResult>(
            Func<T1, T2, T3, AsyncCallback, Object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            var subject = new FromAsyncPatternSubject<T1, T2, T3, TResult>(begin, end);
            return subject.GenerateFunc();
        }

        public static Func<T1, T2, IObservable<TResult>> FromAsyncPattern<T1, T2, TResult>(
            Func<T1, T2, AsyncCallback, Object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            var subject = new FromAsyncPatternSubject<T1, T2, TResult>(begin, end);
            return subject.GenerateFunc();
        }

        public static Func<T1, IObservable<TResult>> FromAsyncPattern<T1, TResult>(
            Func<T1, AsyncCallback, Object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            var subject = new FromAsyncPatternSubject<T1, TResult>(begin, end);
            return subject.GenerateFunc();
        }

        public static Func<IObservable<TResult>> FromAsyncPattern<T1, TResult>(
            Func<AsyncCallback, Object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            var subject = new FromAsyncPatternSubject<TResult>(begin, end);
            return subject.GenerateFunc();
        }
    }
}
