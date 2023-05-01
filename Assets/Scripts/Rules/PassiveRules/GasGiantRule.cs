using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GasGiantRule", menuName = "Rules/GasGiantRule")]
public class GasGiantRule : Rule
{
    public override bool Verify(Package package) {
        errorText = "No shipping through the Asteroid Belt";
        string system = package.Find(Document.Type.Address).GetData("system");
        return  system == "Jovian System" || system == "Kronian System" || system == "Neptunian System" || system == "Uranian System" || system == "Kuiper Belt";
    }

    public override void Break(Package package) {
        int systemIndex = Random.Range(0, 5);
        
        package.Find(Document.Type.Address).SetData("system", destinationRegistry.systems[systemIndex].name);
        package.Find(Document.Type.Address).SetData("location", destinationRegistry.systems[systemIndex].locations[Random.Range(0, destinationRegistry.systems[systemIndex].locations.Length)]);
        package.Find(Document.Type.Stamp).SetData("system", destinationRegistry.systems[systemIndex].name);
    }
}
