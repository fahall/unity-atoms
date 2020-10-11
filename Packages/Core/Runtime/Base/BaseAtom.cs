using UnityEngine;
#if ODIN_INSPECTOR && UNITY_ATOMS_USE_ODIN
using Sirenix.OdinInspector;
#endif

namespace UnityAtoms
{
    /// <summary>
    /// None generic base class for all Atoms.
    /// </summary>
#if ODIN_INSPECTOR && UNITY_ATOMS_USE_ODIN
    public abstract class BaseAtom : SerializedScriptableObject
#else
    public abstract class BaseAtom : ScriptableObject
#endif
    {
        /// <summary>
        /// A description of the Atom made for documentation purposes.
        /// </summary>
        [SerializeField]
        [TextArea(3, 6)]
        private string _developerDescription;
    }
}
