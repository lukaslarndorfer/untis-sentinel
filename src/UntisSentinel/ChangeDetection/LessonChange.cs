using UntisSentinel.Timetable;

namespace UntisSentinel.ChangeDetection;

public sealed record LessonChange(LessonChangeType Type, Lesson? Previous, Lesson Current);
