using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace trayprojeto45
{
    public class Compra
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }

        [Required]
        public string Produto { get; set; }

        [Required]
        public decimal Preco { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Cidade { get; set; }

        public string Estado { get; set; }

        public string Complemento { get; set; }

        public Compra() { }

        public Compra(string produto, decimal preco, string email, string cidade, string estado, string complemento)
        {
            Produto = produto;
            Preco = preco;
            Email = email;
            Cidade = cidade;
            Estado = estado;
            Complemento = complemento;
        }
    }
}
