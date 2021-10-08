using api.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using api.Services;
using System;
using System.Threading.Tasks;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using api.Helpers;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IFarmieRepository _farmieRepository;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UserController(IFarmieRepository farmieRepository, IMapper mapper,IOptions<AppSettings> appSettings)
        {
            _farmieRepository = farmieRepository ??
                throw new ArgumentNullException(nameof(farmieRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _appSettings=appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserAutenticateDto user)
        {
            var userFromRepo = _farmieRepository.Authenticate(user.Username, user.Password);
            if (userFromRepo == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            string Role=null;
            if (userFromRepo.FarmerFlag == true)
                Role = "Farmer";
            else if (userFromRepo.AdministratorFlag == true)
                Role = "Administrator";
            else if (userFromRepo.WorkerFlag == true)
                Role = "Radnik";
            else if(userFromRepo.BuyerFlag == true)
                Role = "Kupac";

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userFromRepo.Id.ToString()),
                    new Claim(ClaimTypes.Role, Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok(

                new {
                        Id = userFromRepo.Id,
                        Name = userFromRepo.Name,
                        Surname = userFromRepo.Surname,
                        PhoneNumber = userFromRepo.PhoneNumber,
                        EMail = userFromRepo.EMail,
                        FarmerFlag = userFromRepo.FarmerFlag,
                        AdministratorFlag = userFromRepo.AdministratorFlag,
                        WorkerFlag = userFromRepo.WorkerFlag,
                        BuyerFlag = userFromRepo.BuyerFlag,
                        Description = userFromRepo.Description,
                        Username = userFromRepo.Username,
                        Token = tokenString
                    }
            );
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDto user)
        {
            var userEntity = _mapper.Map<User>(user);

            try
            {
                _farmieRepository.RegisterUser(userEntity, user.Password);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        } 

        [Authorize(Roles="Administrator")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var usersFromRepo = await _farmieRepository.GetUsersAsync();
            return Ok(_mapper.Map<IEnumerable<GetUserDto>>(usersFromRepo));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task AddUser(User user)
        {
            if(_farmieRepository.AddUserAsync(user))
            {
                await _farmieRepository.SaveChangesAsync();
            }
        }

        [AllowAnonymous]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var userFromRepo = await _farmieRepository.GetUserAsync(userId);
            if(userFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<GetUserDto>(userFromRepo));
        }

        [AllowAnonymous]
        [HttpPut]
        [Route("{userId}/update")]
        public async Task<IActionResult> UpdateUserAuth(int userId, [FromBody] UserUpdateDto user)
        {
            if(!_farmieRepository.UserExist(userId))
                return NotFound();
            var userEntity = _mapper.Map<User>(user);
            userEntity.Id=userId;
            _farmieRepository.UpdateUser(userEntity,user.Password);
            await _farmieRepository.SaveChangesAsync();
            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, PutUserDto UserForUpdate)
        {
            if (!_farmieRepository.UserExist(userId))
                return NotFound();
            
            var userFromRepo = await _farmieRepository.GetUserAsync(userId);
            _mapper.Map(UserForUpdate, userFromRepo);

            await _farmieRepository.SaveChangesAsync();

            return Ok();
        }

        [AllowAnonymous]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            if(!_farmieRepository.UserExist(userId))
                return NotFound();


            var userForDelete = await _farmieRepository.GetUserAsync(userId);
            
            _farmieRepository.DeleteUser(userForDelete);
            await  _farmieRepository.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet("extradata/{userId}")]
        public async Task<IActionResult> GetFarmerExtraData(int userId)
        {
            if(!_farmieRepository.FarmerExist(userId))
                return NotFound();
            return Ok(await _farmieRepository.GetFarmerExtraData(userId));   
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet("unfinishedTasks/{userId}")]
        public async Task<IActionResult> GetUnFinishedTasks(int userId)
        {
            if(!_farmieRepository.FarmerExist(userId))
               return NotFound();

            var tasks = await _farmieRepository.GetUnfinishedTasksForFarmer(userId);
            return Ok(_mapper.Map<IEnumerable<GetWorkingTaskDtoSpecial>>(tasks));
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet]
        [Route("openSeasons/{userId}")]
        public async Task<IActionResult> GetAllPossessionsWithOpenSeason(int userId)
        {
            if(!_farmieRepository.FarmerExist(userId))
                return NotFound();
                
            var possessionsWithOpenSeasons=await _farmieRepository.GetAllPossessionsWithOpenSeason(userId);
            return Ok(possessionsWithOpenSeasons);
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet("{userId}/statisticsGlobal")]
        public async Task<IActionResult> GetStatistics(int userId)
        {
            if(!_farmieRepository.FarmerExist(userId))
                return NotFound();

            return Ok(await _farmieRepository.GetStatisticsForFarmer(userId));
        }
    }
}