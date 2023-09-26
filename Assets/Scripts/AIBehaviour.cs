using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
    [SerializeField] DiceManager diceManager;
    [SerializeField] ClaimButtonsBehaviour claimButtonsBehaviour;

    bool hasLargeStright = false, hasSmallStright = false, hasFullHouse = false, hasFourOfKind = false, hasThreeOfKind = false, hasTwoPair = false;

    public void TakeFirstRoll()
    {
        diceManager.Roll();
        StartCoroutine(WaitForRoll());
    }

    public IEnumerator WaitForRoll()
    {
        yield return new WaitForSeconds(1);
        EvaluateFirstRoll();
    }

    void EvaluateFirstRoll()
    {
        if (DiceEvaluator.IsLargeStraight(diceManager.dice) > 0 && !hasLargeStright)
        {
            hasLargeStright = true;
            return;
        }

        if (DiceEvaluator.IsSmallStraight(diceManager.dice) > 0 && !hasSmallStright) 
        {
            hasSmallStright = true;
            return;
        }

        if(DiceEvaluator.IsFullHouse(diceManager.dice).Item1 > 0 && !hasFullHouse)
        {
            hasFullHouse = true;
            return;
        }

        if(DiceEvaluator.IsFourOfKind(diceManager.dice) > 0 && !hasFourOfKind)
        {
            hasFourOfKind = true;
            return;
        }

        if (DiceEvaluator.IsThreeOfKind(diceManager.dice)  > 0 && !hasThreeOfKind)
        {
            hasThreeOfKind = true;
            return;
        }

        if(DiceEvaluator.IsTwoPair(diceManager.dice).Item1 > 0 && !hasTwoPair)
        {
            hasTwoPair = true;
            return;
        }

        //Roll for straight if 3 or more in sequence or if straight is all there is left.
        if (
            (DiceEvaluator.InSequence(diceManager.dice) >= 3 && !(hasLargeStright && hasSmallStright))
            || (hasTwoPair && hasThreeOfKind && hasFourOfKind && hasFullHouse)
           )
        {
            //Lock dice in sequence
            int[] counts = DiceEvaluator.CountDice(diceManager.dice);
        }
    }
}
