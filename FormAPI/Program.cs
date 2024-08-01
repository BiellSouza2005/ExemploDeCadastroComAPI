using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Adiciona políticas do Cors
const string policyName = "CorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policyName, builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

//Essa linha adiciona deendência do entity framework do projeto para uso pela API
builder.Services.AddDbContext<DatabBaseContext>();


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(policyName);

//AdicionaCliente endpoint para adicionar um novo cliente
app.MapPost("/AdicionarCliente", async (Cliente cliente, DatabBaseContext db) =>
{
    db.Clientes.Add(cliente);
    await db.SaveChangesAsync();

    return Results.Created($"/GetCliente/{cliente.Id}", cliente);
})
.WithName("AdicionarCliente")
.WithOpenApi();


//Retorna todos os clientes já cadastrados
app.MapGet
(
    "/RetornaClientes", async (DatabBaseContext db) =>
    await db.Clientes.ToListAsync()
)
.WithName("RetornaClientes")
.WithOpenApi();


//Retorna Cliente por parâmetro (id)
app.MapGet
(
    "/RetornaClientePeloID/{id}", async (int id, DatabBaseContext db) =>
    await db.Clientes.FindAsync(id)
        is Cliente cliente
            ? Results.Ok(cliente)
            : Results.NotFound()
)
.WithName("RetornaClientePeloID")
.WithOpenApi();

//Altera dados do Cliente pelo ID
app.MapPut("/AlteraInfoClientePeloID/{id}", async (int id, Cliente clienteAtualizado, DatabBaseContext db) =>
{
    var cliente = await db.Clientes.FindAsync(id);

    if (cliente is null) return Results.NotFound();

    cliente.Nome = clienteAtualizado.Nome;
    cliente.CPF = clienteAtualizado.CPF;
    cliente.Usuario = clienteAtualizado.Usuario;
    cliente.Senha = clienteAtualizado.Senha;
    cliente.DataNascimento = clienteAtualizado.DataNascimento;
    cliente.Cidade = clienteAtualizado.Cidade;
    cliente.Estado = clienteAtualizado.Estado;


    await db.SaveChangesAsync();

    return Results.NoContent();
})
.WithName("AlteraInfoClientePeloID")
.WithOpenApi();


//Deleta Cliente por ID
app.MapDelete("/DeletaClientePeloID/{id}", async (int id, DatabBaseContext db) =>
{
    if (await db.Clientes.FindAsync(id) is Cliente cliente)
    {
        db.Clientes.Remove(cliente);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
})
.WithName("DeletaClientePeloID")
.WithOpenApi();

app.Run();

