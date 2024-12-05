using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class Army
{
    public HeroManager owner;
    
    private readonly List<Unit> _units = new List<Unit>(new Unit[7]);

    public Unit this[int index]
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
        return unit?.stackSize ?? 0;
    }

    public void SetStackSize(int index, int size)
    {
        var unit = this[index];
        if(unit == null) throw new InvalidOperationException("Cannot set stack size of an empty unit slot - " + owner.cHeroInfo.Name + " 's army");
        unit.stackSize = size;
    }

    public IEnumerable<Unit> GetAllUnits()
    {
        foreach (Unit unit in _units)
        {
            if(unit != null) yield return unit;
        }
    }
    
    public void AddUnit(Unit unit, int amount)
    {
        for (int i = 0; i < _units.Count; i++)
        {
            if (_units[i].UnitName == unit.UnitName)
            {
                _units[i].stackSize += amount;
                return;
            }
            else if (_units[i] == null)
            {
                _units[i] = unit;
                _units[i].stackSize = amount;
                _units[i].OwnerHero = owner;
                return;
            }
            else
            {
                Debug.LogError($"{unit.UnitName} can't be added to {owner.gameObject.name}'s army, army is full");
            }
        }
        
    }
    
    public void AddUnit(KeyValuePair<Unit, int> kvp)
    {
        AddUnit(kvp.Key, kvp.Value);
    }

    public void RemoveUnit(Unit unit, int amount)
    {
        for (int i = 0; i < _units.Count; i++)
        {
            if (_units[i].UnitName == unit.UnitName)
            {
                _units[i].stackSize = Mathf.Max(0, _units[i].stackSize - amount);
                if (_units[i].stackSize == 0)
                {
                    _units[i] = null;
                }
                return;
            }
            else
            {
                Debug.LogError($"{unit.UnitName} does not exist in {owner.gameObject.name}'s army, something went wrong with the unit input");
            }
        }
    }

    public void RemoveUnit(KeyValuePair<Unit, int> kvp)
    {
        RemoveUnit(kvp.Key, kvp.Value);
    }
    
    void CheckIndex(int index)
    {
        if(index < 0 || index >= _units.Count)
            throw new IndexOutOfRangeException("Army index out of bounds - " + owner.cHeroInfo.Name + " 's army");
    }
}
