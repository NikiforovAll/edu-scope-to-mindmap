using System.ComponentModel.DataAnnotations;
namespace XMindCLI.CommandLine
{
    public class EduScopeEntry
    {
        public string Name { get; set; }

        [Range(0, 100)]
        public int Progress { get; set; }

        public string Status { get; set; }

        public string Priority { get; set; }

        public string Type { get; set; }

        public string Epic { get; set; }

        public string Link { get; set; }

        public override string ToString() =>
            // $"{Name} - {Status} - {Priority} - {Epic}";
            $"{Name}";

    }
}
