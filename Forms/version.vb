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
        If My.Settings.updatesever = True Then
            LinkLabel1.Location = New Point(LinkLabel1.Location.X + 20, LinkLabel1.Location.Y)
            LinkLabel1.Text = "GITHUB"
        End If

    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim m As MERGE
        m = CType(Me.Owner, MERGE)
        If My.Settings.updatesever = False Then
            Process.Start(m.browser, "http://code.google.com/p/mkijiro/source/browse/#svn%2Ftrunk%2FCODEDITOR%2FCDEMOD")
        Else

            Process.Start(m.browser, "https://github.com/haroturbo/CDEMOD")
        End If
    End Sub

    Private Sub CDEupate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CDEupdate.Click
        Dim check As New checkupdate
        Dim m As MERGE
        m = CType(Me.Owner, MERGE)
        If m.TopMost = True Then
            m.TopMost = False
        End If
        check.CDEupater("help")

        If My.Settings.TOP = True Then
            m.TopMost = True
        End If

    End Sub

End Class