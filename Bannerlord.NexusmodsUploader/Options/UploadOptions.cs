using CommandLine;

namespace Bannerlord.NexusmodsUploader.Options
{
    [Verb("upload", HelpText = "Upload mod to nexusmods.")]
    internal class UploadOptions
    {
        [Option('g', "game_id", Required = true)]
        public string GameId { get; set; }

        [Option('m', "mod_id", Required = true)]
        public int ModId { get; set; }

        [Option('n', "name", Required = true)]
        public string Name { get; set; }

        [Option('v', "version", Required = true)]
        public string Version { get; set; }

        [Option('l', "is_latest", Required = true)]
        public bool IsLatest { get; set; }

        [Option('e', "is_new_of_existing", Required = true)]
        public bool IsNewOfExisting { get; set; }

        [Option('d', "description", Required = true)]
        public string Description { get; set; }

        [Option('p', "file_path", Required = true)]
        public string FilePath { get; set; }
    }
}