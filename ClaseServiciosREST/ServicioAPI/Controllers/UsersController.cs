using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicioAPI.Data.Entities;
using ServicioAPI.Data.Repositories;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServicioAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly ImageRepository _imageRepository;

        public UsersController(UserRepository userRepository, ImageRepository imageRepository)
        {
            _userRepository = userRepository;
            _imageRepository = imageRepository;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userRepository.GetAll();

            var usersData = users.Select(u => new
            {
                Id = u.Id,
                Nickname = u.Nickname,
                Name = u.Name,
                UrlPage = u.UrlPage
            });

            return Ok(usersData);
        }

        [HttpGet("{nickname}")]
        public async Task<IActionResult> GetUserByNickname(string nickname)
        {
            UserProfile userProfile = null;
            User user = await _userRepository.GetUserByNicknameAsync(nickname);
            
            if (user != null) userProfile = CastUserToUserProfile(user);

            return Ok(userProfile);
        }

        [HttpPost("signIn")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
        {
            if (string.IsNullOrEmpty(request.Name)) return BadRequest("Nombre vacío");
            if (string.IsNullOrEmpty(request.Nickname)) return BadRequest("Nickname vacío");
            if (string.IsNullOrEmpty(request.Password)) return BadRequest("Contraseña vacía");

            if (await _userRepository.GetUserByNicknameAsync(request.Nickname) != null)
                return BadRequest("Nickname no disponible");

            string hashPassword = CalculateHash(request.Password);

            User user = new User()
            {
                Name = request.Name,
                Nickname = request.Nickname,
                Password = hashPassword
            };

            User newUser = await _userRepository.CreateAsync(user);

            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Nickname)) return BadRequest("Nickname vacío");
            if (string.IsNullOrEmpty(request.Password)) return BadRequest("Contraseña vacía");

            User user = await _userRepository.GetUserByNicknameAsync(request.Nickname);
            string hashPassword = CalculateHash(request.Password);

            if (user == null || user.Password != hashPassword) 
                return BadRequest("Nickname o contraseña incorrecto");


            user.Token = CreateToken();
            user = await _userRepository.UpdateAsync(user);

            return Ok(user.Token);
        }

        
        [HttpPut]
        public async Task<IActionResult> PutUserBody([FromBody] UserEditRequest userEdit)
        {
            return await PutUser(userEdit);
        }

        [HttpPut("image")]
        public async Task<IActionResult> PutUserForm([FromForm] IFormFile image)
        {
            return await PutUser(new UserEditRequest() { Image = image });
        }

        private async Task<IActionResult> PutUser(UserEditRequest userEdit)
        {
            if (!Request.IsAuthorized(_userRepository, out User user)) return Unauthorized();

            if (!string.IsNullOrEmpty(userEdit.Name)) user.Name = userEdit.Name;
            if (!string.IsNullOrEmpty(userEdit.Password)) user.Password = userEdit.Password;
            if (userEdit.Description != null) user.Description = userEdit.Description;
            if (userEdit.UrlPage != null) user.UrlPage = userEdit.UrlPage;
            if (userEdit.Image != null)
            {
                await _imageRepository.DeleteAsync(user.Image);

                MemoryStream auxStream = new MemoryStream();
                await userEdit.Image.CopyToAsync(auxStream);

                user.Image = new Image
                {
                    Format = Path.GetExtension(userEdit.Image.FileName),
                    Data = auxStream.ToArray()
                };
            }

            await _userRepository.UpdateAsync(user);

            return Ok(CastUserToUserProfile(user));
        }


        private string CalculateHash(string password)
        {
            StringBuilder sb = new StringBuilder();

            using (HashAlgorithm algorithm = SHA256.Create())
            {
                byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(password));

                foreach (byte b in hash)
                    sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }

        private string CreateToken()
        {
            string token;
            User user;

            do
            {
                token = Guid.NewGuid().ToString();
                user = _userRepository.GetUserByTokenAsync(token).Result;
            } 
            while (user != null);

            return token;
        }

        private UserProfile CastUserToUserProfile(User user)
        {
            UserProfile userProfile = new UserProfile()
            {
                Id = user.Id,
                Nickname = user.Nickname,
                Name = user.Name,
                Description = user.Description,
                UrlPage = user.UrlPage
            };

            if (user.Image != null)
            {
                userProfile.Image = Url.AbsoluteContent($"~/images/{user.Image.Id}");
            }

            return userProfile;
        }
    }
}
