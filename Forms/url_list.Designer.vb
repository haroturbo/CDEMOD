<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class url_list
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
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.add = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.del = New System.Windows.Forms.Button()
        Me.moveup = New System.Windows.Forms.Button()
        Me.movedown = New System.Windows.Forms.Button()
        Me.OK = New System.Windows.Forms.Button()
        Me.filedialog = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ListView1
        '
        Me.ListView1.FullRowSelect = True
        Me.ListView1.Location = New System.Drawing.Point(28, 12)
        Me.ListView1.MultiSelect = False
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(151, 216)
        Me.ListView1.TabIndex = 0
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(199, 31)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(127, 19)
        Me.TextBox1.TabIndex = 2
        '
        'TextBox2
        '
        Me.TextBox2.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.TextBox2.Location = New System.Drawing.Point(199, 82)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(127, 19)
        Me.TextBox2.TabIndex = 3
        '
        'add
        '
        Me.add.Location = New System.Drawing.Point(199, 107)
        Me.add.Name = "add"
        Me.add.Size = New System.Drawing.Size(50, 23)
        Me.add.TabIndex = 4
        Me.add.Text = "追加"
        Me.add.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(199, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(29, 12)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "名前"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(199, 64)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(27, 12)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "URL"
        '
        'del
        '
        Me.del.Location = New System.Drawing.Point(199, 136)
        Me.del.Name = "del"
        Me.del.Size = New System.Drawing.Size(50, 23)
        Me.del.TabIndex = 7
        Me.del.Text = "削除"
        Me.del.UseVisualStyleBackColor = True
        '
        'moveup
        '
        Me.moveup.Location = New System.Drawing.Point(275, 107)
        Me.moveup.Name = "moveup"
        Me.moveup.Size = New System.Drawing.Size(51, 23)
        Me.moveup.TabIndex = 8
        Me.moveup.Text = "↑"
        Me.moveup.UseVisualStyleBackColor = True
        '
        'movedown
        '
        Me.movedown.Location = New System.Drawing.Point(275, 136)
        Me.movedown.Name = "movedown"
        Me.movedown.Size = New System.Drawing.Size(51, 23)
        Me.movedown.TabIndex = 9
        Me.movedown.Text = "↓"
        Me.movedown.UseVisualStyleBackColor = True
        '
        'OK
        '
        Me.OK.Location = New System.Drawing.Point(226, 205)
        Me.OK.Name = "OK"
        Me.OK.Size = New System.Drawing.Size(75, 23)
        Me.OK.TabIndex = 10
        Me.OK.Text = "OK"
        Me.OK.UseVisualStyleBackColor = True
        '
        'filedialog
        '
        Me.filedialog.Location = New System.Drawing.Point(332, 80)
        Me.filedialog.Name = "filedialog"
        Me.filedialog.Size = New System.Drawing.Size(23, 23)
        Me.filedialog.TabIndex = 11
        Me.filedialog.Text = "..."
        Me.filedialog.UseVisualStyleBackColor = True
        Me.filedialog.Visible = False
        '
        'url_list
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(362, 242)
        Me.Controls.Add(Me.filedialog)
        Me.Controls.Add(Me.OK)
        Me.Controls.Add(Me.movedown)
        Me.Controls.Add(Me.moveup)
        Me.Controls.Add(Me.del)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.add)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.ListView1)
        Me.Name = "url_list"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents add As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents del As System.Windows.Forms.Button
    Friend WithEvents moveup As System.Windows.Forms.Button
    Friend WithEvents movedown As System.Windows.Forms.Button
    Friend WithEvents OK As System.Windows.Forms.Button
    Friend WithEvents filedialog As System.Windows.Forms.Button
End Class
