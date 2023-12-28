using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using dnYara.Interop;

namespace dnYara
{

    public class ScanResult
    {
        public Rule MatchingRule;
        public Dictionary<string, List<Match>> Matches;
        public ProfilingInfo ProfilingInfo;

        public ScanResult()
        {
            MatchingRule = null;
            Matches = new Dictionary<string, List<Match>>();
            ProfilingInfo = null;
        }

        public ScanResult(IntPtr scanContext, YR_RULE matchingRule)
        {
            IntPtr matchesPtr = GetMatchesPtr(scanContext);
            IntPtr profilingInfoPtr = GetProfilingInfoPtr(scanContext);

            MatchingRule = new Rule(matchingRule);
            Matches = new Dictionary<string, List<Match>>();

            var matchingStrings = ObjRefHelper.GetYaraStrings((IntPtr)matchingRule.strings);
            foreach (var str in matchingStrings)
            {
                var identifier = (IntPtr)str.identifier;

                if (identifier == IntPtr.Zero)
                    return;

                var matches = ObjRefHelper.GetStringMatches(matchesPtr, str);

                foreach (var match in matches)
                {
                    string matchText = ObjRefHelper.ReadYaraString(str);

                    if (!Matches.ContainsKey(matchText))
                        Matches.Add(matchText, new List<Match>());

                    Matches[matchText].Add(new Match(match));
                    if (ProfilingInfo == null)
                    {
                        var profInfo = ObjRefHelper.TryGetProfilingInfoForRule(profilingInfoPtr, (int)str.rule_idx);
                        if (profInfo.HasValue)
                        {
                            ProfilingInfo = new ProfilingInfo(profInfo.Value);
                        }
                    }
                }
            }
        }

        private IntPtr GetProfilingInfoPtr(IntPtr scanContext)
        {
            if (Environment.OSVersion.Platform== PlatformID.Win32NT)
            {
                YR_SCAN_CONTEXT_WIN scan_context = (YR_SCAN_CONTEXT_WIN)Marshal.PtrToStructure(scanContext, typeof(YR_SCAN_CONTEXT_WIN));
                return scan_context.profiling_info;
            }

            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                YR_SCAN_CONTEXT_LINUX scan_context = (YR_SCAN_CONTEXT_LINUX)Marshal.PtrToStructure(scanContext, typeof(YR_SCAN_CONTEXT_LINUX));
                return scan_context.profiling_info;
            }

            if (Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                YR_SCAN_CONTEXT_OSX scan_context = (YR_SCAN_CONTEXT_OSX)Marshal.PtrToStructure(scanContext, typeof(YR_SCAN_CONTEXT_OSX));
                return scan_context.profiling_info;
            }
            return IntPtr.Zero;
        }

        private IntPtr GetMatchesPtr(IntPtr scanContext)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                YR_SCAN_CONTEXT_WIN scan_context = (YR_SCAN_CONTEXT_WIN)Marshal.PtrToStructure(scanContext, typeof(YR_SCAN_CONTEXT_WIN));
                return scan_context.matches;
            }

            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                YR_SCAN_CONTEXT_LINUX scan_context = (YR_SCAN_CONTEXT_LINUX)Marshal.PtrToStructure(scanContext, typeof(YR_SCAN_CONTEXT_LINUX));
                return scan_context.matches;
            }

            if (Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                YR_SCAN_CONTEXT_OSX scan_context = (YR_SCAN_CONTEXT_OSX)Marshal.PtrToStructure(scanContext, typeof(YR_SCAN_CONTEXT_OSX));
                return scan_context.matches;
            }
            return IntPtr.Zero;
        }
    }
}
