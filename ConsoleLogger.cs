using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPracEFcore
{
    /*
    해야할 일
    1. 로깅 공급자 등록 ConsoleLoggerProvider
    2. 로거 구현 ConsoleLogger
    */
    public class ConsoleLoggerProvider : ILoggerProvider
    {

        public ILogger CreateLogger(string categoryName)
        {
            // categoryName에 따라 로거를 더 구현할 수 있다.
            // 여기서는 ConsoleLogger 하나만 구현한다. 
            throw new NotImplementedException();
        }

        /// <summary>
        /// logger가 관리하지 않는 리소스를 사용하면 여기서 메모리를 해지한다.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class ConsoleLogger : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            throw new NotImplementedException();
        }
    }
}
