using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using AGL.PeopleAndPets.Models;
using AGL.PeopleAndPets.Service.Interfaces;
using Autofac;

namespace AGL.PeopleAndPets.Console
{
    class Program
    {
        private static readonly string PeopleAndPetsUrl = ConfigurationManager.AppSettings["PeopleAndPetsUrl"];

        static void Main()
        {
            Task t = MainAsync();
            t.Wait();
        }
        static async Task MainAsync()
        {
            var container = ContainerConfig.Configure();
            var service = container.Resolve<IPeopleService>();

            try
            {
                var people = await service.GetPersonList(PeopleAndPetsUrl);
                if (people != null)
                {
                    var resultSet = service.GetCatsByPersonGender(people);

                    DisplayResults(resultSet);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error: {ex.Message}");
            }
            
        }
        
        private static void DisplayResults(Dictionary<string, List<Pet>> resultSet)
        {
            foreach (var group in resultSet)
            {
                System.Console.WriteLine(group.Key);

                foreach (var pet in group.Value)
                {
                    System.Console.WriteLine(" - " + pet.Name);
                }
            }
        }

    }
}
