namespace DotNetTestService.Core
{
    public sealed class AppSettings
    {
        public const string KEY = nameof(AppSettings);

        public string RootWorkDir { get; set; } = default!;
        public string RootTemplateDir { get; set; } = default!;
        public string PackageCacheDir { get; set; } = default!;
        public int TimeoutMinutes { get; set; }
    }
}