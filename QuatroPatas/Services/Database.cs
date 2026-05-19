using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace QuatroPatas;

public class Database
{
    // Caminhos dos arquivos JSON
    private const string CLIENTES_PATH = "Data/clientes.json";
    private const string FUNCIONARIOS_PATH = "Data/funcionarios.json";
    private const string AGENDAMENTOS_PATH = "Data/agendamentos.json";

    public List<Cliente> Clientes { get; private set; }
    public List<Funcionario> Funcionarios { get; private set; }
    public List<Agendamento> Agendamentos { get; private set; }

    public Database()
    {
        // Cria a pasta Data se ela não existir
        Directory.CreateDirectory("Data");

        if (File.Exists(CLIENTES_PATH))
        {
            string json = File.ReadAllText(CLIENTES_PATH);
            Clientes = JsonSerializer.Deserialize<List<Cliente>>(json) ?? new List<Cliente>();
        }
        else
        {
            Clientes = new List<Cliente>();
        }

        if (File.Exists(AGENDAMENTOS_PATH))
        {
            string json = File.ReadAllText(AGENDAMENTOS_PATH);
            Agendamentos = JsonSerializer.Deserialize<List<Agendamento>>(json) ?? new List<Agendamento>();
        }
        else
        {
            Agendamentos = new List<Agendamento>();
        }

        if (File.Exists(FUNCIONARIOS_PATH))
        {
            string json = File.ReadAllText(FUNCIONARIOS_PATH);
            Funcionarios = JsonSerializer.Deserialize<List<Funcionario>>(json) ?? new List<Funcionario>();
        }
        else
        {
            // Se não existir o arquivo, cria um funcionario padrão
            Funcionarios = CriarFuncionarioPadrao();
        }
    }

    public void SalvarClientes()
    {
        JsonSerializerOptions opcoes = new JsonSerializerOptions();
        opcoes.WriteIndented = true;
        string json = JsonSerializer.Serialize(Clientes, opcoes);
        File.WriteAllText(CLIENTES_PATH, json);
    }

    public void SalvarAgendamentos()
    {
        JsonSerializerOptions opcoes = new JsonSerializerOptions();
        opcoes.WriteIndented = true;
        string json = JsonSerializer.Serialize(Agendamentos, opcoes);
        File.WriteAllText(AGENDAMENTOS_PATH, json);
    }

    // Converte a senha em hash para não salvar em texto puro
    public static string Hash(string senha)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(senha);
        byte[] hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash).ToLower();
    }

    private List<Funcionario> CriarFuncionarioPadrao()
    {
        Funcionario admin = new Funcionario();
        admin.Nome = "Admin";
        admin.Email = "admin@quatropatas.com";
        admin.Senha = Hash("admin123");

        List<Funcionario> lista = new List<Funcionario>();
        lista.Add(admin);

        JsonSerializerOptions opcoes = new JsonSerializerOptions();
        opcoes.WriteIndented = true;
        string json = JsonSerializer.Serialize(lista, opcoes);
        File.WriteAllText(FUNCIONARIOS_PATH, json);

        return lista;
    }
}
