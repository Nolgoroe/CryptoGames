using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChainAction
{
    void NotifyObserver(BallBase ball, int currentComboReached);
    void NotifyObserverReset();
}
