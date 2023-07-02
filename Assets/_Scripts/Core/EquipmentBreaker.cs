using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentBreaker
{
    public float TimeStep { get; private set; }
    public List<EquipmentSlot> Slots { get; private set; }
    public Coroutine BreakRoutine { get; set; }

    public EquipmentBreaker(List<EquipmentSlot> slots, float timeStep)
    {
        Slots = slots;
        TimeStep = timeStep;
    }

    public IEnumerator Tick()
    {
        WaitForSeconds wait = new(TimeStep);

        while (true)
        {
            foreach (var slot in Slots)
            {
                if (slot.CurrentItem == null)
                    continue;

                slot.DeathTimer -= TimeStep;

                if (slot.DeathTimer <= 0)
                    slot.SetItem(null);
            }

            yield return wait;
        }
    }
}
