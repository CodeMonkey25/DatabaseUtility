using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;

namespace DatabaseUtility.Views;

public class IconButtonLabel : TemplatedControl
{
    #region Properties
    public static readonly StyledProperty<string> LabelTextProperty = AvaloniaProperty.Register<IconButtonLabel, string>(nameof(LabelText));
    public string LabelText
    {
        get => GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }

    public static readonly StyledProperty<string> IconUnicodeProperty = AvaloniaProperty.Register<IconButtonLabel, string>(nameof(IconUnicode));
    public string IconUnicode
    {
        get => GetValue(IconUnicodeProperty);
        set => SetValue(IconUnicodeProperty, value);
    }

    public static readonly StyledProperty<ICommand?> CommandProperty = AvaloniaProperty.Register<IconButtonLabel, ICommand?>(nameof(Command));
    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly StyledProperty<HorizontalAlignment> MyHorizontalAlignmentProperty = AvaloniaProperty.Register<IconButtonLabel, HorizontalAlignment>(nameof(MyHorizontalAlignment));
    public HorizontalAlignment MyHorizontalAlignment
    {
        get => GetValue(MyHorizontalAlignmentProperty);
        set => SetValue(MyHorizontalAlignmentProperty, value);
    }

    public static readonly StyledProperty<bool> LabelIsVisibleProperty = AvaloniaProperty.Register<IconButtonLabel, bool>(nameof(LabelIsVisible), true);
    public bool LabelIsVisible
    {
        get => GetValue(LabelIsVisibleProperty);
        set => SetValue(LabelIsVisibleProperty, value);
    }
    #endregion
    
}