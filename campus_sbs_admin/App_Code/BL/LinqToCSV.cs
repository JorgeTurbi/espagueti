using LINQtoCSV;
using sbs_DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace campus_sbs_admin
{
    public class LinqToCSV
    {
        public LinqToCSV()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }

        public static IEnumerable<CVS> ReadFileWithExceptionHandling<CVS>(String csvFile)
                where CVS : class, new()
        {
            IEnumerable<CVS> items = null;
            try
            {
                CsvContext cc = new CsvContext();

                CsvFileDescription inputFileDescription = new CsvFileDescription
                {
                    SeparatorChar = ';',
                    FirstLineHasColumnNames = true,
                    FileCultureName = "es-ES", // default is the current culture
                    TextEncoding = Encoding.GetEncoding(1252),
                    MaximumNbrExceptions = 50, // limit number of aggregated exceptions to 50
                };

                items = cc.Read<CVS>(csvFile, inputFileDescription);
            }
            catch (AggregatedException ae)
            {
                // Process all exceptions generated while processing the file

                List<Exception> innerExceptionsList =
                    (List<Exception>)ae.Data["InnerExceptionsList"];

                foreach (Exception e in innerExceptionsList)
                {
                    LogUtils.InsertarLog("Error al procesar fichero CSV " + e);
                }
            }
            catch (DuplicateFieldIndexException dfie)
            {
                // name of the class used with the Read method - in this case "Product"
                string typeName = Convert.ToString(dfie.Data["TypeName"]);

                // Names of the two fields or properties that have the same FieldIndex
                string fieldName = Convert.ToString(dfie.Data["FieldName"]);
                string fieldName2 = Convert.ToString(dfie.Data["FieldName2"]);

                // Actual FieldIndex that the two fields have in common
                int commonFieldIndex = Convert.ToInt32(dfie.Data["Index"]);

                // Do some processing with this information
                // .........


                // Inform user of error situation
                LogUtils.InsertarLog("Error al procesar fichero CSV " + dfie);
            }
            catch (Exception e)
            {
                LogUtils.InsertarLog("[Error] Error al procesar fichero CSV " + e);
            }
            return items;
        }

        public static void SaveToFile<CVS>(String csvFile, IEnumerable<CVS> values)
            where CVS : class, new()
        {
            try
            {
                CsvContext cc = new CsvContext();

                CsvFileDescription outputFileDescription = new CsvFileDescription
                {
                    QuoteAllFields = false,
                    SeparatorChar = ';', // tab delimited
                    FirstLineHasColumnNames = true,
                    FileCultureName = "es-ES", // default is the current culture
                    TextEncoding = Encoding.GetEncoding(1252),
                };

                cc.Write<CVS>(values, csvFile, outputFileDescription);
            }
            catch (Exception e)
            {
                LogUtils.InsertarLog("[Error] Error al guardar fichero CSV " + e);
            }
        }

        public static String SaveToString<CVS>(IEnumerable<CVS> values)
            where CVS : class, new()
        {
            CsvContext cc = new CsvContext();
            CsvFileDescription outputFileDescription = new CsvFileDescription
            {
                QuoteAllFields = false,
                SeparatorChar = ';', // tab delimited
                FirstLineHasColumnNames = true,
                FileCultureName = "es-ES", // default is the current culture
                TextEncoding = Encoding.GetEncoding(1252),
            };

            StringWriter stream = new StringWriter();
            cc.Write<CVS>(values, stream, outputFileDescription);
            return stream.ToString();
        }

        public static String GetString<T>(T value) where T : class, new()
        {
            String csvValue = null;
            try
            {
                CsvContext cc = new CsvContext();

                CsvFileDescription inputFileDescription = new CsvFileDescription
                {
                    SeparatorChar = ';',
                    FileCultureName = "es-ES", // default is the current culture
                    FirstLineHasColumnNames = false,
                };

                TextWriter txtWriter = new StringWriter();
                cc.Write<T>(new List<T>() { value }, txtWriter);
                csvValue = txtWriter.ToString();
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - LinqToCSV.cs::GetString<T>(T value)");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }
            return csvValue;
        }
    }
}