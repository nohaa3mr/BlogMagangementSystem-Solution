using Autofac;

namespace BlogMagangementSystem.Common.Helpers;

public class AutofacModule  :Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var assembly = typeof(AutofacModule).Assembly;

        builder.RegisterGeneric(typeof(GenericRepository<>)).AsSelf().InstancePerLifetimeScope();
        builder.RegisterAssemblyTypes(assembly)
         .Where(t => t.Name.EndsWith("Validator"))
         .AsImplementedInterfaces()
         .InstancePerLifetimeScope();
        builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();

        // Register all request handlers
        builder.RegisterAssemblyTypes(assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<,>))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<>))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        // Register notification handlers
        builder.RegisterAssemblyTypes(assembly)
            .AsClosedTypesOf(typeof(INotificationHandler<>))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        // Register pipeline behaviors
        builder.RegisterAssemblyTypes(assembly)
            .AsClosedTypesOf(typeof(IPipelineBehavior<,>))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        builder.RegisterAssemblyTypes(assembly);
        
    }

}
