'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated from a template.
'
'     Manual changes to this file may cause unexpected behavior in your application.
'     Manual changes to this file will be overwritten if the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Collections.Generic

Partial Public Class tblItems
    Public Property ItemID As Integer
    Public Property ItemUUID As Guid
    Public Property ItemCode As String
    Public Property ItemName As String
    Public Property ItemType As String
    Public Property ItemPackage As String
    Public Property Quantity As Decimal
    Public Property ItemPrice As Decimal
    Public Property ItemCareType As String
    Public Property ItemFrequency As Nullable(Of Short)
    Public Property ItemPatCat As Byte
    Public Property ValidityFrom As Date
    Public Property ValidityTo As Nullable(Of Date)
    Public Property LegacyID As Nullable(Of Integer)
    Public Property AuditUserID As Integer
    Public Property RowID As Byte()

    Public Overridable Property tblClaimItems As ICollection(Of tblClaimItems) = New HashSet(Of tblClaimItems)
    Public Overridable Property tblPLItemsDetail As ICollection(Of tblPLItemsDetail) = New HashSet(Of tblPLItemsDetail)
    Public Overridable Property tblProductItems As ICollection(Of tblProductItems) = New HashSet(Of tblProductItems)

End Class
