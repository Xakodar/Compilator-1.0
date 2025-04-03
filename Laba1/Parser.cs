using System.Collections.Generic;

namespace Laba1
{
    public class ParseError
    {
        public string Message { get; set; }  // Текст ошибки
        public char Symbol { get; set; }  // Какой символ вызвал ошибку (или '\0', если конец строки)
        public int Position { get; set; } // Позиция в строке (0-based или 1-based — на ваш выбор)

        public ParseError(string message, char symbol, int position)
        {
            Message = message;
            Symbol = symbol;
            Position = position;
        }

        public override string ToString()
        {
            return $" [Позиция {Position}] ' {Message} ' ";
        }
    }

    public class ListParser
    {
        private string _input;
        private int _pos; // Текущая позиция в строке
        public List<ParseError> Errors { get; private set; }
        private const char EOF = '\0'; // Для удобства это конец строки

        public ListParser()
        {
            Errors = new List<ParseError>();
        }

        public void Parse(string input)
        {
            _input = input ?? "";
            _pos = 0;
            Errors.Clear();

            // Шаг 1. <START> = <ID> '=' '[' <ELEMS> ']' ';'
            SkipSpaces();
            ParseIdentifier();      // <ID>
            SkipSpaces();

            if (!TryReadChar('='))
            {
                AddError("Ожидался символ '='", CurrentChar());
                AdvancePos();
            }
            SkipSpaces();

            if (!TryReadChar('['))
            {
                AddError("Ожидался символ '['", CurrentChar());
                AdvancePos();
            }
            SkipSpaces();

            ParseElements();        // <ELEMS>
            SkipSpaces();

            if (!TryReadChar(']'))
            {
                AddError("Ожидался символ ']'", CurrentChar());
                AdvancePos();
            }
            SkipSpaces();

            if (!TryReadChar(';'))
            {
                AddError("Ожидался символ ';' в конце", CurrentChar());
                AdvancePos();
            }
            SkipSpaces();

            // Проверка на лишние символы после конца списка
            if (!IsEOF())
            {
                AddError("Лишние символы после конца списка", CurrentChar());
                while (!IsEOF())
                {
                    AdvancePos();
                }
            }
        }

        private void ParseIdentifier()
        {
            if (!IsLetter(CurrentChar()))
            {
                AddError("Идентификатор должен начинаться с английской буквы", CurrentChar());
                AdvancePos();
                return;
            }
            AdvancePos();
            while (IsLetter(CurrentChar()))
            {
                AdvancePos();
            }

            // После идентификатора может быть недопустимый символ
            if (IsInvalidCharacter(CurrentChar()))
            {
                AddError("Недопустимый символ в идентификаторе", CurrentChar());
                AdvancePos();
            }
        }

        private void ParseElements()
        {
            SkipSpaces();

            if (CurrentChar() == ']')
            {
                return;
            }

            ParseElement();

            while (true)
            {
                SkipSpaces();
                if (CurrentChar() == ',')
                {
                    AdvancePos();
                    SkipSpaces();

                    if (CurrentChar() == ']')
                    {
                        break;
                    }
                    ParseElement();
                }
                else
                {
                    break;
                }
            }
        }

        private void ParseElement()
        {
            SkipSpaces();

            char c = CurrentChar();
            if (c == '"')
            {
                ParseString();
            }
            else if (c == '+' || c == '-' || char.IsDigit(c))
            {
                ParseNumber();
            }
            else if (IsInvalidCharacter(c))
            {
                AddError("Недопустимый символ в элементе", c);
                AdvancePos();
            }
            else
            {
                AddError("Ожидался элемент (число или строка)", c);
                AdvancePos();
                SkipUntilDelimiter();
            }
        }

        private void ParseString()
        {
            if (!TryReadChar('"'))
            {
                AddError("Ожидался символ '\"' при начале строки", CurrentChar());
                AdvancePos();
                return;
            }
            while (!IsEOF() && CurrentChar() != '"')
            {
                AdvancePos();
            }
            if (CurrentChar() == '"')
            {
                AdvancePos();
            }
            else
            {
                AddError("Строка не закрыта кавычкой", CurrentChar());
            }
        }

        private void ParseNumber()
        {
            if (CurrentChar() == '+' || CurrentChar() == '-')
            {
                AdvancePos();
            }

            if (!IsDigit(CurrentChar()))
            {
                AddError("После знака числа ожидается цифра", CurrentChar());
                AdvancePos();
                SkipUntilDelimiter();
                return;
            }

            while (IsDigit(CurrentChar()))
            {
                AdvancePos();
            }

            if (CurrentChar() == '.')
            {
                AdvancePos();
                if (!IsDigit(CurrentChar()))
                {
                    AddError("После точки ожидается хотя бы одна цифра", CurrentChar());
                    AdvancePos();
                    SkipUntilDelimiter();
                    return;
                }
                while (IsDigit(CurrentChar()))
                {
                    AdvancePos();
                }
            }
        }

        private bool IsLetter(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        private bool IsDigit(char c)
        {
            return (c >= '0' && c <= '9');
        }

        private bool IsInvalidCharacter(char c)
        {
            // Проверка на недопустимые символы (например, специальные символы)
            return !IsLetter(c) && !IsDigit(c) && c != '+' && c != '-' && c != '.' && c != ',' && c != '"' && c != '[' && c != ']' && c != '=' && c != ';' && !char.IsWhiteSpace(c);
        }

        private void SkipSpaces()
        {
            while (!IsEOF() && char.IsWhiteSpace(CurrentChar()))
            {
                AdvancePos();
            }
        }

        private char CurrentChar()
        {
            if (_pos >= _input.Length)
                return EOF;
            return _input[_pos];
        }

        private bool IsEOF()
        {
            return _pos >= _input.Length;
        }

        private void AdvancePos()
        {
            if (_pos < _input.Length) _pos++;
        }

        private bool TryReadChar(char expected)
        {
            if (CurrentChar() == expected)
            {
                AdvancePos();
                return true;
            }
            return false;
        }

        private void AddError(string message, char symbol)
        {
            Errors.Add(new ParseError(message, symbol, _pos));
        }

        private void SkipUntilDelimiter()
        {
            while (!IsEOF())
            {
                char c = CurrentChar();
                if (c == ',' || c == ']' || c == ';')
                {
                    break;
                }
                AdvancePos();
            }
        }
    }


}
