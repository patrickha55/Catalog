using AutoMapper;
using Catalog.Data.DTOs;
using Catalog.Data.Entities;
using Catalog.Repository.ItemRepositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(IItemRepository itemRepository, IMapper mapper, ILogger<ItemsController> logger)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> Get()
        {
            var items = await _itemRepository.GetItemsAsync();

            if (items is null)
            {
                _logger.LogError("There is no data. {MethodName}", nameof(Get));
                return NotFound("There is no data. Please try again!");
            }

            var itemDtos = _mapper.Map<IEnumerable<ItemDTO>>(items);

            return Ok(itemDtos);
        }

        [HttpGet("FindItemByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ItemDTO>> Get([FromQuery] string name)
        {
            if (name is null)
            {
                _logger.LogError("Invalid request in {MethodName} : {Request}", nameof(Get), name);
                return BadRequest("Invalid request. Please try again!");
            }

            var item = await _itemRepository.GetItemAsync(name);

            if (item is null)
            {
                _logger.LogError("No item in {MethodName} with the name: {Request}", nameof(Get), name);
                return NotFound("There is no item with the provided name. Please try again!");
            }

            var itemDto = _mapper.Map<ItemDTO>(item);

            return Ok(itemDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ItemDTO>> Create([FromBody] ManageItemDTO request)
        {
            if (request is null || !ModelState.IsValid)
            {
                _logger.LogError("Invalid request in {MethodName} : {@Request}", nameof(Create), request);
                return BadRequest("Invalid request. Please try again!");
            }

            var item = _mapper.Map<Item>(request);
            item.Id = new Guid();
            item.CreatedDate = DateTimeOffset.Now;

            await _itemRepository.CreateAsync(item);
            var itemDto = _mapper.Map<ItemDTO>(item);

            return CreatedAtAction(nameof(Get), new { name = itemDto.Name }, itemDto);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update([FromQuery] string name, [FromBody] ManageItemDTO request)
        {
            if (name is null)
            {
                _logger.LogError("Invalid request in {MethodName} : {Request}", nameof(Update), name);
                return BadRequest("Invalid request. Please try again!");
            }

            var item = await _itemRepository.GetItemAsync(name);

            if (item is null)
            {
                _logger.LogError("No item in {MethodName} with the name: {Request}", nameof(Update), name);
                return NotFound("There is no item with the provided name. Please try again!");
            }

            _mapper.Map(request, item);
            await _itemRepository.UpdateAsync(name, item);

            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete([FromQuery] string name)
        {
            if (name is null)
            {
                _logger.LogError("Invalid request in {MethodName} : {Request}", nameof(Delete), name);
                return BadRequest("Invalid request. Please try again!");
            }

            var item = await _itemRepository.GetItemAsync(name);

            if (item is null)
            {
                _logger.LogError("No item in {MethodName} with the name: {Request}", nameof(Delete), name);
                return NotFound("There is no item with the provided name. Please try again!");
            }

            await _itemRepository.DeleteAsync(name);

            return NoContent();
        }
    }
}