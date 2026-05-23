# 🐾 QuatroPatas

Sistema de gerenciamento para petshop desenvolvido em C# com persistência em JSON. O projeto simula o fluxo real de um petshop, com área separada para clientes e funcionários.

---

## 🛠️ Tecnologias

- C# / .NET 10
- JSON (persistência de dados)
- SHA-256 (hash de senha)

---

## ▶️ Como rodar

**Pré-requisitos:** ter o [.NET SDK](https://dotnet.microsoft.com/download) instalado.

```bash
git clone https://github.com/seu-usuario/quatropatas.git
cd QuatroPatas
dotnet run
```

Na primeira execução, a pasta `Data/` será criada automaticamente com um funcionário padrão:

- **Email:** admin@quatropatas.com
- **Senha:** admin123

---

## ✅ Funcionalidades

### Área do Cliente
- Cadastro e login
- Cadastrar e remover pets
- Agendar serviços (Banho, Tosa ou Consulta Veterinária)
- Escolha de data e horário disponíveis
- Visualizar e cancelar agendamentos
- Editar dados e deletar conta (com dupla confirmação)

### Área do Funcionário
- Login com credenciais pré-cadastradas
- Visualizar todos os agendamentos
- Confirmar ou cancelar agendamentos

---

## 📁 Estrutura do projeto

```
QuatroPatas/
├── Data/                        # JSONs gerados em tempo de execução (ignorado pelo git)
│   ├── clientes.json
│   ├── funcionarios.json
│   └── agendamentos.json
├── Models/                      # Classes que representam os dados do sistema
│   ├── Agendamento.cs           # Model de agendamento com enums de serviço e status
│   ├── Cliente.cs               # Model de cliente
│   ├── Funcionario.cs           # Model de funcionário
│   └── Pet.cs                   # Model de pet
├── Services/                    # Regras de negócio e persistência
│   ├── Database.cs              # Leitura e escrita dos arquivos JSON
│   ├── ClienteService.cs        # Regras de negócio do cliente
│   └── FuncionarioService.cs    # Regras de negócio do funcionário
├── Program.cs                   # Menus e fluxo da aplicação
└── QuatroPatas.csproj
```

---

## 📚 Sobre o projeto

Desenvolvido como projeto acadêmico para a disciplina de Métodos Ágeis. O objetivo foi aplicar os conceitos de POO (Programação Orientada a Objetos) em um sistema CRUD funcional, com separação de responsabilidades entre models, services e interface.
