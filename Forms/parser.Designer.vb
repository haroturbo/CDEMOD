<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class parser
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TX = New System.Windows.Forms.TextBox()
        Me.OK = New System.Windows.Forms.Button()
        Me.cancel = New System.Windows.Forms.Button()
        Me.clear = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.MenuStrip2 = New System.Windows.Forms.MenuStrip()
        Me.MenuStrip3 = New System.Windows.Forms.MenuStrip()
        Me.MenuStrip4 = New System.Windows.Forms.MenuStrip()
        Me.検索ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.置換ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip4.SuspendLayout()
        Me.SuspendLayout()
        '
        'TX
        '
        Me.TX.Location = New System.Drawing.Point(12, 12)
        Me.TX.MaxLength = 0
        Me.TX.Multiline = True
        Me.TX.Name = "TX"
        Me.TX.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TX.Size = New System.Drawing.Size(259, 202)
        Me.TX.TabIndex = 0
        '
        'OK
        '
        Me.OK.Location = New System.Drawing.Point(12, 272)
        Me.OK.Name = "OK"
        Me.OK.Size = New System.Drawing.Size(75, 23)
        Me.OK.TabIndex = 1
        Me.OK.Text = "OK"
        Me.OK.UseVisualStyleBackColor = True
        '
        'cancel
        '
        Me.cancel.Location = New System.Drawing.Point(106, 272)
        Me.cancel.Name = "cancel"
        Me.cancel.Size = New System.Drawing.Size(75, 23)
        Me.cancel.TabIndex = 2
        Me.cancel.Text = "キャンセル"
        Me.cancel.UseVisualStyleBackColor = True
        '
        'clear
        '
        Me.clear.Location = New System.Drawing.Point(196, 272)
        Me.clear.Name = "clear"
        Me.clear.Size = New System.Drawing.Size(75, 23)
        Me.clear.TabIndex = 3
        Me.clear.Text = "クリア"
        Me.clear.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(12, 232)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "#補正"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(106, 232)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 5
        Me.Button2.Text = "_L補正"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(196, 232)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(75, 23)
        Me.Button3.TabIndex = 6
        Me.Button3.Text = "_C補正"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 72)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(284, 24)
        Me.MenuStrip1.TabIndex = 7
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'MenuStrip2
        '
        Me.MenuStrip2.Location = New System.Drawing.Point(0, 48)
        Me.MenuStrip2.Name = "MenuStrip2"
        Me.MenuStrip2.Size = New System.Drawing.Size(284, 24)
        Me.MenuStrip2.TabIndex = 8
        Me.MenuStrip2.Text = "MenuStrip2"
        '
        'MenuStrip3
        '
        Me.MenuStrip3.Location = New System.Drawing.Point(0, 24)
        Me.MenuStrip3.Name = "MenuStrip3"
        Me.MenuStrip3.Size = New System.Drawing.Size(284, 24)
        Me.MenuStrip3.TabIndex = 9
        Me.MenuStrip3.Text = "MenuStrip3"
        '
        'MenuStrip4
        '
        Me.MenuStrip4.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.検索ToolStripMenuItem, Me.置換ToolStripMenuItem})
        Me.MenuStrip4.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip4.Name = "MenuStrip4"
        Me.MenuStrip4.Size = New System.Drawing.Size(284, 24)
        Me.MenuStrip4.TabIndex = 10
        Me.MenuStrip4.Text = "MenuStrip4"
        '
        '検索ToolStripMenuItem
        '
        Me.検索ToolStripMenuItem.Name = "検索ToolStripMenuItem"
        Me.検索ToolStripMenuItem.Size = New System.Drawing.Size(41, 20)
        Me.検索ToolStripMenuItem.Text = "検索"
        Me.検索ToolStripMenuItem.Visible = False
        '
        '置換ToolStripMenuItem
        '
        Me.置換ToolStripMenuItem.Name = "置換ToolStripMenuItem"
        Me.置換ToolStripMenuItem.Size = New System.Drawing.Size(41, 20)
        Me.置換ToolStripMenuItem.Text = "置換"
        Me.置換ToolStripMenuItem.Visible = False
        '
        'parser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 303)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.clear)
        Me.Controls.Add(Me.cancel)
        Me.Controls.Add(Me.OK)
        Me.Controls.Add(Me.TX)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.MenuStrip2)
        Me.Controls.Add(Me.MenuStrip3)
        Me.Controls.Add(Me.MenuStrip4)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "parser"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "一括パーサー"
        Me.MenuStrip4.ResumeLayout(False)
        Me.MenuStrip4.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TX As System.Windows.Forms.TextBox
    Friend WithEvents OK As System.Windows.Forms.Button
    Friend WithEvents cancel As System.Windows.Forms.Button
    Friend WithEvents clear As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents MenuStrip2 As System.Windows.Forms.MenuStrip
    Friend WithEvents MenuStrip3 As System.Windows.Forms.MenuStrip
    Friend WithEvents MenuStrip4 As System.Windows.Forms.MenuStrip
    Friend WithEvents 検索ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 置換ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
