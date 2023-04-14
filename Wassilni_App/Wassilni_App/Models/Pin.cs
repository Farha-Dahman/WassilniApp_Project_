using System;
using Wassilni_App.views;
using Xamarin.Forms.Maps;

public class DurationPin : Pin
{
    public DurationPinView DurationView { get; }

    public DurationPin(TimeSpan duration)
    {
        DurationView = new DurationPinView(duration);
    }
}