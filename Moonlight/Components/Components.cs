using Microsoft.Xna.Framework;

namespace Moonlight.Components;

public readonly record struct Sprite(Rectangle Rect, float Depth);
public readonly record struct Position(Vector2 Value);
public readonly record struct Rotation(float Value);
public readonly record struct Scale(Vector2 Value);