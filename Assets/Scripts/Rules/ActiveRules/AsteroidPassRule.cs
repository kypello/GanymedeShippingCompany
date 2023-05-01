using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AsteroidPassRule", menuName = "Rules/Active/AsteroidPassRule")]
public class AsteroidPassRule : Rule
{
    public override bool Verify(Package package) {
        int systemIndex = 0;
        for (int i = 0; i < 10; i++) {
            if (destinationRegistry.systems[i].name == package.Find(Document.Type.Address).GetData("system")) {
                systemIndex = i;
                break;
            }
        }

        if (package.Find(Document.Type.AsteroidPass) != null) {
            if (systemIndex < 5) {
                return true;
            }
            else {
                errorText = "Unnecessary Asteroid Pass";
                return false;
            }
        }
        else {
            if (systemIndex >= 5) {
                return true;
            }
            else {
                errorText = "Missing Asteroid Pass";
                return false;
            }
        }
    }

    public override void Test(Package package) {
        int systemIndex = Random.Range(0, 5);
        package.Find(Document.Type.Address).SetData("system", destinationRegistry.systems[systemIndex].name);
        package.Find(Document.Type.Address).SetData("location", destinationRegistry.systems[systemIndex].locations[Random.Range(0, destinationRegistry.systems[systemIndex].locations.Length)]);
        package.Find(Document.Type.Stamp).SetData("system", destinationRegistry.systems[systemIndex].name);
    }
}
