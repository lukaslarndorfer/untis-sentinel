namespace UntisSentinel.ChangeDetection;

public static class ChangeSeverityClassifier
{
    public static NotificationSeverity Classify(LessonChange change, DateTime now)
    {
        DateTime start = change.Current.Date.ToDateTime(change.Current.Start);

        if (start > now && start < now.AddHours(24) && IsPingType(change.Type))
        {
            return NotificationSeverity.Ping;
        }
        return NotificationSeverity.Info;
    }

    private static bool IsPingType(LessonChangeType changeType)
    {
        return changeType switch
        {
            LessonChangeType.Cancelled => true,
            LessonChangeType.CancellationWithdrawn => true,
            LessonChangeType.TeacherSubstituted => true,
            LessonChangeType.TeacherSubstitutionWithdrawn => false,
            LessonChangeType.TeacherSubstitutionChanged => true,
            LessonChangeType.RoomChanged => true,
            LessonChangeType.LessonAdded => true,
            LessonChangeType.BecameIrregular => true,
            _ => false
        };
    }

}
