using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using AddressBook.Models;

namespace AddressBook.Repositories
{
    public interface IPersonRepository
    {
        IObservable<IEnumerable<Person>> GetAllClients();
        IObservable<bool> Save(Person person);
        IObservable<bool> Delete(Person person);
    }

    public class PersonRepository : IPersonRepository
    {
        private readonly List<Person> _clients;

        public PersonRepository()
        {
            _clients = new List<Person>
            {
                new Person { Name = "Joe", Surname = "Blogs", Address1 = "Flat 2", Address2 = "19 Fleet Street", Address3 = "City", City = "London", Age = 21 },
                new Person { Name = "Mary", Surname = "Jones", Address1 = "Flat 21", Address2 = "Rotherhithe Street", Address3 = "Calder Court", City = "New York", Age = 28 }
            };
        }

        public IObservable<IEnumerable<Person>> GetAllClients()
        {
            return ToObservable(() =>
            {
                IEnumerable<Person> cl = _clients;
                Thread.Sleep(1500);
                return cl;
            });
        }

        public IObservable<bool> Save(Person person)
        {
            return ToObservable(() =>
            {
                _clients.Add(person);
                Thread.Sleep(100);
                return true;
            });
        }

        public IObservable<bool> Delete(Person person)
        {
            return ToObservable(() =>
            {
                var cl = _clients.FirstOrDefault(c => c.Name == person.Name && c.Surname == person.Surname);
                if (cl != null)
                {
                    _clients.Remove(person);
                    Thread.Sleep(800);
                    return true;                    
                }

                return false;
            });
        }

        private IObservable<T> ToObservable<T>(Func<T> func)
        {
            return func.ToAsync()();
        }
    }
}
