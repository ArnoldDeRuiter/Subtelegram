using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Subtelegram.Services;

namespace Subtelegram.Controllers
{
    [Route("api/[controller]")]
    public class UpdateController : Controller
    {
        readonly IUpdateService _updateService;
        readonly BotConfiguration _config;

        public UpdateController(IUpdateService updateService, BotConfiguration config)
        {
            _updateService = updateService;
            _config = config;
        }

        // POST api/update
        [HttpPost]
        public void Post([FromBody]Update update)
        {
            _updateService.HandleUpdate(update);
        }
    }
}