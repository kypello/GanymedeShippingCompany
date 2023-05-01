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
}
