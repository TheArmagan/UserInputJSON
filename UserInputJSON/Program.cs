using System;
using System.Runtime.InteropServices;
using LowLevelInput.Hooks;
using Newtonsoft.Json.Linq;

namespace UserInputJSON
{
  class Program
  {

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

    [DllImport("User32.Dll")]
    public static extern long SetCursorPos(int x, int y);

    [Flags]
    public enum MouseEventFlags
    {
      LEFTDOWN = 0x00000002,
      LEFTUP = 0x00000004,
      MIDDLEDOWN = 0x00000020,
      MIDDLEUP = 0x00000040,
      MOVE = 0x00000001,
      ABSOLUTE = 0x00008000,
      RIGHTDOWN = 0x00000008,
      RIGHTUP = 0x00000010
    }

    [DllImport("user32.dll")]
    static extern uint keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

    [Flags]
    public enum KeybdEventFlags
    {
      EXTENDEDKEY = 0x0001,
      KEYUP = 0x0002,
      KEYDOWN = 0x0000
    }

    static void Main(string[] args)
    {

      InputManager inputManager = new InputManager(true);

      inputManager.OnKeyboardEvent += OnKeyboardEvent;
      inputManager.OnMouseEvent += OnMouseEvent;

      while (true)
      {
        string inputString = Console.ReadLine();
        try
        {
          JObject inputJSON = JObject.Parse(inputString);

          switch (inputJSON["event"]["base"].ToString())
          {
            case "mouse":
              {
                switch (inputJSON["event"]["type"].ToString())
                {
                  case "keydown":
                    {
                      if (
                        inputJSON["data"]["y"].Type != JTokenType.Undefined &&
                        inputJSON["data"]["x"].Type != JTokenType.Undefined
                        )
                      {
                        SetCursorPos(int.Parse(inputJSON["data"]["y"].ToString()), int.Parse(inputJSON["data"]["x"].ToString()));
                      }
                      
                      switch (inputJSON["data"]["key"].ToString())
                      {
                        case "1":
                          {
                            mouse_event((uint)MouseEventFlags.LEFTDOWN, 0, 0, 0, 0);
                            break;
                          }
                        case "2":
                          {
                            mouse_event((uint)MouseEventFlags.RIGHTDOWN, 0, 0, 0, 0);
                            break;
                          }
                        case "3":
                          {
                            mouse_event((uint)MouseEventFlags.MIDDLEDOWN, 0, 0, 0, 0);
                            break;
                          }
                      }

                      break;
                    }
                  case "keyup":
                    {
                      if (
                        inputJSON["data"]["y"].Type != JTokenType.Undefined &&
                        inputJSON["data"]["x"].Type != JTokenType.Undefined
                        )
                      {
                        SetCursorPos(int.Parse(inputJSON["data"]["y"].ToString()), int.Parse(inputJSON["data"]["x"].ToString()));
                      }

                      switch (inputJSON["data"]["key"].ToString())
                      {
                        case "1":
                          {
                            mouse_event((uint)MouseEventFlags.LEFTUP, 0, 0, 0, 0);
                            break;
                          }
                        case "2":
                          {
                            mouse_event((uint)MouseEventFlags.RIGHTUP, 0, 0, 0, 0);
                            break;
                          }
                        case "3":
                          {
                            mouse_event((uint)MouseEventFlags.MIDDLEUP, 0, 0, 0, 0);
                            break;
                          }
                      }
                      break;
                    }
                  case "move":
                    {
                      SetCursorPos(int.Parse(inputJSON["data"]["x"].ToString()), int.Parse(inputJSON["data"]["y"].ToString()));
                      break;
                    }
                }

                break;
              }

            case "keyboard":
              {
                byte keyCode = byte.Parse(inputJSON["data"]["key"].ToString());
                switch (inputJSON["event"]["type"].ToString())
                {
                  case "keydown":
                    {
                      keybd_event(keyCode, 0, (int)KeybdEventFlags.KEYDOWN, 0);
                      break;
                    }
                  case "keyup":
                    {
                      keybd_event(keyCode, 0, (int)KeybdEventFlags.KEYUP, 0);
                      break;
                    }
                  case "keypress":
                    {
                      keybd_event(keyCode, 0, (int)KeybdEventFlags.KEYDOWN, 0);
                      keybd_event(keyCode, 0, (int)KeybdEventFlags.KEYUP, 0);
                      break;
                    }
                }
                break;
              }
          }
        } catch { };
      }
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

      Console.WriteLine(@"{{""event"":{{""base"":""keyboard"",""type"":""{1}""}},""data"":{{""key"":{0}}}}}", keyCode, (state == KeyState.Down ? "keydown" : (state == KeyState.Up ? "keyup" : (state == KeyState.Pressed ? "keypress" : ""))));
    }
  }
}
