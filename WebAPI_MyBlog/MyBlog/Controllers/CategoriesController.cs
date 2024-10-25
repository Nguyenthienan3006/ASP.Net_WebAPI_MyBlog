using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MyBlog.Dto;
using MyBlog.Interfaces;
using MyBlog.Models;
using MyBlog.Repository;


namespace MyBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet("{categoryId}/posts")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Post>))]
        public IActionResult GetPostByCategoryId(int categoryId)
        {
            if (!_categoryRepository.CategoryExist(categoryId))
            {
                return NotFound("Category not found!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var postMapped = _mapper.Map<List<PostDto>>(_categoryRepository.GetPostByCategoryId(categoryId));

            return Ok(postMapped);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(_categoryRepository.GetCategories().Count() <= 0)
            {
                return NotFound("There is no category!");
            }

            var categories = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());

            return Ok(categories);
        }

        [HttpGet("{cateId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int cateId)
        {
            if(!_categoryRepository.CategoryExist(cateId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryMapped = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(cateId));

            return Ok(categoryMapped);
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryToAdd)
        {
            if(categoryToAdd == null)
            {
                return BadRequest(ModelState);
            }

            var categoryExist = _categoryRepository.GetCategories().
                Where(c => c.Name.Trim().ToLower() == categoryToAdd.Name.TrimEnd().ToLower()).FirstOrDefault();
                
            if(categoryExist != null)
            {
                ModelState.AddModelError("", "Category already exists!");
                return StatusCode(500, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryMapped = _mapper.Map<Category>(categoryToAdd);

            if (!_categoryRepository.CreateCategory(categoryMapped))
            {
                ModelState.AddModelError("", "Something went wrong while saving!");
                return StatusCode(500, ModelState);
            }

            return Ok("Create successfully");
        }

        [HttpPut("{cateId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int cateId, [FromBody] CategoryDto categoryToUpdate)
        {
            if(categoryToUpdate == null)
            {
                return BadRequest(ModelState);
            }

            if (cateId != categoryToUpdate.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_categoryRepository.CategoryExist(cateId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryMapped = _mapper.Map<Category>(categoryToUpdate);

            if (!_categoryRepository.UpdateCategory(categoryMapped))
            {
                ModelState.AddModelError("", "Something went wrong while saving!");
                return StatusCode(500, ModelState);
            }

            return Ok("Update successfully");
        }

        [HttpDelete("{cateId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int cateId) 
        {
            if (!_categoryRepository.CategoryExist(cateId))
            {
                return NotFound();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryToDelete = _categoryRepository.GetCategory(cateId);

            if (!_categoryRepository.DeleteCategory(categoryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
                return StatusCode(500, ModelState);
            }

            return Ok("Delete Successfully");
        }

    }
}
