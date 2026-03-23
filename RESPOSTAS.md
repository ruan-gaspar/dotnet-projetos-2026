## Parte 3 — Questões Teóricas (obrigatória)

Responder **no próprio repositório** (em um arquivo `RESPOSTAS.md` ou `RESPOSTAS.txt`):

### Sobre API e HTTP

**3.1)** Explique a diferença entre os códigos HTTP 200, 201, 204 e 404. Em qual situação cada um é retornado no seu Controller?

**Aqui, apresento uma definição de cada status de acordo com o site devmedia.com.br. Em seguida, mostro a implementação com um exemplo no código:**

**200 Ok:** requisição foi bem sucedida, ou seja, foi processada e bem atendida, retornando informação para quem solicitou.

#### GET TODOS:

```bash
return Ok(livros);
```

- No controller, quando uma requisição é atendida e retorna dados, a resposta é 200. Isso vale para o GET por ID também.  

**201 Created:** esse mostra que a requisição foi bem atendida e um ou mais recursos foram criados em decorrência daquela ação.

#### POST:

```bash
return CreatedAtAction(...)
```

- No código, isso é implementado para mostrar que os dados de novo livro foram criados com sucesso, retornando o que foi criado com o ID.

**204 No Content:** esse código serve para dizer que o server não tem um conteúdo para resposta, mas os cabeçalhos podem ser úteis.

#### DELETE:

```bash
return NoContent();
```

- No controller, isso serve para mostrar que a ação funcionou mas não tenho nada para mostrar. Também tem no PUT.

**404 Not Found:** mostra que o server não retornou uma resposta adequada para o que foi requisitado, ou seja, o recurso buscado não foi encontrado.

#### GET ID:

```bash
if (livro == null)
    return NotFound();
```

- Nesse if, retorna 200 quando encontra o livro pelo ID passado ou 404 quando não encontra. 

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

