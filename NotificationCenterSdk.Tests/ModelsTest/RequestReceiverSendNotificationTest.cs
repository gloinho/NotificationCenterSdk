using NotificationCenterSdk.Exceptions;
using NotificationCenterSdk.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationCenterSdk.Tests.ModelsTest
{
    public class RequestReceiverSendNotificationTest
    {
        [Fact]
        public void CriarUmRequestReceiverSendNotificationComTodasAsPropriedadesNulasLancaExcecao()
        {
            Assert.Throws<NotificationException>(() => new RequestReceiverSendNotification(
                "", "",  null, null));
        }

        [Fact]
        public void CriarUmRequestReceiverSendNotificationDeveConterAoMenosUmParametroNaoNulo()
        {
            var result = new RequestReceiverSendNotification(
                "123",null,null,null);
            Assert.NotNull(result);
        }
    }
}
