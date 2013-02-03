Imports System
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

Public Class save_db


    Dim strenc As String() = {"[Shift_JIS,Windows-31J]", "[GBK]", "[Big5-HKSCS]", "[UHC]", "[EUC-JP]", "[Shift_JIS-2004]", "[EUC-JIS-2004]", "[eucJP-ms]", "[UTF16LE]", "[UTF16BE]", "[UTF32LE]", "[UTF32BE]", "[UTF-8]", "[USER_CUSTOM]"}

    Dim unitable As String() = {"table\custom_utf32", "table\custom_utf32_2", "table\custom_utf32_3", "table\custom_utf32_4"}

    Public Function selstr(ByVal enc1 As Integer) As String
        Dim s As String = ""
        If My.Settings.savetype = False Then
            Select Case enc1
                Case 932
                    s = strenc(0)
                Case 936
                    s = strenc(1)
                Case 951
                    s = strenc(2)
                Case 949
                    s = strenc(3)
                Case 51932
                    s = strenc(4)
                Case 2132004
                    s = strenc(5)
                Case 512132004
                    s = strenc(6)
                Case 21220932
                    s = strenc(7)
                Case 1201
                    s = strenc(9)
                Case Else
                    s = strenc(13)
            End Select
        Else
            s = "[CP" & enc1.ToString & "]"
        End If

        Return s & vbCrLf

    End Function

    Public Sub save_cwcheat(ByVal filename As String, ByVal enc1 As Integer)

        Dim m As MERGE = MERGE
        Dim errorct As Integer = 0
        Dim buffer As String()
        Dim errors As Boolean = False
        Dim enc As Boolean = My.Settings.cfid
        Dim encok As Boolean = False

        Dim ew As error_window = error_window
        reset_errors() ' Clear prior save errors if any

        If enc1 = 512132004 Or enc1 = 2132004 Or enc1 = 951 Or enc1 = 21220932 Then
            Dim sel As Integer = 0
            If enc1 = 512132004 Then
                sel = 1
            ElseIf enc1 = 951 Then
                sel = 2
            ElseIf enc1 = 21220932 Then
                sel = 3
            End If
            If File.Exists(unitable(sel)) = True Then
                Dim ctbl As New customtable
                Dim str As String = ""

                Dim tw As New FileStream(filename, FileMode.Create, FileAccess.Write)
                Dim cwcar As String = "_L "
                Dim GID As String = ""
                Dim bs As Byte()
                Dim tfs As New FileStream(unitable(sel), FileMode.Open, FileAccess.Read)
                Dim tbl(CInt(tfs.Length - 1)) As Byte
                tfs.Read(tbl, 0, tbl.Length)
                tfs.Close()
                If My.Settings.saveencode = True Then
                    str = selstr(enc1)
                    bs = ctbl.unicode2custom(str, tbl, sel)
                    tw.Write(bs, 0, bs.Length)
                End If

                Try

                    For Each n As TreeNode In m.codetree.Nodes(0).Nodes

                        GID = n.Tag.ToString.Trim
                        encok = False

                        If GID.Length < 10 Then
                            GID = GID.PadRight(10, CChar("0"))
                        End If
                        str = "_S " & GID.Substring(0, 10) & vbCrLf
                        bs = ctbl.unicode2custom(str, tbl, sel)
                        tw.Write(bs, 0, bs.Length)
                        str = "_G " & n.Text.Trim & vbCrLf

                        If GID.Length = 13 Then
                            'str &= "#CFMODE" & GID.Substring(10, 3) & vbCrLf
                            If Regex.IsMatch(GID, "^[a-zA-Z\-]{5}[0-9A-Fa-f]{8}", RegexOptions.ECMAScript) Then
                                If enc = False AndAlso (Convert.ToInt32(GID.Substring(10, 3), 16) And &H800) = 0 Then
                                    encok = True
                                    str &= ("#CFMODE" & GID.Substring(10, 3) & "->CWCMODE8" & GID.Substring(11, 2) & vbCrLf)
                                Else
                                    str &= ("#CWCMODE" & GID.Substring(10, 3) & vbCrLf)
                                End If
                            End If
                        End If

            bs = ctbl.unicode2custom(Str, tbl, sel)
            tw.Write(bs, 0, bs.Length)

            For Each n1 As TreeNode In n.Nodes

                If n1.Tag Is Nothing Then

                    If n1.Tag.ToString.Substring(0, 1) = "0" Then
                        Str = "_C0 " & n1.Text.Trim & vbCrLf
                        bs = ctbl.unicode2custom(Str, tbl, sel)
                        tw.Write(bs, 0, bs.Length)
                    Else
                        Str = "_C0 " & n1.Text.Trim & vbCrLf
                        bs = ctbl.unicode2custom(Str, tbl, sel)
                        tw.Write(bs, 0, bs.Length)
                    End If
                ElseIf enc = True AndAlso n1.Index = 0 AndAlso n1.Text = "(M)" Then
                    Dim hex As Regex = New Regex("^0x[0-9A-Fa-f]{8} 0x[0-9A-Fa-f]{8}", RegexOptions.ECMAScript)
                    Dim mh As Match = hex.Match(n1.Tag.ToString)
                    If mh.Success AndAlso (Convert.ToInt32(mh.Value.Remove(0, 13), 16) And &H800) = 0 Then
                        Str = "_E " & mh.Value & vbCrLf
                        bs = ctbl.unicode2custom(Str, tbl, sel)
                        tw.Write(bs, 0, bs.Length)
                    End If

                Else

                    buffer = n1.Tag.ToString.Split(CChar(vbLf))

                    For Each s As String In buffer

                        s = s.Trim()


                        If s.Length = 1 Then
                            If s = "0" Or s = "2" Or s = "4" Then
                                If s = "0" Then
                                    cwcar = "_L "
                                ElseIf s = "2" Then
                                    cwcar = "_M "
                                ElseIf s = "4" Then
                                    cwcar = "_N "
                                End If
                                Str = "_C0 " & n1.Text.Trim & vbCrLf
                                bs = ctbl.unicode2custom(Str, tbl, sel)
                                tw.Write(bs, 0, bs.Length)
                            ElseIf s = "1" Or s = "3" Or s = "5" Then
                                If s = "1" Then
                                    cwcar = "_L "
                                ElseIf s = "3" Then
                                    cwcar = "_M "
                                ElseIf s = "5" Then
                                    cwcar = "_N "
                                End If

                                Str = "_C1 " & n1.Text.Trim & vbCrLf
                                bs = ctbl.unicode2custom(Str, tbl, sel)
                                tw.Write(bs, 0, bs.Length)
                            End If
                        ElseIf s.Length > 2 Then

                            If s.Contains("#") Then
                                Str = s.Trim & vbCrLf
                                bs = ctbl.unicode2custom(Str, tbl, sel)
                                If bs.Length > 2 Then
                                    tw.Write(bs, 0, bs.Length)
                                End If

                            Else
                                '0x00000000 0x00000000
                                If Regex.IsMatch(s, "^0x[0-9A-Fa-f]{8} 0x[0-9A-Fa-f]{8}", RegexOptions.ECMAScript) = False AndAlso s.Trim <> "" Then
                                    ' Error, code length was incorrect
                                    errorct += 1
                                    write_errors(errorct, n.Text.Trim, n1.Text.Trim, "不正なコード形式です: " & s.Trim)
                                    errors = True
                                End If

                                If encok = True Then
                                    s = cfdecript(s)
                                End If

                                Str = cwcar & s.Trim & vbCrLf
                                bs = ctbl.unicode2custom(Str, tbl, sel)
                                tw.Write(bs, 0, bs.Length)


                            End If

                        End If

                    Next

                End If

            Next

                    Next

                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try

                tw.Close()
            Else
                MessageBox.Show(unitable(sel) & "がありません,GOOGLECODEからダウンロードして下さい")
            End If

        Else


            Dim tw As New StreamWriter(filename, False, System.Text.Encoding.GetEncoding(enc1))
            Dim cwcar As String = "_L "
            Dim b1 As String = Nothing
            Dim GID As String = ""

            reset_errors() ' Clear prior save errors if any

            If My.Settings.saveencode = True Then
                tw.Write(selstr(enc1))
            End If

            Try

                For Each n As TreeNode In m.codetree.Nodes(0).Nodes

                    GID = n.Tag.ToString.Trim
                    encok = False
                    If GID.Length < 10 Then
                        GID = GID.PadRight(10, CChar("0"))
                    End If
                    tw.Write("_S " & GID.Substring(0, 10) & vbCrLf)
                    tw.Write("_G " & n.Text.Trim & vbCrLf)

                    If GID.Length = 13 Then
                        If Regex.IsMatch(GID, "^[a-zA-Z\-]{5}[0-9A-Fa-f]{8}", RegexOptions.ECMAScript) Then
                            If enc = False AndAlso (Convert.ToInt32(GID.Substring(10, 3), 16) And &H800) = 0 Then
                                encok = True
                                tw.Write("#CFMODE" & GID.Substring(10, 3) & "->CWCMODE8" & GID.Substring(11, 2) & vbCrLf)
                            Else
                                tw.Write("#CWCMODE" & GID.Substring(10, 3) & vbCrLf)
                            End If
                        End If
                    End If

                    For Each n1 As TreeNode In n.Nodes

                        If n1.Tag Is Nothing Then

                            If n1.Tag.ToString.Substring(0, 1) = "0" Then
                                tw.Write("_C0 " & n1.Text.Trim & vbCrLf)
                            Else
                                tw.Write("_C1 " & n1.Text.Trim & vbCrLf)
                            End If
                            ' If the code title had no actual codes, don't save it
                            'i += 1
                            'write_errors(i, n.Text.Trim, n1.Text.Trim, "Error:  Code title contained no codes, not saved.")
                            'errors = True

                            'ElseIf n1.Tag.ToString.Trim >= "0" Or n1.Tag.ToString.Trim <= "5" Then

                            '    If n1.Tag.ToString.Substring(0, 1) = "0" Then
                            '        tw.Write("_C0 " & n1.Text.Trim & vbCrLf)
                            '    Else
                            '        tw.Write("_C1 " & n1.Text.Trim & vbCrLf)
                            '    End If
                            '    ' If the code title had no actual codes, don't save it
                            '    'i += 1
                            '    'write_errors(i, n.Text.Trim, n1.Text.Trim, "Error:  Code title contained no codes, not saved.")
                            '    'errors = True
                        ElseIf enc = True AndAlso n1.Index = 0 AndAlso n1.Text = "(M)" Then
                            Dim hex As Regex = New Regex("0x[0-9A-Fa-f]{8} 0x[0-9A-Fa-f]{8}", RegexOptions.ECMAScript)
                            Dim mh As Match = hex.Match(n1.Tag.ToString)
                            If mh.Success AndAlso (Convert.ToInt32(mh.Value.Remove(0, 13), 16) And &H800) = 0 Then
                                tw.Write("_E " & mh.Value & vbCrLf)
                            End If
                        Else

                            buffer = n1.Tag.ToString.Split(CChar(vbLf))

                            For Each s As String In buffer
                                s = s.Trim()

                                If s.Length = 1 Then
                                    If s = "0" Or s = "2" Or s = "4" Then
                                        If s = "0" Then
                                            cwcar = "_L "
                                        ElseIf s = "2" Then
                                            cwcar = "_M "
                                        ElseIf s = "4" Then
                                            cwcar = "_N "
                                        End If
                                        tw.Write("_C0 " & n1.Text.Trim & vbCrLf)
                                    ElseIf s = "1" Or s = "3" Or s = "5" Then
                                        If s = "1" Then
                                            cwcar = "_L "
                                        ElseIf s = "3" Then
                                            cwcar = "_M "
                                        ElseIf s = "5" Then
                                            cwcar = "_N "
                                        End If
                                        tw.Write("_C1 " & n1.Text.Trim & vbCrLf)
                                    End If
                                ElseIf s.Length > 2 Then
                                    If s.Contains("#") Then

                                        tw.Write(s & vbCrLf)

                                    Else
                                        '0x00000000 0x00000000
                                        If Regex.IsMatch(s, "^0x[0-9A-Fa-f]{8} 0x[0-9A-Fa-f]{8}", RegexOptions.ECMAScript) = False AndAlso s.Trim <> "" Then
                                            ' Error, code length was incorrect
                                            errorct += 1
                                            write_errors(errorct, n.Text.Trim, n1.Text.Trim, "不正なコード形式です: " & s.Trim)
                                            errors = True
                                        End If

                                        If encok = True Then
                                            s = cfdecript(s)
                                        End If

                                        tw.Write(cwcar & s & vbCrLf)


                                    End If

                                End If

                            Next

                        End If

                    Next

                Next

            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try

            tw.Close()
        End If



        If errorct > 0 Then
            ew.Show()
            ew.tab_error.SelectedIndex = 1
            m.Focus()
            reset_toolbar()
        End If

    End Sub

    Public Sub save_psx(ByVal filename As String, ByVal enc1 As Integer)

        Dim m As MERGE = MERGE
        Dim buffer As String()
        Dim errors As Boolean = False
        Dim ew As error_window = error_window
        Dim errorct As Integer = 0
        reset_errors() ' Clear prior save errors if any


        If enc1 = 512132004 Or enc1 = 2132004 Or enc1 = 951 Or enc1 = 21220932 Then
            Dim sel As Integer = 0
            If enc1 = 512132004 Then
                sel = 1
            ElseIf enc1 = 951 Then
                sel = 2
            ElseIf enc1 = 21220932 Then
                sel = 3
            End If

            If File.Exists(unitable(sel)) = True Then
                Dim ctbl As New customtable
                Dim str As String = ""
                Dim cwcar As String = "_L "
                Dim tw As New FileStream(filename, FileMode.Create, FileAccess.Write)
                Dim bs As Byte()
                Dim tfs As New FileStream(unitable(sel), FileMode.Open, FileAccess.Read)
                Dim tbl(CInt(tfs.Length - 1)) As Byte
                tfs.Read(tbl, 0, tbl.Length)
                tfs.Close()
                If My.Settings.saveencode = True Then
                    str = selstr(enc1)
                    bs = ctbl.unicode2custom(str, tbl, sel)
                    tw.Write(bs, 0, bs.Length)
                End If
                Dim code As String = Nothing
                reset_errors() ' Clear prior save errors if any

                For Each n As TreeNode In m.codetree.Nodes(0).Nodes

                    'tw.Write("_S " & n.Tag.ToString.Trim & vbCrLf)
                    'tw.Write("_G " & n.Text.Trim & vbCrLf)
                    str = "_S " & n.Tag.ToString.Trim & vbCrLf
                    bs = ctbl.unicode2custom(str, tbl, sel)
                    tw.Write(bs, 0, bs.Length)
                    str = "_G " & n.Text.Trim & vbCrLf
                    bs = ctbl.unicode2custom(str, tbl, sel)
                    tw.Write(bs, 0, bs.Length)

                    For Each n1 As TreeNode In n.Nodes

                        If n1.Tag Is Nothing Then

                            If n1.Tag.ToString.Substring(0, 1) = "0" Then
                                'tw.Write("_C0 " & n1.Text.Trim & vbCrLf)
                                str = "_C0 " & n1.Text.Trim & vbCrLf
                                bs = ctbl.unicode2custom(str, tbl, sel)
                                tw.Write(bs, 0, bs.Length)
                            Else

                                'tw.Write("_C1 " & n1.Text.Trim & vbCrLf)
                                str = "_C1 " & n1.Text.Trim & vbCrLf
                                bs = ctbl.unicode2custom(str, tbl, sel)
                                tw.Write(bs, 0, bs.Length)
                            End If
                            ' If the code title had no actual codes, don't save it
                            'i += 1
                            'write_errors(i, n.Text.Trim, n1.Text.Trim, "Error:  Code title contained no codes, not saved.")
                            'errors = True

                        ElseIf n1.Tag.ToString.Trim = "0" Or n1.Tag.ToString.Trim = "1" Then

                            If n1.Tag.ToString.Substring(0, 1) = "0" Then
                                'tw.Write("_C0 " & n1.Text.Trim & vbCrLf)
                                str = "_C0 " & n1.Text.Trim & vbCrLf
                                bs = ctbl.unicode2custom(str, tbl, sel)
                                tw.Write(bs, 0, bs.Length)
                            Else
                                'tw.Write("_C1 " & n1.Text.Trim & vbCrLf)
                                str = "_C1 " & n1.Text.Trim & vbCrLf
                                bs = ctbl.unicode2custom(str, tbl, sel)
                                tw.Write(bs, 0, bs.Length)
                            End If
                            ' If the code title had no actual codes, don't save it
                            'i += 1
                            'write_errors(i, n.Text.Trim, n1.Text.Trim, "Error:  Code title contained no codes, not saved.")
                            'errors = True

                        Else

                            If n1.Tag.ToString.Substring(0, 1) = "0" Then
                                'tw.Write("_C0 " & n1.Text.Trim & vbCrLf)
                                str = "_C0 " & n1.Text.Trim & vbCrLf
                                bs = ctbl.unicode2custom(str, tbl, sel)
                                tw.Write(bs, 0, bs.Length)
                            Else
                                'tw.Write("_C1 " & n1.Text.Trim & vbCrLf)
                                str = "_C1 " & n1.Text.Trim & vbCrLf
                                bs = ctbl.unicode2custom(str, tbl, sel)
                                tw.Write(bs, 0, bs.Length)
                            End If


                            buffer = n1.Tag.ToString.Split(CChar(vbLf))

                            For Each s As String In buffer
                                s = s.Trim
                                If s.Length > 1 Then

                                    If s.Contains("#") Then

                                        'tw.Write(s & vbCrLf)
                                        str = s & vbCrLf
                                        bs = ctbl.unicode2custom(str, tbl, sel)
                                        If bs.Length > 2 Then
                                            tw.Write(bs, 0, bs.Length)
                                        End If

                                    Else
                                        If Regex.IsMatch(s, "^[0-9A-Fa-f]{8} [0-9A-Fa-f?]{4}", RegexOptions.ECMAScript) = False AndAlso s <> "" Then
                                            ' Error, code length was incorrect
                                            errorct += 1
                                            write_errors(errorct, n.Text.Trim, n1.Text.Trim, "不正なコード形式です: " & s)
                                            errors = True
                                        End If

                                        'tw.Write("_L " & s & vbCrLf)
                                        str = "_L " & s & vbCrLf
                                        bs = ctbl.unicode2custom(str, tbl, sel)
                                        tw.Write(bs, 0, bs.Length)

                                    End If

                                End If

                            Next

                        End If

                    Next

                Next

                tw.Close()

            Else
                MessageBox.Show(unitable(sel) & "がありません,GOOGLECODEからダウンロードして下さい")
            End If

        Else

            Dim tw As New StreamWriter(filename, False, Encoding.GetEncoding(enc1))
            Dim code As String = Nothing
            

            If My.Settings.saveencode = True Then
                tw.Write(selstr(enc1))
            End If

            reset_errors() ' Clear prior save errors if any

            For Each n As TreeNode In m.codetree.Nodes(0).Nodes

                tw.Write("_S " & n.Tag.ToString.Trim & vbCrLf)
                tw.Write("_G " & n.Text.Trim & vbCrLf)

                For Each n1 As TreeNode In n.Nodes

                    If n1.Tag Is Nothing Then

                        If n1.Tag.ToString.Substring(0, 1) = "0" Then
                            tw.Write("_C0 " & n1.Text.Trim & vbCrLf)
                        Else

                            tw.Write("_C1 " & n1.Text.Trim & vbCrLf)
                        End If
                        ' If the code title had no actual codes, don't save it
                        'i += 1
                        'write_errors(i, n.Text.Trim, n1.Text.Trim, "Error:  Code title contained no codes, not saved.")
                        'errors = True

                    ElseIf n1.Tag.ToString.Trim = "0" Or n1.Tag.ToString.Trim = "1" Then

                        If n1.Tag.ToString.Substring(0, 1) = "0" Then
                            tw.Write("_C0 " & n1.Text.Trim & vbCrLf)
                        Else
                            tw.Write("_C1 " & n1.Text.Trim & vbCrLf)
                        End If
                        ' If the code title had no actual codes, don't save it
                        'i += 1
                        'write_errors(i, n.Text.Trim, n1.Text.Trim, "Error:  Code title contained no codes, not saved.")
                        'errors = True

                    Else

                        If n1.Tag.ToString.Substring(0, 1) = "0" Then
                            tw.Write("_C0 " & n1.Text.Trim & vbCrLf)
                        Else
                            tw.Write("_C1 " & n1.Text.Trim & vbCrLf)
                        End If


                        buffer = n1.Tag.ToString.Split(CChar(vbLf))

                        For Each s As String In buffer

                            s = s.Trim

                            If s.Length > 1 Then


                                If s.Contains("#") Then

                                    tw.Write(s & vbCrLf)

                                Else
                                    If Regex.IsMatch(s, "^[0-9A-Fa-f]{8} [0-9A-Fa-f?]{4}", RegexOptions.ECMAScript) = False AndAlso s <> "" Then
                                        ' Error, code length was incorrect
                                        errorct += 1
                                        write_errors(errorct, n.Text.Trim, n1.Text.Trim, "不正なコード形式です: " & s)
                                        errors = True
                                    End If

                                    tw.Write("_L " & s & vbCrLf)


                                End If

                            End If

                        Next

                    End If

                Next

            Next

            tw.Close()
        End If

        If errorct > 0 Then
            ew.Show()
            ew.tab_error.SelectedIndex = 1
            m.Focus()
            reset_toolbar()
        End If

    End Sub

    Public Sub save_cf(ByVal filename As String, ByVal enc1 As Integer)

        Dim m As MERGE = MERGE
        Dim ctree As TreeView = m.codetree
        Dim i As Integer = 0
        Dim buffer As String()
        Dim fs As New System.IO.FileStream(filename, FileMode.Create, FileAccess.Write)
        Dim cp1201len As Integer = 0
        Dim bs(3 * 1024 * 1024) As Byte '３Mばいとぐらい
        Dim cfutf16be(34) As Byte
        Dim nullcode As Boolean = False
        Dim dummy As Byte() = Encoding.GetEncoding(1201).GetBytes("0000000000000000")
        Dim z As Integer = 0
        Dim notcfname As Byte() = Encoding.GetEncoding(1201).GetBytes("(PSPAR/TEMP)")
        Dim notcflen As Integer = notcfname.Length

        Dim gname As String = ""
        Dim ccname As String = ""
        Dim name() As Byte = Nothing
        Dim cname() As Byte = Nothing
        Dim mode As String = ""
        Dim gid As String = ""
        Dim bytesData(3) As Byte
        Dim sce As Integer() = {0, 0, 0, 0}
        Dim cfmax As Integer = 0
        Dim namebak As Integer = 0
        Dim cfidbk As Integer = 0
        Dim gidst As String = ""
        Dim cfids As Regex = New Regex("0x[0-9A-Fa-f]{8} 0x[0-9A-Fa-f]{8}")
        Dim cfim As Match
        Dim s1 As String = ""
        Dim notcf As Boolean = False
        Dim errors As Boolean = False
        Dim errorct As Integer = 0
        Dim ew As error_window = error_window
        reset_errors() ' Clear prior save errors if any

        Try
            If m.codetree.Nodes(0).Nodes.Count = 0 Then
                Dim newgame As New TreeNode
                With newgame
                    .Name = "新規ゲーム"
                    .Text = "新規ゲーム"
                    .ImageIndex = 1
                    .Tag = "ULJS-00000"
                End With
                m.codetree.Nodes(0).Nodes.Insert(0, newgame)
            End If

            For Each n As TreeNode In m.codetree.Nodes(0).Nodes

                If m.codetree.Nodes(0).Nodes(z).Nodes.Count = 0 Then
                    Dim newcode As New TreeNode
                    With newcode
                        .ImageIndex = 2
                        .SelectedImageIndex = 3
                        .Name = "新規コード"
                        .Text = "新規コード"
                        .Tag = "0"
                    End With
                    m.codetree.Nodes(0).Nodes(z).Nodes.Insert(0, newcode)
                End If
                z += 1
                gname = n.Text
                gidst = n.Tag.ToString


                If gidst.Length < 10 Then
                    gidst = gidst.PadRight(10, CChar("0"))
                End If
                gid = gidst.Remove(4, 1)
                'ASCIIとして文字列に変換
                bytesData = Encoding.GetEncoding(1252).GetBytes(gid)
                gid = cvtsceid2cf(bytesData)
                If gidst.Length = 13 Then
                    gid &= gidst.Remove(0, 5) 'CFID
                Else
                    gid &= gidst.Remove(0, 5) & "820" 'CWC生コードモード
                End If

                If nullcode = True Then

                    If notcf = True Then
                        i -= 2
                        Array.ConstrainedCopy(notcfname, 0, bs, i, notcflen)
                        i += notcflen
                        bs(i) = 10
                        bs(i + 1) = 10
                        i += 2
                        notcf = False
                    Else
                        errorct += 1
                        write_errors(errorct, n.Text.Trim, ccname, "コード内容が空なので代替ダミーが追加されます")
                    End If
                    errors = True

                    bs(i) = &H43 'C コード内容
                    bs(i + 1) = &H20
                    i += 2
                    Array.ConstrainedCopy(dummy, 0, bs, i, 32)
                    i += 32
                    bs(i) = 10
                    bs(i + 1) = 10
                    i += 2
                    nullcode = False

                End If

                    bs(i) = &H47 'G ゲームタイトル
                    bs(i + 1) = &H20
                    i += 2
                    cp1201len = gname.Length * 2
                    Array.Resize(name, cp1201len)
                    name = Encoding.GetEncoding(1201).GetBytes(gname)
                    Array.ConstrainedCopy(name, 0, bs, i, cp1201len)
                    i += cp1201len
                    bs(i) = 10
                    bs(i + 1) = 10
                    i += 2
                    bs(i) = &H4D    'M ゲームID
                bs(i + 1) = &H20
                i += 2
                cfidbk = i
                    cp1201len = gid.Length * 2
                    cfutf16be = Encoding.GetEncoding(1201).GetBytes(gid)
                    Array.ConstrainedCopy(cfutf16be, 0, bs, i, cp1201len)
                    i += cp1201len
                    bs(i) = 10
                    bs(i + 1) = 10
                    i += 2

                    For Each n1 As TreeNode In n.Nodes

                    If nullcode = True Then
                        
                        If notcf = True Then
                            i -= 2
                            Array.ConstrainedCopy(notcfname, 0, bs, i, notcflen)
                            i += notcflen
                            bs(i) = 10
                            bs(i + 1) = 10
                            i += 2
                            notcf = False
                        Else
                            errorct += 1
                            write_errors(errorct, n.Text.Trim, ccname, "コード内容が空なので代替ダミーが追加されます")
                        End If

                        bs(i) = &H43 'C コード内容
                        bs(i + 1) = &H20
                        i += 2
                        Array.ConstrainedCopy(dummy, 0, bs, i, 32)
                        i += 32
                        bs(i) = 10
                        bs(i + 1) = 10
                        i += 2

                        nullcode = False
                    End If

                    If n1.Index = 0 AndAlso n1.Text = "(M)" Then
                        cfim = cfids.Match(n1.Tag.ToString)
                        If cfim.Success Then
                            s1 = cfim.Value
                            s1 = s1.Replace("0x", "")
                            s1 = s1.Replace(" ", "")
                            cfutf16be = Encoding.GetEncoding(1201).GetBytes(s1)
                            Array.ConstrainedCopy(cfutf16be, 0, bs, cfidbk, 32)
                        End If
                    Else
                        bs(i) = &H44 'D コード名
                        bs(i + 1) = &H20
                        i += 2
                        mode = n1.Tag.ToString.Substring(0, 1)

                        ccname = n1.Text.Trim
                        cp1201len = ccname.Length * 2
                        Array.Resize(cname, cp1201len)
                        cname = Encoding.GetEncoding(1201).GetBytes(ccname)
                        Array.ConstrainedCopy(cname, 0, bs, i, cp1201len)
                        i += cp1201len
                        namebak = cp1201len
                        bs(i) = 10
                        bs(i + 1) = 10
                        i += 2
                        nullcode = True
                        cfmax = 0

                        If mode = "0" Or mode = "1" Then
                            buffer = n1.Tag.ToString.Split(CChar(vbLf))

                            For Each s As String In buffer

                                If s.Length > 2 Then
                                    s = s.Trim
                                    If s.Contains("#") = True Then
                                    ElseIf Regex.IsMatch(s, "^0x[0-9A-Fa-f]{8} 0x[0-9A-Fa-f]{8}", RegexOptions.ECMAScript) = True Then

                                        nullcode = False
                                        If cfmax = 100 Then

                                            errorct += 1
                                            write_errors(errorct, n.Text.Trim, ccname, "CODEFREAKの100行制限を超えてるため分割されます")
                                            errors = True


                                            bs(i) = &H44 'D コード名
                                            bs(i + 1) = &H20
                                            i += 2
                                            Array.ConstrainedCopy(cname, 0, bs, i, namebak)
                                            i += namebak
                                            bs(i) = 10
                                            bs(i + 1) = 10
                                            i += 2
                                            cfmax = 0
                                        End If
                                        cfmax += 1
                                        bs(i) = &H43 'C コード内容
                                        bs(i + 1) = &H20
                                        i += 2
                                        s = s.Replace("0x", "")
                                        s = s.Replace(" ", "")
                                        cp1201len = s.Length * 2
                                        cfutf16be = Encoding.GetEncoding(1201).GetBytes(s)
                                        Array.ConstrainedCopy(cfutf16be, 0, bs, i, cp1201len)
                                        i += cp1201len
                                        bs(i) = 10
                                        bs(i + 1) = 10
                                        i += 2
                                    Else

                                        ' Error, code length was incorrect
                                        errorct += 1
                                        write_errors(errorct, n.Text.Trim, n1.Text.Trim, "不正なコード形式です: " & s)
                                        errors = True
                                    End If
                                End If
                            Next
                        Else
                            notcf = True
                            ' Error, code length was incorrect
                            errorct += 1
                            write_errors(errorct, n.Text.Trim, n1.Text.Trim, "CWC用コード形式ではありません")
                            errors = True
                        End If
                    End If
                Next
            Next


            If nullcode = True Then
                bs(i) = &H43 'C コード内容
                bs(i + 1) = &H20
                i += 2
                Array.ConstrainedCopy(dummy, 0, bs, i, 32)
                i += 32
                bs(i) = 10
                bs(i + 1) = 10
                i += 2
            End If

            fs.Write(bs, 0, i)
            fs.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
            fs.Close()
        End Try



        If errorct > 0 Then
            ew.Show()
            ew.tab_error.SelectedIndex = 1
            m.Focus()
            reset_toolbar()
        End If


    End Sub

    Public Function cvtsceid2cf(ByVal b As Byte()) As String
        Dim sb As StringBuilder = New StringBuilder
        For i = 0 To 3
            sb.Append((CType(b(i), Integer) >> 4).ToString("X"))
            sb.Append((CType(b(i), Integer) And 15).ToString("X"))
        Next

        Return sb.ToString

    End Function


    Public Function cfdecript(ByVal s As String) As String
        Dim iu As Long = Convert.ToInt64(s.Substring(2, 8), 16)
        iu = iu Xor &HD6F73BEE
        iu = iu And 4294967295

        s = "0x" & iu.ToString("X8") & s.Remove(0, 10)
        Return s

    End Function


    Public Sub save_ar(ByVal filename As String, ByVal enc1 As Integer)

        Dim m As MERGE = MERGE
        Dim i As Integer = 0
        Dim back As Integer = 0
        Dim back2 As Integer = 0
        Dim buf As String()
        Dim fs As New System.IO.FileStream(filename, FileMode.Create, FileAccess.Write)
        Dim bs(3 * 1024 * 1024) As Byte '３Mばいとぐらい
        Dim nullcode As Boolean = False
        Dim dummy() As Byte = {0, 0, 0, &HCF, 0, 0, 0, 0}
        Dim z As Integer = 0
        Dim cend As Integer = 0
        Dim cendplus As Integer = 0
        Dim binend As Integer = 0
        Dim l As Integer = 0
        Dim k As Integer = 0
        Dim t As UInteger = 0
        Dim u As UInteger = 0
        Dim tmp As Integer = 0

        Dim header() As Byte = Encoding.GetEncoding(932).GetBytes("PSPARC01")
        Dim pheader() As Byte = Encoding.GetEncoding(932).GetBytes("PAPARX01")
        Dim nextoffset(1) As Byte
        Dim beforeoffset(1) As Byte
        Dim tocodehead(1) As Byte
        Dim null() As Byte = Nothing
        Dim codenum(3) As Byte
        Dim gid As String = ""
        Dim ggid(9) As Byte
        Dim gname As String = ""
        Dim ggname() As Byte = Nothing

        Dim lline() As Byte = Nothing
        Dim clen() As Byte = Nothing
        Dim tocheat() As Byte = Nothing
        Dim nextcode() As Byte = Nothing

        Dim ccname As String = ""
        Dim cname() As Byte = Nothing
        Dim code(3) As Byte
        Dim mode As String = ""
        Dim errors As Boolean = False
        Dim overflow As Boolean = False
        Dim arcut As Boolean = My.Settings.arbincut
        Dim ew As error_window = error_window
        Dim errorct As Integer = 0
        Dim arcmt As Integer = 0
        Dim arcutmsg As Boolean = False

        Dim codetlen As Integer = 0
        Dim nodect As Integer = 0


        Dim outputpaparx As Boolean = m.PAPARX01TEST.Checked
        Dim paparx_total As Integer = (m.codetree.GetNodeCount(True)) + 1
        Dim paparx_hidden(paparx_total) As Byte
        Dim paparx_toggle(paparx_total) As Byte
        Dim paparx_folder(paparx_total) As Byte
        Dim bincounter As Integer = 0

        '最初はダミーなので飛ばすよう1にしておく
        Dim bitshifter As Integer = 1

        'ARTOOLBATTERYはダミーなしっぽいので修正
        If m.ARMAX2.Checked = False Then
            bitshifter = 0
        End If


        For k = 0 To paparx_total - 1
            paparx_toggle(k) = 255
        Next

        reset_errors() ' Clear prior save errors if any
        Array.Resize(header, 28)

        Try

            For Each n As TreeNode In m.codetree.Nodes(0).Nodes

                back2 = i - back
                back = i
                arcutmsg = False
                'bitshifterがあればフラグポインタを++、最初はダミーなので必ず++
                If bitshifter > 0 Then
                    bincounter += 1
                End If
                '最初の1bitはゲーム全体をさすので飛ばす
                bitshifter = 1

                gid = n.Tag.ToString
                If gid.Length < 10 Then
                    gid = gid.PadRight(10, CChar("0"))
                End If
                gname = n.Text

                If Regex.IsMatch(gname, "[^\u0020-\u007f\uFF61-\uFF9F]", RegexOptions.ECMAScript) = True Then
                    errorct += 1
                    write_errors(errorct, gname, "NULL", "ゲーム名にアルファベット半角カナ以外が含まれてます")
                    errors = True
                End If

                ggid = Encoding.GetEncoding(932).GetBytes(gid)
                Array.Resize(ggname, gname.Length)
                ggname = Encoding.GetEncoding(932).GetBytes(gname)

                l = gname.Length
                If l > 58 Then
                    errorct += 1
                    write_errors(errorct, gname, "NULL", "ゲーム名が59文字以上あります")
                    errors = True

                    l = 58
                    gname = gname.Substring(0, 58)
                End If

                l += 18
                If (l And 3) = 0 Then
                    l += 4
                Else
                    l += 4 - (l And 3)
                End If

                tocodehead = BitConverter.GetBytes(l)
                Array.ConstrainedCopy(tocodehead, 0, bs, i + 4, 1)
                Array.ConstrainedCopy(ggid, 0, bs, i + 7, 10)
                Array.ConstrainedCopy(ggname, 0, bs, i + 18, gname.Length)
                i += l
                cend = 0
                cendplus = 0
                arcmt = 0
                binend += 1
                overflow = False
                nodect = 0
                codetlen = n.Nodes.Count

                For Each n1 As TreeNode In n.Nodes

                    nodect += 1

                    mode = n1.Tag.ToString.Substring(0, 1)

                    If outputpaparx = True Then
                        paparx_toggle(bincounter) = CByte(paparx_toggle(bincounter) Xor ((CInt(mode) And 1) << bitshifter))
                        bitshifter += 1
                        If bitshifter = 8 Then
                            bincounter += 1
                            bitshifter = 0
                        End If
                    End If

                    If mode = "2" Or mode = "3" Then
                        ccname = n1.Text.Trim


                    Else
                        ccname = n1.Text.Trim & "(CWC/TEMP)"
                    End If

                    If Regex.IsMatch(ccname, "[^\u0020-\u007f\uFF61-\uFF9F]", RegexOptions.ECMAScript) = True Then
                        errorct += 1
                        write_errors(errorct, gname, ccname, "コード名にアルファベット半角カナ以外が含まれてます")
                        errors = True
                    End If

                    l = ccname.Length
                    If l > 58 Then
                        errorct += 1
                        write_errors(errorct, gname, ccname, "コード名が59文字以上あります")

                        l = 58
                        ccname = ccname.Substring(0, 58)
                    End If
                    Array.Resize(cname, l)
                    cname = Encoding.GetEncoding(932).GetBytes(ccname)

                    clen = BitConverter.GetBytes(l + 1)
                    Array.ConstrainedCopy(clen, 0, bs, i + 1, 1)
                    Array.ConstrainedCopy(cname, 0, bs, i + 4, l)
                    l += 4
                    If (l And 3) = 0 Then
                        l += 4
                    Else
                        l += 4 - (l And 3)
                    End If
                    tocheat = BitConverter.GetBytes(l >> 2)
                    Array.ConstrainedCopy(tocheat, 0, bs, i + 2, 1)
                    cend += 1
                    z = 0
                    nullcode = True
                    overflow = False


                    If arcmt > 0 Then
                        arcmt -= 1

                    ElseIf mode = "2" Or mode = "3" Then
                        buf = n1.Tag.ToString.Split(CChar(vbLf))

                        For Each s As String In buf

                            s = s.Trim

                            If s.Length > 2 Then
                                If s.Contains("#") = True Then
                                ElseIf Regex.IsMatch(s, "^0x[0-9A-Fa-f]{8} 0x[0-9A-Fa-f]{8}", RegexOptions.ECMAScript) = True Then

                                    nullcode = False
                                    s = s.Replace("0x", "")
                                    s = s.Replace(" ", "")
                                    t = Convert.ToUInt32(s.Substring(0, 8), 16)
                                    code = BitConverter.GetBytes(t)
                                    Array.ConstrainedCopy(code, 0, bs, i + l + 8 * z, 4)
                                    u = Convert.ToUInt32(s.Substring(8, 8), 16)
                                    code = BitConverter.GetBytes(u)
                                    Array.ConstrainedCopy(code, 0, bs, i + l + 4 + 8 * z, 4)
                                    If overflow = True Then
                                        errorct += 1
                                        If arcut = True AndAlso arcutmsg = True AndAlso nodect = codetlen AndAlso z >= 255 Then
                                            write_errors(errorct, n.Text.Trim, ccname, "最後のコードで255行を超えてます,適用行数÷256の余りXになってしまいます")
                                        Else
                                            write_errors(errorct, n.Text.Trim, ccname, "PSPARの行数限界を超える可能性があるので118行ずつ分割しました")
                                        End If
                                        overflow = False
                                    End If
                                    z += 1

                                    If t = 3472883713 Then
                                        arcmt = CInt(u And 255)
                                    End If

                                    If arcut = True AndAlso nodect = codetlen AndAlso (z >= 118 AndAlso z < 255) Then
                                        arcutmsg = True

                                    ElseIf arcut = True AndAlso nodect = codetlen AndAlso z >= 255 Then
                                        overflow = True

                                    ElseIf z = 118 Then

                                        overflow = True

                                        If outputpaparx = True Then
                                            paparx_toggle(bincounter) = CByte(paparx_toggle(bincounter) Xor ((CInt(mode) And 1) << bitshifter))
                                            bitshifter += 1
                                            If bitshifter = 8 Then
                                                bincounter += 1
                                                bitshifter = 0
                                            End If
                                        End If

                                        lline = BitConverter.GetBytes(z)
                                        Array.ConstrainedCopy(lline, 0, bs, i, 1)
                                        k = (z * 8 + l) >> 2
                                        nextcode = BitConverter.GetBytes(k)
                                        Array.ConstrainedCopy(nextcode, 0, bs, i + 3, 1)
                                        tmp = i
                                        i += (z * 8) + l

                                        Array.ConstrainedCopy(clen, 0, bs, i + 1, 1)
                                        Array.ConstrainedCopy(cname, 0, bs, i + 4, ccname.Length)
                                        Array.ConstrainedCopy(tocheat, 0, bs, i + 2, 1)
                                        cendplus += 1
                                        z = 0
                                    End If
                                Else
                                    ' Error, code length was incorrect
                                    errorct += 1
                                    write_errors(errorct, n.Text.Trim, n1.Text.Trim, "不正なコード形式です; " & s)
                                    errors = True
                                End If
                            End If

                        Next

                        If arcut = True AndAlso arcutmsg = True AndAlso nodect = codetlen Then
                            errorct += 1
                            write_errors(errorct, n.Text.Trim, ccname, "最後のコードで118行を超えています,PSPでは編集/新規追加ができない可能性があります")
                        End If


                        If nullcode = True Then
                            If arcmt = 0 Then
                                errorct += 1
                                write_errors(errorct, n.Text.Trim, ccname, "コード内容が空なので代替ダミーが追加されます")
                            End If
                            z = 1
                            Array.ConstrainedCopy(dummy, 0, bs, i + l, 8)

                        End If

                        If z > 0 Then
                            lline = BitConverter.GetBytes(z)
                            Array.ConstrainedCopy(lline, 0, bs, i, 1)
                            k = (z * 8 + l) >> 2
                            If n.Nodes.Count <> cend Then
                                nextcode = BitConverter.GetBytes(k)
                                Array.ConstrainedCopy(nextcode, 0, bs, i + 3, 1)
                            End If
                            i += (z * 8) + l
                        Else
                            cendplus -= 1
                            Array.Resize(null, ccname.Length + 8)
                            If n.Nodes.Count = cend Then
                                Array.ConstrainedCopy(null, 0, bs, tmp + 3, 1)
                            End If
                            Array.ConstrainedCopy(null, 0, bs, i, null.Length)
                        End If
                    Else

                        ' Error, code length was incorrect
                        If arcmt = 0 Then
                            errorct += 1
                            write_errors(errorct, n.Text.Trim, n1.Text.Trim, "PSPAR用コード形式ではありません,代替ダミーが追加されます")
                            errors = True
                        End If

                        z = 1
                        Array.ConstrainedCopy(dummy, 0, bs, i + l, 8)

                        lline = BitConverter.GetBytes(z)
                        Array.ConstrainedCopy(lline, 0, bs, i, 1)
                        k = (z * 8 + l) >> 2
                        If n.Nodes.Count <> cend Then
                            nextcode = BitConverter.GetBytes(k)
                            Array.ConstrainedCopy(nextcode, 0, bs, i + 3, 1)
                        End If
                        i += (z * 8) + l

                    End If

                Next
                codenum = BitConverter.GetBytes(cend + cendplus)
                Array.ConstrainedCopy(codenum, 0, bs, back + 5, 2)
                nextoffset = BitConverter.GetBytes((i - back) >> 2)
                beforeoffset = BitConverter.GetBytes(back2 >> 2)
                If binend <> m.codetree.Nodes(0).Nodes.Count Then
                    Array.ConstrainedCopy(nextoffset, 0, bs, back, 2)
                End If
                Array.ConstrainedCopy(beforeoffset, 0, bs, back + 2, 2)

            Next

            code = BitConverter.GetBytes(i)
            Array.ConstrainedCopy(code, 0, header, 16, 4)
            Array.Resize(bs, i)

            t = datel_hash(bs, 0, i)
            code = BitConverter.GetBytes(t)
            Array.ConstrainedCopy(code, 0, header, 12, 4)

            If outputpaparx = True Then

                If bitshifter <> 0 Then
                    bincounter += 1
                End If

                t = datel_hash(paparx_hidden, 0, bincounter)
                t = t + datel_hash(paparx_toggle, 0, bincounter)
                t = t + datel_hash(paparx_folder, 0, bincounter)
                Array.Resize(paparx_hidden, bincounter)
                Array.Resize(paparx_toggle, bincounter)
                Array.Resize(paparx_folder, bincounter)

                code = BitConverter.GetBytes(t)
                Array.ConstrainedCopy(code, 0, header, 20, 4)


                code = BitConverter.GetBytes(bincounter * 2)
                Array.ConstrainedCopy(code, 0, header, 24, 4)

                Array.Resize(pheader, 16)
                code = BitConverter.GetBytes(bincounter)
                Array.ConstrainedCopy(code, 0, pheader, 12, 4)

            End If

            t = datel_hash(header, 12, 16)
            code = BitConverter.GetBytes(t)
            Array.ConstrainedCopy(code, 0, header, 8, 4)




        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        fs.Write(header, 0, header.Length)
        fs.Write(bs, 0, bs.Length)

        If outputpaparx = True Then

            fs.Write(pheader, 0, pheader.Length)
            fs.Write(paparx_hidden, 0, paparx_hidden.Length)
            fs.Write(paparx_toggle, 0, paparx_hidden.Length)
            fs.Write(paparx_folder, 0, paparx_hidden.Length)

        End If

        fs.Close()


        If errorct > 0 Then
            ew.Show()
            ew.tab_error.SelectedIndex = 1
            m.Focus()
            reset_toolbar()
        End If

    End Sub

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

            'Dim stt As String = v1.ToString("X")
            v0 = Convert.ToUInt32(bin(s + i))
            a3 = v0 + v1
            'Dim st As String = a3.ToString("X")
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

    Private Sub reset_errors()

        Dim ew As error_window = error_window
        Dim m As MERGE = MERGE

        ew.Hide()
        m.options_error.Text = "エラーログを見る"
        m.options_error.Checked = False
        ew.list_save_error.Items.Clear()

    End Sub

    Private Sub reset_toolbar()

        Dim m As MERGE = MERGE

        If m.options_error.Checked = False Then
            m.options_error.Checked = True
            m.options_error.Text = "エラーログを隠す"
        End If

    End Sub

    Private Sub write_errors(ByVal error_n As Integer, ByVal game_t As String, ByVal code_t As String, ByVal error_r As String)

        Dim ew As error_window = error_window

        Dim itemx As New ListViewItem
        If error_n > 0 AndAlso game_t <> "" AndAlso code_t <> "" AndAlso error_r <> "" Then
            itemx.Text = error_n.ToString
            itemx.SubItems.Add(game_t)
            itemx.SubItems.Add(code_t)
            itemx.SubItems.Add(error_r.Trim)
            ew.list_save_error.Items.Add(itemx)
            Application.DoEvents()
        End If


    End Sub

    Public Sub clipboad(ByVal MODE As String)

        Dim m As MERGE = MERGE
        Dim i As Integer = 0 ' Error count
        Dim buffer As String()
        Dim filename = "TMP"
        Dim ew As error_window = error_window
        Dim errors As Boolean = False
        Dim cwcar As String = "_L "
        Dim b1 As String = Nothing
        Dim gid As String = ""
        Dim enc As String = ""
        Dim scm As String = ""
        Dim cmf As String = ""
        Dim fctxt As String = ""
        Dim fcc As String = ""
        Dim fcode As String = ""
        Dim scmclose As Boolean = False
        Dim out As Boolean = False
        Dim nullcode As Boolean = False
        Dim line As Integer = 0
        Dim nnnn As Integer = 0
        Dim sb As New StringBuilder
        Dim ss(2) As String
        Dim sss As String() = Nothing

        Dim r As New System.Text.RegularExpressions.Regex("0x[0-9a-fA-F]{8}", RegularExpressions.RegexOptions.ECMAScript)
        Dim hex As System.Text.RegularExpressions.Match
        Dim dg As New System.Text.RegularExpressions.Regex("<DGLINE[0-9]?[0-9]?[0-9]?='.*?'>", RegularExpressions.RegexOptions.ECMAScript)
        Dim gdline As System.Text.RegularExpressions.Match
        reset_errors() ' Clear prior save errors if any

        Try

            Dim n As TreeNode = m.codetree.SelectedNode

            If n.Level = 0 Then

            ElseIf n.Level > 0 Then
                If n.Level = 2 Then
                    n = n.Parent
                End If
                gid = n.Tag.ToString.Trim

                If gid.Length = 13 Then
                    'str &= "#CFMODE" & GID.Substring(10, 3) & vbCrLf
                    If Regex.IsMatch(gid, "^[a-zA-Z\-]{5}[0-9A-Fa-f]{8}", RegexOptions.ECMAScript) Then
                        If (Convert.ToInt32(gid.Substring(10, 3), 16) And &H800) = 0 Then
                            enc = ("#CFMODE" & gid.Substring(10, 3) & vbCrLf)
                        Else
                            enc = ("#CWCMODE" & gid.Substring(10, 3) & vbCrLf)
                        End If
                    End If
                    If n.Nodes.Count > 0 AndAlso n.Nodes(0).Text = "(M)" Then
                        Dim hexs As Regex = New Regex("0x[0-9A-Fa-f]{8} 0x[0-9A-Fa-f]{8}", RegexOptions.ECMAScript)
                        Dim mh As Match = hexs.Match(n.Nodes(0).Tag.ToString)
                        If mh.Success AndAlso (Convert.ToInt32(mh.Value.Remove(0, 13), 16) And &H800) = 0 Then
                            enc &= "_E " & mh.Value & vbCrLf
                        End If
                    Else
                        Dim bytesData As Byte() = Encoding.GetEncoding(1252).GetBytes(gid.Remove(4, 1))
                        Dim gidst = ""
                        gidst = cvtsceid2cf(bytesData)
                        gidst &= gid.Remove(0, 5) 'CFID
                        enc &= "_E 0x" & gidst.Insert(8, " 0x") & vbCrLf
                    End If

                    gid = gid.Substring(0, 10)
                End If

                b1 = "_S " & gid & vbCrLf
                scm = "ID:" & gid & vbCrLf
                b1 &= "_G " & n.Text.Trim & vbCrLf
                If enc <> "" Then
                    b1 &= enc
                End If


                scm &= "NAME:" & n.Text.Trim & vbCrLf
                scm &= "$START" & vbCrLf
                cmf = b1
                fctxt = b1

                If m.codetree.SelectedNode.Level = 2 Then
                    b1 = ""
                End If

                For Each n1 As TreeNode In n.Nodes

                    If n1.Tag Is Nothing Then
                        If n1.Tag.ToString.Substring(0, 1) = "0" Then
                            b1 &= "_C0 " & n1.Text.Trim & vbCrLf
                        Else
                            b1 &= "_C1 " & n1.Text.Trim & vbCrLf
                        End If
                        If scmclose = True Then
                            scm = scm.Insert(scm.Length - 1, "}")
                        End If
                        scm &= "$" & n1.Text.Trim & "{" & vbCrLf
                        scm &= "$ $2 $(FFFFFFFF FFFFFFFF)}" & vbCrLf
                        cmf &= "_C0 " & n1.Text.Trim & vbCrLf
                        fctxt &= "_C0 " & n1.Text.Trim & vbCrLf
                        line = 0
                    ElseIf n1.Index = 0 AndAlso n1.Text = "(M)" Then


                    Else

                        buffer = n1.Tag.ToString.Split(CChar(vbCrLf))

                        For Each s As String In buffer
                            If s.Length = 1 Then
                                If s = "0" Or s = "2" Or s = "4" Then
                                    If s = "0" Then
                                        cwcar = "_L "
                                    ElseIf s = "2" Then
                                        cwcar = "_M "
                                    ElseIf s = "4" Then
                                        cwcar = "_N "
                                    End If
                                    b1 &= "_C0 " & n1.Text.Trim & vbCrLf
                                    If MODE <> "CLIP" AndAlso s = "0" AndAlso m.PSX = False Then
                                        If nullcode = True Then
                                            scm &= "$ $2 $(FFFFFFFF FFFFFFFF)" & vbCrLf
                                        End If
                                        If scmclose = True Then
                                            scm = scm.Insert(scm.Length - 2, "}")
                                        End If
                                        scm &= "$" & n1.Text.Trim & "{" & vbCrLf
                                        cmf &= "_C0 " & n1.Text.Trim & vbCrLf
                                        fctxt &= "_C0 " & n1.Text.Trim & vbCrLf
                                        line = 0
                                        out = True
                                        nullcode = True
                                    Else
                                        out = False
                                    End If
                                ElseIf s = "1" Or s = "3" Or s = "5" Then
                                    If s = "1" Then
                                        cwcar = "_L "
                                    ElseIf s = "3" Then
                                        cwcar = "_M "
                                    ElseIf s = "5" Then
                                        cwcar = "_N "
                                    End If
                                    b1 &= "_C1 " & n1.Text.Trim & vbCrLf
                                    If MODE <> "CLIP" AndAlso s = "1" AndAlso m.PSX = False Then
                                        If nullcode = True Then
                                            scm &= "$ $2 $(FFFFFFFF FFFFFFFF)" & vbCrLf
                                        End If
                                        If scmclose = True Then
                                            scm = scm.Insert(scm.Length - 2, "}")
                                        End If
                                        scm &= "$" & n1.Text.Trim & "{" & vbCrLf
                                        cmf &= "_C1 " & n1.Text.Trim & vbCrLf
                                        fctxt &= "_C1 " & n1.Text.Trim & vbCrLf
                                        line = 0
                                        nullcode = True
                                        out = True
                                    Else
                                        out = False
                                    End If
                                End If

                            ElseIf s.Length > 1 Then

                                If s.Contains("#") AndAlso MODE <> "SCM" Then
                                    b1 &= s.Trim & vbCrLf
                                    fcc &= s.Trim & vbCrLf
                                ElseIf m.PSX = True Then
                                    b1 &= cwcar & s.Trim & vbCrLf
                                Else
                                    '0x00000000 0x00000000
                                    If s.Contains("0x") Then
                                        b1 &= cwcar & s.Trim & vbCrLf
                                        scmclose = True
                                        nullcode = False
                                        If out = True AndAlso m.PSX = False Then
                                            If (s.Substring(3, 1) = "4" Or s.Substring(3, 1) = "8" Or s.Substring(3, 1) = "5" Or s.Substring(3, 3) = "305" Or s.Substring(3, 3) = "306") _
                                                AndAlso line = 0 Then
                                                scm &= "$ $2 $(" & s.Trim.Replace("0x", "") & " "
                                                line = 4
                                            ElseIf s.Substring(3, 1) = "6" AndAlso line = 0 Then
                                                scm &= "$ $2 $(" & s.Trim.Replace("0x", "") & " "
                                                line = 6
                                            ElseIf s.Substring(3, 2) = "F0" AndAlso line = 0 Then
                                                scm &= "$暗号不可 $2 $(FFFFFFFF FFFFFFFF)" & vbCrLf
                                                line = 15
                                                nnnn = Convert.ToInt32(s.Substring(9, 2), 16)
                                            ElseIf line = 15 Then
                                                If nnnn > 0 Then
                                                    nnnn -= 1
                                                Else
                                                    line = 0
                                                End If
                                            ElseIf line = 6 Then
                                                If CInt(s.Substring(10, 1)) > 1 Then
                                                    scm &= s.Trim.Replace("0x", "") & " "
                                                    nnnn = CInt(s.Substring(10, 1))
                                                    line = 9
                                                Else
                                                    scm &= s.Trim.Replace("0x", "") & ")" & vbCrLf
                                                    line = 0
                                                End If
                                            ElseIf line = 9 Then
                                                If s.Substring(3, 1) = "2" Or s.Substring(3, 1) = "3" Then
                                                    scm &= s.Trim.Replace("0x", "") & ")" & vbCrLf
                                                    nnnn = nnnn \ 2 - 1
                                                    If nnnn > 0 Then
                                                        line = 2
                                                    Else
                                                        line = 0
                                                    End If
                                                Else
                                                    scm &= s.Trim.Replace("0x", "") & ")" & vbCrLf
                                                    line = 0
                                                End If
                                            ElseIf line = 2 Then
                                                If nnnn > 0 Then
                                                    scm &= "$└ $2 $(" & s.Trim.Replace("0x", "") & ")" & vbCrLf
                                                Else
                                                    scm &= "$ $2 $(" & s.Trim.Replace("0x", "") & ")" & vbCrLf
                                                    line = 0
                                                End If
                                            ElseIf line = 4 Then
                                                scm &= s.Trim.Replace("0x", "") & ")" & vbCrLf
                                                line = 0
                                            Else
                                                scm &= "$ $2 $(" & s.Trim.Replace("0x", "") & ")" & vbCrLf
                                            End If

                                            cmf &= cwcar & s.Trim & vbCrLf
                                            fcode &= s.Trim & vbCrLf
                                        End If
                                    Else
                                        ' Error, code length was incorrect
                                        i += 1
                                        write_errors(i, n.Text.Trim, n1.Text.Trim, "不正なコード形式です: " & s.Trim)
                                        errors = True
                                    End If

                                End If

                            End If


                        Next

                        If MODE = "TXT" Then
                            Dim ls As Integer = 1
                            hex = r.Match(fcode)
                            gdline = dg.Match(fcc)
                            Array.Resize(sss, 1)
                            Array.Resize(sss, 2)
                            While hex.Success
                                Dim l = gdline.Value.IndexOf("'") + 1
                                Dim z = gdline.Value.LastIndexOf("'")
                                If l <> -1 AndAlso z - l > 0 AndAlso l - 9 > 0 Then
                                    ss(0) = gdline.Value.Substring(l, z - l)
                                    ss(1) = gdline.Value.Substring(7, l - 9)
                                    Array.Resize(sss, CInt(ss(1)) + 1)
                                    Array.ConstrainedCopy(ss, 0, sss, CInt(ss(1)), 1)
                                End If
                                gdline = gdline.NextMatch
                                If ls >= sss.Length Then
                                    Array.Resize(sss, ls + 1)
                                End If
                                If sss(ls) <> "" Then
                                    sb.Append("_N2 ")
                                    sb.Append(sss(ls))
                                    sb.Append(vbCrLf)
                                End If
                                sb.Append("_L ")
                                sb.Append(hex.Value)
                                hex = hex.NextMatch
                                sb.Append(" ")
                                sb.Append(hex.Value)
                                hex = hex.NextMatch
                                sb.Append(vbCrLf)
                                ls += 1
                            End While
                            fctxt &= sb.ToString
                            sb.Clear()
                        End If

                        fcc = ""
                        fcode = ""

                    End If

                    If m.codetree.SelectedNode.Level = 2 Then
                        If n1.Index = m.codetree.SelectedNode.Index Then
                            Exit For
                        Else
                            b1 = ""
                        End If
                    End If

                Next

                filename = Application.StartupPath & "\" & gid

                If nullcode = True Then
                    scm &= "$ $2 $(FFFFFFFF FFFFFFFF)" & vbCrLf
                End If
                scm = scm.Insert(scm.Length - 2, "}")

                Dim enc1 = My.Settings.MSCODEPAGE

                If MODE = "CLIP" Then
                    Clipboard.SetText(b1)
                ElseIf MODE = "TXT" Then
                    filename &= ".txt"

                    writers(enc1, fctxt, filename)

                ElseIf MODE = "CMF" Then
                    filename &= ".cmf"
                    writers(enc1, cmf, filename)

                ElseIf MODE = "SCM" Then
                    filename &= ".scm"
                    writers(enc1, scm, filename)

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Public Sub save_tab(ByVal filename As String)

        Dim m As MERGE = MERGE
        Dim buffer As String()
        Dim i As Integer = 0
        Dim k As Integer = 0
        Dim hex As UInteger = 0
        Dim address As ULong = 0
        Dim real As ULong = &H8800000
        Dim dummy() As Byte = {&HFF, &HFF, &H7F, &H8}
        Dim dummy2() As Byte = {&HFF, &HFF, &HFF, &HFF}
        Dim bs() As Byte = Nothing
        Dim header(35) As Byte
        Dim gametitle() As Byte = Nothing
        Dim total(3) As Byte
        Dim acode(7) As Byte
        Dim code(3) As Byte
        Dim name(9) As Byte
        Dim codetotal(1000 * 16) As Byte
        Dim nametotal(1000 * 10) As Byte
        Dim nullcode As Boolean = False
        Dim flag As Boolean = False

        Dim n As TreeNode = m.codetree.SelectedNode

        Try

            If n.Level = 0 Then

            ElseIf n.Level > 0 Then
                If n.Level = 2 Then
                    n = n.Parent
                End If
                name = Encoding.GetEncoding(0).GetBytes(n.Tag.ToString)
                Array.ConstrainedCopy(name, 0, header, 0, 10)
                Array.Resize(gametitle, 2 * n.Text.ToString.Length)
                gametitle = Encoding.GetEncoding(936).GetBytes(n.Text.ToString)
                Array.Resize(gametitle, 26)
                Array.ConstrainedCopy(gametitle, 0, header, 10, gametitle.Length)

                For Each n1 As TreeNode In n.Nodes

                    If n1.Tag Is Nothing Then

                    ElseIf i >= 999 Then
                        Exit For
                    Else

                        buffer = n1.Tag.ToString.Split(CChar(vbCrLf))
                        For Each s As String In buffer
                            If s.Length = 1 Then
                                If s = "0" Or s = "1" Then
                                    If nullcode = True Then
                                        Array.ConstrainedCopy(dummy, 0, codetotal, i * 16, 4)
                                        Array.ConstrainedCopy(dummy2, 0, codetotal, i * 16 + 4, 4)
                                        i += 1
                                    End If
                                    Array.Clear(name, 0, 10)
                                    Array.Resize(name, 2 * n1.Name.Length)
                                    name = Encoding.GetEncoding(936).GetBytes(n1.Name)
                                    Array.Resize(name, 10)
                                    Array.ConstrainedCopy(name, 0, nametotal, i * 10, 10)
                                    If s = "1" Then
                                        flag = True
                                    Else
                                        flag = False
                                    End If
                                    nullcode = True
                                End If
                            ElseIf s.Length > 1 Then
                                '0x00000000 0x00000000
                                If s.Contains("#") Then

                                ElseIf s.Contains("0x") Then
                                    address = Convert.ToUInt64(s.Substring(3, 8), 16)
                                    address += real
                                    acode = BitConverter.GetBytes(address)
                                    Array.ConstrainedCopy(acode, 0, codetotal, i * 16, 4)
                                    hex = Convert.ToUInt32(s.Substring(14, 8), 16)
                                    code = BitConverter.GetBytes(hex)
                                    Array.ConstrainedCopy(code, 0, codetotal, i * 16 + 4, 4)
                                    If flag = True Then
                                        codetotal(i * 16 + 12) = 1
                                    End If
                                    If nullcode = False Then
                                        nametotal(i * 10) = &H2B
                                    End If
                                    nullcode = False
                                    i += 1
                                End If

                                If i >= 999 Then
                                    Exit For
                                End If

                            End If

                        Next

                    End If

                Next

                If nullcode = True Then
                    Array.ConstrainedCopy(dummy, 0, codetotal, i * 16, 4)
                    Array.ConstrainedCopy(dummy2, 0, codetotal, i * 16 + 4, 4)
                    i += 1
                End If

                Array.Resize(nametotal, 10 * i)
                Array.Resize(codetotal, 16 * i)
                code = BitConverter.GetBytes(i)
                Array.Resize(bs, 40 + 26 * i)
                Array.ConstrainedCopy(header, 0, bs, 0, 36)
                Array.ConstrainedCopy(code, 0, bs, 36, 4)
                Array.ConstrainedCopy(codetotal, 0, bs, 40, codetotal.Length)
                Array.ConstrainedCopy(nametotal, 0, bs, 40 + codetotal.Length, nametotal.Length)
                filename = Application.StartupPath & "\" & n.Tag.ToString & ".tab"
                Dim fs As New System.IO.FileStream(filename, System.IO.FileMode.Create, System.IO.FileAccess.Write)
                fs.Write(bs, 0, bs.Length)
                fs.Close()

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Function writers(ByVal enc1 As Integer, ByVal basetxt As String, ByVal filename As String) As Boolean

        If enc1 = 512132004 Or enc1 = 2132004 Or enc1 = 951 Or enc1 = 21220932 Then
            Dim sel As Integer = 0
            If enc1 = 512132004 Then
                sel = 1
            ElseIf enc1 = 951 Then
                sel = 2
            ElseIf enc1 = 21220932 Then
                sel = 3
            End If
            If File.Exists(unitable(sel)) = True Then
                Dim ctbl As New customtable
                Dim str As String = ""

                Dim tw As New FileStream(filename, FileMode.Create, FileAccess.Write)
                Dim bs As Byte()
                Dim tfs As New FileStream(unitable(sel), FileMode.Open, FileAccess.Read)
                Dim tbl(CInt(tfs.Length - 1)) As Byte
                tfs.Read(tbl, 0, tbl.Length)
                tfs.Close()
                bs = ctbl.unicode2custom(basetxt, tbl, sel)
                tw.Write(bs, 0, bs.Length)
                tw.Close()
            End If

        Else

            Dim tw As New StreamWriter(filename, False, System.Text.Encoding.GetEncoding(My.Settings.MSCODEPAGE))
            tw.Write(basetxt)
            tw.Close()
        End If

        Return True
    End Function


    Public Sub datel_hokan()

        Dim m As MERGE = MERGE
        Dim sb As New StringBuilder
        Dim n As TreeNode = m.codetree.SelectedNode
        Dim folder As Integer = 0
        Dim check As Integer = 0
        Dim r As Regex = New Regex("0xCF00000[0-2] 0x000000[0-9A-F]{2}")
        Dim mt As Match

        If n.Level = 0 Then

        ElseIf n.Level > 0 Then
            If n.Level = 2 Then
                n = n.Parent
            End If
            sb.Append("・")
            sb.Append(n.Text)
            sb.Append("(")
            sb.Append(n.Tag.ToString)
            sb.AppendLine(")")

            For Each n1 As TreeNode In n.Nodes

                mt = r.Match(n1.Tag.ToString())
                If mt.Success Then
                    check = Convert.ToInt32(mt.Value.Substring(9, 1), 16)
                    folder = Convert.ToInt32(mt.Value.Remove(0, 13), 16)

                    If check = 1 Then
                        sb.Append("※")
                    End If
                    sb.AppendLine(n1.Text)

                else

                If folder > 0 Then
                    If check = 0 Then
                        sb.Append("○")
                    ElseIf check = 1 Then
                        sb.Append("※")
                    ElseIf check = 2 Then
                        sb.Append("□")
                    End If
                    folder -= 1
                End If

                    sb.AppendLine(n1.Text)

                End If

            Next
        End If

        Clipboard.SetText(m.ConvANK(sb.ToString))
    End Sub

End Class
