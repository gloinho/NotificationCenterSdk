# Sobre

NotificationCenterSdk é uma biblioteca responsável por realizar requisições para as API's do Notifications Center.

# Como Usar

## 1) Configuração do appsettings.json
```json
{
    "NotificationCenter": {
        "AuthBaseUrl": "https://auth-api.example.com",
        "EnginerBaseUrl": "https://enginer-api.example.com",
        "Username": "seu-username",
        "Password": "sua-senha"
    }
}
```
## 2) Injeção de Dependencias

#### Injetando as dependencias manualmente.
```csharp
builder.Services.AddMemoryCache();
var settings = builder.Configuration.GetRequiredSection("NotificationCenter");

builder.Services.AddHttpClient("auth", options =>
{
    options.BaseAddress = new Uri(settings["AuthBaseUrl"]);
});

builder.Services.AddHttpClient("enginer", options =>
{
    options.BaseAddress = new Uri(settings["EnginerBaseUrl"]);
});

builder.Services.AddSingleton(provider =>
{
    return new UserCredentials
    {
        Username = settings["Username"],
        Password = settings["Password"]
    };
});

builder.Services.AddSingleton<INotificationCenter, NotificationCenter>();
```

---
#### Utilizando a extensão `NotificationCenterSdk.DependencyInjection`
```csharp
// Program.cs 
using NotificationCenterSdk.DependencyInjection;

builder.Services.AddNotificationCenter(builder.Configuration);
builder.Services.AddMemoryCache();
```

## 2) Exemplo de utilização
#### Realizando a autenticação automática e salvando o access token no Memory Cache.
```csharp
    public class ExampleController : ControllerBase
    {
        private readonly INotificationCenter _NotificationCenter;
        public ExampleController(INotificationCenter NotificationCenter)
        {
            _NotificationCenter = NotificationCenter;
        }

        [HttpPost]
        public async Task<ActionResult<NotificationResponse>> SendNotification(RequestSendNotification model)
        {
            var response = await _NotificationCenter.SendNotification(model);
            return response;
        }
    }
```
---
#### Realizando a autenticação manual e devolvendo o access token para gerenciamento manual.
```csharp
    public class ExampleController : ControllerBase
    {
        private readonly INotificationCenter _NotificationCenter;
        public ExampleController(INotificationCenter NotificationCenter)
        {
            _NotificationCenter = NotificationCenter;
        }

        [HttpPost]
        public async Task<ActionResult<NotificationResponse>> SendNotification(RequestSendNotification model)
        {
            var token = await _NotificationCenter.Authenticate();
            // Realizar tratativas para armazenamento de token.
            var response = await _NotificationCenter.SendNotification(model,token);
            return response;
        }
    }
```

# Principais Tipos
Os principais tipos fornecidos por essa biblioteca são:
- `NotificationCenter` : Classe responsável por realizar as requisições necessárias para autenticação, autorização e envio de notificações.
- `RequestSendNotification` : Modelo de requisição enviados para o Enginer API para envio de notificação.
- `RequestReceiverSendNotification`: Modelo de receivers enviados para o Enginer API para envio de notificação.
- `HbsTemplateParams`: Modelo de parâmetros opcionais enviados para o Enginer API para envio de notificação.
- `NotificationResponse`: Modelo de response ao realizar o envio de uma notificação.
