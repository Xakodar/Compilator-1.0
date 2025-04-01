using System.Collections.Generic;
using System.Text;

namespace Laba1
{
    public enum TokenCode
    {
        Integer = 1,        // целое
        Float = 2,          // вещественное
        Identifier = 3,     // идентификатор
        AssignOp = 4,       // "="
        Minus = 5,          // (если бы мы хотели отдельный токен)
        Comma = 6,          // ","
        LBracket = 7,       // "["
        RBracket = 8,       // "]"
        Semicolon = 9,      // ";"
        StringLiteral = 10, // "..."
        Keyword = 11,       // например "List"
        Plus = 13,
        String = 14,
        Error = 99
    }

    public class Token
    {
        public TokenCode Code { get; set; }
        public string Type { get; set; }
        public string Lexeme { get; set; }
        public int StartPos { get; set; }
        public int EndPos { get; set; }
        public int Line { get; set; }

        public override string ToString()
        {
            return $"[{Line}:{StartPos}-{EndPos}] ({Code}) {Type} : '{Lexeme}'";
        }
    }

    public class Scanner
    {
        private string _text;
        private int _pos;       // текущая позиция (сквозная по всему тексту)
        private int _line;      // текущая строка
        private int _linePos;   // позиция в текущей строке
        private List<Token> _tokens;

        // Пример набора ключевых слов
        private HashSet<string> _keywords = new HashSet<string> { "List" };

        public Scanner()
        {
            _tokens = new List<Token>();
        }

        public List<Token> Scan(string text)
        {
            _text = text;
            _pos = 0;
            _line = 1;
            _linePos = 1;
            _tokens.Clear();

            while (!IsEnd())
            {
                char ch = CurrentChar();

                // Используем switch
                switch (ch)
                {
                    // Пропускаем незначащие пробелы, табуляцию и переводы строк
                    case var c when char.IsWhiteSpace(c):
                        Advance();
                        break;

                    // Буква - значит начинаем считывать идентификатор (или ключевое слово)
                    case var c when char.IsLetter(c):
                        ReadIdentifierOrKeyword();
                        break;

                    // Минус: может быть частью числа (например, -5 или -2.3)
                    case '-':
                        AddToken(TokenCode.Minus, "знак минус", "-");
                        Advance();
                        break;

                    case '+':
                        AddToken(TokenCode.Plus, "знак плюс", "+");
                        Advance();
                        break;

                    // Цифра - читаем число (целое или вещественное)
                    case var c when char.IsDigit(c):
                        ReadNumber();
                        break;

                    // Оператор присваивания
                    case '=':
                        AddToken(TokenCode.AssignOp, "оператор присваивания", "=");
                        Advance();
                        break;

                    // Открывающая скобка
                    case '[':
                        AddToken(TokenCode.LBracket, "открывающая скобка", "[");
                        Advance();
                        break;

                    // Закрывающая скобка
                    case ']':
                        AddToken(TokenCode.RBracket, "закрывающая скобка", "]");
                        Advance();
                        break;

                    // Запятая
                    case ',':
                        AddToken(TokenCode.Comma, "запятая", ",");
                        Advance();
                        break;

                    // Точка с запятой
                    case ';':
                        AddToken(TokenCode.Semicolon, "конец оператора", ";");
                        Advance();
                        break;

                    // Строка (начинается на ")
                    case '"':
                        AddToken(TokenCode.String, "кавычка", " '' ");
                        Advance();
                        break;

                    // По умолчанию - недопустимый символ
                    default:
                        AddToken(TokenCode.Error, "недопустимый символ", ch.ToString());
                        Advance();
                        break;
                }
            }

            return _tokens;
        }

        /// <summary>
        /// Считывание идентификатора или ключевого слова
        /// </summary>
        private void ReadIdentifierOrKeyword()
        {
            int startPos = _linePos;
            StringBuilder sb = new StringBuilder();
            char c = CurrentChar();

            // Первый символ идентификатора должен быть английской буквой
            if (!((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')))
            {
                AddToken(TokenCode.Error, "недопустимый символ", c.ToString(), _linePos, _linePos, _line);
                Advance();
                return;
            }
            sb.Append(c);
            Advance();

            while (!IsEnd())
            {
                c = CurrentChar();
                if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || char.IsDigit(c) || c == '_')
                {
                    sb.Append(c);
                    Advance();
                }
                else if (char.IsLetter(c))
                {
                    // Здесь обработка всех не английских букв
                    AddToken(TokenCode.Error, "недопустимый символ", c.ToString(), _linePos, _linePos, _line);
                    Advance();
                }
                else
                {
                    break;
                }
            }

            string lexeme = sb.ToString();
            if (_keywords.Contains(lexeme))
                AddToken(TokenCode.Keyword, "ключевое слово", lexeme, startPos, _linePos - 1, _line);
            else
                AddToken(TokenCode.Identifier, "идентификатор", lexeme, startPos, _linePos - 1, _line);
        }


        /// <summary>
        /// Считывание числа (целое или вещественное, может быть с минусом)
        /// </summary>
        private void ReadNumber()
        {
            int startPos = _linePos;
            bool hasDot = false;
            bool hasDigits = false;

            StringBuilder sb = new StringBuilder();

            // Может начинаться с минуса
            if (CurrentChar() == '-')
            {
                sb.Append(CurrentChar());
                Advance();
            }

            // Основной цикл
            while (!IsEnd())
            {
                char ch = CurrentChar();
                if (char.IsDigit(ch))
                {
                    hasDigits = true;
                    sb.Append(ch);
                    Advance();
                }
                else if (ch == '.')
                {
                    if (hasDot)
                    {
                        // Повторная точка - прерываем, либо помечаем ошибку
                        break;
                    }
                    else
                    {
                        hasDot = true;
                        sb.Append(ch);
                        Advance();
                    }
                }
                else
                {
                    // Не цифра и не точка - конец числа
                    break;
                }
            }

            string numberLexeme = sb.ToString();
            if (!hasDigits)
            {
                // Если не встретили ни одной цифры (например, "-.")
                AddToken(TokenCode.Error, "ошибка в числе", numberLexeme, startPos, _linePos - 1, _line);
                return;
            }

            if (hasDot)
            {
                AddToken(TokenCode.Float, "вещественное число", numberLexeme, startPos, _linePos - 1, _line);
            }
            else
            {
                AddToken(TokenCode.Integer, "целое число", numberLexeme, startPos, _linePos - 1, _line);
            }
        }

        /// <summary>
        /// Считывание строкового литерала (открывающая кавычка уже считана)
        /// </summary>
        private void ReadStringLiteral()
        {
            int startPos = _linePos;  // позиция открывающей кавычки
            StringBuilder sb = new StringBuilder();

            // Пропускаем открывающую кавычку
            Advance(); // уходим за '"'
            bool closed = false;

            while (!IsEnd())
            {
                char ch = CurrentChar();
                if (ch == '"')
                {
                    // нашли закрывающую кавычку
                    closed = true;
                    Advance(); // пропускаем закрывающую кавычку
                    break;
                }
                else
                {
                    // добавляем символ в строку (включая пробелы)
                    sb.Append(ch);
                    Advance();
                }
            }

            string strValue = sb.ToString();
            if (!closed)
            {
                // Строка не закрылась
                AddToken(TokenCode.Error, "незакрытая строка", strValue, startPos, _linePos - 1, _line);
            }
            else
            {
                // Закрытая строка
                AddToken(TokenCode.StringLiteral, "строка", strValue, startPos, _linePos - 1, _line);
            }
        }

        #region Вспомогательные методы

        private bool IsEnd()
        {
            return _pos >= _text.Length;
        }

        private char CurrentChar()
        {
            if (IsEnd()) return '\0';
            return _text[_pos];
        }

        private void Advance()
        {
            // Если встретили перевод строки, переходим на следующую строку
            if (CurrentChar() == '\n')
            {
                _line++;
                _linePos = 0;
            }
            _pos++;
            _linePos++;
        }

        private void AddToken(TokenCode code, string type, string lexeme)
        {
            AddToken(code, type, lexeme, _linePos, _linePos, _line);
        }

        private void AddToken(TokenCode code, string type, string lexeme, int startPos, int endPos, int line)
        {
            var token = new Token
            {
                Code = code,
                Type = type,
                Lexeme = lexeme,
                StartPos = startPos,
                EndPos = endPos,
                Line = line
            };
            _tokens.Add(token);
        }

        #endregion
    }
}
