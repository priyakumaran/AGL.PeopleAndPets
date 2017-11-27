using AGL.PeopleAndPets.Service.Interfaces;
using AGL.PeopleAndPets.Service.Services;
using Autofac;

namespace AGL.PeopleAndPets.Console
{
    public class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<PeopleService>().As<IPeopleService>();
            return builder.Build();

        }
    }
}
