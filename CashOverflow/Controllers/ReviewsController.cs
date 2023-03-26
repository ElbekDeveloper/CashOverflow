// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using CashOverflow.Models.Reviews;
using CashOverflow.Models.Reviews.Exceptions;
using CashOverflow.Services.Foundations.Reviews;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace CashOverflow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : RESTFulController
    {
        private readonly IReviewService reviewService;

        public ReviewsController(IReviewService reviewService) =>
            this.reviewService = reviewService;

        [HttpGet]
        public ActionResult<IQueryable<Review>> GetAllReviews()
        {
            try
            {
                IQueryable<Review> allReviews = this.reviewService.RetrieveAllReviews();

                return Ok(allReviews);
            }
            catch (ReviewDependencyException reviewDependencyException)
            {
                return InternalServerError(reviewDependencyException.InnerException);
            }
            catch (ReviewServiceException reviewServiceException)
            {
                return InternalServerError(reviewServiceException.InnerException);
            }
        }
    }
}
