using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using AddressBook.Views;
using RX_MVVM;

namespace AddressBook.Services
{
    public interface IDialogService
    {
        void ShowInfo(string title, string message);
        MessageBoxResult ShowWarning(string title, string message);
        void ShowError(string title, string message);
        bool ShowViewModel(string title, ViewModelBase viewModel);
    }

    public class DialogService : IDialogService
    {
        public void ShowInfo(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public MessageBoxResult ShowWarning(string title, string message)
        {
            return MessageBox.Show(message, title, MessageBoxButton.OKCancel, MessageBoxImage.Warning);
        }

        public void ShowError(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public bool ShowViewModel(string title, ViewModelBase viewModel)
        {
            var win = new DialogWindow(viewModel) {Title = title};
            win.ShowDialog();

            return win.CloseResult;
        }
    }
}
