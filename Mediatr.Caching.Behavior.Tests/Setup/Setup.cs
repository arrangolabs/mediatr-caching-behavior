using System;
using System.Collections.Generic;
using System.Text;
using Fixie;

namespace Mediatr.Caching.Behavior.Tests.Setup
{
    public class Setup
    {
        public class TestingConvention : Execution
        {
            public void Execute(TestClass testClass)
            {
                //TODO get TestClass from DI Container
                var instance = testClass.Construct();

                testClass.RunCases(@case =>
                {
                    @case.Execute(instance);
                });

                instance.Dispose();
            }
        }
    }
}
