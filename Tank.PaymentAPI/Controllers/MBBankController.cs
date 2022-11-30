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
    public class MBBankController : ControllerBase
    {
        private readonly IMBBankRepository _mbbankRepository;

        public MBBankController(IMBBankRepository mbbankRepository)
        {
            _mbbankRepository = mbbankRepository;
        }

        [HttpGet("sendcode/{amount}")]
        public IActionResult GetPaymentCode(int amount)
        {
            try
            {
                string code = _mbbankRepository.GeneratePaymentCode(amount).Result;
                if (code.Contains("false"))
                    return StatusCode(StatusCodes.Status404NotFound, code);
                return Ok(code);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpGet("payment/{userName}&{code}&{serverid}")]
        public IActionResult GetPayment(string userName, string code, int serverid)
        {
            try
            {
                return Ok(_mbbankRepository.PaymentState(userName, code, serverid)); 
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }
    }
}
