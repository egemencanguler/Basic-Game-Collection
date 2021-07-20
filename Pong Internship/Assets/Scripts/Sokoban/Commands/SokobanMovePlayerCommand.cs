using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SokobanMovePlayerCommand : ICommand
{
    Vector3 targetMovePos;
    Transform playerTransform;
    Vector3 playerPrevPos;
    public SokobanMovePlayerCommand(Vector3 targetMovePos, Transform playerTransform) 
    {
        this.targetMovePos = targetMovePos;
        this.playerTransform = playerTransform;
    }

    public SokobanMovePlayerCommand(Vector3 targetMovePos, Vector3 playerPrevPos)
    {
        this.playerPrevPos = playerPrevPos;
        this.targetMovePos = targetMovePos;
    }

    public void Execute()
    {
       PlayerMoveSokoban.MoveToTargetPlayer(targetMovePos,playerTransform);
    }

    public void Undo()
    {
        PlayerMoveSokoban.UndoPlayerMove(targetMovePos, playerTransform);
    }

    public void Destroy()
    {
    }


    public void Instantiate()
    {
        
    }
}
