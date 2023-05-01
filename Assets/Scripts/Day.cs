using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Day", menuName = "Day")]
public class Day : ScriptableObject
{
    public enum LegalDestinations {Jovian, GasGiants, All}
    public LegalDestinations legalDestinations = LegalDestinations.Jovian;

    public Rule[] passiveRules;
    public Rule[] activeRules;

    public int availableDocuments;

    [TextAreaAttribute]
    public string storyText;

    public Rule testRule;
    public int testRulePackageNum;
    public bool doRuleTest;
}
