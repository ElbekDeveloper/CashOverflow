// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using CashOverflow.Models.Reviews;
using CashOverflow.Models.Reviews.Exceptions;

namespace CashOverflow.Services.Foundations.Reviews
{
    public partial class ReviewService
    {

        private static void ValidateReviewNotNull(Review review)
        {
            if (review is null)
            {
                throw new NullReviewException();
            }
        }



    }
}

