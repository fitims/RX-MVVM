using System.Reactive.Concurrency;

namespace RX_MVVM
{
    public interface ISchedulers
    {
        IScheduler Dispatcher { get; }
        IScheduler NewThread { get; }
        IScheduler NewTask { get; }
        IScheduler ThreadPool { get; }
        IScheduler Timer { get; }
    }

    public class Schedulers : ISchedulers
    {


        public IScheduler Dispatcher
        {
            get { return DispatcherScheduler.Current; }
        }

        public IScheduler NewThread
        {
            get { return NewThreadScheduler.Default; }
        }

        public IScheduler NewTask
        {
            get { return TaskPoolScheduler.Default; }
        }

        public IScheduler ThreadPool
        {
            get { return ThreadPoolScheduler.Instance; }
        }

        public IScheduler Timer
        {
            get { return ImmediateScheduler.Instance; }
        }
    }
}
