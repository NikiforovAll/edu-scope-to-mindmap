using System;
using System.Linq;
using System.Collections.Generic;
using XMindAPI.Models;
using static XMindAPI.Core.DOM.DOMConstants;
using XMindCLI.CommandLine;
using XMindAPI.Core;

namespace XMindCLI.Services
{
    public sealed class MindMapBuilder : IMindMapBuilder
    {
        public void BuildMindMapContent(IWorkbook book, IEnumerable<EduScopeEntry> entries)
        {
            var rootTopic = book.GetPrimarySheet()
                .GetRootTopic();
            rootTopic.SetTitle("Scope");
            foreach (var groupping in entries.ToLookup(item => item.Epic))
            {
                var epicTopic = book.CreateTopic();
                epicTopic.SetTitle(groupping.Key);
                rootTopic.Add(epicTopic);
                foreach (var subjectToLearn in groupping)
                {
                    var subjectTopic = book.CreateTopic();
                    subjectTopic.SetTitle(subjectToLearn.Name);
                    subjectTopic.HyperLink = subjectToLearn.Link;
                    subjectTopic.AddMarker(MarkerPriorityFactoryMethod(
                        subjectToLearn.Priority?.ToLower()?.Trim() ?? "low"));
                    subjectTopic.AddMarker(MarkerProgressFactoryMethod(
                        subjectToLearn.Progress));
                    epicTopic.Add(subjectTopic);
                }
            }

            static string MarkerPriorityFactoryMethod(string priority) => priority switch
            {
                "high" => MAR_priority1,
                "medium" => MAR_priority2,
                "low" => MAR_priority3,
                _ => throw new InvalidOperationException($"Unknown priority: {priority}")
            };

            static string MarkerProgressFactoryMethod(int percent) => (percent / 10) switch
            {
                0 => MAR_taskstart,
                1 => MAR_taskoct,
                2 => MAR_taskquarter,
                3 => MAR_taskquarter,
                4 => MAR_task3oct,
                5 => MAR_taskhalf,
                6 => MAR_task5oct,
                7 => MAR_task5oct,
                8 => MAR_task3quar,
                9 => MAR_task7oct,
                10 => MAR_taskdone,
                _ => throw new InvalidOperationException("Percent is out of range")
            };
        }
    }
}
