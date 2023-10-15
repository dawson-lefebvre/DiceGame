using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class DiceEvaluator : MonoBehaviour
{
    public static int[] CountDice(DieBehaviour[] Dice)
    {
        //Makes an array with how many dice of each value is in the set of 5
        int[] results = new int[6];

        //Adds 1 to the proper array spot for each die
        foreach (DieBehaviour d in Dice)
        {
            results[d.DieValue - 1]++;
        }
        return results;
    }

    static public Tuple<int, int> IsTwoPair(DieBehaviour[] Dice)
    {
        //Returns a tuple with the pairs in the dice set
        int[] counts = CountDice(Dice);
        int pairs = 0;
        Tuple<int, int> twoPair = new Tuple<int, int>(0, 0);

        //Add to 1 to pairs if the count is greater than 2 and populate the tuple with the value of the pair.
        for (int i = 0; i < counts.Length; i++)
        {
            if (counts[i] >= 2)
            {
                pairs++;
                //If tuple hasn't been populated yet, assign to item1
                if (twoPair.Item1 == 0)
                {
                    twoPair = new Tuple<int, int>(i + 1, twoPair.Item2);
                }
                else //assign to item 2 if 1 pair has already been found
                {
                    twoPair = new Tuple<int, int>(twoPair.Item1, i + 1);
                }
            }
        }

        return twoPair;
    }

    static public int IsThreeOfKind(DieBehaviour[] Dice)
    {
        int[] counts = CountDice(Dice);

        //Simply return the value of the die if a count is of 3 or more
        for (int i = 0; i < counts.Length; i++)
        {
            if (counts[i] >= 3)
            {
                return i + 1;
            }
        }
        return 0;
    }

    static public int IsFourOfKind(DieBehaviour[] Dice)
    {
        int[] counts = CountDice(Dice);

        for (int i = 0; i < counts.Length; i++)
        {
            //Simply return the value of the die if a count is of 4 or more
            if (counts[i] >= 4)
            {
                return i + 1;
            }
        }
        return 0;
    }

    static public Tuple<int, int> IsFullHouse(DieBehaviour[] Dice) //Returns a tuple with the pair value and tripple value
    {
        //Grabs dice counts
        int[] counts = CountDice(Dice);
        Tuple<int, int> fullHouse = new Tuple<int, int>(0, 0);
        int pairs = 0;
        int trips = 0;

        for (int i = 0; i < counts.Length; i++)
        {
            if (counts[i] == 2) //Adds to pair if the count is exactly 2
            {
                pairs++;
                fullHouse = new Tuple<int, int>(i + 1, fullHouse.Item2);
            }
            else if (counts[i] == 3) //Adds to tripple if count is exactly 3
            {
                trips++;
                fullHouse = new Tuple<int, int>(fullHouse.Item1, i + 1);
            }
        }

        if (pairs == 1 && trips == 1) //Returns the tuple if a pair and trip was found. Else, return a 0, 0 tuple
        {
            return fullHouse;
        }
        else
        {
            return new Tuple<int, int>(0, 0);
        }
    }

    static public int IsSmallStraight(DieBehaviour[] Dice)
    {
        //Grabs numbers in sequence
        int[] sequenceEval = InSequence(Dice);

        //If sequence is 4 or more, return the start of the straight
        if (sequenceEval[0] >= 4)
        {
            return sequenceEval[1];
        }
        else
        {
            return 0;
        }

    }

    static public int IsLargeStraight(DieBehaviour[] Dice)
    {
        //Get dice counts
        int[] counts = CountDice(Dice);
        int lowest = 0;
        int uniqueNumbers = 0;

        //Adds to unique numbers if count is 1 or more
        for (int i = 0; i < counts.Length; i++)
        {
            if (counts[i] > 0)
            {
                if (lowest == 0) { lowest = i + 1; } //If this is the first number, assign the value to lowest of straight
                uniqueNumbers++;
            }
            else
            {
                if (uniqueNumbers == 4) //If a gap is found and there are 4 numbers found, there is no large stright. Breaks out of loop.
                {
                    break;
                }
            }
        }

        if (uniqueNumbers == 5) //If 5 in sequence numbers are found, return the lowest in sequence. Return 0 otherwise
        {
            return lowest;
        }
        else { return 0; }
    }

    public static List<int> InSequenceWithGaps(DieBehaviour[] dice)
    {
        //This method returns a list of the numebers the AI should keep. This will include numbers if there is a gap of 1 in the sequence but no more

        //Get count and create variables
        int[] counts = CountDice(dice);
        int currentSequence = 0, currentGap = 0;
        List<int> numbersToKeep = new List<int>();
        bool keepNumbers = true;



        for(int c = 0; c < counts.Length; c++)
        {
            if (counts[c] > 0) //If the count of this value is more than 0
            {
                currentGap = 0; //Set gap to 0
                currentSequence++; //Add 1 to the in sequence
                if (keepNumbers)
                {
                    numbersToKeep.Add(c + 1); //Add number to the list if no gap of 2 has been found
                }
            }
            else //If gap is found
            {
                currentGap++; //Add 1 to the gap
                if (currentGap > 1 && numbersToKeep.Count > 0) //If the gap is more than 1 and numbers have been added to the list, prevent more numbers from being added.
                {
                    keepNumbers = false;
                }
                currentSequence = 0;
            }
        }

        return numbersToKeep;
    }

    public static int[] InSequence(DieBehaviour[] dice)
    {
        //This is a copy of the previous method which is actually just the original method I wrote that returns a sequence without gaps. This method returns the highest sequence of numbers as the first item in the array, and the start of said sequence as the second item.
        int[] counts = CountDice(dice);
        int currentSequence = 0, currentGap = 0;
        int[] sequence = new int[2];
        sequence[0] = 0;

        for (int c = 0; c < counts.Length; c++)
        {
            if (counts[c] > 0)
            {
                currentSequence++;
                if (currentSequence > sequence[0])
                {
                    //Sets highest and uses formula to set start of sequence
                    sequence[0] = currentSequence;
                    sequence[1] = c + 1 - currentSequence + 1;
                }
            }
            else
            {
                currentSequence = 0;
                currentGap++;
            }
        }

        return sequence;
    }
}
