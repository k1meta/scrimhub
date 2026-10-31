using ScrimHub.Domain;

namespace api.UnitTests;

public class EntityTests
{
    [Fact]
    public void Constructor_SetsUtcTimestamps()
    {
        var entity = new TestEntity();

        Assert.NotEqual(Guid.Empty, entity.Id);
        Assert.Equal(DateTimeKind.Utc, entity.CreatedAt.Kind);
        Assert.Equal(entity.CreatedAt, entity.UpdatedAt);
    }

    [Fact]
    public void Touch_UpdatesUpdatedAt_WhenTimestampIsUtc()
    {
        var entity = new TestEntity();
        var createdAt = entity.CreatedAt;
        var later = createdAt.AddMinutes(5);

        entity.Touch(later);

        Assert.Equal(createdAt, entity.CreatedAt);
        Assert.Equal(later, entity.UpdatedAt);
    }

    [Fact]
    public void Touch_RejectsNonUtcTimestamp()
    {
        var entity = new TestEntity();
        var local = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Local);

        var error = Assert.Throws<ArgumentException>(() => entity.Touch(local));

        Assert.Equal("utcNow", error.ParamName);
    }

    private sealed class TestEntity : Entity;
}