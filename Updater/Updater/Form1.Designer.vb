<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UpdaterFrm
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
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

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UpdaterFrm))
        Me.Pbar = New System.Windows.Forms.ProgressBar()
        Me.StatusLb = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Pbar
        '
        Me.Pbar.Location = New System.Drawing.Point(12, 12)
        Me.Pbar.Name = "Pbar"
        Me.Pbar.Size = New System.Drawing.Size(284, 23)
        Me.Pbar.TabIndex = 0
        '
        'StatusLb
        '
        Me.StatusLb.AutoSize = True
        Me.StatusLb.Location = New System.Drawing.Point(12, 38)
        Me.StatusLb.Name = "StatusLb"
        Me.StatusLb.Size = New System.Drawing.Size(59, 13)
        Me.StatusLb.TabIndex = 1
        Me.StatusLb.Text = "BlueSheep"
        '
        'UpdaterFrm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.DeepSkyBlue
        Me.ClientSize = New System.Drawing.Size(308, 55)
        Me.Controls.Add(Me.StatusLb)
        Me.Controls.Add(Me.Pbar)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "UpdaterFrm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Updater"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Pbar As System.Windows.Forms.ProgressBar
    Friend WithEvents StatusLb As System.Windows.Forms.Label

End Class
