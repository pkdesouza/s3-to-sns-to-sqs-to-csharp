# Projeto bemobi
Aplicação .net core que leia os eventos de notificação do AWS S3 PutObject a partir de uma fila SQS

# Tecnologias
.NET Core 6 + MySql + Docker + AWS 

## Objetivo
É necessária uma aplicação .net core que execute as seguintes etapas: <br>
* 1. Leia os eventos de notificação do AWS S3 PutObject a partir de uma fila SQS. 
* 2. Para cada notificação recebida, é necessário atualizar as colunas filename e filesize no banco de dados mysql. 
* 3. Caso a entrada não exista, é necessário criá-la. Caso o campo last_modified seja mais novo que a notificação, emitir um log e não deverá atualizar o registro.

A estrutura da tabela é:
tabela = files
colunas:
- filename: (primary key, varchar)
- filesize: long
- last modified: datetime

Entregas do projeto:
- código fonte no github, gitlab ou zip
- Dockerfile com o processo de build e execução da aplicação (opcional, ganha mais pontos se conseguir)
- qualquer outra documentação que venha a ser relevante.

Tecnologias:
- AWS SQS
- .NET Core
- Docker (opcional, ganha mais pontos se conseguir)

## Como rodar

### Download do repositório
Acesse o github https://github.com/pkdesouza/s3-to-sns-to-sqs-to-csharp, após o acesso, selecione a opção de realizar o clone do projeto.
```
https://github.com/pkdesouza/s3-to-sns-to-sqs-to-csharp.git
```

Com isso fazemos o download do código fonte que precisamos.

### Configurando os serviços da aws 

Acesse a pasta raiz do projeto (.) e abra o Powershell ou equivalente <br>

* Criar o bucket no s3
```
aws s3 mb s3://bemobi
```
Saída:
```
make_bucket: bemobi
```

* Criar o tópico no sns
```
aws sns create-topic --name bemobi-sns
```
Saída:
```
{
    "TopicArn": "arn:aws:sns:us-east-1:000000000000:bemobi-sns"
}
```

* Criar a fila no sqs:
```
aws sqs create-queue --queue-name bemobi-sqs
```
Saída:
```
{
    "QueueUrl": "http://localhost:4566/000000000000/bemobi-sqs"
}
```
* Obter o arn da queue criada
```
awslocal sqs get-queue-attributes --queue-url "http://localhost:4566/000000000000/bemobi-sqs/bemobi-sqs" --attribute-names All
```
Saída:
```
{
    "Attributes": {
        "ApproximateNumberOfMessages": "0",
        "ApproximateNumberOfMessagesNotVisible": "0",
        "ApproximateNumberOfMessagesDelayed": "0",
        "CreatedTimestamp": "1668207935",
        "DelaySeconds": "0",
        "LastModifiedTimestamp": "1668207935",
        "MaximumMessageSize": "262144",
        "MessageRetentionPeriod": "345600",
        "QueueArn": "arn:aws:sqs:us-east-1:000000000000:bemobi-sqs",
        "ReceiveMessageWaitTimeSeconds": "0",
        "VisibilityTimeout": "30",
        "SqsManagedSseEnabled": "false"
    }
}
```

* Configurações até o momento:

S3:
bucket name: s3://bemobi
ARN: arn:aws:s3:::bemobi
<br>
SNS:
topic name: bemobi-sns
ARN: arn:aws:sns:us-east-1:000000000000:bemobi-sns
<br>
SQS:
queue name: bemobi-sqs
queue URL: http://localhost:4566/000000000000/bemobi-sqs
ARN: arn:aws:sqs:us-east-1:000000000000:bemobi-sqs

* Abrir o arquivo sqs-permission.json e adicionar na key "Resource" o arn do sqs e na "aws:SourceArn" o arn do sns 
* Gerar o json com a key "Policy" para configurar a fila 
```
jq -c '. | { Policy: @text }' sqs-permission.json > sqs.json
```
Se não tiver o jq instalado, é só instalar nesse link https://stedolan.github.io/jq/

* Atribuir essa configuração na queue 
```
aws sqs set-queue-attributes 
--queue-url "http://localhost:4566/000000000000/bemobi-sqs" 
--attributes file://sqs.json
```
* Realizando a inscrição da queue no Tópico do SNS
```
aws sns subscribe 
--topic-arn "arn:aws:sns:us-east-1:000000000000:bemobi-sns" 
--protocol "sqs" 
--notification-endpoint "http://localhost:4566/000000000000/bemobi-sqs"
```
Saída:
```
{
    "SubscriptionArn": "arn:aws:sns:us-east-1:000000000000:bemobi-sns:8dee28f3-cab0-46d1-a7ef-391123236ce8"
}
```
* Abrir o arquivo sns-permission.json e adicionar na key "Resource" o arn do sns e na "aws:SourceArn" o bucket do s3

* Atribuir essa configuração no tópico do sns 

```
aws sns set-topic-attributes 
--topic-arn "arn:aws:sns:us-east-1:000000000000:bemobi-sns" 
--attribute-name Policy 
--attribute-value file://sns-permission.json
```
* Abrir o arquivo s3-notification.json e adicionar na key "TopicArn" o arn do sns
* Atribuir essa configuração notificações do bucket
```
aws s3api put-bucket-notification-configuration 
--bucket bemobi
--notification-configuration file://s3-notification.json
```