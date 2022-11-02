using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
	public static CommandManager Instance { get; private set; }
	private Stack<IMovementCommand> commands = new();
	public int CommandCount { get => commands.Count; }


	private void Awake()
	{
		Instance = this;	
	}

	public void AddCommand(IMovementCommand command)
	{
		command.Execute();
		commands.Push(command);
	}

	public void UndoCommand()
	{
		commands.Pop().Undo();
	}

	public void ResetCommands()
	{
		commands.Clear();
	}
}
