using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AllAgents
{
    private static List<SomeAgent> allAgents = new List<SomeAgent>();

    private static int counter = 0;
    private static int listSize = 0;
    public static void AddAgent(SomeAgent a) {
        allAgents.Add(a);
        listSize++;

        Debug.Log(a.transform.position);
        Debug.Log(listSize);
    }

    public static List<SomeAgent> GetList() {

        return allAgents;
    }

    public static void ResetEnemies() {
        
        foreach (SomeAgent agent in allAgents) {

            for (int i = 0; i > listSize; i++)
            {
                if (agent.Equals(allAgents[counter])){
                    agent.resetTransform(allAgents[counter].transform);
                }
        
            }
        }
        
        }
    }

