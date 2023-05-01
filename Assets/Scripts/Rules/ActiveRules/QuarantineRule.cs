using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuarantineRule", menuName = "Rules/Active/QuarantineRule")]
public class QuarantineRule : Rule
{
    public string system;
    public int systemIndex;

    public override bool Verify(Package package) {
        if (package.Find(Document.Type.Address).GetData("system") != system) {
            if (package.Find(Document.Type.QuarantineMandate) != null) {
                errorText = "Unnecessary quarantine mandate";
                return false;
            }
            return true;
        }
        if (package.Find(Document.Type.QuarantineMandate) != null) {
            return true;
        }
        errorText = "Missing quarantine mandate";
        return false;
    }

    public override void Test(Package package) {
        package.Find(Document.Type.Address).SetData("system", system);
        package.Find(Document.Type.Address).SetData("location", destinationRegistry.systems[systemIndex].locations[Random.Range(0, destinationRegistry.systems[systemIndex].locations.Length)]);
        package.Find(Document.Type.Stamp).SetData("system", system);
    }
}
