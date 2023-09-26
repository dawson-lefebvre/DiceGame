using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    public DieBehaviour[] dice;
    [SerializeField] ClaimButtonsBehaviour claimButtons;
    [SerializeField] TextMeshProUGUI rollNumberText;
    [SerializeField] Button rollButton;

    int rollsLeft = 2;

    public void Roll()
    {
        rollsLeft--;
        rollNumberText.text = $"Rolls Left: {rollsLeft}";
        rollButton.interactable = false;
        foreach (DieBehaviour d in dice)
        {
            if (!d.locked)
            {
                d.Roll();
            }
        }
        StartCoroutine(EvaluateDice());
    }

    IEnumerator EvaluateDice()
    {
        yield return new WaitForSeconds(1);
        claimButtons.RefreshButtons();
        rollButton.interactable = true;

        if (rollsLeft == 0)
        {
            rollButton.interactable = false;
        }
    }

    public void ResetDice()
    {
        foreach (DieBehaviour d in dice)
        {
            d.SetValue(1);
        }

        foreach (Button b in claimButtons.buttons)
        {
            b.interactable = false;
        }
    }
}
