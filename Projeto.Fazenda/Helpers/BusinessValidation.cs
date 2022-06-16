using System.Text.Json;

namespace Projeto.Avaliacao.API.Helpers
{
    public class BusinessValidation
    {
        public class BusinessValidationError
        {
            public string? Message { get; set; }
        }

        public BusinessValidation() { }
        
        public List<BusinessValidationError> Errors { get; set; } = new List<BusinessValidationError>();

        public void Clear()
        {
            this.Errors.Clear();
        }

        public void AddError(BusinessValidationError error, object[] args)
        {
            this.Errors.Add(error);
        }

        public void AddError(string message)
        {
            AddError(new BusinessValidationError() { Message = message }, null);
        }
        public void AddError(string message, params object[] args)
        {
            AddError(new BusinessValidationError() { Message = message }, args);
        }

        public bool HasErros
        {
            get { return this.Errors.Count > 0; }
        }
        public bool IsValid
        {
            get { return this.Errors.Count == 0; }
        }

        /// <summary>
        /// Returns a JSON string representation of current object state.
        /// </summary>
        public override string ToString()
        {
            //return string.Join("\n", Errors.Select(e => e.Message));
            return JsonSerializer.Serialize(this);
        }
    }
}
