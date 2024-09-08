# ConsoleChatApp

Este é um aplicativo de chat de console desenvolvido em C# e .NET 8, utilizando WebSockets para comunicação em tempo real entre clientes e servidor.

![image](https://github.com/user-attachments/assets/2c6461f0-9353-4c47-bbcc-76c6d033871d)



## Conceitos Aplicados

- **WebSockets**: Protocolo que permite comunicação bidirecional e full-duplex entre cliente e servidor, mantendo uma conexão continua. Utilizado aqui para enviar e receber mensagens de forma eficiente e em tempo real.

- **Broadcast**: Técnica usada para enviar mensagens recebidas de um cliente para todos os clientes conectados. Isso permite que todos os usuários vejam as mensagens enviadas por qualquer pessoa no chat, criando uma experiência de chat em grupo em tempo real.

## Estrutura do Projeto

- **Cliente (ConsoleChatApp.WS_Client)**:
  - Permite ao usuário se conectar ao servidor WebSocket, enviar e receber mensagens em tempo real.
  - Utiliza `ClientWebSocket` para estabelecer e manter a conexão.
  - Mensagens são enviadas e recebidas em tarefas assíncronas.

- **Servidor (ConsoleChatApp.WS_Server)**:
  - Aceita conexões WebSocket e gerencia múltiplos clientes simultaneamente.
  - **Broadcast**: Transmite mensagens recebidas de um cliente para todos os clientes conectados. Isso é feito utilizando a função `BroadcastAsync`, que envia a mensagem a todos os WebSockets ativos.
  - Utiliza `WebSocket` e `HttpContext` para gerenciar a comunicação e as conexões.

## Como Executar

1. **Execute o Servidor**:
   - Navegue até o diretório do servidor e execute o comando:
     ```bash
     make server
     ```

2. **Execute o Cliente**:
   - Navegue até o diretório do cliente e execute o comando:
     ```bash
     make client
     ```

3. **Interaja**:
   - No cliente, insira seu nome de usuário e comece a enviar mensagens.
   - Veja as mensagens em tempo real no console.

## Dependências

- .NET 8

## Links Relacionados
https://en.wikipedia.org/wiki/WebSocket </br>
https://learn.microsoft.com/pt-br/aspnet/core/fundamentals/websockets?view=aspnetcore-8.0 </br>
https://balta.io/blog/aspnet-websockets </br>
