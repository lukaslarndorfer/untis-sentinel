using UntisSentinel.Timetable;

namespace UntisSentinel.Tests.Helpers;

internal static class TestLessons
{
    internal static Lesson Create(
        int id = 1,
        LessonStatus status = LessonStatus.Regular,
        IReadOnlyList<int>? teachers = null,
        IReadOnlyList<int>? rooms = null,
        IReadOnlyList<int>? substitutedTeacherIds = null)
        => new(
            Id: id,
            Date: new DateOnly(2026, 9, 28),
            Start: new TimeOnly(8, 50),
            End: new TimeOnly(9, 40),
            Status: status,
            SubstitutedTeacherIds: substitutedTeacherIds ?? [],
            Classes: [],
            Teachers: teachers ?? [],
            Subjects: [],
            Rooms: rooms ?? []);

}
