#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `AtomBaseVariable`. Inherits from `AtomDrawer&lt;AtomBaseVariableEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(HandyBaseVariableEvent))]
    public class AtomBaseVariableEventDrawer : AtomDrawer<HandyBaseVariableEvent> { }
}
#endif
