using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerMoveSokoban
{
    public static void MoveToTargetPlayer(Vector3 targetMovePos, Transform playerTransform)
    {
        playerTransform.LookAt(targetMovePos);
        playerTransform.position = targetMovePos;
    }

    public static void UndoPlayerMove(Vector3 prevPos, Transform playerTransform)
    {
        playerTransform.position = prevPos;
    }
}
