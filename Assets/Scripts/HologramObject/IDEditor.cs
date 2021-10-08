using System.Collections;
using System.Collections.Generic;
using DataScripts;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using TMPro;
using UnityEngine;

public class IDEditor : MonoBehaviour
{
    public string ID_Label { get { return iDLabel.text; } set { iDLabel.text = value; } }
    public UI_KeyboardInputField Field { get { return field; } }
    [SerializeField] private TextMeshPro iDLabel;
    [SerializeField] private UI_KeyboardInputField field;
}
