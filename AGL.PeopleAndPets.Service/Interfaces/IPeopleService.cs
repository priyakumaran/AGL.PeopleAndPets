using System.Collections.Generic;
using System.Threading.Tasks;
using AGL.PeopleAndPets.Models;

namespace AGL.PeopleAndPets.Service.Interfaces
{
    public interface IPeopleService
    {
        Task<List<Person>> GetPersonList(string peopleAndPetsUrl);
        Dictionary<string, List<Pet>> GetCatsByPersonGender(List<Person> people);
        
    }
}
