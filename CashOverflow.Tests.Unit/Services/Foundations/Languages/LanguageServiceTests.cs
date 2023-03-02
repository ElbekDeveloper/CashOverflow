// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Languages;
using CashOverflow.Services.Foundations.Languages;
using Moq;
using Tynamix.ObjectFiller;

namespace CashOverflow.Tests.Unit.Services.Foundations.Languages
{
    public partial class LanguageServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly ILanguageService languageService;

        public LanguageServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();

            this.languageService = new LanguageService(
                storageBroker: this.storageBrokerMock.Object);
        }

        private DateTimeOffset GetRandomDatetimeoffset() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private Language CreateRandomLanguage() =>
            CreateLanguageFiller(dates: GetRandomDatetimeoffset()).Create();

        private Filler<Language> CreateLanguageFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Language>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}
