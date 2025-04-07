using Godot;
using MetalWarriors.Objects.Characters.Nitro;
using MetalWarriorsTests.Utils;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MetalWarriorsTests.Objects.Characters.Nitro;

public class NitroCharacterHandlerTest(ITestOutputHelper testOutputHelper)
{
    private readonly TestOutputConsolePrinter _consolePrinter = new(testOutputHelper);
    
    [Fact]
    public void Nitro_should_rise_constantly_when_launching()
    {
        var nitro = new NitroCharacterImpl();
        nitro.SetIsOnFloor(true);

        var snesController = new SnesControllerImpl(isButtonBPressed: true);
        var nitroCharacterHandler = new NitroCharacterHandler(snesController, nitro, _consolePrinter);
        
        // Act
        nitroCharacterHandler.PhysicsProcess(0.1f);
    
        // Assert
        nitro.Velocity.X.ShouldBe(0);
        nitro.Velocity.Y.ShouldBe(NitroDefaults.MaxRisingVelocity);
        nitro.Direction.ShouldBe(NitroDirection.Right);
        nitro.CurrentAnimation.ShouldBe("launching");
        nitroCharacterHandler.NitroState.ShouldBe(NitroState.Launching);
    }

    [Fact]
    public void Nitro_should_keep_rising_constantly_when_launching()
    {
        var nitro = new NitroCharacterImpl();
        nitro.Velocity = new Vector2(0, NitroDefaults.MaxRisingVelocity);
        nitro.SetIsOnFloor(false);

        var snesController = new SnesControllerImpl(isButtonBPressed: true);
        var nitroCharacterHandler = new NitroCharacterHandler(snesController, nitro, _consolePrinter);
        
        // Act
        nitroCharacterHandler.PhysicsProcess(0.1f);
    
        // Assert
        nitro.Velocity.X.ShouldBe(0);
        nitro.Velocity.Y.ShouldBe(NitroDefaults.MaxRisingVelocity);
        nitro.Direction.ShouldBe(NitroDirection.Right);
        nitro.CurrentAnimation.ShouldBe("launching");
        nitro.PlayedAnimations.ShouldBe(["launching"]);
    }

    [Fact]
    public void Nitro_should_change_from_launching_to_jetting_after_the_launching_animation_is_finished()
    {
        var nitro = new NitroCharacterImpl();
        nitro.Velocity = new Vector2(0, NitroDefaults.MaxRisingVelocity);
        nitro.SetIsOnFloor(false);

        var snesController = new SnesControllerImpl(isButtonBPressed: true);
        var nitroCharacterHandler = new NitroCharacterHandler(snesController, nitro, _consolePrinter);
        
        // Act
        nitroCharacterHandler.LaunchingAnimationFinished();
        nitroCharacterHandler.PhysicsProcess(0.1f);
    
        // Assert
        nitro.Velocity.X.ShouldBe(0);
        nitro.Velocity.Y.ShouldBe(NitroDefaults.MaxRisingVelocity);
        nitro.Direction.ShouldBe(NitroDirection.Right);
        nitro.CurrentAnimation.ShouldBe("flying");
        nitro.PlayedAnimations.ShouldBe(["flying"]);
    }

    [Fact]
    public void Nitro_should_decelerate_when_jetting_is_stopped()
    {
        // Arrange
        var nitro = new NitroCharacterImpl
        {
            Velocity = new Vector2(0, NitroDefaults.MaxRisingVelocity)
        };

        var snesController = new SnesControllerImpl();
        var nitroCharacterHandler = new NitroCharacterHandler(snesController, nitro, _consolePrinter);
        
        // Act
        nitroCharacterHandler.PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.ShouldBe(0);
        nitro.Velocity.Y.ShouldBeLessThan(0);
        nitro.Velocity.Y.ShouldBeGreaterThan(NitroDefaults.MaxRisingVelocity);
    }
    
    [Fact]
    public void Nitro_should_accelerate_when_jetting_is_started_again()
    {
        // Arrange
        var nitro = new NitroCharacterImpl
        {
            Velocity = new Vector2(0, NitroDefaults.MaxFallingVelocity)
        };
        
        var snesController = new SnesControllerImpl(isButtonBPressed: true);
        var nitroCharacterHandler = new NitroCharacterHandler(snesController, nitro, _consolePrinter);
        
        // Act
        nitroCharacterHandler.PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.ShouldBe(0);
        nitro.Velocity.Y.ShouldBeGreaterThan(0);
        nitro.Velocity.Y.ShouldBeLessThan(NitroDefaults.MaxFallingVelocity);
    }
    
    [Fact]
    public void Nitro_should_fall_when_no_longer_decelerating()
    {
        // Arrange
        var nitro = new NitroCharacterImpl
        {
            Velocity = new Vector2(0, 0)
        };
        
        var snesController = new SnesControllerImpl();
        var nitroCharacterHandler = new NitroCharacterHandler(snesController, nitro, _consolePrinter);
        
        // Act
        nitroCharacterHandler.PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.ShouldBe(0);
        nitro.Velocity.Y.ShouldBeGreaterThan(0);
        nitro.CurrentAnimation.ShouldBe("falling");
    }
    
    [Fact]
    public void Nitro_should_not_fall_too_fast()
    {
        // Arrange
        var nitro = new NitroCharacterImpl
        {
            Velocity = new Vector2(0, NitroDefaults.MaxFallingVelocity + 10)
        };
        
        var snesController = new SnesControllerImpl();
        var nitroCharacterHandler = new NitroCharacterHandler(snesController, nitro, _consolePrinter);
        
        // Act
        nitroCharacterHandler.PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.ShouldBe(0);
        nitro.Velocity.Y.ShouldBe(NitroDefaults.MaxFallingVelocity);
    }
    
    [Fact]
    public void Nitro_should_not_accelerate_too_fast()
    {
        // Arrange
        var nitro = new NitroCharacterImpl
        {
            Velocity = new Vector2(0, NitroDefaults.MaxRisingVelocity + 10)
        };
        
        var snesController = new SnesControllerImpl(isButtonBPressed: true);
        var nitroCharacterHandler = new NitroCharacterHandler(snesController, nitro, _consolePrinter);
        
        // Act
        nitroCharacterHandler.PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.ShouldBe(0);
        nitro.Velocity.Y.ShouldBe(NitroDefaults.MaxRisingVelocity);
    }
    
    [Fact]
    public void Nitro_should_not_exceed_max_rising_velocity()
    {
        // Arrange
        var nitro = new NitroCharacterImpl
        {
            Velocity = new Vector2(0, NitroDefaults.MaxRisingVelocity - 10)
        };
        
        var snesController = new SnesControllerImpl(isButtonBPressed: true);
        var nitroCharacterHandler = new NitroCharacterHandler(snesController, nitro, _consolePrinter);
        
        // Act
        nitroCharacterHandler.PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.ShouldBe(0);
        nitro.Velocity.Y.ShouldBe(NitroDefaults.MaxRisingVelocity);
    }
    
    [Fact]
    public void Nitro_should_not_go_farther_down_if_already_on_the_floor()
    {
        // Arrange
        var nitro = new NitroCharacterImpl
        {
            Velocity = new Vector2(0, 0)
        };
        
        nitro.SetIsOnFloor(true);
        
        var snesController = new SnesControllerImpl();
        var nitroCharacterHandler = new NitroCharacterHandler(snesController, nitro, _consolePrinter);
        
        // Act
        nitroCharacterHandler.PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.ShouldBe(0);
        nitro.Velocity.Y.ShouldBe(0, customMessage: "he can't go down more if he's on the floor");
    }
    
    [Fact]
    public void Nitro_should_move_left_when_left_D_Pad_is_pressed()
    {
        // Arrange
        var nitro = new NitroCharacterImpl
        {
            Velocity = new Vector2(0, 0)
        };
        
        nitro.SetIsOnFloor(true);
        
        var snesController = new SnesControllerImpl(isDPadLeftPressed: true);
        var nitroCharacterHandler = new NitroCharacterHandler(snesController, nitro, _consolePrinter);
        
        // Act
        nitroCharacterHandler.PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.ShouldBe(-NitroDefaults.MovementSpeed);
        nitro.Velocity.Y.ShouldBe(0, customMessage: "he's on the floor");
        nitro.Direction.ShouldBe(NitroDirection.Left, customMessage: "he should be moving left");
        nitro.CurrentAnimation.ShouldBe("walking");
    }
    
    [Fact]
    public void Nitro_should_stop_moving_when_left_D_Pad_is_not_pressed()
    {
        // Arrange
        var nitro = new NitroCharacterImpl
        {
            Velocity = new Vector2(-NitroDefaults.MovementSpeed, 0)
        };
        
        nitro.SetIsOnFloor(true);
        
        var snesController = new SnesControllerImpl();
        var nitroCharacterHandler = new NitroCharacterHandler(snesController, nitro, _consolePrinter);
        
        // Act
        nitroCharacterHandler.PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.ShouldBe(0, customMessage: "the left D_Pad is not being pressed");
        nitro.Velocity.Y.ShouldBe(0, customMessage: "he's on the floor");
        nitro.CurrentAnimation.ShouldBe("idle");
    }
    
    [Fact]
    public void Nitro_should_move_right_when_right_D_Pad_is_pressed()
    {
        // Arrange
        var nitro = new NitroCharacterImpl
        {
            Velocity = new Vector2(0, 0)
        };
        
        nitro.SetIsOnFloor(true);
        
        var snesController = new SnesControllerImpl(isDPadRightPressed: true);
        var nitroCharacterHandler = new NitroCharacterHandler(snesController, nitro, _consolePrinter);
        
        // Act
        nitroCharacterHandler.PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.ShouldBe(NitroDefaults.MovementSpeed);
        nitro.Velocity.Y.ShouldBe(0, customMessage: "he's on the floor");
        nitro.Direction.ShouldBe(NitroDirection.Right, customMessage: "he should be moving right");
    
        nitro.CurrentAnimation.ShouldBe("walking");
    }
    
    [Fact]
    public void Nitro_should_stop_moving_when_right_D_Pad_is_not_pressed()
    {
        // Arrange
        var nitro = new NitroCharacterImpl
        {
            Velocity = new Vector2(NitroDefaults.MovementSpeed, 0)
        };
        
        nitro.SetIsOnFloor(true);
        
        var snesController = new SnesControllerImpl();
        var nitroCharacterHandler = new NitroCharacterHandler(snesController, nitro, _consolePrinter);
        
        // Act
        nitroCharacterHandler.PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.ShouldBe(0, customMessage: "the right D_Pad is not being pressed");
        nitro.Velocity.Y.ShouldBe(0, customMessage: "he's on the floor");
    }
}
