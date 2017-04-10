using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleRX
{
    public static class ParallelObservable
    {
        public static IParallelObservable<int> Generate(int initValue, Predicate<int> condition,
            Func<int, int> iterate, Func<int, int> resultSelector)
        {
            return new ParallelGenerateSubject(initValue, condition, iterate, resultSelector);
        }

        //public static IParallelObservable<T> Empty<T>()
        //{
        //    return new EmptySubject<T>();
        //}

        //public static IParallelObservable<T> Return<T>(T p)
        //{
        //    return new ReturnSubject<T>(p);
        //}

        //public static IParallelObservable<T> Return<T>(T[] p)
        //{
        //    return new ReturnSubjectArray<T>(p);
        //}

        //public static IParallelObservable<T> Return<T>(IEnumerable<T> collection)
        //{
        //    return new ReturnSubjectCollection<T>(collection);
        //}

        //public static IParallelObservable<int> Range(int p, int xtime)
        //{
        //    return new RangeSubject(p, xtime);
        //}

        //public static IParallelObservable<int> Range(int p, int xtime, IScheduler scheduler)
        //{
        //    return new RangeSubject(p, xtime, scheduler);
        //}

        //public static IParallelObservable<T> Repeat<T>(T value)
        //{
        //    return new RepeatSubject<T>(value);
        //}

        //public static IParallelObservable<T> Repeat<T>(T value, IScheduler scheduler)
        //{
        //    return new RepeatSubject<T>(value, scheduler);
        //}

        //public static IParallelObservable<T> Repeat<T>(T value, int repeatCount)
        //{
        //    return new RepeatSubject<T>(value, repeatCount);
        //}

        //public static IParallelObservable<T> Repeat<T>(T value, int repeatCount, IScheduler scheduler)
        //{
        //    return new RepeatSubject<T>(value, repeatCount, scheduler);
        //}
    }
}
