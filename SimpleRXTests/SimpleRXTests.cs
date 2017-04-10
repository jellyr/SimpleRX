using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleRX;

namespace SimpleRXTests
{
    [TestClass]
    public class SimpleRXTests
    {
        [TestMethod]
        public void AprioriSubjectTest()
        {
            List<string> t1 = new List<string>() { "i1", "i2", "i5" };
            List<string> t2 = new List<string>() { "i1", "i4" };
            List<string> t3 = new List<string>() { "i2", "i3" };
            List<string> t4 = new List<string>() { "i1", "i2", "i4" };
            List<string> t5 = new List<string>() { "i1", "i3" };
            List<string> t6 = new List<string>() { "i2", "i3" };
            List<string> t7 = new List<string>() { "i1", "i3" };
            List<string> t8 = new List<string>() { "i1", "i2", "i3", "i5" };
            List<string> t9 = new List<string>() { "i1", "i2", "i3" };
            List<string> t10 = new List<string>() { "i1", "i3", "i4", "i6" };
            List<string> t11 = new List<string>() { "i1", "i2", "i4", "i6" };
            List<string> t12 = new List<string>() { "i1", "i2", "i5", "i6" };
            List<List<string>> transactions = new List<List<string>>() { t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12 };
            AprioriSubject subject = new AprioriSubject(transactions);
            subject.Throttle(TimeSpan.FromSeconds(1)).Subscribe(x =>
                {
                    var y = x;
                    Debug.WriteLine(x);
                },
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted")
                );
            subject.OnNext("i1 i3");
            subject.OnCompleted();
            Console.ReadLine();
            
        }

        [TestMethod]
        public void LevenshteinSubjectTest()
        {
            List<string> list = new List<string>() { "lust", "last", "list", "lunge", "lamm", "lanze", "linz", "laus", "lang", "land", "laden" };
            LevenshteinSubject subject = new LevenshteinSubject(list);
            subject.Throttle(TimeSpan.FromSeconds(20)).Subscribe(x => 
                { 
                    var y = x;
                    Debug.WriteLine(x);
                },
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted")
                );
            subject.OnNext("laen");
            subject.OnCompleted();
            Console.ReadLine();
        }

        [TestMethod]
        public void ReturnTest()
        {
            IObservable<int> returnObject = Observable.Return(10);
            returnObject.Subscribe<int>(
                x => Assert.AreEqual(x, 10),
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
        }

        [TestMethod]
        public void RangeTest()
        {
            IObservable<int> rangeObject = Observable.Range(10,10);
            rangeObject.Subscribe<int>(
                x => Assert.AreEqual(x, 10),
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
        }

        [TestMethod]
        public void RepeatTest()
        {
            IObservable<int> repeatObject = Observable.Repeat(10);
            repeatObject.Subscribe<int>(
                x => Assert.AreEqual(x, 10),
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
        }

        [TestMethod]
        public void ThrowTest()
        {
            IObservable<int> throwObject = Observable.Throw<int>(new Exception("OnError"));
            throwObject.Subscribe<int>(
                x => { var y = x; },
                ex => Assert.AreEqual("OnError", ex.Message),
                () => Console.WriteLine("OnCompleted"));
        }

        [TestMethod]
        public void GenerateTest()
        {
            IParallelObservable<int> returnObject = 
                ParallelObservable.Generate(1, i => i <= 5, i => i + 1, i => i).
                WithExecutionMode(ParallelExecutionMode.ForceParallelism);                    
            returnObject.Subscribe<int>(
                x => Console.WriteLine(" {0} ", x),
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
        }

        [TestMethod]
        public void AllTest()
        {
            IObservable<bool> returnObject = Observable.Generate(1, i => i <= 5, i => i + 1, i => i).All(a => a > 2);
            returnObject.Subscribe<bool>(
                x => Assert.AreEqual(x, false),
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
        }

        [TestMethod]
        public void AmbTest()
        {
            IObservable<int> returnObject = Observable.Generate(1, i => i <= 5, i => i + 1, i => i).
                Amb(Observable.GenerateWithTime(1, i => i <= 5, i => i + 1, i => i, t => TimeSpan.FromSeconds((double)t)));
            returnObject.Subscribe<int>(
                x => { var y = x; },
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
        }

        [TestMethod]
        public void AverageTest()
        {
            IObservable<double> returnObject = Observable.Generate(1, i => i <= 5, i => i + 1, i => i).Average();
            returnObject.Subscribe<double>(
                x => { var y = x; },
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
        }

        [TestMethod]
        public void BufferTest()
        {
            IObservable<IList<int>> returnObject = Observable.Generate(1, i => i <= 5, i => i + 1, i => i).Buffer(2);
            returnObject.Subscribe<IList<int>>(
                x => { var y = x; },
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
        }

        [TestMethod]
        public void CatchTest()
        {
            IObservable<int> returnObject = Observable.Throw<int>(new Exception()).Catch(Observable.Return(10));
            returnObject.Subscribe<int>(
                x => { var y = x; },
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
            Console.ReadLine();
        }

        [TestMethod]
        public void CastTest()
        {
            object obj = (object)2.2;
            IObservable<double> returnObject = Observable.Return(obj).Cast<double>();
            returnObject.Subscribe<double>(
                x => { var y = x; },
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
        }

        [TestMethod]
        public void ConcateTest()
        {
            IObservable<int> returnObject = Observable.Range(1, 4).Concat(Observable.Range(5, 5));
            returnObject.Subscribe<int>(
                x => { var y = x; },
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
        }

        [TestMethod]
        public void ContainsTest()
        {
            IObservable<bool> returnObject = Observable.Range(1, 4).Contains(2);
            returnObject.Subscribe<bool>(
                x => Assert.IsTrue(x),
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
        }

        [TestMethod]
        public void CountTest()
        {
            IObservable<int> returnObject = Observable.Range(1, 4).Count();
            returnObject.Subscribe<int>(
                x => Assert.AreEqual<int>(x, 4),
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
        }

        [TestMethod]
        public void DelayTest()
        {
            IObservable<int> returnObject = Observable.Range(1, 4).Delay(TimeSpan.FromSeconds(5));
            returnObject.Subscribe<int>(
                x => { var y = x; },
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
            Console.ReadLine();
        }

        [TestMethod]
        public void DistinctTest()
        {
            IObservable<int> returnObject = Observable.Generate(5, i => i > 0, i => i - 1, i => i).Concat(Observable.Return(6)).Distinct();
            returnObject.Subscribe<int>(
                x => { var y = x; },
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
            Console.ReadLine();
        }

        [TestMethod]
        public void DistinctUntilChangedTest()
        {
            IObservable<int> returnObject = Observable.Generate(5, i => i > 0, i => i - 1, i => i).Concat(Observable.Return(5)).DistinctUntilChanged();
            returnObject.Subscribe<int>(
                x => { var y = x; },
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
            Console.ReadLine();
        }

        [TestMethod]
        public void FinallyTest()
        {
            IObservable<int> returnObject = Observable.Range(1, 4).Finally(() => { Console.WriteLine(); });
            returnObject.Subscribe<int>(
                x => { var y = x; },
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
            Console.ReadLine();
        }

        [TestMethod]
        public void TrasformTest()
        {
            IObservable<int> returnObject = Observable.Range(1, 4).Transform(i => i * 2);
            returnObject.Subscribe<int>(
                x => { var y = x; },
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
            Console.ReadLine();
        }

        [TestMethod]
        public void ScanTest()
        {
            IObservable<int> returnObject = Observable.Range(1, 4).Transform(i => i * 2);
            returnObject.Subscribe<int>(
                x => { var y = x; },
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
            Console.ReadLine();
        }

        [TestMethod]
        public void SelectTest()
        {
            IObservable<int> returnObject = Observable.Range(1, 4).Transform(i => i * 2);
            returnObject.Subscribe<int>(
                x => { var y = x; },
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
            Console.ReadLine();
        }

        [TestMethod]
        public void SelectManyTest()
        {
            IObservable<int> returnObject = Observable.Range(1, 4).Transform(i => i * 2);
            returnObject.Subscribe<int>(
                x => { var y = x; },
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
            Console.ReadLine();
        }

        [TestMethod]
        public void SubjectTest()
        {
            Subject<int> subject = new Subject<int>();
            subject.Min().Subscribe<int>(x => { var y = x; },
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted")
                );
            subject.OnNext(10);
            subject.OnNext(20);
            subject.OnCompleted();
            Console.ReadLine();
        }

        //http://qconlondon.com/dl/qcon-london-2011/slides/BartDeSmet_RxYourPrescriptionToCureAsynchronousProgrammingBlues.pdf
        //http://msdn.microsoft.com/en-us/data/gg577609
        [TestMethod]
        public void ObservableReturnTest()
        {
            IObservable<int> returnObject = Observable.Return<int>(42);
            using (returnObject.Subscribe<int>(
                x => Assert.AreEqual(x, 42),
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted")))
            {


            }
            //IObserver<int> observer = new Observer<int>(x => Console.WriteLine(x), 
            //    ex => Console.WriteLine(ex.Message), () => Console.WriteLine("Completed"));
            //IDisposable subscription = returnObject.Subscribe(observer);
        }

        [TestMethod]
        public void ObservableObserveOnReturnTest()
        {
            IObservable<int> returnObject = Observable.Return<int>(42).ObserveOn(new ThreadScheduler());
            returnObject.Subscribe<int>(
                x => Assert.AreEqual(x, 42),
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
            //Console.ReadLine();   
        }

        [TestMethod]
        public void StartWithTest()
        {
            IObservable<int> returnObject = Observable.Generate(0, i => i < 10, i => i + 1, i => i).StartWith(20, 21, 22, 23, 24);
            returnObject.Subscribe<int>(
                x => { Trace.WriteLine(x); },
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
            //Console.ReadLine();
        }

        [TestMethod]
        public void AggregateTest()
        {
            IObservable<int> generateObject = Observable.Generate(0, i => i < 10, i => i + 1, i => i).Aggregate((x, y) => x + y);
            generateObject.Subscribe<int>(
                x => { var y = x; },
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
        }

        class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public int Kinder { get; set; }
        }

        [TestMethod]
        public void GroupByTest()
        {
            List<Person> list = new List<Person>() { 
                new Person() { Name = "Mathias", Age = 35, Kinder = 2 }, 
                new Person() { Name = "Karoline", Age = 35, Kinder = 2 }, 
                new Person() { Name = "Ursula", Age = 35 }, 
                new Person() { Name = "Emil", Age = 3}, 
                new Person() { Name = "Anna", Age = 32, Kinder = 2 } };
            var generateObject = list.ToObservable().GroupBy(p => p.Kinder);
            generateObject.Subscribe(
                x =>
                {
                    if (x.Key == 2)
                        x.Subscribe<Person, int>(i => { Trace.WriteLine(string.Format("{0} - {1}", i.Age, i.Name)); });
                    //if(x.Key == 35)
                    //    x.Subscribe<Person, int>(i => { Trace.WriteLine(string.Format("{0} - {1}", i.Age, i.Name)); });
                },
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
        }

        [TestMethod]
        public void CreateTest()
        {
            var generatedObject = Observable.Create<int>(o =>
            {
                o.OnNext(10);
                o.OnNext(20);
                return () => { };
            });
            IDisposable disposable = generatedObject.Subscribe(
                x =>
                {
                    var y = x;
                },
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
            disposable.Dispose();
        }

        [TestMethod]
        public void GenerateSimpleTest()
        {
            IObservable<int> generateObject = Observable.Generate(1, i => i <= 10, i => i + 1, i => i);
            generateObject.Subscribe<int>(
                x => Trace.WriteLine(x),
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
        }

        [TestMethod]
        public void ObservableRangeTest()
        {
            IObservable<int> generateObject = Observable.Range(2, 10).Min();
            generateObject.Subscribe<int>(
                x => { var y = x; },
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));

            generateObject.Subscribe<int>(x => { var y = x; },
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
        }

        [TestMethod]
        public void ObservableMinTest()
        {
            IObservable<int> generateObject = Observable.Generate(0, i => i < 2, i => i + 1, i => i).Min();
            generateObject.Subscribe<int>(
                x => { var y = x; },
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
        }

        [TestMethod]
        public void ObservableAverageTest()
        {
            IObservable<double> generateObject = Observable.Generate(0, i => i < 5, i => i + 1, i => i).Average();
            generateObject.Subscribe<double>(
                x => { var y = x; },
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
        }

        [TestMethod]
        public void GenerateWithTimeAndTimestampTest()
        {
            IObservable<Timestamped<int>> generateObject =
                Observable.GenerateWithTime(1, i => i <= 10, i => i + 1, i => i, i => TimeSpan.FromSeconds(i)).Timestamp();
            generateObject.Subscribe(
                x => Trace.WriteLine(string.Format("{0} / {1}",x.Value, x.Timestamp)),
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
            Console.ReadLine();
        }

        [TestMethod]
        public void GenerateWithTimeTest()
        {
            IObservable<int> generateObject =
                Observable.GenerateWithTime(1, i => i <= 10, i => i + 1, i => i, i => TimeSpan.FromSeconds(i));
                    generateObject.Subscribe(
                    x => Trace.WriteLine(string.Format("{0} / {1}", x, DateTime.Now.Second)),
                    ex => Console.WriteLine("OnError {0}", ex.Message),
                    () => Console.WriteLine("OnCompleted"));
            Console.ReadLine();
        }

        [TestMethod]
        public void ObservableOnReturnTest()
        {
            IObservable<double> generateObject = Observable.Return(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }).Average().ObserveOn(new ThreadScheduler());
            generateObject.Subscribe(
                x => { var y = x; },
                ex => Console.WriteLine("OnError {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
            Console.ReadLine();
        }
    }
}
