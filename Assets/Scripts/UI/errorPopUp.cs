using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class errorPopUp : MonoBehaviour
{
    [SerializeField] private Canvas errorCanvas;
    [SerializeField] private TextMeshProUGUI errorTitel;
    [SerializeField] private TextMeshProUGUI errorMessage;
    [SerializeField] private TextMeshProUGUI êrrorButtonText;

    public void SetData(string errorTitel, string errorMessage, bool retry)
    {

    }
}
