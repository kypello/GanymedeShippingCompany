using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StampRule", menuName = "Rules/StampRule")]
public class StampRule : Rule
{
    public override bool Verify(Package package) {
        errorText = "Missing stamp";
        return package.Find(Document.Type.Stamp) != null;
    }

    public override void Break(Package package) {
        package.RemoveDocument(package.Find(Document.Type.Stamp), true);
    }
}
