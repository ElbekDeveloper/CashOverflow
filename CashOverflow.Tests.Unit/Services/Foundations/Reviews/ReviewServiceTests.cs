// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using CashOverflow.Brokers.DateTimes;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Reviews;
using CashOverflow.Services.Foundations.Reviews;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Reviews
{
    public partial class ReviewServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IReviewService reviewService;

        public ReviewServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.reviewService = new ReviewService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        public static TheoryData<int> InvalidStars()
        {
            int starsAboveRange = GetRandomStars();
            int starsBelowRange = GetRandomNegativeStars();

            return new TheoryData<int>
            {
                starsAboveRange,
                starsBelowRange
            };
        }

        private string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomStars() =>
            new IntRange(min: 6, max: 10).GetValue();

        private static int GetRandomNegativeStars() =>
            -1 * new IntRange(min: 1, max: 10).GetValue();

        private static int GetRandomStarsInRange() =>
            new IntRange(min: 1, max: 5).GetValue();

        private Expression<Func<Exception, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 9).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static string GetRandomMessage() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private IQueryable<Review> CreateRandomReviews()
        {
            return CreateReviewFiller(GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber()).AsQueryable();
        }

        private Review CreateRandomReview(int stars) =>
              CreateReviewFiller(stars).Create();

        private Filler<Review> CreateReviewFiller(int stars)
        {
            var filler = new Filler<Review>();
            filler.Setup()
                .OnType<int>().Use(stars)
                .OnType<DateTimeOffset>().Use(GetRandomDateTimeOffset);

            return filler;
        }

        private Review CreateRandomReview(DateTimeOffset dates) =>
            CreateReviewFiller(dates).Create();

        private Review CreateRandomReview() =>
            CreateReviewFiller(dates: GetRandomDateTimeOffset()).Create();

        private Filler<Review> CreateReviewFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Review>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
        private static SqlException CreateSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));
    }
}
