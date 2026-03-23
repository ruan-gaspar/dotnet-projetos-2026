# Livraria API — FIAP

API REST desenvolvida com ASP.NET Core, Oracle e RabbitMQ.

## Tecnologias
- .NET 9
- Entity Framework Core
- Oracle Database
- RabbitMQ (Docker)
- Swagger

## Como rodar
- Defina a string de conexão na variável:

```bash
export ConnectionStrings__OracleConnection="User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/ORCL"

dotnet run
```
