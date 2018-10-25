using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IDataService
    {
        IEnumerable<Person> GetPeopleFromGedcom(string gedcomFile);
        bool FileExists(string gedcomFile);
        Task<string> FindFile();
    }
}
