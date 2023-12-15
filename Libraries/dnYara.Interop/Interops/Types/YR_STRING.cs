using System;
using System.Runtime.InteropServices;


namespace dnYara.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    public struct YR_STRING
    {
        [FieldOffset(0)]
        /// Flags, see STRING_FLAGS_XXX macros defined above.
        public uint flags;

        [FieldOffset(4)]
        /// Index of this string in the array of YR_STRING structures stored in
        /// YR_STRINGS_TABLE.
        public uint idx;

        [FieldOffset(8)]
        /// If the string can only match at a specific offset (for example if the
        /// condition is "$a at 0" the string $a can only match at offset 0), the
        /// fixed_offset field contains the offset, it have the YR_UNDEFINED value for
        /// strings that can match anywhere.
        public long fixed_offset;

        [FieldOffset(16)]
        /// Index of the rule containing this string in the array of YR_RULE
        /// structures stored in YR_RULES_TABLE.
        public uint rule_idx;

        [FieldOffset(20)]
        /// String's length.
        public int length;

        [FieldOffset(24)]
        /// Pointer to the string itself, the length is indicated by the "length"
        /// field.
        public IntPtr string_content;

        [FieldOffset(28)]
        public int string_content_;
        /// Strings are splitted in two or more parts when they contain a "gap" that
        /// is larger than YR_STRING_CHAINING_THRESHOLD. This happens in strings like
        /// { 01 02 03 04 [X-Y] 05 06 07 08 } if Y >= X + YR_STRING_CHAINING_THRESHOLD
        /// and also in { 01 02 03 04 [-] 05 06 07 08 }. In both cases the strings are
        /// split in { 01 02 03 04 } and { 05 06 07 08 }, and the two smaller strings
        /// are searched for independently. If some string S is splitted in S1 and S2,
        /// S2 is chained to S1. In the example above { 05 06 07 08 } is chained to
        /// { 01 02 03 04 }. The same applies when the string is splitted in more than
        /// two parts, if S is split in S1, S2, and S3. S3 is chained to S2 and S2 is
        /// chained to S1 (it can represented as: S1 <- S2 <- S3).
        [FieldOffset(32)]
        public IntPtr chained_to;

        [FieldOffset(36)]
        public int chained_to_;

        /// When this string is chained to some other string, chain_gap_min and
        /// chain_gap_max contain the minimum and maximum distance between the two
        /// strings. For example in { 01 02 03 04 [X-Y] 05 06 07 08 }, the string
        /// { 05 06 07 08 } is chained to { 01 02 03 04 } and chain_gap_min is X
        /// and chain_gap_max is Y. These fields are ignored for strings that are not
        /// part of a string chain.
        [FieldOffset(40)]
        public int chain_gap_min;

        /// When this string is chained to some other string, chain_gap_min and
        /// chain_gap_max contain the minimum and maximum distance between the two
        /// strings. For example in { 01 02 03 04 [X-Y] 05 06 07 08 }, the string
        /// { 05 06 07 08 } is chained to { 01 02 03 04 } and chain_gap_min is X
        /// and chain_gap_max is Y. These fields are ignored for strings that are not
        /// part of a string chain.
        [FieldOffset(44)]
        public int chain_gap_max;

        /// Identifier of this string.
        [FieldOffset(48)]
        public IntPtr identifier;

        [FieldOffset(52)]
        public int identifier_;
    }

}
