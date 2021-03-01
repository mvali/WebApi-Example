﻿using ApiServer.Controllers;
using ApiServer.Profiles;
using AutoMapper;
using Entities.Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Repository.Moq;

namespace NUnitTestApiServer
{
    // Install Moq, NUnit nuget package
    // Each test must have it's own unit tests
    //      - Test all the execution paths - if function shoud return true or false depending on UI data
    public class AlimentControllerTest
    {
        private AlimentController controller;
        private Mock<IAlimentRepository> alimentRepoMock;
        private Mock<JsonPatchDocument<AlimentUpdateDto>> alimentUpdateDto;
        private Mock<IAliment> alimentMock;
        private IAliment aliment;
        private AlimentMoq alimentMoq;
        private JsonPatchDocument<AlimentUpdateDto> jsonpatchDocu_alimUpDto;

        [SetUp]
        public void Setup()
        {
            //not a concrete implementation but a Mock object
            alimentRepoMock = new Mock<IAlimentRepository>();
            alimentMoq = new AlimentMoq();
            aliment = alimentMoq.GetAlimentById(1);
            alimentMock = new Mock<IAliment>();
            alimentUpdateDto = new Mock<JsonPatchDocument<AlimentUpdateDto>>();
            jsonpatchDocu_alimUpDto = new JsonPatchDocument<AlimentUpdateDto>();

            // we are not adding, we are replacing a value
            jsonpatchDocu_alimUpDto.Replace(x => x.Name, "name test");


            var services = new ServiceCollection();
            var provider = services.BuildServiceProvider();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ConstructServicesUsing(provider.GetService);
                cfg.AddProfile(new AlimentProfile());
                //cfg.AddProfile(new SomeOtherMappingProfile());
            });
            var mapper = config.CreateMapper();

            controller = new AlimentController(alimentRepoMock.Object, mapper);
        }

        // Each test must have it's own unit tests
        [Test]
        public void AlimentMethodCheck()
        {
            //A Mock can be: - Instructed, you can tell a mock that if a certain method is called then it can answer with a certain response
            //               - Verified, You carry this out to verify that a certain method has been called with specific arguments

            //To instruct it we use the method Setup()
            // Method Arguments can be : 1. Exact  arguments // p.GetAlimentById(1)
            //                           2. General arguments // use the helper "It", which will allow to instruct the method GetAlimentById() that any values of a certain data type can be passed through
            alimentRepoMock.Setup(p => p.GetAlimentById(1)).Returns(aliment);
            alimentRepoMock.Setup(p => p.AlimentMethod(1, aliment)).Returns(true);

            alimentRepoMock.Setup(p => p.AlimentMethod(It.IsAny<int>(), aliment)).Returns(true);

            //call the actual production code -> pass an implementation of our Mock
            alimentRepoMock.Setup(p => p.AlimentMethod(It.IsAny<int>(), alimentMock.Object)).Returns(true);

            Assert.Pass();
        }

        [Test]
        public void UpdateShouldReturnTrue()
        {
            // ARRANGE
            // assume that "AlimentMethod" method with any <int> param and alimentMock object will return true
            alimentRepoMock.Setup(p => p.GetAlimentById(It.IsAny<int>())).Returns(aliment);

            // ACT
            // evaluate controller main method
            //var result = controller.UpdateAliment(It.IsAny<int>(), alimentUpdateDto.Object);
            var result = controller.UpdateAliment(It.IsAny<int>(), It.IsAny<AlimentUpdateDto>());
            var resultCode = (IStatusCodeActionResult)result;

            // ASSERT
            // I expect the "GetAlimentById()" method to have been called only once with any "<int>" data type
            alimentRepoMock.Verify(x => x.GetAlimentById(It.IsAny<int>()), Times.Once());

            // I expect the "UpdateAliment()" method to have been called only once with an "Aliment" object
            //alimentRepoMock.Verify(x => x.UpdateAliment(alimentMock.Object), Times.Once()); // asked once with aliment object
            alimentRepoMock.Verify(x => x.UpdateAliment(It.IsAny<Aliment>()), Times.Once()); // asked once with aliment object

            Assert.AreEqual(StatusCodes.Status204NoContent, resultCode.StatusCode);
        }

        [Test]
        public void PatchUpdate()
        {
            var services = new ServiceCollection();

            // add your mock service to service collection
            services.AddSingleton<IAlimentRepository, AlimentMoq>();

            // configure the response to be received from db
            alimentRepoMock.Setup(p => p.GetAlimentById(It.IsAny<int>())).Returns(aliment);

            // build it
            var provider = services.BuildServiceProvider();

            // config your mapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ConstructServicesUsing(provider.GetService);
                cfg.AddProfile(new AlimentProfile());
                //cfg.AddProfile(new SomeOtherMappingProfile());
            });
            var mapper = config.CreateMapper();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsNotNull<object>())
            );
            // create controller with test data
            controller = new AlimentController(alimentRepoMock.Object, mapper);
            // inject Validator into your controller - Before ACT function
            controller.ObjectValidator = objectValidator.Object;

            // Act
            var result = controller.PartialUpdate(It.IsAny<int>(), jsonpatchDocu_alimUpDto);
            var resultCode = (IStatusCodeActionResult)result;

            // ASSERT
            // I expect the "GetAlimentById()" method to have been called only once with any "<int>" data type
            alimentRepoMock.Verify(x => x.GetAlimentById(It.IsAny<int>()), Times.Once());

            // I expect the "UpdateAliment()" method to have been called only once with an "Aliment" object
            //alimentRepoMock.Verify(x => x.UpdateAliment(alimentMock.Object), Times.Once()); // asked once with aliment object
            alimentRepoMock.Verify(x => x.UpdateAliment(It.IsAny<Aliment>()), Times.Once()); // asked once with aliment object

            Assert.AreEqual(StatusCodes.Status204NoContent, resultCode.StatusCode);
        }

    }

    public class InstantiateMap
    {
        public InstantiateMap()
        {
        }
    }
}