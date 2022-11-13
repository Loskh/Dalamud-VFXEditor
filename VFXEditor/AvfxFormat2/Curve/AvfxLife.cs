using System;
using System.Collections.Generic;
using System.IO;
using static VfxEditor.AvfxFormat2.Enums;

namespace VfxEditor.AvfxFormat2 {
    public class AvfxLife : AvfxAssignable {
        public bool Enabled = true;
        // Life is kinda strange, can either be -1 (4 bytes = ffffffff) or Val + ValR + RanT

        public readonly AvfxFloat Value = new( "Value", "Val" );
        public readonly AvfxFloat ValRandom = new( "Random Value", "ValR" );
        public readonly AvfxEnum<RandomType> ValRandomType = new( "Random Type", "Type" );

        private readonly List<AvfxBase> Children;
        private readonly List<IUiBase> Display;

        public AvfxLife() : base( "Life" ) {
            Value.SetValue( -1 );
            Children = new() {
                Value,
                ValRandom,
                ValRandomType
            };

            Display = new() {
                Value,
                ValRandom,
                ValRandomType
            };
        }

        public override void ReadContents( BinaryReader reader, int size ) {
            if( size > 4 ) {
                Enabled = true;
                ReadNested( reader, Children, size );
                return;
            }
            Enabled = false;
        }

        protected override void RecurseChildrenAssigned( bool assigned ) => RecurseAssigned( Children, assigned );

        protected override void WriteContents( BinaryWriter writer ) {
            if( !Enabled ) {
                writer.Write( -1 );
                return;
            }
            WriteNested( writer, Children );
        }

        public override void DrawAssigned( string parentId ) {
            var id = parentId + "/Life";
            IUiBase.DrawList( Display, id );
        }

        public override void DrawUnassigned( string id ) => DrawAddButtonRecurse( this, "Life", id );

        public override string GetDefaultText() => "Life";
    }
}
