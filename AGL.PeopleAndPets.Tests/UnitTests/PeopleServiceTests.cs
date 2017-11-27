using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using AGL.PeopleAndPets.Common;
using AGL.PeopleAndPets.Models;
using AGL.PeopleAndPets.Service.Interfaces;
using AGL.PeopleAndPets.Service.Services;
using AutofacContrib.NSubstitute;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AGL.PeopleAndPets.Tests.UnitTests
{
    [TestClass]
    public class PeopleServiceTests
    {
        private readonly IPeopleService _peopleService;
        private readonly string _validApiUrl = ConfigurationManager.AppSettings["PeopleAndPetsUrl"];
        private readonly string _fakeApiUrl = "FakeApiUrl";

        public PeopleServiceTests()
        {
            var container = new AutoSubstitute();
            _peopleService = container.Resolve<PeopleService>();

        }

        [TestMethod]
        public void GroupCatsByPersonGenderAndSortAlphaValid()
        {
            var personList = new List<Person>()
            {
                new Person()
                {
                    Name = "Bob", Age = 23, Gender = "Male",
                    Pets = new List<Pet>()
                    {
                        new Pet() {Name = "Garfield", Type = "CAT"},
                        new Pet() {Name = "Fido", Type = "Dog"}
                    }
                },
                new Person()
                {
                    Name = "Jennifer", Age = 18, Gender = "Female",
                    Pets = new List<Pet>()
                    {
                        new Pet() {Name = "Tom", Type = "Cat"},
                        new Pet() {Name = "Garfield", Type = "Cat"}

                    }
                }
            };

            var result = _peopleService.GetCatsByPersonGender(personList);
            var expectedResult = new Dictionary<string, List<Pet>>();
            var firstPet = new Pet() { Name = "Garfield", Type = "CAT" };
            var secondPet = new Pet() { Name = "Garfield", Type = "Cat" };
            var thirdPet = new Pet() { Name = "Tom", Type = "Cat" };
            expectedResult.Add("Male", new List<Pet>() { firstPet });
            expectedResult.Add("Female", new List<Pet>() { secondPet, thirdPet });

            result.ShouldBeEquivalentTo(expectedResult, "Expected response not the same as actual response.");
            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result.Keys.ToList()[0] == "Male");
            Assert.IsTrue(result["Male"][0].Name == "Garfield");
            Assert.IsTrue(result.Keys.ToList()[1] == "Female");
            Assert.IsTrue(result["Female"].Count == 2);
            Assert.IsTrue(result["Female"][0].Name == "Garfield");
            Assert.IsTrue(result["Female"][1].Name == "Tom");
        }

        [TestMethod]
        public void ValidateGenderCasingIgnoredInGrouping()
        {
            var person1 = new Person()
            {
                Name = "Bob", Age = 23, Gender = "Male",
                Pets = new List<Pet>()
                {
                    new Pet() {Name = "Garfield", Type = "CAT"},
                    new Pet() {Name = "Fido", Type = "Dog"}
                }
            };
            var person2 = new Person()
            {
                Name = "John", Age = 25, Gender = "MALE",
                Pets = new List<Pet>() { new Pet() { Name = "Pluto", Type = "Cat" } }
            };
            var person3 = new Person()
            {
                Name = "Jennifer", Age = 18, Gender = "Female", Pets = new List<Pet>()
                {
                    new Pet() {Name = "Tom", Type = "Cat"},
                    new Pet() {Name = "Jerry", Type = "CAT"},
                }
            };
            var person4 = new Person()
            {
                Name = "Anne", Age = 20, Gender = "FEMALE",
                Pets = new List<Pet>()
                {
                    new Pet() {Name = "Ben", Type = "Cat"}

                }
            };
            var personList = new List<Person>()
            {
                person1,
                person2,
                person3,
                person4
            };

            var result = _peopleService.GetCatsByPersonGender(personList);
            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result.Keys.ToList()[0] == "Male");
            Assert.IsTrue(result.Keys.ToList()[1] == "Female");
            Assert.IsTrue(result["Male"].Count == 2);
            Assert.IsTrue(result["Female"].Count == 3);
        }

        [TestMethod]
        public void GroupCatsByPersonGenderAndSortAlphaInValid()
        {
            var personList = new List<Person>()
            {
                new Person()
                {
                    Name = "Bob", Age = 23, Gender = "Male",
                    Pets = new List<Pet>()
                    {
                        new Pet() {Name = "Garfield", Type = "Cat"},
                        new Pet() {Name = "Fido", Type = "Dog"}
                    }
                },
                new Person()
                {
                    Name = "Samantha", Age = 18, Gender = "Female",
                    Pets = new List<Pet>()
                    {
                        new Pet() {Name = "Tom", Type = "Cat"},
                        new Pet() {Name = "Garfield", Type = "Dog"}

                    }
                }
            };

            var result = _peopleService.GetCatsByPersonGender(personList);
            var expectedResult = new Dictionary<string, List<Pet>>();
            var firstPet = new Pet() { Name = "Garfield", Type = "Cat" };
            var secondPet = new Pet() { Name = "Garfield", Type = "Cat" };
            var thirdPet = new Pet() { Name = "Tom", Type = "Cat" };
            expectedResult.Add("Male", new List<Pet>() { firstPet });
            expectedResult.Add("Female", new List<Pet>() { secondPet, thirdPet });

            CollectionAssert.AreNotEquivalent(expectedResult, result);
            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result.Keys.ToList()[0] == "Male");
            Assert.IsTrue(result["Male"][0].Name == "Garfield");
            Assert.IsTrue(result.Keys.ToList()[1] == "Female");
            Assert.IsTrue(result["Female"].Count != 2);
            Assert.IsTrue(result["Female"][0].Name != "Garfield");
        }


        [TestMethod]
        public void GroupCatsByPersonGenderAndSortAlphaNoPets()
        {
            var personList = new List<Person>()
            {
                new Person(){ Name = "Bob", Age = 23, Gender = "Male", Pets = new List<Pet>()
                },
                new Person()
                {
                    Name = "Samantha", Age = 18, Gender = "Female", Pets = new List<Pet>()
                }
            };

            var result = _peopleService.GetCatsByPersonGender(personList);
            Assert.IsTrue(result.Count == 0);

        }


        [TestMethod]
        public void ValidateGetPersonListReturnsExceptionIfApiUrlNotProvided()
        {
            Func<Task> asyncFunction = async () => { await _peopleService.GetPersonList(null); };
            // Assert
            asyncFunction.ShouldThrow<Exception>(CustomExceptionMessages.ApiConfigMissing);

        }

        [TestMethod]
        public void ValidateGetPersonListReturnsExceptionIfApiUrlIncorrect()
        {
            Func<Task> asyncFunction = async () => { await _peopleService.GetPersonList("test"); };
            // Assert
            asyncFunction.ShouldThrow<Exception>(CustomExceptionMessages.ApiConfigMissing);

        }

        [TestMethod]
        public void ValidateGetPersonListReturnsExceptionIfApiReturnsEmptyJson()
        {
            Func<Task> asyncFunction = async () => { await _peopleService.GetPersonList(_fakeApiUrl); };

            // Assert
            asyncFunction.ShouldThrow<Exception>(CustomExceptionMessages.ApiResponseEmpty);

        }

        [TestMethod]
        public void ValidateGetPersonListDoesNotThrowExceptionWhenCallingValidApi()
        {
            Func<Task> asyncFunction = async () => { await _peopleService.GetPersonList(_validApiUrl); };

            // Assert
            asyncFunction.ShouldNotThrow<Exception>(CustomExceptionMessages.DeserializationUnsuccessful);

        }

    }
}
