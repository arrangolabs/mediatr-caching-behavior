using System.Collections.Generic;
using System.Reflection;
using Fixie;
using Lamar;
using LamarCodeGeneration.Util;
using Microsoft.Extensions.DependencyInjection;

namespace Mediatr.Caching.Behavior.Tests.Setup
{
    public class Setup
    {
        public class TestingConvention : Discovery, Execution
        {
            private Container _container;

            public TestingConvention()
            {
                _container = IoC.GetContainer();
                Classes
                    .Where(x => x.CanBeCastTo<Test>());
            }

            public void Execute(TestClass testClass)
            {
                testClass.RunCases(aCase =>
                {
                    var nestedContainer = _container.GetNestedContainer();

                    var instance = nestedContainer.GetInstance(testClass.Type);

                    aCase.Execute(instance);
                });
            }
            private static void Setup(object aInstance)
            {
                System.Reflection.MethodInfo method = aInstance.GetType().GetMethod(nameof(Setup));
                method?.Execute(aInstance);
            }
        }
    }
}
