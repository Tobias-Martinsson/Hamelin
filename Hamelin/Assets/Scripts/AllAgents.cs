using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AllAgents
{
    private static List<SomeAgent> allAgents = new List<SomeAgent>();

    private static int listSize = 0;
    public static void AddAgent(SomeAgent a) {
        allAgents.Add(a);
        listSize++;

        Debug.Log(a.transform.position);
        Debug.Log(listSize);
    }

    public static void SaveTransforms() {
        foreach (SomeAgent agent in allAgents)
        {
            if (agent != null)
            {
                agent.AgentSaveTransform();
            }
            else {
               
            }
        }
    }
    public static void ResetEnemies() {
        
        foreach (SomeAgent agent in allAgents) {

            if (agent != null)
            {
                agent.AgentResetTransform();
            }
            else
            {
               
            }


        }
        
        }
    }

