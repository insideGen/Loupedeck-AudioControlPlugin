namespace WindowsInterop.ModernApp
{
    using System;

    /// <remarks><a href="https://msdn.microsoft.com/en-us/library/windows/desktop/hh446703.aspx"></a></remarks>
    public sealed class AppxApp
    {
        private readonly IAppxManifestApplication _app;

        internal AppxApp(IAppxManifestApplication app)
        {
            this._app = app;
        }

        public string GetStringValue(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            return AppxPackage.GetStringValue(this._app, name);
        }

        public string BackgroundColor { get; internal set; }
        public string Description { get; internal set; }
        public string DisplayName { get; internal set; }
        public string EntryPoint { get; internal set; }
        public string Executable { get; internal set; }
        public string ForegroundText { get; internal set; }
        public string Id { get; internal set; }
        public string Logo { get; internal set; }
        public string MinWidth { get; internal set; }
        public string ShortName { get; internal set; }
        public string SmallLogo { get; internal set; }
        public string Square30x30Logo { get; internal set; }
        public string Square44x44Logo { get; internal set; }
        public string Square70x70Logo { get; internal set; }
        public string Square71x71Logo { get; internal set; }
        public string Square150x150Logo { get; internal set; }
        public string Square310x310Logo { get; internal set; }
        public string StartPage { get; internal set; }
        public string Wide310x310Logo { get; internal set; }
        public string WideLogo { get; internal set; }
    }
}
