using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Avaliacao.API.DTOs.Response;
using Projeto.Avaliacao.API.Models;
using Projeto.Avaliacao.API.Repository;
using System.Net;

namespace Projeto.Avaliacao.API.Controllers
{
    [Route("api/fazenda")]
    [ApiController]
    public class FazendaController : CustomControllerBase
    {
        private readonly DefaultContext _context;
        private readonly IMapper _mapper;
        public FazendaController(DefaultContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// list of Farm.
        /// </summary>
        /// <returns>Lists all Devices entity records</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IList<FazendaResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var fazendas = await _context.Fazendas
                    .Where(x => x.DeletedAt == null)
                    .ToListAsync();

                var result = _mapper.Map<List<FazendaResponseDto>>(fazendas);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Verifies if new or modified entity complies with business rules.
        /// </summary>
        [NonAction]
        protected async Task<bool> Validate(Fazenda item)
        {
            ValidateModelState<Fazenda>(item);

            if (string.IsNullOrEmpty(item.Name) || string.IsNullOrWhiteSpace(item.Name))
                this.BusinessValidation.AddError("Name is required");

            return await Task.FromResult(this.BusinessValidation.IsValid);
        }

        [HttpPost]
        [ProducesResponseType(typeof(FazendaResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Save([FromBody] FazendaResponseDto data)
        {
            try
            {
                var toAdd = new Fazenda();
                toAdd.CreatedAt = DateTime.UtcNow;
                
                _context.Entry<Fazenda>(toAdd).CurrentValues.SetValues(data.ToSetValuesModel());

                if (!await Validate(toAdd))
                    return new UnprocessableEntityObjectResult(this.BusinessValidation);


                _context.Fazendas.Add(toAdd);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<FazendaResponseDto>(toAdd);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(FazendaResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromRoute] long id, [FromBody] FazendaResponseDto data)
        {
            try
            {
                data.Id = id;

                var existingItem = await _context.Fazendas                    
                    .Where(r => r.Id == data.Id)
                    .FirstOrDefaultAsync();

                if (existingItem == null)
                    return NotFound();

                _context.Entry<Fazenda>(existingItem).CurrentValues.SetValues(data.ToSetValuesModel());
                existingItem.UpdatedAt = DateTime.UtcNow;
                
                if (!await Validate(existingItem))
                    return new UnprocessableEntityObjectResult(this.BusinessValidation);

                _context.Fazendas.Update(existingItem);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<FazendaResponseDto>(existingItem);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Remove([FromBody] List<long> ids)
        {
            try
            {
                await base.SoftRemove<Fazenda>(_context, ids);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var item = await _context.Fazendas                   
                    .FirstOrDefaultAsync(r => r.Name.ToLower().Contains(name.ToLower()));

                if (item == null)
                    return NotFound();

                var result = _mapper.Map<FazendaResponseDto>(item);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}