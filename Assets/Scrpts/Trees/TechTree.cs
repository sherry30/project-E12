using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DictionaryTechCodeTechSkill: SerializableDictionary<TechCode,TechSkill>{}
[System.Serializable] 
public class TechTree
{  
    public DictionaryTechCodeTechSkill techSkills;
    public bool underResearch=false;
    public void StartTurn(){
        foreach(KeyValuePair<TechCode,TechSkill> entry in techSkills){
            entry.Value.StartTurn();
        }
    }

    
}
