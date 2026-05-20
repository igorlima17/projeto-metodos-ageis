using QuatroPatas;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// Inicializa o banco de dados e os serviços
var db = new Database();
var clienteService = new ClienteService(db);
var funcService = new FuncionarioService(db);

// Menu principal
while (true)
{
    Console.Clear();
    Console.WriteLine("=== QuatroPatas ===");
    Console.WriteLine("1. Area do Cliente");
    Console.WriteLine("2. Area do Funcionario");
    Console.WriteLine("0. Sair");
    Console.Write("Opcao: ");

    switch (Console.ReadLine())
    {
        case "1": MenuCliente(); break;
        case "2": MenuFuncionario(); break;
        case "0": return;
    }
}

// ── AREA DO CLIENTE ──────────────────────────────────────────────
void MenuCliente()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("=== Area do Cliente ===");
        Console.WriteLine("1. Cadastrar");
        Console.WriteLine("2. Login");
        Console.WriteLine("0. Voltar");
        Console.Write("Opcao: ");

        switch (Console.ReadLine())
        {
            case "1":
                Console.Write("Nome: "); var nome = Console.ReadLine() ?? "";
                Console.Write("Email: "); var email = Console.ReadLine() ?? "";
                Console.Write("Senha: "); var senha = Console.ReadLine() ?? "";
                Console.WriteLine(clienteService.Cadastrar(nome, email, senha));
                Console.ReadKey();
                break;

            case "2":
                Console.Write("Email: "); var loginEmail = Console.ReadLine() ?? "";
                Console.Write("Senha: "); var loginSenha = Console.ReadLine() ?? "";
                var cliente = clienteService.Login(loginEmail, loginSenha);
                if (cliente is null)
                {
                    Console.WriteLine("Email ou senha incorretos.");
                    Console.ReadKey();
                    break;
                }
                MenuClienteLogado(cliente);
                break;

            case "0": return;
        }
    }
}

void MenuClienteLogado(Cliente cliente)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine($"=== Bem-vindo, {cliente.Nome}! ===");
        Console.WriteLine("1. Meus pets");
        Console.WriteLine("2. Agendar servico");
        Console.WriteLine("3. Meus agendamentos");
        Console.WriteLine("4. Configuracoes");
        Console.WriteLine("0. Logout");
        Console.Write("Opcao: ");

        switch (Console.ReadLine())
        {
            case "1":
                MenuPets(cliente);
                break;

            case "2":
                MenuAgendar(cliente);
                break;

            case "3":
                MenuAgendamentosCliente(cliente);
                break;

            case "4":
                // Retorna true se a conta foi deletada
                if (MenuConfiguracoes(cliente)) return;
                break;

            case "0": return;
        }
    }
}

bool MenuConfiguracoes(Cliente cliente)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("=== Configuracoes ===");
        Console.WriteLine("1. Ver meus dados");
        Console.WriteLine("2. Editar dados");
        Console.WriteLine("3. Deletar conta");
        Console.WriteLine("0. Voltar");
        Console.Write("Opcao: ");

        switch (Console.ReadLine())
        {
            case "1":
                Console.WriteLine($"\nNome:  {cliente.Nome}");
                Console.WriteLine($"Email: {cliente.Email}");
                Console.WriteLine($"Pets:  {(cliente.Pets.Count == 0 ? "nenhum" : string.Join(", ", cliente.Pets.Select(p => p.Nome)))}");
                Console.ReadKey();
                break;

            case "2":
                Console.Write("Novo nome: "); var novoNome = Console.ReadLine() ?? "";
                Console.Write("Novo email: "); var novoEmail = Console.ReadLine() ?? "";
                Console.WriteLine(clienteService.Atualizar(cliente, novoNome, novoEmail));
                Console.ReadKey();
                break;

            case "3":
                // Dupla confirmação antes de deletar
                Console.WriteLine("\nTem certeza que deseja deletar sua conta? (s/n)");
                if (Console.ReadLine()?.ToLower() != "s") break;
                Console.WriteLine("Confirme digitando seu email: ");
                if (Console.ReadLine() != cliente.Email)
                {
                    Console.WriteLine("Email incorreto. Operacao cancelada.");
                    Console.ReadKey();
                    break;
                }
                Console.WriteLine(clienteService.Deletar(cliente));
                Console.ReadKey();
                return true;

            case "0": return false;
        }
    }
}

void MenuPets(Cliente cliente)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("=== Meus Pets ===");
        if (cliente.Pets.Count == 0)
            Console.WriteLine("Nenhum pet cadastrado.");
        else
            foreach (var p in cliente.Pets)
                Console.WriteLine($"- {p.Nome} | {p.Especie} | {p.Raca} | {p.Idade} ");

        Console.WriteLine("\n1. Cadastrar pet");
        Console.WriteLine("2. Remover pet");
        Console.WriteLine("0. Voltar");
        Console.Write("Opcao: ");

        switch (Console.ReadLine())
        {
            case "1":
                Console.Write("Nome do pet: "); var nome = Console.ReadLine() ?? "";
                Console.Write("Especie: "); var especie = Console.ReadLine() ?? "";
                Console.Write("Raca: "); var raca = Console.ReadLine() ?? "";
                Console.Write("Idade (em anos): "); var idade = Console.ReadLine() ?? "";
                Console.WriteLine(clienteService.CadastrarPet(cliente, nome, especie, raca, idade));
                Console.ReadKey();
                break;

            case "2":
                Console.Write("Nome do pet a remover: ");
                Console.WriteLine(clienteService.DeletarPet(cliente, Console.ReadLine() ?? ""));
                Console.ReadKey();
                break;

            case "0": return;
        }
    }
}

void MenuAgendar(Cliente cliente)
{
    Console.Clear();
    Console.WriteLine("=== Novo Agendamento ===");

    if (cliente.Pets.Count == 0)
    {
        Console.WriteLine("Cadastre um pet primeiro.");
        Console.ReadKey();
        return;
    }

    // Escolher pet
    Console.WriteLine("Seus pets:");
    for (int i = 0; i < cliente.Pets.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {cliente.Pets[i].Nome}");
    }
    Console.Write("Escolha o pet: ");
    string petInput = Console.ReadLine() ?? "";
    if (!int.TryParse(petInput, out int petIdx) || petIdx < 1 || petIdx > cliente.Pets.Count)
    {
        Console.WriteLine("Opcao invalida.");
        Console.ReadKey();
        return;
    }
    string nomePet = cliente.Pets[petIdx - 1].Nome;

    // Escolher servico
    Console.WriteLine("\nServicos:");
    Console.WriteLine("1. Banho");
    Console.WriteLine("2. Tosa");
    Console.WriteLine("3. Consulta Veterinaria");
    Console.Write("Opcao: ");
    Servico servico;
    switch (Console.ReadLine())
    {
        case "1": servico = Servico.Banho; break;
        case "2": servico = Servico.Tosa; break;
        case "3": servico = Servico.Consulta; break;
        default:
            Console.WriteLine("Opcao invalida.");
            Console.ReadKey();
            return;
    }

    // Escolher dia (proximos 5 dias a partir de amanha)
    int[] horarios = { 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 };
    Console.WriteLine("\nDias disponiveis:");
    for (int i = 1; i <= 5; i++)
    {
        Console.WriteLine($"{i}. {DateTime.Today.AddDays(i):dd/MM/yyyy}");
    }
    Console.Write("Escolha o dia: ");
    string diaInput = Console.ReadLine() ?? "";
    if (!int.TryParse(diaInput, out int diaIdx) || diaIdx < 1 || diaIdx > 5)
    {
        Console.WriteLine("Opcao invalida.");
        Console.ReadKey();
        return;
    }
    DateTime diaSelecionado = DateTime.Today.AddDays(diaIdx);

    // Escolher horario
    Console.WriteLine("\nHorarios disponiveis:");
    for (int i = 0; i < horarios.Length; i++)
    {
        Console.WriteLine($"{i + 1}. {horarios[i]:00}:00");
    }
    Console.Write("Escolha o horario: ");
    string horarioInput = Console.ReadLine() ?? "";
    if (!int.TryParse(horarioInput, out int horarioIdx) || horarioIdx < 1 || horarioIdx > horarios.Length)
    {
        Console.WriteLine("Opcao invalida.");
        Console.ReadKey();
        return;
    }

    DateTime dataHora = diaSelecionado.AddHours(horarios[horarioIdx - 1]);
    Console.WriteLine(clienteService.Agendar(cliente, nomePet, servico, dataHora));
    Console.ReadKey();
}

void MenuAgendamentosCliente(Cliente cliente)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("=== Meus Agendamentos ===");
        var lista = clienteService.MeusAgendamentos(cliente);

        if (lista.Count == 0)
        {
            Console.WriteLine("Nenhum agendamento.");
            Console.ReadKey();
            return;
        }

        for (int i = 0; i < lista.Count; i++)
        {
            var a = lista[i];
            Console.WriteLine($"{i + 1}. {a.DataHora:dd/MM/yyyy} - {a.DataHora:HH:mm} | {a.Servico} | Pet: {a.NomePet} | {a.Status}");
        }

        Console.WriteLine("\nEscolha um agendamento pelo numero ou 0 para voltar: ");
        Console.Write("Opcao: ");
        var input = Console.ReadLine();

        if (input == "0") return;

        if (!int.TryParse(input, out int idx) || idx < 1 || idx > lista.Count)
        {
            Console.WriteLine("Opcao invalida.");
            Console.ReadKey();
            continue;
        }

        var ag = lista[idx - 1];

        Console.Clear();
        Console.WriteLine("=== Agendamento ===");
        Console.WriteLine($"Data: {ag.DataHora:dd/MM/yyyy} - {ag.DataHora:HH:mm}");
        Console.WriteLine($"Servico: {ag.Servico}");
        Console.WriteLine($"Pet: {ag.NomePet}");
        Console.WriteLine($"Status: {ag.Status}");
        Console.WriteLine("\n1. Cancelar");
        Console.WriteLine("0. Voltar");
        Console.Write("Opcao: ");

        switch (Console.ReadLine())
        {
            case "1":
                Console.WriteLine(clienteService.CancelarAgendamento(cliente, ag.Id));
                Console.ReadKey();
                break;
        }
    }
}

// ── AREA DO FUNCIONARIO ──────────────────────────────────────────
void MenuFuncionario()
{
    Console.Clear();
    Console.WriteLine("=== Area do Funcionario (digite 0 para voltar) ===");
    Console.Write("Email: "); var email = Console.ReadLine() ?? "";
    if (email == "0") return;
    Console.Write("Senha: "); var senha = Console.ReadLine() ?? "";

    var func = funcService.Login(email, senha);
    if (func is null)
    {
        Console.WriteLine("Credenciais invalidas.");
        Console.ReadKey();
        return;
    }

    MenuFuncionarioLogado(func);
}

void MenuFuncionarioLogado(Funcionario func)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine($"=== Painel do Funcionario: {func.Nome} ===");
        Console.WriteLine("1. Ver agendamentos");
        Console.WriteLine("0. Logout");
        Console.Write("Opcao: ");

        switch (Console.ReadLine())
        {
            case "1":
                MenuGerenciarAgendamentos();
                break;

            case "0": return;
        }
    }
}

void MenuGerenciarAgendamentos()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("=== Agendamentos ===");
        var todos = funcService.TodosAgendamentos();

        if (todos.Count == 0)
        {
            Console.WriteLine("Nenhum agendamento.");
            Console.ReadKey();
            return;
        }

        for (int i = 0; i < todos.Count; i++)
        {
            var a = todos[i];
            Console.WriteLine($"{i + 1}. {a.DataHora:dd/MM/yyyy} - {a.DataHora:HH:mm} | {a.Servico} | Pet: {a.NomePet} | Cliente: {a.EmailCliente} | {a.Status}");
        }

        Console.WriteLine("\nEscolha um agendamento pelo numero ou 0 para voltar: ");
        Console.Write("Opcao: ");
        var input = Console.ReadLine();

        if (input == "0") return;

        if (!int.TryParse(input, out int idx) || idx < 1 || idx > todos.Count)
        {
            Console.WriteLine("Opcao invalida.");
            Console.ReadKey();
            continue;
        }

        var ag = todos[idx - 1];

        Console.Clear();
        Console.WriteLine("=== Agendamento ===");
        Console.WriteLine($"Data:    {ag.DataHora:dd/MM/yyyy} - {ag.DataHora:HH:mm}");
        Console.WriteLine($"Servico: {ag.Servico}");
        Console.WriteLine($"Pet:     {ag.NomePet}");
        Console.WriteLine($"Cliente: {ag.EmailCliente}");
        Console.WriteLine($"Status:  {ag.Status}");
        Console.WriteLine("\n1. Confirmar");
        Console.WriteLine("2. Cancelar");
        Console.WriteLine("0. Voltar");
        Console.Write("Opcao: ");

        switch (Console.ReadLine())
        {
            case "1":
                Console.WriteLine(funcService.ConfirmarAgendamento(ag.Id));
                Console.ReadKey();
                break;
            case "2":
                Console.WriteLine(funcService.CancelarAgendamento(ag.Id));
                Console.ReadKey();
                break;
        }
    }
}
