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

    public int rollsLeft = 2;

    public void Roll()
    {
        rollsLeft--; //Decrement rolls left
        rollNumberText.text = $"Rolls Left: {rollsLeft}"; //Set UI text
        rollButton.interactable = false; //Set the roll button to not be interactable
        foreach (DieBehaviour d in dice) //Roll any die that is not locked
        {
            if (!d.locked)
            {
                d.Roll();
            }
        }
        StartCoroutine(EvaluateDice()); //Start coroutine to wait for roll "animation" to finish
    }

    IEnumerator EvaluateDice()
    {
        yield return new WaitForSeconds(2); //Wait
        claimButtons.RefreshButtons(); //Let buttons evaluate
        rollButton.interactable = true; //Set Roll Button to be interactable

        if (rollsLeft == 0) //Set roll button to false again if no rolls left
        {
            rollButton.interactable = false;
        }
    }

    public void ResetDice() //Resets all dice to 1, unlocks dice, and sets rolls back to 2
    {
        rollsLeft = 2;
        foreach (DieBehaviour d in dice)
        {
            d.SetValue(1);
            if (d.locked)
            {
                d.LockUnlock();
            }
        }

        foreach (Button b in claimButtons.buttons)
        {
            b.interactable = false;
        }
    }
}
