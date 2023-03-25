// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Brokers.DateTimes;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Reviews;
using CashOverflow.Models.Reviews.Exceptions;
using CashOverflow.Models.Locations;

namespace CashOverflow.Services.Foundations.Reviews
{
    public partial class ReviewService : IReviewService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public ReviewService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public async ValueTask<Review> AddReviewAsync(Review review)
        {
            try
            {
                if (review is null)
                {
                    throw new NullReviewException();
                }

                return await this.storageBroker.InsertReviewAsync(review);
            }
            catch (NullReviewException nullReviewException)
            {
                var reviewValidationException = new ReviewValidationException(nullReviewException);
                this.loggingBroker.LogError(reviewValidationException);

                throw reviewValidationException;
            }
        }

        public IQueryable<Review> RetrieveAllReviews() =>
            TryCatch(() => this.storageBroker.SelectAllReviews());
    }
}
