Imports System.Text.RegularExpressions
Imports CommandLib
Imports System.IO
Imports System.Reflection
Module CommandLine

    'Sub New()
    '    Parser.CommandList.Add(New CommandGroup("help", "Displays Help") _
    '                      .Add(New Command(AddressOf DoForHelp,
    '                                New CommandArg("-n", "The name of the command you want information on."))) _
    '                      .Add(New Command(AddressOf DoForHelp)))
    '    Parser.CommandList.Add(New CommandGroup("echo", "Prints a string to the console") _
    '                      .Add(New Command(AddressOf DoForEcho, New CommandArg("-s", "The String to be printed."))) _
    '                      .Add(New Command(AddressOf DoForEcho, New CommandArg("-s", "The String to be printed."),
    '                                     New CommandArg("-n", "The Amount of new lines to add after the text"))))
    '    Parser.CommandList.Add(New CommandGroup("debug", "Toggle Parser.Debug Mode") _
    '                      .Add(New Command(AddressOf DoForDebug)))
    '    Parser.CommandList.Add(New CommandGroup("hide", "Hides a file") _
    '                      .Add(New Command(AddressOf DoForHide, New CommandArg("f", "The File to hide."))) _
    '                      .Add(New Command(AddressOf DoForHide, New CommandArg("f", "The File to hide."),
    '                                     New CommandArg("-t", "Toggle file's HIDDEN attribute"))) _
    '                      .Add(New Command(AddressOf DoForHide, New CommandArg("file", "The file to hide"),
    '                                     New CommandArg("hidden", "The File's new hidden attribute"))))
    '    Parser.CommandList.Add(New CommandGroup("cwd", "Prints Current working Directory") _
    '                      .Add(New Command(AddressOf DoForCwd)))
    '    Parser.CommandList.Add(New CommandGroup("cd", "Changes the CWD") _
    '                      .Add(New Command(AddressOf DoForCD, New CommandArg("dir", "The Directory to change to"))))
    '    Parser.CommandList.Add(New CommandGroup("create", "Creates a File of Directory") _
    '                      .Add(New Command(AddressOf DoForMkdir, New CommandArg("-dir", "Tells the code to create a directory"),
    '                                     New CommandArg("file", "The File to create"))) _
    '                      .Add(New Command(AddressOf DoForMkdir, New CommandArg("-file", "Tells the code to create AddressOf File"),
    '                                     New CommandArg("file", "The File to create"))))
    '    Parser.CommandList.Add(New CommandGroup("delete", "Deletes a File or Directory") _
    '                      .Add(New Command(AddressOf DoForDestroy, New CommandArg("file", "The file to erase"))))
    '    Parser.CommandList.Add(New CommandGroup("copyto", "Copies a File or Directory to the specified location") _
    '                      .Add(New Command(AddressOf DoForDuplicate, New CommandArg("orig", "File to copy"),
    '                                     New CommandArg("newlocation", "The location to copy to"))) _
    '                      .Add(New Command(AddressOf DoForDuplicate, New CommandArg("orig", "File to copy"),
    '                                     New CommandArg("newlocation", "The location to copy to"),
    '                                     New CommandArg("-e", "If present, deletes original after copying"))))

    'End Sub

    Private Plugins As New List(Of CommandLineUtilityPlugin)
    Sub Main(Args() As String)
        LoadPlugins()

    End Sub
    Sub LoadPlugins()
        Dim assemblies As New List(Of Assembly)
        Dim plugs As String = "/plugins"
        For Each Plugin In Directory.EnumerateFiles(plugs, "*.dll")
            Dim a = Assembly.LoadFrom(Plugin)
            assemblies.Add(a)
        Next
        Dim plugType As Type = GetType(CommandLineUtilityPlugin)
        Dim PluginTypes As New List(Of Type)
        For Each assembly As Assembly In assemblies
            If assembly <> Nothing Then
                Dim cTypes As Type() = assembly.GetTypes()

                For Each type As Type In cTypes
                    If type.IsInterface Or type.IsAbstract Then
                        Continue For
                    Else
                        If type.GetInterface(plugType.FullName) <> Nothing Then
                            PluginTypes.Add(type)
                        End If
                    End If

                Next
            End If
        Next
        For Each Type In PluginTypes
            Plugins.Add(Activator.CreateInstance(Of CommandLineUtilityPlugin))
        Next
    End Sub
    Sub LogLevelDebug(Text As String)
        If Not Parser.Debug Then
            Return
        End If
        Console.WriteLine(Text)
    End Sub
    Sub LogLevelBasic(Text As String)
        Console.WriteLine(Text)
    End Sub
    Sub ConditionalLog(IfDebug As String, IfNotDebug As String)
        If Not Parser.Debug Then
            LogLevelBasic(IfNotDebug)
            Return
        End If
        LogLevelDebug(IfDebug)
    End Sub
End Module
