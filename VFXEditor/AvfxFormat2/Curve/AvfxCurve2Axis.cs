using ImGuiNET;
using System;
using System.Collections.Generic;
using System.IO;
using static VfxEditor.AvfxFormat2.Enums;

namespace VfxEditor.AvfxFormat2 {
    public class AvfxCurve2Axis : AvfxAssignable {
        public readonly string Name;
        public readonly bool Locked;

        public readonly AvfxEnum<AxisConnect> AxisConnectType = new( "Axis Connect", "ACT" );
        public readonly AvfxEnum<RandomType> AxisConnectRandomType = new( "Axis Connect Random", "ACTR" );
        public readonly AvfxCurve X = new( "X", "X" );
        public readonly AvfxCurve Y = new( "Y", "Y" );
        public readonly AvfxCurve RX = new( "Random X", "XR" );
        public readonly AvfxCurve RY = new( "Random Y", "YR" );

        private readonly List<AvfxBase> Children;
        private readonly List<AvfxCurve> Curves;

        public AvfxCurve2Axis( string name, string avfxName, bool locked = false ) : base( avfxName ) {
            Name = name;
            Locked = locked;

            Children = new() {
                AxisConnectType,
                AxisConnectRandomType,
                X,
                Y,
                RX,
                RY
            };

            Curves = new() {
                X,
                Y,
                RX,
                RY
            };
        }

        public override void ReadContents( BinaryReader reader, int size ) => ReadNested( reader, Children, size );

        protected override void RecurseChildrenAssigned( bool assigned ) => RecurseAssigned( Children, assigned );

        protected override void WriteContents( BinaryWriter writer ) => WriteNested( writer, Children );

        public override void DrawUnassigned( string parentId ) => DrawAddButtonRecurse( this, Name, parentId );

        public override void DrawAssigned( string parentId ) {
            var id = parentId + "/" + Name;
            if( !Locked && DrawRemoveButton( this, Name, id ) ) return;

            AvfxCurve.DrawUnassignedCurves( Curves, id );

            AxisConnectType.Draw( id );
            AxisConnectRandomType.Draw( id );
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );

            AvfxCurve.DrawAssignedCurves( Curves, id );
        }

        public override string GetDefaultText() => Name;
    }
}
