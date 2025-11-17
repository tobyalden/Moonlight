using System;
using System.IO;
using System.Reflection;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Moonlight.Content;
public static class Textures
{
    public static Texture2D Sprites { get; private set; }
    public static readonly Rectangle Enemy = new Rectangle(0, 0, 100, 100);
    public static readonly Rectangle Player = new Rectangle(100, 0, 100, 100);
    public static void Initialize(ContentManager content)
    {
        Sprites = content.Load<Texture2D>("Textures/sprites");
    }
}
public static class Fonts
{
    public static FontSystem Opensans { get; private set; }
    public static void Initialize(ContentManager content)
    {
        Opensans = new FontSystem();
        Opensans.AddFont(File.ReadAllBytes(
            System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), content.RootDirectory, @"Fonts/opensans.ttf"
            )
        ));
    }
}
public static class SFX
{
    public static void Initialize(ContentManager content)
    {
    }
}
public static class Songs
{
    public static void Initialize(ContentManager content)
    {
    }
}

public static class AllContent
{
    public static void Initialize(ContentManager content)
    {
        Textures.Initialize(content);
        Fonts.Initialize(content);
        SFX.Initialize(content);
        Songs.Initialize(content);
    }
}
