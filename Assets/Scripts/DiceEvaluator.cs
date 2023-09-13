using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class DiceEvaluator : MonoBehaviour
{
    static int[] CountDice(DieBehaviour[] Dice)
    {
        int[] results = new int[6];
        foreach (DieBehaviour d in Dice)
        {
            results[d.DieValue - 1]++;
        }
        return results;
    }

    static public Tuple<int, int> IsTwoPair(DieBehaviour[] Dice)
    {
        int[] counts = CountDice(Dice);
        int pairs = 0;
        Tuple<int, int> twoPair = new Tuple<int, int>(0, 0);

        for (int i = 0; i < counts.Length; i++)
        {
            if (counts[i] >= 2)
            {
                pairs++;
                if (twoPair.Item1 == 0)
                {
                    twoPair = new Tuple<int, int>(i + 1, twoPair.Item2);
                }
                else
                {
                    twoPair = new Tuple<int, int>(twoPair.Item1, i + 1);
                }
            }
        }

        if (pairs == 2)
        {
            return twoPair;
        }
        else
        {
            return new Tuple<int, int>(0, 0);
        }
    }

    static public int IsThreeOfKind(DieBehaviour[] Dice)
    {
        int[] counts = CountDice(Dice);

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
            if (counts[i] >= 4)
            {
                return i + 1;
            }
        }
        return 0;
    }

    static public Tuple<int, int> IsFullHouse(DieBehaviour[] Dice)
    {
        int[] counts = CountDice(Dice);
        Tuple<int, int> fullHouse = new Tuple<int, int>(0, 0);
        int pairs = 0;
        int trips = 0;
        for (int i = 0; i < counts.Length; i++)
        {
            if (counts[i] == 2)
            {
                pairs++;
                fullHouse = new Tuple<int, int>(i + 1, fullHouse.Item2);
            }
            else if (counts[i] == 3)
            {
                trips++;
                fullHouse = new Tuple<int, int>(fullHouse.Item1, i + 1);
            }
        }

        if (pairs == 1 && trips == 1)
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
        int[] counts = CountDice(Dice);
        int lowest = 0;
        int uniqueNumbers = 0;
        for (int i = 0; i < counts.Length; i++)
        {
            if (counts[i] > 0)
            {
                if (lowest == 0) { lowest = i + 1; }
                uniqueNumbers++;
            }
            else
            {
                if(uniqueNumbers == 3)
                {
                    break;
                }
            }
        }

        if (uniqueNumbers >= 4)
        {
            return lowest;
        }
        else { return 0; }
    }

    static public int IsLargeStraight(DieBehaviour[] Dice)
    {
        int[] counts = CountDice(Dice);
        int lowest = 0;
        int uniqueNumbers = 0;
        for (int i = 0; i < counts.Length; i++)
        {
            if (counts[i] > 0)
            {
                if (lowest == 0) { lowest = i + 1; }
                uniqueNumbers++;
            }
            else
            {
                if (uniqueNumbers == 4)
                {
                    break;
                }
            }
        }

        if (uniqueNumbers == 5)
        {
            return lowest;
        }
        else { return 0; }
    }
}
