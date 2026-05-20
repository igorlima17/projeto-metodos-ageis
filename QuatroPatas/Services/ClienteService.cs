namespace QuatroPatas;

public class ClienteService
{
    private readonly Database _db;

    public ClienteService(Database db)
    {
        _db = db;
    }

    public string Cadastrar(string nome, string email, string senha)
    {
        if (!email.Contains('@'))
        {
            return "Email invalido.";
        }

        // Verifica se o email já está cadastrado
        foreach (var c in _db.Clientes)
        {
            if (c.Email == email)
            {
                return "Email ja cadastrado.";
            }
        }

        Cliente novoCliente = new Cliente();
        novoCliente.Nome = nome;
        novoCliente.Email = email;
        novoCliente.Senha = Database.Hash(senha);

        _db.Clientes.Add(novoCliente);
        _db.SalvarClientes();
        return "Cadastro realizado com sucesso!";
    }

    public Cliente? Login(string email, string senha)
    {
        string senhaHash = Database.Hash(senha);

        // Procura um cliente com email e senha correspondentes
        foreach (var c in _db.Clientes)
        {
            if (c.Email == email && c.Senha == senhaHash)
            {
                return c;
            }
        }

        return null;
    }

    public string Atualizar(Cliente cliente, string novoNome, string novoEmail)
    {
        if (!novoEmail.Contains('@'))
        {
            return "Email invalido.";
        }

        // Verifica se o novo email já pertence a outro cliente
        foreach (var c in _db.Clientes)
        {
            if (c.Email == novoEmail && c.Email != cliente.Email)
            {
                return "Email ja cadastrado.";
            }
        }

        cliente.Nome = novoNome;
        cliente.Email = novoEmail;
        _db.SalvarClientes();
        return "Dados atualizados!";
    }

    public string Deletar(Cliente cliente)
    {
        // Remove todos os agendamentos do cliente antes de deletar a conta
        _db.Agendamentos.RemoveAll(a => a.EmailCliente == cliente.Email);
        _db.Clientes.Remove(cliente);
        _db.SalvarClientes();
        _db.SalvarAgendamentos();
        return "Conta deletada.";
    }

    public string CadastrarPet(Cliente cliente, string nome, string especie, string raca, string idade)
    {
        // Verifica se já existe um pet com o mesmo nome
        foreach (var p in cliente.Pets)
        {
            if (p.Nome.ToLower() == nome.ToLower())
            {
                return "Voce ja tem um pet com esse nome.";
            }
        }

        Pet novoPet = new Pet();
        novoPet.Nome = nome;
        novoPet.Especie = especie;
        novoPet.Raca = raca;
        novoPet.Idade = idade;

        cliente.Pets.Add(novoPet);
        _db.SalvarClientes();
        return "Pet cadastrado!";
    }

    public string DeletarPet(Cliente cliente, string nomePet)
    {
        Pet? petEncontrado = null;

        foreach (var p in cliente.Pets)
        {
            if (p.Nome.ToLower() == nomePet.ToLower())
            {
                petEncontrado = p;
            }
        }

        if (petEncontrado == null)
        {
            return "Pet nao encontrado.";
        }

        cliente.Pets.Remove(petEncontrado);
        _db.SalvarClientes();
        return "Pet removido.";
    }

    public string Agendar(Cliente cliente, string nomePet, Servico servico, DateTime dataHora)
    {
        Pet? petEncontrado = null;

        // Verifica se o pet pertence ao cliente
        foreach (var p in cliente.Pets)
        {
            if (p.Nome.ToLower() == nomePet.ToLower())
            {
                petEncontrado = p;
            }
        }

        if (petEncontrado == null)
        {
            return "Pet nao encontrado.";
        }

        Agendamento novoAgendamento = new Agendamento();
        novoAgendamento.EmailCliente = cliente.Email;
        novoAgendamento.NomePet = nomePet;
        novoAgendamento.Servico = servico;
        novoAgendamento.DataHora = dataHora;

        _db.Agendamentos.Add(novoAgendamento);
        _db.SalvarAgendamentos();
        return "Agendamento realizado!";
    }

    public List<Agendamento> MeusAgendamentos(Cliente cliente)
    {
        List<Agendamento> lista = new List<Agendamento>();

        // Filtra apenas os agendamentos do cliente logado
        foreach (var a in _db.Agendamentos)
        {
            if (a.EmailCliente == cliente.Email)
            {
                lista.Add(a);
            }
        }

        return lista;
    }

    public string CancelarAgendamento(Cliente cliente, string id)
    {
        Agendamento? agEncontrado = null;

        foreach (var a in _db.Agendamentos)
        {
            if (a.Id == id && a.EmailCliente == cliente.Email)
            {
                agEncontrado = a;
            }
        }

        if (agEncontrado == null)
        {
            return "Agendamento nao encontrado.";
        }

        if (agEncontrado.Status == StatusAgendamento.Cancelado)
        {
            return "Ja esta cancelado.";
        }

        agEncontrado.Status = StatusAgendamento.Cancelado;
        _db.SalvarAgendamentos();
        return "Agendamento cancelado.";
    }
}
