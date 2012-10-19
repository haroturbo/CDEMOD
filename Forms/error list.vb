Imports System
Imports System.Text
Imports System.Windows.Forms


Public Class error_window


    Private Sub error_list_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True
        Me.Hide()
        MERGE.options_error.Checked = False
        MERGE.options_error.Text = "エラー画面の表示"
    End Sub

    Private Sub error_window_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim point As New Point
        point.X = MERGE.Location.X
        point.Y = MERGE.Location.Y + MERGE.Height
        Me.Width = MERGE.Width
        Me.Location = point

        Me.AutoSize = True

    End Sub

    Private Sub error_window_visible(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.VisibleChanged
        Dim point As New Point
        point.X = MERGE.Location.X
        point.Y = MERGE.Location.Y + MERGE.Height
        Me.Width = MERGE.Width
        Me.Location = point
    End Sub

    Private Sub list_pasrse_error_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles list_parse_error.DoubleClick
        Dim p As parser = parser
        p = CType(Me.Owner, parser)
        If list_parse_error.SelectedIndices.Count > 0 Then
            Dim sel As Integer = list_parse_error.SelectedIndices(0)
            p.TX.SelectionStart = CInt(list_parse_error.Items(sel).SubItems(2).Text)
            p.TX.SelectionLength = 0
            p.TX.ScrollToCaret()
            p.TX.Focus()
        End If

    End Sub

    Private Sub コピーToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles コピーToolStripMenuItem.Click, HTML.Click
        Dim sb As StringBuilder = New StringBuilder
        Dim mode As Integer = 0
        Dim max As Integer = 6

        Select Case tab_error.SelectedIndex
            Case 0
                If list_load_error.SelectedItems.Count > 0 Then

                    For i = 0 To list_load_error.SelectedItems.Count - 1
                        sb.Append(txt(list_load_error.SelectedItems(i), 6))
                    Next
                End If
            Case 1
                If list_save_error.SelectedItems.Count > 0 Then
                    max = 4

                    For i = 0 To list_save_error.SelectedItems.Count - 1
                        sb.Append(txt(list_save_error.SelectedItems(i), 4))
                    Next
                End If
            Case 2
                If list_parse_error.SelectedItems.Count > 0 Then

                    For i = 0 To list_parse_error.SelectedItems.Count - 1
                        sb.Append(txt(list_parse_error.SelectedItems(i), 6))
                    Next
                End If

        End Select

        If sb.Length > 0 Then
            If sender.ToString = "HTMLに変換" Then
                Clipboard.SetText(tsv2html(sb.ToString, max))
            Else
                Clipboard.SetText(sb.ToString)
            End If
        End If

    End Sub

    Private Function txt(ByVal ls As ListViewItem, ByVal max As Integer) As String
        Dim sb As StringBuilder = New StringBuilder

        For i = 0 To max - 1
            sb.Append(ls.SubItems(i).Text)
            If i = max - 1 Then
                sb.AppendLine()
            Else
                sb.Append(vbTab)
            End If
        Next

        Return sb.ToString
    End Function



    Private Function tsv2html(ByVal tsv As String, ByVal max As Integer) As String

        Dim sb As StringBuilder = New StringBuilder

        If tsv <> "" Then

            sb.Append("<TABLE CELLSPACING=1 COLS=")
            sb.Append(max.ToString)
            sb.Append(" BORDER=1><TBODY>")

            Dim ss As String() = tsv.Split(CChar(vbLf))
            For Each s As String In ss
                sb.Append("<TR>")
                Dim sss As String() = s.Split(CChar(vbTab))
                For Each s1 As String In sss
                    sb.Append("<TD>")
                    sb.Append(s1)
                    sb.Append("</TD>")
                Next
                sb.Append("</TR>")
            Next

            sb.Append("</TBODY></TABLE>")
        End If

        Return sb.ToString.Replace(vbCr, "")

    End Function



    Private Sub 全て選択ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 全て選択ToolStripMenuItem.Click

        Select Case tab_error.SelectedIndex
            Case 0
                For i = 0 To list_load_error.Items.Count - 1
                    list_load_error.Items(i).Selected = True
                Next
            Case 1
                For i = 0 To list_save_error.Items.Count - 1
                    list_save_error.Items(i).Selected = True
                Next
            Case 2
                For i = 0 To list_parse_error.Items.Count - 1
                    list_parse_error.Items(i).Selected = True
                Next

        End Select
    End Sub

End Class
