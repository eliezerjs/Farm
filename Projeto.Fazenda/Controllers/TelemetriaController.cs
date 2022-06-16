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
    [Route("api/telemetria")]
    [ApiController]
    public class TelemetriaController : CustomControllerBase
    {
        private readonly DefaultContext _context;
        private readonly IMapper _mapper;
        public TelemetriaController(DefaultContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all Records.
        /// </summary>
        /// <returns>Lists all Devices entity records</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IList<TelemetriaResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var Telemetrias = await _context.Telemetrias
                    .Include(x => x.Dispositivo)
                    .ThenInclude(f => f.Fazenda)
                    .Where(x => x.DeletedAt == null)
                    .ToListAsync();

                var result = _mapper.Map<List<TelemetriaResponseDto>>(Telemetrias);

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
        protected async Task<bool> Validate(Telemetria item)
        {
            ValidateModelState<Telemetria>(item);
            this.BusinessValidation.Clear();

            if (item.DispositivoId.Equals(0))
                this.BusinessValidation.AddError("Dispositivo is required");

            if (item.Temperatura.Equals(0))
                this.BusinessValidation.AddError("Temperatura is required");

            if (item.Umidade.Equals(0))
                this.BusinessValidation.AddError("Umidade is required");

            return await Task.FromResult(this.BusinessValidation.IsValid);
        }

        /// <summary>
        /// Save Record.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Record that was recorded</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TelemetriaResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Save([FromBody] TelemetriaRequestDto data)
        {
            try
            {
                var toAdd = new Telemetria();
                toAdd.CreatedAt = DateTime.UtcNow;

                _context.Entry<Telemetria>(toAdd).CurrentValues.SetValues(data.ToSetValuesModel());

                if (!await Validate(toAdd))
                    return new UnprocessableEntityObjectResult(this.BusinessValidation);

                _context.Telemetrias.Add(toAdd);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<TelemetriaResponseDto>(toAdd);

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
        [ProducesResponseType(typeof(TelemetriaResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromRoute] long id, [FromBody] TelemetriaRequestDto data)
        {
            try
            {
                data.Id = id;

                var existingItem = await _context.Telemetrias
                    .Where(r => r.Id == data.Id)
                    .FirstOrDefaultAsync();

                if (existingItem == null)
                    return NotFound();

                _context.Entry<Telemetria>(existingItem).CurrentValues.SetValues(data.ToSetValuesModel());
                existingItem.UpdatedAt = DateTime.UtcNow;

                if (!await Validate(existingItem))
                    return new UnprocessableEntityObjectResult(this.BusinessValidation);

                _context.Telemetrias.Update(existingItem);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<TelemetriaResponseDto>(existingItem);

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
                await base.SoftRemove<Telemetria>(_context, ids);
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
        /// <returns>Telemetry List</returns>
        [HttpGet]
        [Route("fazenda/{name}")]
        public async Task<IActionResult> GetTelemetryByFarmName(string name)
        {
            try
            {
                var item = await _context.Telemetrias
                    .Include(x => x.Dispositivo)
                    .ThenInclude(f => f.Fazenda)
                    .FirstOrDefaultAsync(r => r.Dispositivo.Fazenda.Name.Contains(name));

                if (item == null)
                    return NotFound();

                var result = _mapper.Map<TelemetriaResponseDto>(item);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}