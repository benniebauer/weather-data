using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace WeatherSimulation
{
    static class FileIO
    {
        //Generic File Input Output functions
        public static DataTable DelimitedFileToDataTable(string psFileName, char psDelimiter)
        {
            //read a delimted file into a datatable. Returns the Datatable.
            //Assume 1st line of file contains column headings.
            StreamReader sr = new StreamReader(psFileName);
            string _sRow = sr.ReadLine();
            //char _Delimeter = new char[] { psDelimiter };
            var rows = _sRow.Split(psDelimiter);
            DataTable _DT = new DataTable();
            foreach (string column in rows)
            {
                //creates the columns of new datatable based on first row of csv
                _DT.Columns.Add(column);
            }
            _sRow = sr.ReadLine();
            while (_sRow != null)
            {
                //runs until string reader returns null and adds rows to dt 
                rows = _sRow.Split(psDelimiter);
                _DT.Rows.Add(rows);
                _sRow = sr.ReadLine();
            }
            sr.Close();
            sr.Dispose();
            return _DT;
        }

        public static void WriteTextToFile(string psText, string psFilePath)
        {
            System.IO.File.WriteAllText(psFilePath, psText);
        }


    }
}
