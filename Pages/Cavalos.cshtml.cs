using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace HipicaFacil.Pages
{
    public class Cavalo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Raca { get; set; }
        public decimal Peso { get; set; }
        public int Idade { get; set; }
        public decimal Altura { get; set; }

    }

    public class CavalosModel : PageModel
    {
        [BindProperty]
        public Cavalo NovoCavalo { get; set; }

        public List<Cavalo> Cavalos { get; set; }

        private string ConnectionString = "Server=127.0.0.1;port=3306;Database=bd_hipicafacil;Uid=root;Pwd=06042001";

        public void OnGet()
        {
            CarregarCavalos();
        }

        public IActionResult OnPostAdicionar()
        {
            if (NovoCavalo != null)
            {
                AdicionarCavalo(NovoCavalo);
            }
            else
            {
                ModelState.AddModelError("", "Falha ao adicionar o cavalo. Por favor, verifique os dados fornecidos.");
            }

            CarregarCavalos();

            return Page();
        }

        public IActionResult OnPostApagar(int idCavalo)
        {
            ApagarCavalo(idCavalo);
            CarregarCavalos();
            return Page();
        }

        public IActionResult OnPostEditar(int idCavalo, string novoNome)
        {
            EditarCavalo(idCavalo, novoNome);
            CarregarCavalos();
            return Page();
        }

        private void CarregarCavalos()
        {
            Cavalos = new List<Cavalo>();

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                string sql = "SELECT id_cavalo, nome_cavalo, raca_cavalo,peso_cavalo, idade_cavalo FROM tb_cavalos";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Cavalos.Add(new Cavalo
                            {
                                Id = reader.GetInt32(0),
                                Nome = reader.GetString(1),
                                Raca = reader.GetString(2),
                                Peso = reader.GetDecimal(3),
                                Idade = reader.GetInt32(4)
                            });
                        }
                    }
                }
            }
        }

        private void AdicionarCavalo(Cavalo cavalo)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string sql = "INSERT INTO Tb_cavalos (nome_cavalo, raca_cavalo, peso_cavalo, idade_cavalo) VALUES (@nome, @raca, @peso, @idade)";

                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@nome", cavalo.Nome);
                        command.Parameters.AddWithValue("@raca", cavalo.Raca);
                        command.Parameters.AddWithValue("@peso", cavalo.Peso);
                        command.Parameters.AddWithValue("@idade", cavalo.Idade);

                        command.ExecuteNonQuery();
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
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string sql = "DELETE FROM Tb_cavalos WHERE id_cavalo = @id";

                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", idCavalo);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao apagar Cavalo: " + ex.Message);
                }
            }
        }

        private void EditarCavalo(int idCavalo, string novoNome)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string sql = "UPDATE Tb_cavalos SET nome_cavalo = @novoNome WHERE id_cavalo = @id";

                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", idCavalo);
                        command.Parameters.AddWithValue("@novoNome", novoNome);

                        command.ExecuteNonQuery();
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


