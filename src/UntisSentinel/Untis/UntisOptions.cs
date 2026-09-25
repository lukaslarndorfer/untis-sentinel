using System.ComponentModel.DataAnnotations;

namespace UntisSentinel.Untis;

public sealed class UntisOptions
{
    public const string SectionName = "Untis";

    [Required] public string Host { get; set; } = "";
    [Required] public string School { get; set; } = "";
    [Required] public string User { get; set; } = "";
    [Required] public string Password { get; set; } = "";
}
