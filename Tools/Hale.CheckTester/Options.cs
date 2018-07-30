namespace Hale.CheckTester
{
    using System.Collections.Generic;
    using CommandLine;

    internal class Options
    {
        [Option('p', "module-path", Required = false,  HelpText = "Path to module directory")]
        public string ModulePath { get; set; }

        [Option('d', "dll", Required = true, HelpText = "DLL file name")]
        public string Dll { get; set; }

        [OptionList('c', "check-target", Separator = ',')]
        public List<string> CheckTargets { get; set; } = new List<string>();

        [OptionList('i', "info-target", Separator = ',')]
        public List<string> InfoTargets { get; set; } = new List<string>();

        [OptionList('a', "action-target", Separator = ',')]
        public List<string> ActionTargets { get; set; } = new List<string>();

        [OptionList('A', "alert-target", Separator = ',')]
        public List<string> AlertTargets { get; set; } = new List<string>();

        [ParserState]
        public IParserState LastParserState { get; set; }
    }
}
