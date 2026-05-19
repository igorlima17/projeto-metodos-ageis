namespace QuatroPatas;

public class Cliente
{
    public string Nome { get; set; } = "";
    public string Email { get; set; } = "";
    public string Senha { get; set; } = "";

    // Lista de pets do cliente
    public List<Pet> Pets { get; set; } = [];
}
