using System;
using System.Threading;
using System.Threading.Tasks;
using Bank.Controllers;
using Bank.DTOs;
using Bank.Models;
using Bank.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Bank.Tests.Controllers;

public class AccountStatusControllerTests
{
    private readonly Mock<ILogger<AccountStatusController>> _mockLogger;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly Mock<IAccountService> _mockAccountService;
    private readonly Mock<IHistoryService> _mockHistoryService;
    private readonly Mock<IDataCreditService> _mockDataCreditService;
    private readonly AccountStatusController _controller;

    public AccountStatusControllerTests()
    {
        _mockLogger = new Mock<ILogger<AccountStatusController>>();
        _mockTokenService = new Mock<ITokenService>();
        _mockAccountService = new Mock<IAccountService>();
        _mockHistoryService = new Mock<IHistoryService>();
        _mockDataCreditService = new Mock<IDataCreditService>();

        _controller = new AccountStatusController(
            _mockLogger.Object,
            _mockTokenService.Object,
            _mockAccountService.Object,
            _mockHistoryService.Object,
            _mockDataCreditService.Object);
    }

    [Fact]
    public async Task Post_WithValidRequest_ReturnsOkResult()
    {
        // Arrange
        var request = new AccountStatusRequest
        {
            DocumentId = "11111",
            DocumentType = "1",
            CountryId = "1"
        };

        var authToken = "test-auth-token";
        var accountResponse = new AccountResponse[] { new() { AccountNumber = 12345, Status = "Active" } };
        var historyResponse = new CreditHistoryDTO { History = Array.Empty<Credit>() };
        var dataCreditResponse = new DataCreditResponse { ListCredits = Array.Empty<DataCreditDTO>(), Footprints = Array.Empty<Footprint>() };

        _mockTokenService.Setup(x => x.GetAccessTokenAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(authToken);

        _mockAccountService.Setup(x => x.GetAccountStatusAsync(request, authToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(accountResponse);

        _mockHistoryService.Setup(x => x.GetCreditHistoryAsync(request, authToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(historyResponse);

        _mockDataCreditService.Setup(x => x.GetDataCreditAsync(request, authToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dataCreditResponse);

        // Act
        var result = await _controller.Post(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<AccountStatusResponse>(okResult.Value);
        
        Assert.NotNull(response);
        Assert.Equal(accountResponse, response.Accounts);
        Assert.Equal(historyResponse.History, response.History);
        Assert.Equal(dataCreditResponse.ListCredits, response.Credits);
        Assert.Equal(dataCreditResponse.Footprints, response.Footprints);
    }

    [Fact]
    public async Task Post_WithNullRequest_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.Post(null!);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid request", badRequestResult.Value);
    }

    [Fact]
    public async Task Post_WhenTokenServiceFails_ReturnsUnauthorized()
    {
        // Arrange
        var request = new AccountStatusRequest();
        _mockTokenService.Setup(x => x.GetAccessTokenAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Token service error"));

        // Act
        var result = await _controller.Post(request);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Contains("An unexpected error occurred", statusCodeResult.Value?.ToString());
    }

    [Fact]
    public async Task Post_WhenAccountServiceFails_ReturnsInternalServerError()
    {
        // Arrange
        var request = new AccountStatusRequest();
        var authToken = "test-auth-token";

        _mockTokenService.Setup(x => x.GetAccessTokenAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(authToken);

        _mockAccountService.Setup(x => x.GetAccountStatusAsync(request, authToken, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Account service error"));

        // Act
        var result = await _controller.Post(request);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Contains("An unexpected error occurred", statusCodeResult.Value?.ToString());
    }

    [Fact]
    public async Task Post_WhenHistoryServiceFails_StillReturnsPartialData()
    {
        // Arrange
        var request = new AccountStatusRequest();
        var authToken = "test-auth-token";
        var accountResponse = new AccountResponse[] { new() };
        var dataCreditResponse = new DataCreditResponse { ListCredits = Array.Empty<DataCreditDTO>(), Footprints = Array.Empty<Footprint>() };

        _mockTokenService.Setup(x => x.GetAccessTokenAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(authToken);

        _mockAccountService.Setup(x => x.GetAccountStatusAsync(request, authToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(accountResponse);

        _mockHistoryService.Setup(x => x.GetCreditHistoryAsync(request, authToken, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("History service error"));

        _mockDataCreditService.Setup(x => x.GetDataCreditAsync(request, authToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dataCreditResponse);

        // Act
        var result = await _controller.Post(request);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Contains("An unexpected error occurred", statusCodeResult.Value?.ToString());
    }

    [Fact]
    public async Task Post_WhenDataCreditServiceFails_StillReturnsPartialData()
    {
        // Arrange
        var request = new AccountStatusRequest();
        var authToken = "test-auth-token";
        var accountResponse = new AccountResponse[] { new() };
        var historyResponse = new CreditHistoryDTO { History = Array.Empty<Credit>() };

        _mockTokenService.Setup(x => x.GetAccessTokenAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(authToken);

        _mockAccountService.Setup(x => x.GetAccountStatusAsync(request, authToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(accountResponse);

        _mockHistoryService.Setup(x => x.GetCreditHistoryAsync(request, authToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(historyResponse);

        _mockDataCreditService.Setup(x => x.GetDataCreditAsync(request, authToken, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Data credit service error"));

        // Act
        var result = await _controller.Post(request);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Contains("An unexpected error occurred", statusCodeResult.Value?.ToString());
    }
}
