Imports System.Net.WebRequestMethods
Imports System.Diagnostics

Public Class frmMain
    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        Dim Open As New OpenFileDialog()
        Open.Filter = "Rich Text Document (*.rtf)|*.rtf|All files (*.*)|*.*"
        Open.CheckFileExists = True
        If Open.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Try
                rtbEditor.LoadFile(Open.FileName)
                Me.Text = "Helios TypeWriter - " & Open.FileName
            Catch ex As Exception
                ' Do nothing on Exception
            End Try
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewToolStripMenuItem.Click
        Dim Response As MsgBoxResult
        Response = MsgBox("Are you sure you want to start a New Document?",
                          MsgBoxStyle.Question + MsgBoxStyle.YesNo,
                          "Helios TypeWriter")
        If Response = MsgBoxResult.Yes Then
            rtbEditor.Clear()
            Me.Text = "Helios TypeWriter - Untitled"
        End If
    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
        Dim Save As New SaveFileDialog()
        Save.Filter = "Rich Text Document (*.rtf)|*.rtf|All files (*.*)|*.*"
        Save.CheckPathExists = True
        If Save.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Try
                rtbEditor.SaveFile(Save.FileName)
                Me.Text = "Helios TypeWriter - " & Save.FileName
            Catch ex As Exception
                ' Do nothing on Exception
            End Try
        End If
    End Sub

    Private Sub PrintToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrintToolStripMenuItem.Click
        Dim PrintDialog As New PrintDialog()
        PrintDialog.Document = rtbEditor.PrintDocument
        If PrintDialog.ShowDialog(Me) = DialogResult.OK Then
            Try
                rtbEditor.PrintDocument.Print() ' Print Document
            Catch ex As Exception
                ' Do nothing on Exception
            End Try
        End If
    End Sub

    Private Sub PrintPreviewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrintPreviewToolStripMenuItem.Click
        Dim PrintPreview As New PrintPreviewDialog()
        PrintPreview.Document = rtbEditor.PrintDocument
        PrintPreview.ShowDialog(Me)
    End Sub

    Private Sub PageSetupToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PageSetupToolStripMenuItem.Click
        Dim PageSetup As New PageSetupDialog
        PageSetup.Document = rtbEditor.PrintDocument
        If PageSetup.ShowDialog(Me) = DialogResult.OK Then
            Try
                rtbEditor.PrintDocument.PrinterSettings =
                PageSetup.PrinterSettings ' Set Page Settings
            Catch ex As Exception
                ' Do nothing on Exception
            End Try
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Dim Response As MsgBoxResult
        Response = MsgBox("Are you sure you want to exit Helios TypeWriter?",
                          MsgBoxStyle.Question + MsgBoxStyle.YesNo,
                          "Helios TypeWriter")
        If Response = MsgBoxResult.Yes Then
            End
        End If
    End Sub

    Private Sub CutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CutToolStripMenuItem.Click
        rtbEditor.Cut()
    End Sub

    Private Sub CopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToolStripMenuItem.Click
        rtbEditor.Copy()
    End Sub

    Private Sub PasteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PasteToolStripMenuItem.Click
        rtbEditor.Paste()
    End Sub

    Private Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click
        rtbEditor.SelectedText = ""
    End Sub

    Private Sub SelectAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectAllToolStripMenuItem.Click
        rtbEditor.SelectAll()
    End Sub

    Private Sub TimeDateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TimeDateToolStripMenuItem.Click
        rtbEditor.SelectedText = Format(Now, "HH:mm dd/MM/yyyy")
    End Sub

    Private Sub FontToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FontToolStripMenuItem.Click
        Dim Font As New FontDialog()
        Font.Font = rtbEditor.SelectionFont
        If Font.ShowDialog(Me) = DialogResult.OK Then
            Try
                rtbEditor.SelectionFont = Font.Font
            Catch ex As Exception
                ' Do nothing on Exception
            End Try
        End If
    End Sub

    Private Sub ColourToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ColourToolStripMenuItem.Click
        Dim Colour As New ColorDialog()
        Colour.Color = rtbEditor.SelectionColor
        If Colour.ShowDialog(Me) = DialogResult.OK Then
            Try
                rtbEditor.SelectionColor = Colour.Color
            Catch ex As Exception
                ' Do nothing on Exception
            End Try
        End If
    End Sub

    Private Sub HighlightToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HighlightToolStripMenuItem.Click
        Dim Colour As New ColorDialog()
        Colour.Color = rtbEditor.SelectionBackColor
        If Colour.ShowDialog(Me) = DialogResult.OK Then
            Try
                rtbEditor.SelectionBackColor = Colour.Color
            Catch ex As Exception
                ' Do nothing on Exception
            End Try
        End If
    End Sub

    Private Sub AlignLeftToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AlignLeftToolStripMenuItem.Click
        rtbEditor.SelectionAlignment = HorizontalAlignment.Left
        AlignLeftToolStripMenuItem.Checked = True
        ToolStripButton15.Checked = True
        AlignCentreToolStripMenuItem.Checked = False
        ToolStripButton16.Checked = False
        AlignRightToolStripMenuItem.Checked = False
        ToolStripButton17.Checked = False
    End Sub

    Private Sub ToolStripButton15_Click(sender As Object, e As EventArgs) Handles ToolStripButton15.Click
        rtbEditor.SelectionAlignment = HorizontalAlignment.Left
        AlignLeftToolStripMenuItem.Checked = True
        ToolStripButton15.Checked = True
        AlignCentreToolStripMenuItem.Checked = False
        ToolStripButton16.Checked = False
        AlignRightToolStripMenuItem.Checked = False
        ToolStripButton17.Checked = False
    End Sub

    Private Sub AlignCenterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AlignCentreToolStripMenuItem.Click
        rtbEditor.SelectionAlignment = HorizontalAlignment.Center
        AlignLeftToolStripMenuItem.Checked = False
        ToolStripButton15.Checked = False
        AlignCentreToolStripMenuItem.Checked = True
        ToolStripButton16.Checked = True
        AlignRightToolStripMenuItem.Checked = False
        ToolStripButton17.Checked = False
    End Sub

    Private Sub ToolStripButton16_Click(sender As Object, e As EventArgs) Handles ToolStripButton16.Click
        rtbEditor.SelectionAlignment = HorizontalAlignment.Center
        AlignLeftToolStripMenuItem.Checked = False
        ToolStripButton15.Checked = False
        AlignCentreToolStripMenuItem.Checked = True
        ToolStripButton16.Checked = True
        AlignRightToolStripMenuItem.Checked = False
        ToolStripButton17.Checked = False
    End Sub

    Private Sub AlignRightToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AlignRightToolStripMenuItem.Click
        rtbEditor.SelectionAlignment = HorizontalAlignment.Right
        AlignLeftToolStripMenuItem.Checked = False
        ToolStripButton15.Checked = False
        AlignCentreToolStripMenuItem.Checked = False
        ToolStripButton16.Checked = False
        AlignRightToolStripMenuItem.Checked = True
        ToolStripButton17.Checked = True
    End Sub

    Private Sub ToolStripButton17_Click(sender As Object, e As EventArgs) Handles ToolStripButton17.Click
        rtbEditor.SelectionAlignment = HorizontalAlignment.Right
        AlignLeftToolStripMenuItem.Checked = False
        ToolStripButton15.Checked = False
        AlignCentreToolStripMenuItem.Checked = False
        ToolStripButton16.Checked = False
        AlignRightToolStripMenuItem.Checked = True
        ToolStripButton17.Checked = True
    End Sub

    Private Sub BoldToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BoldToolStripMenuItem.Click
        If rtbEditor.SelectionFont.Bold Then
            rtbEditor.SelectionFont = New Font(rtbEditor.SelectionFont,
                                               rtbEditor.SelectionFont.Style And Not FontStyle.Bold)
        Else
            rtbEditor.SelectionFont = New Font(rtbEditor.SelectionFont,
                                               rtbEditor.SelectionFont.Style Or FontStyle.Bold)
        End If
        BoldToolStripMenuItem.Checked = rtbEditor.SelectionFont.Bold
        ToolStripButton11.Checked = rtbEditor.SelectionFont.Bold
    End Sub

    Private Sub ItalicToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ItalicsToolStripMenuItem.Click
        If rtbEditor.SelectionFont.Italic Then
            rtbEditor.SelectionFont = New Font(rtbEditor.SelectionFont,
                                               rtbEditor.SelectionFont.Style And Not FontStyle.Italic)
        Else
            rtbEditor.SelectionFont = New Font(rtbEditor.SelectionFont,
                                               rtbEditor.SelectionFont.Style Or FontStyle.Italic)
        End If
        ItalicsToolStripMenuItem.Checked = rtbEditor.SelectionFont.Italic
        ToolStripButton12.Checked = rtbEditor.SelectionFont.Italic
    End Sub

    Private Sub UnderlineToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UnderlineToolStripMenuItem.Click
        If rtbEditor.SelectionFont.Underline Then
            rtbEditor.SelectionFont = New Font(rtbEditor.SelectionFont,
                                               rtbEditor.SelectionFont.Style And Not FontStyle.Underline)
        Else
            rtbEditor.SelectionFont = New Font(rtbEditor.SelectionFont,
                                               rtbEditor.SelectionFont.Style Or FontStyle.Underline)
        End If
        UnderlineToolStripMenuItem.Checked = rtbEditor.SelectionFont.Underline
        ToolStripButton13.Checked = rtbEditor.SelectionFont.Underline
    End Sub

    Private Sub BulletsListsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BulletsToolStripMenuItem.Click
        rtbEditor.SelectionBullet = Not rtbEditor.SelectionBullet
        BulletsToolStripMenuItem.Checked = rtbEditor.SelectionBullet
        ToolStripButton14.Checked = rtbEditor.SelectionBullet
    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AlignLeftToolStripMenuItem.Checked = True
        Me.Text = "Helios TypeWriter - Untitled"
    End Sub

    Private Sub rtbEditor_SelectionChanged(sender As Object, e As EventArgs)
        AlignLeftToolStripMenuItem.Checked = False
        ToolStripButton15.Checked = False
        AlignCentreToolStripMenuItem.Checked = False
        ToolStripButton16.Checked = False
        AlignRightToolStripMenuItem.Checked = False
        ToolStripButton17.Checked = False
        Select Case rtbEditor.SelectionAlignment
            Case HorizontalAlignment.Left
                AlignLeftToolStripMenuItem.Checked = True
                ToolStripButton15.Checked = True
            Case HorizontalAlignment.Center
                AlignCentreToolStripMenuItem.Checked = True
                ToolStripButton16.Checked = True
            Case HorizontalAlignment.Right
                AlignRightToolStripMenuItem.Checked = True
                ToolStripButton16.Checked = True
        End Select

        ' Menu Items To Affect Text

        BoldToolStripMenuItem.Checked = rtbEditor.SelectionFont.Bold
        ItalicsToolStripMenuItem.Checked = rtbEditor.SelectionFont.Italic
        UnderlineToolStripMenuItem.Checked = rtbEditor.SelectionFont.Underline
        BulletsToolStripMenuItem.Checked = rtbEditor.SelectionBullet

        ' Toolstrip Items To Affect Text

        ToolStripButton11.Checked = rtbEditor.SelectionFont.Bold
        ToolStripButton12.Checked = rtbEditor.SelectionFont.Italic
        ToolStripButton13.Checked = rtbEditor.SelectionFont.Underline
        ToolStripButton14.Checked = rtbEditor.SelectionBullet
    End Sub

    Private Sub ToolStripSplitButton1_ButtonClick(sender As Object, e As EventArgs)

    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Dim Response As MsgBoxResult
        Response = MsgBox("Are you sure you want to start a New Document?",
                          MsgBoxStyle.Question + MsgBoxStyle.YesNo,
                          "Helios TypeWriter")
        If Response = MsgBoxResult.Yes Then
            rtbEditor.Clear()
            Me.Text = "Helios TypeWriter - Untitled"
        End If
    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        Dim Save As New SaveFileDialog()
        Save.Filter = "Rich Text Document (*.rtf)|*.rtf|All files (*.*)|*.*"
        Save.CheckPathExists = True
        If Save.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Try
                rtbEditor.SaveFile(Save.FileName)
                Me.Text = "Helios TypeWriter - " & Save.FileName
            Catch ex As Exception
                ' Do nothing on Exception
            End Try
        End If
    End Sub

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        Dim Open As New OpenFileDialog()
        Open.Filter = "Rich Text Document (*.rtf)|*.rtf|All files (*.*)|*.*"
        Open.CheckFileExists = True
        If Open.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Try
                rtbEditor.LoadFile(Open.FileName)
                Me.Text = "Helios TypeWriter - " & Open.FileName
            Catch ex As Exception
                ' Do nothing on Exception
            End Try
        End If
    End Sub

    Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click
        Dim PrintDialog As New PrintDialog()
        PrintDialog.Document = rtbEditor.PrintDocument
        If PrintDialog.ShowDialog(Me) = DialogResult.OK Then
            Try
                rtbEditor.PrintDocument.Print() ' Print Document
            Catch ex As Exception
                ' Do nothing on Exception
            End Try
        End If
    End Sub

    Private Sub ToolStripButton5_Click(sender As Object, e As EventArgs) Handles ToolStripButton5.Click
        rtbEditor.Undo()
    End Sub

    Private Sub ToolStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles ToolStrip1.ItemClicked

    End Sub

    Private Sub UndoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UndoToolStripMenuItem.Click
        rtbEditor.Undo()
    End Sub

    Private Sub RedoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RedoToolStripMenuItem.Click
        rtbEditor.Redo()
    End Sub

    Private Sub ToolStripButton6_Click(sender As Object, e As EventArgs) Handles ToolStripButton6.Click
        rtbEditor.Redo()
    End Sub

    Private Sub ToolStripButton7_Click(sender As Object, e As EventArgs) Handles ToolStripButton7.Click
        rtbEditor.Cut()
    End Sub

    Private Sub ToolStripButton8_Click(sender As Object, e As EventArgs) Handles ToolStripButton8.Click
        rtbEditor.Copy()
    End Sub

    Private Sub ToolStripButton9_Click(sender As Object, e As EventArgs) Handles ToolStripButton9.Click
        rtbEditor.Paste()
    End Sub

    Private Sub ToolStripButton10_Click(sender As Object, e As EventArgs) Handles ToolStripButton10.Click
        Dim Font As New FontDialog()
        Font.Font = rtbEditor.SelectionFont
        If Font.ShowDialog(Me) = DialogResult.OK Then
            Try
                rtbEditor.SelectionFont = Font.Font
            Catch ex As Exception
                ' Do nothing on Exception
            End Try
        End If
    End Sub

    Private Sub ToolStripSplitButton2_Click(sender As Object, e As EventArgs) Handles ToolStripSplitButton2.Click
        Dim Colour As New ColorDialog()
        Colour.Color = rtbEditor.SelectionColor
        If Colour.ShowDialog(Me) = DialogResult.OK Then
            Try
                rtbEditor.SelectionColor = Colour.Color
            Catch ex As Exception
                ' Do nothing on Exception
            End Try
        End If
    End Sub

    Private Sub ToolStripSplitButton1_Click(sender As Object, e As EventArgs) Handles ToolStripSplitButton1.Click
        Dim Colour As New ColorDialog()
        Colour.Color = rtbEditor.SelectionBackColor
        If Colour.ShowDialog(Me) = DialogResult.OK Then
            Try
                rtbEditor.SelectionBackColor = Colour.Color
            Catch ex As Exception
                ' Do nothing on Exception
            End Try
        End If
    End Sub

    Private Sub ToolStripButton11_Click(sender As Object, e As EventArgs) Handles ToolStripButton11.Click
        If rtbEditor.SelectionFont.Bold Then
            rtbEditor.SelectionFont = New Font(rtbEditor.SelectionFont,
                                               rtbEditor.SelectionFont.Style And Not FontStyle.Bold)
        Else
            rtbEditor.SelectionFont = New Font(rtbEditor.SelectionFont,
                                               rtbEditor.SelectionFont.Style Or FontStyle.Bold)
        End If
        ToolStripButton11.Checked = rtbEditor.SelectionFont.Bold
        BoldToolStripMenuItem.Checked = rtbEditor.SelectionFont.Bold
    End Sub

    Private Sub ToolStripButton12_Click(sender As Object, e As EventArgs) Handles ToolStripButton12.Click
        If rtbEditor.SelectionFont.Italic Then
            rtbEditor.SelectionFont = New Font(rtbEditor.SelectionFont,
                                               rtbEditor.SelectionFont.Style And Not FontStyle.Italic)
        Else
            rtbEditor.SelectionFont = New Font(rtbEditor.SelectionFont,
                                               rtbEditor.SelectionFont.Style Or FontStyle.Italic)
        End If
        ToolStripButton12.Checked = rtbEditor.SelectionFont.Italic
        ItalicsToolStripMenuItem.Checked = rtbEditor.SelectionFont.Italic
    End Sub

    Private Sub ToolStripButton13_Click(sender As Object, e As EventArgs) Handles ToolStripButton13.Click
        If rtbEditor.SelectionFont.Underline Then
            rtbEditor.SelectionFont = New Font(rtbEditor.SelectionFont,
                                               rtbEditor.SelectionFont.Style And Not FontStyle.Underline)
        Else
            rtbEditor.SelectionFont = New Font(rtbEditor.SelectionFont,
                                               rtbEditor.SelectionFont.Style Or FontStyle.Underline)
        End If
        ToolStripButton13.Checked = rtbEditor.SelectionFont.Underline
        UnderlineToolStripMenuItem.Checked = rtbEditor.SelectionFont.Underline
    End Sub

    Private Sub TypeWriterOnGitHubToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TypeWriterOnGitHubToolStripMenuItem.Click
        Dim webAddress As String = "https://github.com/Slate-Technologies/helios-typewriter"
        Process.Start(webAddress)
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        Dim Response As MsgBoxResult
        Response = MsgBox("Helios TypeWriter v2.0" + Environment.NewLine + "© Slate Technologies 2020. All rights reserved.",
                          MsgBoxStyle.Information + MsgBoxStyle.OkOnly,
                          "Helios TypeWriter")
    End Sub

    Private Sub ViewLicenseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewLicenseToolStripMenuItem.Click
        Dim licenseAddress As String = "https://raw.githubusercontent.com/Slate-Technologies/helios-typewriter/master/LICENSE"
        Process.Start(licenseAddress)
    End Sub

    Private Sub ToolStripButton14_Click(sender As Object, e As EventArgs) Handles ToolStripButton14.Click
        rtbEditor.SelectionBullet = Not rtbEditor.SelectionBullet
        ToolStripButton14.Checked = rtbEditor.SelectionBullet
        BulletsToolStripMenuItem.Checked = rtbEditor.SelectionBullet
    End Sub

    Private Sub OpenPlainTextEditorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenPlainTextEditorToolStripMenuItem.Click
        Process.Start("C:\Windows\System32\notepad.exe")
    End Sub
End Class
