using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRX
{
    public static class ObservableExtensions
    {
        public static IObservable<T> ToObservable<T>(this T[] array)
        {
            return new ReturnSubjectArray<T>(array);
        }

        public static IObservable<T> ToObservable<T>(this IEnumerable<T> collection)
        {
            return new ReturnSubjectCollection<T>(collection);
        }

        public static IObservable<T> ToObservable<T>(this List<T> collection)
        {
            return new ReturnSubjectCollection<T>((IEnumerable<T>)collection);
        }

        public static IObservable<T> ToObservable<T>(this T value)
        {
            return new ReturnSubject<T>(value);
        }

        public async static Task<IObservable<T>> ToObservableAsync<T>(this T value)
        {
            TaskFactory taskFactory = new TaskFactory();
            Task<IObservable<T>> task = taskFactory.StartNew(() =>
            {
                return (IObservable<T>)new ReturnSubject<T>(value);
            });
            await task;
            return task.Result;
            
        }

        public async static Task<IObservable<T>> ToObservableAsync<T>(this T[] array)
        {
            TaskFactory taskFactory = new TaskFactory();
            Task<IObservable<T>> task = taskFactory.StartNew(() =>
            {
                return (IObservable<T>) new ReturnSubjectArray<T>(array);
            });
            await task;
            return task.Result;
        }

        public static IDisposable Subscribe<T>(this IObservable<T> value, Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            Observer<T> observer = new Observer<T>(onNext, onError, onCompleted);
            IDisposable subject = value.Subscribe(observer);
            return subject;
        }

        public static IDisposable Subscribe<T>(this IObservable<T> value, Action<T> onNext, Action<Exception> onError)
        {
            Observer<T> observer = new Observer<T>(onNext, onError);
            IDisposable subject = value.Subscribe(observer);
            return subject;
        }       

        public static IDisposable Subscribe<T>(this IObservable<T> value, Action<T> onNext)
        {
            Observer<T> observer = new Observer<T>(onNext);
            IDisposable subject = value.Subscribe(observer);
            return subject;
        }

        public static IDisposable Subscribe<T, TKey>(this IGroupedObservable<T, TKey> value, Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            Observer<T> observer = new Observer<T>(onNext, onError, onCompleted);
            IDisposable subject = value.Subscribe(observer);
            return subject;
        }

        public static IDisposable Subscribe<T, TKey>(this IGroupedObservable<T, TKey> value, Action<T> onNext, Action<Exception> onError)
        {
            Observer<T> observer = new Observer<T>(onNext, onError);
            IDisposable subject = value.Subscribe(observer);
            return subject;
        }       

        public static IDisposable Subscribe<T, TKey>(this IGroupedObservable<T, TKey> value, Action<T> onNext)
        {
            Observer<T> observer = new Observer<T>(onNext);
            IDisposable subject = value.Subscribe(observer);
            return subject;
        }       
    }
}
