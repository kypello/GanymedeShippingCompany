using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VisaSystemRule", menuName = "Rules/Active/VisaSystemRule")]
public class VisaSystemRule : Rule
{
    public override bool Verify(Package package) {
        errorText = "Visa destination system doesn't match";

        if (package.Find(Document.Type.Visa) == null) {
            return true;
        }

        return package.Find(Document.Type.Visa).GetData("system") == package.Find(Document.Type.Address).GetData("system");
    }
}
