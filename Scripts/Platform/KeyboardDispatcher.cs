﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;

public interface IKeyboardSubscriber
{
    void RecieveTextInput(char inputChar);
    void RecieveTextInput(string text);
    void RecieveCommandInput(char command);
    void RecieveSpecialInput(Keys key);

    bool Selected { get; set; } //or Focused
}

public class KeyboardDispatcher
{
    public KeyboardDispatcher(IntPtr windowHandle)
    {
        var window = EventInput.EventInput.AddWindow(windowHandle);
        window.CharEntered += new EventInput.CharEnteredHandler(EventInput_CharEntered);
        window.KeyDown += new EventInput.KeyEventHandler(EventInput_KeyDown);
    }

    void EventInput_KeyDown(object sender, EventInput.KeyEventArgs e)
    {
        if (_subscriber == null)
            return;

        _subscriber.RecieveSpecialInput(e.KeyCode);
    }

    void EventInput_CharEntered(object sender, EventInput.CharacterEventArgs e)
    {
        if (_subscriber == null)
            return;
        if (char.IsControl(e.Character))
        {
            //ctrl-v
            if (e.Character == 0x16)
            {
                //XNA runs in Multiple Thread Apartment state, which cannot recieve clipboard
                Thread thread = new Thread(PasteThread);
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
                _subscriber.RecieveTextInput(_pasteResult);
            }
            else
            {
                _subscriber.RecieveCommandInput(e.Character);
            }
        }
        else
        {
            _subscriber.RecieveTextInput(e.Character);
        }
    }

    IKeyboardSubscriber _subscriber;
    public IKeyboardSubscriber Subscriber
    {
        get { return _subscriber; }
        set
        {
            if (_subscriber != null)
                _subscriber.Selected = false;
            _subscriber = value;
            if (value != null)
                value.Selected = true;
        }
    }

    //Thread has to be in Single Thread Apartment state in order to receive clipboard
    string _pasteResult = "";
    [STAThread]
    void PasteThread()
    {
        if (System.Windows.Forms.Clipboard.ContainsText())
        {
            _pasteResult = System.Windows.Forms.Clipboard.GetText();
        }
        else
        {
            _pasteResult = "";
        }
    }
}