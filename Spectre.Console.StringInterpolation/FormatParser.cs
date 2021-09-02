using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Spectre.Console;

namespace StringInterpolation
{
    internal static class FormatParser
    {
        private static readonly MarkupFormatProvider Provider = new();

        public static string Parse(FormattableString text) => text.ToString(Provider);

        private class MarkupFormatProvider : IFormatProvider
        {
            private static readonly MarkupFormatter MarkupFormatter = new();

            public object GetFormat(Type? formatType) => MarkupFormatter;
        }

        private class MarkupFormatter : ICustomFormatter
        {
            public string Format(string? format, object? arg, IFormatProvider? formatProvider)
            {
                object? inputValue;
                Style? style;

                switch (arg)
                {
                    case ITuple { Length: 2 } tuple:
                        (inputValue, style) = ParseStyleTuple(tuple);
                        break;
                    case StyledText styledText:
                        inputValue = styledText.Value;
                        style = styledText.Style;
                        break;
                    default:
                        inputValue = arg;
                        style = null;
                        break;
                }

                if (inputValue == null)
                {
                    return string.Empty;
                }

                string formattedValue = inputValue switch
                {
                    RawText rawText => rawText.Text,
                    IFormattable formattable => formattable.ToString(format, CultureInfo.InvariantCulture).EscapeMarkup(),
                    _ => inputValue.ToString().EscapeMarkup()
                };

                return style == null ?
                    formattedValue :
                    $"[{style.ToMarkup()}]{formattedValue}[/]";
            }

            private static (object? inputValue, Style? style) ParseStyleTuple(ITuple tuple)
            {
                var inputValue = tuple[0];
                var style = tuple[1] switch
                {
                    null => null,
                    Style styleValue => styleValue,
                    Color styleColor => new Style(styleColor),
                    string styleString => Style.Parse(styleString),
                    _ => null,
                };
                return (inputValue, style);
            }
        }
    }
}