// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using CashOverflow.Models.Reviews;
using CashOverflow.Models.Reviews.Exceptions;
using System.Data;

namespace CashOverflow.Services.Foundations.Reviews
{
    public partial class ReviewService
    {


        private static void ValidateReviewOnAdd(Review review)
        {
            ValidateReviewNotNull(review);

            Validate((Rule: IsInvalid(review.Id), Parameter: nameof(Review.Id)),
                     (Rule: IsInvalid(review.CompanyName), Parameter: nameof(Review.CompanyName)),
                     (Rule: IsInvalid(review.Stars), Parameter: nameof(Review.Stars)),
                     (Rule: IsInvalid(review.Thoughts), Parameter: nameof(Review.Thoughts)),
                     (Rule: IsOutOfRange(review.Stars), Parameter: nameof(Review.Stars)));
        }
        private static dynamic IsInvalid(Guid Id) => new
        {
            Condition = Id == default,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(int stars) => new
        {
            Condition = stars == 0,
            Message = "Stars are required"
        };

        private static dynamic IsOutOfRange(int stars) => new
        {
            Condition = stars is > 5 or < 0,
            Message = "Stars are out of range"
        };

        private static void ValidateReviewNotNull(Review review)
        {
            if (review is null)
            {
                throw new NullReviewException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidReviewException = new InvalidReviewException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidReviewException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidReviewException.ThrowIfContainsErrors();
        }


    }
}

