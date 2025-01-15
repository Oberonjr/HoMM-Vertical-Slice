using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class Army
{
    [HideInInspector]public HeroInfo owner;
    
    public UnitSlot[] _units = new UnitSlot[7];

    public Army()
    {
        for (int i = 0; i < _units.Length; i++)
        {
            if (_units[i] == null) return;
            _units[i] = new UnitSlot(null, 0);
        }
    }
    public UnitSlot this[int index]
    {
        get
        {
            CheckIndex(index);
            return _units[index];
        }
        set
        {
            CheckIndex(index);
            _units[index] = value;
        }
    }

    public int GetStackSize(int index)
    {
        var unit = this[index];
        return unit?.amount ?? 0;
    }

    public void SetStackSize(int index, int size)
    {
        var unit = this[index];
        if(unit == null) throw new InvalidOperationException("Cannot set stack size of an empty unit slot - " + owner.Name + " 's army");
        unit.amount = size;
    }

    public IEnumerable<UnitSlot> GetAllUnits()
    {
        foreach (UnitSlot unit in _units)
        {
            if(unit != null) yield return unit;
        }
    }
    
    public void AddUnit(UnitStats unit, int amount)
    {
        for (int i = 0; i < _units.Length; i++)
        {
            if (_units[i] != null && _units[i].identifier == unit.unitName)
            {
                _units[i].amount += amount;
                return;
            }
            else if (_units[i] == null || _units[i].amount <= 0)
            {
                _units[i].stats = unit;
                _units[i].amount = amount;
                return;
            }
            else
            {
                Debug.LogError($"{unit.unitName} can't be added to {owner.Name}'s army, army is full");
            }
        }
        
    }
    
    public void AddUnit(KeyValuePair<UnitStats, int> kvp)
    {
        AddUnit(kvp.Key, kvp.Value);
    }

    public void RemoveUnit(UnitStats unit, int amount)
    {
        for (int i = 0; i < _units.Length; i++)
        {
            if (_units[i] != null && _units[i].identifier == unit.unitName)
            {
                _units[i].amount = Mathf.Max(0, _units[i].amount - amount);
                if (_units[i].amount == 0)
                {
                    _units[i].stats = null;
                    _units[i].identifier = null;
                    
                }
                return;
            }
            else
            {
                Debug.LogError($"{unit.unitName} does not exist in {owner.Name}'s army, something went wrong with the unit input");
            }
        }
    }

    public void RemoveUnit(KeyValuePair<UnitStats, int> kvp)
    {
        RemoveUnit(kvp.Key, kvp.Value);
    }
    
    void CheckIndex(int index)
    {
        if(index < 0 || index >= _units.Length)
            throw new IndexOutOfRangeException("Army index out of bounds - " + owner.Name + " 's army");
    }
}
