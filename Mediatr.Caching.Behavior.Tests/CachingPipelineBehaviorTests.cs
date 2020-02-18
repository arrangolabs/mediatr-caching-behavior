using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fixie.Internal.Listeners;
using MediatR;
using Mediatr.Caching.Behavior.Tests.Helpers;
using Moq;
using Shouldly;

namespace Mediatr.Caching.Behavior.Tests
{
    public class CachingPipelineBehaviorTests: Test
    {
        private readonly IMediator _mediator;
        private readonly Mock<ICacheProvider> _mockCacheProvider;
        public CachingPipelineBehaviorTests(
            IMediator mediator, 
            ICacheProvider cacheProvider)
        {
            _mockCacheProvider = Mock.Get(cacheProvider);
            _mediator = mediator;
        }


        public void It_Exists()
        {
            var behavior =
                new CachingPipelineBehavior<TestableMediatr.Request,
                    TestableMediatr.Response>(
                    new List<ICachingProfile<TestableMediatr.Request,
                            TestableMediatr.Response>>()
                        {new TestableMediatr.CacheableProfile()}, new Logger(),
                    new Mock<ICacheProvider>().Object);

        }

        public void It_Implements_Interface()
        {
            var behavior =
                new CachingPipelineBehavior<TestableMediatr.Request,
                    TestableMediatr.Response>(
                    new List<ICachingProfile<TestableMediatr.Request,
                            TestableMediatr.Response>>()
                        {new TestableMediatr.CacheableProfile()}, new Logger(),
                    new Mock<ICacheProvider>().Object);

            behavior.ShouldBeAssignableTo<IPipelineBehavior<TestableMediatr.Request,
                TestableMediatr.Response>>();
        }

        public void It_can_pass_through_if_no_profiles_exist()
        {
            var behavior =
                new CachingPipelineBehavior<TestableMediatr.Request,
                    TestableMediatr.Response>(
                    new List<ICachingProfile<TestableMediatr.Request,
                        TestableMediatr.Response>>(), new Logger(),
                    new Mock<ICacheProvider>().Object);

            var expected = new TestableMediatr.Response();

            var result = behavior.Handle(new TestableMediatr.Request {Id = Guid.NewGuid().ToString()},
                new CancellationToken(), async () => expected).Result;
            result.ShouldBe(expected);
        }

        public void It_can_pass_through_if_a_null_profiles_collection_is_provided()
        {
            var behavior =
                new CachingPipelineBehavior<TestableMediatr.Request,
                    TestableMediatr.Response>(null,
                    new Logger(),
                    new Mock<ICacheProvider>().Object);

            var expected = new TestableMediatr.Response();

            var result = behavior.Handle(new TestableMediatr.Request {Id = Guid.NewGuid().ToString()},
                new CancellationToken(), async () => expected).Result;
            result.ShouldBe(expected);
        }

        public void It_can_use_first_profile_in_collection_and_get_results_from_cacheProvider()
        {
            var cacheProvider = new Mock<ICacheProvider>();
            var expectedProfile = new TestableMediatr.CacheableProfile();
            var request = new TestableMediatr.Request {Id = Guid.NewGuid().ToString()};
            var behavior =
                new CachingPipelineBehavior<TestableMediatr.Request,
                    TestableMediatr.Response>(
                    new List<ICachingProfile<TestableMediatr.Request,
                            TestableMediatr.Response>>()
                        {expectedProfile, new TestableMediatr.CacheableProfile()}, new Logger(),
                    cacheProvider.Object);

            var expected = new TestableMediatr.Response();
            Func<Task<TestableMediatr.Response>> requestHandlerDelegate = async () => expected;

            cacheProvider.Setup(a => a.GetAsync<TestableMediatr.Response>(
                    expectedProfile.GetCacheKey(request),
                    It.IsAny<DateTime>(),
                    It.IsAny<Func<Task<TestableMediatr.Response>>>()))
                .ReturnsAsync(expected);

            var result = behavior.Handle(request,
                new CancellationToken(), async () => expected).Result;
            
            result.ShouldBe(expected);

        }
    }
}
