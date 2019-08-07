﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SourceCheckUtil.ExternalConfig;

namespace SourceCheckUtil.Managers
{
    internal class FileProcessingManager
    {
        public FileProcessingManager(ExternalConfigData externalData)
        {
            if (externalData == null)
                throw new ArgumentNullException(nameof(externalData));
            _matches = externalData.FileProcessing.Select(data => new FileMatch(data)).ToList();
        }

        public Boolean SkipFileProcessing(String filePath)
        {
            if (String.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));
            foreach (FileMatch match in _matches)
            {
                if (match.IsMatch(filePath))
                    return match.Mode == FileProcessingMode.Exclude;
            }
            return _matches.Count > 0 && _matches.Last().Mode == FileProcessingMode.Only;
        }

        private readonly IList<FileMatch> _matches;

        private class FileMatch
        {
            public FileMatch(FileProcessingData fileProcessingData)
            {
                _fileMaskRegex = CreateFileMaskRegex(fileProcessingData.Mask);
                Mode = fileProcessingData.Mode;
            }

            public FileProcessingMode Mode { get; }

            public Boolean IsMatch(String filename)
            {
                return _fileMaskRegex.IsMatch(filename);
            }

            private Regex CreateFileMaskRegex(String fileMask)
            {
                StringBuilder builder = new StringBuilder();
                foreach (Char ch in fileMask)
                {
                    // TODO (std_string) : think about another approach
                    if (ch == '*')
                        builder.Append(".*");
                    else if (ch == '?')
                        builder.Append(".*");
                    else if (ch == '/')
                        builder.Append("\\\\");
                    else if ("+()^$.{}[]|\\".IndexOf(ch) != -1)
                        builder.Append("\\").Append(ch);
                    else
                        builder.Append(ch);
                }
                return new Regex(builder.ToString());
            }

            private readonly Regex _fileMaskRegex;
        }
    }
}
