using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OneNet.PubSub.Server.Application.DTOs;
using OneNet.PubSub.Server.Application.Exceptions;
using OneNet.PubSub.Server.Application.Repository;
using OneNet.PubSub.Server.Application.Services;

namespace OneNet.PubSub.Server.Infrastructures.Api.Controllers
{
    [ApiController]
    [Route("api/topic")]
    public class TopicApiController : Controller
    {
        private readonly ITopicSearchingService _topicSearchingService;

        public TopicApiController(ITopicSearchingService topicSearchingService)
        {
            _topicSearchingService = topicSearchingService;
        }
        [Route("search")]
        public async Task<IActionResult> SearchTopic([FromQuery] SearchTopicRequest request)
        {
            var res = await _topicSearchingService.Search(request);
            return Ok(ApiResponse.CreateSuccess(res));
        }

        [Route("get-by-name")]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            var res = await _topicSearchingService.GetByName(name);
            return Ok(ApiResponse.CreateSuccess(res));
        }
    }
}