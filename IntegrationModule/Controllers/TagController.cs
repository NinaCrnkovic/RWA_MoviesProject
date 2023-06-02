using AutoMapper;
using BL.BLModels;
using BL.DALModels;
using BL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationModule.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public TagController(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<Tag>> GetAllTags()
        {
            try
            {
                var blTags = _tagRepository.GetAll();
                var tags = _mapper.Map<IEnumerable<Tag>>(blTags);
                return Ok(tags);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while fetching the data you requested: " + ex);
            }
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<Tag>> SearchTag(string searchPart)
        {
            try
            {
                var dbTags = _tagRepository.GetAll().Where(x => x.Name.ToLower().Contains(searchPart.ToLower()));
                var tags = _mapper.Map<IEnumerable<Tag>>(dbTags);
                return Ok(tags);
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while fetching the data you requested");
            }
        }

        [HttpGet("[action]/{id}")]
        public ActionResult<Tag> GetTagById(int id)
        {
            try
            {
                var blTag = _tagRepository.GetById(id);
                if (blTag == null)
                    return NotFound($"Could not find tag with id {id}");

                var tag = _mapper.Map<Tag>(blTag);
                return Ok(tag);
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while fetching the data you requested");
            }
        }

        [HttpPost("[action]")]
        public ActionResult<Tag> CreateTag(Tag tag)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                tag.Id = 0;
                var blTag = _mapper.Map<BLTag>(tag);
                var newTag = _tagRepository.Add(blTag);
                var createdTag = _mapper.Map<Tag>(newTag);

                return Ok(createdTag);
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while creating the tag.");
            }
        }

        [HttpPut("[action]/{id}")]
        public ActionResult<Tag> UpdateTag(int id, Tag tag)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var blTag = _mapper.Map<BLTag>(tag);
                var updatedTag = _tagRepository.Update(id, blTag);
                var modifiedTag = _mapper.Map<Tag>(updatedTag);

                return Ok(modifiedTag);
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while updating the tag.");
            }
        }

        [HttpDelete("[action]/{id}")]
        public ActionResult<Tag> Delete(int id)
        {
            try
            {
                var blTag = _tagRepository.GetById(id);
                if (blTag == null)
                    return NotFound($"Could not find tag with id {id}");

                _tagRepository.Delete(id);
                return Ok(blTag);
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while deleting the tag.");
            }
        }
    }

}

