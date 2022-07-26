using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OutlayManagerPortable.DTO;
using OutlayManagerPortable.Infraestructure.Data.Abstraction;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutlayManagerPortable.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataMasterController : ControllerBase
    {
        private readonly ILogger<DataMasterController> _logger;
        private readonly IMasterData _masterDataService;

        public DataMasterController(ILogger<DataMasterController> logger, IMasterData masterDataService)
        {
            _logger = logger;
            _masterDataService = masterDataService;
        }

        [HttpGet("TransactionType")]        
        [ProducesResponseType(typeof(List<TransactionType>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(String), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> TransactionTypes()
        {
            try
            {
                _logger.LogInformation("Recovering transaction types");

                IEnumerable<TransactionType> transactionTypes = await _masterDataService.TransactionsTypes();
                return Ok(transactionTypes);
            }
            catch (Exception e)
            {
                _logger.LogError("Error recovering transaction types: {error}", e.Message);
                return Problem(e.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("TransactionCode")]
        [ProducesResponseType(typeof(List<TransactionCode>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(String), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> TransactionCodes()
        {
            try
            {
                _logger.LogInformation("Recovering transaction codes");

                IEnumerable<TransactionCode> transactionCodes = await _masterDataService.TransactionsCodes();
                return Ok(transactionCodes);
            }
            catch (Exception e)
            {
                _logger.LogError("Error recovering transactions codes: {error}", e.Message);
                return Problem(e.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
