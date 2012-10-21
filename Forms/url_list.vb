Imports System
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Windows.Forms

Public Class url_list
    Dim listmax As Integer = 30

    Private Sub ffload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If My.Settings.TOP = True Then
            Me.TopMost = True
        End If

        Dim base As String = My.Settings.urls




        If Me.Text = "ランチャー編集" Then
            base = My.Settings.apps
            Label2.Text = "パス"
            filedialog.Visible = True
        End If

        Try
            Dim ss As String() = base.Split(CChar(vbLf))


            ListView1.Columns.Add("名前", -1, HorizontalAlignment.Left)
            ListView1.Columns.Add("URL", -1, HorizontalAlignment.Left)

            Dim itemx As New ListViewItem
            itemx.SubItems.Add("")

            Dim lss As ListViewItem() = Nothing
            Array.Resize(lss, listmax)
            Dim i As Integer = 0

            For Each s In ss
                s = s.Trim
                If s.Contains(vbTab) Then
                    itemx = CType(itemx.Clone, ListViewItem)
                    itemx.Text = s.Substring(0, s.IndexOf(vbTab))
                    itemx.SubItems(1).Text = s.Remove(0, s.IndexOf(vbTab) + 1)
                    lss(i) = itemx
                    i += 1
                End If
            Next

            Array.Resize(lss, i)

            ListView1.Items.AddRange(lss)
            ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)

        Catch ex As Exception
            MessageBox.Show(Me, ex.Message)

        End Try

    End Sub


    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count > 0 Then
            TextBox1.Text = ListView1.SelectedItems(0).Text
            TextBox2.Text = ListView1.SelectedItems(0).SubItems(1).Text
            ListView1.TopItem = ListView1.SelectedItems(0)
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles add.Click

        Dim itemx As New ListViewItem

        If Me.Text = "ランチャー編集" Then
            itemx.Text = "新しいAPP"
            itemx.SubItems.Add("C:\Program Files\")
        Else
            itemx.Text = "新しいURL"
            itemx.SubItems.Add("http://")
        End If
        If ListView1.Items.Count < listmax Then
            ListView1.Items.Add(itemx)
            ListView1.Items(ListView1.Items.Count - 1).Selected = True
            ListView1.Focus()
        End If

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles del.Click

        If ListView1.SelectedItems.Count > 0 Then
            Dim i As Integer = ListView1.SelectedItems(0).Index
            ListView1.Items.Remove(ListView1.SelectedItems(0))
            ListView1.Focus()
            If ListView1.Items.Count = 0 Then

            ElseIf ListView1.Items.Count = i Then
                ListView1.Items(i - 1).Selected = True
            Else
                ListView1.Items(i).Selected = True
            End If
            ListView1_SelectedIndexChanged(sender, e)
        End If

    End Sub

    Private Function updatelist() As Boolean

        If ListView1.SelectedItems.Count > 0 Then
            Dim i As Integer = ListView1.SelectedItems(0).Index
            ListView1.Items(i).Text = TextBox1.Text.Trim
            ListView1.Items(i).SubItems(1).Text = TextBox2.Text.Trim
        End If

        Return True

    End Function

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles moveup.Click

        If ListView1.SelectedItems.Count > 0 Then
            Dim i As Integer = ListView1.SelectedItems(0).Index
            If i > 0 Then
                Dim b1 As String = ListView1.Items(i).Text
                Dim b2 As String = ListView1.Items(i).SubItems(1).Text
                ListView1.Items(i).Text = ListView1.Items(i - 1).Text
                ListView1.Items(i).SubItems(1).Text = ListView1.Items(i - 1).SubItems(1).Text
                ListView1.Items(i - 1).Text = b1
                ListView1.Items(i - 1).SubItems(1).Text = b2
                ListView1.Items(i - 1).Selected = True
            End If
            ListView1.Focus()
        End If


    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles movedown.Click

        If ListView1.SelectedItems.Count > 0 Then
            Dim i As Integer = ListView1.SelectedItems(0).Index
            If i < ListView1.Items.Count - 1 Then
                Dim b1 As String = ListView1.Items(i).Text
                Dim b2 As String = ListView1.Items(i).SubItems(1).Text
                ListView1.Items(i).Text = ListView1.Items(i + 1).Text
                ListView1.Items(i).SubItems(1).Text = ListView1.Items(i + 1).SubItems(1).Text
                ListView1.Items(i + 1).Text = b1
                ListView1.Items(i + 1).SubItems(1).Text = b2
                ListView1.Items(i + 1).Selected = True
            End If
            ListView1.Focus()
        End If

    End Sub


    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles OK.Click
        Dim sb As StringBuilder = New StringBuilder
        For Each l As ListViewItem In ListView1.Items
            sb.Append(l.Text)
            sb.Append(vbTab)
            sb.AppendLine(l.SubItems(1).Text)
        Next

        If Me.Text = "ランチャー編集" Then
            My.Settings.apps = sb.ToString.Trim
        Else
            My.Settings.urls = sb.ToString.Trim
        End If
        Me.Close()


    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles filedialog.Click
        Dim ofd As New OpenFileDialog()

        Dim r As New System.Text.RegularExpressions.Regex("^[c-zC-Z]:\\", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim m As System.Text.RegularExpressions.Match = r.Match(TextBox2.Text)


        If TextBox2.Text = "" Then
            ofd.InitialDirectory = "C:\Program Files"
        ElseIf m.Success = False Then
            ofd.InitialDirectory = Path.GetDirectoryName(Application.StartupPath & "\" & TextBox2.Text)
        Else
            ofd.InitialDirectory = Path.GetDirectoryName(TextBox2.Text)
        End If

        ofd.Filter = "EXEファイル(*.exe)|*.exe|BATファイル(*.bat)|*.bat"
        ofd.Title = "起動するEXE/BATを選んでください"
        If ofd.ShowDialog() = DialogResult.OK Then
            TextBox2.Text = ofd.FileName.Replace(Application.StartupPath & "\", "")
            TextBox1.Text = Path.GetFileNameWithoutExtension(ofd.FileName)
            updatelist()
        End If
    End Sub

    'Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.Validated, TextBox2.Validated
    '    If Me.Text = "ランチャー編集" Then
    '        If File.Exists(TextBox2.Text) = False Then
    '            MessageBox.Show(Me, "ファイルが存在しません", "エラー")
    '            Exit Sub
    '        End If
    '    Else
    '        Dim r As New Regex("^s?https?://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$")
    '        Dim m As Match = r.Match(TextBox2.Text)
    '        If m.Success = False Then
    '            MessageBox.Show(Me, "正しいURIではありません", "エラー")
    '            Exit Sub
    '        End If
    '    End If

    '    updatelist()
    'End Sub
End Class