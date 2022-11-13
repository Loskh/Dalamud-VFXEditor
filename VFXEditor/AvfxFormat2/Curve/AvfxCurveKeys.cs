using System;
using System.Collections.Generic;
using System.IO;
using static VfxEditor.AvfxFormat2.Enums;

namespace VfxEditor.AvfxFormat2 {
    public class AvfxCurveKeys : AvfxBase {
        public readonly List<AVFXCurveKey> Keys = new();

        public AvfxCurveKeys() : base( "Keys" ) { }

        public override void ReadContents( BinaryReader reader, int size ) {
            var count = size / 16;
            for( var i = 0; i < count; i++ ) Keys.Add( new AVFXCurveKey( reader ) );
        }

        protected override void RecurseChildrenAssigned( bool assigned ) { }

        protected override void WriteContents( BinaryWriter writer ) {
            foreach( var key in Keys ) key.Write( writer );
        }
    }

    public class AVFXCurveKey {
        public KeyType Type;
        public int Time;

        public float X;
        public float Y;
        public float Z;

        public AVFXCurveKey( KeyType type, int time, float x, float y, float z ) {
            Type = type;
            Time = time;
            X = x;
            Y = y;
            Z = z;
        }

        public AVFXCurveKey( BinaryReader reader ) {
            Time = reader.ReadInt16();
            Type = ( KeyType )reader.ReadInt16();
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            Z = reader.ReadSingle();
        }

        public void Write( BinaryWriter writer ) {
            writer.Write( ( short )Time );
            writer.Write( ( short )Type );
            writer.Write( X );
            writer.Write( Y );
            writer.Write( Z );
        }
    }
}
