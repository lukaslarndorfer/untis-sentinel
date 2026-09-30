using System;

using UntisSentinel.Timetable;

namespace UntisSentinel.ChangeDetection;

public static class LessonChangeDetector
{
    public static IReadOnlyList<LessonChange> DetectChanges(IReadOnlyList<Lesson> previous, IReadOnlyList<Lesson> current)
    {
        if(previous.Count == 0)
        {
            return []; // no previous lessons, so we don't consider this a change (can be initial fetch)
        }

        List<LessonChange> changes = [];
        var previousLessonsById = previous.ToDictionary(l => l.Id);

        

        var earliestDate = previous.Min(l => l.Date);
        var latestDate = previous.Max(l => l.Date);
        foreach (Lesson lesson in current)
        {
            if (!previousLessonsById.TryGetValue(lesson.Id, out var prevLesson))
            {
                if(lesson.Date <= latestDate && lesson.Date >= earliestDate)
                {
                    changes.Add(new LessonChange(LessonChangeType.LessonAdded, null, lesson));
                }
            }
            else
            {
                if (prevLesson.Status != lesson.Status)
                {
                    if (lesson.Status == LessonStatus.Cancelled)
                    {
                        changes.Add(new LessonChange(LessonChangeType.Cancelled, prevLesson, lesson));
                    }
                    if (prevLesson.Status == LessonStatus.Cancelled && lesson.Status == LessonStatus.Regular)
                    {
                        changes.Add(new LessonChange(LessonChangeType.CancellationWithdrawn, prevLesson, lesson));
                    }
                    if (lesson.Status == LessonStatus.Irregular)
                    {
                        changes.Add(new LessonChange(LessonChangeType.BecameIrregular, prevLesson, lesson));
                    }
                }
                if (lesson.SubstitutedTeacherIds.Count > 0 && prevLesson.SubstitutedTeacherIds.Count == 0)
                {
                    changes.Add(new LessonChange(LessonChangeType.TeacherSubstituted, prevLesson, lesson));
                }
                if (lesson.SubstitutedTeacherIds.Count == 0 && prevLesson.SubstitutedTeacherIds.Count > 0)
                {
                    changes.Add(new LessonChange(LessonChangeType.TeacherSubstitutionWithdrawn, prevLesson, lesson));
                }
                if (lesson.Rooms.Order().SequenceEqual(prevLesson.Rooms.Order()) == false)
                {
                    changes.Add(new LessonChange(LessonChangeType.RoomChanged, prevLesson, lesson));
                }

            }
        }

        return changes;
    }
}
