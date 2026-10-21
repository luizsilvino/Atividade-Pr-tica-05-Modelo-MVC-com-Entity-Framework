namespace Exemplo_EF.Models
{
    public class Categoria
    {
        public Guid CategoriaId { get; set; }
        public string Nome { get; set; }

        public ICollection<Produto> Produtos { get; set; }
    }
}
