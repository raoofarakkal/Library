'Imports Library2.Db2
Imports System.Text
Imports System.Xml
Imports Library2.Web.Common

Namespace Library2.Code

    Public Class ObjBuilder2
        Inherits _Library._Base.LibraryBase

#Region " SCRIPT GENERATION "

#Region " COMMON "

        Public Sub New(ByVal pDefenition As DataTable)
            mDefenition = pDefenition
        End Sub

        Private mDefenition As DataTable
        Private ReadOnly Property Defenition() As DataTable
            Get
                Return mDefenition
            End Get
        End Property

        Public Function Generate() As String
            Return cGenerate()
        End Function

        Public Function MultiThreadGenerate() As String
            Return _MultiThreadGenerate()
        End Function

        Private Function GenProperties(ByVal Field As String, ByVal ReturnType As String, ByVal Limit As String, ByVal SkipItem As String) As String
            Dim mStr As New StringBuilder
            If ReturnType.ToLower = "string" Then
                mStr.AppendLine(String.Format("Friend _{0} As {1} = """"", Field, ReturnType))
            Else
                mStr.AppendLine(String.Format("Friend _{0} As {1}", Field, ReturnType))
            End If
            'If Field.Length > 2 Then
            '    If Mid(Field, Field.Length - 1, 2).ToLower = "id" Then
            '        mStr.AppendLine(String.Format("friend Property {0}() As {1}", Field, ReturnType))
            '    Else
            '        mStr.AppendLine(String.Format("Public Property {0}() As {1}", Field, ReturnType))
            '    End If
            'Else
            mStr.AppendLine(String.Format("Public Property {0}() As {1}", Field, ReturnType))
            'End If
            mStr.AppendLine(String.Format("    Get"))
            mStr.AppendLine(String.Format("        Return _{0}", Field))
            mStr.AppendLine(String.Format("    End Get"))
            mStr.AppendLine(String.Format("    {0}Set(ByVal value As {1})", IIf(SkipItem.ToLower = "yes", "Friend ", ""), ReturnType))
            If _Library._Web._Common.MyCint(Limit) > 0 Then
                mStr.AppendLine(String.Format("        _{0} = left(value.tostring.trim,{1})", Field, _Library._Web._Common.MyCint(Limit)))
            Else
                mStr.AppendLine(String.Format("        _{0} = value", Field))
            End If
            mStr.AppendLine(String.Format("    End Set"))
            mStr.AppendLine(String.Format("End Property"))
            Return mStr.ToString
        End Function

        Private Function getDtType(ByVal pVal As String) As String
            Dim mRet As String = pVal
            If InStr(pVal.ToLower, "bool") > 0 Then
                mRet = "Integer" '"boolean"
            ElseIf InStr(pVal.ToLower, "bit") > 0 Then
                mRet = "Integer"
            ElseIf InStr(pVal.ToLower, "decimal") > 0 Then
                mRet = "Double"
            ElseIf InStr(pVal.ToLower, "float") > 0 Then
                mRet = "Double"
            ElseIf InStr(pVal.ToLower, "int") > 0 Then
                mRet = "Integer"
            ElseIf InStr(pVal.ToLower, "num") > 0 Then
                mRet = "Double"
            ElseIf InStr(pVal.ToLower, "char") > 0 Then
                mRet = "String"
            ElseIf InStr(pVal.ToLower, "text") > 0 Then
                mRet = "String"
            ElseIf InStr(pVal.ToLower, "date") > 0 Then
                mRet = "Date"
            ElseIf InStr(pVal.ToLower, "time") > 0 Then
                mRet = "DateTime"
            End If
            Return mRet
        End Function

        Private Function NeedQuote(ByVal pField As String) As Boolean
            Dim mRet As Boolean = False
            If DataType(pField) = _DataType._String Then
                mRet = True
            ElseIf DataType(pField) = _DataType._Date Then
                mRet = True
            End If
            Return mRet
        End Function

        Private Enum _DataType
            _Other = 0
            _String = 1
            _Date = 2
            _Int = 3
            _Double = 4
        End Enum

        Private Function DataType(ByVal pField As String) As _DataType
            Dim mRet As _DataType = _DataType._Other
            If InStr(getDtType(pField).ToLower, "str") > 0 Then
                mRet = _DataType._String
            ElseIf InStr(getDtType(pField).ToLower, "date") > 0 Then
                mRet = _DataType._Date
            ElseIf InStr(getDtType(pField).ToLower, "double") > 0 Then
                mRet = _DataType._Double
            ElseIf InStr(getDtType(pField).ToLower, "int") > 0 Then
                mRet = _DataType._Int
            End If
            Return mRet
        End Function

        Private Function GenPageProp() As String
            Dim mStr As New StringBuilder
            mStr.AppendLine("Private mPageInfo As Library2.Db2.PagingInfo")
            mStr.AppendLine("Public Readonly Property PagingInformation() As Library2.Db2.PagingInfo")
            mStr.AppendLine("    Get")
            mStr.AppendLine("        If mPageInfo Is Nothing Then")
            mStr.AppendLine("            mPageInfo = New Library2.Db2.PagingInfo")
            mStr.AppendLine("        End If")
            mStr.AppendLine("        Return mPageInfo")
            mStr.AppendLine("    End Get")
            mStr.AppendLine("End Property")
            Return mStr.ToString
        End Function

        Private Function GenJsoncollection() As String
            Dim mRet As New Text.StringBuilder
            mRet.AppendLine("Public function Json(Optional NoOuterCurlyBrace As Boolean = False) As String")
            mRet.AppendLine("   Dim mJson As String = """"")
            mRet.AppendLine("   Dim mComma As String = """"")
            mRet.AppendLine("   For Each mObj As " & Defenition.TableName & " In Me")
            mRet.AppendLine("       mJson = mJson & mComma & mObj.Json")
            mRet.AppendLine("       mComma = "",""")
            mRet.AppendLine("   Next")
            mRet.AppendLine("        If NoOuterCurlyBrace Then")
            mRet.AppendLine("           mJson = """"""" & Defenition.TableName & """"":["" & mJson & ""]""")
            mRet.AppendLine("        Else")
            mRet.AppendLine("           mJson = ""{""""" & Defenition.TableName & """"":["" & mJson & ""]}""")
            mRet.AppendLine("        End If")
            mRet.AppendLine("   Return mJson")
            mRet.AppendLine("End function")
            Return mRet.ToString
        End Function

        Private Function GetUniqueID(ByVal pField As _UniqueType, Optional ByVal pIndexForSqlStatementStringDotFormatValue As Integer = 0) As String
            Dim mRet As String = "UniqueID"
            For Each mR As DataRow In Me.Defenition.Rows
                If mR.Item("PK").ToString.ToLower = "yes" Then
                    Select Case pField
                        Case _UniqueType.LowerOfDbFieldName
                            If NeedQuote(mR.Item("data_type").ToString) Then
                                mRet = String.Format(" lower({0}) ", mR.Item(0).ToString)
                            Else
                                mRet = mR.Item(0).ToString
                            End If
                        Case _UniqueType.DbFieldName
                            mRet = mR.Item(0).ToString
                        Case _UniqueType.SqlStatementStringDotFormatValue
                            If NeedQuote(mR.Item("data_type").ToString) Then
                                mRet = " '{" & pIndexForSqlStatementStringDotFormatValue & "}' "
                            Else
                                mRet = " {" & pIndexForSqlStatementStringDotFormatValue & "} "
                            End If
                        Case _UniqueType.PropertyDotToLower
                            If NeedQuote(mR.Item("data_type").ToString) Then
                                mRet = String.Format(" {0}.ToLower ", mR.Item(0).ToString)
                            Else
                                mRet = mR.Item(0).ToString
                            End If
                        Case _UniqueType.Property
                            mRet = mR.Item(0).ToString
                        Case _UniqueType.PropertyWithAsType
                            mRet = mR.Item(0).ToString & " as " & getDtType(mR.Item("data_type").ToString)
                    End Select
                End If
            Next
            Return mRet
        End Function

        Private Enum _UniqueType
            LowerOfDbFieldName = 1
            SqlStatementStringDotFormatValue = 2
            PropertyDotToLower = 3
            [Property] = 4
            DbFieldName = 5
            PropertyWithAsType = 6
        End Enum


#Region " SqlDbType Enumeration "
        'BigInt
        'Binary
        'Bit
        'Char
        'DateTime
        'Decimal
        'Float
        'Image
        'Int
        'Money
        'NChar
        'NText
        'NVarChar
        'Real
        'UniqueIdentifier
        'SmallDateTime
        'SmallInt
        'SmallMoney
        'Text
        'Timestamp
        'TinyInt
        'VarBinary
        'VarChar
        'Variant
        'Xml
        'Udt
        'Structured
        'Date
        'Time
        'DateTime2
        'DateTimeOffset
#End Region
        Private Function GetUniqueSqlDbType() As String
            Dim mRet As String = ""
            For Each mR As DataRow In Me.Defenition.Rows
                If mR.Item("PK").ToString.ToLower = "yes" Then
                    Dim mVal As String = mR.Item("data_type").ToString
                    If InStr(mVal.ToLower, "bool") > 0 Then
                        mRet = "Int"
                    ElseIf InStr(mVal.ToLower, "bit") > 0 Then
                        mRet = "int"
                    ElseIf InStr(mVal.ToLower, "decimal") > 0 Then
                        mRet = "Decimal"
                    ElseIf InStr(mVal.ToLower, "float") > 0 Then
                        mRet = "Float"
                    ElseIf InStr(mVal.ToLower, "int") > 0 Then
                        mRet = "Int"
                    ElseIf InStr(mVal.ToLower, "num") > 0 Then
                        mRet = "Float"
                    ElseIf InStr(mVal.ToLower, "char") > 0 Then
                        mRet = "NVarChar"
                    ElseIf InStr(mVal.ToLower, "text") > 0 Then
                        mRet = "NText"
                    ElseIf InStr(mVal.ToLower, "date") > 0 Then
                        mRet = "DateTime"
                    ElseIf InStr(mVal.ToLower, "time") > 0 Then
                        mRet = "DateTime"
                    End If
                End If
            Next
            Return String.Format("SqlDbType.{0}", mRet)
        End Function

        Private Function GetSqlDbType(ByVal pR As DataRow) As String
            Dim mRet As String = ""
            Dim mVal As String = pR.Item("data_type").ToString
            If InStr(mVal.ToLower, "bool") > 0 Then
                mRet = "Int"
            ElseIf InStr(mVal.ToLower, "bit") > 0 Then
                mRet = "int"
            ElseIf InStr(mVal.ToLower, "decimal") > 0 Then
                mRet = "Decimal"
            ElseIf InStr(mVal.ToLower, "float") > 0 Then
                mRet = "Float"
            ElseIf InStr(mVal.ToLower, "int") > 0 Then
                mRet = "Int"
            ElseIf InStr(mVal.ToLower, "num") > 0 Then
                mRet = "Float"
            ElseIf InStr(mVal.ToLower, "char") > 0 Then
                mRet = "NVarChar"
            ElseIf InStr(mVal.ToLower, "text") > 0 Then
                mRet = "NText"
            ElseIf InStr(mVal.ToLower, "date") > 0 Then
                mRet = "DateTime"
            ElseIf InStr(mVal.ToLower, "time") > 0 Then
                mRet = "DateTime"
            End If
            Return String.Format("SqlDbType.{0}", mRet)
        End Function

        Private Function GetSqlDbTypeLen(ByVal pR As DataRow) As Integer
            Dim mRet As String = 0
            mRet = pR.Item("character_maximum_length").ToString
            Return MyCInt(mRet)
        End Function

#End Region

#Region " CONNECTED MODEL "

        Private Function cGenerate() As String
            Dim mStr As New StringBuilder
            If Me.Defenition IsNot Nothing Then
                If Me.Defenition.Rows.Count > 0 Then
                    mStr.AppendLine(String.Format("Imports System.Collections.Generic"))
                    mStr.AppendLine(String.Format("Imports System.Data"))
                    mStr.AppendLine(String.Format("Imports Library2.Db2"))
                    mStr.AppendLine(String.Format("Imports Library2.Web.Common"))
                    mStr.AppendLine(" ")
                    mStr.AppendLine("#Region "" Code Generated by Object Builder Ver 2.0  "" ")
                    mStr.AppendLine(" ")
                    mStr.AppendLine(String.Format("Partial Public Class c{0}", Defenition.TableName))
                    mStr.AppendLine(String.Format("Inherits List(Of {0})", Defenition.TableName))
                    mStr.AppendLine(" ")
                    mStr.AppendLine(GenPageProp)
                    mStr.AppendLine(" ")
                    mStr.AppendLine(GenJsoncollection)
                    mStr.AppendLine(" ")
                    mStr.AppendLine(String.Format("End Class"))
                    mStr.AppendLine(String.Format(" "))
                    mStr.AppendLine(String.Format("Partial Public Class {0}", Defenition.TableName))
                    mStr.AppendLine(String.Format("Inherits obj{0}", Defenition.TableName))
                    mStr.AppendLine(String.Format(" "))
                    mStr.AppendLine(String.Format("Dim _MyDb As Library2.Db2.MyDB"))
                    mStr.AppendLine(String.Format(" "))
                    mStr.AppendLine(cGenNew())
                    mStr.AppendLine(cGenLoad)
                    mStr.AppendLine(cGenSave)
                    mStr.AppendLine(cGenFind)
                    mStr.AppendLine(cGenDrop)

                    mStr.AppendLine(" ")

                    mStr.AppendLine("    Public Function Json() As String")
                    mStr.AppendLine("        Dim mJson As String = """"")
                    Dim mComma As String = ""
                    For Each mR As DataRow In Defenition.Rows
                        Try
                            Dim mDtType As String = getDtType(mR.Item("data_type").ToString)
                            If mDtType.Contains("String") Or mDtType.Contains("Date") Then
                                mStr.AppendLine("        mJson = mJson & " & mComma & " String.Format(""""""{0}"""":""""{1}"""""", """ & mR.Item("Column_Name").ToString & """, " & mR.Item("Column_Name").ToString & ")")
                            Else
                                mStr.AppendLine("        mJson =  mJson & " & mComma & "String.Format(""""""{0}"""":{1}"", """ & mR.Item("Column_Name").ToString & """, " & mR.Item("Column_Name").ToString & ")")
                            End If
                            mComma = ""","" & "
                        Catch ex As Exception
                            mStr.AppendLine("'''Object Builder Error:" & ex.Message)
                        End Try
                    Next

                    mStr.AppendLine("        'mJson =  mJson & " & mComma & " Me.ChildObject.Json(True)")
                    mStr.AppendLine("        Return ""{"" & mJson & ""}"" ")
                    mStr.AppendLine("    End Function")

                    mStr.AppendLine(" ")

                    mStr.AppendLine(String.Format("End Class"))
                    mStr.AppendLine(String.Format(" "))
                    mStr.AppendLine(String.Format("Partial Public Class obj{0}", Defenition.TableName))
                    mStr.AppendLine(String.Format(" "))
                    For Each mR As DataRow In Defenition.Rows
                        Try
                            mStr.AppendLine(GenProperties(mR.Item("Column_Name").ToString, getDtType(mR.Item("data_type").ToString), mR.Item("character_maximum_length").ToString, mR.Item("SKIP").ToString))
                        Catch ex As Exception
                            mStr.AppendLine("'''Object Builder Error:" & ex.Message)
                        End Try
                    Next
                    mStr.AppendLine(" ")


                    mStr.AppendLine(String.Format("End Class"))
                    mStr.AppendLine(" ")
                    mStr.AppendLine("#End Region ")
                    mStr.AppendLine(" ")
                End If
            End If
            Return mStr.ToString
        End Function

        Private Function cGenNew() As String
            Dim mStr As New StringBuilder
            mStr.AppendLine(String.Format("Public Sub New(ByRef pMyDb As Library2.Db2.MyDB)"))
            mStr.AppendLine(String.Format("_MyDb = pMyDb "))
            mStr.AppendLine(String.Format("End Function"))
            mStr.AppendLine(String.Format(" "))
            mStr.AppendLine(String.Format("Private Sub TableRowToObj(pDataRow as DataRow)"))
            For Each mR As DataRow In Me.Defenition.Rows
                Select Case DataType(mR.Item("data_type").ToString)
                    Case _DataType._Date
                        mStr.AppendLine(String.Format(" MyBase._{0} = MyCDate(pDataRow.Item(""{0}"").ToString)", mR.Item("Column_Name").ToString))
                    Case _DataType._Double
                        mStr.AppendLine(String.Format(" MyBase._{0} = MyCDbl(pDataRow.Item(""{0}"").ToString)", mR.Item("Column_Name").ToString))
                    Case _DataType._Int
                        mStr.AppendLine(String.Format(" MyBase._{0} = MyClng(pDataRow.Item(""{0}"").ToString)", mR.Item("Column_Name").ToString))
                    Case Else
                        mStr.AppendLine(String.Format(" MyBase._{0} = pDataRow.Item(""{0}"").ToString", mR.Item("Column_Name").ToString))
                End Select
            Next
            mStr.AppendLine(String.Format("End Function"))
            Return mStr.ToString
        End Function

        Private Function cGenLoad() As String
            Dim mStr As New StringBuilder
            mStr.AppendLine(String.Format("Friend Sub Load(p" & GetUniqueID(_UniqueType.PropertyWithAsType) & ") "))
            mStr.AppendLine(String.Format(" Dim mDt As New Data.DataTable"))
            mStr.AppendLine(String.Format(" Dim mSQL As String = """""))
            mStr.AppendLine(String.Format(" Try"))
            'mStr.AppendLine("     mSQL = String.Format(""SELECT * FROM " & Defenition.TableName & " WHERE " & GetUniqueID(_UniqueType.LowerOfDbFieldName) & " = " & GetUniqueID(_UniqueType.SqlStatementStringDotFormatValue, 0) & " "",p" & GetUniqueID(_UniqueType.Property) & ")")
            mStr.AppendLine("     mSQL = ""SELECT * FROM " & Defenition.TableName & " WHERE " & GetUniqueID(_UniqueType.LowerOfDbFieldName) & " = @" & GetUniqueID(_UniqueType.LowerOfDbFieldName) & " "" ")

            mStr.AppendLine("     Dim mSqlParas As New List(Of System.Data.SqlClient.SqlParameter)")
            mStr.AppendLine("     Dim mSqlpara As New System.Data.SqlClient.SqlParameter(""@" & GetUniqueID(_UniqueType.LowerOfDbFieldName) & """, " & GetUniqueSqlDbType() & ")")
            mStr.AppendLine("     mSqlpara.Value = p" & GetUniqueID(_UniqueType.Property))
            mStr.AppendLine("     mSqlParas.Add(mSqlpara)")


            mStr.AppendLine(String.Format("     mDt = _MyDb.GetTable(mSQL,mSqlParas)"))
            mStr.AppendLine(String.Format("     If mDt.Rows.Count > 0 Then"))
            mStr.AppendLine(String.Format("         try"))
            mStr.AppendLine(String.Format("             TableRowToObj(mDt.Rows(0))"))
            mStr.AppendLine(String.Format("         Catch ex As Exception"))
            mStr.AppendLine(String.Format("             Throw New Exception(""Error on converting data table to object""& ex.message)"))
            mStr.AppendLine(String.Format("         End Try"))
            mStr.AppendLine(String.Format("     Else"))
            mStr.AppendLine(String.Format("         Throw New Exception(""Load() Failed. Couldn't find specified ID"")"))
            mStr.AppendLine(String.Format("     End If"))
            mStr.AppendLine(String.Format(" Catch ex As Exception"))
            mStr.AppendLine(String.Format("     Throw ex"))
            mStr.AppendLine(String.Format(" Finally"))
            mStr.AppendLine(String.Format("     mDt = Nothing"))
            mStr.AppendLine(String.Format(" End Try"))
            mStr.AppendLine(String.Format("End Sub"))
            Return mStr.ToString
        End Function

        Private Function cGenFind() As String
            Dim mStr As New StringBuilder
            mStr.AppendLine(String.Format("Friend Shared Function Find(pSqlParameters As List(Of System.Data.SqlClient.SqlParameter),ByRef PageInfo as c{0} , ByRef pMyDb As Library2.Db2.MyDB) as c{0}", Defenition.TableName))
            mStr.AppendLine("'''TODO: change """" to WHERE <condition> by using Sql Parameters. for example : WHERE ID = @ID")
            mStr.AppendLine(String.Format(" Return _Find("""",pSqlParameters,PageInfo,pMyDb)", Defenition.TableName))
            mStr.AppendLine(String.Format("End Function"))
            mStr.AppendLine(String.Format(" "))
            mStr.AppendLine(String.Format("Private Shared Function _Find(pWhereCondition As String,pSqlParameters As List(Of System.Data.SqlClient.SqlParameter),ByRef PageInfo as c{0} , ByRef pMyDb As Library2.Db2.MyDB) as c{0}", Defenition.TableName))
            mStr.AppendLine(String.Format(" Dim mRet As New c{0}", Defenition.TableName))
            mStr.AppendLine(String.Format(" Dim mSingle As {0} = Nothing", Defenition.TableName))
            mStr.AppendLine(String.Format(" Dim mDt As New Data.DataTable"))
            mStr.AppendLine(String.Format(" Dim mSQL As String = """""))
            mStr.AppendLine(String.Format(" Dim mCNT As String = """""))
            mStr.AppendLine(String.Format(" Dim mTot As Long"))
            mStr.AppendLine(String.Format(" Dim mBeginByMe As Boolean = False"))
            mStr.AppendLine(String.Format(" Try"))
            mStr.AppendLine("     mBeginByMe = pMyDb.Begin ")
            mStr.AppendLine("     mCNT = ""SELECT count(" & GetUniqueID(_UniqueType.DbFieldName) & ") FROM " & Defenition.TableName & " "" & pWhereCondition ")
            mStr.AppendLine(String.Format("     mDt = New Data.DataTable"))
            mStr.AppendLine(String.Format("     mDt = pMyDb.GetTable(mCNT,pSqlParameters)"))
            mStr.AppendLine(String.Format("     If mDt.Rows.Count > 0 Then"))
            mStr.AppendLine(String.Format("         mTot = mDt.Rows(0).Item(0).ToString"))
            mStr.AppendLine(String.Format("     End If"))
            mStr.AppendLine(String.Format("     If mTot > 0 Then"))
            mStr.AppendLine(String.Format("         PageInfo.PagingInformation.TotalPageAvailable = Math.Ceiling(mTot / PageInfo.PagingInformation.RecordsPerPage)"))
            mStr.AppendLine(String.Format("         If PageInfo.PagingInformation.TotalPageAvailable < PageInfo.PagingInformation.CurrentPage Then"))
            mStr.AppendLine(String.Format("             PageInfo.PagingInformation.CurrentPage = PageInfo.PagingInformation.TotalPageAvailable"))
            mStr.AppendLine(String.Format("         End If"))

            'mStr.AppendLine("         mSQL = ""SELECT " & GetUniqueID(_UniqueType.DbFieldName) & " FROM " & Defenition.TableName & " "" & pWhereCondition & "" ORDER BY " & GetUniqueID(_UniqueType.DbFieldName) & " """)

            mStr.AppendLine("         mSQL = ""SELECT * FROM " & Defenition.TableName & " "" & pWhereCondition & "" ORDER BY " & GetUniqueID(_UniqueType.DbFieldName) & " """)
            mStr.AppendLine(String.Format("         mDt = New Data.DataTable"))
            mStr.AppendLine(String.Format("         mDt = pMyDb.GetTable(mSQL, PageInfo.PagingInformation.CurrentPage, PageInfo.PagingInformation.RecordsPerPage,pSqlParameters)", Defenition.TableName))
            mStr.AppendLine(String.Format("         If mDt.Rows.Count > 0 Then"))
            mStr.AppendLine(String.Format("             mRet = New c{0}", Defenition.TableName))
            mStr.AppendLine(String.Format("             mRet.PagingInformation.TotalRecordsAvailable = mTot"))
            mStr.AppendLine(String.Format("             mRet.PagingInformation.CurrentPage = PageInfo.PagingInformation.CurrentPage", Defenition.TableName))
            mStr.AppendLine(String.Format("             mRet.PagingInformation.RecordsPerPage = PageInfo.PagingInformation.RecordsPerPage", Defenition.TableName))
            mStr.AppendLine(String.Format("             If mTot > PageInfo.PagingInformation.RecordsPerPage Then", Defenition.TableName))
            mStr.AppendLine(String.Format("                 mRet.PagingInformation.TotalPageAvailable = Math.Ceiling(mTot / PageInfo.PagingInformation.RecordsPerPage)", Defenition.TableName))
            mStr.AppendLine(String.Format("             else"))
            mStr.AppendLine(String.Format("                 mRet.PagingInformation.TotalPageAvailable = 1"))
            mStr.AppendLine(String.Format("             end if"))
            mStr.AppendLine(String.Format("             for each mR as datarow in mDt.Rows"))

            'mStr.AppendLine(String.Format("                 mSingle = Load(mR.Item(0).ToString,pMyDb)"))
            'mStr.AppendLine(String.Format("                 If mSingle IsNot Nothing Then"))
            'mStr.AppendLine(String.Format("                     mRet.Add(mSingle)"))
            'mStr.AppendLine(String.Format("                 End If"))

            mStr.AppendLine(String.Format("                 mSingle = new {0}(pMyDb) ", Defenition.TableName))
            mStr.AppendLine(String.Format("                 mSingle.TableRowToObj(mR)"))
            mStr.AppendLine(String.Format("                 mRet.Add(mSingle)"))
            mStr.AppendLine(String.Format("             next"))
            mStr.AppendLine(String.Format("         endif"))
            mStr.AppendLine(String.Format("     endif"))
            mStr.AppendLine(String.Format("     If mBeginByMe Then"))
            mStr.AppendLine(String.Format("         pMyDb.Commit()"))
            mStr.AppendLine(String.Format("     End If"))
            mStr.AppendLine(String.Format(" Catch ex As Exception"))
            mStr.AppendLine(String.Format("     If mBeginByMe Then"))
            mStr.AppendLine(String.Format("         pMyDb.RollBack()"))
            mStr.AppendLine(String.Format("     End If"))
            mStr.AppendLine(String.Format("     Throw ex"))
            mStr.AppendLine(String.Format(" Finally"))
            mStr.AppendLine(String.Format("     mDt = Nothing"))
            mStr.AppendLine(String.Format(" End Try"))
            mStr.AppendLine(String.Format(" Return mRet"))
            mStr.AppendLine(String.Format("End Function"))
            Return mStr.ToString
        End Function

        Private Function cGenSave() As String
            Dim mStr As New StringBuilder
            mStr.AppendLine(String.Format("Friend Sub Save() "))
            mStr.AppendLine(String.Format(" Dim mSQL As String = """""))
            mStr.AppendLine(String.Format(" Try"))
            mStr.AppendLine(String.Format("     If IsExist(me.{0}) Then 'Edit", GetUniqueID(_UniqueType.Property)))
            mStr.AppendLine(cGenUPDsql)
            mStr.AppendLine(String.Format("     Else 'Add"))
            mStr.AppendLine(cGenINSsql)
            mStr.AppendLine(String.Format("     End If"))
            mStr.AppendLine(String.Format(" Catch ex As Exception"))
            mStr.AppendLine(String.Format("     Throw ex"))
            mStr.AppendLine(String.Format(" End Try"))
            mStr.AppendLine(String.Format("End Sub"))
            mStr.AppendLine(" ")
            mStr.AppendLine(cGenIsExist())
            Return mStr.ToString
        End Function

        Private Function cGenIsExist() As String
            Dim mStr As New StringBuilder
            mStr.AppendLine(String.Format("Private Function IsExist(ByVal p{0}) As Boolean", GetUniqueID(_UniqueType.PropertyWithAsType)))
            mStr.AppendLine(String.Format(" Dim mRet As Boolean = False"))
            mStr.AppendLine(String.Format(" Dim mDt As New Data.DataTable"))
            mStr.AppendLine(String.Format(" Dim mSQL As String = """))
            mStr.AppendLine(String.Format(" Try"))
            'mStr.AppendLine("mSQL = String.Format(""SELECT " & GetUniqueID(_UniqueType.DbFieldName) & " FROM " & Defenition.TableName & " WHERE " & GetUniqueID(_UniqueType.LowerOfDbFieldName) & " = " & GetUniqueID(_UniqueType.SqlStatementStringDotFormatValue, 0) & " "", p" & GetUniqueID(_UniqueType.PropertyDotToLower).Trim & ")")
            mStr.AppendLine("mSQL = ""SELECT " & GetUniqueID(_UniqueType.DbFieldName) & " FROM " & Defenition.TableName & " WHERE " & GetUniqueID(_UniqueType.LowerOfDbFieldName) & " = @" & GetUniqueID(_UniqueType.LowerOfDbFieldName) & " "" ")

            mStr.AppendLine("     Dim mSqlParas As New List(Of System.Data.SqlClient.SqlParameter)")
            mStr.AppendLine("     Dim mSqlpara As New System.Data.SqlClient.SqlParameter(""@" & GetUniqueID(_UniqueType.LowerOfDbFieldName) & """, " & GetUniqueSqlDbType() & ")")
            mStr.AppendLine("     mSqlpara.Value = p" & GetUniqueID(_UniqueType.Property))
            mStr.AppendLine("     mSqlParas.Add(mSqlpara)")

            mStr.AppendLine(String.Format("     mDt = _MyDb.GetTable(mSQL,mSqlParas)"))
            mStr.AppendLine(String.Format("     If mDt.Rows.Count > 0 Then"))
            mStr.AppendLine(String.Format("         mRet = True"))
            mStr.AppendLine(String.Format("     End If"))
            mStr.AppendLine(String.Format(" Catch ex As Exception"))
            mStr.AppendLine(String.Format("     Throw ex"))
            mStr.AppendLine(String.Format(" Finally"))
            mStr.AppendLine(String.Format("     mDt = Nothing"))
            mStr.AppendLine(String.Format(" End Try"))
            mStr.AppendLine(String.Format(" Return mRet"))
            mStr.AppendLine(String.Format("End Function"))
            Return mStr.ToString
        End Function

        Private Function cGenUPDsql() As String
            Dim mRet As New StringBuilder
            Dim mComma As String = ""
            Dim mF As String = ""
            Dim mV As String = ""
            Dim mV2 As String = ""

            mRet.AppendLine("mSQL = "" UPDATE " & Defenition.TableName & " SET ")
            For Each mR As DataRow In Me.Defenition.Rows
                If mR.Item("SKIP").ToString.ToLower <> "yes" Then
                    mF = mR.Item(0).ToString & " = "
                    mV = "@" & mR.Item(0).ToString
                    'Select Case DataType(mR.Item("data_type").ToString)
                    '    Case _DataType._Date
                    '        'mV = "{0}"
                    '        mV2 = " UseNullIfEmptyDate(me._" & mR.Item(0).ToString & ") "
                    '    Case _DataType._String
                    '        'mV = "'{0}'"
                    '        mV2 = "me._" & mR.Item(0).ToString
                    '    Case Else
                    '        'mV = "{0}"
                    '        mV2 = "UseNullIfZero(me._" & mR.Item(0).ToString & ")"
                    'End Select


                    'mRet.AppendLine("mSQL += string.format("" " & mComma & mF & mV & " ""," & mV2 & ")")
                    mRet.AppendLine("mSQL += string.format("" " & mComma & mF & mV & " "")")
                    mComma = ","


                End If
            Next
            'mRet.AppendLine("mSQL += string.format("" WHERE " & GetUniqueID(_UniqueType.LowerOfDbFieldName) & " = " & GetUniqueID(_UniqueType.SqlStatementStringDotFormatValue, 0) & " "", me._" & GetUniqueID(_UniqueType.PropertyDotToLower) & " )")
            mRet.AppendLine("mSQL += string.format("" WHERE " & GetUniqueID(_UniqueType.LowerOfDbFieldName) & " = @" & GetUniqueID(_UniqueType.LowerOfDbFieldName) & """ )")
            mRet.AppendLine("")
            mRet.AppendLine("     Dim mSqlParas As New List(Of System.Data.SqlClient.SqlParameter)")
            mRet.AppendLine("     Dim mSqlpara As System.Data.SqlClient.SqlParameter")
            mRet.AppendLine("")
            mRet.AppendLine("     mSqlpara = New System.Data.SqlClient.SqlParameter(""@" & GetUniqueID(_UniqueType.LowerOfDbFieldName) & """, " & GetUniqueSqlDbType() & ")")
            mRet.AppendLine("     mSqlpara.Value = me." & GetUniqueID(_UniqueType.Property))
            mRet.AppendLine("     mSqlParas.Add(mSqlpara)")
            mRet.AppendLine("")
            For Each mR As DataRow In Me.Defenition.Rows
                If mR.Item("PK").ToString.ToLower <> "yes" Then
                    If mR.Item("SKIP").ToString.ToLower <> "yes" Then
                        If GetSqlDbTypeLen(mR) > 0 Then
                            mRet.AppendLine("     mSqlpara = New System.Data.SqlClient.SqlParameter(""@" & mR.Item(0).ToString & """, " & GetSqlDbType(mR) & "," & GetSqlDbTypeLen(mR) & ")")
                        Else
                            mRet.AppendLine("     mSqlpara = New System.Data.SqlClient.SqlParameter(""@" & mR.Item(0).ToString & """, " & GetSqlDbType(mR) & ")")
                        End If
                        Select Case DataType(mR.Item("data_type").ToString)
                            Case _DataType._Date
                                mRet.AppendLine("     mSqlpara.Value = me._" & mR.Item(0).ToString & " ")
                            Case _DataType._String
                                mRet.AppendLine("     mSqlpara.Value = me._" & mR.Item(0).ToString)
                            Case _DataType._Int
                                mRet.AppendLine("     mSqlpara.Value = Convert.ToInt32(me._" & mR.Item(0).ToString & ") ")
                            Case _DataType._Double
                                'mRet.AppendLine("     mSqlpara.Value = Convert.ToDouble(me._" & mR.Item(0).ToString & ") ")
                                mRet.AppendLine("     mSqlpara.Value = me._" & mR.Item(0).ToString & " ")
                            Case Else
                                'mRet.AppendLine("     mSqlpara.Value = UseNullIfZero(me._" & mR.Item(0).ToString & ") ")
                                mRet.AppendLine("     mSqlpara.Value = me._" & mR.Item(0).ToString & " ")
                        End Select
                        mRet.AppendLine("     mSqlParas.Add(mSqlpara)")
                        mRet.AppendLine("")
                    End If
                End If
            Next
            mRet.AppendLine("")
            mRet.AppendLine(String.Format("_MyDb.ExecuteNonQuery(mSQL,New ExecuteNonQueryPara(True,False, False),mSqlParas)"))
            Return mRet.ToString

        End Function

        Private Function cGenINSsql() As String
            Dim mRet As New StringBuilder
            Dim mComma As String = ""
            Dim mF As String = ""
            Dim mV As String = ""
            Dim mV2 As String = ""
            Dim i As Integer = 0
            Dim mIdentFld As String = ""
            For Each mR As DataRow In Me.Defenition.Rows
                If mR.Item("SKIP").ToString.ToLower <> "yes" Then
                    mF += mComma & mR.Item(0).ToString
                    mV += mComma & "@" & mR.Item(0).ToString

                    'Select Case DataType(mR.Item("data_type").ToString)
                    '    Case _DataType._Date
                    '        'mV += mComma & "{" & i & "}"
                    '        mV2 += mComma & " UseNullIfEmptyDate(me._" & mR.Item(0).ToString & ") "
                    '    Case _DataType._String
                    '        'mV += mComma & "'{" & i & "}'"
                    '        mV2 += mComma & " me._" & mR.Item(0).ToString
                    '    Case Else
                    '        ''mV += mComma & IIf(NeedQuote(mR.Item("data_type").ToString), "'{" & i & "}'", "{" & i & "}")
                    '        ''mV2 += mComma & "me." & mR.Item(0).ToString
                    '        'mV += mComma & "{" & i & "}"
                    '        mV2 += mComma & "UseNullIfZero(me._" & mR.Item(0).ToString & ")"
                    'End Select
                    mComma = ","
                    'i += 1
                Else
                    mIdentFld = mR.Item(0).ToString.ToLower
                End If
            Next
            mRet.AppendLine("mSQL = ""INSERT INTO " & Defenition.TableName & "(" & mF & ") "" ")
            mRet.AppendLine("mSQL += "" VALUES(" & mV & ")""")
            mRet.AppendLine("")
            mRet.AppendLine("     Dim mSqlParas As New List(Of System.Data.SqlClient.SqlParameter)")
            mRet.AppendLine("     Dim mSqlpara As System.Data.SqlClient.SqlParameter")
            For Each mR As DataRow In Me.Defenition.Rows
                If mR.Item("PK").ToString.ToLower <> "yes" Then
                    If mR.Item("SKIP").ToString.ToLower <> "yes" Then
                        If GetSqlDbTypeLen(mR) > 0 Then
                            mRet.AppendLine("     mSqlpara = New System.Data.SqlClient.SqlParameter(""@" & mR.Item(0).ToString & """, " & GetSqlDbType(mR) & "," & GetSqlDbTypeLen(mR) & ")")
                        Else
                            mRet.AppendLine("     mSqlpara = New System.Data.SqlClient.SqlParameter(""@" & mR.Item(0).ToString & """, " & GetSqlDbType(mR) & ")")
                        End If
                        Select Case DataType(mR.Item("data_type").ToString)
                            Case _DataType._Date
                                mRet.AppendLine("     mSqlpara.Value = me._" & mR.Item(0).ToString & " ")
                            Case _DataType._String
                                mRet.AppendLine("     mSqlpara.Value = me._" & mR.Item(0).ToString)
                            Case _DataType._Int
                                mRet.AppendLine("     mSqlpara.Value = Convert.ToInt32(me._" & mR.Item(0).ToString & ") ")
                            Case _DataType._Double
                                'mRet.AppendLine("     mSqlpara.Value = Convert.ToDouble(me._" & mR.Item(0).ToString & ") ")
                                mRet.AppendLine("     mSqlpara.Value = me._" & mR.Item(0).ToString & " ")
                            Case Else
                                mRet.AppendLine("     mSqlpara.Value = me._" & mR.Item(0).ToString & " ")
                        End Select
                        mRet.AppendLine("     mSqlParas.Add(mSqlpara)")
                        mRet.AppendLine("")
                    End If
                End If
            Next
            mRet.AppendLine("")
            If mIdentFld.Length > 0 Then
                mRet.AppendLine(String.Format("me._{0} = _MyDb.ExecuteNonQuery(mSQL,New ExecuteNonQueryPara(True, True, False),mSqlParas)", mIdentFld))
            Else
                mRet.AppendLine(String.Format("_MyDb.ExecuteNonQuery(mSQL,New ExecuteNonQueryPara(True,False, False),mSqlParas)"))
            End If
            Return mRet.ToString
        End Function

        Private Function cGenDrop() As String
            Dim mStr As New StringBuilder
            mStr.AppendLine(String.Format("Friend Shared Sub Drop(ByRef p{0} As {0}, ByRef pMyDb As Library2.Db2.MyDB)", Defenition.TableName))
            mStr.AppendLine(String.Format(" Dim mSQL As String = """" "))
            mStr.AppendLine(String.Format(" Try"))
            'mStr.AppendLine("     mSQL = String.Format(""DELETE  FROM " & Defenition.TableName & " WHERE " & GetUniqueID(_UniqueType.LowerOfDbFieldName) & " = " & GetUniqueID(_UniqueType.SqlStatementStringDotFormatValue, 0) & " "",p" & Defenition.TableName & "._" & GetUniqueID(_UniqueType.PropertyDotToLower) & ")")
            mStr.AppendLine("     mSQL = String.Format(""DELETE  FROM " & Defenition.TableName & " WHERE " & GetUniqueID(_UniqueType.LowerOfDbFieldName) & " = @" & GetUniqueID(_UniqueType.LowerOfDbFieldName) & " "") ")
            mStr.AppendLine("     Dim mSqlParas As New List(Of System.Data.SqlClient.SqlParameter)")
            mStr.AppendLine("     Dim mSqlpara As New System.Data.SqlClient.SqlParameter(""@" & GetUniqueID(_UniqueType.LowerOfDbFieldName) & """, " & GetUniqueSqlDbType() & ")")
            mStr.AppendLine("     mSqlpara.Value = p" & Defenition.TableName & "._" & GetUniqueID(_UniqueType.PropertyDotToLower))
            mStr.AppendLine("     mSqlParas.Add(mSqlpara)")
            mStr.AppendLine(String.Format("        pMyDb.ExecuteNonQuery(mSQL,mSqlParas)"))
            mStr.AppendLine(String.Format("        p{0} = Nothing", Defenition.TableName))
            mStr.AppendLine(String.Format("    Catch ex As Exception"))
            mStr.AppendLine(String.Format("        Throw ex"))
            mStr.AppendLine(String.Format("    End Try"))
            mStr.AppendLine(String.Format("End Sub"))
            Return mStr.ToString
        End Function

#End Region

#Region " MULTI-THREAD SAMPLE "
        Private Function _MultiThreadGenerate() As String
            Dim mStr As New StringBuilder

            mStr.AppendLine("Imports System.Threading")
            mStr.AppendLine(" ")

            mStr.AppendLine("Class MyThread")

            mStr.AppendLine("    Public Event onStillExecuting(ByVal sender As Object, ByVal e As EventArgs)")

            mStr.AppendLine("    Dim mThrds As List(Of Thread)")
            mStr.AppendLine(" ")
            mStr.AppendLine("    Private mMaxThreads As Integer = 1")
            mStr.AppendLine("    Public Property MaxThreads() As Integer")
            mStr.AppendLine("        Get")
            mStr.AppendLine("            Return mMaxThreads")
            mStr.AppendLine("        End Get")
            mStr.AppendLine("        Set(ByVal value As Integer)")
            mStr.AppendLine("            mMaxThreads = value")
            mStr.AppendLine("        End Set")
            mStr.AppendLine("    End Property")
            mStr.AppendLine(" ")

            mStr.AppendLine("    Private mPriority As System.Threading.ThreadPriority")
            mStr.AppendLine("    Public Property ThreadPriority() As System.Threading.ThreadPriority")
            mStr.AppendLine("        Get")
            mStr.AppendLine("            Return mPriority")
            mStr.AppendLine("        End Get")
            mStr.AppendLine("        Set(ByVal value As System.Threading.ThreadPriority)")
            mStr.AppendLine("            mPriority = value")
            mStr.AppendLine("        End Set")
            mStr.AppendLine("    End Property")
            mStr.AppendLine(" ")


            mStr.AppendLine("    Public Function CanStartNewThread() As Boolean")
            mStr.AppendLine("        If mThrds IsNot Nothing Then")
            mStr.AppendLine("            Return (mThrds.Count < mMaxThreads)")
            mStr.AppendLine("        Else")
            mStr.AppendLine("            Return True")
            mStr.AppendLine("        End If")
            mStr.AppendLine("    End Function")


            mStr.AppendLine("    Public Function StartNewThread(Optional ByVal DelayinMilliSeconds As Long = 100) As Integer")
            mStr.AppendLine("        If mThrds Is Nothing Then")
            mStr.AppendLine("            mThrds = New List(Of Thread)")
            mStr.AppendLine("        End If")
            mStr.AppendLine("        If mThrds.Count < mMaxThreads Then")
            mStr.AppendLine("            mThrds.Add(New Thread(AddressOf Me.Run))")
            mStr.AppendLine("            mThrds(mThrds.Count - 1).Priority = Me.ThreadPriority")
            mStr.AppendLine("            Try")
            mStr.AppendLine("                mThrds(mThrds.Count - 1).Start()")
            mStr.AppendLine("                Dim mTime As Date = Now")
            mStr.AppendLine(" ")
            mStr.AppendLine("                'DELAY")
            mStr.AppendLine("                Do While True")
            mStr.AppendLine("                    Application.DoEvents()")
            mStr.AppendLine("                    If Now > mTime.AddMilliseconds(DelayinMilliSeconds) Then")
            mStr.AppendLine("                        Exit Do")
            mStr.AppendLine("                    End If")
            mStr.AppendLine("                Loop")
            mStr.AppendLine(" ")
            mStr.AppendLine("            Catch ex As Exception")
            mStr.AppendLine("                Cancel(mThrds.Count)")
            mStr.AppendLine("                Return Nothing")
            mStr.AppendLine("               'Throw New Exception(String.Format(""StartNewThread(). {0}"", ex.Message))")
            mStr.AppendLine("            End Try")
            mStr.AppendLine("            Application.DoEvents()")
            mStr.AppendLine("            Return mThrds.Count")
            mStr.AppendLine("        Else")
            mStr.AppendLine("            'Throw New Exception(""Maximum threads reached"")")
            mStr.AppendLine("            Return Nothing")
            mStr.AppendLine("        End If")
            mStr.AppendLine("    End Function")
            mStr.AppendLine(" ")
            mStr.AppendLine("    Public Function ActiveThreadCount() As Integer")
            mStr.AppendLine("        Dim mRet As Integer = 0")
            mStr.AppendLine("        Try")
            mStr.AppendLine("            If mThrds IsNot Nothing Then")
            mStr.AppendLine("                For i As Integer = 1 To mThrds.Count")
            mStr.AppendLine("                    If mThrds(i - 1) IsNot Nothing Then")
            mStr.AppendLine("                        If mThrds(i - 1).IsAlive Then")
            mStr.AppendLine("                            mRet += 1")
            mStr.AppendLine("                        End If")
            mStr.AppendLine("                    End If")
            mStr.AppendLine("                Next")
            mStr.AppendLine("            End If")
            mStr.AppendLine("        Catch ex As Exception")
            mStr.AppendLine("        End Try")
            mStr.AppendLine("        Return mRet")
            mStr.AppendLine("    End Function")
            mStr.AppendLine(" ")
            mStr.AppendLine("    Public Function Count() As Integer")
            mStr.AppendLine("        If mThrds IsNot Nothing Then")
            mStr.AppendLine("            Return mThrds.Count")
            mStr.AppendLine("        Else")
            mStr.AppendLine("            Return 0")
            mStr.AppendLine("        End If")
            mStr.AppendLine("    End Function")
            mStr.AppendLine(" ")

            mStr.AppendLine("    Public Function Cancel(ByVal index As Integer) As Boolean")
            mStr.AppendLine("        If index <= Count() Then")
            mStr.AppendLine("            If mThrds(index - 1) IsNot Nothing Then")
            mStr.AppendLine("                mThrds(index - 1).Abort()")
            mStr.AppendLine("                mThrds(index - 1) = Nothing")
            mStr.AppendLine("            End If")
            mStr.AppendLine("        End If")
            mStr.AppendLine("    End Function")
            mStr.AppendLine(" ")

            mStr.AppendLine("    Public Function Cancel() As Boolean")
            mStr.AppendLine("        If mThrds IsNot Nothing Then")
            mStr.AppendLine("            For i As Integer = 1 To mThrds.Count")
            mStr.AppendLine("                Cancel(i)")
            mStr.AppendLine("            Next")
            mStr.AppendLine("            mThrds.Clear()")
            mStr.AppendLine("        End If")
            mStr.AppendLine("    End Function")
            mStr.AppendLine(" ")

            mStr.AppendLine("    Public Sub clearUnwantedThreads()")
            mStr.AppendLine("        For i As Integer = 1 To mThrds.Count")
            mStr.AppendLine("            If mThrds(i - 1) IsNot Nothing Then")
            mStr.AppendLine("                If Not mThrds(i - 1).IsAlive Then")
            mStr.AppendLine("                    Cancel(i)")
            mStr.AppendLine("                End If")
            mStr.AppendLine("            End If")
            mStr.AppendLine("        Next")
            mStr.AppendLine("        For i As Integer = mThrds.Count To 1 Step -1")
            mStr.AppendLine("            If mThrds(i - 1) Is Nothing Then")
            mStr.AppendLine("                mThrds.RemoveAt(i - 1)")
            mStr.AppendLine("            End If")
            mStr.AppendLine("        Next")
            mStr.AppendLine(" ")
            mStr.AppendLine("    End Sub")
            mStr.AppendLine(" ")

            mStr.AppendLine("    Private Sub Run()")
            mStr.AppendLine(" ")
            mStr.AppendLine("       '''TODO")
            mStr.AppendLine("       ''' DO YOUR STUFF HERE")
            mStr.AppendLine("       ''' DONT FORGET TO CALL RaiseEvent onStillExecuting(Me, Nothing) in your loop")
            mStr.AppendLine("       ''' ")
            mStr.AppendLine("       'Dim mTm As Date = Now")
            mStr.AppendLine("       'Do While True")
            mStr.AppendLine("       '    RaiseEvent onStillExecuting(Me, Nothing)")
            mStr.AppendLine("       '    If mTm.AddSeconds(5) <= Now Then")
            mStr.AppendLine("       '        Exit Do")
            mStr.AppendLine("       '    End If")
            mStr.AppendLine("       'Loop")
            mStr.AppendLine(" ")
            mStr.AppendLine("        clearUnwantedThreads()")
            mStr.AppendLine(" ")
            mStr.AppendLine("    End Sub")
            mStr.AppendLine(" ")

            mStr.AppendLine("   Public Sub dispose()")
            mStr.AppendLine("       Cancel()")
            mStr.AppendLine("   End Function")
            mStr.AppendLine(" ")

            mStr.AppendLine("End Class")

            Return mStr.ToString

        End Function
#End Region

#End Region

    End Class

End Namespace