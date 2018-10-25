using System;
using System.Collections.Generic;

namespace Interfaces
{
    public interface IDataService
    {
        IEnumerable<Person> GetPeopleFromGedcom(string gedcomFile);
    }
}
