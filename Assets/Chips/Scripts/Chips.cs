using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chips : MonoBehaviour
{
    // ! === Properties ===
    public List<Chip> denominations = new();
    
    /// <summary>
    /// Returns a dictionary of calculated fields
    /// representing how many chips and of which denominations
    /// a player could have.
    /// </summary>
    /// <param name="balance"></param>
    /// <returns></returns>
    public Dictionary<string, int> CalculateFrom(int balance)
    {   
        // Create Copies
        var ascending = new List<Chip>(denominations);
        var descending = new List<Chip>(denominations);
        descending.Reverse();

        Dictionary<string, int> output = new();
        int remaining = balance;

        // First Pass, Minimum Catch
        foreach (Chip chip in ascending)
        {
            int possible = remaining / chip.denomination;
            int amount = System.Math.Min(possible, chip.min);
            remaining -= amount * chip.denomination;
            output[chip.Id] = amount;
        }

        // Second Pass, Total Catch
        foreach (Chip chip in descending)
        {
            int possible = remaining / chip.denomination;
            remaining -= possible * chip.denomination;
            output[chip.Id] += possible;
        }

        return output;
    }

    /// <summary>
    /// Gets a Chip from a specified id
    /// </summary>
    /// <param name="from_id"></param>
    /// <returns></returns>
    public Chip GetChip(string from_id)
    {
        return denominations.Find(element => element.Id == from_id);
    }
    
    /// <summary>
    /// Gets a Chip from a specified denomination
    /// </summary>
    /// <param name="from_denomination"></param>
    /// <returns></returns>
    public Chip GetChip(int from_denomination)
    {
        return denominations.Find(element => element.denomination == from_denomination);
    }

    // ! === Base Listeners ===
    void Awake()
    {
        denominations = GetComponentsInChildren<Chip>().ToList();
    }


}
