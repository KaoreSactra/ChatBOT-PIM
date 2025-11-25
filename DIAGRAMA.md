# Diagrama de Classe do Banco de Dados - ChatBOT-PIM

## Visão Geral

Este documento descreve a estrutura das classes de entidade do banco de dados do sistema ChatBOT-PIM.

---

## Diagrama UML (Texto)

```
┌─────────────────────────────┐
│         User                │
├─────────────────────────────┤
│ - Id: int (PK)              │
│ - Email: string (Required)  │
│ - PasswordHash: string (R)  │
│ - Role: string (Required)   │
└─────────────────────────────┘


┌─────────────────────────────┐
│      ChatMessage            │
├─────────────────────────────┤
│ - Role: string? (user/asst) │
│ - Content: string?          │
└─────────────────────────────┘
```

---

## Descrição das Entidades

### 1. User (Usuário)

Representa um usuário registrado no sistema.

| Propriedade      | Tipo     | Restrições             | Descrição                                        |
| ---------------- | -------- | ---------------------- | ------------------------------------------------ |
| **Id**           | `int`    | PK, Auto-increment     | Identificador único do usuário                   |
| **Email**        | `string` | Required, EmailAddress | Email único para login e identificação           |
| **PasswordHash** | `string` | Required               | Senha criptografada do usuário                   |
| **Role**         | `string` | Required               | Papel/permissão do usuário (ex: "user", "admin") |

**Anotações:**

- `@Required`: Todos os campos principais são obrigatórios
- `@EmailAddress`: Valida o formato do email

---

### 2. ChatMessage (Mensagem de Chat)

Representa uma mensagem na conversa do chat com a IA.

| Propriedade | Tipo      | Restrições | Descrição                                 |
| ----------- | --------- | ---------- | ----------------------------------------- |
| **Role**    | `string?` | Opcional   | Define o remetente: "user" ou "assistant" |
| **Content** | `string?` | Opcional   | Conteúdo da mensagem                      |

**Nota:** Esta entidade não possui chave primária configurada explicitamente. Pode ser usada como modelo para requisições/respostas de chat.

---

## Diagrama de Contexto (DbContext)

```
┌──────────────────────────────────┐
│     ApiDbContext                 │
│  (Entity Framework Context)       │
├──────────────────────────────────┤
│ + Users: DbSet<User>             │
│ + OnConfiguring()                │
│ + OnModelCreating()              │
└──────────────────────────────────┘
         │
         ├──> Users Table
         │
         └──> ChatMessage (Modelo)
```

---

## Relacionamentos

Atualmente, o banco de dados possui:

- **Sem relacionamentos diretos** configurados entre as entidades
- `User` é uma entidade independente
- `ChatMessage` é um modelo de transferência de dados (DTO-like)

### Relacionamentos Potenciais para Expansão

```
User (1) ──────────── (N) ChatMessage
   │
   └─ Cada usuário pode ter múltiplas mensagens de chat
```

---

## Scripts SQL Equivalentes

### Tabela Users

```sql
CREATE TABLE [Users] (
    [Id] INT NOT NULL IDENTITY(1,1),
    [Email] NVARCHAR(255) NOT NULL,
    [PasswordHash] NVARCHAR(MAX) NOT NULL,
    [Role] NVARCHAR(50) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
    CONSTRAINT [UQ_Users_Email] UNIQUE ([Email])
);
```

---

## Tecnologias Utilizadas

- **ORM:** Entity Framework Core
- **Banco de Dados:** SQL Server / SQLite
- **Validação:** Data Annotations

---

## Observações

1. **ChatMessage** não está configurado como `DbSet` no contexto, funcionando como modelo de DTO
2. Recomenda-se adicionar campos de auditoria como `CreatedAt`, `UpdatedAt` para melhor rastreamento
3. Considerar adicionar uma tabela de `ChatHistory` para persistir mensagens em banco de dados
4. O Role pode ser normalizado com uma tabela separada de `Roles` para maior flexibilidade

---

## Próximos Passos Sugeridos

- [ ] Normalizar a coluna `Role` com uma tabela de referência
- [ ] Adicionar índices para melhor performance
- [ ] Implementar `ChatHistory` como entidade persistida
- [ ] Adicionar campos de auditoria (CreatedAt, UpdatedAt, DeletedAt)
- [ ] Configurar relacionamentos de chave estrangeira se necessário
