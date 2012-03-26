using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Data;


namespace Voodoo.IO
{
    public class CsvReader:StreamReader
    {
        const char m_CellSeparator = ',';
        static string m_LineSeparator = Environment.NewLine;
        private int m_CellLengthMax = 64;
        /// <summary>
        /// 单元格的最大长度，超出抛异常，缺省为64字节
        /// </summary>
        public int CellLengrhMax
        {
            get { return m_CellLengthMax; }
            set { m_CellLengthMax = value; }
        }
        #region Ctors
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamReader"/> class for
        /// the specified stream.
        /// </summary>
        /// <param name="stream">The stream to be read.</param>
        public CsvReader(Stream stream)
            : base(stream)
        {
            this.Initialize();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamReader"/> class for
        /// the specified file name.
        /// </summary>
        /// <param name="path">The complete file path to be read.</param>
        public CsvReader(string path)
            : base(path)
        {
            this.Initialize();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamReader"/> class for
        /// the specified file name, with the specified byte order mark detection option.
        /// </summary>
        /// <param name="path">The complete file path to be read.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look
        /// for byte order marks at the beginning of the file.</param>
        public CsvReader(string path, bool detectEncodingFromByteOrderMarks)
            : base(path, detectEncodingFromByteOrderMarks)
        {
            this.Initialize();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamReader"/> class for
        /// the specified file name, with the specified character encoding. 
        /// </summary>
        /// <param name="path">The complete file path to be read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        public CsvReader(string path, Encoding encoding)
            : base(path, encoding)
        {
            this.Initialize();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamReader"/> class for the
        /// specified stream, with the specified byte order mark detection option. 
        /// </summary>
        /// <param name="stream">The stream to be read.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look
        /// for byte order marks at the beginning of the file.</param>
        public CsvReader(Stream stream, bool detectEncodingFromByteOrderMarks)
            : base(stream, detectEncodingFromByteOrderMarks)
        {
            this.Initialize();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamReader"/> class for the
        /// specified stream, with the specified character encoding.
        /// </summary>
        /// <param name="stream">The stream to be read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        public CsvReader(Stream stream, Encoding encoding)
            : base(stream, encoding)
        {
            this.Initialize();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamReader"/> class for
        /// the specified stream, with the specified character encoding and byte
        /// order mark detection option. 
        /// </summary>
        /// <param name="stream">The stream to be read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look
        /// for byte order marks at the beginning of the file.</param>
        public CsvReader(Stream stream, Encoding encoding,
            bool detectEncodingFromByteOrderMarks)
            : base(stream, encoding, detectEncodingFromByteOrderMarks)
        {
            this.Initialize();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamReader"/> class for
        /// the specified file name, with the specified character encoding and byte
        /// order mark detection option. 
        /// </summary>
        /// <param name="path">The complete file path to be read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look
        /// for byte order marks at the beginning of the file.</param>
       public CsvReader(string path, Encoding encoding,
            bool detectEncodingFromByteOrderMarks)
            : base(path, encoding, detectEncodingFromByteOrderMarks)
        {
            this.Initialize();
        }
 
        private void Initialize()
        {
            if (this.BaseStream.Length > int.MaxValue)
            {
                throw new NotSupportedException(
                    "This stream reader cannot process very big files.");
            }
            m_LineSeparator = Environment.NewLine;
        }
        #endregion
        /// <summary>
        /// 流中的下一数据行；如果到达了流的末尾，则为 string[0]
        /// </summary>
        /// <returns>一行中的所有单元格</returns>
        public string[] ReadRow()
        {
            if (Peek() == -1) return new string[0];
            ArrayList cells = new ArrayList();
            StringBuilder readBuffer = new StringBuilder(16, m_CellLengthMax);
            int c;
            while ((c = Read()) != -1)
            {
                #region 处理特殊带(")的单元格
                if (c == '"' && readBuffer.Length == 0)
                {
                    //StringBuilder quoteBuffer = new StringBuilder(16, m_CellLengthMax);
                    //quoteBuffer.Append((char)c);  //首引号
                    try
                    {
                        while ((c = Read()) != -1)
                        {
                            if (c == 34)                // '"'的值为34;
                            {
                                if (Peek() == 34)       //忽略一个连续双引号
                                {
                                    Read();             //等效于c = Read();
                                }
                                else                    //非连续双引号,为结束引号,
                                {
                                    //quote = quoteBuffer.ToString();
                                    break;              //goto continue
                                }
                            }
                            readBuffer.Append((char)c);//双引号中的内容原样输出;
                        }
                        continue;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        //this.Close();
                        throw new Exception("The cell Length overed maximum value. ");
                    }
                }
                #endregion
                #region //单元格分隔
                if (c == m_CellSeparator)
                {
                    cells.Add(readBuffer.ToString());
                    readBuffer.Remove(0, readBuffer.Length);
                    continue;
                }
                #endregion
                #region //行分隔
                if (c == '\r' || c == '\n')
                {
                    if (c == '\r' && Peek() == '\n')       //忽略'\n'
                    {
                        Read();
                    }
                    cells.Add(readBuffer.ToString());
                    //readBuffer.Remove(0, readBuffer.Length);
                    return (string[])(cells.ToArray(string.Empty.GetType()));
                }
                #endregion
                readBuffer.Append((char)c);
            }
            cells.Add(readBuffer.ToString());
            return (string[])(cells.ToArray(string.Empty.GetType()));
        }

        public DataTable GetTable()
        {

            DataTable dt_result = new DataTable();

            string[] header = ReadRow();
            foreach (string columnName in header)
            {
                dt_result.Columns.Add(columnName, typeof(System.String));
            }

            string[] data = ReadRow();
            while (data.Length > 0)
            {
                dt_result.Rows.Add(data);
                data = ReadRow();
            }

            return dt_result;

        }
    
    }
}
