using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuarantineDaysRule", menuName = "Rules/Active/QuarantineDaysRule")]
public class QuarantineDaysRule : Rule
{
    [System.Serializable]
    public struct LocationQuarantinePeriod {
        public string location;
        public string days;
    }

    public LocationQuarantinePeriod[] locationQuarantinePeriods;

    public override bool Verify(Package package) {
        if (package.Find(Document.Type.QuarantineMandate) == null) {
            return true;
        }

        errorText = "Incorrect quarantine period";

        foreach (LocationQuarantinePeriod locationQuarantinePeriod in locationQuarantinePeriods) {
            if (locationQuarantinePeriod.location == package.Find(Document.Type.Address).GetData("location")) {
                return locationQuarantinePeriod.days == package.Find(Document.Type.QuarantineMandate).GetData("days");
            }
        }

        return true;
    }
}
