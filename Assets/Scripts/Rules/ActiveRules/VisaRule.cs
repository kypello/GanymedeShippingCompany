using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VisaRule", menuName = "Rules/Active/VisaRule")]
public class VisaRule : Rule
{
    public override bool Verify(Package package) {
        if (package.Find(Document.Type.Address).GetData("system") == "Jovian System") {
            if (package.Find(Document.Type.Visa) != null) {
                errorText = "Unnecessary interplanetary visa";
                return false;
            }
            return true;
        }
        if (package.Find(Document.Type.Visa) != null) {
            return true;
        }
        errorText = "Missing interplanetary visa";
        return false;
    }

    public override void Test(Package package) {
        int systemIndex = Random.Range(6, 10);
        package.Find(Document.Type.Address).SetData("system", destinationRegistry.systems[systemIndex].name);
        package.Find(Document.Type.Address).SetData("location", destinationRegistry.systems[systemIndex].locations[Random.Range(0, destinationRegistry.systems[systemIndex].locations.Length)]);
        package.Find(Document.Type.Stamp).SetData("system", destinationRegistry.systems[systemIndex].name);
    }
}
