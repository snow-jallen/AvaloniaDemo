using GeneGenie.Gedcom.Parser;
using Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Data
{
    public class DefaultDataService : IDataService
    {
        public IEnumerable<Person> GetPeopleFromGedcom(string gedcomFile)
        {
            var reader = GedcomRecordReader.CreateReader(gedcomFile);
            var db = reader.Database;

            return from p in db.Individuals
                   where p.Names.Any()
                   let name = p.Names.First()
                   select new Person(
                       name.Surname,
                       name.Given,
                       p.Birth?.Date?.DateTime1,
                       p.Birth?.Address?.ToString(),
                       p.Death?.Date?.DateTime1,
                       p.Death?.Address?.ToString());
        }

        public bool FileExists(string gedcomFile)
        {
            return File.Exists(gedcomFile);
        }
    }
}
