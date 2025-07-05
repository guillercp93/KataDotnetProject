using System.Net;
using Microsoft.Extensions.Options;
using Bank.DTOs;
using Bank.Models;

namespace Bank.Services;

public class DataCreditService : IDataCreditService
{
    private readonly ILogger<DataCreditService> _logger;
    private readonly IRequestClient _requestClient;
    private readonly string _serviceUrl;

    public DataCreditService(
        ILogger<DataCreditService> logger,
        IRequestClient requestClient,
        IOptions<ServiceSettings> settings)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _requestClient = requestClient ?? throw new ArgumentNullException(nameof(requestClient));
        _serviceUrl = settings?.Value?.DataCreditServiceUrl ?? throw new ArgumentNullException(nameof(settings));
    }

    public async Task<DataCreditResponse> GetDataCreditAsync(
        AccountStatusRequest request, 
        string token, 
        CancellationToken cancellationToken = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        if (token == null) throw new ArgumentNullException(nameof(token));
        if (string.IsNullOrWhiteSpace(token)) throw new ArgumentException("Token cannot be empty.", nameof(token));

        _logger.LogInformation("Retrieving data credit for document: {DocumentId}", request.DocumentId);

        try
        {
            var dataCredit = await _requestClient.PostAsync<AccountStatusRequest, DataCredit>(
                _serviceUrl, request, token, cancellationToken) 
                ?? throw new InvalidOperationException("Received invalid data from the credit service.");

            var allCredits = GetAllCredits(dataCredit);
            
            return new DataCreditResponse
            {
                ListCredits = allCredits,
                Footprints = dataCredit.Footprints.ToArray()
            };
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogWarning(ex, "Data credit not found for document: {DocumentId}", request.DocumentId);
            throw new KeyNotFoundException("No credit data found for the specified document.", ex);
        }
        catch (Exception ex) when (ex is not ApplicationException and not KeyNotFoundException)
        {
            _logger.LogError(ex, "Error retrieving data credit for document: {DocumentId}", request.DocumentId);
            throw new ApplicationException("An error occurred while retrieving credit data. Please try again later.", ex);
        }
    }

    private DataCreditDTO[] GetAllCredits(DataCredit dataCredit)
    {
        return dataCredit.Credits.Select(ToDataCreditDto)
            .Concat(dataCredit.Mortgages.Select(ToDataCreditDto))
            .Concat(dataCredit.Savings.Select(ToDataCreditDto))
            .Concat(dataCredit.Cards.Select(ToDataCreditDto))
            .ToArray();
    }

    private DataCreditDTO ToDataCreditDto(CreditAccount credit) => new()
    {
        AccountNumber = credit.AccountNumber,
        CreditType = credit.CreditType,
        Status = credit.Status
    };

    private DataCreditDTO ToDataCreditDto(MortgageAccount mortgage) => new()
    {
        AccountNumber = mortgage.AccountNumber,
        CreditType = mortgage.CreditType,
        Status = mortgage.Status
    };

    private DataCreditDTO ToDataCreditDto(SavingsAccount savings) => new()
    {
        AccountNumber = savings.AccountNumber,
        CreditType = savings.CreditType,
        Status = savings.Status
    };

    private DataCreditDTO ToDataCreditDto(CardAccount card) => new()
    {
        AccountNumber = card.AccountNumber,
        CreditType = card.CreditType,
        Status = card.Status,
        CreditLimit = card.CreditLimit,
        Franchise = card.Franchise
    };
}