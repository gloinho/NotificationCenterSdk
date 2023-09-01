# Sobre

RaroNotifications é uma biblioteca em .NET responsável por realizar requisições para as API's do Notifications Center.

# Como Usar

## 1) Configuração do appsettings.json
```json
{
    "NotificationSender": {
        "CustomerBaseUrl": "https://customer-api.example.com",
        "EnginerBaseUrl": "https://enginer-api.example.com",
        "Username": "seu-username",
        "Password": "sua-senha"
    }
}
```
## 2) Injeção de Dependencias

```csharp
// Program.cs

using RaroNotifications.DependencyInjection;
builder.Services.AddNotificationSender(builder.Configuration);
```

## 2) Exemplo de utilização
```csharp
    public class ExampleController : ControllerBase
    {
        private readonly INotificationSender _notificationSender;
        public ExampleController(INotificationSender notificationSender)
        {
            _notificationSender = notificationSender;
        }

        [HttpPost]
        public async Task<ActionResult<NotificationResponse>> SendNotification(RequestSendNotification model)
        {
            var response = await _notificationSender.SendNotification(model);
            return response;
        }
    }
```

# Tipos Principais
Os principais tipos fornecidos por essa biblioteca são:
- `NotificationSender` : Classe responsável por realizar as requisições necessárias para autenticação, autorização e envio de notificações.
- `RequestSendNotification` : Modelo de requisição enviados para o Enginer API para envio de notificação.
- `RequestReceiverSendNotification`: Modelo de receivers enviados para o Enginer API para envio de notificação.
- `HbsTemplateParams`: Modelo de parâmetros opcionais enviados para o Enginer API para envio de notificação.
- `NotificationResponse`: Modelo de response ao realizar o envio de uma notificação.