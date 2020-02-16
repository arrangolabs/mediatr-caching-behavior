using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fixie.Internal.Listeners;
using MediatR;
using Moq;
using Shouldly;

namespace Mediatr.Caching.Behavior.Tests
{
    public class CachingPipelineBehaviorTests
    {
        public void It_Exists()
        {
            var behavior =
                new CachingPipelineBehavior<TestableMediatr.TestableMediatr.Request,
                    TestableMediatr.TestableMediatr.Response>(
                    new List<ICachingProfile<TestableMediatr.TestableMediatr.Request,
                            TestableMediatr.TestableMediatr.Response>>()
                        {new TestableMediatr.TestableMediatr.CacheableRequest()}, new Logger(),
                    new Mock<ICacheProvider>().Object);

        }

        public void It_Implements_Interface()
        {
            var behavior =
                new CachingPipelineBehavior<TestableMediatr.TestableMediatr.Request,
                    TestableMediatr.TestableMediatr.Response>(
                    new List<ICachingProfile<TestableMediatr.TestableMediatr.Request,
                            TestableMediatr.TestableMediatr.Response>>()
                        {new TestableMediatr.TestableMediatr.CacheableRequest()}, new Logger(),
                    new Mock<ICacheProvider>().Object);

            behavior.ShouldBeAssignableTo<IPipelineBehavior<TestableMediatr.TestableMediatr.Request,
                TestableMediatr.TestableMediatr.Response>>();
        }

        public void It_can_pass_through_if_no_profiles_exist()
        {
            var behavior =
                new CachingPipelineBehavior<TestableMediatr.TestableMediatr.Request,
                    TestableMediatr.TestableMediatr.Response>(
                    new List<ICachingProfile<TestableMediatr.TestableMediatr.Request,
                        TestableMediatr.TestableMediatr.Response>>(), new Logger(),
                    new Mock<ICacheProvider>().Object);

            var expected = new TestableMediatr.TestableMediatr.Response();

            var result = behavior.Handle(new TestableMediatr.TestableMediatr.Request {Id = Guid.NewGuid().ToString()},
                new CancellationToken(), async () => expected).Result;
            result.ShouldBe(expected);
        }

        public void It_can_pass_through_if_a_null_profiles_collection_is_provided()
        {
            var behavior =
                new CachingPipelineBehavior<TestableMediatr.TestableMediatr.Request,
                    TestableMediatr.TestableMediatr.Response>(null,
                    new Logger(),
                    new Mock<ICacheProvider>().Object);

            var expected = new TestableMediatr.TestableMediatr.Response();

            var result = behavior.Handle(new TestableMediatr.TestableMediatr.Request {Id = Guid.NewGuid().ToString()},
                new CancellationToken(), async () => expected).Result;
            result.ShouldBe(expected);
        }

        public void It_can_use_first_profile_in_collection_and_get_results_from_cacheProvider()
        {
            var cacheProvider = new Mock<ICacheProvider>();
            var expectedProfile = new TestableMediatr.TestableMediatr.CacheableRequest();
            var request = new TestableMediatr.TestableMediatr.Request {Id = Guid.NewGuid().ToString()};
            var behavior =
                new CachingPipelineBehavior<TestableMediatr.TestableMediatr.Request,
                    TestableMediatr.TestableMediatr.Response>(
                    new List<ICachingProfile<TestableMediatr.TestableMediatr.Request,
                            TestableMediatr.TestableMediatr.Response>>()
                        {expectedProfile, new TestableMediatr.TestableMediatr.CacheableRequest()}, new Logger(),
                    cacheProvider.Object);

            var expected = new TestableMediatr.TestableMediatr.Response();
            Func<Task<TestableMediatr.TestableMediatr.Response>> requestHandlerDelegate = async () => expected;

            cacheProvider.Setup(a => a.GetAsync<TestableMediatr.TestableMediatr.Response>(
                    expectedProfile.GetCacheKey(request),
                    It.IsAny<DateTime>(),
                    It.IsAny<Func<Task<TestableMediatr.TestableMediatr.Response>>>()))
                .ReturnsAsync(expected);

            var result = behavior.Handle(request,
                new CancellationToken(), async () => expected).Result;
            
            result.ShouldBe(expected);

        }
    }
}
