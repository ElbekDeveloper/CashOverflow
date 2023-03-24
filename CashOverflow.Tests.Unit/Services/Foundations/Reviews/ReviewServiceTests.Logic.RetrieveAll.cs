// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using CashOverflow.Models.Reviews;
using FluentAssertions;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Reviews
{
    public partial class ReviewServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllReviews()
        {
            // given 
            IQueryable<Review> randomReviews = CreateRandomReviews();
            IQueryable<Review> storageReviews = randomReviews;
            IQueryable<Review> expectedReviews = storageReviews;

            this.storageBrokerMock.Setup(broker => broker.SelectAllReviews()).Returns(storageReviews);

            // when 
            IQueryable<Review> actualReviews = this.reviewService.RetrieveAllReviews();

            // then 
            actualReviews.Should().BeEquivalentTo(expectedReviews);

            this.storageBrokerMock.Verify(broker => broker.SelectAllReviews(), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
