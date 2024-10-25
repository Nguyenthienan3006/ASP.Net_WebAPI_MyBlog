using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Dto;
using MyBlog.Interfaces;
using MyBlog.Models;

namespace MyBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentController(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Comment>))]
        public IActionResult GetComments()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var commentMapped = _mapper.Map<List<CommentDto>>(_commentRepository.GetComments());

            return Ok(commentMapped);
        }

        [HttpGet("{commentId}")]
        [ProducesResponseType(200, Type = typeof(Comment))]
        [ProducesResponseType(400)]
        public IActionResult GetComment(int commentId)
        {
            if (!_commentRepository.CommentExist(commentId))
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var commentMapped = _mapper.Map<CommentDto>(_commentRepository.GetComment(commentId));

            return Ok(commentMapped);
        }

        [HttpDelete("{commentId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteComment(int commentId)
        {
            if (!_commentRepository.CommentExist(commentId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var commentToDelete = _commentRepository.GetComment(commentId);

            if (!_commentRepository.DeleteComment(commentToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting comment");
                return StatusCode(500, ModelState);
            }

            return Ok("Delete Successfully");
        }
    }
}
