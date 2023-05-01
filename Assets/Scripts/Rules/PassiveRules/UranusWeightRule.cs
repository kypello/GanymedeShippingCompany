using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UranusWeightRule", menuName = "Rules/UranusWeightRule")]
public class UranusWeightRule : Rule
{
    public override bool Verify(Package package) {
        errorText = "No packages above 20kg can be shipped to the Uranian system";

        if (package.Find(Document.Type.Address).GetData("system") != "Uranian System") {
            return true;
        }

        return int.Parse(package.Find(Document.Type.Address).GetData("actual weight")) < 20;
    }

    public override void Break(Package package) {
        package.Find(Document.Type.Address).SetData("system", destinationRegistry.systems[8].name);
        package.Find(Document.Type.Address).SetData("location", destinationRegistry.systems[8].locations[Random.Range(0, destinationRegistry.systems[8].locations.Length)]);
        package.Find(Document.Type.Stamp).SetData("system", destinationRegistry.systems[8].name);
        package.Find(Document.Type.Address).SetData("weight", "" + Random.Range(25, 40));
        package.Find(Document.Type.Address).SetData("actual weight", package.Find(Document.Type.Address).GetData("weight"));
    }

    public override void Test(Package package) {
        Break(package);
    }
}
