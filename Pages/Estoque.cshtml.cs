using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

public class EstoqueModel : PageModel
{
    [BindProperty]
    public Produto Produto { get; set; } = new Produto();

    [BindProperty(SupportsGet = true)]
    public int IdProduto { get; set; }

    // Propriedades para o formulário de edição
    [BindProperty]
    public Produto ProdutoEdit { get; set; } = new Produto();

    private string connectionString = "Server=127.0.0.1;Port=3306;Database=bd_hipicafacil;Uid=root;Pwd=felipe";

    public List<Produto> Produtos { get; set; } = new List<Produto>();

    public void OnGet()
    {
        CarregarProdutos();
    }

    public IActionResult OnPost()
    {
        if (!string.IsNullOrEmpty(Produto.Nome))
        {
            AdicionarProduto(Produto);
            CarregarProdutos();
            Produto = new Produto(); // Limpa os campos para permitir adicionar mais produtos
        }
        else if (IdProduto != 0)
        {
            RemoverProduto(IdProduto);
            CarregarProdutos();
        }

        return RedirectToPage("/Estoque");
    }

    // Método para processar o formulário de edição
    public IActionResult OnPostEditar()
    {
        if (ProdutoEdit != null && ProdutoEdit.Id != 0)
        {
            AtualizarProduto(ProdutoEdit);
            CarregarProdutos(); // Recarrega a lista de produtos após a edição
        }

        // Redireciona de volta para a página de estoque
        return RedirectToPage("/Estoque");
    }

    private void CarregarProdutos()
    {
        Produtos.Clear();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string sql = "SELECT id_produto, nome_produto, fabricante_produto, quantidade_produto, validade_produto FROM Tb_produtos";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Produtos.Add(new Produto
                        {
                            Id = reader.GetInt32(0),
                            Nome = reader.GetString(1),
                            Fabricante = reader.GetString(2),
                            Quantidade = reader.GetInt32(3),
                            Validade = reader.IsDBNull(4) ? null : reader.GetDateTime(4).ToString("yyyy-MM-dd")
                        });
                    }
                }
            }
        }
    }

    private void AdicionarProduto(Produto produto)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string sql = "INSERT INTO Tb_produtos (nome_produto, fabricante_produto, quantidade_produto, validade_produto) VALUES (@nome, @fabricante, @quantidade, @validade)";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@nome", produto.Nome);
                command.Parameters.AddWithValue("@fabricante", produto.Fabricante);
                command.Parameters.AddWithValue("@quantidade", produto.Quantidade);
                command.Parameters.AddWithValue("@validade", string.IsNullOrEmpty(produto.Validade) ? (object)DBNull.Value : DateTime.Parse(produto.Validade));
                command.ExecuteNonQuery();
            }
        }
    }

    private void RemoverProduto(int idProduto)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string sql = "DELETE FROM Tb_produtos WHERE id_produto = @id";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", idProduto);
                command.ExecuteNonQuery();
            }
        }
    }

    private void AtualizarProduto(Produto produto)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string sql = "UPDATE Tb_produtos SET nome_produto = @nome, fabricante_produto = @fabricante, quantidade_produto = @quantidade, validade_produto = @validade WHERE id_produto = @id";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@nome", produto.Nome);
                command.Parameters.AddWithValue("@fabricante", produto.Fabricante);
                command.Parameters.AddWithValue("@quantidade", produto.Quantidade);
                command.Parameters.AddWithValue("@validade", string.IsNullOrEmpty(produto.Validade) ? (object)DBNull.Value : DateTime.Parse(produto.Validade));
                command.Parameters.AddWithValue("@id", produto.Id);
                command.ExecuteNonQuery();
            }
        }
    }
}

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Fabricante { get; set; }
    public int Quantidade { get; set; }
    public string Validade { get; set; }
}
