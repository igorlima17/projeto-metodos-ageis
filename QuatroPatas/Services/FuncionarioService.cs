namespace QuatroPatas;

public class FuncionarioService
{
    private readonly Database _db;

    public FuncionarioService(Database db)
    {
        _db = db;
    }

    public Funcionario? Login(string email, string senha)
    {
        string senhaHash = Database.Hash(senha);

        // Procura um funcionario com email e senha correspondentes
        foreach (var f in _db.Funcionarios)
        {
            if (f.Email == email && f.Senha == senhaHash)
            {
                return f;
            }
        }

        return null;
    }

    // Retorna todos os agendamentos do sistema
    public List<Agendamento> TodosAgendamentos()
    {
        return _db.Agendamentos;
    }

    public string ConfirmarAgendamento(string id)
    {
        Agendamento? agEncontrado = null;

        foreach (var a in _db.Agendamentos)
        {
            if (a.Id == id)
            {
                agEncontrado = a;
            }
        }

        if (agEncontrado == null)
        {
            return "Agendamento nao encontrado.";
        }

        // Nao é possível confirmar um agendamento já cancelado
        if (agEncontrado.Status == StatusAgendamento.Cancelado)
        {
            return "Agendamento ja foi cancelado.";
        }

        agEncontrado.Status = StatusAgendamento.Confirmado;
        _db.SalvarAgendamentos();
        return "Agendamento confirmado!";
    }

    public string CancelarAgendamento(string id)
    {
        Agendamento? agEncontrado = null;

        foreach (var a in _db.Agendamentos)
        {
            if (a.Id == id)
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
