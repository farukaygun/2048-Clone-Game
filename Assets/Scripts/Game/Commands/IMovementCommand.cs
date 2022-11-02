using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementCommand
{
	void Execute();
	void Undo();
}
