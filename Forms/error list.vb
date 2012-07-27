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

End Class
