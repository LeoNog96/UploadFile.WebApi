using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UploadFile.WebApi.DTOs;
using UploadFile.WebApi.Models;
using UploadFile.WebApi.Services.Interfaces;

namespace UploadFile.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<object>> PostLogin(LoginDTO login)
        {
            try
            {
                return await _service.Auth(login);
            }
            catch(Exception)
            {
                return Unauthorized("Usuario invalido");
            }
        }

        [HttpGet ("{id:long}", Name = "GetUser")]
        public async Task<ActionResult<User>> Get(long id)
        {
            var user = await _service.Get(id);

            return user ?? (ActionResult<User>)NotFound();
        }

        [HttpGet ("all")]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            var users = await _service.GetAll();

            if (users.Count == 0)
            {
                return NotFound();
            }

            return users;
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post(User user)
        {
            var newUser = await _service.Save(user);

            if (newUser == null)
            {
                return BadRequest();
            }

            return CreatedAtRoute ("GetUser", new { id = newUser.Id }, newUser);
        }

        [HttpPut]
        public async Task<ActionResult> Put(User user)
        {
            await _service.Update(user);

            return NoContent (); 
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> Delete(long id)
        {
            await _service.Delete(id);

            return NoContent ();
        }
    }
}