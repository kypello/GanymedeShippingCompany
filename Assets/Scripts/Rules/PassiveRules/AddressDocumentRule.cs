using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AddressDocumentRule", menuName = "Rules/AddressDocumentRule")]
public class AddressDocumentRule : Rule
{
    public override bool Verify(Package package) {
        errorText = "Missing destination sticker";
        return package.Find(Document.Type.Address) != null;
    }

    public override void Break(Package package) {
        package.RemoveDocument(package.Find(Document.Type.Address), true);
    }
}
