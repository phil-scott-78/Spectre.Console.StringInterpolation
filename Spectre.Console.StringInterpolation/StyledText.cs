using System;
using Spectre.Console;

namespace StringInterpolation
{
    public static class StyledTextHelpers
    {
        public static StyledText Styled(object value, Style style) => new(value, style);
        public static StyledText Styled(object value, Color color) => new(value, new Style(color));
        public static StyledText Styled(object value, string text) => new(value, Style.Parse(text));

        public static RawText RawText(string text) => new(text);
    }

    public class RawText
    {
        public RawText(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }

    public class StyledText : IFormattable
    {
        public StyledText(object value, Style style)
        {
            Value = value;
            Style = style;
        }

        public object Value { get; }

        public Style Style { get; }

        // we'll never use this so just return an empty string
        public string ToString(string? format, IFormatProvider? formatProvider) => string.Empty;
    }
}