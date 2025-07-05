using Microsoft.AspNetCore.Mvc;
using Bank.DTOs;
using Bank.Services;

namespace Bank.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountStatusController : ControllerBase
{
    private readonly ILogger<AccountStatusController> _logger;
    private readonly ITokenService _tokenService;
    private readonly IAccountService _accountService;
    private readonly IHistoryService _historyService;
    private readonly IDataCreditService _dataCreditService;

    public AccountStatusController(
        ILogger<AccountStatusController> logger,
        ITokenService tokenService,
        IAccountService accountService,
        IHistoryService historyService,
        IDataCreditService dataCreditService)
    {
        _logger = logger;
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        _historyService = historyService ?? throw new ArgumentNullException(nameof(historyService));
        _dataCreditService = dataCreditService ?? throw new ArgumentNullException(nameof(dataCreditService));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AccountStatusRequest request)
    {
        using var scope = _logger.BeginScope($"Processing account status for request: {request?.DocumentId}");
        
        try
        {           
            if (request == null)
            {
                _logger.LogWarning("Invalid request received (null request body)");
                return BadRequest("Invalid request");
            }

            // Get access token
            var token = await _tokenService.GetAccessTokenAsync();
            
            // Process the request here using the token
            var accounts = await _accountService.GetAccountStatusAsync(request, token);
            var history = await _historyService.GetCreditHistoryAsync(request, token);
            var dataCredit = await _dataCreditService.GetDataCreditAsync(request, token);

            // Rebuid response
            var response = new AccountStatusResponse()
            {
                Accounts = accounts,
                Credits = dataCredit.ListCredits,
                Footprints = dataCredit.Footprints,
                History = history.History,
                LastCredit = history.LastCredit,
                StatusLate = history.StatusLate,
                TotalAmountLate = history.TotalAmountLate,
                TotalDaysLate = history.TotalDaysLate
            };
            return Ok(response);
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError(httpEx, "HTTP request failed while processing account status for DocumentId: {DocumentId}", request?.DocumentId);
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Unable to connect to the authentication service. Please try again later.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error processing account status for DocumentId: {DocumentId}", request?.DocumentId);
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while processing your request.");
        }
    }
}
