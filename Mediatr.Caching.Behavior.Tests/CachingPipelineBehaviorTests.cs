using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Shouldly;

namespace Mediatr.Caching.Behavior.Tests
{
    public class CachingPipelineBehaviorTests
    {
        public void It_Exists()
        {
            var behavior =
                new CachingPipelineBehavior<TestableMediatr.TestableMediatr.Request,
                    TestableMediatr.TestableMediatr.Response>();

        }

        public void It_Implements_Interface()
        {
            var behavior = new CachingPipelineBehavior<TestableMediatr.TestableMediatr.Request,
                TestableMediatr.TestableMediatr.Response>();
            behavior.ShouldBeAssignableTo<IPipelineBehavior<TestableMediatr.TestableMediatr.Request,
                TestableMediatr.TestableMediatr.Response>>();
        }
    }
}
