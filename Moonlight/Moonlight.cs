using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MoonTools.ECS;
using Moonlight.Systems;
using Moonlight.Components;
using Moonlight.Renderers;
using FontStashSharp;
using Moonlight.Content;

namespace Moonlight;

public class Moonlight : Game
{
    public static readonly int RenderWidth = 640;
    public static readonly int RenderHeight = 480;
    public static readonly float AspectRatio = (float)RenderWidth / RenderHeight;
    public static RenderTarget2D RenderTarget;

    GraphicsDeviceManager GraphicsDeviceManager { get; }

    /*
    the World is the place where all our entities go.
    */
    World World { get; } = new World();

    Input Input;
    SpriteRenderer SpriteRenderer;

    SpriteBatch SpriteBatch;

    [STAThread]
    internal static void Main()
    {
        using (Moonlight game = new Moonlight())
        {
            game.Run();
        }
    }
    private Moonlight()
    {
        //setup our graphics device, default window size, etc
        //here is where i will make a plea to you, intrepid game developer:
        //please default your game to windowed mode.
        GraphicsDeviceManager = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";

        GraphicsDeviceManager.PreferredBackBufferWidth = 1280;
        GraphicsDeviceManager.PreferredBackBufferHeight = 720;
        GraphicsDeviceManager.SynchronizeWithVerticalRetrace = true;

        IsFixedTimeStep = false;
        IsMouseVisible = true;
    }

    //you'll want to do most setup in LoadContent() rather than your constructor.
    protected override void LoadContent()
    {
        /*
        CONTENT
        */

        RenderTarget = new RenderTarget2D(GraphicsDevice, RenderWidth, RenderHeight);

        /*
        SpriteBatch is FNA/XNA's abstraction for drawing sprites on the screen.
        you want to do is send all the sprites to the GPU at once, 
        it's much more efficient to send one huge batch than to send sprites piecemeal. 
        See more in the Renderers/ExampleRenderer.cs. 
        */
        SpriteBatch = new(GraphicsDevice);

        AllContent.Initialize(Content);
        /*
        SYSTEMS
        */

        /*
        here we set up all our systems. 
        you can pass in information that these systems might need to their constructors.
        it doesn't matter what order you create the systems in, we'll specify in what order they run later.
        */
        Input = new(World);

        /*
        RENDERERS
        */

        //same as above, but for the renderer
        SpriteRenderer = new SpriteRenderer(World, SpriteBatch);

        /*
        ENTITIES
        */

        var player = World.CreateEntity();
        World.Set(player, new Sprite(Textures.Player, 0.0f));
        World.Set(player, new Position(new Vector2(RenderWidth * 0.5f, RenderHeight * 0.5f)));

        base.LoadContent();
    }

    //sometimes content needs to be unloaded, but it usually doesn't.
    protected override void UnloadContent()
    {
        base.UnloadContent();
    }


    protected override void Update(GameTime gameTime)
    {
        /*
        here we call all our system update functions. 
        call them in the order you want them to run. 
        other ECS libraries have a master "update" function that does this for you,
        but moontools.ecs does not. this lets you have more control
        over the order systems run in, and whether they run at all.
        */
        Input.Update(gameTime.ElapsedGameTime); //always update this before anything that takes inputs
        //ExampleSystem.Update(gameTime.ElapsedGameTime);
        World.FinishUpdate(); //always call this at the end of your update function.
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(RenderTarget);

        GraphicsDevice.Clear(Color.CornflowerBlue);
        SpriteRenderer.Draw();

        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.Black);
        SpriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.Opaque,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise
            );

        var height = Window.ClientBounds.Height;
        // height -= (height % RenderHeight); // uncomment for integer scaling
        var width = (int)MathF.Floor(height * AspectRatio);
        var wDiff = Window.ClientBounds.Width - width;
        var hDiff = Window.ClientBounds.Height - height;

        SpriteBatch.Draw(
            RenderTarget,
            new Rectangle(
                (int)MathF.Floor(wDiff * 0.5f),
                (int)MathF.Floor(hDiff * 0.5f),
                width,
                height),
            null,
            Color.White
        );

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
