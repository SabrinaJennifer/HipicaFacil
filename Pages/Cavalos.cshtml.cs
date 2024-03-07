using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace HipicaFacil.Pages
{
    public class Cavalo()
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Raca { get; set; }
        public decimal Peso { get; set; }
        public int Idade { get; set; }
        public decimal Altura { get; set; }
        // criar caixa de seleção com tipos de cavalos (salto, adestramento...)
        // precisa criar classe para upload de documento?
    }
    public class CavalosModel : PageModel
    {
        [BindProperty]
        public int IdCavalo { get; set; }

        [BindProperty]
        public string? NomeCavalo { get; set; }

        [BindProperty]
        public string? NovoNomeCavalo { get; set; }

        [BindProperty]
        public int IdCavaloEditar { get; set; }

        [BindProperty]
        public string? RacaCavalo { get; set; }

        [BindProperty]
        public string? NovoRacaCavalo { get; set; }

        [BindProperty]
        public string? PesoCavalo { get; set; }

        [BindProperty]
        public string? NovoPesoCavalo { get; set; }
        [BindProperty]
        public string? IdadeCavalo { get; set; }

        [BindProperty]
        public string? NovoIdadeCavalo { get; set; }
        [BindProperty]
        public string? AlturaCavalo { get; set; }

        [BindProperty]
        public string? NovoAlturaCavalo { get; set; }

        public List<Cavalo>? Cavalos { get; set; }

        public void OnGet()
        {
            CarregarCavalos();
        }
        public IActionResult OnPost()
        {
            // Se o ID do cavalo for fornecido, apaga o cavalo
            if (IdCavalo != 0)
            {
                ApagarCavalo(IdCavalo);
            }
            if (!(IdCavaloEditar == 0 || string.IsNullOrEmpty(NovoNomeCavalo)))
            {
                EditarCavalo(IdCavaloEditar, NovoNomeCavalo);
            }
            else
            {
                AdicionarCavalo(NomeCavalo); // Adiciona um novo Cavalo
            }


            // Redireciona de volta para a página inicial após adicionar ou apagar o cavalo
            return RedirectToPage("/Cavalos");
        }
        public void CarregarCavalos()
        {
            // String de conexão com o banco de dados MySQL
            string connectionString = "Server=127.0.0.1;Port=3307;Database=hipicafacil;Uid=root;Pwd=root;";

            Cavalos = new List<Cavalo>();

            // Cria uma nova conexão com o banco de dados
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                // Abre a conexão
                connection.Open();

                // Comando SQL para selecionar todos os cavalos
                string sql = "SELECT id_cavalo, nome_cavalo, raca_cavalo, peso_cavalo, idade_cavalo  FROM tb_cavalos";

                // Cria um novo comando SQL
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    // Executa o comando SQL e lê os resultados
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Cavalos.Add(new Cavalo
                            {
                                Id = reader.GetInt32(0),
                                Nome = reader.GetString(1),
                                Raca = reader.GetString(2),
                                Peso = reader.GetInt32(3),
                                Idade = reader.GetInt32(4)

                            });
                        }
                    }
                }
            }
            private void AdicionarCavalo(string nomeCavalo)
            {
                // String de conexão com o banco de dados MySQL
                string connectionString = "Server=127.0.0.1;Port=3307;Database=hipicafacil;Uid=root;Pwd=root;";

                // Cria uma nova conexão com o banco de dados
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        // Abre a conexão
                        connection.Open();

                        // Comando SQL para inserir um novo cavalo na tabela Tb_cavalos
                        string sql = "INSERT INTO Tb_cavalo (nome_cavalo) VALUES (@nome)";

                        // Cria um novo comando SQL
                        using (MySqlCommand command = new MySqlCommand(sql, connection))
                        {
                            // Define os parâmetros do comando SQL
                            command.Parameters.AddWithValue("@nome", nomeCavalo);

                            // Executa o comando SQL
                            command.ExecuteNonQuery();

                            Console.WriteLine("Cavalo adicionado com sucesso!");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro ao adicionar Cavalo: " + ex.Message);
                    }
                }
            }

            private void ApagarCavalo(int idCavalo)
            {
                // String de conexão com o banco de dados MySQL
                string connectionString = "Server=127.0.0.1;Port=3307;Database=hipicafacil;Uid=root;Pwd=root;";

                // Cria uma nova conexão com o banco de dados
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        // Abre a conexão
                        connection.Open();

                        // Comando SQL para apagar o cavalo da tabela Tb_cavalos
                        string sql = "DELETE FROM Tb_cavalos WHERE id_ = @id";

                        // Cria um novo comando SQL
                        using (MySqlCommand command = new MySqlCommand(sql, connection))
                        {
                            // Define os parâmetros do comando SQL
                            command.Parameters.AddWithValue("@id", idCavalo);

                            // Executa o comando SQL
                            command.ExecuteNonQuery();

                            Console.WriteLine("Cavalo removido com sucesso!");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro ao remover Cavalo: " + ex.Message);
                    }
                }
            }

            private void EditarCavalo(int idCavalo, string novoNome)
            {
                // String de conexão com o banco de dados MySQL
                string connectionString = "Server=127.0.0.1;Port=3307;Database=hipicafacil;Uid=root;Pwd=root;";

                // Cria uma nova conexão com o banco de dados
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        // Abre a conexão
                        connection.Open();

                        // Comando SQL para atualizar o nome do cavalo na tabela Tb_cavalos
                        string sql = "UPDATE Tb_cavalos SET nome_cavalo = @novoNome WHERE id_cavalo = @id";

                        // Cria um novo comando SQL
                        using (MySqlCommand command = new MySqlCommand(sql, connection))
                        {
                            // Define os parâmetros do comando SQL
                            command.Parameters.AddWithValue("@id", idCavalo);
                            command.Parameters.AddWithValue("@novoNome", novoNome);

                            // Executa o comando SQL
                            command.ExecuteNonQuery();

                            Console.WriteLine("Cavalo editado com sucesso!");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro ao editar Cavalo: " + ex.Message);
                    }
                }
            }
        }
    }
  
}

