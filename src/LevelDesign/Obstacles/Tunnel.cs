using Godot;
using System;

public class Tunnel : Node2D
{
    private Node2D middleNode2D;
    private Node2D exitNode2D;

    [Export]
    public int xtraMiddleSegments = 10;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.middleNode2D = GetNode<Node2D>("MiddleNode2D");
        this.exitNode2D = GetNode<Node2D>("ExitNode2D");

        Vector2 middleNode2DScaleV2 = this.middleNode2D.Scale;
        middleNode2DScaleV2.x = this.xtraMiddleSegments; // 10.0f;
        this.middleNode2D.Scale = middleNode2DScaleV2;

        Vector2 exitNode2DPositionV2 = this.exitNode2D.Position;
        exitNode2DPositionV2.x += (this.xtraMiddleSegments - 1) * 10.0f;//90.0f;
        this.exitNode2D.Position = exitNode2DPositionV2;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//
//  }
}
