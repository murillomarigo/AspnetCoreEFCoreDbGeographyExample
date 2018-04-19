# Exemplo de uso de Geography do SQL Server

Fiz esse projeto apenas como forma de estudo de como usar o geography numa aplicação que usa EF Core.
Primeira coisa que eu fiz foi estudar como esse tipo de dado funciona direito no banco de dados (a versão que eu usei foi Microsoft SQL Server 2016 (SP1))

## Uso direto no SQL

### Criar uma tabela com uma coluna tipo geography

    CREATE TABLE [dbo].[Locais] (
        [Id]    INT               IDENTITY (1, 1) NOT NULL,
        [Nome]  VARCHAR (100)     NOT NULL,
        [Ponto] [sys].[geography] NOT NULL
    );

### Inseririndo um dado geography
    insert into dbo.Locais (nome, ponto) values ('Local', geography::STGeomFromText('POINT (-22.951911 -43.2126759)', 4326))

### Retorna registros ordenados pela distancia de um ponto dado

    DECLARE @g geography = 'POINT (-23.5614091 -46.6580706)';  
    SELECT Nome, Ponto.ToString(),Ponto.STDistance(@g) FROM Locais
    order by Ponto.STDistance(@g)

Isso é bem básico, mas já atende o que eu vou precisar no futuro

## Usando com EF Core

Basicamente ainda não foi implementado esse tipo de dado no EF Core, mas li em algum lugar que esta no roadmap. 
Então para usar esse tipo de dado é necessario fazer as querys como string e passar para o DbContext executa-las.
Na modelagem do banco é possivel mapear uma propriedade do modelo como string e o usar o HasColumnType para definir como tipo sys.geography.
O problema que eu encontrei apenas é que ao tentar resgatar do banco as entidades que tem esse tipo no banco o EF joga uma exceção que o tipo Udt não é compativel, por enquanto eu estou usando FromSql, mas eu já encontrei uma solução no stackoverflow para o mesmo problema https://stackoverflow.com/questions/44873740/entity-framework-core-type-udt-is-not-supported-on-this-platform-spatial-data