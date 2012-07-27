Public Class url


    Private Sub form3_load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim m As MERGE
        m = CType(Me.Owner, MERGE)
        Me.Location = New Point(m.Location.X + 400, m.Location.Y + 80)
        If TextBox1.Text <= "" Then
            TextBox1.Text = "http://"
        End If

        If m.fixedform.Checked = True Then
                Me.AutoSize = True
        End If

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If System.Text.RegularExpressions.Regex.IsMatch( _
    TextBox1.Text, _
    "^s?https?://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$") Then
            Me.Close()
        Else
            MessageBox.Show("不正なURLです。", "入力エラー")
        End If
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

    End Sub
End Class