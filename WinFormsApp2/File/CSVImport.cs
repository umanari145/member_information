using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WinFormsApp2.Factory;
using WinFormsApp2.Request;
using WinFormsApp2.Validation;

namespace WinFormsApp2.File
{
    public class CSVImport
    {
        private FileResult _fileResult;

        private Func<string[], RequestRegist> _mapper;

        private readonly DB _db = new DB();
        private RequestValidator _validator;


        public CSVImport(RequestValidator validator)
        {
            _validator = validator;
        }

        public FileResult ImportCsv(FileInfo fileInfo, Func<string[], RequestRegist> _mapper)
        {
            _fileResult = new FileResult();
            StreamReader? reader = null;
            try
            {
                reader = new StreamReader(fileInfo.filePath, Encoding.UTF8);
                reader.ReadLine(); // ヘッダーを読み飛ばす
                string? line;
                int lineNo = 1;
                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        lineNo++;
                        if (IsEmptyLine(line, lineNo))
                        {
                            continue;
                        }

                        string[] cols = line.Split(',');
                        if (IsInvalidColLength(line, cols, fileInfo.colLength, lineNo))
                        {
                            continue;
                        }

                        RequestRegist request = _mapper(cols);

                        if (IsInValidItem(request, lineNo))
                        {
                            continue;
                        }

                        var (sql, parameters) = request.BuildInsert(fileInfo.tableName);
                        _db.Execute(sql, parameters);
                        _fileResult.InsertCount++;
                    }
                    catch(Exception ex)
                    {
                        _fileResult.Errors.Add(lineNo + "行目:" + modCommon.Messages.ImportFileError.ExceptionItem + ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                _fileResult.Errors.Add(modCommon.Messages.ImportFileError.CannotOpneFile + ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                }
            }

            return _fileResult;
        }

        private bool IsEmptyLine(string line, int lineNo)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                _fileResult.Errors!.Add(lineNo + "行目:" + modCommon.Messages.ImportFileError.EmptyLine);
                return true;
            }
            return false;
        }

        private bool IsInvalidColLength(string line, string[] cols, int colLength, int lineNo)
        {            
            if (cols.Length < colLength)
            {
                _fileResult.Errors!.Add(lineNo + "行目:" + modCommon.Messages.ImportFileError.ShortageItem);
                return true;
            }
            return false;
        }

        private bool IsInValidItem(RequestRegist request, int lineNo)
        {            
            Validation.ValidationResult  validation = _validator.Validate(request);

            if (!validation.IsValid)
            {
                foreach (var err in validation.Errors)
                {
                    _fileResult.Errors!.Add(lineNo + "行目: " + err.Message);
                }
                return true;
            }
            return false;
        }

    }
}
