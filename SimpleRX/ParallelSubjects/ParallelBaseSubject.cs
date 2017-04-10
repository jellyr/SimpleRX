using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleRX
{
    public abstract class ParallelBaseSubject<T> : BaseSubject<T>, IParallelObservable<T> 
    {
        private ParallelExecutionMode parallelExecutionMode_;

        public ParallelExecutionMode ParallelExecutionMode_
        {
            get { return parallelExecutionMode_; }
            set { parallelExecutionMode_ = value; }
        }
        private int degreeOfParallelism_;

        public int DegreeOfParallelism_
        {
            get { return degreeOfParallelism_; }
            set { degreeOfParallelism_ = value; }
        }
        private ParallelMergeOptions parallelMergeOptions_;

        public ParallelMergeOptions ParallelMergeOptions_
        {
            get { return parallelMergeOptions_; }
            set { parallelMergeOptions_ = value; }
        }
        private CancellationToken cancellationToken_;

        public CancellationToken CancellationToken_
        {
            get { return cancellationToken_; }
            set { cancellationToken_ = value; }
        }

        public abstract void BuildScheduler();
    }
}
