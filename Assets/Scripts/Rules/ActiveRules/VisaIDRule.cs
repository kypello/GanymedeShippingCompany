using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VisaIDRule", menuName = "Rules/Active/VisaIDRule")]
public class VisaIDRule : Rule
{
    public override bool Verify(Package package) {
        errorText = "Visa ID doesn't match";

        if (package.Find(Document.Type.Visa) == null) {
            return true;
        }
        
        return package.Find(Document.Type.Visa).GetData("id") == package.Find(Document.Type.Address).GetData("id");
    }
}
