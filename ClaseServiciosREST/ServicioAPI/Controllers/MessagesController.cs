using Common;
using Microsoft.AspNetCore.Mvc;
using ServicioAPI.Data.Entities;
using ServicioAPI.Data.Repositories;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ServicioAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly MessageRepository _messageRepository;
        private readonly ImageRepository _imageRepository;
        private readonly UserRepository _userRepository;

        public MessagesController(MessageRepository messageRepository, ImageRepository imageRepository, UserRepository userRepository)
        {
            _messageRepository = messageRepository;
            _imageRepository = imageRepository;
            _userRepository = userRepository;
        }

        [HttpGet("receiver/{receiverNickname}")]
        public async Task<IActionResult> GetMessagesByReceiver(string receiverNickname)
        {
            IActionResult result;

            User receiver = await _userRepository.GetUserByNicknameAsync(receiverNickname);

            if (receiver == null)
            {
                result = BadRequest("No existe ese destinatario");
            }
            else
            {
                var messages = _messageRepository.GetMessagesByReceiver(receiver);
                result = Ok(messages.Select(CastToMessageResponse));
            }

            return result;
        }

        [HttpGet("author/{authorNickname}")]
        public async Task<IActionResult> GetMessagesByAuthor(string authorNickname)
        {
            IActionResult result;

            User author = await _userRepository.GetUserByNicknameAsync(authorNickname);

            if (author == null)
            {
                result = BadRequest("No existe ese autor");
            }
            else
            {
                var messages = _messageRepository.GetMessagesByAuthor(author);
                result = Ok(messages.Select(CastToMessageResponse));
            }

            return result;
        }

        [HttpPost]
        public async Task<IActionResult> PostMessageBody([FromBody] MessageRequest message)
        {
            return await PostMessage(message);
        }

        [HttpPost("image")]
        public async Task<IActionResult> PostMessageForm([FromForm] MessageRequest message)
        {
            return await PostMessage(message);
        }

        private async Task<IActionResult> PostMessage(MessageRequest message)
        {
            IActionResult result;
            User author;

            if (!Request.IsAuthorized(_userRepository, out author))
            {
                result = Unauthorized();
            }
            else if (string.IsNullOrEmpty(message.Text) && message.Image == null)
            {
                result = Ok("Mensaje vacío");
            }
            else
            {
                User receiver = await _userRepository.GetUserByNicknameAsync(message.Receiver);

                if (receiver == null)
                {
                    result = BadRequest("No existe el destinatario");
                }
                else
                {
                    var entityMessage = new Message
                    {
                        Author = author,
                        Receiver = receiver,
                        Text = message.Text,
                        CreatedTime = DateTime.UtcNow
                    };

                    if (message.Image != null)
                    {
                        MemoryStream auxStream = new MemoryStream();
                        await message.Image.CopyToAsync(auxStream);

                        entityMessage.Image = new Image
                        {
                            Format = Path.GetExtension(message.Image.FileName),
                            Data = auxStream.ToArray()
                        };
                    }

                    Message newMessage = await _messageRepository.CreateAsync(entityMessage);

                    MessageResponse response = CastToMessageResponse(newMessage);

                    result = Created("", response);
                }
            }

            return result;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage(int id, [FromBody] MessageEditRequest message)
        {
            IActionResult result;

            Message editMessage = await _messageRepository.GetByIdAsync(id);

            if (editMessage == null)
            {
                result = BadRequest($"No hay mensaje con id {id}");
            }
            else if (!Request.IsAuthorized(_userRepository, out User user) || editMessage.Author.Id != user.Id)
            {
                result = Unauthorized();
            }
            else
            {
                editMessage.Text = message.Text;
                editMessage.EditedTime = DateTime.UtcNow;
                editMessage = await _messageRepository.UpdateAsync(editMessage);
                MessageResponse response = CastToMessageResponse(editMessage);

                result = Ok(response);
            }

            return result;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            IActionResult result;

            Message message = await _messageRepository.GetByIdAsync(id);

            if (message == null)
            {
                result = BadRequest($"No hay mensaje con id {id}");
            }
            else if (!Request.IsAuthorized(_userRepository, out User user) || message.Author.Id != user.Id)
            {
                result = Unauthorized();
            }
            else
            {
                await _imageRepository.DeleteAsync(message.Image);
                await _messageRepository.DeleteAsync(message);
                result = NoContent();
            }

            return result;
        }
        
        private MessageResponse CastToMessageResponse(Message message)
        {
            MessageResponse response = new MessageResponse()
            {
                Id = message.Id,
                Text = message.Text,
                Author = message.Author?.Nickname,
                Receiver = message.Receiver?.Nickname,
                CreatedTime = message.CreatedTime,
                EditedTime = message.EditedTime
            };

            if (message.Image != null)
            {
                response.Image = Url.AbsoluteContent($"~/images/{message.Image.Id}");
            }

            return response;
        }
    }
}
