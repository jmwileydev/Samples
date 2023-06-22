<h1>MaterialDesignColorsToXamlBrushes</h1>
This is a simply little program which allows me to take the exported colors from MaterialDesign's Theme Builder
Web site and turn it into a set of colors and brushes for WPF and XAML. The program takes input of the form:<br>

    val md_theme_light_primary = Color(0xFF4D54B7)

And will create a color and a brush for it.<br>

    <Color x:Key="light_primary">#FF4D54B7</Color>
    <SolidColorBrush x:Key="light_primaryBrush" Color="{StaticResource light_primary}"></SolidColorBrush>

<br>

<h2>Usage</h2>

    MaterialDesignColorsToXamlBrushes (-I|-Inputfile) <input file> [-O|-OutputFile] [output file]

If the colors.kt file is parsed properly the Colors.xaml by default will be written to standard out.
For example

<h2>Examples</h2>
The following example will read the input colors.kit file and create the file Colors.xaml in the same directory.

    MaterialDesignColorsToXamlBrushes -i colors.kt -o Colors.xaml -o Colors.xaml


