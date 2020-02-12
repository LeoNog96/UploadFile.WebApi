read -p "Digite o nome da migração: " migration

migration=${migration}

dotnet ef migrations add $migration