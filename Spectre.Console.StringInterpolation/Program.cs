using System;
using System.Text;
using Spectre.Console;
using StringInterpolation;
using static StringInterpolation.StyledTextHelpers;

Console.OutputEncoding = Encoding.UTF8;

var name = "Phil Scott";
var date = new DateTime(1978, 12, 29);
var color = Color.Blue;
var style = new Style(Color.Green);
var data = "[[[]]";

// automatically escape the parameters.
MarkupFormatLine($"No worries about weird data like {data} causing markup issues at runtime");

// but wait, there is more! we can also use strongly typed colors and styles.

//  tuples with strings
MarkupFormatLine($"On {(date, "blue"):MMMM dd, yyyy}, {(name, "green")} was born.");

// tuples with objects for styling
MarkupFormatLine($"On {(date, color):MMMM dd, yyyy}, {(name, style)} was born.");

// Styled text with strings. No boxing and IDEs like the formattable values better.
MarkupFormatLine($"On {Styled(date, "blue"):MMMM dd, yyyy}, {Styled(name, "green")} was born.");

// Styled text with objects for styling
MarkupFormatLine($"On {Styled(date, color):MMMM dd, yyyy}, {Styled(name, style)} was born.");

// mixing interpolated string styling with class markdown
MarkupFormatLine($"[purple]On {Styled(date, "blue"):MMMM dd, yyyy}, [link=http://thirty25.com]{Styled(name, "green")}[/] was born.[/]");

// REALLY want to use markup within parameters? Declare it as rawtext and it won't be escaped.
var colorText = RawText("[blue]");
MarkupFormatLine($"On {colorText}{date:MMMM dd, yyyy}[/], {Styled(name, style)} was born.");

static void MarkupFormatLine(FormattableString input)
{
    AnsiConsole.MarkupLine(FormatParser.Parse(input));
}
