using System;
using System.Collections.Generic;
using System.Linq;
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
            Console.WriteLine(scanContext.ToString("x"));
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
                //Console.WriteLine(matches.Count());
                foreach (var match in matches)
                {
                    //Console.WriteLine(match.is_private);
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
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                YR_SCAN_CONTEXT_WIN scan_context = (YR_SCAN_CONTEXT_WIN)Marshal.PtrToStructure(scanContext, typeof(YR_SCAN_CONTEXT_WIN));
                return scan_context.profiling_info;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                YR_SCAN_CONTEXT_OSX scan_context = (YR_SCAN_CONTEXT_OSX)Marshal.PtrToStructure(scanContext, typeof(YR_SCAN_CONTEXT_OSX));
                return scan_context.profiling_info;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
            {
                YR_SCAN_CONTEXT_LINUX scan_context = (YR_SCAN_CONTEXT_LINUX)Marshal.PtrToStructure(scanContext, typeof(YR_SCAN_CONTEXT_LINUX));
                return scan_context.profiling_info;
            }


            return IntPtr.Zero;
        }

        private IntPtr GetMatchesPtr(IntPtr scanContext)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                YR_SCAN_CONTEXT_WIN scan_context = (YR_SCAN_CONTEXT_WIN)Marshal.PtrToStructure(scanContext, typeof(YR_SCAN_CONTEXT_WIN));
                return scan_context.matches;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                YR_SCAN_CONTEXT_OSX scan_context = (YR_SCAN_CONTEXT_OSX)Marshal.PtrToStructure(scanContext, typeof(YR_SCAN_CONTEXT_OSX));
                return scan_context.matches;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)|| RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
            {
                YR_SCAN_CONTEXT_LINUX scan_context = (YR_SCAN_CONTEXT_LINUX)Marshal.PtrToStructure(scanContext, typeof(YR_SCAN_CONTEXT_LINUX));
                Console.WriteLine(scan_context.matches.ToString("x"));
                return scan_context.matches;
            }

            return IntPtr.Zero;
        }
    }
}
