using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Mediatr.Caching.Behavior.Tests.Helpers;
using Moq;
using Shouldly;

namespace Mediatr.Caching.Behavior.Tests
{
    public class CachingPipelineBehaviorScenarioTests: Test
    {
        private readonly IMediator _mediator;
        private readonly Mock<ILogger> _mockLogger;
        private readonly Mock<ICacheProvider> _mockCacheProvider;
        public CachingPipelineBehaviorScenarioTests(
            IMediator mediator,
            ICacheProvider cacheProvider,
            ILogger logger)
        {
            _mockCacheProvider = Mock.Get(cacheProvider);
            _mediator = mediator;
            _mockLogger = Mock.Get(logger);
        }

        public void It_can_return_result_from_cache_provider()
        {
            var request = new TestableMediatr.Request();
            var expectedProfile = new TestableMediatr.CacheableProfile();
            TestableMediatr.Response expected = new TestableMediatr.Response { Id = "Randy" };
            _mockCacheProvider.Setup(a => a.GetAsync<TestableMediatr.Response>(
                    expectedProfile.GetCacheKey(request),
                    It.IsAny<DateTime>(),
                    It.IsAny<Func<Task<TestableMediatr.Response>>>()))
                .ReturnsAsync(expected);
            var result = _mediator.Send(new TestableMediatr.Request()).Result;

            result.Id.ShouldBe(expected.Id);
        }

        public void It_can_throw_exception_when_cacheProvider_throws_exception()
        {
            var request = new TestableMediatr.Request();
            var expectedProfile = new TestableMediatr.CacheableProfile();
            TestableMediatr.Response expected = new TestableMediatr.Response { Id = "Randy" };
            _mockCacheProvider.Setup(a => a.GetAsync<TestableMediatr.Response>(
                    expectedProfile.GetCacheKey(request),
                    It.IsAny<DateTime>(),
                    It.IsAny<Func<Task<TestableMediatr.Response>>>()))
                .Throws<ArgumentOutOfRangeException>();
            Should.Throw<ArgumentOutOfRangeException>(async() =>  await _mediator.Send(new TestableMediatr.Request()));
        }

    }
}
