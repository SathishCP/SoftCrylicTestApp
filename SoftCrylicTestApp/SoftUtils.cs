﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SoftCrylicTestApp
{
    public class SoftUtils
    {
        public void GetConnect()
        {
            try
            {
                string str = System.Configuration.ConfigurationManager.ConnectionStrings["sqlConn"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(str))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable ConvertListToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public byte[] ExportDataTableIntoMultipleExcelSheets(DataTable dt, string fileName)
        {
            byte[] bytes = new byte[] { };
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    using (System.IO.MemoryStream mStream = new System.IO.MemoryStream()) {
                        using (System.IO.StreamWriter excelDoc = new System.IO.StreamWriter(mStream))
                        {
                            string line = Environment.NewLine;
                            string startExcelXML = "<?xml version='1.0' encoding='utf-16' standalone='yes'?>" + line + "<?mso-application progid=\"Excel.Sheet\"?>" + "<Workbook " + "xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"" + line + " xmlns:o=\"urn:schemas-microsoft-com:office:office\"" + line + " " + "xmlns:x=\"urn:schemas-microsoft-com:office:excel\"" + line + "xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">" + line + "xmlns:msxsl=\"urn:schemas-microsoft-com:xslt\" xmlns:user=\"urn:my-scripts\"" + "xmlns:msxsl=\"urn:schemas-microsoft-com:xslt\" xmlns:user=\"urn:my-scripts\"" + " <Styles>" + line + " " + "<Style ss:ID=\"Default\" ss:Name=\"Normal\">" + line + " " + "<Alignment ss:Vertical=\"Justify\"/>" + line + " <Borders/>" + line + " <Font/>" + line + " <Interior/>" + line + " <NumberFormat/>" + line + " <Protection/>" + line + " </Style>" + line + " " + "<Style ss:ID=\"BoldColumn\">" + line + " <Font " + "x:Family=\"Swiss\" ss:Bold=\"1\"/>" + line + " </Style>" + line + " " + "<Style ss:ID=\"StringLiteral\">" + line + " <NumberFormat" + " ss:Format=\"@\"/>" + line + " </Style>" + line + " <Style " + "ss:ID=\"Decimal\">" + line + " <NumberFormat " + "ss:Format=\"0.0000\"/>" + line + " </Style>" + line + " " + "<Style ss:ID=\"Integer\">" + line + " <NumberFormat " + "ss:Format=\"0\"/>" + line + " </Style>" + line + " <Style " + "ss:ID=\"DateLiteral\">" + line + " <NumberFormat " + "ss:Format=\"dd/mm/yyyy;@\"/>" + line + " </Style>" + line + " " + "</Styles>" + line + " ";
                            const string endExcelXML = "</Workbook>";
                            int rowCount = 0;
                            const int lotSize = short.MaxValue;
                            int sheetCount = 1;
                            excelDoc.Write(startExcelXML);
                            excelDoc.Write("<Worksheet ss:Name=\"Sheet" + sheetCount + "\">");
                            excelDoc.Write("<Table ss:DefaultColumnWidth=\"75\">");
                            excelDoc.Write("<Row>");
                            for (int i = 0; i <= dt.Columns.Count - 1; i++)
                            {
                                excelDoc.Write("<Cell ss:StyleID=\"BoldColumn\"><Data ss:Type=\"String\">");
                                excelDoc.Write(dt.Columns[i].ColumnName);
                                excelDoc.Write("</Data></Cell>");
                            }
                            excelDoc.Write("</Row>");
                            foreach (DataRow dr in dt.Rows)
                            {
                                rowCount += 1;
                                if (rowCount.Equals(lotSize))
                                {
                                    rowCount = 0;
                                    sheetCount += 1;
                                    excelDoc.Write("</Table>");
                                    excelDoc.Write(" </Worksheet>");
                                    excelDoc.Write("<Worksheet ss:Name=\"Sheet" + sheetCount + "\">");
                                    excelDoc.Write("<Table ss:DefaultColumnWidth=\"100\">");
                                    excelDoc.Write("<Row>");
                                    for (int i = 0; i <= dt.Columns.Count - 1; i++)
                                    {
                                        excelDoc.Write("<Cell ss:StyleID=\"BoldColumn\"><Data ss:Type=\"String\">");
                                        excelDoc.Write(dt.Columns[i].ColumnName);
                                        excelDoc.Write("</Data></Cell>");
                                    }
                                    excelDoc.Write("</Row>");
                                }
                                excelDoc.Write("<Row>");
                                for (int i = 0; i <= dt.Columns.Count - 1; i++)
                                {
                                    Type rowType;
                                    rowType = dr[i].GetType();
                                    switch (rowType.ToString())
                                    {
                                        case "System.String":
                                            {
                                                string XMLstring = dr[i].ToString();
                                                XMLstring = XMLstring.Trim();
                                                XMLstring = XMLstring.Replace("&", "&");
                                                XMLstring = XMLstring.Replace(">", ">");
                                                XMLstring = XMLstring.Replace("<", "<");
                                                excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" + "<Data ss:Type=\"String\">");
                                                excelDoc.Write(XMLstring);
                                                excelDoc.Write("</Data></Cell>");
                                                break;
                                            }

                                        case "System.DateTime":
                                            {
                                                System.DateTime XMLDate = System.Convert.ToDateTime(dr[i]);
                                                string XMLDateToString = string.Empty;
                                                XMLDateToString = XMLDate.Year.ToString() + "-" + (XMLDate.Month < 10 ? "0" + XMLDate.Month.ToString() : XMLDate.Month.ToString()) + "-" + (XMLDate.Day < 10 ? "0" + XMLDate.Day.ToString() : XMLDate.Day.ToString()) + "T" + (XMLDate.Hour < 10 ? "0" + XMLDate.Hour.ToString() : XMLDate.Hour.ToString()) + ":" + (XMLDate.Minute < 10 ? "0" + XMLDate.Minute.ToString() : XMLDate.Minute.ToString()) + ":" + (XMLDate.Second < 10 ? "0" + XMLDate.Second.ToString() : XMLDate.Second.ToString()) + ".000";
                                                excelDoc.Write("<Cell ss:StyleID=\"DateLiteral\">" + "<Data ss:Type=\"DateTime\">");
                                                excelDoc.Write(XMLDateToString);
                                                excelDoc.Write("</Data></Cell>");
                                                break;
                                            }

                                        case "System.Boolean":
                                            {
                                                excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" + "<Data ss:Type=\"String\">");
                                                excelDoc.Write(dr[i].ToString());
                                                excelDoc.Write("</Data></Cell>");
                                                break;
                                            }

                                        case "System.Int16":
                                        case "System.Int32":
                                        case "System.Int64":
                                        case "System.Byte":
                                            {
                                                excelDoc.Write("<Cell ss:StyleID=\"Integer\">" + "<Data ss:Type=\"Number\">");
                                                excelDoc.Write(dr[i].ToString());
                                                excelDoc.Write("</Data></Cell>");
                                                break;
                                            }

                                        case "System.Decimal":
                                        case "System.Double":
                                            {
                                                excelDoc.Write("<Cell ss:StyleID=\"Decimal\">" + "<Data ss:Type=\"Number\">");
                                                excelDoc.Write(dr[i].ToString());
                                                excelDoc.Write("</Data></Cell>");
                                                break;
                                            }

                                        case "System.DBNull":
                                            {
                                                excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" + "<Data ss:Type=\"String\">");
                                                excelDoc.Write(string.Empty);
                                                excelDoc.Write("</Data></Cell>");
                                                break;
                                            }

                                        default:
                                            {
                                                throw new Exception(rowType.ToString() + " not handled.");
                                            }
                                    }
                                }
                                excelDoc.Write("</Row>");
                            }
                            excelDoc.Write("</Table>");
                            excelDoc.Write(" </Worksheet>");
                            excelDoc.Write(endExcelXML);
                            excelDoc.Flush(); excelDoc.Close();
                        }
                        mStream.Read(bytes, 0, bytes.Length);
                        bytes = mStream.ToArray();
                    }                    
                }
                return bytes;
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                System.GC.Collect();
            }
        }
    }
}