using System;
using LowLevelInput.Hooks;

namespace UserInputJSON
{
    class Program
    {
        static void Main(string[] args)
        {

            InputManager inputManager = new InputManager(true, true);

            inputManager.OnKeyboardEvent += OnKeyboardEvent;
            inputManager.OnMouseEvent += OnMouseEvent;

            Console.ReadLine();
        }

        static void OnMouseEvent(VirtualKeyCode key, KeyState state, int x, int y)
        {
            string keyCode = ((int)key).ToString();
            string xString = x.ToString();
            string yString = y.ToString();

            Console.WriteLine(@"{{""event"":{{""base"":""mouse"",""type"":""{3}""}},""data"":{{""x"":{0},""y"":{1},""key"":{2}}}}}", xString, yString, keyCode, (state == KeyState.Down ? "keydown" : (state == KeyState.Up ? "keyup" : (state == KeyState.Pressed ? "keypress" : "move"))));
        }

        static void OnKeyboardEvent(VirtualKeyCode key, KeyState state)
        {
            string keyCode = ((int)key).ToString();

            Console.WriteLine(@"{{""event"":{{""base"":""keyboard"",""type"":""{1}""}},""key"":{0}}}", keyCode, (state == KeyState.Down ? "keydown" : (state == KeyState.Up ? "keyup" : (state == KeyState.Pressed ? "keypress" : ""))));
        }
    }
}
