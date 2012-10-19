<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class error_window
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(error_window))
        Me.list_load_error = New System.Windows.Forms.ListView()
        Me.col_error = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.col_line = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.col_game = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.col_title = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.col_linetext = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.コピーToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.全て選択ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tab_error = New System.Windows.Forms.TabControl()
        Me.tab_load = New System.Windows.Forms.TabPage()
        Me.tab_save = New System.Windows.Forms.TabPage()
        Me.list_save_error = New System.Windows.Forms.ListView()
        Me.scol_error = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.scol_game = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.scol_codetitle = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.scol_reason = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tab_parse = New System.Windows.Forms.TabPage()
        Me.list_parse_error = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.HTML = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tab_error.SuspendLayout()
        Me.tab_load.SuspendLayout()
        Me.tab_save.SuspendLayout()
        Me.tab_parse.SuspendLayout()
        Me.SuspendLayout()
        '
        'list_load_error
        '
        Me.list_load_error.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.col_error, Me.col_line, Me.col_game, Me.col_title, Me.col_linetext})
        Me.list_load_error.ContextMenuStrip = Me.ContextMenuStrip1
        Me.list_load_error.Dock = System.Windows.Forms.DockStyle.Fill
        Me.list_load_error.FullRowSelect = True
        Me.list_load_error.GridLines = True
        Me.list_load_error.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.list_load_error.Location = New System.Drawing.Point(3, 3)
        Me.list_load_error.Name = "list_load_error"
        Me.list_load_error.Size = New System.Drawing.Size(604, 140)
        Me.list_load_error.TabIndex = 0
        Me.list_load_error.UseCompatibleStateImageBehavior = False
        Me.list_load_error.View = System.Windows.Forms.View.Details
        '
        'col_error
        '
        Me.col_error.Text = "エラー #"
        Me.col_error.Width = 63
        '
        'col_line
        '
        Me.col_line.Text = "行数 #"
        Me.col_line.Width = 57
        '
        'col_game
        '
        Me.col_game.Text = "ゲーム名"
        Me.col_game.Width = 94
        '
        'col_title
        '
        Me.col_title.Text = "コード名"
        Me.col_title.Width = 178
        '
        'col_linetext
        '
        Me.col_linetext.Text = "テキスト内容"
        Me.col_linetext.Width = 300
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.コピーToolStripMenuItem, Me.全て選択ToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(173, 70)
        '
        'コピーToolStripMenuItem
        '
        Me.コピーToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HTML})
        Me.コピーToolStripMenuItem.Name = "コピーToolStripMenuItem"
        Me.コピーToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
        Me.コピーToolStripMenuItem.Text = "選択箇所のコピー"
        '
        '全て選択ToolStripMenuItem
        '
        Me.全て選択ToolStripMenuItem.Name = "全て選択ToolStripMenuItem"
        Me.全て選択ToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
        Me.全て選択ToolStripMenuItem.Text = "全て選択"
        '
        'tab_error
        '
        Me.tab_error.Controls.Add(Me.tab_load)
        Me.tab_error.Controls.Add(Me.tab_save)
        Me.tab_error.Controls.Add(Me.tab_parse)
        Me.tab_error.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tab_error.Location = New System.Drawing.Point(0, 0)
        Me.tab_error.Name = "tab_error"
        Me.tab_error.SelectedIndex = 0
        Me.tab_error.Size = New System.Drawing.Size(618, 172)
        Me.tab_error.TabIndex = 1
        '
        'tab_load
        '
        Me.tab_load.Controls.Add(Me.list_load_error)
        Me.tab_load.Location = New System.Drawing.Point(4, 22)
        Me.tab_load.Name = "tab_load"
        Me.tab_load.Padding = New System.Windows.Forms.Padding(3)
        Me.tab_load.Size = New System.Drawing.Size(610, 146)
        Me.tab_load.TabIndex = 0
        Me.tab_load.Text = "読み込み時エラー"
        Me.tab_load.UseVisualStyleBackColor = True
        '
        'tab_save
        '
        Me.tab_save.Controls.Add(Me.list_save_error)
        Me.tab_save.Location = New System.Drawing.Point(4, 22)
        Me.tab_save.Name = "tab_save"
        Me.tab_save.Padding = New System.Windows.Forms.Padding(3)
        Me.tab_save.Size = New System.Drawing.Size(610, 146)
        Me.tab_save.TabIndex = 1
        Me.tab_save.Text = "保存時エラー"
        Me.tab_save.UseVisualStyleBackColor = True
        '
        'list_save_error
        '
        Me.list_save_error.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.scol_error, Me.scol_game, Me.scol_codetitle, Me.scol_reason})
        Me.list_save_error.ContextMenuStrip = Me.ContextMenuStrip1
        Me.list_save_error.Dock = System.Windows.Forms.DockStyle.Fill
        Me.list_save_error.FullRowSelect = True
        Me.list_save_error.GridLines = True
        Me.list_save_error.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.list_save_error.Location = New System.Drawing.Point(3, 3)
        Me.list_save_error.Name = "list_save_error"
        Me.list_save_error.Size = New System.Drawing.Size(604, 140)
        Me.list_save_error.TabIndex = 0
        Me.list_save_error.UseCompatibleStateImageBehavior = False
        Me.list_save_error.View = System.Windows.Forms.View.Details
        '
        'scol_error
        '
        Me.scol_error.Text = "エラー #"
        Me.scol_error.Width = 62
        '
        'scol_game
        '
        Me.scol_game.Text = "ゲーム"
        Me.scol_game.Width = 134
        '
        'scol_codetitle
        '
        Me.scol_codetitle.Text = "コード名"
        Me.scol_codetitle.Width = 167
        '
        'scol_reason
        '
        Me.scol_reason.Text = "保存エラー"
        Me.scol_reason.Width = 300
        '
        'tab_parse
        '
        Me.tab_parse.Controls.Add(Me.list_parse_error)
        Me.tab_parse.Location = New System.Drawing.Point(4, 22)
        Me.tab_parse.Name = "tab_parse"
        Me.tab_parse.Padding = New System.Windows.Forms.Padding(3)
        Me.tab_parse.Size = New System.Drawing.Size(610, 146)
        Me.tab_parse.TabIndex = 2
        Me.tab_parse.Text = "パーサー"
        Me.tab_parse.UseVisualStyleBackColor = True
        '
        'list_parse_error
        '
        Me.list_parse_error.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader6, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5})
        Me.list_parse_error.ContextMenuStrip = Me.ContextMenuStrip1
        Me.list_parse_error.Dock = System.Windows.Forms.DockStyle.Fill
        Me.list_parse_error.FullRowSelect = True
        Me.list_parse_error.GridLines = True
        Me.list_parse_error.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.list_parse_error.Location = New System.Drawing.Point(3, 3)
        Me.list_parse_error.Name = "list_parse_error"
        Me.list_parse_error.Size = New System.Drawing.Size(604, 140)
        Me.list_parse_error.TabIndex = 1
        Me.list_parse_error.UseCompatibleStateImageBehavior = False
        Me.list_parse_error.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "エラー #"
        Me.ColumnHeader1.Width = 63
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "行数 #"
        Me.ColumnHeader2.Width = 57
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "#位置"
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "ゲーム名"
        Me.ColumnHeader3.Width = 83
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "コード名"
        Me.ColumnHeader4.Width = 178
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "テキスト内容"
        Me.ColumnHeader5.Width = 300
        '
        'HTML
        '
        Me.HTML.Name = "HTML"
        Me.HTML.Size = New System.Drawing.Size(152, 22)
        Me.HTML.Text = "HTMLに変換"
        '
        'error_window
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(618, 172)
        Me.Controls.Add(Me.tab_error)
        Me.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "error_window"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "エラーリスト"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tab_error.ResumeLayout(False)
        Me.tab_load.ResumeLayout(False)
        Me.tab_save.ResumeLayout(False)
        Me.tab_parse.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents list_load_error As System.Windows.Forms.ListView
    Friend WithEvents col_error As System.Windows.Forms.ColumnHeader
    Friend WithEvents col_line As System.Windows.Forms.ColumnHeader
    Friend WithEvents col_game As System.Windows.Forms.ColumnHeader
    Friend WithEvents col_title As System.Windows.Forms.ColumnHeader
    Friend WithEvents col_linetext As System.Windows.Forms.ColumnHeader
    Friend WithEvents tab_error As System.Windows.Forms.TabControl
    Friend WithEvents tab_load As System.Windows.Forms.TabPage
    Friend WithEvents tab_save As System.Windows.Forms.TabPage
    Friend WithEvents list_save_error As System.Windows.Forms.ListView
    Friend WithEvents scol_error As System.Windows.Forms.ColumnHeader
    Friend WithEvents scol_game As System.Windows.Forms.ColumnHeader
    Friend WithEvents scol_codetitle As System.Windows.Forms.ColumnHeader
    Friend WithEvents scol_reason As System.Windows.Forms.ColumnHeader
    Friend WithEvents tab_parse As System.Windows.Forms.TabPage
    Friend WithEvents list_parse_error As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents コピーToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 全て選択ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HTML As System.Windows.Forms.ToolStripMenuItem

End Class
