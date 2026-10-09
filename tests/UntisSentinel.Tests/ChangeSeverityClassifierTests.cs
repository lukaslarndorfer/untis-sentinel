using AwesomeAssertions;

using Microsoft.Extensions.Time.Testing;

using UntisSentinel.ChangeDetection;
using UntisSentinel.Tests.Helpers;
using UntisSentinel.Timetable;

namespace UntisSentinel.Tests;

public class ChangeSeverityClassifierTests
{

    [Theory]
    // "now" values based on testlesson default values (2026-09-28; start 08:50, end 09:40)
    [InlineData("2026-09-28 09:00", NotificationSeverity.Ping)]
    [InlineData("2026-09-27 11:00", NotificationSeverity.Ping)]
    [InlineData("2026-09-27 07:00", NotificationSeverity.Info)]
    [InlineData("2026-09-28 10:00", NotificationSeverity.Info)]
    [InlineData("2026-09-28 09:40", NotificationSeverity.Info)]
    [InlineData("2026-09-27 08:50", NotificationSeverity.Info)]
    public void Classify_ReturnsExpectedSeverity(string rawNow, NotificationSeverity expectedSeverity)
    {
        DateTime now = DateTime.Parse(rawNow);

        var fakeTime = new FakeTimeProvider(new DateTimeOffset(now, TimeSpan.Zero));
        fakeTime.SetLocalTimeZone(TimeZoneInfo.Utc);

        Lesson lesson = TestLessons.Create();
        var change = new LessonChange(LessonChangeType.Cancelled, lesson, lesson);

        NotificationSeverity severity = ChangeSeverityClassifier.Classify(change, fakeTime);
        severity.Should().Be(expectedSeverity);

    }
}
