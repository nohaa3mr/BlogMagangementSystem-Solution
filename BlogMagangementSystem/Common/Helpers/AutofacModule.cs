using Autofac;
using FluentValidation;

namespace BlogMagangementSystem.Common.Helpers;

public class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var assembly = typeof(AutofacModule).Assembly;

        // Register Generic Repository
        builder.RegisterGeneric(typeof(GenericRepository<>)).AsSelf().InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(assembly)
            .AsClosedTypesOf(typeof(IValidator<>))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        // Register MediatR handlers
        builder.RegisterAssemblyTypes(assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<,>))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<>))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}
