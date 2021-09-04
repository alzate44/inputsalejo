using inputsalejo.Common.Models;
using inputsalejo.Functions.Entities;
using inputsalejo.Functions.Functions;
using inputsalejo.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using Xunit;

namespace inputsalejo.Tests.Tests
{
    public class InputApiTest
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void CreateInput_ShouldReturn_200()
        {
            //Arrenge

            MockCloudTableInputs mockInputs = new MockCloudTableInputs(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Input inputRequest = TestFactory.GetInputRequest();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(inputRequest);

            //Act

            IActionResult response = await InputApi.CreateInput(request, mockInputs, logger);

            //Assert

            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

        }

        [Fact]
        public async void UpdateInput_ShouldReturn_200()
        {
            //Arrenge

            MockCloudTableInputs mockInputs = new MockCloudTableInputs(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Input inputRequest = TestFactory.GetInputRequest();
            Guid inputId = Guid.NewGuid();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(inputId, inputRequest);

            //Act

            IActionResult response = await InputApi.UpdateInput(request, mockInputs, inputId.ToString(), logger);

            //Assert

            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

        }


        [Fact]
        public  void GetInputById_ShouldReturn_200()
        {
            //Arrenge

            MockCloudTableInputs mockInputs = new MockCloudTableInputs(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Input inputRequest = TestFactory.GetInputRequest();
            InputEntity inputEntity = new InputEntity();
            Guid inputId = Guid.NewGuid();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(inputId, inputRequest);

            //Act

            IActionResult response =  InputApi.GetInputById(request, inputEntity, inputId.ToString(), logger);

            //Assert

            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

        }

        [Fact]
        public async void DeleteInput_ShouldReturn_200()
        {
            //Arrenge

            MockCloudTableInputs mockInputs = new MockCloudTableInputs(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Input inputRequest = TestFactory.GetInputRequest();
            InputEntity inputEntity = new InputEntity();
            //TableEntity tableEntity = new TableEntity();
            Guid inputId = Guid.NewGuid();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(inputId, inputRequest);

            //Act

            IActionResult response = await InputApi.DeleteInput(request, inputEntity, mockInputs, inputId.ToString(), logger);

            //Assert

            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

        }

        [Fact]
        public async void GetAllInputs_ShouldReturn_200()
        {
            //Arrenge

            MockCloudTableInputs mockInputs = new MockCloudTableInputs(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Input inputRequest = TestFactory.GetInputRequest();
            Guid inputId = Guid.NewGuid();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(inputId, inputRequest);

            //Act

            IActionResult response = await InputApi.GetAllInputs(request, mockInputs, logger);

            //Assert

            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

        }

    }
}
