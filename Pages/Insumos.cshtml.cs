using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

public class Insumo
{
    public int Id { get; set; }
    public string? Nome { get; set; }
}

public class InsumosModel : PageModel
{
    [BindProperty]
    public int IdInsumo { get; set; }

    [BindProperty]
    public string? NomeInsumo { get; set; }

    [BindProperty]
    public string? NovoNomeInsumo { get; set; }

    [BindProperty]
    public int IdInsumoEditar { get; set; }

    public List<Insumo>? Insumos { get; set; }
    private string connectionString = "Server=127.0.0.1;Port=3306;Database=bd_hipicafacil;Uid=root;Pwd=06042001";

    public void OnGet()
    {
        CarregarInsumos();
    }

    public IActionResult OnPost()
    {
        // Se o ID do insumo for fornecido, apaga o insumo
        if (IdInsumo != 0)
        {
            ApagarInsumo(IdInsumo);
        }
        if (!(IdInsumoEditar == 0 || string.IsNullOrEmpty(NovoNomeInsumo)))
        {
            EditarInsumo(IdInsumoEditar, NovoNomeInsumo);
        }
        else
        {
            AdicionarInsumo(NomeInsumo); // Adiciona um novo insumo
        }


        // Redireciona de volta para a página inicial após adicionar ou apagar o insumo
        return RedirectToPage("/Insumos");
    }

    public void CarregarInsumos()
    {

        Insumos = new List<Insumo>();

        // Cria uma nova conexão com o banco de dados
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            // Abre a conexão
            connection.Open();

            // Comando SQL para selecionar todos os insumos
            string sql = "SELECT id_insumo, nome_insumo FROM Tb_insumos";

            // Cria um novo comando SQL
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                // Executa o comando SQL e lê os resultados
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Insumos.Add(new Insumo
                        {
                            Id = reader.GetInt32(0),
                            Nome = reader.GetString(1)
                        });
                    }
                }
            }
        }
    }

    private void AdicionarInsumo(string nomeInsumo)
    {
        // Cria uma nova conexão com o banco de dados
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                // Abre a conexão
                connection.Open();

                // Comando SQL para inserir um novo insumo na tabela Tb_insumos
                string sql = "INSERT INTO Tb_insumos (nome_insumo) VALUES (@nome)";

                // Cria um novo comando SQL
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    // Define os parâmetros do comando SQL
                    command.Parameters.AddWithValue("@nome", nomeInsumo);

                    // Executa o comando SQL
                    command.ExecuteNonQuery();

                    Console.WriteLine("Insumo adicionado com sucesso!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao adicionar insumo: " + ex.Message);
            }
        }
    }

    private void ApagarInsumo(int idInsumo)
    {
        // Cria uma nova conexão com o banco de dados
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                // Abre a conexão
                connection.Open();

                // Comando SQL para apagar o insumo da tabela Tb_insumos
                string sql = "DELETE FROM Tb_insumos WHERE id_insumo = @id";

                // Cria um novo comando SQL
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    // Define os parâmetros do comando SQL
                    command.Parameters.AddWithValue("@id", idInsumo);

                    // Executa o comando SQL
                    command.ExecuteNonQuery();

                    Console.WriteLine("Insumo removido com sucesso!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao remover insumo: " + ex.Message);
            }
        }
    }

    private void EditarInsumo(int idInsumo, string novoNome)
    {
        // Cria uma nova conexão com o banco de dados
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                // Abre a conexão
               connection.Open();

                // Comando SQL para atualizar o nome do insumo na tabela Tb_insumos
                string sql = "UPDATE Tb_insumos SET nome_insumo = @novoNome WHERE id_insumo = @id";

                // Cria um novo comando SQL
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    // Define os parâmetros do comando SQL
                    command.Parameters.AddWithValue("@id", idInsumo);
                    command.Parameters.AddWithValue("@novoNome", novoNome);

                    // Executa o comando SQL
                    command.ExecuteNonQuery();

                    Console.WriteLine("Insumo editado com sucesso!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao editar insumo: " + ex.Message);
            }
        }
    }
}