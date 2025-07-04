﻿using System;
using Godot;

namespace MetalWarriors.Objects.Characters.Pilot.States;

public class PilotIdleState(IPilotCharacter pilot) : BasePilotState(pilot)
{
    public override void Enter()
    {
        pilot.Console.Print($"Entering state: {GetType().Name}");
        
        pilot.PlayAnimation("idle");
    }
    
    public override Type? ProcessOrPass(double delta)
    {
        if (pilot.Controller.IsDPadLeftPressed || pilot.Controller.IsDPadRightPressed)
        {
            return typeof(PilotWalkingState);
        }
        
        return null;
    }
}
