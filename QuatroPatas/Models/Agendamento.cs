namespace QuatroPatas;

// Tipos de servico disponiveis no petshop
public enum Servico { Banho, Tosa, Consulta }

// Status possíveis de um agendamento
public enum StatusAgendamento { Pendente, Confirmado, Cancelado }

public class Agendamento
{
    // ID único gerado automaticamente
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string EmailCliente { get; set; } = "";
    public string NomePet { get; set; } = "";
    public Servico Servico { get; set; }
    public DateTime DataHora { get; set; }

    // Todo agendamento começa como pendente
    public StatusAgendamento Status { get; set; } = StatusAgendamento.Pendente;
}
