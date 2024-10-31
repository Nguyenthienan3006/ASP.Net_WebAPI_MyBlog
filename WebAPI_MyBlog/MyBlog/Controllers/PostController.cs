using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MyBlog.Dto;
using MyBlog.Interfaces;
using MyBlog.Models;
using System.Security.Claims;

namespace MyBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;

        public PostController(IPostRepository postRepository, IMapper mapper, IUserRepository userRepository, ICategoryRepository categoryRepository)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Post>))]

        public IActionResult GetPosts()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var posts = _postRepository.GetPosts().Select(post => new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                Author = (_userRepository.GetUser(post.UsersId) as User).UserName,
                CommentNumber = _postRepository.GetCommentOfPost(post.Id).Count(),
                PostCategory = _categoryRepository.GetPostCategoryName(post.Id),
            });
            

            return Ok(posts);
        }

        [HttpGet("{postId}")]
        [ProducesResponseType(200, Type = typeof(Post))]
        [ProducesResponseType(400)]
        [Authorize]
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
            var post = _postRepository.GetPost(postId);
            var postMapped = _mapper.Map<PostDto>(post);
            postMapped.Author = (_userRepository.GetUser(post.UsersId) as User).UserName;
            postMapped.CommentNumber = _postRepository.GetCommentOfPost(postId).Count();

            return Ok(postMapped);

        }

        [HttpGet("{postId}/comments")]
        [ProducesResponseType(200, Type = typeof(Comment))]
        [ProducesResponseType(400)]
        [Authorize]
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
        [Authorize]
        public IActionResult CreatePost([FromBody] PostDto postToAdd)
        {
            if(postToAdd  == null)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("Invalid user");
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
            postMapped.UsersId = int.Parse(userId);

            if (!_postRepository.CreatePost(postMapped))
            {
                ModelState.AddModelError("", "Something went wrong while saving!");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Successfully");
        }

        [HttpPut("{postId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [Authorize]
        public IActionResult UpdatePost(int postId, [FromBody] PostDto postToUpdate)
        {
            if(postToUpdate == null)
            {
                return BadRequest(ModelState);
            }

            if(postId != postToUpdate.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_postRepository.PostExist(postId))
            {
                return NotFound();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var postMapped = _mapper.Map<Post>(postToUpdate);
            postMapped.UsersId = int.Parse(userId);

            if (!_postRepository.UpdatePost(postMapped))
            {
                ModelState.AddModelError("", "Something went wrong while saving!");
                return StatusCode(500, ModelState);
            }

            return Ok("Update Successfully");
        }

        [HttpDelete("{postId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [Authorize]
        public IActionResult DeletePost(int postId)
        {
            if (!_postRepository.PostExist(postId))
            {
                return NotFound();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(!_postRepository.DeletePost(_postRepository.GetPost(postId)))
            {
                ModelState.AddModelError("", "Something went wrong while saving!");
                return StatusCode(500, ModelState);
            }


            return Ok("Delete Successfully");
        }
    }
}
