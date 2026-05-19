using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace QuatroPatas;

public class Database
{
    private const string CLIENTES_PATH = "Data/clientes.json";
    private const string FUNCIONARIOS_PATH = "Data/funcionarios.json";
    private const string AGENDAMENTOS_PATH = "Data/agendamentos.json";

    private static readonly JsonSerializerOptions _opts = new() { WriteIndented = true };

    public List<Cliente> Clientes { get; private set; }
    public List<Funcionario> Funcionarios { get; private set; }
    public List<Agendamento> Agendamentos { get; private set; }

    public Database()
    {
        Clientes = Carregar<List<Cliente>>(CLIENTES_PATH) ?? [];
        Funcionarios = Carregar<List<Funcionario>>(FUNCIONARIOS_PATH) ?? SeedFuncionarios();
        Agendamentos = Carregar<List<Agendamento>>(AGENDAMENTOS_PATH) ?? [];
    }

    private static T? Carregar<T>(string path) =>
        File.Exists(path) ? JsonSerializer.Deserialize<T>(File.ReadAllText(path)) : default;

    public void SalvarClientes() =>
        File.WriteAllText(CLIENTES_PATH, JsonSerializer.Serialize(Clientes, _opts));

    public void SalvarAgendamentos() =>
        File.WriteAllText(AGENDAMENTOS_PATH, JsonSerializer.Serialize(Agendamentos, _opts));

    public static string Hash(string senha) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(senha))).ToLower();

    // Cria funcionarios padrao se o arquivo nao existir
    private List<Funcionario> SeedFuncionarios()
    {
        var lista = new List<Funcionario>
        {
            new() { Nome = "Admin", Email = "admin@quatropatas.com", Senha = Hash("admin123") }
        };
        File.WriteAllText(FUNCIONARIOS_PATH, JsonSerializer.Serialize(lista, _opts));
        return lista;
    }
}
