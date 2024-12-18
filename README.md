# TaskManager

Esse projeto é uma implementação de cadastro de projetos e tarefas seguindo princípios de Domain-Driven Design (DDD) e Command Query Responsibility Segregation (CQRS).

## Execução simplificada

1 - Baixe e instale o Docker em sua máquina;
2 - Renomeie o arquivo .env-example para .env
3 - Execute o arquivo "up.bat".

## Tecnologias Utilizadas

- .NET Core 8: Framework utilizado para o desenvolvimento da aplicação.
- Entity Framework Core: Para interagir com o banco de dados relacional (SQL Server).
- SQL Server: Banco de dados relacional para escrita e local para facilitar a execução.
- Auto History: Utilizado biblioteca de Auto History para gravar histórico de dados ao alterar e incluir
- MongoDB: Banco de dados NoSQL utilizado para leitura.
- MediatR: Para mensageria interna e implementação do padrão CQRS.
- Swagger: Para documentação e testes da API.
- FluentValidation: Para validação de comandos.
- xUnit e Moq: Para testes unitários no backend.
- Containers: Aplicação possui containers para facilitar a execução.

## Arquitetura BackEnd

- Core: Contém todas as entidades abstratas para utilização nas demais camadas
- Domain: Contém as entidades de domínio, eventos e interfaces de repositórios.
- Infra: Contém a implementação dos repositórios e contextos de dados.
- Application: Contém comandos, consultas, manipuladores.
- API: Camada de apresentação, contém os controladores.

### Funcionalidades

- Cadastro de Projetos: Inclui operações de criação, leitura, atualização e exclusão (CRUD).
- Cadastro de Tarefas: Inclui operações de criação, leitura, atualização e exclusão (CRUD).
- Extraçã ode relatórios: Inclui operações de leitura personalizada.
- CQRS Implementado: Separação clara entre operações de escrita e leitura.
- Mensageria Interna: Utilização do MediatR para comunicação entre componentes da aplicação para sincronizar dados entre o banco de escrita e leitura.
- Validações com FluentValidation: Validações de comandos de entrada.

## Estrutura do Projeto Backend

```sh
TaskManager
│
├── src/
│   ├── TaskManager.API/
│   │   ├── Controllers/
│   │   ├── Program.cs
│   │   └── ...
│   ├── TaskManager.Application/
│   │   ├── Commands/
│   │   ├── Queries/
│   │   ├── Events/
│   │   └── ...
│   ├── TaskManager.Domain/
│   │   ├── ClientAggregate/
│   │   ├── Data/
│   │   ├── DomainObjects/
│   │   └── ...
│   ├── TaskManager.Infra/
│   │   ├── Contexts/
│   │   ├── Repositories/
│   │   └── ...
│   ├── TaskManager.Core/
│   │   ├── CrossCutting/
│   │   ├── Data/
│   │   └── ...
|   tests/
│   ├── TaskManager.Tests/
│   |   ├── Commands/
│   |   ├── Queries/
│   |   ├── Events/
│   |   └── ...
├── README.md
└── Dockerfile
```


#### Testes Unitários Backend

Os testes unitários para comandos, consultas e manipuladores de eventos são implementados utilizando xUnit e Moq para mock. Estes testes garantem a integridade das operações de criação, leitura, atualização e exclusão (CRUD) e a correta publicação de eventos.

##### Executando os Testes Backend

Para executar os testes unitários, utilize o seguinte comando no diretório do projeto de testes:

```sh
dotnet test
```

# Para Criar uma Migration
É necessário estar no diretório que contenha o Contexto do EF Core da aplicação.

`cd src/TaskManager.Infra`

Em seguida execute o comando para gerar a nova migration apontando para o projeto de startup, esse projeto deve ter obrigatoriamente o pacote Microsoft.EntityFrameworkCore.Design instalado.

Mesmo sendo uma má pratica utilizá-lo, haja visto que é uma necessidade inerente a utilização da estratégia do Database Tenant Provider ©. :).

`dotnet ef migrations add --startup-project ../TaskManager.API [NAME_MIGRATION] --context Context`

Se atente para o nome da sua migration, ela será imutável e irá perdurar por toda a vida do projeto :p, o EF Core irá criar um prefixo com a data atual, portanto não é necessário inserir no nome da migration nenhuma informação referente a data que você está criando-a.

# Para Atualizar a Base de Dados
Ainda no diretório que se encontra o Context da aplicação, execute o comando:

`dotnet ef database update --startup-project ../TaskManager.API --context Context`

# Para Visualizar o SQL da Migration
Ainda no diretório que se encontra o Context da aplicação, execute o comando:

`dotnet ef migrations script --startup-project ../TaskManager.Api`


# Fase 2: Refinamento
Para a segunda fase, escreva no arquivo README.md em uma sessão dedicada, o que você perguntaria para o PO visando o refinamento para futuras implementações ou melhorias.

1 - O que temos atende uma demanda inicial? O que precisa ser implementado para que essa demanda inicial seja atendida?
2 - Precisamos de prazos para execução das tarefas ou projetos? Caso sim, devemos avisar dos prazos para os usuários relacionados?
3 - Os históricos que armazenamos está bom? Precisa de mais detalhes?
4 - Quando podemos iniciar testes com o Front?

# Fase 3: Final
Na terceira fase, escreva no arquivo README.md em uma sessão dedicada o que você melhoraria no projeto, identificando possíveis pontos de melhoria, implementação de padrões, visão do projeto sobre arquitetura/cloud, etc.

1 - Padrão nos retornos da API
2 - Padrão de retorno para os validators
3 - Padrão para receber objetos na mensageria ao invés de string
4 - Tabela de comentários com usuários para as tasks
5 - Melhoria nas consultas e gravações das tabelas de leitura
6 - Melhoria na cobertura dos testes
7 - Realizar testes de integração
