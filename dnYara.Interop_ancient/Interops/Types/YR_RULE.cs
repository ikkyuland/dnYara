using System;
using System.Runtime.InteropServices;

namespace dnYara.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct YR_RULE
    {
        // Global flags
        public int flags;

        // Number of atoms generated for this rule.
        public int num_atoms;

        public long identifier;

        public long tags;

        public long metas;

        public long strings;

        public long ns;
    }

}
