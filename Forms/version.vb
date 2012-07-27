Imports System.Text.RegularExpressions
Imports System.IO


Public Class version
    Public Sub form2_load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim m As MERGE
        m = CType(Me.Owner, MERGE)
        Me.Location = New Point(m.Location.X + 500, m.Location.Y + 40)

        If m.fixedform.Checked = True Then
            Me.AutoSize = True
        End If

    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim m As MERGE
        m = CType(Me.Owner, MERGE)
        Process.Start(m.browser, "http://code.google.com/p/mkijiro/source/browse/#svn%2Ftrunk%2FCODEDITOR%2FCDEMOD")
    End Sub

    Private Sub CDEupate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CDEupdate.Click
        Dim check As New checkupdate
        check.CDEupater("help")
    End Sub

End Class