# ✅ ToDo API

API REST para gerenciamento de tarefas (**ToDo**), desenvolvida com **ASP.NET Core**, seguindo os princípios de **Clean Architecture**, com foco em **boas práticas**, **segurança** e **manutenibilidade**.

Este projeto tem caráter **educacional e de portfólio**, simulando uma aplicação real de produção.

---

## 🎯 Objetivo do Projeto

- Consolidar conhecimentos em **ASP.NET Core**
- Compor portfólio
- Aplicar **Clean Architecture**
- Implementar **autenticação e autorização** com JWT
- Utilizar **MediatR** para desacoplamento
- Praticar **boas práticas de API REST**
- Preparar base para testes automatizados e escalabilidade

---

## 🏗️ Arquitetura

O projeto segue **Clean Architecture**, separando responsabilidades em camadas bem definidas:

```
src/
 ├── ToDo.Api              → Controllers, Middlewares, Auth
 ├── ToDo.Application      → Use Cases, DTOs, Commands, Queries
 ├── ToDo.Domain           → Entidades, Enums, Regras de Negócio
 └── ToDo.Infrastructure  → EF Core, Repositórios, Persistência
```

### Principais conceitos aplicados

- Separação de responsabilidades
- Dependência apontando para o domínio
- Baixo acoplamento
- Alta testabilidade

---

## 🔐 Autenticação & Autorização

- Autenticação via **JWT (JSON Web Token)**
- Controle de acesso baseado em **roles**
    - `Admin`
    - `User`
- Proteção de endpoints com `[Authorize]`
- Restrições de acesso por perfil
- Claims corretamente mapeadas (`NameIdentifier`, `Role`)

---

## 🔗 Endpoints principais

### 👤 Autenticação

- `POST /api/auth/login`
- `POST /api/auth/register`

### 📋 Usuários

- `GET /api/users` → Admin
- `PUT /api/users/{id}` → Admin
- `DELETE /api/users/{id}` → Admin
- `PUT /api/users` → Usuário autenticado
- `DELETE /api/users` → Usuário autenticado

### ✅ Tarefas (ToDo)

- `POST /api/todos`
- `GET /api/todos`
- `PUT /api/todos/{id}`
- `DELETE /api/todos/{id}`

*(os endpoints podem variar conforme a implementação)*

---

## 🛠️ Tecnologias Utilizadas

- ASP.NET Core Web API
- C#
- Entity Framework Core
- MediatR
- JWT Bearer Authentication
- Swagger (OpenAPI)
- Clean Architecture

---

## 🧪 Testes via Swagger

A API está integrada ao **Swagger**, permitindo:

- Autenticação via JWT
- Teste de endpoints protegidos
- Visualização clara dos contratos da API

---

## ▶️ Como executar o projeto

### Pré-requisitos

- .NET SDK 10+
- PostgreSQL
- Visual Studio / VS Code

### Passos

```
git clone https://github.com/seu-usuario/todo-api.git
cd todo-api
dotnet restore
dotnet ef database update
dotnet run
```

---

## 🐳 Executando com Docker-Compose

O projeto também pode ser executado utilizando **Docker-Compose**, facilitando a configuração do ambiente e eliminando a necessidade de instalar dependências localmente (como banco de dados).

### 🔧 Pré-requisitos

- Docker
- Docker-Compose

---

### ▶️ Como executar

1. Na raiz do projeto, execute:

```
docker-compose up--build
```

1. Aguarde a inicialização dos containers
2. Acesse a API em:

```
http://localhost:8080/swagger
```

---

### 🧩 O que o Docker Compose sobe

- API ASP.NET Core
- Banco de dados (PostgreSQL)
- Rede interna para comunicação entre os serviços

## ⚙️ Configuração do arquivo `.env`

O projeto utiliza um arquivo **`.env`** para armazenar **variáveis de ambiente**, evitando que informações sensíveis fiquem versionadas no código-fonte.

Esse arquivo é consumido pelo **Docker Compose** durante a inicialização dos containers.

---

### 📄 Exemplo de `.env`

```
# ASP.NET Core
ASPNETCORE_URLS=http://+:8080

# JWT
JWT__ISSUER=ToDo.Api
JWT__AUDIENCE=ToDo.Api
JWT__KEY=super-secret-key-change-me

# Database
DatabaseConnection=Host=localhost;Database=Todo;Port=5432;Username=postgres;Password=...
```

---

### 🔐 JWT

- `JWT__ISSUER` → Emissor do token
- `JWT__AUDIENCE` → Público válido do token
- `JWT__KEY` → Chave usada para assinar o JWT

Essas variáveis são lidas automaticamente pelo ASP.NET Core através do `IConfiguration`.

---

### 🗄️ Banco de Dados

As variáveis de banco são utilizadas no `docker-compose.yml` para:

- configurar o container do banco
- montar a **connection string** da aplicação

Exemplo de connection string gerada:

```
DatabaseConnection=Host=localhost;Database=Todo;Port=5432;Username=postgres;Password=...
```

---

### 🧠 Convenção usada

O padrão `JWT__KEY` (com **duplo underline**) é uma convenção do ASP.NET Core para mapear configurações hierárquicas, equivalente a:

```
{
  "JWT": {
    "KEY":"super-secret-key-change-me"
  }
}
```

---

### ▶️ Fluxo de execução com `.env`

1. O Docker Compose carrega o arquivo `.env`
2. As variáveis são injetadas nos containers
3. O ASP.NET Core lê essas variáveis automaticamente
4. A aplicação inicia já configurada

---

## 📌 Boas práticas adotadas

- DTOs para comunicação externa
- Validações no Application Layer
- Controllers enxutos
- Regra de negócio isolada no domínio
- Segurança baseada em roles
- Código organizado e legível

---

## 👨‍💻 Autor

**Gustavo Soares**

Desenvolvedor .NET em início de carreira, focado em backend, APIs REST e boas práticas de arquitetura.

## Meu portfólio
https://sirsoares33.github.io/gustavosoares.github.io/index.html
