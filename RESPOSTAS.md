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

Ele automatiza comportamentos para simplificar a criação de APIs. Na API Livraria, ele reduz a quantidade de código, mas mantém a segurança e a padronização.
Se um JSON for enviado com campo obrigatório vazio, vai retornar status code 400 Bad Request.

**3.3)** Por que o método `GetById` retorna `NotFound()` em vez de retornar `null`? Qual a diferença para o cliente da API?

NotFound é mais assertivo na interpretação. Se retornasse null, a margem de interpretação disso seria muito maior, enquanto que notfound é mais direcionado, evidenciando que o recurso buscado não existe. No GET por ID da API, o notfound aparece quando o usuário tenta buscar um id que não existe, muito mais coerente e esclarecedor do que retornar null.

### Sobre Entity Framework Core

**3.4)** O que é o Change Tracker do EF Core? Explique o que acontece internamente quando você chama `_ctx.SeuDbSet.Add(objeto)` seguido de `SaveChangesAsync()`.

É o mecanismo que rastreia mudanças na memória. Mais especificamente, nas entidades em memória. Ele, basicamente, marca essa entidade quando o método é chamado, mas sem alterar o banco. Até que, quando o segundo método SaveChangesAsync é chamado, o banco é alterado. Com isso, o desenvolvimento fica assíncrono, ou seja, tudo vai sendo salvo em memória mas só o EF Core gerencia e sincroniza as alterações.

**3.5)** Qual a diferença entre `FindAsync(id)` e `ToListAsync()`? Qual SQL cada um gera?

FindAssync(id) é usado para buscar um registro específico, passando o id como parâmetro de busca, enquanto que o ToListAsync() busca todos os registros da tabela. O segundo método não tem um filtro como o id, então ele retorna uma quantidade de dados muito maior. O primeiro comando é um WHERE passando o id enquanto que o segundo comando é um SELECT * FROM.

**3.6)** Por que usamos `EntityState.Modified` no PUT ao invés de buscar o objeto primeiro e alterar campo a campo?

Porque o EntityState.Modified passa para o EF uma alteração na entidade sem ter que fazer uma busca antes. O EF então, de forma automática, gera um UPDATE no banco com o que foi passado. Parece bem simples, mas isso gera ganho de performance em termos de quantidade de código.

### Sobre Mensageria

**3.7)** Qual a diferença entre comunicação síncrona e assíncrona? Dê um exemplo real (fora do projeto) de cada uma.

Como os próprios nomes nos permite intuir, se trata de eventos que, em um caso, faz parte de uma sequência de ações, enquanto que em outro, não é dependente disso. Ou seja, comunicação síncrona segue uma ordem onde existe uma requisição e uma resposta é obrigatória antes da execução ou processamento continuar. Já na comunicação assíncrona, essa sequência não é necessária. A execução continua a acontecer, independente da resposta ser imediata ou não. No projeto isso foi visto nas filas do RabbitMQ, onde as requisições são assíncronas e independentes. 
Trazendo para o mundo real, podemos comparar os dois tipos de comunicação com um serviço de e-mail e um telefonema. Enquanto que o telefonema retrata uma comunicação síncrona, onde uma pessoa aguarda a resposta da outra antes de continuar o assunto, no e-mail isso não acontece, haja vista que permite respostas diferidas, indenpendente de tempo. 
Outro exemplo prático é a requisição HTTP padrão ao consultar um site, onde as ações seguintes dependem da resposta. Já no caso de assincronicidade, temos como exemplo uma mensagem enviada para processamento em segundo plano. 

**3.8)** O que é o ACK (Acknowledge) no RabbitMQ? O que acontece se o Consumer processar a mensagem mas NÃO enviar o ACK?

É um recurso para confirmar que uma mensagem foi processada pelo consumer. Quando isso acontece, a mensagem é removida da fila. Se um consumer não envia o ACK, O RabbitMQ entende como falha de processamento, podendo reenviar a mensagem para outro consumer, resguardando a perda de mensagens e de dados.

**3.9)** Por que o `RabbitMqConsumer` herda de `BackgroundService` e não de `ControllerBase`? Qual a diferença de ciclo de vida?

**3.10)** Se o RabbitMQ estiver fora do ar no momento do POST, o que acontece? O produto é salvo no Oracle? A API retorna erro? Sugira uma melhoria para tratar esse caso.

