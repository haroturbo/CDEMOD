Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Text

Public Class parser

    Public Opener As Form
    Friend gid_gname As Boolean = False
    Dim path As String = Application.StartupPath & "\APP\seekerror.txt"
    Dim last As Integer = -1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            Dim f As MERGE = MERGE
            f = CType(Me.Owner, MERGE)
            Dim cmbss As String() = Nothing

            If f.fixedform.Checked = True Then
                Me.AutoSize = True
            End If

            Dim mode As String = Nothing

            If f.codetree.Nodes.Count >= 1 Then
                If f.codetree.SelectedNode Is Nothing Then
                    f.codetree.SelectedNode = f.codetree.TopNode
                End If
                If f.codetree.SelectedNode.Level = 0 Then
                    mode = "(Lv0,新規追加)"
                ElseIf f.codetree.SelectedNode.Level > 0 Then
                    mode = "(Lv" & f.codetree.SelectedNode.Level.ToString & ",継ぎ足し)"
                End If
            Else
                mode = "(NONE,コードなし)"
            End If

            Me.Text &= mode


            FIND_REGEX.Items.Clear()
            cmbss = lsread(cmbss)
            If cmbss.Length > 0 Then
                FIND_REGEX.Items.AddRange(cmbss)
            End If
            FIND_REGEX.Text = My.Settings.perror

        Catch ex As Exception

        End Try

    End Sub

    Private Function lsread(ByVal cmbss As String()) As String()
        If (File.Exists(path)) Then
            Dim sr As StreamReader = New StreamReader(path, Encoding.GetEncoding(65001))
            Dim s As String = sr.ReadToEnd
            sr.Close()
            cmbss = s.Split(CChar(vbLf))
        End If
        Return cmbss
    End Function

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        Dim f As MERGE
        f = CType(Me.Owner, MERGE)
        check_errors(sender, e)
        If gid_gname = True Then
            f.cmt_tb.Text = TX.Text
            My.Settings.perror = FIND_REGEX.Text
            Me.Close()
        End If
    End Sub

    Private Sub clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clear.Click
        TX.Text = Nothing
    End Sub

    '#補正
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CMT_FIX.Click
        Dim b1 As String() = TX.Text.Split(CChar(vbLf))
        Dim i As Integer = 0
        Dim g As Integer = 0
        Dim code As Integer = 0
        Dim wikiend As Integer = 0
        Dim jane2ch As Integer = 0
        Dim sb As StringBuilder = New StringBuilder
        Dim b2 As String = Nothing
        Dim b3 As String = Nothing
        Dim cwcar As String = Nothing
        Dim rmmode As Integer = 0

        Dim p As New Regex("-{60}", RegexOptions.IgnoreCase)
        '661 名前：名無しさん＠お腹いっぱい。[sage] 投稿日：2011/06/28(火) 19:45:22.31 ID:Nl1EJEAd
        Dim r As New Regex("[0-9]+ 名前.+投稿日.+ID.+", RegexOptions.IgnoreCase)
        Dim l As Match
        Dim m As Match

        For Each s As String In b1
            If s.Length >= 2 Then
                b3 = s.Substring(0, 2)
                If b3 = "_S" Or b3 = "_C" Then
                    g = 1
                End If

                If s.Length >= 7 Then
                    Dim t As New Regex( _
            "トラックバック.[0-9]+.+リンク元.[0-9]+", _
            RegexOptions.IgnoreCase)
                    Dim w As Match = t.Match(s)
                    If w.Success Then
                        sb.AppendLine("_CW cwcwiki用")
                        s = ""
                        'g = 0
                        wikiend = 1
                    ElseIf s.Substring(0, 6) = "抽出レス数：" Then
                        sb.AppendLine("_CJ JANE2CH用")
                        jane2ch = 1
                    End If
                End If

                If g = 1 Then
                    If s.Length >= 2 Then

                        If b3 = "_C" Then
                            s = s.PadRight(3)
                            Dim b4 = s.Substring(2, 1)
                            If i = 0 Then
                                If b4 = "!" Then
                                    rmmode = 1
                                End If
                                sb.AppendLine(s.Trim)
                                code = 0
                            ElseIf b4 = "D" Then
                                code = 0
                            ElseIf b4 = "J" Then
                                sb.AppendLine(s.Trim)
                                jane2ch = 1
                                code = 0
                            ElseIf b4 = "W" Then
                                sb.AppendLine(s.Trim)
                                wikiend = 1
                                code = 0
                            ElseIf b4 = "!" Then
                                rmmode = 1
                            ElseIf b4 = "#" Then
                                rmmode = 0
                            Else
                                sb.AppendLine(s.Trim)
                                code = 0
                            End If
                            i += 1

                            '_L 0x12345678 0x12345678
                        ElseIf b3 = "_L" Or b3 = "_M" Or b3 = "_N" Then
                            cwcar = s.Substring(0, 3)
                            s = s.Replace(vbCr, "")
                            s = s.PadRight(24, "0"c)
                            If s.Substring(3, 2) = "0x" And s.Substring(14, 2) = "0x" Then
                                s = Regex.Replace(s, "[g-zG-Z]", "A")
                                s = s.ToUpper
                                s = s.Replace("_A ", cwcar)
                                s = s.Replace(" 0A", " 0x")
                                sb.AppendLine(s.Substring(0, 24))
                                code = 1
                                'popdb
                            ElseIf s.Substring(2, 1) = " " And s.Substring(11, 1) = " " Then
                                s = Regex.Replace(s, "[g-zG-Z]", "A")
                                s = s.ToUpper
                                s = s.Replace("_A ", "_L ")
                                sb.AppendLine(s.Substring(0, 16))
                                code = 1
                            End If
                        ElseIf rmmode = 1 Or b3 = "_S" Or b3 = "_G" Or b3 = "_E" Then
                            s = s.Replace("#", "")
                            sb.AppendLine(s.Trim)
                        ElseIf code = 1 Then
                            l = p.Match(s)
                            m = r.Match(s)
                            If m.Success Then
                                sb.AppendLine(s.Trim)
                            ElseIf l.Success Then
                            Else
                                s = s.Replace("#", "")
                                sb.Append("#")
                                sb.AppendLine(s.Trim)
                            End If
                        Else
                            s = s.Replace("#", "")
                            sb.Append("#")
                            sb.AppendLine(s.Trim)
                        End If
                    End If
                End If
            End If
        Next
        If jane2ch = 0 And wikiend = 0 Then
            sb.AppendLine("_CD パーサー用ダミー")
        End If
        TX.Text = sb.ToString
    End Sub

    'L補正
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CODE_FIX.Click
        Dim ss As String() = TX.Text.Split(CChar(vbLf))
        Dim sb As StringBuilder = New StringBuilder
        Dim bc2 As String = Nothing
        Dim m As MERGE = MERGE
        Dim p As New Regex("[0-9A-Fa-f]{8} [0-9A-Fa-f?]{4}", RegexOptions.IgnoreCase)
        Dim mp As Match
        Dim cf As New Regex("[0-9A-Fa-f]{8} [0-9A-Fa-f]{8}", RegexOptions.IgnoreCase)
        Dim mc As Match
        Dim ar As New Regex("0x[0-9A-Fa-f]{8} 0x[0-9A-Fa-f]{8}", RegexOptions.IgnoreCase)
        Dim ma As Match

        For Each s As String In ss
            If s.Length > 12 Then
                mp = p.Match(s)
                mc = cf.Match(s)
                ma = ar.Match(s)

                bc2 = s.Substring(0, 2)
                s = s.Trim
                If mc.Success AndAlso bc2 <> "_M" AndAlso bc2 <> "_N" AndAlso bc2 <> "_E" Then
                    s = mc.Value
                    s = s.Replace("_L ", "")
                    s = s.Insert(0, "0x")
                    s = s.Insert(11, "0x")
                    s = "_L " & s & vbCrLf
                ElseIf mp.Success AndAlso m.PSX = True Then
                    s = mp.Value
                    s = s.Replace("_L ", "")
                    s = "_L " & s & vbCrLf
                ElseIf ma.Success AndAlso bc2 <> "_L" AndAlso bc2 <> "_N" AndAlso bc2 <> "_E" Then
                    s = ma.Value
                    s = s.Replace("_M ", "")
                    s = "_M " & s & vbCrLf
                End If
            End If
            If s.Length >= 2 Then
                sb.AppendLine(s.Trim)
            End If
        Next
        TX.Text = sb.ToString
    End Sub

    'C補正
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NAME_FIX.Click

        Dim b1 As String() = TX.Text.Split(CChar(vbLf))
        Dim sb As StringBuilder = New StringBuilder
        Dim bc1 As String = Nothing
        Dim bc2 As String = Nothing
        Dim bc3 As String = Nothing
        Dim rmmode As Integer

        For Each s As String In b1
            If s.Length >= 2 Then
                s = s.PadRight(3)
                bc1 = s.Substring(0, 1)
                bc2 = s.Substring(0, 2)
                bc3 = s.Substring(0, 3)

                If bc3 = "_CR" Then
                    rmmode = 1
                ElseIf bc3 = "_CM" Then
                    rmmode = 0
                End If

                '661 名前：名無しさん＠お腹いっぱい。[sage] 投稿日：2011/06/28(火) 19:45:22.31 ID:Nl1EJEAd
                Dim r As New Regex( _
        "[0-9]+ 名前.+投稿日.+ID.+", _
        RegexOptions.IgnoreCase)
                Dim m As Match = r.Match(s)
                Dim p As New Regex( _
        "-{60}", _
        RegexOptions.IgnoreCase)
                Dim l As Match = p.Match(s)

                If m.Success Or bc1 = "#" Or bc2 = "_L" Or bc2 = "_M" Or bc2 = "_N" Then
                    s = s.Trim & vbCrLf
                ElseIf bc3 = "_CR" Or bc3 = "_CM" Or l.Success Then
                    s = ""
                ElseIf bc3 = "_CJ" Or bc3 = "_CW" Or bc3 = "_CD" Or bc3 = "_C#" Or bc3 = "_C!" Then

                ElseIf rmmode = 1 Then
                    s = s.Replace("_C0 ", "")
                    s = s.Replace("_C1 ", "")
                ElseIf bc3 = "_C0" Or bc3 = "_C1" Or bc2 = "_S" Or bc2 = "_G" Or bc2 = "_E" Then
                Else
                    s = s.Replace("_C0", "")
                    s = s.Replace("_C1 ", "")
                    s = "_C0 " & s.Trim & vbCrLf
                End If
                sb.AppendLine(s.Trim)
            End If
        Next
        TX.Text = sb.ToString
    End Sub

    Private Sub cancel_Click(sender As System.Object, e As System.EventArgs) Handles cancel.Click
        Me.Close()
    End Sub

    Private Sub 検索ToolStripMenuItem_Click_1(sender As System.Object, e As System.EventArgs) Handles 検索ToolStripMenuItem.Click
        Dim f As txrp = txrp
        f.Text = "検索"
        f.Show(Me)

    End Sub

    Private Sub 置換ToolStripMenuItem_Click_1(sender As System.Object, e As System.EventArgs) Handles 置換ToolStripMenuItem.Click
        Dim f As txrp = txrp
        f.Text = "置換"
        f.Show(Me)
    End Sub

    Private Sub check_errors(sender As System.Object, e As System.EventArgs) Handles PARSE_CHECK.Click

        Dim ew As error_window = error_window
        Dim m As MERGE = MERGE
        reset_errors()
        Dim gname As String = "(NULL)"
        Dim cname As String = "(NULL)"
        If Me.Text.Contains("継ぎ足し") Then
            If m.codetree.SelectedNode.Level = 1 Then
                gname = m.codetree.SelectedNode.Text
            ElseIf m.codetree.SelectedNode.Level = 2 Then
                gname = m.codetree.SelectedNode.Parent.Text
            End If
        End If
        Dim line As Integer = 0
        Dim erct As Integer = 0
        Dim pos As Integer = 0
        Dim l As Integer = 0
        Dim bk As Integer = 0
        Dim ss As String() = TX.Text.Split(CChar(vbLf))
        Dim linest As Integer = 0
        Dim selnode1stlv = 1
        If Me.Text.Contains("(Lv0,新規追加)") Then
            selnode1stlv = 0
        End If
        Dim ps1 As Regex = New Regex("^_L [0-9A-Fa-f]{8} [0-9A-Fa-f]{4}")
        Dim psm As Match
        Dim psp As Regex = New Regex("^_[LMN] 0x[0-9A-Fa-f]{8} 0x[0-9A-Fa-f]{8}")
        Dim pspm As Match
        Dim gg As Integer = 0

        For Each s As String In ss
            linest += bk
            bk = s.Length + 1
            s = s.Trim
            line += 1
            If s.Length >= 3 Then
                If selnode1stlv = 0 AndAlso s.Substring(0, 2) = "_S" Then
                    gg = gg Or 1

                ElseIf selnode1stlv = 0 AndAlso s.Substring(0, 2) = "_G" Then
                    If ((gg And 1) = 1) Then
                        gg = gg Or 2
                    End If
                    gname = s.Remove(0, 3)
                ElseIf selnode1stlv = 1 AndAlso s.Substring(0, 2) = "_S" Then
                    erct += 1
                    write_errors(line, erct, s & " <継ぎ足しモード時は無視される行です、ゲームごと追加したい場合はツリービューのトップ選択して(Lv0,新規追加)モードにして下さい,S>", gname, cname, linest) ' Write the ignored line to the error list
                ElseIf selnode1stlv = 1 AndAlso s.Substring(0, 2) = "_G" Then
                    erct += 1
                    write_errors(line, erct, s & " <継ぎ足しモード時は無視される行です、ゲームごと追加したい場合はツリービューのトップ選択して(Lv0,新規追加)モードにして下さい,G>", gname, cname, linest) ' Write the ignored line to the error list

                ElseIf s.Substring(0, 2) = "_C" Then
                    cname = s.Remove(0, 3)

                ElseIf s.Substring(0, 2) = "_L" Or s.Substring(0, 2) = "_M" Or s.Substring(0, 2) = "_N" Then
                    If m.PSX = True Then
                        psm = ps1.Match(s)
                        '_L 12345678 1234
                        If psm.Success = True Then
                        Else
                            erct += 1
                            write_errors(line, erct, s & " <不正なコード形式です,LMN>", gname, cname, linest) ' Write the ignored line to the error list
                        End If
                    Else
                        '_L 0x12345678 0x12345678
                        pspm = psp.Match(s)
                        '_L 12345678 1234
                        If pspm.Success = True Then
                        Else
                            erct += 1
                            write_errors(line, erct, s & " <不正なコード形式です,LMN>", gname, cname, linest) ' Write the ignored line to the error list
                        End If
                    End If
                ElseIf s.Substring(0, 1) = "#" Then
                Else
                    erct += 1
                    write_errors(line, erct, s & " <無視される行です,CMT>", gname, cname, linest) ' Write the ignored line to the error list
                End If
            ElseIf s.Length >= 2 AndAlso s.Substring(0, 1) = "#" Then

            ElseIf s.Trim = "" Then
                erct += 1
                write_errors(line, erct, " <空白しかない行です>", gname, cname, linest) ' Write the ignored line to the error list
            Else
                erct += 1
                write_errors(line, erct, s & "<無視される行です,CMT>", gname, cname, linest) ' Write the ignored line to the error list
            End If
        Next


        If erct > 0 Then
            ew.tab_error.SelectedIndex = 2
            ew.Show(Me)
        End If

        If gg = 3 Then
            gid_gname = True
        ElseIf Me.Text.Contains("継ぎ足し") Then
            gid_gname = True
        Else
            gid_gname = False
            MessageBox.Show("_S _G が含まれてません。" & vbCrLf & _
                            "新規追加モードでは_S _G がないと追加できません。")
        End If

    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles SEEK_ERROR.Click
        Dim ew As error_window = error_window

        Dim r As Regex = New Regex(FIND_REGEX.Text)
        Dim m As Match
        If last > ew.list_parse_error.Items.Count Then
            last = -1
        End If
        Dim i As Integer = 0
        For i = 0 To ew.list_parse_error.Items.Count - 1
            m = r.Match(ew.list_parse_error.Items(i).SubItems(5).Text)
            If m.Success AndAlso i > last Then
                ew.list_parse_error.Items(i).Selected = True
                ew.list_parse_error.TopItem = ew.list_parse_error.Items(i)
                ew.list_parse_error.Focus()
                last = i
                Exit For
            End If
        Next

        If ew.list_parse_error.Items.Count = 0 Then
            MessageBox.Show("エラーリストが作成されてません、チェックを押して下さい")
            last = -1
        ElseIf i = ew.list_parse_error.Items.Count Then
            MessageBox.Show("みつかりませんでした")
            last = -1
        End If

    End Sub

    Private Sub write_errors(ByVal line As Integer, ByVal error_n As Integer, ByVal error_t As String, ByVal game_t As String, ByVal code_t As String, ByVal sel As Integer)
        Dim ew As error_window = error_window
        Dim itemx As New ListViewItem
        If error_n > 0 AndAlso game_t <> "" AndAlso code_t <> "" AndAlso error_t <> "" Then
            itemx.Text = error_n.ToString
            itemx.SubItems.Add(line.ToString)
            itemx.SubItems.Add(sel.ToString.Trim)
            itemx.SubItems.Add(game_t)
            itemx.SubItems.Add(code_t)
            itemx.SubItems.Add(error_t.Trim)
            ew.list_parse_error.Items.Add(itemx)
            Application.DoEvents()
        End If

    End Sub

    Private Sub reset_errors()

        Dim ew As error_window = error_window
        Dim m As MERGE = MERGE
        ew.Hide()
        m.options_error.Text = "エラーログを見る"
        m.options_error.Checked = False
        ew.list_parse_error.Items.Clear()

    End Sub

    Private Sub reset_toolbar()
        Dim m As MERGE = MERGE
        If m.options_error.Checked = False Then
            m.options_error.Checked = True
            m.options_error.Text = "エラーログを隠す"
        End If
    End Sub

    Private Sub エラーリスト検索文字編集ToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles エラーリスト検索文字編集ToolStripMenuItem.Click
        If (File.Exists(path)) Then
            Process.Start(path)
        End If
    End Sub
End Class