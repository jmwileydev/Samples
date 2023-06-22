using System.Text.RegularExpressions;

namespace MaterialDesignColorsToXamlBrushes;

/// <summary>
/// Program
/// </summary>
public partial class Program
{
    /// <summary>
    /// Simply prints the proper command usage
    /// </summary>
    static void PrintUsage()
    {
        Console.WriteLine("MaterialDesignColorsToXamlBrushes (-I|-Inputfile) <input file> [-O|-OutputFile] [output file]");
    }

    /// <summary>
    /// Skips past the header for the Kt file.
    /// </summary>
    /// <param name="sr"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    static int ReadKtHeader(StreamReader sr)
    {
        var packageLine = sr.ReadLine();
        if (String.IsNullOrEmpty(packageLine) ||
            packageLine != "package com.example.compose")
        {
            throw new Exception("Unexpected first line, expected the Kt package line");
        }

        var importLine = sr.ReadLine();
        if (String.IsNullOrEmpty(importLine) ||
            importLine != "import androidx.compose.ui.graphics.Color")
        {
            throw new Exception("Expected the androidx import statement as the second line");
        }

        return 2;
    }

    /// <summary>
    /// A record to hold the result of processing the CommandLine
    /// </summary>
    readonly record struct CommandLineArgs
    {
        public String InputFile { get; init; }
        public String? OutputFile { get; init; }
    }

    /// <summary>
    /// This routine will process the command line arguments and return the CommandLineArgs record
    /// if successful with the input file and the optional output file
    /// </summary>
    /// <param name="args">the command line parameters</param>
    /// <returns>CommandLineArgs if found or null if an error occurs</returns>
    static CommandLineArgs? ProcessArgs(String[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("\nError: At least an input file needs to be supplied.\n");
            PrintUsage();
            return null;
        }

        int curArg = 0;
        String? outputFile = null;
        String? inputFile = null;

        while (curArg < args.Length)
        {
            switch (args[curArg].ToLower())
            {
                case "-o":
                case "-outputfile":
                    if (++curArg == args.Length)
                    {
                        Console.WriteLine("\nError: outputfile argument not found\n");
                        PrintUsage();
                        return null;
                    }

                    outputFile = args[curArg++];
                    break;

                case "-i":
                case "-inputfile":
                    if (++curArg == args.Length)
                    {
                        Console.WriteLine("\nError: outputfile argument not found\n");
                        PrintUsage();
                        return null;
                    }

                    inputFile = args[curArg++];
                    break;

                default:
                    Console.WriteLine($"\nError: Unexpected command line argument {args[curArg]}");
                    PrintUsage();
                    return null;
            }
        }

        if (inputFile == null)
        {
            Console.WriteLine("\nError: An input file must be specified\n");
            return null;
        }

        return new CommandLineArgs
        {
            InputFile = inputFile!,
            OutputFile = outputFile
        };

    }

    /// <summary>
    /// Creates a stream writer for either the passed in file name or standard output
    /// </summary>
    /// <param name="outputFile">optional output file name</param>
    /// <returns>null or an opened StreamWriter if successful</returns>
    static StreamWriter? OpenOutputStream(String? outputFile)
    {
        StreamWriter? streamWriter;
        if (outputFile == null)
        {
            try
            {
                streamWriter = new StreamWriter(Console.OpenStandardOutput());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError trying to write to standard output: {ex.Message}\n");
                return null;
            }
        }
        else
        {
            try
            {
                streamWriter = new StreamWriter(outputFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError trying to write to {outputFile}: {ex.Message}\n");
                return null;
            }
        }

        return streamWriter;
    }

    // Generated regular expression for parsing a color line in the .kt file.
    [GeneratedRegex(@"[ ]*val md_theme_([^ ].*) = Color\(0x([0-9,A-F,a-f].*)\)", RegexOptions.None)]
    private static partial Regex ktColorLineRegex();

    static void WriteColorsToStream(StreamWriter streamWriter, List<(String name, int color)> namesAndColors)
    {
        streamWriter.WriteLine("<!-- Generated XAML colors and brushes from color.kt file. -->");
        streamWriter.WriteLine(@"<ResourceDictionary xmlns = ""http://schemas.microsoft.com/winfx/2006/xaml/presentation""");
        streamWriter.WriteLine(@"                    xmlns:x = ""http://schemas.microsoft.com/winfx/2006/xaml"" >");
        streamWriter.WriteLine("\n<!-- Colors -->");
        foreach (var (name, color) in namesAndColors)
        {
            streamWriter.WriteLine($"<Color x:Key=\"{name}\">#{color:X}</Color>");
        }

        streamWriter.WriteLine("\n<!-- Brushes -->");
        foreach (var (name, _) in namesAndColors)
        {
            streamWriter.WriteLine($"<SolidColorBrush x:Key=\"{name}Brush\" Color=\"{{StaticResource {name}}}\"></SolidColorBrush>");
        }

        streamWriter.WriteLine("</ResourceDictionary>");
    }

    static void Main(string[] args)
    {
        int curLine = 0;

        var commandLineArgs = ProcessArgs(args);
        if (commandLineArgs == null)
        {
            return;
        }

        // Now begin to parse the file. We can read this file line by line
        List<(String name, int color)> namesAndColors = new();
        try
        {
            using var sr = new StreamReader(commandLineArgs.Value.InputFile);
            curLine = ReadKtHeader(sr);
            String? line;
            while ((line = sr.ReadLine()) != null)
            {
                curLine++;
                if (line.Length > 0 && !line.Contains("val seed"))
                {
                    var match = ktColorLineRegex().Match(line);
                    if (!match.Success || match.Groups.Count != 3)
                    {
                        throw new Exception("Unexpected format for a color line");
                    }

                    namesAndColors.Add((match.Groups[1].Value, Convert.ToInt32(match.Groups[2].Value, 16)));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing input file Line {curLine}\nException: {ex.Message}");
            return;
        }

        if (namesAndColors.Count == 0)
        {
            Console.WriteLine($"Error: No colors found in the file {commandLineArgs.Value.InputFile}");
            return;
        }

        // If I got here then I was able to parse the file properly, let's spit out the colors
        StreamWriter? streamWriter = OpenOutputStream(commandLineArgs.Value.OutputFile);
        if (streamWriter == null)
        {
            return;
        }

        using (streamWriter)
        {
            WriteColorsToStream(streamWriter, namesAndColors);
        }

        if (commandLineArgs.Value.OutputFile != null)
        {
            Console.WriteLine($"\nSuccessfully wrote colors to {commandLineArgs.Value.OutputFile}\n");
        }
    }
}
