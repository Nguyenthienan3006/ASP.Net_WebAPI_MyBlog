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
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public PostController(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Post>))]
        public IActionResult GetPosts()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var posts = _mapper.Map<List<PostDto>>(_postRepository.GetPosts());

            return Ok(posts);
        }

        [HttpGet("{postId}")]
        [ProducesResponseType(200, Type = typeof(Post))]
        [ProducesResponseType(400)]
        public IActionResult GetPost(int postId)
        {
            if (!_postRepository.PostExist(postId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var postMapped = _mapper.Map<PostDto>(_postRepository.GetPost(postId));
            return Ok(postMapped);

        }

        [HttpGet("{postId}/comments")]
        [ProducesResponseType(200, Type = typeof(Comment))]
        [ProducesResponseType(400)]
        public IActionResult GetCommentOfPost(int postId)
        {
            if (!_postRepository.PostExist(postId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var commentMapped = _mapper.Map<List<CommentDto>>(_postRepository.GetCommentOfPost(postId));

            return Ok(commentMapped);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePost([FromBody] PostDto postToAdd)
        {
            if(postToAdd  == null)
            {
                return BadRequest(ModelState);
            }

            var post = _postRepository.GetPosts().
                Where(p => p.Title.Trim().ToLower() == postToAdd.Title.TrimEnd().ToLower()).FirstOrDefault();

            if(post != null)
            {
                ModelState.AddModelError("", "Post tittle already exists!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var postMapped = _mapper.Map<Post>(postToAdd);

            if (!_postRepository.CreatePost(postMapped))
            {
                ModelState.AddModelError("", "Something went wrong while saving!");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Successfully");
        }
    }
}
