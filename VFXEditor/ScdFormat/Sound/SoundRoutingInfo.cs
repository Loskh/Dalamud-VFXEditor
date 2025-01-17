using Dalamud.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Parsing;

namespace VfxEditor.ScdFormat {
    public enum InsertEffectName {
        NoEffect,
        LowPassFilter,
        HighPassFilter,
        BandPassFilter,
        BandEliminateFilter,
        LowShelvingFilter,
        HighShelvingFilter,
        PeakingFilter,
        Equalizer,
        Compressor,
        Reverb,
        GranularSynthesizer,
        Delay,
        SimpleMeter
    }

    public enum FilterType {
        Bypass,
        LowPass,
        HighPass,
        BandPass,
        BandEliminate,
        LowShelving,
        HighShelving,
        Peaking
    }

    public class SoundRoutingInfo {
        public uint DataSize = 0x10;
        public byte SendCount;
        private readonly ParsedReserve Reserve1 = new( 11 );
        // Send Info
        public readonly List<SoundSendInfo> SendInfo = new();
        public readonly SoundEffectParam EffectParam = new();

        public void Read( BinaryReader reader ) {
            DataSize = reader.ReadUInt32();
            SendCount = reader.ReadByte();
            Reserve1.Read( reader );

            for( var i = 0; i < SendCount; i++ ) {
                var newInfo = new SoundSendInfo();
                newInfo.Read( reader );
                SendInfo.Add( newInfo );
            }

            EffectParam.Read( reader );
        }

        public void Write( BinaryWriter writer ) {
            writer.Write( DataSize );
            writer.Write( SendCount );
            Reserve1.Write( writer );
            SendInfo.ForEach( x => x.Write( writer ) );
            EffectParam.Write( writer );
        }

        public void Draw( string parentId ) {
            
        }
    }

    public class SoundSendInfo {
        public readonly ParsedByte Target = new( "Target" );
        private readonly ParsedReserve Reserve1 = new( 3 );
        public readonly ParsedFloat Volume = new( "Volume" );
        private readonly ParsedReserve Reserve2 = new( 2 * 4 );

        public void Read( BinaryReader reader ) {
            Target.Read( reader );
            Reserve1.Read( reader );
            Volume.Read( reader );
            Reserve2.Read( reader );
        }

        public void Write( BinaryWriter writer ) {
            Target.Write( writer );
            Reserve1.Write( writer );
            Volume.Write( writer );
            Reserve2.Write( writer );
        }
    }

    public class SoundEffectParam {
        public readonly ParsedEnum<InsertEffectName> Type = new( "Type", size: 1 );
        private readonly ParsedReserve Reserve1 = new( 3 );
        // Equalizer Effect
        public readonly List<SoundFilterParam> Filters = new();
        public readonly ParsedInt NumFilters = new( "Filter Count" );
        private readonly ParsedReserve Reserve2 = new( 2 * 4 );

        public void Read( BinaryReader reader ) {
            Type.Read( reader );
            Reserve1.Read( reader );

            for( var i = 0; i < 8; i++ ) {
                var newFilter = new SoundFilterParam();
                newFilter.Read( reader );
                Filters.Add( newFilter );
            }

            NumFilters.Read( reader );
            Reserve2.Read( reader );
        }

        public void Write( BinaryWriter writer ) {
            Type.Write( writer );
            Reserve1.Write( writer );

            Filters.ForEach( x => x.Write( writer ) );
            
            NumFilters .Write( writer );
            Reserve2 .Write( writer );
        }
    }

    public class SoundFilterParam {
        public readonly ParsedFloat Frequency = new( "Frequency" );
        public readonly ParsedFloat Invq = new( "Invq" );
        public readonly ParsedFloat Gain = new( "Gain" );
        public readonly ParsedEnum<FilterType> Type = new( "Type" );

        public void Read( BinaryReader reader ) {
            Frequency.Read( reader );
            Invq.Read( reader );
            Gain.Read( reader );
            Type.Read( reader );
        }

        public void Write( BinaryWriter writer ) {
            Frequency.Write( writer );
            Invq.Write( writer );
            Gain.Write( writer );
            Type.Write( writer );
        }
    }
}
