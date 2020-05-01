using System.Collections.Generic;
using XMindCLI.CommandLine;
using XMindAPI.Core;
namespace XMindCLI.Services
{
    public interface IMindMapBuilder
    {
        void BuildMindMapContent(IWorkbook book, IEnumerable<EduScopeEntry> entries);
    }
}
