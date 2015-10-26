using System;
using System.Reactive.Disposables;

namespace RX_MVVM
{
    public interface IWhereFluidCommand<T>
    {
        void Where(IObservable<bool> observer);
    }

    public interface IDoFluidCommand<T> : IDisposable
    {
        IWhereFluidCommand<T> Do(Action<T> action);
    }

    public class FluidCommand<T> : IDoFluidCommand<T>, IWhereFluidCommand<T>
    {
        private readonly CompositeDisposable _compositeDisposable;
        private readonly ICommandObserver<T> _cmd;

        public FluidCommand(ICommandObserver<T> cmd)
        {
            _cmd = cmd;
            _compositeDisposable = new CompositeDisposable();
        }

        public IWhereFluidCommand<T> Do(Action<T> action)
        {
            _compositeDisposable.Add(_cmd.OnExecute.Subscribe(action));
            return this;
        }

        public void Where(IObservable<bool> observable)
        {
            _compositeDisposable.Add(observable.Subscribe(_cmd.SetCanExecute));
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }

}
