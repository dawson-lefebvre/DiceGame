using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AIBehaviour : MonoBehaviour
{
    //References
    [SerializeField] DiceManager diceManager;
    [SerializeField] ClaimButtonsBehaviour claimButtonsBehaviour;
    [SerializeField] Button endTurnButton;

    //Bools for what combos the AI has
    bool hasLargeStright = false, hasSmallStright = false, hasFullHouse = false, hasFourOfKind = false, hasThreeOfKind = false, hasTwoPair = false;

    public void TakeRoll()
    {
        //Rolls dice and starts the wait coroutine
        diceManager.Roll(); 
        StartCoroutine(WaitForRoll());
    }

    public IEnumerator WaitForRoll()
    {
        yield return new WaitForSeconds(5); //Wait 5 seconds

        if (diceManager.rollsLeft == 1) //If this is the first roll, look for combos. If no combo claimed, evaluate
        {
            if (!LookAndClaim())
            {
                EvaluateFirstRoll();
            }
            else
            {
                EndTurn();
            }
        }
        else //If second roll, look for combos, claim if can, and end turn
        {
            LookAndClaim();
            EndTurn();
        }
    }

    void EvaluateFirstRoll()
    {
        //Roll for straight if 3 or more in sequence with gap or if straight is all there is left.
        if (
            (DiceEvaluator.InSequenceWithGaps(diceManager.dice).Count >= 3 && !(hasLargeStright && hasSmallStright))
            || (hasTwoPair && hasThreeOfKind && hasFourOfKind && hasFullHouse)
           )
        {

            List<int> numbersToKeep = DiceEvaluator.InSequenceWithGaps(diceManager.dice);

            //Lock dice in sequence
            foreach (DieBehaviour d in diceManager.dice)
            {
                if (numbersToKeep.Contains(d.DieValue))
                {
                    //Locks Die and removes number to look for from array
                    d.LockUnlock();
                    numbersToKeep.Remove(d.DieValue);
                }
            }

            //Roll dice and eval again
            TakeRoll();
            return;
        }

        //Check for triples 
        if (DiceEvaluator.IsThreeOfKind(diceManager.dice) > 0 && (!hasFullHouse || !hasFourOfKind || !hasTwoPair))
        {
            int trips = DiceEvaluator.IsThreeOfKind(diceManager.dice);
            int diceToLock = 3;

            //Locks 3 if needs four of kind or full house, otherwise rolls for 2 pair
            if (!hasFullHouse || !hasFourOfKind)
            {
                diceToLock = 3;
            }
            else
            {
                diceToLock = 2;
            }

            //Lock dice of that value
            foreach (DieBehaviour d in diceManager.dice)
            {
                if (d.DieValue == trips)
                {
                    d.LockUnlock();
                    diceToLock--;
                    if (diceToLock == 0)
                    {
                        break;
                    }
                }
            }
            TakeRoll();
            return;
        }

        //Check for pairs
        if (DiceEvaluator.IsTwoPair(diceManager.dice).Item1 > 0 && (!hasFourOfKind || !hasFullHouse || !hasThreeOfKind))
        {

            Tuple<int, int> pairs = DiceEvaluator.IsTwoPair(diceManager.dice);
            //Locks both pairs if two are present and full house is needed. Otherwise locks one pair
            if (pairs.Item2 > 0 && !hasFullHouse)
            { 
                foreach (DieBehaviour d in diceManager.dice)
                {
                    if(d.DieValue == pairs.Item1 || d.DieValue == pairs.Item2)
                    {
                        d.LockUnlock();
                    }
                }
            }
            else
            {
                foreach (DieBehaviour d in diceManager.dice)
                {
                    if (d.DieValue == pairs.Item1)
                    {
                        d.LockUnlock();
                    }
                }
            }

            TakeRoll();
            return;
        }

        TakeRoll();
        return;
    }

    //Returns true if claimed
    bool LookAndClaim()
    {
        //Check for combo in order of rarity. Claim if true.
        if (DiceEvaluator.IsLargeStraight(diceManager.dice) > 0 && !hasLargeStright)
        {
            hasLargeStright = true;
            claimButtonsBehaviour.buttons[5].gameObject.SetActive(false);
            return true;
        }

        if (DiceEvaluator.IsSmallStraight(diceManager.dice) > 0 && !hasSmallStright)
        {
            //If AI doesn't have a large stright and this is the first roll, return false and send to evaluation. AI will see this and roll the die not apart of the straight to try and get large straight.
            if (!hasLargeStright && diceManager.rollsLeft == 1)
            {
                return false;
            }
            hasSmallStright = true;
            claimButtonsBehaviour.buttons[4].gameObject.SetActive(false);
            return true;
        }

        if (DiceEvaluator.IsFullHouse(diceManager.dice).Item1 > 0 && !hasFullHouse)
        {
            hasFullHouse = true;
            claimButtonsBehaviour.buttons[3].gameObject.SetActive(false);
            return true;
        }

        if (DiceEvaluator.IsFourOfKind(diceManager.dice) > 0 && !hasFourOfKind)
        {
            hasFourOfKind = true;
            claimButtonsBehaviour.buttons[2].gameObject.SetActive(false);
            return true;
        }

        if (DiceEvaluator.IsThreeOfKind(diceManager.dice) > 0 && !hasThreeOfKind)
        {
            //If AI doesn't have a four of a kind or full house and this is the first roll, return false and send to evaluation. AI will see this and roll the dice not apart of the three to try and get four of a kind or full house.
            if ((!hasFourOfKind || !hasFullHouse) && diceManager.rollsLeft == 1)
            {
                return false;
            }
            hasThreeOfKind = true;
            claimButtonsBehaviour.buttons[1].gameObject.SetActive(false);
            return true;
        }

        if (DiceEvaluator.IsTwoPair(diceManager.dice).Item2 > 0 && !hasTwoPair)
        {
            //If AI doesn't have a full house and this is the first roll, return false and send to evaluation. AI will see this and roll the dice not apart of the pairs to try and get full house.
            if (!hasFullHouse && diceManager.rollsLeft == 1)
            {
                return false;
            }
            hasTwoPair = true;
            claimButtonsBehaviour.buttons[0].gameObject.SetActive(false);
            return true;
        }

        return false;
    }

    void EndTurn()
    {
        endTurnButton.interactable = true;
    }

    public void TakeNextTurn()
    {
        //Resets dice, and rolls again.
        diceManager.ResetDice();
        endTurnButton.interactable = false;
        TakeRoll();
    }
}
