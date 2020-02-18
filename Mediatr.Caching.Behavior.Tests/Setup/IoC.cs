using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Lamar;
using MediatR;
using Moq;

namespace Mediatr.Caching.Behavior.Tests.Setup
{
    public class IoC
    {
        public static Container GetContainer()
        {
            var container = new Container(x =>
            {
                x.Scan(scanner =>
                {
                    scanner.WithDefaultConventions();
                    scanner.TheCallingAssembly();
                    scanner.AssemblyContainingType<ICacheProvider>();
                    scanner.AddAllTypesOf(typeof(IRequestHandler<,>));
                    scanner.AddAllTypesOf(typeof(INotificationHandler<>));
                    scanner.AddAllTypesOf(typeof(ICachingProfile<,>));
                    scanner.AddAllTypesOf(typeof(Test));
                });

                
                x.For<ServiceFactory>().Use(ctx => ctx.GetInstance);
                x.For<IMediator>().Use<Mediator>();
                x.For(typeof(IPipelineBehavior<,>)).Add(typeof(CachingPipelineBehavior<,>));
                x.For<ICacheProvider>().Use(new Mock<ICacheProvider>().Object).Singleton();
                x.For<ILogger>().Use(new Mock<ILogger>().Object).Singleton();
            });

            Debug.WriteLine(container.WhatDidIScan());
            Debug.WriteLine(container.WhatDoIHave());
            return container;
        }
    }
}
