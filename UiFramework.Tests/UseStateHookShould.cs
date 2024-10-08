﻿using UiFramework.Tests.Helpers;
using UiFramework.WPF;
using static UiFramework.Framework;
using static UiFramework.WPF.Elements;

namespace UiFramework.Tests;

public class UseStateHookShould
{
    private readonly TestAppViewModel _testAppViewModel;
    private readonly RootController _root;

    public UseStateHookShould()
    {
        _testAppViewModel = new();
        _root = CreateRoot(_testAppViewModel, nameof(_testAppViewModel.Content));
    }

    private void Render(Element element) => _root.Render(element);
    private T Root<T>() => _testAppViewModel.Content.As<T>()!;

    [Fact]
    public void Persist_State_Between_Rerenders()
    {
        Render(CreateElement(StateComponent));

        Root<ContainerViewModel>().Children[0].As<TextViewModel>().Text.Should().Be("initial text");
        Root<ContainerViewModel>().Children[1].As<ButtonViewModel>().Click.Execute(null);
        Root<ContainerViewModel>().Children[0].As<TextViewModel>().Text.Should().Be("button clicked");
        return;

        Element StateComponent(IDictionary<string, object?>? props, params Element?[] elementFactories)
        {
            var (text, setText) = UseState("initial text");

            return CreateElement(Container, null,
                CreateElement(Text, new { text }),
                CreateElement(Button, new { onClick = new Action(() => setText("button clicked")) })
            );
        }
    }

    [Fact]
    public void Restore_Component_State_On_Render()
    {
        Render(CreateElement(RootComponent));

        Root<ContainerViewModel>().Children[^1].As<ButtonViewModel>().Click.Execute(null);
        Root<ContainerViewModel>().Children[0].As<TextViewModel>().Text.Should().Be("B");
        return;

        Element ItemComponent(IDictionary<string, object?>? props, params Element?[] elementFactories)
        {
            var (state, _) = UseState((string)props?["name"]!);
            return CreateElement(Text, new { text = state });
        }

        Element RootComponent(dynamic? props, params Element?[] elementFactories)
        {
            var (showA, setShowA) = UseState(true);

            return CreateElement(Container, null,
                showA ? CreateElement(ItemComponent, new { name = "A" }) : null,
                CreateElement(ItemComponent, new { name = "B" }),
                CreateElement(Button, new { onClick = new Action(() => setShowA(false)) })
            );
        }
    }

    [Fact]
    public void Clear_State_For_Unmounted_Components()
    {
        Render(CreateElement(RootComponent));

        var initialText = Root<ContainerViewModel>().Children[0].As<TextViewModel>().Text;
        Root<ContainerViewModel>().Children[^1].As<ButtonViewModel>().Click.Execute(null);
        Root<ContainerViewModel>().Children[^1].As<ButtonViewModel>().Click.Execute(null);
        Root<ContainerViewModel>().Children[0].As<TextViewModel>().Text.Should().NotBe(initialText);
        return;

        Element ItemComponent(IDictionary<string, object?> props, params Element?[] elementFactories)
        {
            var (state, _) = UseState(Guid.NewGuid().ToString());
            return CreateElement(Text, new { text = state });
        }

        Element RootComponent(dynamic? props, params Element?[] elementFactories)
        {
            var (show, setShow) = UseState(true);

            return CreateElement(Container, null,
                show ? CreateElement(ItemComponent) : null,
                CreateElement(Button, new { onClick = new Action(() => setShow(!show)) })
            );
        }
    }

    [Fact]
    public void Manage_Multiple_States_For_Single_Component()
    {
        Render(CreateElement(RootComponent));

        Root<ContainerViewModel>().Children[2].As<ButtonViewModel>().Click.Execute(null);
        Root<ContainerViewModel>().Children[0].As<TextViewModel>().Text.Should().Be("state 1 updated");
        Root<ContainerViewModel>().Children[1].As<TextViewModel>().Text.Should().Be("state 2");

        Root<ContainerViewModel>().Children[3].As<ButtonViewModel>().Click.Execute(null);
        Root<ContainerViewModel>().Children[0].As<TextViewModel>().Text.Should().Be("state 1 updated");
        Root<ContainerViewModel>().Children[1].As<TextViewModel>().Text.Should().Be("state 2 updated");

        return;
        Element RootComponent(dynamic? props, params Element?[] elementFactories)
        {
            var (state1, setState1) = UseState("state 1");
            var (state2, setState2) = UseState("state 2");

            return CreateElement(Container, null,
                CreateElement(Text, new { text = state1 }),
                CreateElement(Text, new { text = state2 }),
                CreateElement(Button, new { onClick = new Action(() => setState1("state 1 updated"))  }),
                CreateElement(Button, new { onClick = new Action(() => setState2("state 2 updated"))  })
            );
        }
    }
}
