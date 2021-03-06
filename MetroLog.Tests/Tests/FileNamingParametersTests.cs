﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MetroLog.Targets;
using MetroLog.Internal;

namespace MetroLog.Tests
{
    public class FileNamingParametersTests
    {
        private LogEventInfo GetLogEventInfo()
        {
            return new LogEventInfo(LogLevel.Info, "foobar", "barfoo", null);
        }

        [Fact]
        public void TestEverythingOff()
        {
            var naming = new FileNamingParameters()
            {
                IncludeLevel = false,
                IncludeLogger = false,
                IncludeSequence = false,
                IncludeSession = false,
                IncludeTimestamp = FileTimestampMode.None
            };

            // ok...
            var info = GetLogEventInfo();

            // check...
            var filename = naming.GetFilename(new LogWriteContext(), info);
            Assert.Equal("Log.log", filename);
        }

        [Fact]
        public void TestLevelOn()
        {
            var naming = new FileNamingParameters()
            {
                IncludeLevel = true,
                IncludeLogger = false,
                IncludeSequence = false,
                IncludeSession = false,
                IncludeTimestamp = FileTimestampMode.None
            };

            // ok...
            var info = GetLogEventInfo();

            // check...
            var filename = naming.GetFilename(new LogWriteContext(), info);
            Assert.Equal("Log - INFO.log", filename);
        }

        [Fact]
        public void TestLoggerOn()
        {
            var naming = new FileNamingParameters()
            {
                IncludeLevel = false,
                IncludeLogger = true,
                IncludeSequence = false,
                IncludeSession = false,
                IncludeTimestamp = FileTimestampMode.None
            };

            // ok...
            var info = GetLogEventInfo();

            // check...
            var filename = naming.GetFilename(new LogWriteContext(), info);
            Assert.Equal("Log - foobar.log", filename);
        }

        [Fact]
        public void TestSequenceOn()
        {
            var naming = new FileNamingParameters()
            {
                IncludeLevel = false,
                IncludeLogger = false,
                IncludeSequence = true,
                IncludeSession = false,
                IncludeTimestamp = FileTimestampMode.None
            };

            // ok...
            var info = GetLogEventInfo();

            // check...
            var filename = naming.GetFilename(new LogWriteContext(), info);
            Assert.Equal(string.Format("Log - {0}.log", info.SequenceID), filename);
        }

        [Fact]
        public void TestTimestampDate()
        {
            var naming = new FileNamingParameters()
            {
                IncludeLevel = false,
                IncludeLogger = false,
                IncludeSequence = false,
                IncludeSession = false,
                IncludeTimestamp = FileTimestampMode.Date
            };

            // ok...
            var info = GetLogEventInfo();

            // check...
            var filename = naming.GetFilename(new LogWriteContext(), info);
            Assert.Equal(string.Format("Log - {0}.log", LogManagerBase.GetDateTime().ToString("yyyyMMdd")), filename);
        }

        [Fact]
        public void TestTimestampTime()
        {
            var naming = new FileNamingParameters()
            {
                IncludeLevel = false,
                IncludeLogger = false,
                IncludeSequence = false,
                IncludeSession = false,
                IncludeTimestamp = FileTimestampMode.Time
            };

            // ok...
            var info = GetLogEventInfo();

            // check...
            var filename = naming.GetFilename(new LogWriteContext(), info);
            Assert.Equal(string.Format("Log - {0}.log", LogManagerBase.GetDateTime().ToString("HHmmss")), filename);
        }

        [Fact]
        public void TestTimestampBoth()
        {
            var naming = new FileNamingParameters()
            {
                IncludeLevel = false,
                IncludeLogger = false,
                IncludeSequence = false,
                IncludeSession = false,
                IncludeTimestamp = FileTimestampMode.DateTime
            };

            // ok...
            var info = GetLogEventInfo();

            // check...
            var filename = naming.GetFilename(new LogWriteContext(), info);
            Assert.Equal(string.Format("Log - {0}.log", LogManagerBase.GetDateTime().ToString("yyyyMMdd HHmmss")), filename);
        }

        [Fact]
        public void TestEverythingOn()
        {
            var naming = new FileNamingParameters()
            {
                IncludeLevel = true,
                IncludeLogger = true,
                IncludeSequence = true,
                IncludeSession = true,
                IncludeTimestamp = FileTimestampMode.DateTime
            };

            // ok...
            var info = GetLogEventInfo();

            // check...
            var context = new LogWriteContext();
            var filename = naming.GetFilename(context, info);
            Assert.Equal(string.Format("Log - INFO - foobar - {0} - {1} - {2}.log", LogManagerBase.GetDateTime().ToString("yyyyMMdd HHmmss"),
                context.Environment.SessionId, info.SequenceID), filename);
        }

        [Fact]
        public void TestRegexEverythingOff()
        {
            var naming = new FileNamingParameters()
            {
                IncludeLevel = false,
                IncludeLogger = false,
                IncludeSequence = false,
                IncludeSession = false,
                IncludeTimestamp = FileTimestampMode.None
            };

            // get...
            var regex = naming.GetRegex();

            // create...
            var info = GetLogEventInfo();

            // check...
            var context = new LogWriteContext();
            var filename = naming.GetFilename(context, info);

            // check...
            Assert.True(regex.Match(filename).Success);
        }

        [Fact]
        public void TestRegexEverythingOn()
        {
            var naming = new FileNamingParameters()
            {
                IncludeLevel = true,
                IncludeLogger = true,
                IncludeSequence = true,
                IncludeSession = true,
                IncludeTimestamp = FileTimestampMode.DateTime
            };

            // get...
            var regex = naming.GetRegex();

            // create...
            var info = GetLogEventInfo();

            // check...
            var context = new LogWriteContext();
            var filename = naming.GetFilename(context, info);
  
            // check...
            Assert.True(regex.Match(filename).Success);
        }
    }
}
