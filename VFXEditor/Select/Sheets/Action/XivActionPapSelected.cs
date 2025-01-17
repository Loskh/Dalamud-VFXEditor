using System.Collections.Generic;

namespace VfxEditor.Select.Rows {
    public class XivActionPapSelected {
        public readonly XivActionPap ActionPap;

        public readonly Dictionary<string, string> StartAnimations;
        public readonly Dictionary<string, string> EndAnimations;
        public readonly Dictionary<string, string> HitAnimations;

        public XivActionPapSelected( XivActionPap actionPap, Dictionary<string, string> startAnimations, Dictionary<string, string> endAnimations, Dictionary<string, string> hitAnimations ) {
            ActionPap = actionPap;
            StartAnimations = startAnimations;
            EndAnimations = endAnimations;
            HitAnimations = hitAnimations;
        }
    }
}
