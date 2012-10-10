Imports System.IO
Imports System.Text     'Encoding用
Imports System.Text.RegularExpressions

Public Class load_db

    Public Sub read_PSP(ByVal filename As String, ByVal enc1 As Integer)
        Try
            Dim filenamebk As String = filename
            If enc1 = 2132004 Or enc1 = 512132004 Or enc1 = 951 Or enc1 = 21220932 Then
                Dim ctbl As New customtable
                Dim tw As New StreamWriter(Application.StartupPath & "\table\tmp", False, System.Text.Encoding.GetEncoding(65001))

                Dim fs As New FileStream(filename, FileMode.Open, FileAccess.Read)
                Dim bs(CInt(fs.Length - 1)) As Byte
                fs.Read(bs, 0, bs.Length)
                fs.Close()
                tw.Write(ctbl.custom_pasrse(bs, enc1))
                tw.Close()
                filename = Application.StartupPath & "\table\tmp"
                enc1 = 65001
            End If

            Dim m As MERGE = MERGE
            Dim ew As error_window = error_window
            Dim memory As New MemoryManagement
            Dim file As New FileStream(filename, FileMode.Open, FileAccess.Read)
            Dim sr As New StreamReader(file, System.Text.Encoding.GetEncoding(enc1))

            If filename = Application.StartupPath & "\table\tmp" Then
                filename = filenamebk
            End If

            Dim buffer(2) As String ' 0 = stream buffer, 1 = Game ID address, 2 = Game name, 3 = Codes 4= comment
            Dim code As New StringBuilder
            Dim cmt As New StringBuilder

            Dim counts(2) As Integer ' 0 = Line #, 1 = Progress bar counter, 2 = Total formatting errors
            Dim percent As Double = 0
            Dim gnode As New TreeNode ' Game name node for the TreeView control
            Dim cnode As New TreeNode ' Code name node for the TreeView control
            Dim skip As Boolean = False
            Dim NULLCODE As Boolean = False
            Dim cwcar As String = "_L"
            Dim z As Integer = 0

            m.codetree.Nodes.Add(Path.GetFileNameWithoutExtension(filename)).ImageIndex = 0 ' Add the root node and set its icon
            m.progbar.Value = 0 ' Reset the progress bar
            m.progbar.Visible = True ' Show the progress bar 

            reset_errors() ' Clear the error list before loading

            Try

                Do Until sr.EndOfStream = True ' Begin reading the file and stop when we reach the end

                    If skip = False Then

                        buffer(0) = sr.ReadLine
                        percent = (sr.BaseStream.Position * 100) / sr.BaseStream.Length
                        counts(0) += 1 ' Keep track of the line #
                        counts(1) += 1

                    End If

                    If sr.EndOfStream = True Then 'Check if we are at the end of the file
                        If buffer(0).Length >= 4 Then
                            If buffer(0).Substring(0, 3) = "_G " Then
                                buffer(2) = buffer(0).Substring(3, buffer(0).Length - 3).Trim
                                gnode = New TreeNode(buffer(0).Substring(3, buffer(0).Length - 3).Trim)
                                With gnode
                                    .Name = buffer(0).Substring(3, buffer(0).Length - 3).Trim
                                    .Tag = buffer(1)
                                    .ImageIndex = 1
                                End With
                                m.codetree.Nodes(0).Nodes.Add(gnode)

                                code.Clear()
                                cmt.Clear()
                                Exit Do
                            End If

                            If buffer(0).Substring(0, 3) = "_L " Or buffer(0).Substring(0, 3) = "_M " Or buffer(0).Substring(0, 3) = "_N " Then
                                NULLCODE = False
                                cwcar = buffer(0).Substring(0, 2)
                                '_L 0x12345678 0x12345678 24文字
                                buffer(0) = System.Text.RegularExpressions.Regex.Replace( _
                        buffer(0), "[g-zG-Z]", "A")
                                buffer(0) = buffer(0).ToUpper
                                buffer(0) = buffer(0).Replace(" 0A", " 0x")
                                buffer(0) = buffer(0).PadRight(24, "0"c)
                                If buffer(0).Substring(3, 2) = "0x" And buffer(0).Substring(14, 2) = "0x" Then 'If it is a correctly formed code record it
                                    code.AppendLine(buffer(0).Substring(3, 21).Trim)
                                End If
                            End If
                            If buffer(0).Substring(0, 2) = "_C" Then
                                If NULLCODE = True Then
                                    code.Append(cmt.ToString)
                                    cnode.Tag = code.ToString
                                End If
                                code.Clear()
                                cmt.Clear()

                                If buffer(0).Substring(2, 1) = "1" Then
                                    code.AppendLine("1")
                                Else
                                    code.AppendLine("0")
                                End If

                                cnode = New TreeNode(buffer(0).Substring(3, buffer(0).Length - 3).Trim)
                                cnode.Name = buffer(0).Substring(3, buffer(0).Length - 3).Trim
                                cnode.ImageIndex = 2
                                gnode.Nodes.Add(cnode)
                                NULLCODE = True
                            End If
                        End If
                        If NULLCODE = True Then
                            code.AppendLine()
                        End If
                        If buffer(0).Trim.Length > 1 AndAlso buffer(0).Substring(0, 1) = "#" Then
                            cmt.AppendLine(buffer(0))
                        End If
                        code.Append(cmt.ToString)
                        cnode.Tag = code.ToString
                        Exit Do
                    End If


                    If buffer(0).Length >= 4 Then

                        Select Case buffer(0).Substring(0, 3)

                            Case Is = "_S "
                                skip = False
                                If NULLCODE = True Then
                                    code.Append(cmt.ToString)
                                    cnode.Tag = code.ToString
                                    NULLCODE = False
                                End If
                                code.Clear()
                                cmt.Clear()
                                buffer(1) = buffer(0).Substring(3, buffer(0).Length - 3).Trim

                            Case Is = "_G "
                                skip = False
                                buffer(2) = buffer(0).Substring(3, buffer(0).Length - 3).Trim
                                gnode = New TreeNode(buffer(0).Substring(3, buffer(0).Length - 3).Trim)
                                With gnode
                                    .Name = buffer(0).Substring(3, buffer(0).Length - 3).Trim
                                    .Tag = buffer(1)
                                    .ImageIndex = 1
                                End With
                                m.codetree.Nodes(0).Nodes.Add(gnode)

                            Case Is = "_C0", "_C1", "_C2", "_CO"
                                skip = False

                                If NULLCODE = True Then
                                    code.Append(cmt.ToString)
                                    cnode.Tag = code.ToString
                                End If
                                code.Clear()
                                cmt.Clear()

                                If buffer(0).Substring(2, 1) = "1" Then
                                    code.AppendLine("1")
                                Else
                                    code.AppendLine("0")
                                End If

                                cnode = New TreeNode(buffer(0).Substring(3, buffer(0).Length - 3).Trim)
                                cnode.Name = buffer(0).Substring(3, buffer(0).Length - 3).Trim
                                cnode.ImageIndex = 2
                                gnode.Nodes.Add(cnode)
                                NULLCODE = True

                            Case Is = "_L ", "_M ", "_N "
                                NULLCODE = False
                                skip = False
                                cwcar = buffer(0).Substring(0, 3)
                                If cwcar = "_M " Then
                                    z = Integer.Parse(code.ToString.Substring(0, 1))
                                    code.Remove(0, 1)
                                    z = z And 1
                                    z = 2 Or z
                                    code.Insert(0, z.ToString())

                                ElseIf cwcar = "_N " Then
                                    z = Integer.Parse(code.ToString.Substring(0, 1))
                                    code.Remove(0, 1)

                                    z = z And 1
                                    z = 4 Or z
                                    code.Insert(0, z.ToString())
                                End If

                                '_L 0x12345678 0x12345678 24文字
                                buffer(0) = buffer(0).PadRight(24, "0"c)
                                If buffer(0).Substring(3, 2) = "0x" And buffer(0).Substring(14, 2) = "0x" Then 'If it is a correctly formed code record it
                                    buffer(0) = System.Text.RegularExpressions.Regex.Replace( _
                            buffer(0), "[g-zG-Z]", "A")
                                    buffer(0) = buffer(0).ToUpper
                                    buffer(0) = buffer(0).Replace(" 0A", " 0x")
                                    code.AppendLine(buffer(0).Substring(3, 21).Trim)

                                ElseIf buffer(0).Substring(0, 1) = "#" AndAlso buffer(0).Trim <> "#" Then
                                    cmt.AppendLine(buffer(0))

                                Else ' If it is incorrectly formed, ignore it.

                                    counts(2) += 1

                                    If buffer(0).Trim = Nothing Then 'If the line is blank
                                        write_errors(counts(0), counts(2), "<空白しかない行です,L-1>", gnode.Text, cnode.Text)
                                    Else
                                        write_errors(counts(0), counts(2), buffer(0) & " <対応してないコード形式です,L-1>", gnode.Text, cnode.Text) ' Write the ignored line to the error list
                                    End If

                                End If

                                Do Until skip = True

                                    buffer(0) = sr.ReadLine
                                    counts(0) += 1 ' Keep track of the line #
                                    percent = (sr.BaseStream.Position * 100) / sr.BaseStream.Length
                                    counts(1) += 1

                                    If buffer(0) = Nothing Then ' If we've reached the end of the file or a blank line

                                        If sr.EndOfStream = True Then 'Check if we are at the end of the file
                                            code.Append(cmt.ToString)
                                            cnode.Tag = code.ToString
                                            code.Clear()
                                            cmt.Clear()
                                            Exit Do
                                        End If
                                    End If

                                    If buffer(0).Length >= 2 Then
                                        buffer(0) = buffer(0).PadRight(24)
                                        If buffer(0).Substring(0, 3) = cwcar Then
                                            If buffer(0).Substring(3, 2) = "0x" And buffer(0).Substring(14, 2) = "0x" Then 'If it is a correctly formed code record it
                                                buffer(0) = System.Text.RegularExpressions.Regex.Replace( _
                                        buffer(0), "[g-zG-Z]", "A")
                                                buffer(0) = buffer(0).ToUpper
                                                buffer(0) = buffer(0).Replace(" 0A", " 0x")
                                                code.AppendLine(buffer(0).Substring(3, 21).Trim)

                                            Else ' If it is incorrectly formed, add it to the error list and ignore it
                                                counts(2) += 1

                                                If buffer(0).Trim = Nothing Then 'If the line is blank
                                                    write_errors(counts(0), counts(2), "<空白しかない行です,L-LOOP>", gnode.Text, cnode.Text)
                                                Else
                                                    write_errors(counts(0), counts(2), buffer(0) & " <対応してないコード形式です,L-LOOP>", gnode.Text, cnode.Text) ' Write the ignored line to the error list
                                                End If

                                            End If

                                        ElseIf buffer(0).Substring(0, 1) = "#" AndAlso buffer(0).Trim <> "#" Then
                                            cmt.AppendLine(buffer(0))

                                        ElseIf buffer(0).Substring(0, 2) = "_C" Or buffer(0).Substring(0, 2) = "_S" Then
                                            code.Append(cmt.ToString)
                                            cnode.Tag = code.ToString
                                            code.Clear()
                                            cmt.Clear()
                                            ' Store all collected codes in the nodes 'tag'
                                            skip = True ' If a new game or code is found, skip the initial read so it is processed

                                        End If

                                        If counts(1) >= 100 Then
                                            ' Update the progressbar every 20 repetitions otherwise the program 
                                            ' will slow to a crawl from the constant re-draw of the progress bar
                                            m.progbar.Value = Convert.ToInt32(percent)
                                            m.progbar.PerformStep()
                                            Application.DoEvents()
                                            counts(1) = 0
                                        End If
                                    End If
                                Loop


                            Case Else ' This will catch anything that is out of place

                                buffer(0) = buffer(0).PadRight(2)
                                If buffer(0).Substring(0, 1) = "#" AndAlso buffer(0).Trim <> "#" Then
                                    cmt.AppendLine(buffer(0))

                                ElseIf counts(0) = 1 AndAlso buffer(0).Contains("[") AndAlso buffer(0).Contains("]") Then

                                Else ' If what we found isn't a comment, ignore it

                                    counts(2) += 1

                                    If buffer(0).Trim = Nothing Then 'If the line is blank
                                        write_errors(counts(0), counts(2), "<空白しかない行です,H-EX>", gnode.Text, cnode.Text)
                                    Else
                                        write_errors(counts(0), counts(2), buffer(0) & " <追加されませんでした,H-EX>", gnode.Text, cnode.Text) ' Write the ignored line to the error list
                                    End If

                                    buffer(0) = sr.ReadLine ' Read the next line after the error
                                    counts(0) += 1
                                    counts(1) += 1
                                    skip = True ' Skip the intial read
                                End If

                        End Select

                    Else
                        buffer(0) = buffer(0).PadRight(2)
                        If NULLCODE = False AndAlso buffer(0).Substring(0, 1) = "#" Then
                            cmt.AppendLine(buffer(0).Trim)
                        Else
                            ' This is set if there is a garbage line in the database and
                            ' will write the line to the error window and try to continue loading
                            counts(2) += 1
                            'Determine if it's a blank line
                            If buffer(0).Trim = Nothing Then
                                write_errors(counts(0), counts(2), "<空白しかない行です>", gnode.Text, cnode.Text)
                            Else
                                write_errors(counts(0), counts(2), buffer(0) & " <追加されませんでした>", gnode.Text, cnode.Text)
                            End If
                        End If
                        skip = False
                    End If

                    If counts(1) >= 100 Then

                        ' Update the progressbar every 20 repetitions otherwise the program 
                        ' will slow to a crawl from the constant re-draw of the progress bar
                        m.progbar.Value = Convert.ToInt32(percent)
                        m.progbar.PerformStep()
                        Application.DoEvents()
                        counts(1) = 0

                    End If
                Loop

            Catch ex As Exception

                MessageBox.Show(ex.Message)

            End Try

            If (counts(2) > 0) Then
                ew.Show()
                ew.tab_error.SelectedIndex = 0
                m.Focus()
                reset_toolbar()
            End If

            If ew.list_load_error.Items.Count = 0 And ew.list_save_error.Items.Count > 0 Then
                ew.Show()
                ew.tab_error.SelectedIndex = 1
                m.Focus()
                reset_toolbar()
            End If

            m.progbar.Visible = False
            sr.Close()
            file.Close()
            memory.FlushMemory() ' Force a garbage collection after all the memory processing

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub read_PSX(ByVal filename As String, ByVal enc1 As Integer)
        Try
            Dim filenamebk As String = filename
            If enc1 = 2132004 Or enc1 = 512132004 Or enc1 = 951 Or enc1 = 21220932 Then
                Dim ctbl As New customtable
                Dim tw As New StreamWriter(Application.StartupPath & "\table\tmp", False, System.Text.Encoding.GetEncoding(65001))

                Dim fs As New FileStream(filename, FileMode.Open, FileAccess.Read)
                Dim bs(CInt(fs.Length - 1)) As Byte
                fs.Read(bs, 0, bs.Length)
                fs.Close()
                tw.Write(ctbl.custom_pasrse(bs, enc1))
                tw.Close()
                filename = Application.StartupPath & "\table\tmp"
                enc1 = 65001
            End If

            Dim m As MERGE = MERGE
            Dim ew As error_window = error_window
            Dim memory As New MemoryManagement
            Dim file As New FileStream(filename, FileMode.Open, FileAccess.Read)
            Dim sr As New StreamReader(file, System.Text.Encoding.GetEncoding(enc1))

            If filename = Application.StartupPath & "\table\tmp" Then
                filename = filenamebk
            End If

            Dim buffer(4) As String ' 0 = stream buffer, 1 = SLUS address, 2 = Game name, 3 = Codes, 4 = fixed codes
            Dim code As New StringBuilder
            Dim cmt As New StringBuilder
            Dim counts(2) As Integer ' 0 = Line #, 1 = Progress bar counter, 2 = Total formatting errors, 3 = Error number
            Dim percent As Double = 0
            Dim gnode As New TreeNode ' Game name node for the TreeView control
            Dim cnode As New TreeNode ' Code name node for the TreeView control
            Dim skip As Boolean = False
            Dim nullcode As Boolean = False
            buffer(0) = Nothing
            gnode.Text = Nothing
            cnode.Text = Nothing
            m.codetree.Nodes.Add(Path.GetFileNameWithoutExtension(filename)).ImageIndex = 0 ' Add the root node and set its icon
            m.progbar.Visible = True ' Show the progress bar and reset it's value
            m.progbar.Value = 0 ' Reset the progress bar
            reset_errors() ' Clear the error list before loading

            Try

                Do Until sr.EndOfStream = True ' Begin reading the file and stop when we reach the end

                    If skip = False Then

                        buffer(0) = sr.ReadLine
                        percent = (sr.BaseStream.Position * 100) / sr.BaseStream.Length
                        counts(0) += 1 ' Keep track of the line #
                        counts(1) += 1

                    End If

                    If buffer(0).Length >= 4 Then

                        Select Case buffer(0).Substring(0, 2)

                            Case Is = "_S"
                                skip = False

                                If nullcode = True Then
                                    code.AppendLine()
                                    code.Append(cmt.ToString)
                                    cnode.Tag = code.ToString
                                End If
                                code.Clear()
                                cmt.Clear()
                                buffer(1) = buffer(0).Substring(3, buffer(0).Length - 3).Trim

                            Case Is = "_G"
                                skip = False
                                buffer(2) = buffer(0).Substring(3, buffer(0).Length - 3).Trim
                                gnode = New TreeNode(buffer(0).Substring(3, buffer(0).Length - 3).Trim)
                                With gnode
                                    .Name = buffer(0).Substring(3, buffer(0).Length - 3).Trim
                                    .Tag = buffer(1)
                                    .ImageIndex = 1
                                End With
                                m.codetree.Nodes(0).Nodes.Add(gnode)

                            Case Is = "_C"
                                skip = False
                                If nullcode = True Then
                                    code.Append(cmt.ToString)
                                    cnode.Tag = code.ToString

                                End If
                                code.Clear()
                                cmt.Clear()


                                If buffer(0).Substring(2, 1) = "1" Then
                                    code.AppendLine("1")
                                Else
                                    code.AppendLine("0")
                                End If

                                cnode = New TreeNode(buffer(0).Substring(3, buffer(0).Length - 3).Trim)
                                cnode.Name = buffer(0).Substring(3, buffer(0).Length - 3).Trim
                                cnode.ImageIndex = 2

                                gnode.Nodes.Add(cnode)
                                nullcode = True

                            Case Is = "_L"

                                skip = False
                                nullcode = False
                                buffer(0) = buffer(0).Trim
                                '_L 12345678 1234
                                If buffer(0).Length = 16 Then
                                    If buffer(0).Substring(11, 1) = " " Then 'If it is a correctly formed code record it
                                        buffer(0) = buffer(0).Replace("?", "A")
                                        code.AppendLine(buffer(0).Substring(3, 13))
                                    End If
                                Else

                                    buffer(4) = clean_PSX(buffer(0))

                                    If buffer(4).Length = 16 Then 'Attempt to remove white spaces and re-check
                                        code.AppendLine(buffer(4).Substring(3, 13))

                                    Else ' If it is incorrectly formed, ignore it.

                                        counts(2) += 1
                                        If buffer(0).Trim = Nothing Then 'If the line is blank
                                            write_errors(counts(0), counts(2), "<空白しかない行です,L-1>", gnode.Text, cnode.Text)
                                        Else
                                            write_errors(counts(0), counts(2), buffer(0) & " <追加されませんでした,L-1>", gnode.Text, cnode.Text) ' Write the ignored line to the error list
                                        End If

                                    End If

                                End If

                                Do Until skip = True

                                    buffer(0) = sr.ReadLine
                                    counts(0) += 1 ' Keep track of the line #
                                    percent = (sr.BaseStream.Position * 100) / sr.BaseStream.Length
                                    counts(1) += 1

                                    If buffer(0) = Nothing Then ' If we've reached the end of the file

                                        If sr.EndOfStream = True Then
                                            code.Append(cmt.ToString)
                                            cnode.Tag = code.ToString
                                            code.Clear()
                                            cmt.Clear()
                                        End If

                                        Exit Do

                                    ElseIf buffer(0).Length >= 4 Then

                                        If buffer(0).Substring(0, 2) = "_L" Then
                                            buffer(0) = buffer(0).Trim

                                            If buffer(0).Length = 16 Then
                                                If buffer(0).Substring(11, 1) = " " Then
                                                    buffer(0) = buffer(0).Replace("?", "A")
                                                    code.AppendLine(buffer(0).Substring(3, 13))
                                                End If
                                            Else

                                                buffer(4) = clean_PSX(buffer(0).Trim)

                                                If buffer(4).Length = 16 Then
                                                    If buffer(0).Substring(11, 1) = " " Then 'Attempt to remove white spaces and re-check
                                                        code.AppendLine(buffer(4).Substring(3, 13))
                                                    End If
                                                Else ' If it is incorrectly formed, ignore it.

                                                    counts(2) += 1
                                                    If buffer(0).Replace(" ", "") = Nothing Then 'If the line is blank
                                                        write_errors(counts(0), counts(2), "<空白しかない行です,L-LOOP>", gnode.Text, cnode.Text)
                                                    Else
                                                        write_errors(counts(0), counts(2), buffer(0) & " <追加されませんでした,L-LOOP>", gnode.Text, cnode.Text) ' Write the ignored line to the error list
                                                    End If

                                                End If

                                            End If

                                        ElseIf buffer(0).Substring(0, 2) = "_S" Or buffer(0).Substring(0, 2) = "_C" Then
                                            code.Append(cmt.ToString)
                                            cnode.Tag = code.ToString
                                            code.Clear()
                                            cmt.Clear()
                                            skip = True ' If a new game or code is found, skip the initial read so it is processed

                                        End If
                                    End If
                                    If buffer(0).Length >= 2 Then
                                        If nullcode = False AndAlso buffer(0).Substring(0, 1) = "#" Then
                                            cmt.AppendLine(buffer(0))
                                        End If
                                    End If

                                    If counts(1) >= 20 Then
                                        ' Update the progressbar every 20 repetitions otherwise the program 
                                        ' will slow to a crawl from the constant re-draw of the progress bar
                                        m.progbar.Value = Convert.ToInt32(percent)
                                        m.progbar.PerformStep()
                                        Application.DoEvents()
                                        counts(1) = 0
                                    End If

                                Loop

                            Case Else ' This will catch anything that is out of place
                                If buffer(0).Length >= 2 Then

                                    If buffer(0).Substring(0, 1) = "#" Then

                                        cmt.AppendLine(buffer(0).Trim)

                                    ElseIf counts(0) = 1 AndAlso buffer(0).Contains("[") AndAlso buffer(0).Contains("]") Then

                                    Else ' what we found isn't a comment, ignore it

                                        counts(2) += 1
                                        If buffer(0).Trim = Nothing Then 'If the line is blank
                                            write_errors(counts(0), counts(2), "<空白しかない行です,H-EX>", gnode.Text, cnode.Text)
                                        Else
                                            write_errors(counts(0), counts(2), buffer(0) & " <追加されませんでした,H-EX>", gnode.Text, cnode.Text) ' Write the ignored line to the error list
                                        End If

                                        buffer(0) = sr.ReadLine
                                        counts(0) += 1
                                        counts(1) += 1
                                        skip = True

                                    End If
                                End If
                        End Select

                    Else

                        If buffer(0).Length >= 2 Then

                            If buffer(0).Substring(0, 1) = "#" Then
                                cmt.AppendLine(buffer(0).Trim)
                            Else
                                ' This is set if there is a garbage line or blank line in the database and
                                ' will write the line to the error window and try to continue loading
                                counts(2) += 1
                                'Determine if it's a blank line

                                If buffer(0).Trim = Nothing Then
                                    write_errors(counts(0), counts(2), "<!空白しかない行です>", gnode.Text, cnode.Text)
                                Else
                                    write_errors(counts(0), counts(2), buffer(0) & " <!追加されませんでした>", gnode.Text, cnode.Text)
                                End If

                                skip = False

                            End If
                        End If

                        If counts(1) >= 100 Then

                            ' Update the progressbar every 20 repetitions otherwise the program 
                            ' will slow to a crawl from the constant re-draw of the progress bar
                            m.progbar.Value = Convert.ToInt32(percent)
                            m.progbar.PerformStep()
                            Application.DoEvents()
                            counts(1) = 0

                        End If
                    End If
                Loop

            Catch ex As Exception
                MessageBox.Show(ex.Message, "エラー")
            End Try


            If (counts(2) > 0) Then
                ew.Show()
                ew.tab_error.SelectedIndex = 0
                m.Focus()
                reset_toolbar()
            End If

            If ew.list_load_error.Items.Count = 0 And ew.list_save_error.Items.Count > 0 Then
                ew.Show()
                ew.tab_error.SelectedIndex = 1
                m.Focus()
                reset_toolbar()
            End If


            m.progbar.Visible = False
            sr.Close()
            file.Close()
            memory.FlushMemory() ' Force a garbage collection after all the memory processing

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub read_cf(ByVal filename As String, ByVal enc1 As Integer)

        Dim t1 As Integer = System.Environment.TickCount
        Dim m As MERGE = MERGE
        Dim ew As error_window = error_window
        Dim memory As New MemoryManagement
        Dim file As New FileStream(filename, FileMode.Open, FileAccess.Read)
        Dim counts(2) As Integer ' 0 = Line #, 1 = Progress bar counter, 2 = Total formatting errors, 3 = Error number
        Dim percent As Double = 0
        Dim gnode As New TreeNode ' Game name node for the TreeView control
        Dim cnode As New TreeNode ' Code name node for the TreeView control
        Dim skip As Boolean = False
        Dim b3 As String = Nothing
        Dim b4 As String = Nothing
        Dim b5 As String = Nothing
        Dim b6 As String = Nothing
        m.codetree.Nodes.Add(Path.GetFileNameWithoutExtension(filename)).ImageIndex = 0 ' Add the root node and set its icon
        m.progbar.Visible = True ' Show the progress bar and reset it's value
        m.progbar.Value = 0 ' Reset the progress bar


        reset_errors() ' Clear the error list before loading
        'ファイルを読み込むバイト型配列を作成する
        Dim bs(CInt(file.Length)) As Byte
        'ファイルの内容をすべて読み込む
        file.Read(bs, 0, bs.Length)
        Dim cfdatlen As Integer = bs.Length
        Dim cf_utf16(33) As Byte
        Dim cfid(4) As Byte
        cfid(4) = &H2D
        Dim str As String = Nothing
        Dim gname() As Byte = Nothing
        Dim cname() As Byte = Nothing
        Dim i As Integer = 0
        Dim n As Integer = 0
        Dim sb As New System.Text.StringBuilder()
        counts(0) = cfdatlen \ 36

        Try
            While i < cfdatlen - 3

                If (i And 1) = 0 Then
                    If bs(i) = &H47 AndAlso bs(i + 1) = &H20 Then 'G ゲーム名
                        If b6 <> Nothing Then
                            cnode.Tag = b6
                            b6 = Nothing
                        End If
                        i += 2
                        'ヽ|・∀・|ノCP1201　上=0x\4E0A
                        '　|＿＿＿|
                        '　　|　|
                        Do Until bs(i) = 10 AndAlso bs(i + 1) = 10 AndAlso (i And 1) = 0 '0A0A
                            n += 1
                            i += 1
                        Loop
                        Array.Resize(gname, n)
                        Array.ConstrainedCopy(bs, i - n, gname, 0, n)
                        str = Encoding.GetEncoding(1201).GetString(gname)
                        n = 0
                        gnode = New TreeNode(str.Trim)
                        With gnode
                            .Name = str.Trim
                            .Tag = Nothing
                            .ImageIndex = 1
                        End With
                        m.codetree.Nodes(0).Nodes.Add(gnode)
                        counts(1) += 1

                    ElseIf bs(i) = &H4D AndAlso bs(i + 1) = &H20 Then 'M ゲームID
                        i += 34
                        Array.ConstrainedCopy(bs, i - 32, cf_utf16, 0, 32)
                        str = System.Text.Encoding.GetEncoding(1201).GetString(cf_utf16)
                        sb.Clear()
                        sb.Append(cf2sceid(cfid, str))
                        b3 = sb.ToString()
                        b3 = b3.Replace(CChar(Chr(0)), "0")
                        gnode.Tag = b3
                        counts(1) += 1

                    ElseIf bs(i) = &H44 AndAlso bs(i + 1) = &H20 Then 'D コード名

                        If b6 <> Nothing Then
                            cnode.Tag = b6
                            b6 = Nothing
                        End If
                        i += 2
                        'ヽ|・∀・|ノCP1201　上=0x\4E0A
                        '　|＿＿＿|
                        '　　|　|
                        Do Until bs(i) = 10 AndAlso bs(i + 1) = 10 AndAlso (i And 1) = 0  '0A0A
                            n += 1
                            i += 1
                        Loop
                        Array.Resize(cname, n)
                        Array.ConstrainedCopy(bs, i - n, cname, 0, n)
                        str = System.Text.Encoding.GetEncoding(1201).GetString(cname)
                        n = 0
                        cnode = New TreeNode(str.Trim)
                        cnode.Name = str.Trim
                        cnode.ImageIndex = 2
                        gnode.Nodes.Add(cnode)
                        b6 = "0" & vbCrLf
                        counts(1) += 1

                    ElseIf bs(i) = &H43 AndAlso bs(i + 1) = &H20 Then 'C コード内容
                        b5 = Nothing
                        i += 34
                        Array.ConstrainedCopy(bs, i - 32, cf_utf16, 0, 32)
                        str = System.Text.Encoding.GetEncoding(1201).GetString(cf_utf16)
                        sb.Clear()
                        sb.Append("0x")
                        sb.Append(str.Substring(0, 8))
                        sb.Append(" 0x")
                        sb.AppendLine(str.Substring(8, 8))
                        b5 = sb.ToString
                        b6 &= b5
                        counts(1) += 1
                    End If
                End If
                i += 1

                If counts(1) = counts(0) Then

                    ' Update the progressbar every 20 repetitions otherwise the program 
                    ' will slow to a crawl from the constant re-draw of the progress bar
                    percent = (i * 100) / cfdatlen
                    m.progbar.Value = Convert.ToInt32(percent)
                    m.progbar.PerformStep()
                    Application.DoEvents()
                    counts(1) = 0
                End If

            End While

            If b6 <> Nothing Then
                cnode.Tag = b6
            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message)

        End Try

        If ew.list_load_error.Items.Count = 0 And ew.list_save_error.Items.Count > 0 Then
            ew.Show()
            ew.tab_error.SelectedIndex = 1
            m.Focus()
            reset_toolbar()
        End If

        m.progbar.Visible = False
        file.Close()
        memory.FlushMemory() ' Force a garbage collection after all the memory processing

        m.tt.Text = t1.ToString

    End Sub

    Public Sub read_cfcp1201(ByVal filename As String, ByVal enc1 As Integer)

        Dim t1 As Integer = System.Environment.TickCount

        Dim m As MERGE = MERGE
        Dim ew As error_window = error_window
        Dim memory As New MemoryManagement
        Dim file As New FileStream(filename, FileMode.Open, FileAccess.Read)
        Dim counts(2) As Integer ' 0 = Line #, 1 = Progress bar counter, 2 = Total formatting errors, 3 = Error number
        Dim percent As Double = 0
        Dim gnode As New TreeNode ' Game name node for the TreeView control
        Dim cnode As New TreeNode ' Code name node for the TreeView control
        Dim skip As Boolean = False
        Dim b3 As String = "0" & vbCrLf
        m.codetree.Nodes.Add(Path.GetFileNameWithoutExtension(filename)).ImageIndex = 0 ' Add the root node and set its icon
        m.progbar.Visible = True ' Show the progress bar and reset it's value
        m.progbar.Value = 0 ' Reset the progress bar

        reset_errors() ' Clear the error list before loading

        Dim bs(CInt(file.Length)) As Byte
        file.Read(bs, 0, bs.Length)
        Dim cfdatlen As Integer = bs.Length

        Dim cfid(4) As Byte
        cfid(4) = &H2D
        Dim s As String = Nothing
        Dim i As Integer = 0
        Dim k As Integer = 0
        Dim n As Integer = 0
        Dim cmt As Boolean = False
        Dim sb As New System.Text.StringBuilder()
        counts(0) = cfdatlen \ 36

        ''CP1201　UFT16ビッグエンディアンでコードフリークDATを読む
        Dim sr = New StreamReader(filename, Encoding.GetEncoding(1201))
        s = sr.ReadToEnd()
        sr.Close()
        s = rpstringin(s)
        ''U+0A0Aで分割
        Dim ss As String() = s.Split(CChar("ਊ"))
        Dim head As String = ""
        Dim sbb As String = ""

        Try
            For i = 0 To ss.Length - 1
                If (ss(i).Length > 1) Then
                    head = ss(i).Substring(0, 1)
                    s = ss(i).Remove(0, 1)
                    ''U+4720でコードタイトル
                    If (head = "䜠") Then
                        cnode.Tag = sb.ToString
                        sb.Clear()
                        gnode = New TreeNode(s)
                        With gnode
                            .Name = s
                            .Tag = Nothing
                            .ImageIndex = 1
                        End With
                        m.codetree.Nodes(0).Nodes.Add(gnode)
                        ''U+4D20でゲームID
                    ElseIf (head = "䴠") Then
                        cnode = New TreeNode("(M)")
                        cnode.Name = "(M)"
                        cnode.ImageIndex = 2
                        gnode.Tag = cf2sceid(cfid, s)
                        s = s.Insert(8, " 0x")
                        sb.AppendLine("0")
                        sb.Append("0x")
                        sb.AppendLine(s)
                        gnode.Nodes.Add(cnode)
                        ''U+4420でコード名
                    ElseIf (head = "䐠") Then
                        ''コード名が’’(アポストロフィx2)の場合コメント
                        If (ss(i).Length > 2 AndAlso s.Substring(0, 2) = "''") Then
                            cmt = True
                            sbb = s
                        Else
                            cnode.Tag = sb.ToString
                            sb.Clear()
                            cmt = False
                            cnode = New TreeNode(s)
                            cnode.Name = s
                            cnode.ImageIndex = 2
                            sb.AppendLine("0")
                            gnode.Nodes.Add(cnode)
                        End If

                        ''U+4320でコード内容
                    ElseIf (head = "䌠") Then
                        If (cmt = False) Then
                            sb.Append("0x")
                            sb.AppendLine(s.Insert(8, " 0x"))
                            counts(1) += 1
                            ''コメント
                        Else
                            sb.Append("#")
                            sb.AppendLine(sbb)
                        End If
                    End If

                    If counts(1) = counts(0) Then
                        ' Update the progressbar every 20 repetitions otherwise the program 
                        ' will slow to a crawl from the constant re-draw of the progress bar
                        percent = (i * 100) / cfdatlen
                        m.progbar.Value = Convert.ToInt32(percent)
                        m.progbar.PerformStep()
                        Application.DoEvents()
                        counts(1) = 0
                    End If
                End If
            Next

            cnode.Tag = sb.ToString



        Catch ex As Exception

            MessageBox.Show(ex.Message)

        End Try

        If ew.list_load_error.Items.Count = 0 And ew.list_save_error.Items.Count > 0 Then
            ew.Show()
            ew.tab_error.SelectedIndex = 1
            m.Focus()
            reset_toolbar()
        End If

        m.progbar.Visible = False
        file.Close()
        memory.FlushMemory() ' Force a garbage collection after all the memory processing

        m.tt.Text = t1.ToString
    End Sub

    Dim pattern As String() = {"<(C)>", "<(R)>", "<(TM)>", "<肉>", "<どくろ>", "<顔白>", "<かえる>"}
    Dim rp As String() = {"\xA9", "\xAE", "\x2122", "\x1C", "\x1D", "\x1E", "\x1F"}

    Public Function cf2sceid(ByVal cfid As Byte(), ByVal str As String) As String
        For k = 0 To 3
            cfid(k) = CByte(Convert.ToInt32(str.Substring(2 * k, 2), 16))
        Next
        str = Encoding.ASCII.GetString(cfid) & str.Substring(8, 8)     'str.Substring(8, 5)
        Return str
    End Function

    Private Function rpstringout(ByVal s As String) As String
        Dim r = New Regex("<.*?>")
        Dim m As Match = r.Match(s)
        While (m.Success = True)
            For i = 0 To 2
                If (m.Value = pattern(i)) Then
                    s = s.Replace(m.Value, rp(i))
                    Exit While
                End If
            Next
            m = m.NextMatch()
        End While
        Return s
    End Function

    Private Function rpstringin(ByVal s As String) As String
        Dim r = New Regex("[\x1C-\x1F]")
        Dim m As Match = r.Match(s)
        While (m.Success = True)
            For i = 3 To 6
                If (m.Value = rp(i)) Then
                    s = s.Replace(m.Value, pattern(i))
                    Exit While
                End If
                m = m.NextMatch()
            Next
        End While
        Dim u = New Regex("<.*?>")
        Dim n As Match = u.Match(s)
        While (n.Success = True)
            For i = 0 To 2
                If (n.Value = pattern(i)) Then
                    s = s.Replace(n.Value, rp(i))
                    Exit While
                End If
                n = n.NextMatch()
            Next
        End While
        Return s
    End Function

    Public Sub read_ar(ByVal filename As String, ByVal enc1 As Integer)

        Dim m As MERGE = MERGE
        Dim ew As error_window = error_window
        Dim memory As New MemoryManagement
        Dim file As New FileStream(filename, FileMode.Open, FileAccess.Read)
        Dim counts(2) As Integer ' 0 = Line #, 1 = Progress bar counter, 2 = Total formatting errors, 3 = Error number
        Dim percent As Double = 0
        Dim gnode As New TreeNode ' Game name node for the TreeView control
        Dim cnode As New TreeNode ' Code name node for the TreeView control
        Dim skip As Boolean = False
        m.codetree.Nodes.Add(Path.GetFileNameWithoutExtension(filename)).ImageIndex = 0 ' Add the root node and set its icon
        m.progbar.Visible = True ' Show the progress bar and reset it's value
        m.progbar.Value = 0 ' Reset the progress bar

        reset_errors() ' Clear the error list before loading
        'ファイルを読み込むバイト型配列を作成する
        Dim bs(CInt(file.Length)) As Byte
        'ファイルの内容をすべて読み込む
        file.Read(bs, 0, bs.Length)
        Dim datellen As Integer = bs.Length
        Dim str As String = Nothing
        Dim i As Integer = 28
        Dim k As Integer = 0
        Dim l As Integer = 0
        Dim blocklen As Integer = 0
        Dim id(9) As Byte
        Dim code(3) As Byte
        Dim gname() As Byte = Nothing
        Dim cname() As Byte = Nothing
        Dim codeline As Integer = 0
        Dim parsemode As Boolean = False
        Dim nextcode As Integer = 0
        Dim sb As New System.Text.StringBuilder()
        counts(0) = datellen \ 32
        Try

            While i < datellen
                blocklen = (CInt(bs(i)) + (CInt(bs(i + 1)) << 8)) << 2
                If blocklen = 0 Then
                    blocklen = datellen - i
                End If
                While k < blocklen
                    If parsemode = False Then
                        Array.ConstrainedCopy(bs, i + 7, id, 0, 10)
                        str = Encoding.GetEncoding(932).GetString(id)
                        str = str.PadRight(10, " "c)
                        gnode = New TreeNode(str)
                        With gnode
                            .Name = Nothing
                            .Tag = str
                            .ImageIndex = 1
                        End With
                        m.codetree.Nodes(0).Nodes.Add(gnode)
                        k = CInt(bs(i + 4)) - 18
                        Array.Resize(gname, k)
                        Array.ConstrainedCopy(bs, i + 18, gname, 0, k)
                        str = Encoding.GetEncoding(932).GetString(gname)
                        str = str.Replace(vbNullChar, "")
                        gnode.Text = str
                        gnode.Name = str
                        k = CInt(bs(i + 4))
                        l = CInt(bs(i + 5)) + CInt(bs(i + 6)) << 8
                        If l = 0 Then
                            Exit While
                        End If
                        parsemode = True
                    ElseIf parsemode = True Then
                        codeline = CInt(bs(i + k))
                        l = CInt(bs(i + k + 1)) - 1
                        Array.Resize(cname, l)
                        Array.ConstrainedCopy(bs, i + k + 4, cname, 0, l)
                        str = Encoding.GetEncoding(932).GetString(cname)
                        cnode = New TreeNode(str.Trim)
                        cnode.Name = str.Trim
                        cnode.ImageIndex = 2
                        gnode.Nodes.Add(cnode)
                        sb.Clear()
                        sb.Append("2")
                        sb.Append(vbCrLf)
                        l = CInt(bs(i + k + 2)) << 2
                        While codeline > 0
                            Array.ConstrainedCopy(bs, i + k + l, code, 0, 4)
                            str = Convert.ToString(BitConverter.ToInt32(code, 0), 16)
                            str = str.ToUpper.PadLeft(8, "0"c)
                            sb.Append("0x")
                            sb.Append(str)
                            sb.Append(" 0x")
                            Array.ConstrainedCopy(bs, i + k + l + 4, code, 0, 4)
                            str = Convert.ToString(BitConverter.ToInt32(code, 0), 16)
                            str = str.ToUpper.PadLeft(8, "0"c)
                            sb.Append(str)
                            sb.Append(vbCrLf)
                            l += 8
                            codeline -= 1
                        End While
                        cnode.Tag = sb.ToString
                        nextcode = CInt(bs(i + k + 3)) << 2
                        k += nextcode
                        counts(1) += 1
                        If nextcode = 0 Then
                            Exit While
                        End If
                    End If
                End While
                i += blocklen
                k = 0
                parsemode = False

                If counts(1) = counts(0) Then

                    ' Update the progressbar every 20 repetitions otherwise the program 
                    ' will slow to a crawl from the constant re-draw of the progress bar
                    percent = (i * 100) / datellen
                    m.progbar.Value = Convert.ToInt32(percent)
                    m.progbar.PerformStep()
                    Application.DoEvents()
                    counts(1) = 0
                End If

            End While

        Catch ex As Exception

            MessageBox.Show(ex.Message)

        End Try

        If ew.list_load_error.Items.Count = 0 And ew.list_save_error.Items.Count > 0 Then
            ew.Show()
            ew.tab_error.SelectedIndex = 1
            m.Focus()
            reset_toolbar()
        End If


            m.progbar.Visible = False
            file.Close()
            memory.FlushMemory() ' Force a garbage collection after all the memory processing

    End Sub

    Private Sub write_errors(ByVal line As Integer, ByVal error_n As Integer, ByVal error_t As String, ByVal game_t As String, ByVal code_t As String)

        Dim ew As error_window = error_window
        Dim itemx As New ListViewItem
        If error_n > 0 AndAlso game_t <> "" AndAlso code_t <> "" AndAlso error_t <> "" Then
            itemx.Text = error_n.ToString
            itemx.SubItems.Add(line.ToString)
            itemx.SubItems.Add(game_t)
            itemx.SubItems.Add(code_t)
            itemx.SubItems.Add(error_t.Trim)
            ew.list_load_error.Items.Add(itemx)
            Application.DoEvents()
        End If

    End Sub

    Private Sub reset_errors()

        Dim ew As error_window = error_window
        Dim m As MERGE = MERGE

        ew.Hide()
        m.options_error.Text = "エラーログを見る"
        m.options_error.Checked = False
        ew.list_load_error.Items.Clear()

    End Sub

    Private Sub reset_toolbar()

        If MERGE.options_error.Checked = False Then
            MERGE.options_error.Checked = True
            MERGE.options_error.Text = "エラーログを隠す"
        End If

    End Sub

    Private Function clean_PSX(ByVal s As String) As String

        ' This will attempt to remove any extra white spaces
        ' if the attempt fails, it will be marked as incorrect
        ' and written into the error list.

        Dim i As Integer = 0
        clean_PSX = Nothing

        For i = 0 To s.Length - 1

            If s.Substring(i, 1) = " " And i <> 10 And i <> 2 Then ' If we're not on the 3rd space or the 11th
            Else
                clean_PSX &= s.Substring(i, 1)
            End If

        Next

        ' This will attempt to fix a broken code missing the code type after the white spaces are removed.
        ' First it will check if the length is incorrect. If so, calculate the value and place the correct code type.
        ' The only problem with this is if the code was a 16-bit 'equal to' type (AKA joker), it will be incorrect
        ' since there is no way to determine if it was.  More than likely it won't be.

        If clean_PSX.Length = 15 Then ' If we are 1 characters short of an actual code

            If clean_PSX.Substring(3, 1) = "0" Then ' We know it's missing its code type

                If clean_PSX.Substring(clean_PSX.Length - 4, 4) = "????" _
                Or clean_PSX.Substring(clean_PSX.Length - 3, 3) = "???" _
                Or clean_PSX.Substring(clean_PSX.Length - 4, 2) = "??" _
                Or clean_PSX.Substring(clean_PSX.Length - 4, 1) = "?" Then

                    clean_PSX = clean_PSX.Substring(0, 3) & "8" & clean_PSX.Substring(3, clean_PSX.Length - 3) ' 16-bit write

                ElseIf clean_PSX.Substring(clean_PSX.Length - 2, 2) = "??" Or clean_PSX.Substring(clean_PSX.Length - 1, 1) = "?" Then

                    clean_PSX = clean_PSX.Substring(0, 3) & "3" & clean_PSX.Substring(3, clean_PSX.Length - 3) ' 8-bit write

                Else

                    If Convert.ToInt32(clean_PSX.Substring(clean_PSX.Length - 4, 4), 16) < 256 Then

                        clean_PSX = clean_PSX.Substring(0, 3) & "3" & clean_PSX.Substring(3, clean_PSX.Length - 3) ' 8-bit write

                    Else

                        clean_PSX = clean_PSX.Substring(0, 3) & "8" & clean_PSX.Substring(3, clean_PSX.Length - 3) ' 16-bit write

                    End If

                End If

            End If

        End If

    End Function

    Dim enctable As Integer() = {0, 37, 437, 500, 708, 709, 710, 720, 737, 775, 850, 852, 855, 857, 858, 860, 861, 862, 863, 864, 865, 866, 869, 870, 874, 875, 932, 936, 949, 950, 1026, 1047, 1140, 1141, 1142, 1143, 1144, 1145, 1146, 1147, 1148, 1149, 1200, 1201, 1250, 1251, 1252, 1253, 1254, 1255, 1256, 1257, 1258, 1361, 10000, 10001, 10002, 10003, 10004, 10005, 10006, 10007, 10008, 10010, 10017, 10021, 10029, 10079, 10081, 10082, 12000, 12001, 20000, 20001, 20002, 20003, 20004, 20005, 20105, 20106, 20107, 20108, 20127, 20261, 20269, 20273, 20277, 20278, 20280, 20284, 20285, 20290, 20297, 20420, 20423, 20424, 20833, 20838, 20866, 20871, 20880, 20905, 20924, 20932, 20936, 20949, 21025, 21027, 21866, 28591, 28592, 28593, 28594, 28595, 28596, 28597, 28598, 28599, 28603, 28605, 29001, 38598, 50220, 50221, 50222, 50225, 50227, 50229, 50930, 50931, 50933, 50935, 50936, 50937, 50939, 51932, 51936, 51949, 51950, 52936, 54936, 57002, 57003, 57004, 57005, 57006, 57007, 57008, 57009, 57010, 57011, 65000, 65001, 951, 2132004, 512132004, 21220932}

    Public Function check_enc(ByVal filename As String) As Integer

        Dim file As New FileStream(filename, FileMode.Open, FileAccess.Read)
        Dim cp(20) As Byte
        Dim str As String
        If file.Length < 21 Then
            file.Read(cp, 0, 20)
        ElseIf file.Length > 1000 Then
            Array.Resize(cp, 1000)
            file.Read(cp, 0, 1000)
        ElseIf file.Length > 300 Then
            Array.Resize(cp, 300)
            file.Read(cp, 0, 300)
        End If
        file.Close()

        If My.Settings.checkcpstr = True Then
            '5B 43 50 39 33 36 5D
            If My.Settings.checkcpstr = True AndAlso cp(0) = &H5B Then
                str = Encoding.GetEncoding(0).GetString(cp)


                Dim r As New Regex("^\[.+\]", RegexOptions.ECMAScript)
                Dim m As Match = r.Match(str)
                If m.Success Then
                    str = m.Value
                    If (str.Contains("2004") = False AndAlso (str.Contains("Shift_JIS") Or str.Contains("Windows-31J"))) Or str = "[CP932]" Then
                        Return 932
                    ElseIf str = "[GBK]" Or str = "[CP936]" Then
                        Return 936
                    ElseIf str = "[Big5-HKSCS]" Or str = "[CP951]" Then
                        Return 951
                    ElseIf str = "[UHC]" Or str = "[CP949]" Then
                        Return 949
                    ElseIf str = "[EUC-JP]" Or str = "[CP51932]" Then
                        Return 51932
                    ElseIf str = "[Shift_JIS-2004]" Or str = "[CP2132004]" Then
                        Return 2132004
                    ElseIf str = "[EUC-JIS-2004]" Or str = "[CP512132004]" Then
                        Return 512132004
                    ElseIf str = "[eucJP-ms]" Or str = "[CP21220932]" Then
                        Return 21220932
                    ElseIf str = "[USER_CUSTOM]" Or str.Contains("CP") Then
                        Return My.Settings.usercp
                    End If
                End If
            End If

        End If

        If cp(0) = &H0 AndAlso cp(1) = &H0 AndAlso cp(2) = &HFE AndAlso cp(3) = &HFF Then
            My.Settings.MSCODEPAGE = 12001
        ElseIf cp(0) = &HFF AndAlso cp(1) = &HFE AndAlso cp(2) = &H0 AndAlso cp(3) = &H0 Then
            My.Settings.MSCODEPAGE = 12000
        ElseIf cp(0) = &HEF AndAlso cp(1) = &HBB AndAlso cp(2) = &HBF Then
            My.Settings.MSCODEPAGE = 65001
        ElseIf cp(0) = &HFE AndAlso cp(1) = &HFF Then
            My.Settings.MSCODEPAGE = 1201
        ElseIf cp(0) = &HFF AndAlso cp(1) = &HFE Then
            My.Settings.MSCODEPAGE = 1200
        End If

        For i = 0 To enctable.Length - 1
            If My.Settings.MSCODEPAGE = enctable(i) Then
                My.Settings.usercp = My.Settings.MSCODEPAGE
                Return My.Settings.MSCODEPAGE
            End If
        Next

        Dim k As Integer = My.Settings.MSCODEPAGE

        MessageBox.Show("対応してないエンコードのようです,デフォルトコードページ0にします")
        My.Settings.MSCODEPAGE = 0

        Return 0

    End Function

    ''' <summary>
    ''' 文字コードを判別する
    ''' </summary>
    ''' <remarks>
    ''' Jcode.pmのgetcodeメソッドを移植したものです。
    ''' Jcode.pm(http://openlab.ring.gr.jp/Jcode/index-j.html)
    ''' Jcode.pmのCopyright: Copyright 1999-2005 Dan Kogai
    ''' </remarks>
    ''' <param name="bytes">文字コードを調べるデータ</param>
    ''' <returns>適当と思われるEncodingオブジェクト。
    ''' 判断できなかった時はnull。</returns>
    Public Function GetCode(ByVal bytes As Byte()) As Integer
        Const bEscape As Byte = &H1B
        Const bAt As Byte = &H40
        Const bDollar As Byte = &H24
        Const bAnd As Byte = &H26
        Const bOpen As Byte = &H28 ''('
        Const bB As Byte = &H42
        Const bD As Byte = &H44
        Const bJ As Byte = &H4A
        Const bI As Byte = &H49
        Dim ctbl As New customtable

        Dim len As Integer = bytes.Length
        Dim b1 As Byte, b2 As Byte, b3 As Byte, b4 As Byte

        'Encode::is_utf8 は無視

        Dim isBinary As Boolean = False
        Dim i As Integer
        For i = 0 To len - 1
            b1 = bytes(i)
            If b1 <= &H6 OrElse b1 = &H7F OrElse b1 = &HFF Then
                ''binary'
                isBinary = True
                If b1 = &H0 AndAlso i < len - 1 AndAlso bytes(i + 1) <= &H7F Then
                    'smells like raw unicode
                    Return 1200
                End If
            End If
        Next
        If isBinary Then
            Return Nothing
        End If

        'not Japanese
        Dim notJapanese As Boolean = True
        For i = 0 To len - 1
            b1 = bytes(i)
            If b1 = bEscape OrElse &H80 <= b1 Then
                notJapanese = False
                Exit For
            End If
        Next
        If notJapanese Then
            Return 20127
        End If

        For i = 0 To len - 3
            b1 = bytes(i)
            b2 = bytes(i + 1)
            b3 = bytes(i + 2)

            If b1 = bEscape Then
                If b2 = bDollar AndAlso b3 = bAt Then
                    'JIS_0208 1978
                    'JIS
                    Return 50220
                ElseIf b2 = bDollar AndAlso b3 = bB Then
                    'JIS_0208 1983
                    'JIS
                    Return 50220
                ElseIf b2 = bOpen AndAlso (b3 = bB OrElse b3 = bJ) Then
                    'JIS_ASC
                    'JIS
                    Return 50220
                ElseIf b2 = bOpen AndAlso b3 = bI Then
                    'JIS_KANA
                    'JIS
                    Return 50220
                End If
                If i < len - 3 Then
                    b4 = bytes(i + 3)
                    If b2 = bDollar AndAlso b3 = bOpen AndAlso b4 = bD Then
                        'JIS_0212
                        'JIS
                        Return 50220
                    End If
                    If i < len - 5 AndAlso _
                        b2 = bAnd AndAlso b3 = bAt AndAlso b4 = bEscape AndAlso _
                        bytes(i + 4) = bDollar AndAlso bytes(i + 5) = bB Then
                        'JIS_0208 1990
                        'JIS
                        Return 50220
                    End If
                End If
            End If
        Next

        'should be euc|sjis|utf8
        'use of (?:) by Hiroki Ohzaki <ohzaki@iod.ricoh.co.jp>
        Dim enc As Integer() = {0, 0, 0, 0, 0, 0}
        Dim encode As String() = {"sjis", "euc", "utf8", "gbk", "big5", "uhc"}


        For i = 0 To len - 2
            b1 = bytes(i)
            b2 = bytes(i + 1)
            If ((&H81 <= b1 AndAlso b1 <= &H9F) OrElse _
                (&HE0 <= b1 AndAlso b1 <= &HFC)) AndAlso _
                ((&H40 <= b2 AndAlso b2 <= &H7E) OrElse _
                 (&H80 <= b2 AndAlso b2 <= &HFC)) Then
                'SJIS_C
                enc(0) += 2
                i += 1
            End If
        Next
        For i = 0 To len - 2
            b1 = bytes(i)
            b2 = bytes(i + 1)
            If ((&HA1 <= b1 AndAlso b1 <= &HFE) AndAlso _
                (&HA1 <= b2 AndAlso b2 <= &HFE)) OrElse _
                (b1 = &H8E AndAlso (&HA1 <= b2 AndAlso b2 <= &HDF)) Then
                'EUC_C
                'EUC_KANA
                enc(1) += 2
                i += 1
            ElseIf i < len - 2 Then
                b3 = bytes(i + 2)
                If b1 = &H8F AndAlso (&HA1 <= b2 AndAlso b2 <= &HFE) AndAlso _
                    (&HA1 <= b3 AndAlso b3 <= &HFE) Then
                    'EUC_0212
                    enc(1) += 3
                    i += 2
                End If
            End If
        Next
        For i = 0 To len - 2
            b1 = bytes(i)
            b2 = bytes(i + 1)
            If (&HC0 <= b1 AndAlso b1 <= &HDF) AndAlso _
                (&H80 <= b2 AndAlso b2 <= &HBF) Then
                'UTF8
                enc(2) += 2
                i += 1
            ElseIf i < len - 2 Then
                b3 = bytes(i + 2)
                If (&HE0 <= b1 AndAlso b1 <= &HEF) AndAlso _
                    (&H80 <= b2 AndAlso b2 <= &HBF) AndAlso _
                    (&H80 <= b3 AndAlso b3 <= &HBF) Then
                    'UTF8
                    enc(2) += 3
                    i += 2
                End If
            ElseIf i < len - 3 Then
                b3 = bytes(i + 2)
                b4 = bytes(i + 3)
                If (&HF0 <= b1 AndAlso b1 <= &HF7) AndAlso _
                    (&H80 <= b2 AndAlso b2 <= &HBF) AndAlso _
                    (&H80 <= b3 AndAlso b3 <= &HBF) AndAlso _
                    (&H80 <= b4 AndAlso b4 <= &HBF) Then
                    'UTF8
                    enc(2) += 4
                    i += 3
                End If
            End If
        Next
        For i = 0 To len - 2
            b1 = bytes(i)
            b2 = bytes(i + 1)
            If ((&H81 <= b1 AndAlso b1 <= &HFE) AndAlso _
                ((&H40 <= b2 AndAlso b2 <= &H7E) OrElse _
                 (&H80 <= b2 AndAlso b2 <= &HFE))) Then
                'GBK
                enc(3) += 2
                i += 1
            End If
        Next
        For i = 0 To len - 2
            b1 = bytes(i)
            b2 = bytes(i + 1)
            If ((&H88 <= b1 AndAlso b1 <= &HFE) AndAlso _
                ((&H40 <= b2 AndAlso b2 <= &H7E) OrElse _
                 (&HA1 <= b2 AndAlso b2 <= &HFE))) Then
                'big5HKSCS
                enc(4) += 2
                i += 1
            End If
        Next
        For i = 0 To len - 2
            b1 = bytes(i)
            b2 = bytes(i + 1)
            If ((&H81 <= b1 AndAlso b1 <= &HFE) AndAlso _
                ((&H41 <= b2 AndAlso b2 <= &H5A) OrElse _
                 (&H61 <= b2 AndAlso b2 <= &H7A) OrElse _
                 (&H81 <= b2 AndAlso b2 <= &HFE))) Then
                'EUCKR/UHC
                enc(5) += 2
                i += 1
            End If
        Next
        'M. Takahashi's suggestion
        'utf8 += utf8 / 2;

        'Dim sjis As Integer = enc(0)
        'Dim euc As Integer = enc(1)
        'Dim gbk As Integer = enc(3)
        'Dim big5 As Integer = enc(4)
        'Dim uhc As Integer = enc(5)

        Array.Sort(enc, encode)

        If encode(5) = "utf8" Then
            'UTF8
            Return 65001
        Else

            Dim enc2 As Integer() = {0, 0, 0, 0, 0, 0, 0}
            Dim encode2 As String() = {"sjis", "gbk", "big5", "euc", "sj2004", "eu2004", "eums"}

            Dim s As String = Encoding.GetEncoding(936).GetString(bytes)
            For i = 0 To s.Length - 1
                If s(i) = "?" Then
                    enc2(1) += 1
                ElseIf s(i) = "・" Then
                    enc2(1) += 1
                ElseIf s(i) = "□" Then
                    enc2(3) += 1
                End If
            Next

            s = Encoding.GetEncoding(51932).GetString(bytes)
            For i = 0 To s.Length - 1
                If s(i) = "?" Then
                    enc2(3) += 1
                ElseIf s(i) = "・" Then
                    enc2(3) += 1
                ElseIf s(i) = "□" Then
                    enc2(3) += 1
                End If
            Next

            s = ctbl.custom_pasrse(bytes, 951)
            For i = 0 To s.Length - 1
                If s(i) = "?" Then
                    enc2(2) += 1
                ElseIf s(i) = "・" Then
                    enc2(2) += 1
                ElseIf s(i) = "□" Then
                    enc2(2) += 1
                End If
            Next
            s = ctbl.custom_pasrse(bytes, 512132004)
            For i = 0 To s.Length - 1
                If s(i) = "?" Then
                    enc2(5) += 1
                ElseIf s(i) = "・" Then
                    enc2(5) += 1
                ElseIf s(i) = "□" Then
                    enc2(5) += 1
                End If
            Next
            s = ctbl.custom_pasrse(bytes, 21220932)
            For i = 0 To s.Length - 1
                If s(i) = "?" Then
                    enc2(5) += 1
                ElseIf s(i) = "・" Then
                    enc2(5) += 1
                ElseIf s(i) = "□" Then
                    enc2(5) += 1
                End If
            Next
            s = ctbl.custom_pasrse(bytes, 2132004)
            For i = 0 To s.Length - 1
                If s(i) = "?" Then
                    enc2(4) += 1
                ElseIf s(i) = "・" Then
                    enc2(4) += 1
                ElseIf s(i) = "□" Then
                    enc2(4) += 1
                End If
            Next
            s = Encoding.GetEncoding(932).GetString(bytes)
            For i = 0 To s.Length - 1
                If s(i) = "?" Then
                    enc2(0) += 1
                ElseIf s(i) = "・" Then
                    enc2(0) += 1
                ElseIf s(i) = "□" Then
                    enc2(0) += 1
                End If
            Next

            Array.Sort(enc2, encode2)
            'SJIS
            If encode2(0) = "sjis" Then
                Return 932
            End If

            If encode2(0) = "euc" Then
                'EUC
                Return 51932
            End If

            If encode2(0) = "gbk" Then
                'GBK
                Return 936
            End If
            
            If encode2(0) = "big5" Then
                'big5hk
                Return 951
            End If
            If encode2(0) = "eu2004" Then
                'EU24
                Return 512132004
            End If
            If encode2(0) = "eums" Then
                'EUms
                Return 21220932
            End If
            If encode2(0) = "sj2004" Then
                'sj24
                Return 2132004
            End If

        End If

        Return 0
    End Function

    Public Function check_db(ByVal filename As String, ByVal enc1 As Integer) As Boolean

        Dim file As New FileStream(filename, FileMode.Open, FileAccess.Read)
        Dim sr As New StreamReader(file, System.Text.Encoding.GetEncoding(enc1))
        Dim buffer As String = Nothing
        Dim cwc As New Regex("^_L [0-9A-Fa-f]{8} [0-9A-Fa-f]{4}", RegexOptions.ECMAScript)
        Dim gn As New Regex("^_S .?", RegexOptions.ECMAScript)
        Dim gid As New Regex("^_G .?", RegexOptions.ECMAScript)
        Dim cn As New Regex("^_C\d .?", RegexOptions.ECMAScript)
        Dim cwcm As Match
        Dim gnm As Match
        Dim gidm As Match
        Dim cnm As Match
        Dim cwcpop As Boolean = False
        Dim ct As Integer = 0
        Dim mode As Integer = 0

        Do Until ct = 100 Or sr.EndOfStream = True

            buffer = sr.ReadLine
            ct += 1
            Try
                If mode = 0 Then
                    gnm = gn.Match(buffer)
                    If gnm.Success Then
                        mode = 1
                    End If
                ElseIf mode = 1 Then
                    gidm = gid.Match(buffer)
                    If gidm.Success Then
                        mode = 2
                    End If
                ElseIf mode = 2 Then
                    cnm = cn.Match(buffer)
                    If cnm.Success Then
                        mode = 3
                    End If
                ElseIf mode = 3 Then
                    cwcm = cwc.Match(buffer)
                    If cwcm.Success Then
                        cwcpop = True
                        Exit Do
                    End If
                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        Loop

        file.Close()
        sr.Close()

        Return cwcpop

    End Function

    Public Function no_db(ByVal filename As String, ByVal enc1 As Integer) As Boolean

        Dim file As New FileStream(filename, FileMode.Open, FileAccess.Read)
        If enc1 > 655535 Or enc1 = 1201 Or enc1 = 951 Then
            enc1 = 0
        End If
        Dim sr As New StreamReader(file, System.Text.Encoding.GetEncoding(enc1))
        Dim buffer As String = Nothing
        Dim nodb As Boolean = True
        Dim cwc As New Regex("^_(L|M|N) 0x[0-9A-Fa-f]{8} 0x[0-9A-Fa-f]{8}", RegexOptions.ECMAScript)
        Dim gn As New Regex("^_S .?", RegexOptions.ECMAScript)
        Dim gid As New Regex("^_G .?", RegexOptions.ECMAScript)
        Dim cn As New Regex("^_C\d .?", RegexOptions.ECMAScript)
        Dim cwcm As Match
        Dim gnm As Match
        Dim gidm As Match
        Dim cnm As Match
        Dim ct As Integer = 0
        Dim mode As Integer = 0

        Do Until ct = 100 Or sr.EndOfStream = True

            buffer = sr.ReadLine
            buffer = buffer.PadRight(2)
            ct += 1
            Try
                If mode = 0 Then
                    gnm = gn.Match(buffer)
                    If gnm.Success Then
                        mode = 1
                    End If
                ElseIf mode = 1 Then
                    gidm = gid.Match(buffer)
                    If gidm.Success Then
                        mode = 2
                    End If
                ElseIf mode = 2 Then
                    cnm = cn.Match(buffer)
                    If cnm.Success Then
                        mode = 3
                    End If
                ElseIf mode = 3 Then
                    cwcm = cwc.Match(buffer)
                    If cwcm.Success Then
                        nodb = False
                        Exit Do
                    End If

                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        Loop

        If nodb = True Then
            MessageBox.Show("どのコードフォーマットに該当しませんでした。CWC形式ならば" & vbCrLf & _
                            "_S ゲーム名" & vbCrLf & _
                            "_G GAMEID" & vbCrLf & _
                            "_C0 コード名(_C1だとコードON)" & vbCrLf & _
                            "_L 0x12345678 0x12345678 (POPS時,_L 12345678 1234)" & vbCrLf & _
                            "#コメント行" & vbCrLf & _
                            "_の順番どおりになっているか確認して下さい。" & vbCrLf & _
                            "PROACTIONREPLAYやCODEFREAK専用のバイナリ形式は自動で判断されます。", "コード形式不明")
        End If

        file.Close()
        sr.Close()
        Return nodb
    End Function

    Public Function check2_db(ByVal filename As String, ByVal enc1 As Integer) As Boolean

        Dim file As New FileStream(filename, FileMode.Open, FileAccess.Read)
        Dim cf As Boolean = False
        Dim Code(1) As Byte
        Dim bs(1) As Byte
        If file.ReadByte = &H47 And file.Length Mod 2 = 0 And file.Length > 54 Then
            file.Seek(file.Length - 36, SeekOrigin.Begin)
            file.Read(Code, 0, 2)
            file.Seek(file.Length - 2, SeekOrigin.Begin)
            file.Read(bs, 0, 2)
            If Code(0) = &H43 And Code(1) = &H20 And bs(0) = 10 And bs(1) = 10 Then
                cf = True
            End If
        End If

        file.Close()

        Return cf
    End Function

    Public Function check3_db(ByVal filename As String, ByVal enc1 As Integer) As Boolean

        Dim file As New FileStream(filename, FileMode.Open, FileAccess.Read)
        Dim DATEL_AR As Boolean = False
        Dim Code(CInt(file.Length)) As Byte
        Dim binsize(3) As Byte
        Dim binstr(3) As Byte
        Dim arheader As String = Nothing
        Dim size As Integer = 0
        Dim hex As UInteger = 0
        Dim hash(1) As String
        Dim digit(1) As String
        Dim z As UInteger = 0
        If file.ReadByte = &H50 Then
            file.Seek(0, SeekOrigin.Begin)
            file.Read(Code, 0, CInt(file.Length))
            arheader = Encoding.GetEncoding(0).GetString(Code)
            arheader = arheader.Substring(0, 8)
            Array.ConstrainedCopy(Code, 16, binsize, 0, 4)
            size = BitConverter.ToInt32(binsize, 0)
            If arheader = "PSPARC01" Then 'AndAlso size = file.Length Then
                'ARCを抜いたへっだのはっしゅ
                Array.ConstrainedCopy(Code, 8, binstr, 0, 4)
                hex = BitConverter.ToUInt32(binstr, 0)
                digit(1) = hex.ToString("X")

                'コード部ばいなりのはっしゅ
                Array.ConstrainedCopy(Code, 12, binstr, 0, 4)
                hex = BitConverter.ToUInt32(binstr, 0)
                digit(0) = hex.ToString("X")

                'JADでおｋ
                z = datel_hash(Code, 12, 16)
                hash(1) = Convert.ToString(z, 16).ToUpper

                z = datel_hash(Code, 28, size)
                hash(0) = Convert.ToString(z, 16).ToUpper

                'z = datel_hash(Code, &HF9B4 + 16 + &HB4, &HB4)
                'Dim h As String = Convert.ToString(z, 16).ToUpper

                'PAPARX01有りがあわないようなのでチェック修正
                If hash(0) = digit(0) AndAlso hash(1) = digit(1) Then
                    DATEL_AR = True
                End If
            End If
        End If

        file.Close()

        Return DATEL_AR
    End Function

    Public Function datel_hash(ByVal bin() As Byte, ByVal s As Integer, ByVal w As Integer) As UInteger

        'FNC_00010400:					# int datelhash(int fileaddress_a0,int hashrange_a1)
        '	beq		a1, zero, $00010440	# 00010400:10a0000f	▼__00010440:DATELHASH
        '	addu	v1, zero, zero		# 00010404:00001821	
        '	lui		t0, $1000	    	# 00010408:3c081000	t0=$10000000
        '	lbu		v0, $0000(a0)		# 0001040c:90820000	値をロード
        '__00010410:					# 
        '	addiu	a0, a0, $0001		# 00010410:24840001	アドレス+1*
        '	addu	a3, v0, v1		    # 00010414:00433821	a3=値+前の計算結果v1
        '	srl		v1, a3, 1		    # 00010418:00071842	v1=a3>>1
        '	andi	v0, a3, $0001		# 0001041c:30e20001	a3奇数偶数判定
        '	bne		v0, zero, $0001042c	# 00010420:14400002	▼__0001042c
        '	or		v1, v1, t0		    # 00010424:00681825	a3が奇数の時はv1=(a3>>1) | 0x10000000
        '	srl		v1, a3, 1		    # 00010428:00071842	a3が偶数の時はv1=a3>>1
        '__0001042c:					# 
        '	addiu	a1, a1, $ffff   	# 0001042c:24a5ffff	範囲回数-1
        '	bnel	a1, zero, $00010410	# 00010430:54a0fff7	▲__00010410
        '	lbu		v0, $0000(a0)		# 00010434:90820000	*+1hの値をロード
        '	jr		r       a			# 00010438:03e00008	
        '	xor		v0, a2, v1  		# 0001043c:00c31026	

        Dim v1 As UInteger = 0
        Dim v0 As UInteger = 0
        Dim a3 As UInteger = 0
        Dim t0 As UInteger = &H10000000
        Dim a2 As UInteger = &H17072008
        Dim i As Integer = 0
        For i = 0 To w - 1
            v0 = Convert.ToUInt32(bin(s + i))
            a3 = v0 + v1
            v1 = a3 >> 1
            If ((a3 And 1) <> 0) Then
                v1 = v1 Or t0
            End If
        Next
        v0 = a2 Xor v1
        Return v0

        'http://www.playarts.co.jp/psptool/download.php　PLAYATRS one
        'http://www.varaneckas.com/jad JADED with playarts tool
        'Dim z As UInteger = 0
        'Dim y As UInteger = &H20000000
        'Dim x As UInteger = &H17072008
        'Dim i As Integer = 0
        'For i = 0 To w-1
        '    z += Convert.ToUInt32(bin(s+i))
        '    If ((z And 1) = 1) Then
        '        z += y
        '    End If
        '    z >>= 1
        'next
        'z = z Xor x
        'Return z
    End Function

End Class