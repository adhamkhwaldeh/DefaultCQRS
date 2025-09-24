# AlJawad.DefaultCQRS

A powerful and scalable CQRS library for .NET applications.

## Table of Contents

- [Introduction](#introduction)
- [Getting Started](#getting-started)
  - [Installation](#installation)
  - [Configuration](#configuration)
- [Usage](#usage)
  - [Commands](#commands)
  - [Queries](#queries)
- [Advanced Topics](#advanced-topics)
  - [Validation](#validation)
  - [Unit of Work](#unit-of-work)
- [Contributing](#contributing)
- [License](#license)

## Introduction

`AlJawad.DefaultCQRS` is a .NET library that provides a ready-to-use implementation of the Command Query Responsibility Segregation (CQRS) pattern. It's designed to be highly generic and scalable, allowing you to quickly build robust and maintainable applications.

The library offers a set of default commands and queries for common CRUD operations, as well as support for caching, validation, and a unit of work. This allows you to focus on your business logic instead of writing boilerplate code.

### Key Benefits

- **Reduce Boilerplate Code:** The library provides a set of generic commands, queries, and handlers that can be used for any entity. This eliminates the need to write repetitive code for basic CRUD operations.
- **Scalability:** By separating read and write operations, CQRS allows you to scale each side of your application independently. This library is designed to support this scalability.
- **Flexibility:** The library is highly customizable. You can easily extend the default commands and queries or create your own to meet your specific needs.
- **Testability:** The separation of concerns in CQRS makes it easier to test your application's business logic.
- [Getting Started](#getting-started)
  - [Installation](#installation)
  - [Configuration](#configuration)

## Getting Started

### Installation

To install the library, you can use the .NET CLI:

```bash
dotnet add package AlJawad.DefaultCQRS
```

### Configuration

Configuring the `AlJawad.DefaultCQRS` library involves two main steps: initializing the library's services and configuring your entities.

#### 1. Initialize the Library

First, you need to call the `InitializeDefaultCQRS` extension method in your `Program.cs` file. This method sets up the necessary model binders for the library.

```csharp
// In Program.cs

#region required for Default CQRS
builder.Services.InitializeDefaultCQRS();
#endregion
```

#### 2. Configure Your Entities

For each entity you want to use with the library, you'll need to call the `AddEntityDynamicConfiguration` extension method. This method registers the default command and query handlers for your entity.

Here's an example of how to configure a `Product` entity:

```csharp
// In Program.cs

#region required for Default CQRS
builder.Services.AddEntityDynamicConfiguration<Product, int, CreateProductDto, UpdateProductDto, ProductDto, ProductAuthorizationHandler>(builder.Configuration);
#endregion
```

In this example:
- `Product` is the entity class.
- `int` is the type of the entity's primary key.
- `CreateProductDto` is the data transfer object (DTO) for creating a new product.
- `UpdateProductDto` is the DTO for updating an existing product.
- `ProductDto` is the DTO for reading a product.
- `ProductAuthorizationHandler` is the authorization handler for the `Product` entity.

You will also need to configure the database connection string in your `appsettings.json` file.
- [Usage](#usage)
  - [Commands](#commands)
  - [Queries](#queries)

## Usage

Once you've configured the library, you can use the `IMediator` interface to send commands and queries.

### Commands

The library provides a set of default commands for common CRUD operations.

#### Create

To create a new entity, you can send an `EntityCreateCommand`.

```csharp
var command = new EntityCreateCommand<CreateProductDto, Response<ProductDto>>(createProductDto);
var response = await _mediator.Send(command);
```

#### Update

To update an existing entity, you can send an `EntityUpdateCommand`.

```csharp
var command = new EntityUpdateCommand<int, UpdateProductDto, Response<ProductDto>>(productId, updateProductDto);
var response = await _mediator.Send(command);
```

#### Delete

To delete an entity, you can send an `EntityDeleteCommand`.

```csharp
var command = new EntityDeleteCommand<int, Response<ProductDto>>(productId);
var response = await _mediator.Send(command);
```

### Queries

The library also provides a set of default queries for reading data.

#### Get by ID

To retrieve an entity by its ID, you can send an `EntityIdentifierQuery`.

```csharp
var query = new EntityIdentifierQuery<int, Response<ProductDto>>(productId);
var response = await _mediator.Send(query);
```

#### Get a List

To retrieve a list of entities, you can send an `EntityListQuery`.

```csharp
var query = new EntityListQuery<ResponseArray<ProductDto>>();
var response = await _mediator.Send(query);
```

#### Get a Paged List

To retrieve a paged list of entities, you can send an `EntityPagedQuery`.

```csharp
var query = new EntityPagedQuery<ResponseList<ProductDto>>(pageNumber, pageSize);
var response = await _mediator.Send(query);
```
- [Advanced Topics](#advanced-topics)
  - [Validation](#validation)
  - [Unit of Work](#unit-of-work)

## Advanced Topics

### Validation

The library uses FluentValidation to validate commands. This is handled by the `ValidateEntityModelCommandBehavior`, which is automatically registered when you configure your entities.

You can create validators for your command DTOs, and they will be automatically applied. For example, to create a validator for the `CreateProductDto`, you would create a class that inherits from `BaseValidator<CreateProductDto, Product, long, ProductDto>`.

Here's an example of a validator for the `CreateProductDto`:

```csharp
public class ProductCreateValidator : BaseValidator<CreateProductDto, Product, long, ProductDto>
{
    public ProductCreateValidator(IUnitOfWork unitOfWork, IDistributedCache cache)
        : base(unitOfWork, cache)
    {
        var _repository = unitOfWork.Set<Product>();

        RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Required");
        RuleFor(x => x.Price).NotNull().WithMessage("Required");
        RuleFor(e => e).Custom((p, context) =>
        {
            var alreadyExist = _repository.FirstOrDefault(x => x.Name == p.Name);
            if (alreadyExist != null)
            {
                context.AddFailure(new ValidationFailure("Name", "Already Existed"));
            }
        });
    }
}
```

### Unit of Work

The library uses the Unit of Work pattern to ensure that all database operations within a single command are transactional. The `UnitOfWork` class is automatically registered with the dependency injection container and can be injected into your command handlers.
- [Contributing](#contributing)
- [License](#license)