using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AddressBook.Models;
using AddressBook.ViewModels;

namespace AddressBook.Extensions
{
    public static class PersonExtensions
    {
        public static Person ToModel(this IPersonViewModel personVm)
        {
            return new Person
            {
                Name = personVm.Name,
                Surname = personVm.Surname,
                Age = personVm.Age,
                Address1 = personVm.Address1,
                Address2 = personVm.Address2,
                Address3 = personVm.Address3,
                City = personVm.City,

            };
        }
    }
}
