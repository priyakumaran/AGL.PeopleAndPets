using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AGL.PeopleAndPets.Common;
using AGL.PeopleAndPets.Models;
using AGL.PeopleAndPets.Service.Interfaces;
using Newtonsoft.Json;

namespace AGL.PeopleAndPets.Service.Services
{
    public class PeopleService : IPeopleService
    {
        public Dictionary<string, List<Pet>> GetCatsByPersonGender(List<Person> people)
        {
            if (people == null || people.Count <= 0) return new Dictionary<string, List<Pet>>();
            var query = (from person in people
                where person.Pets != null
                from pet in person.Pets
                where pet.Type.ToUpper() == "CAT"
                group pet by Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(person.Gender.ToLower()) into g
                select new { Gender = g.Key, Pets = g.OrderBy(p => p.Name).ToList() }).ToList();
            
            var result = query.ToDictionary(a => a.Gender, a => a.Pets);

            return result;
        }

        public async Task<Dictionary<string, List<Pet>>> GetCatsByGenderResults(string peopleAndPetsUrl)
        {
            var people = await GetPersonList(peopleAndPetsUrl);

            var resultSet = GetCatsByPersonGender(people);

            return resultSet;
        }

        public async Task<List<Person>> GetPersonList(string peopleAndPetsUrl)
        {
          
            if (string.IsNullOrEmpty(peopleAndPetsUrl)) throw new Exception(CustomExceptionMessages.ApiConfigMissing);
            var peopleJson = await GetApiResult(peopleAndPetsUrl);
            if (string.IsNullOrEmpty(peopleJson))
                throw new Exception($"{CustomExceptionMessages.ApiResponseEmpty}");

            return DeserializeResult(peopleJson);
           
        }

        private static List<Person> DeserializeResult(string peopleJson)
        {
            try
            {
                var people = JsonConvert.DeserializeObject<List<Person>>(peopleJson);
                return people;
            }
            catch (Exception)
            {
                throw new Exception($"{CustomExceptionMessages.DeserializationUnsuccessful}");
            }
        }

        private async Task<string> GetApiResult(string peopleAndPetsUrl)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(peopleAndPetsUrl);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"{CustomExceptionMessages.ApiResponseFail} {peopleAndPetsUrl}");
            var peopleJson = response.Content.ReadAsStringAsync().Result;
            return peopleJson;
        }
    }
}
