namespace AGL.PeopleAndPets.Common
{
    public static class CustomExceptionMessages
    {
        public const string ApiConfigMissing = "API Url not provided.";
        public const string DeserializationUnsuccessful = "Deserialization was not successful";
        public const string ApiResponseFail = "Response unsuccessful. Please check configuration.";
        public const string ApiResponseEmpty = "Empty People JSON retrieved.";
    }
}
