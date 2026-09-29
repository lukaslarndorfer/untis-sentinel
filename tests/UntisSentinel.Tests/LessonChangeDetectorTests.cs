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

    [Fact]
    public void DetectChanges_SubstitutionAppears_ReturnsTeacherSubstituted()
    {
        const int previousTeacherId = 2;
        const int newTeacherId = 3;
        var previous = TestLessons.Create(teachers: [previousTeacherId]);
        var current = TestLessons.Create(teachers: [newTeacherId], substitutedTeacherIds: [previousTeacherId]);

        IReadOnlyList<LessonChange> changes = LessonChangeDetector.DetectChanges([previous], [current]);

        var change = changes.Should().ContainSingle().Subject;
        change.Type.Should().Be(LessonChangeType.TeacherSubstituted);
        change.Previous.Should().BeSameAs(previous);
        change.Current.Should().BeSameAs(current);
    }

    [Fact]
    public void DetectChanges_SubstitutionDisappears_ReturnsTeacherSubstitutionWithdrawn()
    {
        const int previousTeacherId = 2;
        const int newTeacherId = 3;
        var previous = TestLessons.Create(teachers: [newTeacherId], substitutedTeacherIds: [previousTeacherId]);
        var current = TestLessons.Create(teachers: [previousTeacherId]);

        IReadOnlyList<LessonChange> changes = LessonChangeDetector.DetectChanges([previous], [current]);

        var change = changes.Should().ContainSingle().Subject;
        change.Type.Should().Be(LessonChangeType.TeacherSubstitutionWithdrawn);
        change.Previous.Should().BeSameAs(previous);
        change.Current.Should().BeSameAs(current);
    }

    [Fact]
    public void DetectChanges_RoomChanged_ReturnsRoomChanged()
    {
        const int previousRoomId = 2;
        const int newRoomId = 3;
        var previous = TestLessons.Create(rooms: [previousRoomId]);
        var current = TestLessons.Create(rooms: [newRoomId]);

        IReadOnlyList<LessonChange> changes = LessonChangeDetector.DetectChanges([previous], [current]);

        var change = changes.Should().ContainSingle().Subject;
        change.Type.Should().Be(LessonChangeType.RoomChanged);
        change.Previous.Should().BeSameAs(previous);
        change.Current.Should().BeSameAs(current);
    }

    [Fact]
    public void DetectChanges_RoomAndTeacherChanged_ReturnsTwoChanges()
    {
        const int previousRoomId = 2;
        const int newRoomId = 3;
        const int previousTeacherId = 4;
        const int newTeacherId = 5;

        var previous = TestLessons.Create(rooms: [previousRoomId], teachers: [previousTeacherId]);
        var current = TestLessons.Create(rooms: [newRoomId], teachers: [newTeacherId], substitutedTeacherIds: [previousTeacherId]);
        
        IReadOnlyList<LessonChange> changes = LessonChangeDetector.DetectChanges([previous], [current]);

        changes.Select(c => c.Type).Should().BeEquivalentTo(
        [LessonChangeType.RoomChanged, LessonChangeType.TeacherSubstituted]);
    }

        [Fact]
    public void DetectChanges_LessonOnlyInCurrent_ReturnsEmpty()
    {
        var current = TestLessons.Create();

        IReadOnlyList<LessonChange> changes = LessonChangeDetector.DetectChanges([], [current]);

        changes.Should().BeEmpty();
    } // decide: new lesson = no change?






}
