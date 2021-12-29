using M.YZ.Basement.EndPoints.Web.Controllers;
using M.YZ.Basement.MiniBlog.Core.ApplicationServices.Writers.Commands.CreateWriters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace M.YZ.Basement.MiniBlog.EndPoints.Api.Controllers
{
    [Route("api/[Controller]")]
    public class WriterController : BaseController
    {
        [HttpPost("Save")]
        public async Task<IActionResult> Post([FromBody] CreateWiterCommand createWtirer)
        {
            return await Create<CreateWiterCommand, long>(createWtirer);
        }
    }
}
