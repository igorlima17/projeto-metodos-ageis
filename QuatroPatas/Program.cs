using QuatroPatas;

var auth = new AuthService();

while (true)
{
    Console.Clear();
    Console.WriteLine("=== QuatroPatas ===");
    Console.WriteLine("1. Cadastrar");
    Console.WriteLine("2. Login");
    Console.WriteLine("0. Sair");
    Console.Write("Opcao: ");

    switch (Console.ReadLine())
    {
        case "1":
            Console.Write("Nome: "); var nome = Console.ReadLine() ?? "";
            Console.Write("Email: "); var email = Console.ReadLine() ?? "";
            Console.Write("Senha: "); var senha = Console.ReadLine() ?? "";
            Console.WriteLine(auth.Cadastrar(nome, email, senha));
            Console.ReadKey();
            break;

        case "2":
            Console.Write("Email: "); var loginEmail = Console.ReadLine() ?? "";
            Console.Write("Senha: "); var loginSenha = Console.ReadLine() ?? "";
            var user = auth.Login(loginEmail, loginSenha);

            if (user is null)
            {
                Console.WriteLine("Email ou senha incorretos.");
                Console.ReadKey();
                break;
            }

            MenuLogado(user);
            break;

        case "0":
            return;
    }
}

void MenuLogado(Usuario user)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine($"=== Bem-vindo, {user.Nome}! ===");
        Console.WriteLine("1. Ver meus dados");
        Console.WriteLine("2. Editar dados");
        Console.WriteLine("3. Deletar conta");
        Console.WriteLine("0. Logout");
        Console.Write("Opcao: ");

        switch (Console.ReadLine())
        {
            case "1":
                Console.WriteLine($"\nNome: {user.Nome}");
                Console.WriteLine($"Email: {user.Email}");
                Console.ReadKey();
                break;

            case "2":
                Console.Write("Novo nome: "); var novoNome = Console.ReadLine() ?? "";
                Console.Write("Novo email: "); var novoEmail = Console.ReadLine() ?? "";
                Console.WriteLine(auth.Atualizar(user, novoNome, novoEmail));
                Console.ReadKey();
                break;

            case "3":
                Console.WriteLine(auth.Deletar(user));
                Console.ReadKey();
                return;

            case "0":
                return;
        }
    }
}
