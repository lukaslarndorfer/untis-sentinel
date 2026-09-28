using AwesomeAssertions;

using UntisSentinel.ChangeDetection;
using UntisSentinel.Tests.Helpers;
using UntisSentinel.Timetable;

namespace UntisSentinel.Tests;

public class LessonChangeDetectorTests
{

    [Fact]
    public void DetectChanges_NothingChanged_ReturnsEmpty()
    {
        var previous = TestLessons.Create();
        var current = TestLessons.Create();

        IReadOnlyList<LessonChange> changes = LessonChangeDetector.DetectChanges([previous], [current]);

        changes.Should().BeEmpty();
    }

    [Fact]
    public void DetectChanges_RegularToCancelled_ReturnsCancelled()
    {
        var previous = TestLessons.Create();
        var current = TestLessons.Create(status: LessonStatus.Cancelled);

        IReadOnlyList<LessonChange> changes = LessonChangeDetector.DetectChanges([previous], [current]);

        var change = changes.Should().ContainSingle().Subject;
        change.Type.Should().Be(LessonChangeType.Cancelled);
        change.Previous.Should().BeSameAs(previous);
        change.Current.Should().BeSameAs(current);
    }

    [Fact]
    public void DetectChanges_CancelledToRegular_ReturnsCancellationWithdrawn()
    {
        var previous = TestLessons.Create(status: LessonStatus.Cancelled);
        var current = TestLessons.Create(status: LessonStatus.Regular);

        IReadOnlyList<LessonChange> changes = LessonChangeDetector.DetectChanges([previous], [current]);

        var change = changes.Should().ContainSingle().Subject;
        change.Type.Should().Be(LessonChangeType.CancellationWithdrawn);
        change.Previous.Should().BeSameAs(previous);
        change.Current.Should().BeSameAs(current);
    }

}
