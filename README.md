# UploadFile
webapi em .net core 3.1 para upload de arquivos com persistencia no banco e autenticação JWT

### Configuração do projeto
- dotnet core >= 3.1
- Postgresql >= 12

### Documentação
- URL_SERVER/swagger

### Restauração de dependencias
```
dotnet restore
```

### Compilando o projeto
```
dotnet build
```
### Rodando o projeto
```
dotnet run --project UploadFile.WebApi/UploadFile.WebApi.csproj
```

### Publicando o projeto para produção
```
dotnet publish --release
```