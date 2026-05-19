namespace QuatroPatas;

public enum Servico { Banho, Tosa, Consulta }
public enum StatusAgendamento { Pendente, Confirmado, Cancelado }

public class Agendamento
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string EmailCliente { get; set; } = "";
    public string NomePet { get; set; } = "";
    public Servico Servico { get; set; }
    public DateTime DataHora { get; set; }
    public StatusAgendamento Status { get; set; } = StatusAgendamento.Pendente;
}
