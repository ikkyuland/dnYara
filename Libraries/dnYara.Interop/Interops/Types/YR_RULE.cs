using System;
using System.Runtime.InteropServices;

namespace dnYara.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    public struct YR_RULE
    {
        [FieldOffset(0)]
        // Global flags
        public int flags;

        [FieldOffset(4)]
        // Number of atoms generated for this rule.
        public int num_atoms;

        [FieldOffset(12)]
        public IntPtr identifier;

        [FieldOffset(20)]
        public IntPtr tags;

        [FieldOffset(28)]
        public IntPtr metas;

        [FieldOffset(36)]
        public IntPtr strings;

        [FieldOffset(44)]
        public IntPtr ns;
    }

}
