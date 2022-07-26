using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OutlayManagerPortable.DTO;
using OutlayManagerPortable.Infraestructure.MessageBus.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OutlayManagerPortable.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionOutlayController : ControllerBase
    {
        private readonly ILogger<TransactionOutlayController> _logger;
        private readonly IMessageBusService _messageBusService;

        public TransactionOutlayController(ILogger<TransactionOutlayController> logger, IMessageBusService messageBusService)
        {
            _logger = logger;
            _messageBusService = messageBusService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<TransactionMessage>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(String), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> TransactionMessagesPublished()
        {
            try
            {
                _logger.LogInformation("Messages published");

                List<TransactionMessage> messagesList = await _messageBusService.MessagesQueuedAsync();
                return Ok(messagesList);
            }
            catch (Exception e)
            {
                _logger.LogError("Error recovering messages published: {error}", e.Message);
                return Problem(e.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(TransactionMessage), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(String), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PublishTransactionMessage([Required][FromBody] TransactionMessage transactionMessage)
        {
            try
            {
                _logger.LogInformation("Publish transaction to queue");

                TransactionMessage transactionMessageSaved = await _messageBusService.PublishMessageAsync(transactionMessage);
                return Ok(transactionMessageSaved);
            }
            catch (Exception e)
            {
                _logger.LogError("Error publish messages: {error}", e.Message);
                return Problem(e.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(TransactionMessage), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(String), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTransactionMessage([Required][FromBody] TransactionMessage transactionMessage)
        {
            try
            {
                _logger.LogInformation("Update transaction");

                TransactionMessage transactionMessageUpdated = await _messageBusService.UpdateMessageAsync(transactionMessage);
                return Ok(transactionMessageUpdated);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogError("Error updating messages published: Id not found:{id}", transactionMessage.Id);
                return NotFound(transactionMessage.Id);
            }
            catch (Exception e)
            {
                _logger.LogError("Error updating messages published: {error}", e.Message);
                return Problem(e.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{transactionMessageID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(String), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTransactionMessage([Required]Guid transactionMessageID)
        {
            try
            {
                _logger.LogInformation("Delete transaction");

                await _messageBusService.DeleteMessageAsync(transactionMessageID);
                return Ok();
            }
            catch(KeyNotFoundException)
            {
                _logger.LogError("Error delete messages: Message not found ID:{id}",transactionMessageID);
                return NotFound(transactionMessageID);
            }
            catch (Exception e)
            {
                _logger.LogError("Error delete messages: {error}", e.Message);
                return Problem(e.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
