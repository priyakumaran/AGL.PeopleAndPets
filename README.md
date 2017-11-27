### IDE
- Visual Studio 2017  


### Build & Run
1. Clone the repository  
2. Run the Console Project (AGL.PeopleAndPets.Console) . 


### Project Structure
AGL.PeopleAndPets consists of the following projects:
1. AGL.PeopleAndPets.Console : 
   This console app will call the API and display the expected results in the Console.
2. AGL.PeopleAndPets.Common:
   This project consists of CustomExceptionMessages
3. AGL.PeopleAndPets.Models:
   This project consists of the Models (Person and Pet) used in the application.
4. AGL.PeopleAndPets.Service:
   This project consists of the service (PeopleService) and interface (IPeopleService).
5. AGL.PeopleAndPets.Tests:
   This project consists of unit tests

### Features
1. This application consumes the json from a web service and output a list of all the cats in alphabetical order under a heading of the gender of their owner.
2. When looking for pets of type "Cat" and when grouping the owners by their gender, system ignores the casing. E.g. "male", "MALE", "Male" are grouped together
3. Exception scenarios that are handled are:
   a. API Url is missing in the configuration
   b. Incorrect API Url in configuration
   c. API does not return any JSON
   d. JSON Deserialization failure



