using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleRX
{
    public static class ParallelObservableExtensions
    {
        //public static IParallelObservable<TSource> AsParallel<TSource>(
        //    this IObserver<TSource> source)
        //{
        //    ParallelObserver<TSource> parallelObserver =
        //        new ParallelObserver<TSource>(source);
        //    return parallelObserver;
        //}

        public static IParallelObservable<TSource> WithExecutionMode<TSource>(
            this IParallelObservable<TSource> source,
            ParallelExecutionMode executionMode)
        {
            ParallelBaseSubject<TSource> parallelsubject = (ParallelBaseSubject<TSource>)source;
            parallelsubject.ParallelExecutionMode_ = executionMode;
            parallelsubject.BuildScheduler();
            return parallelsubject;
        }

        public static IParallelObservable<TSource> WithDegreeOfParallelism<TSource>(
            this IParallelObservable<TSource> source,
            int degreeOfParallelism)
        {
            ParallelBaseSubject<TSource> parallelsubject = (ParallelBaseSubject<TSource>)source;
            parallelsubject.DegreeOfParallelism_ = degreeOfParallelism;
            parallelsubject.BuildScheduler();
            return parallelsubject;            
        }

        public static IParallelObservable<TSource> WithMergeOptions<TSource>(
            this IParallelObservable<TSource> source,
            ParallelMergeOptions mergeOptions
        )
        {
            ParallelBaseSubject<TSource> parallelsubject = (ParallelBaseSubject<TSource>)source;
            parallelsubject.ParallelMergeOptions_ = mergeOptions;
            parallelsubject.BuildScheduler();
            return parallelsubject;                  
        }

        public static IParallelObservable<TSource> WithCancellation<TSource>(
            this IParallelObservable<TSource> source,
            CancellationToken cancellationToken
        )
        {
            ParallelBaseSubject<TSource> parallelsubject = (ParallelBaseSubject<TSource>)source;
            parallelsubject.CancellationToken_ = cancellationToken;
            parallelsubject.BuildScheduler();
            return parallelsubject;                      
        }

        public static IDisposable Subscribe<T>(this IParallelObservable<T> value, Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            ParallelObserver<T> observer = new ParallelObserver<T>(onNext, onError, onCompleted);
            IDisposable subject = value.Subscribe(observer);
            return subject;
        }

        public static IDisposable Subscribe<T>(this IParallelObservable<T> value, Action<T> onNext, Action<Exception> onError)
        {
            ParallelObserver<T> observer = new ParallelObserver<T>(onNext, onError);
            IDisposable subject = value.Subscribe(observer);
            return subject;
        }

        public static IDisposable Subscribe<T>(this IParallelObservable<T> value, Action<T> onNext)
        {
            ParallelObserver<T> observer = new ParallelObserver<T>(onNext);
            IDisposable subject = value.Subscribe(observer);
            return subject;
        }

        //public static IDisposable Subscribe<T, TKey>(this IGroupedObservable<T, TKey> value, Action<T> onNext, Action<Exception> onError, Action onCompleted)
        //{
        //    Observer<T> observer = new Observer<T>(onNext, onError, onCompleted);
        //    IDisposable subject = value.Subscribe(observer);
        //    return subject;
        //}

        //public static IDisposable Subscribe<T, TKey>(this IGroupedObservable<T, TKey> value, Action<T> onNext, Action<Exception> onError)
        //{
        //    Observer<T> observer = new Observer<T>(onNext, onError);
        //    IDisposable subject = value.Subscribe(observer);
        //    return subject;
        //}

        //public static IDisposable Subscribe<T, TKey>(this IGroupedObservable<T, TKey> value, Action<T> onNext)
        //{
        //    Observer<T> observer = new Observer<T>(onNext);
        //    IDisposable subject = value.Subscribe(observer);
        //    return subject;
        //}    
    }
}
