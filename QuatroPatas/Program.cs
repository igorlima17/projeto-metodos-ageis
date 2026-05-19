using QuatroPatas;

var db = new Database();
var clienteService = new ClienteService(db);
var funcService = new FuncionarioService(db);

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
                if (MenuConfiguracoes(cliente)) return;
                break;

            case "0": return;
        }
    }
}

// Retorna true se a conta foi deletada (para sair do loop do cliente logado)
bool MenuConfiguracoes(Cliente cliente)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine($"=== Configuracoes ===");
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
                Console.WriteLine($"- {p.Nome} | {p.Especie} | {p.Raca}");

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
                Console.WriteLine(clienteService.CadastrarPet(cliente, nome, especie, raca));
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

    Console.WriteLine("Seus pets:");
    foreach (var p in cliente.Pets)
        Console.WriteLine($"  - {p.Nome}");
    Console.Write("Nome do pet: "); var nomePet = Console.ReadLine() ?? "";

    Console.WriteLine("\nServicos:");
    Console.WriteLine("1. Banho");
    Console.WriteLine("2. Tosa");
    Console.WriteLine("3. Consulta Veterinaria");
    Console.Write("Opcao: ");
    var servico = Console.ReadLine() switch
    {
        "1" => Servico.Banho,
        "2" => Servico.Tosa,
        "3" => Servico.Consulta,
        _ => Servico.Banho
    };

    Console.Write("Data e hora (dd/MM/yyyy HH:mm): ");
    if (!DateTime.TryParse(Console.ReadLine(), out var dataHora))
    {
        Console.WriteLine("Data invalida.");
        Console.ReadKey();
        return;
    }

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
            Console.WriteLine("Nenhum agendamento.");
        else
            foreach (var a in lista)
                Console.WriteLine($"[{a.Id[..8]}] {a.DataHora:dd/MM/yyyy HH:mm} | {a.Servico} | Pet: {a.NomePet} | {a.Status}");

        Console.WriteLine("\n1. Cancelar agendamento");
        Console.WriteLine("0. Voltar");
        Console.Write("Opcao: ");

        switch (Console.ReadLine())
        {
            case "1":
                Console.Write("ID do agendamento (primeiros 8 chars): "); var idParcial = Console.ReadLine() ?? "";
                var ag = lista.FirstOrDefault(a => a.Id.StartsWith(idParcial));
                Console.WriteLine(ag is null ? "Nao encontrado." : clienteService.CancelarAgendamento(cliente, ag.Id));
                Console.ReadKey();
                break;

            case "0": return;
        }
    }
}

// ── AREA DO FUNCIONARIO ──────────────────────────────────────────
void MenuFuncionario()
{
    Console.Clear();
    Console.WriteLine("=== Area do Funcionario ===");
    Console.Write("Email: "); var email = Console.ReadLine() ?? "";
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
            Console.WriteLine($"{i + 1}. {a.DataHora:dd/MM/yyyy HH:mm} | {a.Servico} | Pet: {a.NomePet} | Cliente: {a.EmailCliente} | {a.Status}");
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
        Console.WriteLine($"=== Agendamento ===");
        Console.WriteLine($"Data:    {ag.DataHora:dd/MM/yyyy HH:mm}");
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
