using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ITimer
{
    void InitTimer();
    void AddToTime(float timeToAdd);
    void TickTime();

}
