﻿using System;

public interface IInteractable
{
    void Interact();

    void Interact(Action callback);
}
