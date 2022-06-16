using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Avaliacao.API.DTOs.Request;
using Projeto.Avaliacao.API.DTOs.Response;
using Projeto.Avaliacao.API.Models;
using Projeto.Avaliacao.API.Repository;
using System.Net;

namespace Projeto.Avaliacao.API.Controllers
{
    [Route("api/dispositivo")]
    [ApiController]
    public class DispositivoController : CustomControllerBase
    {
        private readonly DefaultContext _context;
        private readonly IMapper _mapper;
        public DispositivoController(DefaultContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Search all Device records.
        /// </summary>
        /// <returns>Get All Records of Devices</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IList<DispositivoResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var Dispositivos = await _context.Dispositivos
                    .Include(f => f.Fazenda)
                    .Where(x => x.DeletedAt == null)
                    .ToListAsync();

                var result = _mapper.Map<List<DispositivoResponseDto>>(Dispositivos);

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
        protected async Task<bool> Validate(Dispositivo item)
        {
            ValidateModelState<Dispositivo>(item);

            this.BusinessValidation.Clear();

            if (string.IsNullOrEmpty(item.Name) || string.IsNullOrWhiteSpace(item.Name))
                this.BusinessValidation.AddError("Name is required");

            return await Task.FromResult(this.BusinessValidation.IsValid);
        }

        /// <summary>
        /// Save record.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Record that was recorded</returns>
        [HttpPost]
        [ProducesResponseType(typeof(DispositivoResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Save([FromBody] DispositivoRequestDto data)
        {
            try
            {
                var toAdd = new Dispositivo();
                toAdd.CreatedAt = DateTime.UtcNow;

                _context.Entry<Dispositivo>(toAdd).CurrentValues.SetValues(data.ToSetValuesModel());

                if (!await Validate(toAdd))
                    return new UnprocessableEntityObjectResult(this.BusinessValidation);

                _context.Dispositivos.Add(toAdd);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<DispositivoResponseDto>(toAdd);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Update Record.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns>Record that was recorded</returns>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(DispositivoResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromRoute] long id, [FromBody] DispositivoRequestDto data)
        {
            try
            {
                data.Id = id;

                var existingItem = await _context.Dispositivos
                    .Where(r => r.Id == data.Id)
                    .FirstOrDefaultAsync();

                if (existingItem == null)
                    return NotFound();

                _context.Entry<Dispositivo>(existingItem).CurrentValues.SetValues(data.ToSetValuesModel());
                existingItem.UpdatedAt = DateTime.UtcNow;

                if (!await Validate(existingItem))
                    return new UnprocessableEntityObjectResult(this.BusinessValidation);


                _context.Dispositivos.Update(existingItem);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<DispositivoResponseDto>(existingItem);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Logically removes the record in the database.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>Ok</returns>
        [HttpDelete]
        public async Task<IActionResult> Remove([FromBody] List<long> ids)
        {
            try
            {
                await base.SoftRemove<Dispositivo>(_context, ids);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Search all records by the farm name parameter.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Devices List</returns>
        [HttpGet("fazenda/{name}")]
        [ProducesResponseType(typeof(IList<DispositivoResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDevicesByFarmName(string name)
        {
            try
            {
                var item = await _context.Dispositivos
                    .Include(f => f.Fazenda)
                    .FirstOrDefaultAsync(r => r.Fazenda.Name.ToLower().Contains(name.ToLower()));

                if (item == null)
                    return NotFound();

                var result = _mapper.Map<DispositivoResponseDto>(item);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}