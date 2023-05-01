using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Rule : ScriptableObject
{
    public bool appearsInRulebook = false;
    public string rulebookEntry;
    public string errorText;
    public DestinationRegistry destinationRegistry;

    public abstract bool Verify(Package package);

    public virtual void Break(Package package) {}

    public virtual void Enforce(Package package) {}

    public virtual void Test(Package package) {}
}
