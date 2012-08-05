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
End Class
