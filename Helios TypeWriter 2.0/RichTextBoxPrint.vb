Imports System.Drawing.Printing
Imports System.Runtime.InteropServices
Public Class RichTextBoxPrint
    Inherits RichTextBox

    ' Private Constants

    Private Const AnInch As Double = 14.4 ' Convert .NET Units to Win32 API Units
    Private Const WM_USER As Integer = &H400
    Private Const EM_FORMATRANGE As Integer = WM_USER + 57

    ' API Structures and Method

    <StructLayout(LayoutKind.Sequential)>
    Private Structure RECT
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer
    End Structure
    <StructLayout(LayoutKind.Sequential)>
    Private Structure CHARRANGE
        Public cpMin As Integer ' First character of range (0 for start of doc)
        Public cpMax As Integer ' Last character of range (-1 for end of doc)
    End Structure
    <StructLayout(LayoutKind.Sequential)>
    Private Structure FORMATRANGE
        Public hdc As IntPtr ' Actual Device Context to draw on
        Public hdcTarget As IntPtr ' Target DC for text formatting
        Public rc As RECT ' Region of the DC to draw to in twips
        Public rcPage As RECT ' Region of the whole DC (page size) in twips
        Public chrg As CHARRANGE ' Range of text to draw
    End Structure

    Private Declare Function SendMessage Lib "USER32" Alias "SendMessageA" _
    (ByVal hWnd As IntPtr, ByVal msg As Integer,
     ByVal wp As IntPtr, ByVal lp As IntPtr) As IntPtr

    ' Private and Public Members

    Private checkPrint As Integer = 0 ' Check Print Value
    Public WithEvents PrintDocument As New PrintDocument ' Printer Document

    ' Name    : Print
    ' Params  : charFrom as Integer, charTo as Integer, e as PrintPageEventArg
    ' Returns : Print as Integer
    ' Desc    : Produce RichTextBox Printable Output
    Private Function Print(ByVal charFrom As Integer, ByVal charTo As Integer,
                           ByVal e As PrintPageEventArgs) As Integer
        Dim cRange As CHARRANGE
        Dim rectToPrint As RECT
        Dim rectPage As RECT
        Dim hdc As IntPtr = e.Graphics.GetHdc()
        Dim fmtRange As FORMATRANGE
        Dim res As IntPtr = IntPtr.Zero
        Dim wparam As IntPtr = IntPtr.Zero
        Dim lparam As IntPtr = IntPtr.Zero
        ' Mark start and end character
        cRange.cpMin = charFrom
        cRange.cpMax = charTo
        ' Calculate the area to render and print
        rectToPrint.Top = CInt(e.MarginBounds.Top * AnInch)
        rectToPrint.Bottom = CInt(e.MarginBounds.Bottom * AnInch)
        rectToPrint.Left = CInt(e.MarginBounds.Left * AnInch)
        rectToPrint.Right = CInt(e.MarginBounds.Right * AnInch)
        ' Calculate the size of the page
        rectPage.Top = CInt(e.PageBounds.Top * AnInch)
        rectPage.Bottom = CInt(e.PageBounds.Bottom * AnInch)
        rectPage.Left = CInt(e.PageBounds.Left * AnInch)
        rectPage.Right = CInt(e.PageBounds.Right * AnInch)
        ' Format Range
        fmtRange.chrg = cRange ' Character from to character to 
        fmtRange.hdc = hdc  ' Same device context for measuring and rendering
        fmtRange.hdcTarget = hdc ' Point at printer hDC
        fmtRange.rc = rectToPrint ' Area on page to print
        fmtRange.rcPage = rectPage ' Whole size of page
        ' API
        wparam = New IntPtr(1)
        lparam = Marshal.AllocCoTaskMem(Marshal.SizeOf(fmtRange))
        Marshal.StructureToPtr(fmtRange, lparam, False)
        res = SendMessage(Handle, EM_FORMATRANGE, wparam, lparam) ' Print Rendered Data
        Marshal.FreeCoTaskMem(lparam) ' Free block of memory allocated
        e.Graphics.ReleaseHdc(hdc) ' Release device context handle
        Return res.ToInt32() ' Return last + 1 character
    End Function

    Private Sub PrintDocument_BeginPrint(ByVal sender As Object,
                            ByVal e As System.Drawing.Printing.PrintEventArgs) _
                            Handles PrintDocument.BeginPrint
        checkPrint = 0
    End Sub

    Private Sub PrintDocument_PrintPage(ByVal sender As Object,
                                   ByVal e As System.Drawing.Printing.PrintPageEventArgs) _
                                   Handles PrintDocument.PrintPage
        ' Print the content of the RichTextBox. Store the last character printed.
        checkPrint = Me.Print(checkPrint, Me.TextLength, e)
        If checkPrint < Me.TextLength Then ' Look for more pages
            e.HasMorePages = True
        Else
            e.HasMorePages = False
        End If
    End Sub
End Class
