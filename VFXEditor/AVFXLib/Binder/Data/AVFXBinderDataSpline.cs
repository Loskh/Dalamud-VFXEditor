using System.Collections.Generic;
using VFXEditor.AVFXLib.Curve;

namespace VFXEditor.AVFXLib.Binder {
    public class AVFXBinderDataSpline : AVFXGenericData {
        public readonly AVFXCurve CarryOverFactor = new( "COF" );
        public readonly AVFXCurve CarryOverFactorRandom = new( "COFR" );

        public AVFXBinderDataSpline() : base() {
            Children = new List<AVFXBase> {
                CarryOverFactor,
                CarryOverFactorRandom
            };
        }
    }
}