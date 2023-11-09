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

        [FieldOffset(8)]
        public IntPtr identifier;

        [FieldOffset(12)]
        public int identifier_;

        [FieldOffset(16)]
        public IntPtr tags;

        [FieldOffset(20)]
        public int tags_;

        [FieldOffset(24)]
        public IntPtr metas;

        [FieldOffset(28)]
        public int metas_;

        [FieldOffset(32)]
        public IntPtr strings;

        [FieldOffset(36)]
        public int strings_;

        [FieldOffset(40)]
        public IntPtr ns;

        [FieldOffset(44)]
        public int ns_;
    }

}
