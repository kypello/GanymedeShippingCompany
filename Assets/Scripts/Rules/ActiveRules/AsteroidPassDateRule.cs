using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AsteroidPassDateRule", menuName = "Rules/Active/AsteroidPassDateRule")]
public class AsteroidPassDateRule : Rule
{
    public override bool Verify(Package package) {
        if (package.Find(Document.Type.AsteroidPass) == null) {
            return true;
        }

        errorText = "Incorrect date of arrival in Asteroid Belt";

        return int.Parse(package.Find(Document.Type.AsteroidPass).GetData("date")) == DateAuthority.date + 3;
    }
}
