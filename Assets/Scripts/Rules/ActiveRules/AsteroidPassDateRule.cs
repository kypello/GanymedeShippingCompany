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

        int dateField;
        bool fieldIsInt = int.TryParse(package.Find(Document.Type.AsteroidPass).GetData("date"), out dateField);

        return fieldIsInt && dateField == DateAuthority.date + 3;
    }
}
