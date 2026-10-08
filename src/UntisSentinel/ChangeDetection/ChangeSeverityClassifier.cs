namespace UntisSentinel.ChangeDetection;

public static class ChangeSeverityClassifier
{
    public static NotificationSeverity ClassifyChangeSeverity(LessonChangeType changeType)
    {
        return changeType switch
        {
            LessonChangeType.Cancelled => NotificationSeverity.Ping,
            LessonChangeType.CancellationWithdrawn => NotificationSeverity.Ping,
            LessonChangeType.TeacherSubstituted => NotificationSeverity.Ping,
            LessonChangeType.TeacherSubstitutionWithdrawn => NotificationSeverity.Info,
            LessonChangeType.TeacherSubstitutionChanged => NotificationSeverity.Ping,
            LessonChangeType.RoomChanged => NotificationSeverity.Ping,
            LessonChangeType.LessonAdded => NotificationSeverity.Ping,
            LessonChangeType.BecameIrregular => NotificationSeverity.Ping,
            _ => NotificationSeverity.Info
        };
    }

}
