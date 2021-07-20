using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker : MonoBehaviour
{
    static Queue<ICommand> commandQueue;
    static List<ICommand> oldCommands;
    static List<int> hazardSpawnIndex;
    static List<int> hazardDestroyIndex;

    bool undoEverything = false;
    public float undoTime = 1f;
    float timer = 0f;
    static List<int> playerIndex;

    private void Awake()
    {
        commandQueue = new Queue<ICommand>();
        oldCommands = new List<ICommand>();
        hazardSpawnIndex = new List<int>();
        hazardDestroyIndex = new List<int>();
        playerIndex = new List<int>();
    }

    public static void AddCommand(ICommand command) 
    {
        commandQueue.Enqueue(command);
    }

    public static void SaveDestroyCommand(ICommand command) 
    {
        oldCommands.Add(command);
        hazardDestroyIndex.Add(oldCommands.Count - 1);
    }
    public static void SaveSpawnCommand(ICommand command)
    {
        oldCommands.Add(command);
        hazardSpawnIndex.Add(oldCommands.Count - 1);
    }
    public static void SavePlayerCommand(ICommand command)
    {
        oldCommands.Add(command);
        playerIndex.Add(oldCommands.Count - 1);
    }

    public static void SaveHazardCommand(ICommand command) 
    {
        oldCommands.Add(command);
    }

    public static void SaveBoxCommand(ICommand command)
    {
        oldCommands.Add(command);
    }
    public static void RemoveLastSavedCommand()
    {
        oldCommands.RemoveAt(oldCommands.Count - 1);
    }

    void Update()
    {
        timer += Time.deltaTime;

        for (int a = 0; a < commandQueue.Count; a++) 
        {
            ICommand command =  commandQueue.Dequeue();
            command.Execute();
        }

        //The undo process works by travelling through a list.
        //When the last registered player move is found all the commands up to it are undone at the same frame.
        //This makes all the undo process happen at one turn
        if (Input.GetKeyDown(KeyCode.R) && oldCommands.Count > 0 && playerIndex.Count > 0)
        {
            while (oldCommands.Count - 1 >= playerIndex[playerIndex.Count - 1] && oldCommands.Count > 0)
            {
                int count = oldCommands.Count;
                if (hazardDestroyIndex.Count > 0) 
                {
                    if (oldCommands.Count - 1 == hazardDestroyIndex[hazardDestroyIndex.Count - 1])
                    {
                        ICommand command = oldCommands[oldCommands.Count - 1];
                        command.Destroy();
                        RemoveLastSavedCommand();
                        hazardDestroyIndex.RemoveAt(hazardDestroyIndex.Count - 1);
                    }
                }

                if (hazardSpawnIndex.Count > 0)
                {
                    if (oldCommands.Count - 1 == hazardSpawnIndex[hazardSpawnIndex.Count - 1])
                    {
                        ICommand command = oldCommands[oldCommands.Count - 1];
                        command.Instantiate();
                        RemoveLastSavedCommand();
                        hazardSpawnIndex.RemoveAt(hazardSpawnIndex.Count - 1);
                    }
                }

                if (count == oldCommands.Count) 
                {
                    UndoCommand();
                }
            }

            if (playerIndex.Count > 0)
            {
                playerIndex.RemoveAt(playerIndex.Count - 1);
            }

        }

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            undoEverything = true;
        }

        if (undoEverything && oldCommands.Count > 0 && playerIndex.Count > 0 && timer >= undoTime) 
        {
            timer = 0f;
            while (oldCommands.Count - 1 >= playerIndex[playerIndex.Count - 1] && oldCommands.Count > 0) 
            {
                int count = oldCommands.Count;
                if (hazardDestroyIndex.Count > 0)
                {
                    if (oldCommands.Count - 1 == hazardDestroyIndex[hazardDestroyIndex.Count - 1])
                    {
                        ICommand command = oldCommands[oldCommands.Count - 1];
                        command.Destroy();
                        RemoveLastSavedCommand();
                        hazardDestroyIndex.RemoveAt(hazardDestroyIndex.Count - 1);
                    }
                }

                if (hazardSpawnIndex.Count > 0)
                {
                    if (oldCommands.Count - 1 == hazardSpawnIndex[hazardSpawnIndex.Count - 1])
                    {
                        ICommand command = oldCommands[oldCommands.Count - 1];
                        command.Instantiate();
                        RemoveLastSavedCommand();
                        hazardSpawnIndex.RemoveAt(hazardSpawnIndex.Count - 1);
                    }
                }

                if (count == oldCommands.Count)
                {
                    UndoCommand();
                }
            }
            if (playerIndex.Count > 0) 
            {
                playerIndex.RemoveAt(playerIndex.Count - 1);
            }
            if (oldCommands.Count == 1) 
            {
                UndoCommand();
            }
            if (oldCommands.Count == 0) 
            {
                undoEverything = false;
            }
        }
    }

    public void UndoCommand() 
    {
        oldCommands[oldCommands.Count - 1].Undo();
        RemoveLastSavedCommand();
    }
}
