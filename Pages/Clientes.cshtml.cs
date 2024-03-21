using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class ClientesModel : PageModel
{
    [BindProperty]
    public Cliente Cliente { get; set; } = new Cliente();

    [BindProperty(SupportsGet = true)]
    public string? CPFCliente { get; set; }

    // Propriedades para o formulário de edição
    [BindProperty]
    public Cliente ClienteEdit { get; set; } = new Cliente();

    private string connectionString = "Server=127.0.0.1;Port=3306;Database=bd_hipicafacil;Uid=root;Pwd=felipe";

    public List<Cliente> Clientes { get; set; } = new List<Cliente>();

    public void OnGet()
    {
        CarregarClientes();
    }

    public IActionResult OnPost()
    {
        if (!string.IsNullOrEmpty(Cliente.Nome))
        {
            AdicionarCliente(Cliente);
            CarregarClientes();
            Cliente = new Cliente(); // Limpa os campos para permitir adicionar mais clientes
        }
        else if (!string.IsNullOrEmpty(CPFCliente))
        {
            RemoverCliente(CPFCliente);
            CarregarClientes();
        }

        return RedirectToPage("/Clientes");
    }

    // Método para processar o formulário de edição
    public IActionResult OnPostEditar()
    {
        if (ClienteEdit != null && !string.IsNullOrEmpty(ClienteEdit.CPF))
        {
            AtualizarCliente(ClienteEdit);
            CarregarClientes(); // Recarrega a lista de clientes após a edição
        }

        // Redireciona de volta para a página de clientes
        return RedirectToPage("/Clientes");
    }


    private void CarregarClientes()
    {
        Clientes.Clear();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string sql = "SELECT cpf_cliente, nome_cliente, endereco_cliente, email_cliente, telefone_cliente FROM Tb_clientes";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Clientes.Add(new Cliente
                        {
                            CPF = reader.GetString(0),
                            Nome = reader.GetString(1),
                            Endereco = reader.GetString(2),
                            Email = reader.GetString(3),
                            Telefone = reader.GetString(4)
                        });
                    }
                }
            }
        }
    }

    private void AdicionarCliente(Cliente cliente)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string sql = "INSERT INTO Tb_clientes (cpf_cliente, nome_cliente, endereco_cliente, email_cliente, telefone_cliente) VALUES (@cpf, @nome, @endereco, @email, @telefone)";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@cpf", cliente.CPF);
                command.Parameters.AddWithValue("@nome", cliente.Nome);
                command.Parameters.AddWithValue("@endereco", cliente.Endereco);
                command.Parameters.AddWithValue("@email", cliente.Email);
                command.Parameters.AddWithValue("@telefone", cliente.Telefone);
                command.ExecuteNonQuery();
            }
        }
    }

    private void RemoverCliente(string cpfCliente)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string sql = "DELETE FROM Tb_clientes WHERE cpf_cliente = @cpf";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@cpf", cpfCliente);
                command.ExecuteNonQuery();
            }
        }
    }

    private void AtualizarCliente(Cliente cliente)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string sql = "UPDATE Tb_clientes SET nome_cliente = @nome, endereco_cliente = @endereco, email_cliente = @email, telefone_cliente = @telefone WHERE cpf_cliente = @cpf";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@nome", cliente.Nome);
                command.Parameters.AddWithValue("@endereco", cliente.Endereco);
                command.Parameters.AddWithValue("@email", cliente.Email);
                command.Parameters.AddWithValue("@telefone", cliente.Telefone);
                command.Parameters.AddWithValue("@cpf", cliente.CPF);
                command.ExecuteNonQuery();
            }
        }
    }
}

public class Cliente
{
    public string CPF { get; set; }
    public string Nome { get; set; }
    public string Endereco { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
}
