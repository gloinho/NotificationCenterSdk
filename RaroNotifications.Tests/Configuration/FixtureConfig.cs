using AutoFixture;

namespace RaroNotifications.Tests.Configuration
{
    public static class FixtureConfig
    {
        public static Fixture Get()
        {
            var fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            return fixture;
        }
    }
}
