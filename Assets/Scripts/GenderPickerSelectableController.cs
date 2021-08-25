using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GenderPickerSelectableController : MonoBehaviour, 
    ISelectHandler,
    IEventSystemHandler
{
    private static readonly string selectedGender = PlayerPrefsKey.menuGenderSelectionPrefName;

    public void OnSelect(BaseEventData eventData)
    {
        PlayerPrefs.SetString(selectedGender, this.gameObject.name);
    }
}
