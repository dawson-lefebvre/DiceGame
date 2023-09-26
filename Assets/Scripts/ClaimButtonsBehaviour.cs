using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClaimButtonsBehaviour : MonoBehaviour
{
    public Button[] buttons;
    [SerializeField] DieBehaviour[] dice;

    private void Start()
    {
     foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }

    public void RefreshButtons()
    {
        Tuple<int, int> twoPair = DiceEvaluator.IsTwoPair(dice);
        if (twoPair.Item1 != 0)
        {
            buttons[0].interactable = true;
        }
        else
        {
            buttons[0].interactable = false;
        }

        int threeOfKind = DiceEvaluator.IsThreeOfKind(dice);
        if(threeOfKind > 0)
        {
            buttons[1].interactable = true;
        }
        else
        {
            buttons[1].interactable = false;
        }

        int fourOfKind = DiceEvaluator.IsFourOfKind(dice);
        if (fourOfKind > 0)
        {
            buttons[2].interactable = true;
        }
        else
        {
            buttons[2].interactable = false;
        }

        Tuple<int, int> fullHouse = DiceEvaluator.IsFullHouse(dice);
        if(fullHouse.Item1 > 0)
        {
            buttons[3].interactable = true;
        }
        else
        {
            buttons[3].interactable = false;
        }

        int smallStright = DiceEvaluator.IsSmallStraight(dice);
        if(smallStright > 0)
        {
            buttons[4].interactable = true;
        }
        else
        {
            buttons[4].interactable = false;
        }

        int largeStright = DiceEvaluator.IsLargeStraight(dice);
        if (largeStright > 0)
        {
            buttons[5].interactable = true;
        }
        else
        {
            buttons[5].interactable = false;
        }
    }
}
