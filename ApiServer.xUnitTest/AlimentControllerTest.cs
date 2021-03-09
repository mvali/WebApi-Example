using ApiServer.Controllers;
using AutoFixture;
using AutoMapper;
using Entities.Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using FluentAssertions;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Repository.Moq;
using Xunit;

namespace ApiServer.xUnitTest
{
    // NSubstiture - for Mocking
    // AutoFixture - for easy creating test objects
    // FluentAssertions - making assertions better
    public class AlimentControllerTest
    {
        private readonly AlimentController _controller;
        
        // this is the mock of IAlimentRepository
        private readonly IAlimentRepository _alimentRepository = Substitute.For<IAlimentRepository>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();
        private readonly ILogger<AlimentController> _logger = Substitute.For<ILogger<AlimentController>>();
        private readonly ILoggerManager _logm = Substitute.For<ILoggerManager>();
        private readonly IFixture _fixture = new Fixture();

        public AlimentControllerTest()
        {
            // initialize controller with Mocked services
            _controller = new AlimentController(_alimentRepository, _mapper, _logger, _logm);
        }

        [Fact] // fact and Theory can not be both alive over the same method, Fact Methods can not have parameters, testData
        /*[Theory] // used for testing with multiple sets of data
        [InlineData(1, "Vali")] // parameters for tested method
        [InlineData(2, "John")]
        public void Update_ShouldReturnTrue_WhenDataAvailable(int alimentId, string name) */
        public void Update_ShouldReturnTrue_WhenDataAvailable()
        {
            // ARRANGE
            const int alimentId = 1; // activated for FACT
            //IAliment aliment =  new AlimentMoq().GetAlimentById(alimentId); // data from AlimentMoq
            IAliment aliment = _fixture.Build<Aliment>() // data generated automatically
                                       .With(x => x.Id, alimentId)
                                       .Create();
            AlimentUpdateDto _alimentDto = _fixture.Build<AlimentUpdateDto>() // using AutoFixture to create 2 random objects connected between them
                .With(x => x.Name, aliment.Name)
                .With(x => x.Line, aliment.Line)
                .Create();
            var alimentUpdateDto = _mapper.Map<IAliment>(_alimentDto);
            // _mapper.Map(_alimentDto, alimentUpdateDto); // just another way of mapping using AutoMapper

            _alimentRepository.GetAlimentById(alimentId).Returns(aliment);

            // ACT
            var result = _controller.UpdateAliment(alimentId,  _alimentDto);
            var resultCode = (IStatusCodeActionResult)result;

            // ASSERT
            resultCode.StatusCode.Should().Be(StatusCodes.Status204NoContent); // this is from FluentAssertions
            Assert.Equal(StatusCodes.Status204NoContent, resultCode.StatusCode); // using classic xUnit

            _alimentRepository.Received(1).UpdateAliment(Arg.Any<Aliment>());
        }
    }
}
