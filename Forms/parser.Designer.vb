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
        Me.OK = New System.Windows.Forms.Button()
        Me.cancel = New System.Windows.Forms.Button()
        Me.clear = New System.Windows.Forms.Button()
        Me.CMT_FIX = New System.Windows.Forms.Button()
        Me.CODE_FIX = New System.Windows.Forms.Button()
        Me.NAME_FIX = New System.Windows.Forms.Button()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.編集ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.検索ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.置換ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.エラーリスト検索文字編集ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PARSE_CHECK = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.SEEK_ERROR = New System.Windows.Forms.Button()
        Me.FIND_REGEX = New System.Windows.Forms.ComboBox()
        Me.TX = New System.Windows.Forms.TextBox()
        Me.MenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'OK
        '
        Me.OK.Location = New System.Drawing.Point(6, 51)
        Me.OK.Name = "OK"
        Me.OK.Size = New System.Drawing.Size(75, 23)
        Me.OK.TabIndex = 1
        Me.OK.Text = "OK"
        Me.OK.UseVisualStyleBackColor = True
        '
        'cancel
        '
        Me.cancel.Location = New System.Drawing.Point(87, 51)
        Me.cancel.Name = "cancel"
        Me.cancel.Size = New System.Drawing.Size(75, 23)
        Me.cancel.TabIndex = 2
        Me.cancel.Text = "キャンセル"
        Me.cancel.UseVisualStyleBackColor = True
        '
        'clear
        '
        Me.clear.Location = New System.Drawing.Point(168, 51)
        Me.clear.Name = "clear"
        Me.clear.Size = New System.Drawing.Size(75, 23)
        Me.clear.TabIndex = 3
        Me.clear.Text = "クリア"
        Me.clear.UseVisualStyleBackColor = True
        '
        'CMT_FIX
        '
        Me.CMT_FIX.Location = New System.Drawing.Point(6, 8)
        Me.CMT_FIX.Name = "CMT_FIX"
        Me.CMT_FIX.Size = New System.Drawing.Size(75, 23)
        Me.CMT_FIX.TabIndex = 4
        Me.CMT_FIX.Text = "#補正"
        Me.CMT_FIX.UseVisualStyleBackColor = True
        '
        'CODE_FIX
        '
        Me.CODE_FIX.Location = New System.Drawing.Point(87, 8)
        Me.CODE_FIX.Name = "CODE_FIX"
        Me.CODE_FIX.Size = New System.Drawing.Size(75, 23)
        Me.CODE_FIX.TabIndex = 5
        Me.CODE_FIX.Text = "_L補正"
        Me.CODE_FIX.UseVisualStyleBackColor = True
        '
        'NAME_FIX
        '
        Me.NAME_FIX.Location = New System.Drawing.Point(168, 8)
        Me.NAME_FIX.Name = "NAME_FIX"
        Me.NAME_FIX.Size = New System.Drawing.Size(75, 23)
        Me.NAME_FIX.TabIndex = 6
        Me.NAME_FIX.Text = "_C補正"
        Me.NAME_FIX.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.編集ToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(283, 26)
        Me.MenuStrip1.TabIndex = 10
        Me.MenuStrip1.Text = "MenuStrip4"
        '
        '編集ToolStripMenuItem
        '
        Me.編集ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.検索ToolStripMenuItem, Me.置換ToolStripMenuItem, Me.エラーリスト検索文字編集ToolStripMenuItem})
        Me.編集ToolStripMenuItem.Name = "編集ToolStripMenuItem"
        Me.編集ToolStripMenuItem.Size = New System.Drawing.Size(44, 22)
        Me.編集ToolStripMenuItem.Text = "検索"
        '
        '検索ToolStripMenuItem
        '
        Me.検索ToolStripMenuItem.Name = "検索ToolStripMenuItem"
        Me.検索ToolStripMenuItem.Size = New System.Drawing.Size(220, 22)
        Me.検索ToolStripMenuItem.Text = "検索"
        '
        '置換ToolStripMenuItem
        '
        Me.置換ToolStripMenuItem.Name = "置換ToolStripMenuItem"
        Me.置換ToolStripMenuItem.Size = New System.Drawing.Size(220, 22)
        Me.置換ToolStripMenuItem.Text = "置換"
        '
        'エラーリスト検索文字編集ToolStripMenuItem
        '
        Me.エラーリスト検索文字編集ToolStripMenuItem.Name = "エラーリスト検索文字編集ToolStripMenuItem"
        Me.エラーリスト検索文字編集ToolStripMenuItem.Size = New System.Drawing.Size(220, 22)
        Me.エラーリスト検索文字編集ToolStripMenuItem.Text = "エラーリスト検索文字編集"
        '
        'PARSE_CHECK
        '
        Me.PARSE_CHECK.Location = New System.Drawing.Point(162, 16)
        Me.PARSE_CHECK.Name = "PARSE_CHECK"
        Me.PARSE_CHECK.Size = New System.Drawing.Size(69, 23)
        Me.PARSE_CHECK.TabIndex = 11
        Me.PARSE_CHECK.Text = "チェック"
        Me.PARSE_CHECK.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Controls.Add(Me.CMT_FIX)
        Me.Panel1.Controls.Add(Me.CODE_FIX)
        Me.Panel1.Controls.Add(Me.clear)
        Me.Panel1.Controls.Add(Me.NAME_FIX)
        Me.Panel1.Controls.Add(Me.OK)
        Me.Panel1.Controls.Add(Me.cancel)
        Me.Panel1.Location = New System.Drawing.Point(12, 257)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(259, 139)
        Me.Panel1.TabIndex = 13
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.SEEK_ERROR)
        Me.GroupBox1.Controls.Add(Me.PARSE_CHECK)
        Me.GroupBox1.Controls.Add(Me.FIND_REGEX)
        Me.GroupBox1.Location = New System.Drawing.Point(6, 80)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(237, 51)
        Me.GroupBox1.TabIndex = 14
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "エラーリスト検索/チェック"
        '
        'SEEK_ERROR
        '
        Me.SEEK_ERROR.Location = New System.Drawing.Point(94, 16)
        Me.SEEK_ERROR.Name = "SEEK_ERROR"
        Me.SEEK_ERROR.Size = New System.Drawing.Size(62, 23)
        Me.SEEK_ERROR.TabIndex = 14
        Me.SEEK_ERROR.Text = "検索"
        Me.SEEK_ERROR.UseVisualStyleBackColor = True
        '
        'FIND_REGEX
        '
        Me.FIND_REGEX.FormattingEnabled = True
        Me.FIND_REGEX.Items.AddRange(New Object() {"NULL"})
        Me.FIND_REGEX.Location = New System.Drawing.Point(6, 18)
        Me.FIND_REGEX.Name = "FIND_REGEX"
        Me.FIND_REGEX.Size = New System.Drawing.Size(82, 20)
        Me.FIND_REGEX.TabIndex = 13
        Me.FIND_REGEX.Text = "^[0-9A-Fa-f]{8}"
        '
        'TX
        '
        Me.TX.Location = New System.Drawing.Point(18, 29)
        Me.TX.MaxLength = 0
        Me.TX.Multiline = True
        Me.TX.Name = "TX"
        Me.TX.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TX.Size = New System.Drawing.Size(253, 222)
        Me.TX.TabIndex = 14
        '
        'parser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(283, 400)
        Me.Controls.Add(Me.TX)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Name = "parser"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "一括パーサー"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents OK As System.Windows.Forms.Button
    Friend WithEvents cancel As System.Windows.Forms.Button
    Friend WithEvents clear As System.Windows.Forms.Button
    Friend WithEvents CMT_FIX As System.Windows.Forms.Button
    Friend WithEvents CODE_FIX As System.Windows.Forms.Button
    Friend WithEvents NAME_FIX As System.Windows.Forms.Button
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents PARSE_CHECK As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents 編集ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 検索ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 置換ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FIND_REGEX As System.Windows.Forms.ComboBox
    Friend WithEvents SEEK_ERROR As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents TX As System.Windows.Forms.TextBox
    Friend WithEvents エラーリスト検索文字編集ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
