using Avalonia.Controls;
using GeneGenie.Gedcom.Parser;
using Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public class DefaultDataService : IDataService
    {
        public Task<IEnumerable<Person>> GetPeopleFromGedcomAsync(string gedcomFile)
        {
            return Task.Run(() =>
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
            });
        }

        public bool FileExists(string gedcomFile)
        {
            return File.Exists(gedcomFile);
        }

        public async Task<string> FindFileAsync()
        {
            var openFileDialog = new OpenFileDialog()
            {
                AllowMultiple=false,
                Title="What Gedcom file do you want to use?"
            };
            var pathArray = await openFileDialog.ShowAsync();

            if ((pathArray?.Length ?? 0) > 0)
                return pathArray[0];
            return null;
        }

        public async Task<string> GetWeather(string location)
        {
            var client = new OpenWeatherMap.OpenWeatherMapClient(getWeatherApiAppId());
            var weather = await client.CurrentWeather.GetByName(location);
            return $"City: {weather.City.Name} w/ {weather.Clouds.Value} clouds; last updated {weather.LastUpdate.Value:d}";
        }

        private static string getWeatherApiAppId()
        {
            var configurationBuilder = new ConfigurationBuilder()
                            .AddUserSecrets("weatherServiceAppId");
            var config = configurationBuilder.Build();
            var weatherAppId = config["weatherServiceAppId"];
            return weatherAppId;
        }
    }
}
