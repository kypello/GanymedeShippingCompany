using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JovianRule", menuName = "Rules/JovianRule")]
public class JovianRule : Rule
{
    public override bool Verify(Package package) {
        errorText = "No shipping beyond the Jovian system";
        return package.Find(Document.Type.Address).GetData("system") == "Jovian System";
    }

    public override void Break(Package package) {
        int systemIndex;
        do {
            systemIndex = Random.Range(0, destinationRegistry.systems.Length);
        } while (destinationRegistry.systems[systemIndex].name == "Jovian System");
        package.Find(Document.Type.Address).SetData("system", destinationRegistry.systems[systemIndex].name);
        package.Find(Document.Type.Address).SetData("location", destinationRegistry.systems[systemIndex].locations[Random.Range(0, destinationRegistry.systems[systemIndex].locations.Length)]);
        package.Find(Document.Type.Stamp).SetData("system", destinationRegistry.systems[systemIndex].name);
    }

    public override void Test(Package package) {
        Break(package);
    }
}
