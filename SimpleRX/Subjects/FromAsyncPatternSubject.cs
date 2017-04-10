using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class FromAsyncPatternSubject<T1, T2, T3, TResult> : BaseSubject<TResult>
    {
        private Func<T1, T2, T3, AsyncCallback, Object, IAsyncResult> begin;
        private Func<IAsyncResult, TResult> end;
        private List<TResult> results = new List<TResult>();
        private bool resultArrived = false;

        public FromAsyncPatternSubject(Func<T1, T2, T3, AsyncCallback, Object, IAsyncResult> begin,
            Func<IAsyncResult, TResult> end)
        {
            this.begin = begin;
            this.end = end;
        }

        public Func<T1, T2, T3, IObservable<TResult>> GenerateFunc()
        {
            return new Func<T1, T2, T3, IObservable<TResult>>((x, y, z) =>
            {
                IAsyncResult result = begin(x, y, z, iar =>
                {
                    TResult innerResult = end(iar);
                    results.Add(innerResult);
                    resultArrived = true;
                    Execute();
                }, null);
                return this;
            });
        }

        public override void Execute()
        {
            if (resultArrived)
            {
                foreach (var item in observers)
                {
                    foreach (var result in results)
                    {
                        item.OnNext(result);
                    }
                    item.OnCompleted();
                }
            }
        }
    }

    public class FromAsyncPatternSubject<T1, T2, TResult> : BaseSubject<TResult>
    {
        private Func<T1, T2, AsyncCallback, Object, IAsyncResult> begin;
        private Func<IAsyncResult, TResult> end;
        private List<TResult> results = new List<TResult>();
        private bool resultArrived = false;

        public FromAsyncPatternSubject(Func<T1, T2, AsyncCallback, Object, IAsyncResult> begin,
            Func<IAsyncResult, TResult> end)
        {
            this.begin = begin;
            this.end = end;
        }

        public Func<T1, T2, IObservable<TResult>> GenerateFunc()
        {
            return new Func<T1, T2, IObservable<TResult>>((a, b) =>
            {
                IAsyncResult result = begin(a, b, iar =>
                {
                    TResult innerResult = end(iar);
                    results.Add(innerResult);
                    resultArrived = true;
                    Execute();
                }, null);
                return this;
            });
        }

        public override void Execute()
        {
            if (resultArrived)
            {
                foreach (var item in observers)
                {
                    foreach (var result in results)
                    {
                        item.OnNext(result);
                    }
                    item.OnCompleted();
                }
            }
        }
    }

    public class FromAsyncPatternSubject<T1, TResult> : BaseSubject<TResult>
    {
        private Func<T1, AsyncCallback, Object, IAsyncResult> begin;
        private Func<IAsyncResult, TResult> end;
        private List<TResult> results = new List<TResult>();
        private bool resultArrived = false;

        public FromAsyncPatternSubject(Func<T1, AsyncCallback, Object, IAsyncResult> begin,
            Func<IAsyncResult, TResult> end)
        {
            this.begin = begin;
            this.end = end;
        }

        public Func<T1, IObservable<TResult>> GenerateFunc()
        {
            return new Func<T1, IObservable<TResult>>(a =>
            {
                IAsyncResult result = begin(a, iar =>
                {
                    TResult innerResult = end(iar);
                    results.Add(innerResult);
                    resultArrived = true;
                    Execute();
                }, null);
                return this;
            });
        }

        public override void Execute()
        {
            if (resultArrived)
            {
                foreach (var item in observers)
                {
                    foreach (var result in results)
                    {
                        item.OnNext(result);
                    }
                    item.OnCompleted();
                }
            }
        }
    }

    public class FromAsyncPatternSubject<TResult> : BaseSubject<TResult>
    {
        private Func<AsyncCallback, Object, IAsyncResult> begin;
        private Func<IAsyncResult, TResult> end;
        private List<TResult> results = new List<TResult>();
        private bool resultArrived = false;

        public FromAsyncPatternSubject(Func<AsyncCallback, Object, IAsyncResult> begin,
            Func<IAsyncResult, TResult> end)
        {
            this.begin = begin;
            this.end = end;
        }

        public Func<IObservable<TResult>> GenerateFunc()
        {
            return new Func<IObservable<TResult>>(() =>
            {
                IAsyncResult result = begin(iar =>
                {
                    TResult innerResult = end(iar);
                    results.Add(innerResult);
                    resultArrived = true;
                    Execute();
                }, null);
                return this;
            });
        }

        public override void Execute()
        {
            if (resultArrived)
            {
                foreach (var item in observers)
                {
                    foreach (var result in results)
                    {
                        item.OnNext(result);
                    }
                    item.OnCompleted();
                }
            }
        }
    }
}
