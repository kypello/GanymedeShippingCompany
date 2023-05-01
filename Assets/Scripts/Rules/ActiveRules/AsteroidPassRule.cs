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
}
