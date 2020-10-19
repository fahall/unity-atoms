using System;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Interface for all Atom Collections.
    /// </summary>
    public interface IWithCollectionEvents
    {
        HandyBaseVariableEvent Added { get; set; }
        HandyBaseVariableEvent Removed { get; set; }
        VoidEvent Cleared { get; set; }
    }
}