using Bunit;
using BasketballScoreboard.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System.Linq;

namespace BasketballScoreboard.Tests;

public class BuzzerTests : TestContext
{
    [Fact]
    public void Buzzer_InitialState_ShouldRenderAudioElement()
    {
        // Arrange & Act
        var component = RenderComponent<Buzzer>();

        // Assert
        Assert.Contains("<audio", component.Markup);
        Assert.Contains("id=\"buzzer\"", component.Markup);
        Assert.Contains("buzzer.wav", component.Markup);
    }

    [Fact]
    public void Buzzer_ShouldHaveCorrectAudioSource()
    {
        // Arrange & Act
        var component = RenderComponent<Buzzer>();

        // Assert
        var audio = component.Find("audio");
        Assert.Equal("buzzer", audio.GetAttribute("id"));
        
        var source = component.Find("source");
        Assert.Equal("/buzzer.wav", source.GetAttribute("src"));
    }

    [Fact]
    public async Task Buzzer_Play_ShouldCallJavaScriptInterop()
    {
        // Arrange
        // Setup the JavaScript interop mock
        JSInterop.SetupVoid("playAudio");
        
        var component = RenderComponent<Buzzer>();

        // Act
        await component.Instance.Play();
        
        // Assert
        // Verify that the JavaScript function was called
        JSInterop.VerifyInvoke("playAudio");
    }

    [Fact]
    public void Buzzer_AudioElement_ShouldNotHaveControlsAttribute()
    {
        // Arrange & Act
        var component = RenderComponent<Buzzer>();

        // Assert
        var audio = component.Find("audio");
        
        // Should not have controls attribute (headless audio for buzzer sounds)
        Assert.Null(audio.GetAttribute("controls"));
    }

    [Fact]
    public void Buzzer_SourceElement_ShouldBeWavFile()
    {
        // Arrange & Act
        var component = RenderComponent<Buzzer>();

        // Assert
        var source = component.Find("source");
        var src = source.GetAttribute("src");
        
        Assert.NotNull(src);
        Assert.EndsWith(".wav", src);
        Assert.StartsWith("/", src); // Should be absolute path
    }
}