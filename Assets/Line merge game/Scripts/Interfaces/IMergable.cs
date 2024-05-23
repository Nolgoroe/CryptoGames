using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IMergable
{
    void MergeAction(BallBase otherBall);

    void OnMergeActions();
}
