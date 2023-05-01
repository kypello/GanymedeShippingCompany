using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StampMatchRule", menuName = "Rules/StampMatchRule")]
public class StampMatchRule : Rule
{
    public override bool Verify(Package package) {
        errorText = "Stamp doesn't match destination system";
        return package.Find(Document.Type.Stamp).GetData("system") == package.Find(Document.Type.Address).GetData("system");
    }

    public override void Break(Package package) {
        do {
            package.Find(Document.Type.Stamp).SetData("system", destinationRegistry.systems[Random.Range(0, destinationRegistry.systems.Length)].name);
        } while (package.Find(Document.Type.Stamp).GetData("system") == package.Find(Document.Type.Address).GetData("system"));
    }
}
