using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Projeto.Avaliacao.API.Controllers
{
    public abstract class CustomControllerBase : ControllerBase
    {
        protected Avaliacao.API.Helpers.BusinessValidation BusinessValidation { get; set; } = new();

        protected new IActionResult Ok(object value = null)
        {
            return Ok(value, "Action executed sucessfully.");
        }

        protected IActionResult Ok(object value, string message)
        {
            return base.Ok(new
            {
                success = true,
                data = value,
                message = message
            });
        }

        protected IActionResult InternalServerError(Exception ex)
        {
            return StatusCode(
                Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, 
                $"{ex.Message} \n Inner Exception: {ex.InnerException?.Message} \n Stack Trace: {ex.StackTrace}"
            );
        }
        protected IActionResult NoDataFound(string message)
        {
            return StatusCode(
                StatusCodes.Status404NotFound,
               message
            );
        }

        /// <summary>
        /// Validates model state and add business error message if any.
        /// </summary>
        protected void ValidateModelState<T>(T item)
        {
            if (!TryValidateModel(item, nameof(T)))
                foreach (string message in ModelState.SelectMany(m => m.Value.Errors).Select(e => e.ErrorMessage))
                    this.BusinessValidation.AddError(message);
        }

        /// <summary>
        /// Soft deletes entities listed in IDs list, by updating DeletedAt and DeletedBy property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        protected async Task<bool> SoftRemove<T>(Repository.DefaultContext context, ICollection<long> ids) where T : Models.ModelBase
        {
            var items = await context.Set<T>()
                   .Where(i => ids.Contains(i.Id))
                   .ToListAsync();
                   
             foreach (T i in items)
                i.DeletedAt = DateTime.UtcNow;

            return true;
        }
    }
}
