using System;

namespace Bannerlord.NexusmodsUploader
{
    internal class CookieEntry
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}