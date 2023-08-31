# Sobre

RaroNotifications é uma biblioteca em .NET responsável por realizar requisições para as API's do Notifications Center.

# Como Usar

## 1) Realize a injeção de dependência do `IMemoryCache`

```csharp
// Program.cs
builder.Services.AddMemoryCache();
```

## 2) Exemplo de utilização
```csharp
private readonly IMemoryCache _memoryCache;
public WeatherForecastController(IMemoryCache memoryCache)
{
    _memoryCache = memoryCache;
}

[HttpPost]
public async Task<ActionResult<NotificationResponse>> SendNotification(RequestSendNotificationModel model)
{
	var sender = new NotificationSender(_memoryCache,
	"http://baseUrl",
	"yourEmail@example.com",
	"example@1234");
	var response = sender.SendNotification(model);
	return Ok(response);
}
```

# Tipos Principais
Os principais tipos fornecidos por essa biblioteca são:
- `NotificationSender` : Classe responsável por realizar as requisições necessárias para autenticação, autorização e envio de notificações.
- `RequestSendNotification` : Modelo de requisição enviados para o Enginer API para envio de notificação.
- `RequestReceiverSendNotification`: Modelo de receivers enviados para o Enginer API para envio de notificação.
- `HbsTemplateParams`: Modelo de parâmetros opcionais enviados para o Enginer API para envio de notificação.