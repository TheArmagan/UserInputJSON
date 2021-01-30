using System;
using LowLevelInput.Hooks;

namespace UserInputJSON
{
    class Program
    {
        static void Main(string[] args)
        {
            InputManager inputManager = new InputManager();

            inputManager.OnKeyboardEvent += OnKeyboardEvent;
            inputManager.OnMouseEvent += OnMouseEvent;

            inputManager.Initialize();
            Console.ReadLine();
        }

        static void OnMouseEvent(VirtualKeyCode key, KeyState state, int x, int y)
        {
            if (key == VirtualKeyCode.Invalid)
            {
                Console.WriteLine(@"{{""event"": ""mouseMove"", ""xPosition"": {0}, ""yPosition"": {1}}}", x.ToString(), y.ToString());
            } else
            {
                Console.WriteLine(@"{{""event"": ""mouseStateChange"", ""keyName"": ""{0}"", ""keyCode"": {1}, ""keyState"": ""{2}"", ""keyStateCode"": {3}, ""xPosition"": {4}, ""yPosition"": {5}}}", key.ToString(), ((int)key).ToString(), state.ToString(), ((int)state).ToString(), x.ToString(), y.ToString());
            }
            
        }

        static void OnKeyboardEvent(VirtualKeyCode key, KeyState state)
        {
            Console.WriteLine(@"{{""event"": ""keyboardStateChange"", ""keyName"": ""{0}"", ""keyCode"": {1}, ""keyState"": ""{2}"", ""keyStateCode"": {3}}}", key.ToString(), ((int)key).ToString(), state.ToString(), ((int)state).ToString());
        }
    }
}
