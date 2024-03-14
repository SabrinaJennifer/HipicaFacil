using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

public class CavalosModel : PageModel
{
    [BindProperty]
    public Cavalo Cavalo { get; set; } = new Cavalo();

    [BindProperty(SupportsGet = true)]
    public int IdCavalo { get; set; }

    public List<Cavalo> Cavalos { get; set; } = new List<Cavalo>();

    private string connectionString = "Server=127.0.0.1;Port=3306;Database=bd_hipicafacil;Uid=root;Pwd=06042001";

    public void OnGet()
    {
        CarregarCavalos();
    }

    public IActionResult OnPost()
    {
        if (!string.IsNullOrEmpty(Cavalo.Nome))
        {
            AdicionarCavalo(Cavalo);
            CarregarCavalos();
            Cavalo = new Cavalo(); // Limpa os campos para permitir adicionar mais cavalos
        }
        else if (IdCavalo != 0)
        {
            RemoverCavalo(IdCavalo);
            CarregarCavalos();
        }

        return RedirectToPage("/Cavalos");
    }

    private void CarregarCavalos()
    {
        Cavalos.Clear();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string sql = "SELECT id_cavalo, nome_cavalo, raca_cavalo, idade_cavalo, peso_cavalo FROM Tb_cavalos";

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
                            Idade = reader.GetInt32(3),
                            Peso = reader.GetInt32(4)
                        });
                    }
                }
            }
        }
    }

    private void AdicionarCavalo(Cavalo cavalo)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string sql = "INSERT INTO Tb_cavalos (nome_cavalo, raca_cavalo, idade_cavalo, peso_cavalo) VALUES (@nome, @raca, @idade, @peso)";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@nome", cavalo.Nome);
                command.Parameters.AddWithValue("@raca", cavalo.Raca);
                command.Parameters.AddWithValue("@idade", cavalo.Idade);
                command.Parameters.AddWithValue("@peso", cavalo.Peso);
                command.ExecuteNonQuery();
            }
        }
    }

    private void RemoverCavalo(int idCavalo)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string sql = "DELETE FROM Tb_cavalos WHERE id_cavalo = @id";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", idCavalo);
                command.ExecuteNonQuery();
            }
        }
    }
}

public class Cavalo
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Raca { get; set; }
    public int Idade { get; set; }
    public int Peso { get; set; }
}
