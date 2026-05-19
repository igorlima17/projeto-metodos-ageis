namespace QuatroPatas;

public class ClienteService
{
    private readonly Database _db;

    public ClienteService(Database db) => _db = db;

    // Auth
    public string Cadastrar(string nome, string email, string senha)
    {
        if (!email.Contains('@')) return "Email invalido.";
        if (_db.Clientes.Any(c => c.Email == email)) return "Email ja cadastrado.";
        _db.Clientes.Add(new Cliente { Nome = nome, Email = email, Senha = Database.Hash(senha) });
        _db.SalvarClientes();
        return "Cadastro realizado com sucesso!";
    }

    public Cliente? Login(string email, string senha) =>
        _db.Clientes.FirstOrDefault(c => c.Email == email && c.Senha == Database.Hash(senha));

    // Dados pessoais
    public string Atualizar(Cliente cliente, string novoNome, string novoEmail)
    {
        if (!novoEmail.Contains('@')) return "Email invalido.";
        if (novoEmail != cliente.Email && _db.Clientes.Any(c => c.Email == novoEmail))
            return "Email ja cadastrado.";
        cliente.Nome = novoNome;
        cliente.Email = novoEmail;
        _db.SalvarClientes();
        return "Dados atualizados!";
    }

    public string Deletar(Cliente cliente)
    {
        _db.Agendamentos.RemoveAll(a => a.EmailCliente == cliente.Email);
        _db.Clientes.Remove(cliente);
        _db.SalvarClientes();
        _db.SalvarAgendamentos();
        return "Conta deletada.";
    }

    // Pets
    public string CadastrarPet(Cliente cliente, string nome, string especie, string raca)
    {
        if (cliente.Pets.Any(p => p.Nome.ToLower() == nome.ToLower()))
            return "Voce ja tem um pet com esse nome.";
        cliente.Pets.Add(new Pet { Nome = nome, Especie = especie, Raca = raca });
        _db.SalvarClientes();
        return "Pet cadastrado!";
    }

    public string DeletarPet(Cliente cliente, string nomePet)
    {
        var pet = cliente.Pets.FirstOrDefault(p => p.Nome.ToLower() == nomePet.ToLower());
        if (pet is null) return "Pet nao encontrado.";
        cliente.Pets.Remove(pet);
        _db.SalvarClientes();
        return "Pet removido.";
    }

    // Agendamentos
    public string Agendar(Cliente cliente, string nomePet, Servico servico, DateTime dataHora)
    {
        if (!cliente.Pets.Any(p => p.Nome.ToLower() == nomePet.ToLower()))
            return "Pet nao encontrado.";
        _db.Agendamentos.Add(new Agendamento
        {
            EmailCliente = cliente.Email,
            NomePet = nomePet,
            Servico = servico,
            DataHora = dataHora
        });
        _db.SalvarAgendamentos();
        return "Agendamento realizado!";
    }

    public List<Agendamento> MeusAgendamentos(Cliente cliente) =>
        _db.Agendamentos.Where(a => a.EmailCliente == cliente.Email).ToList();

    public string CancelarAgendamento(Cliente cliente, string id)
    {
        var ag = _db.Agendamentos.FirstOrDefault(a => a.Id == id && a.EmailCliente == cliente.Email);
        if (ag is null) return "Agendamento nao encontrado.";
        if (ag.Status == StatusAgendamento.Cancelado) return "Ja esta cancelado.";
        ag.Status = StatusAgendamento.Cancelado;
        _db.SalvarAgendamentos();
        return "Agendamento cancelado.";
    }
}
