﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OneNet.PubSub.Server.Application.DTOs;
using OneNet.PubSub.Server.Application.Exceptions;
using OneNet.PubSub.Server.Application.Repository;

namespace OneNet.PubSub.Server.Infrastructures.Api.Controllers
{
    [ApiController]
    [Route("api/topic")]
    public class TopicApiController : Controller
    {
        private readonly ITopicRepository _topicRepository;

        public TopicApiController(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        [Route("search")]
        public async Task<IActionResult> SearchTopic([FromQuery] FindTopicRequest request)
        {
            var topics = await _topicRepository.Search(request.Name);
            var rs = topics.Select(tp => new TopicDTO(tp))
                .ToList();
            return Ok(new ApiResponse()
            {
                Status = 0,
                Data = rs
            });
        }

        [Route("get-by-name")]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            var topic = await _topicRepository.GetByName(name);
            if (topic == null)
                throw new NotFoundTopicException(name);
            var topicDto = new TopicDTO(topic);
            return Ok(ApiResponse.CreateSuccess(topicDto));
        }
    }
}