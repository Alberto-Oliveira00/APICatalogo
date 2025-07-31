using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs;

public class ProdutoDTOUpdateRequest : IValidatableObject
{
    [Range(1,99999, ErrorMessage = "O estoque deve estar entre 1 e 9999")]
    public float Estoque { get; set; }
    public DateTime DataCadastro { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DataCadastro > DateTime.Now)
        {
            yield return new ValidationResult("A data de cadastro não pode ser uma data futura", 
                new[] { nameof(this.DataCadastro) });
        }
    }
}
