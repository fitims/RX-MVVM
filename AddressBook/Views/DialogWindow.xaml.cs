using System;
using System.Reactive.Linq;
using System.Windows;
using AddressBook.ViewModels;
using RX_MVVM;

namespace AddressBook.Views
{
    public partial class DialogWindow : Window
    {
        public DialogWindow(ViewModelBase viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            var closeable = viewModel as ICloseable;
            if (closeable != null)
            {
                closeable.Close.ObserveOnDispatcher().Subscribe(r =>
                {
                    CloseResult = r;
                    Close();
                });
            }
        }

        public bool CloseResult { get; private set; }
    }
}
