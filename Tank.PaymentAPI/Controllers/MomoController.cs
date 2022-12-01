using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Tank.PaymentAPI.Datas;
using Tank.PaymentAPI.Helpers;
using Tank.PaymentAPI.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using Tank.PaymentAPI.Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Tank.PaymentAPI.Interfaces.IRepository;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace Tank.PaymentAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MomoController : ControllerBase
    {
        private readonly IMomoRepository _momoRepository;

        public MomoController(IMomoRepository momoRepository)
        {
            _momoRepository = momoRepository;
        }

        [HttpGet("payment/{userName}&{tranid}&{serverid}")]
        public IActionResult GetPayment(string userName, long tranid, int serverid)
        {
            try
            {
                return Ok(_momoRepository.PaymentState(userName, tranid, serverid)); 
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }
    }
}
