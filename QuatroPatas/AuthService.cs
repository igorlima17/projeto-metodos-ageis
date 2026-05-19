using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace QuatroPatas;

public class AuthService
{
    private const string DB_PATH = "usuarios.json";
    private List<Usuario> _usuarios;

    public AuthService()
    {
        _usuarios = File.Exists(DB_PATH)
            ? JsonSerializer.Deserialize<List<Usuario>>(File.ReadAllText(DB_PATH)) ?? []
            : [];
    }

    private void Salvar() =>
        File.WriteAllText(DB_PATH, JsonSerializer.Serialize(_usuarios, new JsonSerializerOptions { WriteIndented = true }));

    private static string Hash(string senha) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(senha))).ToLower();

    public string Cadastrar(string nome, string email, string senha)
    {
        if (!email.Contains('@')) return "Email invalido.";
        if (_usuarios.Any(u => u.Email == email)) return "Email ja cadastrado.";

        _usuarios.Add(new Usuario { Nome = nome, Email = email, Senha = Hash(senha) });
        Salvar();
        return "Cadastrado com sucesso!";
    }

    public Usuario? Login(string email, string senha) =>
        _usuarios.FirstOrDefault(u => u.Email == email && u.Senha == Hash(senha));

    public string Atualizar(Usuario usuario, string novoNome, string novoEmail)
    {
        if (!novoEmail.Contains('@')) return "Email invalido.";
        if (novoEmail != usuario.Email && _usuarios.Any(u => u.Email == novoEmail)) return "Email ja cadastrado.";

        usuario.Nome = novoNome;
        usuario.Email = novoEmail;
        Salvar();
        return "Dados atualizados!";
    }

    public string Deletar(Usuario usuario)
    {
        _usuarios.Remove(usuario);
        Salvar();
        return "Conta deletada.";
    }
}