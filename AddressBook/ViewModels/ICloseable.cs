using System;

namespace AddressBook.ViewModels
{
    public interface ICloseable
    {
        IObservable<bool> Close { get; } 
    }
}