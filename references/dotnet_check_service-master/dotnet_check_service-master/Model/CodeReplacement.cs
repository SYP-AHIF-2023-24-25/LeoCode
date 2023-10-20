namespace DotNetTestService.Model
{
    public sealed class CodeReplacement
    {
        public int SectionNo { get; set; }
        public string RawCode { get; set; } = default!;
    }
}