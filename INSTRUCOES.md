# Atividade Prática — API REST + Oracle + Mensageria

**Professor:** Rafael S Novo Pereira  
**Entrega:** Código-fonte do projeto no GitHub ou ZIP  
**Valor:** A definir pelo professor

---

## Contexto

Vocês aprenderam a construir uma API REST com ASP.NET Core, Entity Framework Core, Oracle e RabbitMQ. Agora é hora de aplicar tudo em um cenário diferente.

Escolha **um** dos cenários abaixo (ou proponha um próprio com aprovação do professor):

| Cenário | Entidade principal | Campos sugeridos |
|---------|-------------------|------------------|
| A — Livraria | `Livro` | Id, Titulo, Autor, Preco, AnoPublicacao |
| B — Clínica | `Paciente` | Id, Nome, Cpf, DataNascimento, Email |
| C — Estoque | `Item` | Id, Nome, Quantidade, PrecoUnitario, Categoria |
| D — RH | `Funcionario` | Id, Nome, Cargo, Salario, DataAdmissao |

---

## Parte 1 — API + Oracle (obrigatória)

Construir o projeto do zero seguindo os mesmos 8 passos da aula.

### Entregáveis

**1.1 — Projeto ASP.NET Core Web API**
- Criar o projeto no Visual Studio com o nome adequado ao cenário (ex: `FiapLivraria`, `FiapClinica`)
- Instalar os 4 pacotes NuGet

**1.2 — Model**
- Criar a classe com **pelo menos 5 propriedades** (incluindo Id)
- Usar as annotations corretas:
  - `[Key]` e `[DatabaseGenerated]` no Id
  - `[Required]` nos campos obrigatórios
  - `[StringLength(n)]` nos campos texto
  - `[Table("NomeDaTabela")]` na classe

**1.3 — DbContext**
- Criar `AppDbContext` na pasta `Data/`
- Registrar o `DbSet<SuaEntidade>`

**1.4 — Configuração**
- Connection String no `appsettings.json` apontando para o Oracle FIAP
- `Program.cs` com `AddDbContext` + `UseOracle`

**1.5 — Migration**
- Executar `Add-Migration` e `Update-Database`
- A tabela deve existir no Oracle

**1.6 — Controller CRUD**
- Criar `SuaEntidadeController.cs` na pasta `Controllers/`
- Implementar os 5 endpoints:

| Método | Rota | Ação | Retorno esperado |
|--------|------|------|-----------------|
| GET | /api/seurecurso | Listar todos | 200 + array JSON |
| GET | /api/seurecurso/{id} | Buscar por Id | 200 ou 404 |
| POST | /api/seurecurso | Criar novo | 201 + objeto com Id |
| PUT | /api/seurecurso/{id} | Atualizar | 204 |
| DELETE | /api/seurecurso/{id} | Remover | 204 ou 404 |

**1.7 — Teste no Swagger**
- Tirar **print** de cada operação funcionando no Swagger:
  - POST criando um registro (mostrar o 201)
  - GET listando os registros
  - GET por Id (mostrar o 200)
  - PUT atualizando (mostrar o 204)
  - DELETE removendo (mostrar o 204)

---

## Parte 2 — Mensageria com RabbitMQ (obrigatória)

### Entregáveis

**2.1 — Docker**
- Subir o RabbitMQ com Docker
- Tirar **print** do painel em `http://localhost:15672` mostrando a fila criada

**2.2 — Producer**
- Criar `Messaging/RabbitMqProducer.cs`
- O método `Publish<T>` deve serializar para JSON e enviar para uma fila com nome adequado ao seu cenário (ex: `"livro-criado"`, `"paciente-criado"`)

**2.3 — Integração no Controller**
- Adicionar a chamada `RabbitMqProducer.Publish(objeto)` no método POST (Create)
- A publicação deve acontecer **após** o `SaveChangesAsync`

**2.4 — Consumer**
- Criar `Messaging/RabbitMqConsumer.cs` herdando de `BackgroundService`
- O Consumer deve:
  - Conectar ao RabbitMQ
  - Escutar a fila correta
  - Ao receber mensagem: imprimir no console com `Console.WriteLine`
  - Enviar ACK manual

**2.5 — Registro no Program.cs**
- Adicionar `AddHostedService<RabbitMqConsumer>()`

**2.6 — Teste**
- Tirar **print** do Output Window do Visual Studio mostrando a mensagem recebida pelo Consumer após um POST no Swagger

---

## Parte 3 — Questões Teóricas (obrigatória)

Responder **no próprio repositório** (em um arquivo `RESPOSTAS.md` ou `RESPOSTAS.txt`):

### Sobre API e HTTP

**3.1)** Explique a diferença entre os códigos HTTP 200, 201, 204 e 404. Em qual situação cada um é retornado no seu Controller?

**3.2)** O que o atributo `[ApiController]` faz? O que acontece se você enviar um JSON com o campo obrigatório vazio?

**3.3)** Por que o método `GetById` retorna `NotFound()` em vez de retornar `null`? Qual a diferença para o cliente da API?

### Sobre Entity Framework Core

**3.4)** O que é o Change Tracker do EF Core? Explique o que acontece internamente quando você chama `_ctx.SeuDbSet.Add(objeto)` seguido de `SaveChangesAsync()`.

**3.5)** Qual a diferença entre `FindAsync(id)` e `ToListAsync()`? Qual SQL cada um gera?

**3.6)** Por que usamos `EntityState.Modified` no PUT ao invés de buscar o objeto primeiro e alterar campo a campo?

### Sobre Mensageria

**3.7)** Qual a diferença entre comunicação síncrona e assíncrona? Dê um exemplo real (fora do projeto) de cada uma.

**3.8)** O que é o ACK (Acknowledge) no RabbitMQ? O que acontece se o Consumer processar a mensagem mas NÃO enviar o ACK?

**3.9)** Por que o `RabbitMqConsumer` herda de `BackgroundService` e não de `ControllerBase`? Qual a diferença de ciclo de vida?

**3.10)** Se o RabbitMQ estiver fora do ar no momento do POST, o que acontece? O produto é salvo no Oracle? A API retorna erro? Sugira uma melhoria para tratar esse caso.

---

## Parte 4 — Desafio Extra (opcional, vale pontos adicionais)

Escolha **um ou mais** dos desafios abaixo:

### 4.1 — Segundo Model com Relacionamento
- Criar uma segunda entidade relacionada à primeira (ex: `Emprestimo` que referencia `Livro`, `Consulta` que referencia `Paciente`)
- Configurar a Foreign Key via annotation `[ForeignKey]`
- Criar nova Migration para a segunda tabela
- Implementar o CRUD da segunda entidade

### 4.2 — Consumer com Lógica
- Em vez de só imprimir no console, fazer o Consumer executar alguma lógica:
  - Cenário A: "enviar e-mail" simulado (Console.WriteLine com template de e-mail)
  - Cenário B: gravar um log em outra tabela no banco
  - Cenário C: validar regra de negócio (ex: item com quantidade < 0 gera alerta)

### 4.3 — Tratamento de Erros
- Adicionar `try/catch` no método POST do Controller
- Se o `Publish` falhar (RabbitMQ fora do ar), o POST ainda deve retornar 201 (o produto já foi salvo)
- Logar o erro com `Console.WriteLine` ou `ILogger`

### 4.4 — Múltiplas Filas
- Criar uma segunda fila (ex: `"livro-atualizado"`) que é publicada no PUT
- Criar um segundo Consumer que escuta essa fila

---

## Critérios de Avaliação

| Critério | Peso |
|----------|------|
| Parte 1 — API funcional com CRUD + prints | 40% |
| Parte 2 — Mensageria funcional + prints | 25% |
| Parte 3 — Respostas teóricas | 25% |
| Parte 4 — Desafio extra | 10% (bônus) |
| Código organizado e sem erros | Pré-requisito |

### O que será avaliado no código

- Estrutura de pastas correta (Models/, Data/, Controllers/, Messaging/)
- Annotations do Model condizentes com os tipos de dados
- Connection String configurada (pode mascarar a senha)
- Controller com os 5 endpoints retornando os códigos HTTP corretos
- Producer e Consumer funcionais
- Código limpo, sem código morto ou comentários desnecessários

### O que NÃO será aceito

- Projeto que não compila
- Prints falsificados ou de outro projeto
- Respostas teóricas copiadas (serão verificadas)
- Projeto idêntico ao de outro aluno

---

## Entrega

- **Formato:** Repositório GitHub (link) ou arquivo ZIP
- **Conteúdo:** Projeto + prints + arquivo RESPOSTAS.md
- **Prazo:** A definir pelo professor

---

## Estrutura esperada do projeto

```
SeuProjeto/
├── Controllers/
│   └── SuaEntidadeController.cs
├── Data/
│   └── AppDbContext.cs
├── Messaging/
│   ├── RabbitMqProducer.cs
│   └── RabbitMqConsumer.cs
├── Migrations/
│   └── (arquivos gerados)
├── Models/
│   └── SuaEntidade.cs
├── appsettings.json
├── Program.cs
├── RESPOSTAS.md
└── prints/
    ├── post-201.png
    ├── get-todos.png
    ├── get-por-id.png
    ├── put-204.png
    ├── delete-204.png
    ├── rabbitmq-painel.png
    └── consumer-output.png
```
