using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainer;
    [SerializeField] private Transform actionBusyUI;

    private List<ActionButtonUI> actionButtonUIList = new List<ActionButtonUI>();

    private void Start() {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChange += UnitActionSystem_OnSelectActionChanged;
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }

    private void CreateUnitActionButtons() {
        foreach(Transform buttonTransform in actionButtonContainer) {
            Destroy(buttonTransform.gameObject);
        }

        actionButtonUIList.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        if (!selectedUnit) return;

        BaseAction[] actions = selectedUnit.GetBaseActionArray();

        foreach(BaseAction action in actions) {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainer);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(action);
            actionButtonUIList.Add(actionButtonUI);
        }
    }

    private void UnitActionSystem_OnSelectUnitChanged(object sender, EventArgs empty) {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }

    private void UnitActionSystem_OnSelectActionChanged(object sender, EventArgs empty) {
        UpdateSelectedVisual();
    }

    private void UnitActionSystem_OnBusyChanged(object sender, bool isBusy) {
        actionBusyUI.gameObject.SetActive(isBusy);
    }

    private void UpdateSelectedVisual() {
        foreach(ActionButtonUI actionButtonUI in actionButtonUIList) {
            actionButtonUI.UpdateSelectedVisual();
        }
    }
}
