namespace QuatroPatas;

public class FuncionarioService
{
    private readonly Database _db;

    public FuncionarioService(Database db) => _db = db;

    public Funcionario? Login(string email, string senha) =>
        _db.Funcionarios.FirstOrDefault(f => f.Email == email && f.Senha == Database.Hash(senha));

    public List<Agendamento> TodosAgendamentos() => _db.Agendamentos;

    public string ConfirmarAgendamento(string id)
    {
        var ag = _db.Agendamentos.FirstOrDefault(a => a.Id == id);
        if (ag is null) return "Agendamento nao encontrado.";
        if (ag.Status == StatusAgendamento.Cancelado) return "Agendamento ja foi cancelado.";
        ag.Status = StatusAgendamento.Confirmado;
        _db.SalvarAgendamentos();
        return "Agendamento confirmado!";
    }

    public string CancelarAgendamento(string id)
    {
        var ag = _db.Agendamentos.FirstOrDefault(a => a.Id == id);
        if (ag is null) return "Agendamento nao encontrado.";
        if (ag.Status == StatusAgendamento.Cancelado) return "Ja esta cancelado.";
        ag.Status = StatusAgendamento.Cancelado;
        _db.SalvarAgendamentos();
        return "Agendamento cancelado.";
    }
}
