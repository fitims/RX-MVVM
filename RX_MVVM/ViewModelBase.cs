using System;
using System.ComponentModel;
using System.Reactive.Disposables;

namespace RX_MVVM
{
    /// <summary>
    /// Base abstract class for all View models
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) 
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ShouldDispose(IDisposable disposable)
        {
            _disposables.Add(disposable);   
        }

        public IDoFluidCommand<T> For<T>(ICommandObserver<T> cmd)
        {
            var  fluidCommand = new FluidCommand<T>(cmd);
            _disposables.Add(fluidCommand);

            return fluidCommand;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
