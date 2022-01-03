using M.YZ.Basement.EndPoints.Web.Controllers;
using M.YZ.Basement.MiniBlog.Core.ApplicationServices.People.Commands.CreatePerson;
using M.YZ.Basement.MiniBlog.Core.ApplicationServices.People.Commands.TestExternal;
using M.YZ.Basement.Utilities.Services.MessageBus;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace M.YZ.Basement.MiniBlog.EndPoints.Api.Controllers
{
    [Route("api/[Controller]")]
    public class PeopleController : BaseController
    {
        private readonly ISendMessageBus _messageBus;

        public PeopleController(ISendMessageBus messageBus)
        {
            _messageBus = messageBus;
        }
        [HttpPost("TestEvent")]
        public async Task<IActionResult> TestEvent([FromBody] CreatePersonCommand createPerson)
        {

            return await Create<CreatePersonCommand, long>(createPerson);
        }

        [HttpGet("TestCommand")]
        public IActionResult TestCommand([FromQuery] string name)
        {
            _messageBus.SendCommandTo("MiniBlogService01", "TestCommand", new TestCommand
            {
                Name = name
            });

            return Ok("Command Sent");
        }
    }
}
