Imports System.Text
Imports System.IO
Imports CommandLib
Module FuncHolder
    Public Sub DoForHelp(Args As List(Of String))
        If (Args.Count = 0) Then
            LogLevelDebug("Recognised Input as help")
            LogLevelDebug("Creating StringBuilder for help printout")
            Dim ToEcho As StringBuilder = New StringBuilder("Printing Help")
            LogLevelDebug("Looping through CommandGroups")
            For Each CommandGroup In Parser.CommandList
                ToEcho.Append(CommandGroup.Name)
                ToEcho.Append(" : ")
                ToEcho.Append(CommandGroup.Description)
                ToEcho.Append(" and has variations with ")
                LogLevelDebug("Looping Through commands in " & CommandGroup.Name)
                For Each Command In CommandGroup
                    ToEcho.Append(Command.Args.Length & ", ")
                Next
                ToEcho.Remove(ToEcho.Length - 2, 2)
                ToEcho.Append(" arguments")
                ToEcho.Append(vbCrLf)
            Next
            Console.WriteLine(ToEcho.ToString)
        Else
            LogLevelDebug("Recognised input as help COMMAND")
            LogLevelDebug("Printing Help for " & Args(0))
            For Each CommandGroup In Parser.CommandList
                If CommandGroup.Name = Args(0) Then
                    LogLevelDebug("Found Match")
                    Console.WriteLine(CommandGroup.ToString())
                End If
            Next
        End If
    End Sub
    Public Sub DoForEcho(Args As List(Of String))
        For Each Arg In Args
            LogLevelDebug(Arg)
        Next
        If Args.Count = 1 Then
            Console.WriteLine(Args(0))
        ElseIf Args.Count = 2 Then
            Console.WriteLine(Args(0))
            For I As Integer = 0 To Integer.Parse(Args(1)) - 1
                Console.WriteLine()
            Next
        End If
    End Sub
    Public Sub DoForDebug(Args As List(Of String))
        Parser.Debug = Not Parser.Debug
        ConditionalLog("Debug Mode ACTIVATED", "Debug Mode OFF")
    End Sub
    Public Sub DoForHide(Args As List(Of String))
        If (Args.Count = 1) Then
            LogLevelDebug("Recognised Pattern as FILE")
            File.SetAttributes(Args(0), FileAttributes.Hidden)
        ElseIf Args.Count = 2 AndAlso Args(1) = "-t" Then
            LogLevelDebug("Recognised Pattern as FILE TOGGLE")
            Dim attrReader As FileAttributes = File.GetAttributes(Args(0))
            If ((attrReader And FileAttributes.Hidden) > 0) Then
                File.SetAttributes(Args(0), FileAttributes.Normal)
            Else
                File.SetAttributes(Args(0), FileAttributes.Hidden)
            End If
        ElseIf Args.Count = 2 Then
            LogLevelDebug("Recognised Pattern as FILE ISHIDDEN(T/F)")
            If Boolean.Parse(Args(1)) Then
                File.SetAttributes(Args(0), FileAttributes.Hidden)
                LogLevelDebug("Hid File " & Args(0))
            Else
                File.SetAttributes(Args(0), FileAttributes.Normal)
                LogLevelDebug("File " & Args(0) & " is not hidden anymore")
            End If
        End If
    End Sub
    Public Sub DoForCwd(Args As List(Of String))
        Console.WriteLine(Environment.CurrentDirectory)
    End Sub
    Public Sub DoForCD(Args As List(Of String))
        Try
            Directory.SetCurrentDirectory(Args(0))
        Catch E As Exception
            ConditionalLog("The query :" & Args(0) & "Does not exist", "That path was not found")
        End Try
    End Sub


    Public Sub DoForMkdir(Args As List(Of String))
        If Not Args.Count = 2 Then
            LogLevelBasic("Incorrect Number of Arguments")
            Exit Sub
        End If
        If Args(0).ToLower.Equals("-dir") Then
            Directory.CreateDirectory(Args(1))
        Else
            Directory.CreateDirectory(Directory.GetParent(Args(1)).FullName)
            File.Create(Args(1)).Close()

        End If
    End Sub
    Public Sub DoForDestroy(Args As List(Of String))
        If (Args.Count = 1) Then
            Try
                If Not File.Exists(Args(0)) Then
                    LogLevelBasic("File did Not exist in the first place!")
                    Return
                End If
                File.Delete(Args(0))
            Catch Err As Exception
                ConditionalLog(Err.Data.ToString, "Could not delete. Try again with option debug on to see details")

            End Try
        End If
    End Sub
    Public Sub DoForDuplicate(ARgs As List(Of String))

    End Sub
End Module
