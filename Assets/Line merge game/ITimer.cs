using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ITimer
{
    void InitTimer();
    void AddToTime(int timeToAdd);
    void TickTime();

}
