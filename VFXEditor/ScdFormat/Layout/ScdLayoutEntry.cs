using Dalamud.Logging;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VfxEditor.Parsing;

namespace VfxEditor.ScdFormat {
    public enum SoundObjectType {
        Null,
        Ambient,
        Direction,
        Point,
        PointDir,
        Line,
        Polyline,
        Surface,
        BoardObstruction,
        BoxObstruction,
        PolylineObstruction,
        Polygon,
        BoxExtController,
        LineExtController,
        PolygonObstruction
    }

    [Flags]
    public enum SoundObjectFlags1 {
        UseFixedDirection = 0x01,
        UnboundedDistance = 0x02,
        FirstInactive = 0x04,
        BottomInfinity = 0x08,
        TopInfinity = 0x10,
        Flag3D = 0x20,
        PointExpansion = 0x40,
        IsLittleEndian = 0x80
    }

    [Flags]
    public enum SoundObjectFlags2 {
        IsMaxRangeInterior = 0x01,
        UseDistanceFilters = 0x02,
        UseDirFirstPos = 0x04,
        IsWooferOnly = 0x08,
        IsFixedVolume = 0x10,
        IsIgnoreObstruction = 0x20,
        IsFirstFixedDirection = 0x40,
        IsLocalFixedDirection = 0x80
    }

    public class ScdLayoutEntry : ScdEntry, IScdSimpleUiBase {
        public ushort Size = 0x80;
        public readonly ParsedEnum<SoundObjectType> Type = new( "Type", size: 1 );
        public readonly ParsedByte Version = new( "Version" );
        public readonly ParsedFlag<SoundObjectFlags1> Flag1 = new( "Flag1", size: 1 );
        public readonly ParsedByte GroupNumber = new( "Group Number" );
        public readonly ParsedShort LocalId = new( "Local Id" );
        public readonly ParsedInt BankId = new( "Bank Id" );
        public readonly ParsedFlag<SoundObjectFlags2> Flag2 = new( "Flag2", size: 1 );
        public readonly ParsedByte ReverbType = new( "Reverb Type" );
        public readonly ParsedShort AbGroupNumber = new( "AB Group Number" );
        public readonly ParsedFloat4 Volume = new( "Volume" );

        public ScdLayoutData Data = null;

        private readonly List<ParsedBase> Parsed;

        public ScdLayoutEntry() {
            Parsed = new() {
                Type,
                Version,
                Flag1,
                GroupNumber,
                LocalId,
                BankId,
                Flag2,
                ReverbType,
                AbGroupNumber,
                Volume
            };
            
            Type.ExtraCommandGenerator = () => {
                return new ScdLayoutEntryExtraCommand( this );
            };
        }

        public override void Read( BinaryReader reader ) {
            Size = reader.ReadUInt16();
            Parsed.ForEach( x => x.Read( reader ) );

            UpdateData();
            Data?.Read( reader );
        }

        public override void Write( BinaryWriter writer ) {
            writer.Write( Size );
            Parsed.ForEach( x => x.Write( writer ) );

            Data?.Write( writer );
        }

        public void UpdateData() {
            Data = Type.Value switch {
                SoundObjectType.Ambient => new LayoutAmbientData(),
                SoundObjectType.Direction => new LayoutDirectionData(),
                SoundObjectType.Point => new LayoutPointData(),
                SoundObjectType.PointDir => new LayoutPointDirData(),
                SoundObjectType.Line => new LayoutLineData(),
                SoundObjectType.Polyline => new LayoutPolylineData(),
                SoundObjectType.Surface => new LayoutSurfaceData(),
                SoundObjectType.BoardObstruction => new LayoutBoardObstructionData(),
                SoundObjectType.BoxObstruction => new LayoutBoxObstructionData(),
                SoundObjectType.PolylineObstruction => new LayoutPolylineObstructionData(),
                SoundObjectType.Polygon => new LayoutPolygonData(),
                SoundObjectType.LineExtController => new LayoutLineExtControllerData(),
                SoundObjectType.PolygonObstruction => new LayoutPolygonObstructionData(),
                _ => null
            };
        }

        public void Draw( string id ) {
            Parsed.ForEach( x => x.Draw( id, CommandManager.Scd ) );

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 3 );
            ImGui.Separator();
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 3 );

            Data?.Draw( $"{id}/Data" );
        }
    }
}
