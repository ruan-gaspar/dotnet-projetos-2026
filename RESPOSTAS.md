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

