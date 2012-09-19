<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class list
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
        Me.APPLY = New System.Windows.Forms.Button()
        Me.lsclose = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.rb10 = New System.Windows.Forms.RadioButton()
        Me.rb9 = New System.Windows.Forms.RadioButton()
        Me.rb8 = New System.Windows.Forms.RadioButton()
        Me.rb7 = New System.Windows.Forms.RadioButton()
        Me.rb6 = New System.Windows.Forms.RadioButton()
        Me.rb5 = New System.Windows.Forms.RadioButton()
        Me.rb4 = New System.Windows.Forms.RadioButton()
        Me.rb3 = New System.Windows.Forms.RadioButton()
        Me.rb2 = New System.Windows.Forms.RadioButton()
        Me.rb1 = New System.Windows.Forms.RadioButton()
        Me.grid = New System.Windows.Forms.CheckBox()
        Me.NumericUpDown1 = New System.Windows.Forms.NumericUpDown()
        Me.ls_save = New System.Windows.Forms.CheckBox()
        Me.zebra = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ListView1
        '
        Me.ListView1.Location = New System.Drawing.Point(12, 12)
        Me.ListView1.MultiSelect = False
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(211, 247)
        Me.ListView1.TabIndex = 2
        Me.ListView1.UseCompatibleStateImageBehavior = False
        '
        'APPLY
        '
        Me.APPLY.Location = New System.Drawing.Point(12, 265)
        Me.APPLY.Name = "APPLY"
        Me.APPLY.Size = New System.Drawing.Size(57, 23)
        Me.APPLY.TabIndex = 5
        Me.APPLY.Text = "適用"
        Me.APPLY.UseVisualStyleBackColor = True
        '
        'lsclose
        '
        Me.lsclose.Location = New System.Drawing.Point(75, 265)
        Me.lsclose.Name = "lsclose"
        Me.lsclose.Size = New System.Drawing.Size(58, 23)
        Me.lsclose.TabIndex = 6
        Me.lsclose.Text = "閉じる"
        Me.lsclose.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rb10)
        Me.GroupBox1.Controls.Add(Me.rb9)
        Me.GroupBox1.Controls.Add(Me.rb8)
        Me.GroupBox1.Controls.Add(Me.rb7)
        Me.GroupBox1.Controls.Add(Me.rb6)
        Me.GroupBox1.Controls.Add(Me.rb5)
        Me.GroupBox1.Controls.Add(Me.rb4)
        Me.GroupBox1.Controls.Add(Me.rb3)
        Me.GroupBox1.Controls.Add(Me.rb2)
        Me.GroupBox1.Controls.Add(Me.rb1)
        Me.GroupBox1.Location = New System.Drawing.Point(229, 28)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(106, 231)
        Me.GroupBox1.TabIndex = 9
        Me.GroupBox1.TabStop = False
        '
        'rb10
        '
        Me.rb10.AutoSize = True
        Me.rb10.Location = New System.Drawing.Point(7, 213)
        Me.rb10.Name = "rb10"
        Me.rb10.Size = New System.Drawing.Size(37, 16)
        Me.rb10.TabIndex = 9
        Me.rb10.Text = "10:"
        Me.rb10.UseVisualStyleBackColor = True
        '
        'rb9
        '
        Me.rb9.AutoSize = True
        Me.rb9.Location = New System.Drawing.Point(7, 190)
        Me.rb9.Name = "rb9"
        Me.rb9.Size = New System.Drawing.Size(31, 16)
        Me.rb9.TabIndex = 8
        Me.rb9.Text = "9:"
        Me.rb9.UseVisualStyleBackColor = True
        '
        'rb8
        '
        Me.rb8.AutoSize = True
        Me.rb8.Location = New System.Drawing.Point(7, 167)
        Me.rb8.Name = "rb8"
        Me.rb8.Size = New System.Drawing.Size(31, 16)
        Me.rb8.TabIndex = 7
        Me.rb8.Text = "8:"
        Me.rb8.UseVisualStyleBackColor = True
        '
        'rb7
        '
        Me.rb7.AutoSize = True
        Me.rb7.Location = New System.Drawing.Point(7, 144)
        Me.rb7.Name = "rb7"
        Me.rb7.Size = New System.Drawing.Size(31, 16)
        Me.rb7.TabIndex = 6
        Me.rb7.Text = "7:"
        Me.rb7.UseVisualStyleBackColor = True
        '
        'rb6
        '
        Me.rb6.AutoSize = True
        Me.rb6.Location = New System.Drawing.Point(7, 121)
        Me.rb6.Name = "rb6"
        Me.rb6.Size = New System.Drawing.Size(31, 16)
        Me.rb6.TabIndex = 5
        Me.rb6.Text = "6:"
        Me.rb6.UseVisualStyleBackColor = True
        '
        'rb5
        '
        Me.rb5.AutoSize = True
        Me.rb5.Location = New System.Drawing.Point(7, 97)
        Me.rb5.Name = "rb5"
        Me.rb5.Size = New System.Drawing.Size(31, 16)
        Me.rb5.TabIndex = 4
        Me.rb5.Text = "5:"
        Me.rb5.UseVisualStyleBackColor = True
        '
        'rb4
        '
        Me.rb4.AutoSize = True
        Me.rb4.Location = New System.Drawing.Point(7, 75)
        Me.rb4.Name = "rb4"
        Me.rb4.Size = New System.Drawing.Size(31, 16)
        Me.rb4.TabIndex = 3
        Me.rb4.Text = "4:"
        Me.rb4.UseVisualStyleBackColor = True
        '
        'rb3
        '
        Me.rb3.AutoSize = True
        Me.rb3.Location = New System.Drawing.Point(7, 52)
        Me.rb3.Name = "rb3"
        Me.rb3.Size = New System.Drawing.Size(31, 16)
        Me.rb3.TabIndex = 2
        Me.rb3.Text = "3:"
        Me.rb3.UseVisualStyleBackColor = True
        '
        'rb2
        '
        Me.rb2.AutoSize = True
        Me.rb2.Location = New System.Drawing.Point(7, 29)
        Me.rb2.Name = "rb2"
        Me.rb2.Size = New System.Drawing.Size(31, 16)
        Me.rb2.TabIndex = 1
        Me.rb2.Text = "2:"
        Me.rb2.UseVisualStyleBackColor = True
        '
        'rb1
        '
        Me.rb1.AutoSize = True
        Me.rb1.Location = New System.Drawing.Point(7, 7)
        Me.rb1.Name = "rb1"
        Me.rb1.Size = New System.Drawing.Size(31, 16)
        Me.rb1.TabIndex = 0
        Me.rb1.Text = "1:"
        Me.rb1.UseVisualStyleBackColor = True
        '
        'grid
        '
        Me.grid.AutoSize = True
        Me.grid.Location = New System.Drawing.Point(229, 269)
        Me.grid.Name = "grid"
        Me.grid.Size = New System.Drawing.Size(57, 16)
        Me.grid.TabIndex = 10
        Me.grid.Text = "ぐりっど"
        Me.grid.UseVisualStyleBackColor = True
        '
        'NumericUpDown1
        '
        Me.NumericUpDown1.Location = New System.Drawing.Point(275, 12)
        Me.NumericUpDown1.Maximum = New Decimal(New Integer() {9, 0, 0, 0})
        Me.NumericUpDown1.Name = "NumericUpDown1"
        Me.NumericUpDown1.Size = New System.Drawing.Size(31, 19)
        Me.NumericUpDown1.TabIndex = 11
        '
        'ls_save
        '
        Me.ls_save.AutoSize = True
        Me.ls_save.Location = New System.Drawing.Point(139, 269)
        Me.ls_save.Name = "ls_save"
        Me.ls_save.Size = New System.Drawing.Size(84, 16)
        Me.ls_save.TabIndex = 12
        Me.ls_save.Text = "適用時保存"
        Me.ls_save.UseVisualStyleBackColor = True
        '
        'zebra
        '
        Me.zebra.AutoSize = True
        Me.zebra.Location = New System.Drawing.Point(285, 268)
        Me.zebra.Name = "zebra"
        Me.zebra.Size = New System.Drawing.Size(60, 16)
        Me.zebra.TabIndex = 13
        Me.zebra.Text = "しましま"
        Me.zebra.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(234, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(35, 12)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "ページ"
        '
        'list
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(347, 296)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.zebra)
        Me.Controls.Add(Me.ls_save)
        Me.Controls.Add(Me.NumericUpDown1)
        Me.Controls.Add(Me.grid)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lsclose)
        Me.Controls.Add(Me.APPLY)
        Me.Controls.Add(Me.ListView1)
        Me.Location = New System.Drawing.Point(100, 100)
        Me.Name = "list"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "リスト"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents APPLY As System.Windows.Forms.Button
    Friend WithEvents lsclose As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rb10 As System.Windows.Forms.RadioButton
    Friend WithEvents rb9 As System.Windows.Forms.RadioButton
    Friend WithEvents rb8 As System.Windows.Forms.RadioButton
    Friend WithEvents rb7 As System.Windows.Forms.RadioButton
    Friend WithEvents rb6 As System.Windows.Forms.RadioButton
    Friend WithEvents rb5 As System.Windows.Forms.RadioButton
    Friend WithEvents rb4 As System.Windows.Forms.RadioButton
    Friend WithEvents rb3 As System.Windows.Forms.RadioButton
    Friend WithEvents rb2 As System.Windows.Forms.RadioButton
    Friend WithEvents rb1 As System.Windows.Forms.RadioButton
    Friend WithEvents grid As System.Windows.Forms.CheckBox
    Friend WithEvents NumericUpDown1 As System.Windows.Forms.NumericUpDown
    Friend WithEvents ls_save As System.Windows.Forms.CheckBox
    Friend WithEvents zebra As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
