
# Hackaton Pós-Tech

A partir de uma POC (prove of concept) em .NET com C#, que realiza um processamento de um vídeo e retorna as imagens em um arquivo .zip, foi proposto o desafio de aprimoprar o projeto utilizando boas práticas, tais como DDD, Clean Architecture, Qualidade de Software e Mensageria.

### Para isso, o sistema comprta as seguintes funcionalidades:

- Processar mais de um vídeo;
- Em caso de picos o sistema não perde uma requisição;
- O sistema tem uma tela para receber os inputs;
- O sistema tem um local de armazenamento de dados, logo tem uma tela de listagem de envios;

### Casos de uso
![image](https://github.com/leilaoligama/HackathonPosTech/assets/37404819/39d5334c-1490-4faa-b6ae-88c0a3cf01cf)

### Fluxograma
![image](https://github.com/leilaoligama/HackathonPosTech/assets/37404819/dbb5e680-df4d-46d5-8890-868249222c9d)

### Arquitetura do Sistema
![image](https://github.com/leilaoligama/HackathonPosTech/assets/37404819/b774b080-1b73-445c-b9ae-f10941a73c89)

## Stack utilizada

**Front-end:** .NET 7, Razor Pages, C#

**Back-end:** C#, .NET 7, Worker Service, Service Bus, SQL Server

## Scripts de criação do Banco de Dados

```bash
CREATE TABLE [dbo].[upload]
  (
     [id]            [INT] IDENTITY(1, 1) NOT NULL,
     [nome]          [VARCHAR](50) NULL,
     [email]         [VARCHAR](50) NULL,
     [caminhoupload] [VARCHAR](max) NULL,
     [caminhozip]    [VARCHAR](max) NULL,
     CONSTRAINT [PK_Upload] PRIMARY KEY CLUSTERED ( [id] ASC )WITH (
     statistics_norecompute = OFF, ignore_dup_key = OFF,
     optimize_for_sequential_key = OFF) ON [PRIMARY]
  )
ON [PRIMARY]
textimage_on [PRIMARY]
go 
```




