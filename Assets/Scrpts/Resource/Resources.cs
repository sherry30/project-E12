using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Resources
{
     [SerializeField]
    public  DictionaryResFloat resources;
    [SerializeField]
    public DictionaryRawFloat RawMaterials;
    [SerializeField]
    public DictionaryEnergyFloat Energies;
    [SerializeField]
    public DictionaryOtherResFloat OtherResources;
    [SerializeField]
    public DictionaryCrystalFloat Crystals;
    public Resources(){
    }
    public void Initialize(){
        resources = new DictionaryResFloat();
        foreach(Resource pieceType in Enum.GetValues(typeof(Resource)))
        {
            resources.Add(pieceType,0);
        }
        
        //checking if Raw material cost is met
        RawMaterials = new DictionaryRawFloat();
        foreach(Raw_Material pieceType in Enum.GetValues(typeof(Raw_Material)))
        {
            RawMaterials.Add(pieceType,0);
        }
        //checking if otherResource cost is met
        Energies = new DictionaryEnergyFloat();
        foreach(Energy pieceType in Enum.GetValues(typeof(Energy)))
        {
            Energies.Add(pieceType,0);
        }
        OtherResources = new DictionaryOtherResFloat();
        foreach(OtherResource pieceType in Enum.GetValues(typeof(OtherResource)))
        {
            OtherResources.Add(pieceType,0);
        }
        Crystals = new DictionaryCrystalFloat();
        foreach(crystal pieceType in Enum.GetValues(typeof(crystal)))
        {
            Crystals.Add(pieceType,0);
        }
    }

}
