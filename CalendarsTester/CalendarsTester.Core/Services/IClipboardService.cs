namespace CalendarsTester.Core.Services
{
    /// Reference implementation
    /// Based on http://projectmarvin.tumblr.com/post/121101741333/implementing-copy-to-clipboard-across-mobile
    ///
    public interface IClipboardService
    {
        void CopyToClipboard(string text);
    }
}
