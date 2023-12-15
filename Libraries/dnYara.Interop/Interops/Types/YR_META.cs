using System;
using System.Runtime.InteropServices;


namespace dnYara.Interop
{

    /// <summary>
    /// Data structure representing a metadata value. Based on `type`, zero or one of `integer` or `strings` will be filled,
    /// and if `type` is `META_TYPE_BOOLEAN` then `integer` should be parsed as a boolean
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct YR_META
    {
        /// <summary>
        /// Meta identifier.
        /// </summary>
        [FieldOffset(0)]
        public IntPtr identifier;

        [FieldOffset(4)]
        public int identifier_;

        [FieldOffset(8)]
        public IntPtr strings;

        [FieldOffset(12)]
        public int strings_;

        [FieldOffset(16)]
        public long integer;

        /// <summary>
        /// One of the following metadata types:
        /// <code>META_TYPE_NULL META_TYPE_INTEGER META_TYPE_STRING META_TYPE_BOOLEAN</code>
        /// </summary>
        [FieldOffset(24)]
        public int type;

        [FieldOffset(28)]
        public int flags;
    }

}
