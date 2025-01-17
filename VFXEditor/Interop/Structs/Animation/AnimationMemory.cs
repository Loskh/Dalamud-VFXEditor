using System.Runtime.InteropServices;

namespace VfxEditor.Structs.Animation {
    public enum CharacterModes : byte {
        None = 0,
        Normal = 1,
        EmoteLoop = 3,
        Mounted = 4,
        AnimLock = 8,
        Carrying = 9,
        InPositionLoop = 11,
        Performance = 16,
    }

    public enum AnimationSlots : int {
        FullBody = 0,
        UpperBody = 1,
        Facial = 2,
        Add = 3,
        Lips = 7,
        Parts1 = 8,
        Parts2 = 9,
        Parts3 = 10,
        Parts4 = 11,
        Overlay = 12,
    }

    // https://github.com/imchillin/Anamnesis/blob/340ae29d9cc6825c270842c7404d9fd8ea3cf208/Anamnesis/Memory/AnimationMemory.cs

    [StructLayout( LayoutKind.Explicit )]
    public unsafe struct ActorMemoryStruct {
        [FieldOffset( 0x1B00 )] public byte CharacterMode;
        [FieldOffset( 0x1B01 )] public byte CharacterModeInput;
        [FieldOffset( 0x0900 )] public AnimationMemory Animation;

        public bool CanAnimate => ( CharacterModes)CharacterMode == CharacterModes.Normal || ( CharacterModes)CharacterMode == CharacterModes.AnimLock;
        public bool IsAnimationOverride => ( CharacterModes )CharacterMode == CharacterModes.AnimLock;
    }

    [StructLayout( LayoutKind.Explicit )]
    public unsafe struct AnimationMemory {
        [FieldOffset( 0x0E0 )] public fixed ushort AnimationIds[13];
        [FieldOffset( 0x154 )] public fixed float Speeds[13];
        [FieldOffset( 0x1E2 )] public byte SpeedTrigger;
        [FieldOffset( 0x2CC )] public ushort BaseOverride;
        [FieldOffset( 0x2CE )] public ushort LipsOverride;
    }
}
