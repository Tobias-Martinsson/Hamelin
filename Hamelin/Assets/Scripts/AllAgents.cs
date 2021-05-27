using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AllAgents 
{
    public static List<SomeAgent> allAgents = new List<SomeAgent>();
    public static List<EnemySaveData> allAgentSaveData = new List<EnemySaveData>();


    private static int listSize = 0;
    public static void AddAgent(SomeAgent a) {
        allAgents.Add(a);
        listSize++;
    }

    public static List<EnemySaveData> convertToSaveData()
    {
        allAgentSaveData.Clear();
        foreach (SomeAgent a in allAgents)
        {
            
            EnemySaveData temp = new EnemySaveData(a);
            allAgentSaveData.Add(temp);

        }
        return allAgentSaveData;
    }

    public static void removeAgent(SomeAgent a)
    {
        allAgents.Remove(a);
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

