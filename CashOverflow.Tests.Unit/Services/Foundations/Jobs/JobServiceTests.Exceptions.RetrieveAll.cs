//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CashOverflow.Models.Jobs.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Jobs
{
    public partial class JobServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfSqlErrorOccursAndLogIt()
        {
            //given 
            SqlException sqlException = CreateSqlException();
            var failedJobStorageException = 
                new FailedJobStorageException(sqlException);

            var expectedJobDependencyException = 
                new JobDependencyException(failedJobStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllJobs())
                    .Throws(sqlException);

            //when
            Action retrieveAllJobsAction = () =>
                this.jobService.RetrieveAllJobs();
            
            JobDependencyException actualJobDependencyException = 
                Assert.Throws<JobDependencyException>(retrieveAllJobsAction);

            //then
            actualJobDependencyException.Should().BeEquivalentTo(expectedJobDependencyException);

            this.storageBrokerMock.Verify(broker => 
                broker.SelectAllJobs(), Times.Once);
            
            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedJobDependencyException ))), Times.Once);
    
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenAllServiceErrorOccursAndLogIt()
        {
            //given
            string exceptionMessage = GetRandomString();
            var serviceException = 
                new Exception(exceptionMessage);

            var failedJobServiceException = 
                new FailedJobServiceException(serviceException);

            var expectedJobServiceException = 
                new JobServiceException(failedJobServiceException);
            
            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllJobs()).Throws(serviceException);
            
            //when
            Action retrieveAllJobAction = () =>
                this.jobService.RetrieveAllJobs();
            
            JobServiceException actualJobServiceException = 
                Assert.Throws<JobServiceException>(retrieveAllJobAction);
            
            //then
            actualJobServiceException.Should().BeEquivalentTo(expectedJobServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllJobs(), Times.Once);
            
            this.loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(SameExceptionAs(
                    expectedJobServiceException))), Times.Once);
            
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}