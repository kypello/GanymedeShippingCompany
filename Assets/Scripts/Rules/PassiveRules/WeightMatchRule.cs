using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeightMatchRule", menuName = "Rules/WeightMatchRule")]
public class WeightMatchRule : Rule
{
    public override bool Verify(Package package) {
        errorText = "Package weight doesn't match";
        return package.Find(Document.Type.Address).GetData("weight") == package.Find(Document.Type.Address).GetData("actual weight");
    }

    public override void Break(Package package) {
        package.Find(Document.Type.Address).SetData("actual weight", "" + (int.Parse(package.Find(Document.Type.Address).GetData("weight")) + Random.Range(5, 11)));
    }
}
